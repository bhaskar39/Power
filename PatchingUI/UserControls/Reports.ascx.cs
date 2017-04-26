using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using PatchingToolUI;
using System.Security.Principal;
using System.Security.Cryptography;
using System.IO;
using System.Xml;
using System.Diagnostics;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;

namespace PatchingUI.UserControls
{
    public partial class Reports : System.Web.UI.UserControl
    {
        #region Variables
        string path = string.Empty;
        public string exereport = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region ReportsTab


        protected void btnPlanSummaryReports_Click(object sender, EventArgs e)
        {
            try
            {
                // rvDeployment.Visible = false;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_5','content_5')</script>");

                string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
                string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                rvMainReports.ServerReport.ReportServerUrl = new Uri(@url);
                path = ConfigurationManager.AppSettings["PlanSummaryReportPath"].ToString();
                rvMainReports.ServerReport.ReportPath = ReportViewerPath + path;
                exereport = rvMainReports.ServerReport.ReportPath;
                rvMainReports.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                rvMainReports.ShowParameterPrompts = true;
                rvMainReports.ShowPrintButton = true;
                rvMainReports.ServerReport.Timeout = 30000;


                rvMainReports.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("StartDate", txtReportsStartDate.Text.Replace("-", "/")));

                rvMainReports.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("EndDate", txtReportsEndDate.Text.Replace("-", "/")));


                rvMainReports.ServerReport.Refresh();

            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }

        protected void btnOverallReports_Click(object sender, EventArgs e)
        {
            try
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_5','content_5')</script>");
                // rvDeployment.Visible = true;
                // rvMainReports.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;

                //trODPOption.Attributes.Add("style", "height:50px");
                //rvMainReports.Attributes.Add("style", "height:50px");
                string url = ConfigurationManager.AppSettings["OverAllReportServerURL"].ToString();
                string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                rvMainReports.ServerReport.ReportServerUrl = new Uri(@url);
                path = ConfigurationManager.AppSettings["OverallSummaryReportPath"].ToString();
                rvMainReports.ServerReport.ReportPath = ReportViewerPath + path;
                exereport = rvMainReports.ServerReport.ReportPath;
                rvMainReports.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                rvMainReports.ShowParameterPrompts = true;
                rvMainReports.ShowPrintButton = true;
                rvMainReports.ServerReport.Timeout = 30000;

                //  rvMainReports.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("StartDate", txtReportsStartDate.Text.Replace("-", "/")));
                // rvMainReports.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("EndDate", txtReportsEndDate.Text.Replace("-", "/")));
                //  rvDeployment.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;

                rvMainReports.ServerReport.Refresh();
                //rvMainReports.Height =300;
                //rvDeployment.ServerReport.ReportServerUrl = new Uri(@url);
                //string Deppath = string.Empty;
                //Deppath = ConfigurationManager.AppSettings["PatchDeploymentStatus"].ToString();
                //rvDeployment.ServerReport.ReportPath = ReportViewerPath + Deppath;
                //exereport = rvDeployment.ServerReport.ReportPath;
                //rvDeployment.ShowParameterPrompts = false;
                //rvDeployment.ShowPrintButton = true;
                //rvDeployment.ServerReport.Timeout = 30000;
                //rvDeployment.ServerReport.Refresh();
                //System.Drawing.Printing.PageSettings pg = new System.Drawing.Printing.PageSettings();
                //pg.Margins.Top = 1000;
                //pg.Margins.Bottom = 1000;
                //pg.Margins.Left = 1000;
                //pg.Margins.Right = 1000;
                //System.Drawing.Printing.PaperSize size = new System.Drawing.Printing.PaperSize();
                //size.RawKind = (int)System.Drawing.Printing.PaperKind.A5;
                //pg.PaperSize = size;
                //rvMainReports.SetPageSettings(pg);
                rvMainReports.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }

        protected void btnExecuteReports_Click(object sender, EventArgs e)
        {
            try
            {
                //  rvDeployment.Visible = false;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_5','content_5')</script>");
                rvMainReports.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
                string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                rvMainReports.ServerReport.ReportServerUrl = new Uri(@url);
                path = ConfigurationManager.AppSettings["ExceuteSummaryReportPath"].ToString();
                rvMainReports.ServerReport.ReportPath = ReportViewerPath + path;
                exereport = rvMainReports.ServerReport.ReportPath;
                rvMainReports.ShowParameterPrompts = true;
                rvMainReports.ShowPrintButton = true;
                rvMainReports.ServerReport.Timeout = 30000;
                rvMainReports.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("StartDate", txtReportsStartDate.Text.Replace("-", "/")));

                rvMainReports.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("EndDate", txtReportsEndDate.Text.Replace("-", "/")));


                rvMainReports.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }

        protected void btnValidationReports_Click(object sender, EventArgs e)
        {
            try
            {
                //  rvDeployment.Visible = false;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_5','content_5')</script>");
                rvMainReports.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
                string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                rvMainReports.ServerReport.ReportServerUrl = new Uri(@url);
                path = ConfigurationManager.AppSettings["ValidationReportPath"].ToString();
                rvMainReports.ServerReport.ReportPath = ReportViewerPath + path;
                exereport = rvMainReports.ServerReport.ReportPath;
                rvMainReports.ShowParameterPrompts = true;
                rvMainReports.ShowPrintButton = true;
                rvMainReports.ServerReport.Timeout = 30000;
                rvMainReports.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("StartDate", txtReportsStartDate.Text.Replace("-", "/")));

                rvMainReports.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("EndDate", txtReportsEndDate.Text.Replace("-", "/")));


                rvMainReports.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                WriteError(ex);
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