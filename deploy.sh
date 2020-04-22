set -x
git pull
systemctl stop cslabs-backend
cd CSLabs.Api
dotnet ef database update
dotnet publish --configuration Release
systemctl start cslabs-backend