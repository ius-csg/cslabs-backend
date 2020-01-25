using System;
using System.Collections;
using CSLabs.Api.Config;
using CSLabs.Api.Models.UserModels;
using Novell.Directory.Ldap;

namespace CSLabs.Api.Services
{
    public class LdapService
    {
        private readonly string _ldapHost;
        private readonly int _ldapPort;
        private readonly string _adminDn;
        private readonly string _adminPass;

        public LdapService(LdapSettings settings)
        {
            _ldapHost = settings.Server;
            _ldapPort = settings.Port;
            _adminDn = settings.AdminDn;
            _adminPass = settings.AdminPassword;
        }
        public void CreateEntry(User user, string pass)
        {
            LdapConnection ldapConn = new LdapConnection();
        
            ldapConn.Connect(_ldapHost, _ldapPort);
        
            ldapConn.Bind(_adminDn, _adminPass);
        
            LdapAttributeSet userAttributeSet = new LdapAttributeSet();
            userAttributeSet.Add( new LdapAttribute("objectclass", user.UserType));
            userAttributeSet.Add( new LdapAttribute("cn", user.GetEmail()));
            userAttributeSet.Add( new LdapAttribute("givenname", user.FirstName));
            userAttributeSet.Add( new LdapAttribute("sn", user.LastName));
            userAttributeSet.Add( new LdapAttribute("stdntemail", user.SchoolEmail));
            userAttributeSet.Add( new LdapAttribute("prsnlemail", user.PersonalEmail));
            userAttributeSet.Add(new LdapAttribute("pass", pass));

            string dn = "cn=TestUser,ou=users,dc=csg,dc=ius,dc=edu";
            LdapEntry newEntry = new LdapEntry(dn, userAttributeSet);
            ldapConn.Add(newEntry);
        }
        
        public void Search()
        {
            LdapConnection ldapConn = new LdapConnection();
        
            ldapConn.Connect(_ldapHost, _ldapPort);
            LdapSearchResults lsc = ldapConn.Search("uid=jgallavi,cn=users,cn=accounts,dc=csg,dc=ius,dc=edu", LdapConnection.SCOPE_SUB, "objectClass=*", null, false);

            while (lsc.HasMore())
            {
                LdapEntry nextEntry = null;
                try
                {
                    nextEntry = lsc.Next();
                }
                catch (LdapException e)
                {
                    Console.WriteLine("Error: " + e.LdapErrorMessage);
                    //Exception is thrown, go for next entry
                    continue;
                }

                Console.WriteLine("\n" + nextEntry.DN);
            }

        }

        public void ModifyAttribute(User user, string attrName, string attrString, string pass)
        {
            string userDN = "cn=TestUser,ou=users,dc=csg,dc=ius,dc=edu";
            
            LdapConnection ldapConn = new LdapConnection();
        
            ldapConn.Connect(_ldapHost, _ldapPort);

            try
            {
                ldapConn.Bind(userDN, pass);
            }
            catch
            {
                throw new AuthenticationFailureException();
            }
            

            ArrayList modList = new ArrayList();
            
            //modify the attribute based on the attribute name that was passed in
            LdapAttribute attribute = new LdapAttribute(attrName, attrString);
            modList.Add(new LdapModification(LdapModification.ADD, attribute));

            LdapModification[] mods = new LdapModification[modList.Count];
            Type mtype = Type.GetType("Novell.Directory.LdapModification");
            mods = (LdapModification[]) modList.ToArray(typeof(LdapModification));

            ldapConn.Modify( userDN, mods);
        }

        public void LdapConnect(User user, string pass)
        {
            string userDN = "cn=TestUser,ou=users,dc=csg,dc=ius,dc=edu";
            
            LdapConnection ldapConn = new LdapConnection();

            ldapConn.Connect(_ldapHost, _ldapPort);

            try
            {
                ldapConn.Bind(userDN, pass);
            }
            catch
            {
                
            }
            
        }
    }
}