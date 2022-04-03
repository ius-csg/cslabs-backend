# Deploying The Application

This document covers information relating to the current application setup in the dev and production
environments, and how to deploy new versions of CSLabs.

## Webserver

The application uses a webserver to direct web requests to the correct place. Depending on the requested 
resource, the webserver will either respond with a static web resource in the case of
the front-end application, or will direct the request to the back-end application service running on the
server. The back-end service must be running and responding to requests in order for the front-end to 
work properly.

Currently, the application uses Nginx as its webserver. The application is hosted on internal virtual
machines managed via Proxmox on the internal cluster. There is one virtual machine for the dev environment
and another for production.

## Nginx

Nginx must be installed on the server and configured to direct requests to the appropriate location. The
configuration file that configured Nginx to do this correctly is committed to the back-end repository, in
the root directory. `nginx.conf.dev` is the configuration for the dev environment, and `nginx.conf` is the
configuration for the production environment.

The official Nginx documentation can be found [here](http://nginx.org/en/docs/).

## Deploying the front-end

The front-end consists of static web sources - HTML, CSS, and JS files with some image and other assets.
Nginx simply needs to be told where these static files are on the server and which URLs to direct
to these resources. Currently, these are configured to live at the `/var/www/cslabs-webapp/build/`
directory. This is the default directory where the React build scripts leave the production-ready build
files.

To deploy a new version of the webapp, clone the branch or tag that you wish to deploy into
`/var/www/cslabs-webapp/build/` and run the React build scripts, which are documented in the front-end
repository.

Alternatively, if the front-end repo is already cloned to the server, the `deploy.sh` script
located in the root directory of the front-end repo will automatically pull the latest changes
from the branch that the local repo is on at script runtime. It will then build the application and
place the result in the `/build/` directory in the repo. If the local repo on the server is already
in `/var/www/cslabs-webapp/` then you only need to run this script to deploy a new version.

### Note: 403 Forbidden

If you are having issues with the server responding with `403 Forbidden` to requests for front-end
resources, check the following:

- Check that the nginx user has read, write, and execute permissions along the entire filepath to
`var/www/cslabs-webapp/build/` and all files in the directory. (Currently, the Nginx user is simply
`nginx`, this can be configured from the Nginx config file)
- Check SELinux - It must be set to permissive, otherwise Nginx will be denied permission to serve
these files and you will see `403 Forbidden` responses.

## Deploying the back-end

The back-end is a .NET application running as a Linux Kestrel service. The status of the service can be
viewed using `systemctl status cslabs-backend`. The service configuration file currently lives at
`/etc/systemd/system/cslabs-backend.service`. The current version is committed to the root directory
of the back-end repository. This service is configured to run from compiled .NET binaries located at
`/var/www/cslabs-backend/CSLabs.Api/bin/Release/net6.0/publish` which is the default build directory
for .NET 6.

The back-end must have a connection to the database to work correctly. The build script will signal a
database error if the application cannot make a connection. DB connection info can be found in the project
Trello board in the deployment card. The `CSLabs.Api/appsettings.json` file must have the correct
mail credentials - these can be found in the project Trello board.

To deploy a new version, run the `deploy.sh` script in the root directory of the back-end repo. Similar
to the front-end script, this will pull down the latest changes of the current branch, and then build
a new version to the default directory, noted above. It will then restart the Kestrel service to apply
the changes.

### Note: Emails

CSLabs uses mail services to send email to users. If this isn't working, it can be (mostly) bypassed
by commenting out the email lines in the login method of UserController.cs. This should not be done on
production, but can be used as a temporary solution for testing in dev environment if necessary.

## Github Actions

Automated deployment actions are undertaken on our servers by a self-hosted Github Actions runner. You can
see the documentation for self-hosted runners [here](https://docs.github.com/en/actions/hosting-your-own-runners/adding-self-hosted-runners).
On each of our servers, the actions runner runs under the `github` username, with the same password as root.
There is one runner for dev, named `cslabs-dev` and another for prod named `cslabs-prod`. The work folder for
the actions runner is `_actions_work`. The actions runner is configured to run as a service so that it will 
automatically run on system startup.

Currently, the actions runner only handles deployment of new versions of the software. On a fresh machine,
the first deployment will have to be done manually. After that, subsequent deployments will be automatically
handled by the actions runner. These deployments are set up to trigger automatically when a pull request is
merged to either the dev or main branch. The workflow will automatically determine which
environment to deploy to based on which branch triggered the job.