using System.Data.SqlClient;
using System.Xml;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using PatchingToolUI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.Configuration;
using System.IO;
using System.Security.Principal;
using System.Collections.ObjectModel;
using System.Text;
using System.Security.Cryptography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using System.ComponentModel;
using System.ServiceProcess;


namespace PatchingUI
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        #region Variables
        private WindowsIdentity wiCurrentUser = WindowsIdentity.GetCurrent();        
        string timerrefreshinterval = string.Empty;
        string Footerversion = string.Empty;
        string strServerlist = string.Empty;       
        string path = string.Empty;
        public string exereport = string.Empty;
        string filename = string.Empty;
        StreamWriter sw = null;
        string txtOnlyQFE = string.Empty;
        string txtExcludeQFE = string.Empty;
        string rblBPU = string.Empty;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
           // if (!IsPostBack)
            {
                if (Request.Url.ToString().Contains("Plan"))
                    MenuTab1.Attributes.Add("class", "selected");
                else if (Request.Url.ToString().Contains("Prep"))
                    MenuTab2.Attributes.Add("class", "selected");
                else if (Request.Url.ToString().Contains("Execute"))
                    MenuTab3.Attributes.Add("class", "selected");
                else if (Request.Url.ToString().Contains("Validate"))
                    MenuTab4.Attributes.Add("class", "selected");
                else if (Request.Url.ToString().Contains("Reports"))
                    MenuTab5.Attributes.Add("class", "selected");

                Footerversion = ConfigurationManager.AppSettings["Footerversion"];
                VersionNumber.Text = Footerversion;
               // timerrefreshinterval = ConfigurationManager.AppSettings["TimerRefreshInterval"];
               // timerSrvcChk.Interval = Convert.ToInt32(timerrefreshinterval);
            }

            string currentDomainUser = wiCurrentUser.Name;
            currentDomainUser = HttpContext.Current.Request.LogonUserIdentity.Name;
            try
            {
                string[] splitDomainAndUser = currentDomainUser.Split('\\');
                if (splitDomainAndUser.Length == 2)
                {
                    ReadOnlyCollection<GCUserInfo> currentUser = new ReadOnlyCollection<GCUserInfo>(Helper.GetUserInfo(splitDomainAndUser[1]));
                    CurrentUserFullName.Text = currentUser[0].FirstName + " " + currentUser[0].LastName;
                }
                else
                {
                    CurrentUserFullName.Text = HttpContext.Current.Request.LogonUserIdentity.Name;
                }
            }
            catch (Exception)
            {
                CurrentUserFullName.Text = HttpContext.Current.Request.LogonUserIdentity.Name;

            }
        }

        #endregion

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
}
