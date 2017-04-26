using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using System.Security.Permissions;
using System.Collections.ObjectModel;

using System.DirectoryServices;
using System.Diagnostics;
using System.Xml;
using System.IO;

namespace PatchingUI
{
    public class Helper
    {/// <summary>
        /// Initializes static members of the LDAPHelper class.
        /// </summary>
        static Helper()
        {
            RootPath = @"GC://corp.microsoft.com";
        }

        /// <summary>
        /// Gets or sets the Path to the Active Directory server [Optional].
        /// </summary>
        /// <value>The Path to the Active Directory server.</value>
        /// <remarks>The value "LDAP:\\" will be used by default.</remarks>
        public static string RootPath { get; set; }

        /// <summary>
        /// Gets or sets the UserName to be used for authentication [Optional].
        /// </summary>
        /// <value>The UserName to be used for authentication [Optional].</value>
        /// <remarks>
        /// When the code is running on a remote machine, set this value to 
        /// impersonate a user of the domain that you're querying.
        /// </remarks>
        public static string UserName { get; set; }

        /// <summary>
        /// Gets or sets the PassWord to be used for authentication [Optional].
        /// </summary>
        /// <value>The PassWord to be used for authentication [Optional].</value>
        /// <remarks>
        /// When the code is running on a remote machine, set this value to 
        /// impersonate a user of the domain that you're querying.
        /// </remarks>
        public static string Password { get; set; }

        /// <summary>
        /// Checks if a full object path is valid.
        /// </summary>
        /// <param name="fullObjectPath">The full object path.</param>
        /// <returns>True if the path is valid.</returns>
        /// <remarks>Impersonation is not possible here.</remarks>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.NoFlags)]
        public static bool Exists(string fullObjectPath)
        {
            bool found = false;

            if (DirectoryEntry.Exists(fullObjectPath))
            {
                found = true;
            }

            return found;
        }

        /// <summary>
        /// Verifies if a User exists.
        /// </summary>
        /// <param name="userName">The UserName to verify.</param>
        /// <returns>True if the UserName exists in Active Directory, false if not.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.NoFlags)]
        public static bool UserExists(string userName)
        {
            using (DirectorySearcher searcher = GetDirectorySearcher())
            {
                searcher.Filter = "(&(ObjectClass=user)(sAMAccountName=" + userName.Substring(userName.IndexOf('\\') + 1) + "))";

                // for performance reasons only request needed properties
                searcher.PropertiesToLoad.AddRange(new string[] { "sAMAccountName" });

                SearchResult result = searcher.FindOne();

                return result != null;
            }
        }

        /// <summary>
        /// Verifies if a Group exists.
        /// </summary>
        /// <param name="groupName">The GroupName to verify.</param>
        /// <returns>True if the GroupName exists in Active Directory, false if not.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.NoFlags)]
        public static bool GroupExists(string groupName)
        {
            using (DirectorySearcher searcher = GetDirectorySearcher())
            {

                searcher.Filter = "(&(objectClass=Group)(sAMAccountName=" + groupName + "))";

                // for performance reasons only request needed properties
                searcher.PropertiesToLoad.AddRange(new string[] { "sAMAccountName" });

                SearchResult result = searcher.FindOne();

                return result != null;
            }
        }

        /// <summary>
        /// Returns all stored UserIds.
        /// </summary>
        /// <returns>A list of all UserIds.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.NoFlags)]
        public static ReadOnlyCollection<string> GetAllUserNames()
        {
            DirectoryEntry de = GetDirectoryEntry();
            List<string> result = new List<string>();

            using (DirectorySearcher srch = new DirectorySearcher(de, "(objectClass=user)"))
            {
                SearchResultCollection results = srch.FindAll();

                foreach (SearchResult item in results)
                {
                    result.Add(item.Properties["sAMAccountName"][0].ToString());
                }
            }
            ReadOnlyCollection<string> read = new ReadOnlyCollection<string>(result);
            return read;
        }

        /// <summary>
        /// Returns all stored GroupNames.
        /// </summary>
        /// <returns>A list of all GroupNames.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.NoFlags)]
        public static ReadOnlyCollection<string> GetAllGroupNames()
        {
            DirectoryEntry de = GetDirectoryEntry();
            List<string> result = new List<string>();
            using (DirectorySearcher srch = new DirectorySearcher(de, "(objectClass=Group)"))
            {
                SearchResultCollection results = srch.FindAll();

                foreach (SearchResult item in results)
                {
                    result.Add(item.Properties["sAMAccountName"][0].ToString());
                }
            }
            ReadOnlyCollection<string> read = new ReadOnlyCollection<string>(result);
            return read;
        }

        /// <summary>
        /// Returns all UserIds in a Group.
        /// </summary>
        /// <param name="groupName">The Group.</param>
        /// <returns>A list of all UserIds in the Group.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.NoFlags)]
        public static ReadOnlyCollection<string> GetAllGroupMembers(string groupName)
        {
            List<string> result = new List<string>();
            DirectorySearcher searcher = GetDirectorySearcher();
            searcher.Filter = "(CN=" + groupName + ")";
            SearchResultCollection groups = searcher.FindAll();
            foreach (SearchResult group in groups)
            {
                ResultPropertyCollection props = group.Properties;
                foreach (object member in props["member"])
                {
                    DirectoryEntry memberEntry = GetDirectoryEntry();
                    memberEntry.Path = RootPath + @"/" + member;
                    PropertyCollection userProps = memberEntry.Properties;
                    object userName = userProps["sAMAccountName"].Value;
                    if (null != userName)
                    {
                        result.Add(userName.ToString());
                    }
                }
            }
            ReadOnlyCollection<string> read = new ReadOnlyCollection<string>(result);
            return read;
        }

        /// <summary>
        /// Validates a UserId / Password combination.
        /// </summary>
        /// <param name="userName">The userid.</param>
        /// <param name="password">The password.</param>
        /// <returns>True if the user can be authenticated, False if not.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.NoFlags)]
        public static bool IsValidUser(string userName, string password)
        {
            bool authenticated = false;
            try
            {
                DirectoryEntry entry = new DirectoryEntry(RootPath, userName, password);

                object nativeObject = entry.NativeObject;
                authenticated = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return authenticated;
        }

        /// <summary>
        /// Returns a properly configured DirectoryEntry.
        /// </summary>
        /// <returns>A properly configured DirectoryEntry.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.NoFlags)]
        private static DirectoryEntry GetDirectoryEntry()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                // No Impersonation
                return new DirectoryEntry(RootPath);
            }
            else
            {
                // Impersonation
                return new DirectoryEntry(RootPath, UserName, Password);
            }
        }
        /// <summary>
        /// Returns a properly configured DirectorySearcher.
        /// </summary>
        /// <returns>A properly configured DirectorySearcher.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.NoFlags)]
        private static DirectorySearcher GetDirectorySearcher()
        {
            return new DirectorySearcher(GetDirectoryEntry());
        }

        /// <summary>
        /// Get mail addresss.
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.NoFlags)]
        public static ReadOnlyCollection<GCUserInfo> GetUserInfo(string userAlias)
        {
            List<GCUserInfo> gcUserInfo = new List<GCUserInfo>();
            GCUserInfo userinfo = new GCUserInfo();
            using (DirectorySearcher searcher = GetDirectorySearcher())
            {
                string[] requiredUserProperties = new string[] { "mail", "displayname", "givenName", "sn" };
                searcher.PropertiesToLoad.AddRange(requiredUserProperties);

                searcher.Filter = "(&(ObjectClass=User)(sAMAccountName=" + userAlias.Substring(userAlias.IndexOf('\\') + 1) + "))";
                SearchResult result = searcher.FindOne();
                if (result == null)
                {
                    searcher.Filter = "(&(ObjectClass=Group)(sAMAccountName=" + userAlias.Substring(userAlias.IndexOf('\\') + 1) + "))";
                    result = searcher.FindOne();
                }
                if (result != null)
                {
                    foreach (String userProperty in requiredUserProperties)
                    {
                        foreach (Object propertyValue in result.Properties[userProperty])
                        {
                            userinfo.MailAddress = (userProperty == "mail") ? propertyValue.ToString() : userinfo.MailAddress;
                            userinfo.DisplayName = (userProperty == "displayname") ? propertyValue.ToString() : userinfo.DisplayName;
                            userinfo.FirstName = (userProperty == "givenName") ? propertyValue.ToString() : userinfo.FirstName;
                            userinfo.LastName = (userProperty == "sn") ? propertyValue.ToString() : userinfo.LastName;

                        }
                    }
                    gcUserInfo.Add(userinfo);
                }
            }
            ReadOnlyCollection<GCUserInfo> gcread = new ReadOnlyCollection<GCUserInfo>(gcUserInfo);
            return gcread;
        }
        #region writeerror
        /// <summary>
        /// method to get current page name
        /// </summary>
        /// <author>Sudha Gubbala</author>
        /// <CreatedDate>6/18/2012</CreatedDate>
        /// <returns></returns>
        public static string GetCurrentPageName()
        {
            var sPath = HttpContext.Current.Request.Url.AbsolutePath;
            var strarry = sPath.Split('/');
            var lengh = strarry.Length;
            var sRet = strarry[lengh - 1];
            return sRet;
        }
        /// <summary>
        /// Method to log error message to xml file
        /// </summary>
        /// <author>Sudha Gubbala</author>
        /// <CreatedDate>6/18/2012</CreatedDate>
        /// <param name="ex"></param>
        public static void WriteError(Exception ex)
        {
            try
            {
                var doc = new XmlDocument();
                var xmlPath = HttpContext.Current.Server.MapPath("Errorlog.xml");
                doc.Load(@xmlPath);
                var oldXmlNode = doc.ChildNodes[1].ChildNodes[0];
                var newXmlNode = oldXmlNode.CloneNode(true);
                var stackTrace = new StackTrace();
                var stackFrame = stackTrace.GetFrame(1);
                var methodBase = stackFrame.GetMethod();
                newXmlNode.ChildNodes[0].InnerText = DateTime.Now.ToString();
                newXmlNode.ChildNodes[1].InnerText = GetCurrentPageName();
                newXmlNode.ChildNodes[2].InnerText = methodBase.DeclaringType.FullName;
                newXmlNode.ChildNodes[3].InnerText = methodBase.Name;
                newXmlNode.ChildNodes[4].InnerText = ex.TargetSite.Name;
                newXmlNode.ChildNodes[5].InnerText = ex.Message;
                newXmlNode.ChildNodes[6].InnerText = ex.StackTrace;
                newXmlNode.ChildNodes[7].InnerText = HttpContext.Current.Request.UserHostAddress;
                newXmlNode.ChildNodes[8].InnerText = HttpContext.Current.Request.Url.OriginalString;
                doc.ChildNodes[1].AppendChild(newXmlNode);
                if ((File.GetAttributes(@xmlPath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    File.SetAttributes(@xmlPath, FileAttributes.Normal);
                doc.Save(@xmlPath);
                doc.RemoveAll();
            }
            catch
            {

            }
        }
        #endregion
    }
    public partial class GCUserInfo
    {
        public string SamAccountName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CNName { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string TitleName { get; set; }
        public string MailNickname { get; set; }
        public string MailAddress { get; set; }
        public string UserPrincipalName { get; set; }
        public string TelephoneNumber { get; set; }
        public string PhysicalDeliveryOfficeName { get; set; }
        public string ManagerName { get; set; }
        public string Homepage { get; set; }
        public string DistinguishedName { get; set; }
        public string ObjectCategory { get; set; }
        public string MSExchUserCulture { get; set; }
        public string WhenCreated { get; set; }


    }


}

