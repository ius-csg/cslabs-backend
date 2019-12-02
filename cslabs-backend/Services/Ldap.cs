using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabsBackend.Models;
using CSLabsBackend.Models.Enums;
using CSLabsBackend.Models.UserModels;
using static CSLabsBackend.Models.Enums.UserType;
using CSLabsBackend.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Novell.Directory.Ldap;

namespace CSLabsBackend.Services
{
    public class Ldap
    {
        private static string ldapHost = "<ldap host>";
        private static int ldapPort = 389;
        private static string serverDN = "";
        private static string serverPass = "<server pass>";
        public static void CreateEntry(User user, string pass)
        {
            LdapConnection ldapConn = new LdapConnection();
        
            ldapConn.Connect(ldapHost, ldapPort);
        
            ldapConn.Bind(serverDN, serverPass);
        
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

        public static void ModifyPassword(User user, string attrName, string attrString, string pass)
        {
            string userDN = "cn=TestUser,ou=users,dc=csg,dc=ius,dc=edu";
            
            LdapConnection ldapConn = new LdapConnection();
        
            ldapConn.Connect(ldapHost, ldapPort);

            try
            {
                ldapConn.Bind(userDN, pass);
            }
            catch
            {
                
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

        public static void LdapConnect(User user, string pass)
        {
            string userDN = "cn=TestUser,ou=users,dc=csg,dc=ius,dc=edu";
            
            LdapConnection ldapConn = new LdapConnection();

            ldapConn.Connect(ldapHost, ldapPort);

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