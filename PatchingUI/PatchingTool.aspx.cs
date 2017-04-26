using System;
using System.Web;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data.SqlClient;
using System.Web.UI;

namespace PatchingToolUI
{
    
    public partial class PatchingTool : System.Web.UI.Page
    {
        #region Variables Declaration
        public string exereport = string.Empty;
        string domainAcctNm = string.Empty;
        string scriptsPath = string.Empty;
        string logsPath = string.Empty;
        string output = null;
        string uniqueGUID = null;
        string filename = string.Empty;
        RunbookOperations rs = null;
        string strServerlist = string.Empty;
        string strFupValue = string.Empty;
        private Boolean IsPageRefresh = false;
        string path = string.Empty;
        string strServerNames = string.Empty;
        static string SCRIPT_PATH = HttpContext.Current.Server.MapPath("AddAdmin\\NewAddAdminScript.ps1");
        string connectionString = null;
        SqlConnection conn = null;
        SqlCommand command = null;
        SqlDataAdapter dataAdapter = null;
        DataSet ds;
        

        #endregion

        #region Page_Load
        /// <summary>
        /// Page load event
        /// </summary>
        /// <author>Sudha Gubbala</author>
        /// <CreatedDate>9/26/2012></CreatedDate>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        protected void Page_Load(object sender, EventArgs e)
        {


           // ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", "return disableBackButton()");
            //txtDomainAcctPwd.Attributes.Add("value", "^YHNnhy6");
            txtDomainAcctPwd.Attributes.Add("value", ConfigurationManager.AppSettings["password"].ToString());
            
            // Page.ClientScript.RegisterStartupScript(this.GetType(), "load", "load();", true);
            if (!IsPostBack)
            {
                //Code to maintain unique session id for each tab
                Random r = new Random(DateTime.Now.Millisecond + DateTime.Now.Second * 1000 + DateTime.Now.Minute * 60000 + DateTime.Now.Minute * 3600000);
                PageID.Value = r.Next().ToString();
                ViewState[PageID.Value] = System.Guid.NewGuid().ToString();
                Session[PageID.Value] = System.Guid.NewGuid().ToString();
                BrowserRefresh();
     

            }

          else
            {
                  

                //if (ViewState[PageID.Value].ToString() != Session[PageID.Value].ToString())
                //{
                //    IsPageRefresh = true;
                //}
                //Session[PageID.Value] = System.Guid.NewGuid().ToString();
                //ViewState[PageID.Value] = Session[PageID.Value];    

              BrowserRefresh();
                trODPOption.Attributes.Add("style", "display:none");
                trSimpleUpdateOption.Attributes.Add("style", "display:none");
             

                //if (ViewState[PageID.Value].ToString() != Session[PageID.Value].ToString())
                //{
                //    IsPageRefresh = true;
                //}
                //Session[PageID.Value] = System.Guid.NewGuid().ToString();
                //ViewState[PageID.Value] = Session[PageID.Value];

                if (ddlPatchingOption.SelectedIndex == 2)
                {
                    trODPOption.Attributes.Add("style", "display:block");
                    trSimpleUpdateOption.Attributes.Add("style", "display:none");

                }
                else if (ddlPatchingOption.SelectedIndex == 3)
                {
                    trODPOption.Attributes.Add("style", "display:none");
                    trSimpleUpdateOption.Attributes.Add("style", "display:block");

                }
                else if (ddlPatchingOption.SelectedIndex == 4)
                {
                    trODPOption.Attributes.Add("style", "display:block");
                    trSimpleUpdateOption.Attributes.Add("style", "display:block");

                }
            } 
           
        }

        #endregion
        public void BrowserRefresh()
        {
            if (ViewState[PageID.Value].ToString() != Session[PageID.Value].ToString())
            {
                IsPageRefresh = true;
            }
            Session[PageID.Value] = System.Guid.NewGuid().ToString();
            ViewState[PageID.Value] = Session[PageID.Value];
        }


       //protected override void OnPreRender(EventArgs e)
       // {
       // base.OnPreRender(e);
       // //string strDisAbleBackButton;
       // //strDisAbleBackButton = "<script language="javascript">\n";
       // //strDisAbleBackButton += "window.history.forward(1);\n";
       // //strDisAbleBackButton += "\n</script>";
       // //ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
       // }

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

        #region btnExecute_Click
        /// <summary>
        ///  Click event for calling runbook based on selected tool option
        /// </summary>
        /// <author>Sudha Gubbala</author>
        /// <CreatedDate>9/26/2012</CreatedDate>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void btnExecute_Click(object sender, EventArgs e)
        {
            btnExecute.Enabled = false;
            hdnTimer.Value = "";
            btnCheck.Enabled = false;
            ReportViewer1.Visible = true;
           // fupExcel.Enabled = false;
            gvResults.Visible = false;
            //if (isRefresh == false)
            //{
                //if (IsPageRefresh == false)
                //{
                if (!IsPageRefresh)
                {
                try
                {
                    Guid runbookid = new Guid();
                    rs = new RunbookOperations();
                    string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
                    string serviceRoot = null;
                    string strDestPath = null;
                    //if (flag == "Tk5stosrv03")
                    //{
                    //    serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv03Orchestrator"].ToString();
                    //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk5stosrv03PatchingGUID"].ToString());
                    //    strDestPath = ConfigurationManager.AppSettings["Tk5stoSrv03ExcelFilesPath"].ToString();
                    //}

                    //if (flag == "Tk3OrchSQL01")
                    //{
                    //    serviceRoot = ConfigurationManager.AppSettings["Tk3OrchSQL01Orchestrator"].ToString();
                    //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk3OrchSQL01PatchingGUID"].ToString());
                    //    strDestPath = ConfigurationManager.AppSettings["Tk3orchsql01ExcelFilesPath"].ToString();
                    //}
                    //if (flag == "Tk5stosrv01")
                    //{
                    //    serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv01Orchestrator"].ToString();
                    //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk5stosrv01PatchingGUID"].ToString());
                    //    strDestPath = ConfigurationManager.AppSettings["Tk5stosrv01ExcelFilesPath"].ToString();
                    //}
                    runbookid = new Guid(ConfigurationManager.AppSettings["PatchingGUID"].ToString());
                    strDestPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString();
                    uniqueGUID = Guid.NewGuid().ToString();
                    hdnExecute.Value = uniqueGUID;
                   
                    if (fupExcel.HasFile)
                    {

                        string strName = fupExcel.FileName.Replace(".xlsx", uniqueGUID);
                        string strFileName = strName + ".xlsx";

                        strServerlist = strDestPath + strFileName;
                        fupExcel.SaveAs(strServerlist);
                        hdnFileName.Value = fupExcel.FileName;
                        string strheader = "";
                        if (fupExcel.FileName.Contains(".xlsx"))
                        {
                            strheader = fupExcel.FileName.Replace(".xlsx", "");
                        }
                        else if (fupExcel.FileName.Contains(".xls"))
                        {
                            strheader = fupExcel.FileName.Replace(".xls", "");
                        }
                        

                        output = rs.StartRunbookWithParameters(runbookid, strServerlist, txtDomainAccName.Text, txtDomainAcctPwd.Text, ddlPatchingOption.SelectedIndex.ToString(), txtLogPath.Text, ddlODPOption.SelectedIndex.ToString(), "nope", ddlSimpleUpdateOption.SelectedValue, uniqueGUID, strheader);

                        if (output != null)
                        {
                            if (ddlPatchingOption.SelectedIndex == 1)
                            {
                                path = ConfigurationManager.AppSettings["MSNReportPath"].ToString();
                                //path = "/PatchingReport";
                            }
                            else if (ddlPatchingOption.SelectedIndex == 2)
                            {
                                path = ConfigurationManager.AppSettings["ODPReportPath"].ToString();
                                //path = "/OnDemandPatchingReport";
                            }
                            else if (ddlPatchingOption.SelectedIndex == 3)
                            {
                                path = ConfigurationManager.AppSettings["SimpleUpdateReportPath"].ToString();
                                //path = "/Simple Update Report";
                            }
                            else if (ddlPatchingOption.SelectedIndex == 4)
                            {
                                path = ConfigurationManager.AppSettings["ChainingReportPath"].ToString();
                                //path = "/Chaining Patch Report";
                            }

                            lblRVExecute.Text = "Report Name: " + strheader;
                            LinkExecute.Enabled = true;
                            LinkExecute.ForeColor = System.Drawing.Color.Blue;

                            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                            string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
                            string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                            ReportViewer1.ServerReport.ReportServerUrl = new Uri(@url);
                            ReportViewer1.ServerReport.ReportPath = ReportViewerPath + path;
                            exereport = ReportViewer1.ServerReport.ReportPath;
                            ReportViewer1.ShowParameterPrompts = false;
                            ReportViewer1.ShowPrintButton = true;

                            //Addded by Naveen Arigela for passing UniqueID parameter to SSRS report on 26th Oct 2012
                            // ReportViewer1.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter(uniqueGUID));

                            ReportViewer1.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", uniqueGUID));

                            ReportViewer1.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UserName", txtDomainAccName.Text));


                            ReportViewer1.ServerReport.Refresh();
                            Timer1.Enabled = true;
                            Timer1.Interval = Convert.ToInt32(ddlRefresh.SelectedValue.ToString());
                            hdnExecutionFlag.Value = "link";
                            if (ddlPatchingOption.SelectedIndex == 1)
                            {
                                string s = ConfigurationManager.AppSettings["MSNReportViewerPath"].ToString() + "&rs:Command=Render&UniqueID=" + uniqueGUID;
                                hlLogs.NavigateUrl = s;
                                hlLogs.Enabled = true;
                                hlLogs.Visible = true;
                                //Session[PageID.Value + "UniqueID"] = s;

                            }
                            else if (ddlPatchingOption.SelectedIndex == 2)
                            {
                                //string s = "http://tk5stosrv03.redmond.corp.microsoft.com/ReportServer/Pages/ReportViewer.aspx?%2fPatchingLogs_PreProd%2fOnDemandPatchingReport&rs:Command=Render&UniqueID=" + uniqueGUID;
                                string s = ConfigurationManager.AppSettings["ODPReportViewerPath"].ToString() + "&rs:Command=Render&UniqueID=" + uniqueGUID;
                                hlLogs.NavigateUrl = s;
                                hlLogs.Enabled = true;
                                hlLogs.Visible = true;
                                //Session[PageID.Value + "UniqueID"] = s;
                            }
                            else if (ddlPatchingOption.SelectedIndex == 3)
                            {
                                //string s = "http://tk5stosrv03.redmond.corp.microsoft.com/ReportServer/Pages/ReportViewer.aspx?%2fPatchingLogs_PreProd%2fSimple+Update+Report&rs:Command=Render&UniqueID=" + uniqueGUID;
                                string s = ConfigurationManager.AppSettings["SimpleUpdateReportViewerPath"].ToString() + "&rs:Command=Render&UniqueID=" + uniqueGUID;
                                hlLogs.NavigateUrl = s;
                                hlLogs.Enabled = true;
                                hlLogs.Visible = true;
                                //Session[PageID.Value + "UniqueID"] = s;
                            }
                            else if (ddlPatchingOption.SelectedIndex == 4)
                            {
                                //string s = "http://tk5stosrv03.redmond.corp.microsoft.com/ReportServer/Pages/ReportViewer.aspx?%2fPatchingLogs_PreProd%2fChaining+Patch+Report&rs:Command=Render&UniqueID=" + uniqueGUID;
                                // string s = ConfigurationManager.AppSettings["ChainingReportViewerPath"].ToString() + "&rs:Command=Render&UniqueID=" + uniqueGUID +"&UserName="+txtDomainAccName.Text;

                                string s = ConfigurationManager.AppSettings["ChainingReportViewerPath"].ToString() + "&rs:Command=Render&UniqueID=" + uniqueGUID;
                                hlLogs.NavigateUrl = s;
                                hlLogs.Enabled = true;
                                hlLogs.Visible = true;
                                //Session[PageID.Value + "UniqueID"] = s;
                            }


                        }
                    }


                }





                catch (Exception ex)
                {
                    WriteError(ex);

                }

                finally
                {

                }
            //}
            //}
            //else
            //{
            //    btnExecute.Enabled = false;
            //    if (Session[PageID.Value + "UniqueID"].ToString() != "")
            //    {
            //        Timer1.Enabled = true;
            //        hlLogs.NavigateUrl = Session[PageID.Value + "uniqueID"].ToString();
            //        hlLogs.Enabled = true;
            //        hlLogs.Visible = true;
            //        ReportViewer1.Enabled = true;
            //        ReportViewer1.Visible = true;
            //        //ShowUnitTestLog(Session[PageID.Value + "UniqueID"].ToString());
            //    }
            //    Page.ClientScript.RegisterStartupScript(typeof(Page), "Refresh", "<script type='text/jscript'>alert('Please do not click on Refresh');</script>");
            //}
            }


            else
            {
               // Response.Write("<script type=\"text/javascript\">alert('" + "Please do not click Refresh" + "');</script>");
               // Timer1.Enabled = true;
                //Timer1.Interval = Convert.ToInt32(ddlRefresh.SelectedValue.ToString());
              //  ReportViewer1.Visible = true;
               // Page.ClientScript.RegisterStartupScript(typeof(Page), "Message1", "<script type='text/jscript'>alert('Please do not click  Refresh');</script>");
            }
            
        }
        #endregion

        #region RunScript
        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <CreatedDate></CreatedDate>
        /// <param name="scriptText"></param>
        /// <returns></returns>
        private string RunScript(string scriptText)
        {
            try
            {
                // create Powershell runspace
                Runspace runspace = RunspaceFactory.CreateRunspace();

                // open it
                runspace.Open();

                // create a pipeline and feed it the script text
                Pipeline pipeline = runspace.CreatePipeline();
                pipeline.Commands.AddScript(scriptText);
                // execute the script
                Collection<PSObject> results = pipeline.Invoke();
                // close the runspace
                runspace.Close();
                // convert the script result into a single string
                StringBuilder stringBuilder = new StringBuilder();
                foreach (PSObject obj in results)
                {
                    stringBuilder.AppendLine(obj.ToString());
                }

                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                return "ERRORInHealthCheckTool: " + ex.Message.ToString();
            }
        }

        #endregion

        #region CheckAccess

        #region btnCheck_Click
        /// <summary>
        /// Click event to call a Check the access of servers in the serverlist path
        /// </summary>
        /// <author></author>
        /// <CreatedDate></CreatedDate>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            if (!IsPageRefresh)
            {
                //ReportViewer1.Visible = false;
              //  Timer1.Enabled = false;
                gvResults.DataSource = null;
                gvResults.Visible = false;
                btnCheck.Enabled = false;
                lblRVExecute.Text = "";

                string Orchflag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();

                string strDestPath = null;
                //if (Orchflag == "Tk5stosrv03")
                //{
                //    strDestPath = ConfigurationManager.AppSettings["Tk5stoSrv03ExcelFilesPath"].ToString();
                //}

                //if (Orchflag == "Tk3OrchSQL01")
                //{
                //    strDestPath = ConfigurationManager.AppSettings["Tk3orchsql01ExcelFilesPath"].ToString();
                //}
                //if (Orchflag == "Tk5stosrv01")
                //{
                //    strDestPath = ConfigurationManager.AppSettings["Tk5stosrv01ExcelFilesPath"].ToString();
                //}
                
                //string strDestPath = ConfigurationManager.AppSettings["Tk3orchsql01ExcelFilesPath"].ToString();
                strDestPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString();
                uniqueGUID = Guid.NewGuid().ToString();
                if (fupExcel.HasFile)
                {
                    hdnTimer.Value = "CheckAccess";
                    string strName = "";
                    string strFileName = "";
                    if (fupExcel.FileName.Contains(".xlsx"))
                    {
                        strName = fupExcel.FileName.Replace(".xlsx", uniqueGUID);
                        strFileName = strName + ".xlsx";

                    }
                    else if (fupExcel.FileName.Contains(".xls"))
                    {
                        strName = fupExcel.FileName.Replace(".xls", uniqueGUID);
                        strFileName = strName + ".xls";
                    }
                    else
                    {
                    }
                    strServerlist = strDestPath + strFileName;

                    fupExcel.SaveAs(strServerlist);


                    try
                    {
                        uniqueGUID = Guid.NewGuid().ToString();
                        hdnUniqueAccess.Value = uniqueGUID;
                        rs = new RunbookOperations();
                       // Guid runbookid = new Guid(ConfigurationManager.AppSettings["CheckAccessGUID"].ToString());
                        Guid runbookid = new Guid();
                        // Guid runbookid = new Guid(ConfigurationManager.AppSettings["CheckAccesswithFreeSapceGUID"].ToString());

                        string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
                        string serviceRoot = null;
                        //if (flag == "Tk5stosrv03")
                        //{
                        //    serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv03Orchestrator"].ToString();
                        //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk5stosrv03CheckAccessGUID"].ToString());
                        //}

                        //if (flag == "Tk3OrchSQL01")
                        //{
                        //    serviceRoot = ConfigurationManager.AppSettings["Tk3OrchSQL01Orchestrator"].ToString();
                        //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk3OrchSQL01CheckAccessGUID"].ToString());
                        //}

                        //if (flag == "Tk5stosrv01")
                        //{
                        //    serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv01Orchestrator"].ToString();
                        //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk5stosrv01CheckAccessGUID"].ToString());
                        //}
                        runbookid = new Guid(ConfigurationManager.AppSettings["CheckAccessGUID"].ToString());
                        output = rs.StartValidateRunbook(runbookid, strServerlist, txtDomainAccName.Text, txtDomainAcctPwd.Text, txtLogPath.Text, uniqueGUID,serviceRoot);
                        //uniqueGUID = "abcd01";
                       // System.Threading.Thread.Sleep(10000);
                       // int checkvalue = PrepareCheckAccessGrid(hdnUniqueAccess.Value);
                        path = ConfigurationManager.AppSettings["CheckAccessReportPath"].ToString();

                        ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                        string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
                        string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                        ReportViewer1.ServerReport.ReportServerUrl = new Uri(@url);
                        ReportViewer1.ServerReport.ReportPath = ReportViewerPath + path;
                        exereport = ReportViewer1.ServerReport.ReportPath;
                        ReportViewer1.ShowParameterPrompts = false;
                        ReportViewer1.ShowPrintButton = true;
                        ReportViewer1.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", uniqueGUID));

                    //    ReportViewer1.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UserName", txtDomainAccName.Text));


                        ReportViewer1.ServerReport.Refresh();
                        Timer1.Enabled = true;
                        Timer1.Interval = Convert.ToInt32(ddlRefresh.SelectedValue.ToString());
                      //  CheckAccessTimer.Enabled = true;
                       // CheckAccessTimer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["CheckAccessTimeOut"].ToString());

                       
                    }
                    catch (Exception ex)
                    {

                        WriteError(ex);

                    }

                    finally
                    {

                    }

                }
            }
            else
            {
            }
              
        }
        protected void CheckAccessTimer_Tick(object sender, EventArgs e)
        {

            TimerCheckAccessValues(hdnUniqueAccess.Value);
            CheckAccessTimer.Enabled = false;

        }

        public int PrepareCheckAccessGrid(string UniqueID)
        {
          

            int count = 1;
            System.Threading.Thread.Sleep(10000);
            TimerCheckAccessValues(UniqueID);
            while (count != 0)
            {
                string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();

                if (flag == "Tk5stosrv03")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["TK5_ConnectionString"].ConnectionString;
                }
                if (flag == "Tk3OrchSQL01")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["TK3_ConnectionString"].ConnectionString;

                }
                if (flag == "Tk5stosrv01")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["TK5stosrv01_ConnectionString"].ConnectionString;

                }
                
                //connectionString = ConfigurationManager.ConnectionStrings["TK5_ConnectionString"].ConnectionString;
                SqlConnection sqlConnection1 = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                //cmd.CommandText = "select count(*) from dbo.CheckAdminAccess (nolock) where (AdminAccess= " + "'Access'" + "or AdminAcess =" + "'NoAccess'" + " ) and UniqueID='" + UniqueID + "'";
                //cmd.CommandText = "select count(*) from dbo.CheckAdminAccess (nolock) where AdminAccess in ('InProgress','') and UniqueID='" + UniqueID + "'";

                cmd.CommandText = "select count(*) from dbo.tblCheckAdminAccess (nolock) where AdminAccess in ('InProgress','') and UniqueID='" + UniqueID + "'";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;
                sqlConnection1.Open();
                count = (int)cmd.ExecuteScalar();
                // Data is accessible through the DataReader object here.
                sqlConnection1.Close();
            }
            if (count == 0)
            {
               // Timer1.Enabled = false;
                TimerCheckAccessValues(UniqueID);
                
            }
            else
            {
                hdnTimer.Value = "CheckAccess";
                //Timer1.Enabled = true;
            }
            return 0;
        }

        public bool checkboxVisible(string value)
        {
            bool flag = false;
            if (value == "Access")
                flag = false;
            else
                flag = true;
            return flag;
        }
        public void TimerCheckAccessValues(string uniqueID)
        {
           
            try
            {
               // uniqueID = "bfd42bab-a028-476b-b22b-24604a4fd569";
                ds = new DataSet();
                string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();

                if (flag == "Tk5stosrv03")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["TK5_ConnectionString"].ConnectionString;
                }
                if (flag == "Tk3OrchSQL01")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["TK3_ConnectionString"].ConnectionString;

                }
                if (flag == "Tk5stosrv01")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["TK5stosrv01_ConnectionString"].ConnectionString;

                }
                
               // connectionString = ConfigurationManager.ConnectionStrings["TK5_ConnectionString"].ConnectionString;
                conn = new SqlConnection(connectionString);
                conn.Open();
                command = new SqlCommand("usp_GetCheckAccessWithSpaceResults", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UniqueID", uniqueID);
                //dr = command.ExecuteReader();
                dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvResults.DataSource = ds.Tables[0];

                    gvResults.DataBind();
                }
                else
                {
                    gvResults.DataSource = null;
                    gvResults.DataBind();
                }

                hdnServerNames.Value = "";
                foreach (GridViewRow gvRow in gvResults.Rows)
                {
                    CheckBox chkAdmin = (CheckBox)gvRow.FindControl("chkAdmin");
                    Label lblServername = (Label)gvRow.FindControl("lblAdminStatus");
                    if (lblServername.Text == "Access")
                    {

                        chkAdmin.Visible = false;
                    }
                    else
                        chkAdmin.Visible = true;

                }         
                //if (dr.HasRows)
                //{
                //    // output =1 ;
                //    gvResults.DataSource = dr;
                //    gvResults.DataBind();
                //}

            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
            finally
            {
                conn.Close();
            }
        }

        public void TimerAddAdminValues(string uniqueID)
        {
          
            try
            {
                // uniqueID = "bfd42bab-a028-476b-b22b-24604a4fd569";
                ds = new DataSet();
                string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();

                if (flag == "Tk5stosrv03")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["TK5_ConnectionString"].ConnectionString;
                }
                if (flag == "Tk3OrchSQL01")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["TK3_ConnectionString"].ConnectionString;

                }
                if (flag == "Tk5stosrv01")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["TK5stosrv01_ConnectionString"].ConnectionString;

                }
                
               // connectionString = ConfigurationManager.ConnectionStrings["TK5_ConnectionString"].ConnectionString;
                conn = new SqlConnection(connectionString);
                conn.Open();
                command = new SqlCommand("usp_GetAddAdminResults", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UniqueID", uniqueID);
                //dr = command.ExecuteReader();
                dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvAddAdmin.DataSource = ds.Tables[0];
                    gvAddAdmin.DataBind();
                }

              
                //if (dr.HasRows)
                //{
                //    // output =1 ;
                //    gvResults.DataSource = dr;
                //    gvResults.DataBind();
                //}

            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region CallCheckAccessScript
        /// <summary>
        ///checks the admin access for servers listed in the excel file 
        /// </summary>
        /// <authot>sudha gubbala</authot>
        /// <Createddate>16Oct2012</Createddate>
        /// <param name="servername"></param>
        /// <returns></returns>

        Collection<PSObject> CallCheckAccessScript(string servername)
        {
            string checkSCRIPT = HttpContext.Current.Server.MapPath("AddAdmin\\CheckAccess.ps1");
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            RunspaceInvoke runSpaceInvoker = new RunspaceInvoke(runspace);
            runSpaceInvoker.Invoke("Set-ExecutionPolicy Unrestricted");
            // create a pipeline and feed it the script text        
            Pipeline pipeline = runspace.CreatePipeline();

            Command command = new Command(checkSCRIPT);
            //foreach (var file in filesToMerge)         
            //{            
            //  command.Parameters.Add(null, file);       
            //}         

            command.Parameters.Add(null, txtDomainAccName.Text);
            command.Parameters.Add(null, txtDomainAcctPwd.Text);
            command.Parameters.Add(null, servername);


            pipeline.Commands.Add(command);
            Collection<PSObject> returnObjects = pipeline.Invoke();
            runspace.Close();

            return returnObjects;
        }

        #endregion     

        #endregion

        #region AddAdminScript
        /// <summary>
        /// 
        /// </summary>
        /// <author>Sudha Gubbala</author>
        /// <CreatedDate>12 Oct 2012</CreatedDate>
        /// <ModifiedDate>15 Oct 2012</ModifiedDate>
        //Code added for sending existing username and passwords to powershell script
        /// <param name="serverspath"></param>
        /// <param name="resultspath"></param>
        /// <param name="AdminAcctName"></param>
        /// <returns></returns>
        Collection<PSObject> UsingPowerShell(string serverspath, string resultspath)
        {         // create Powershell runspace       
            string strAdminName = txtAdmin.Text;
            string strPassword = txtPassword.Text;
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            RunspaceInvoke runSpaceInvoker = new RunspaceInvoke(runspace);
            runSpaceInvoker.Invoke("Set-ExecutionPolicy Unrestricted");
            // create a pipeline and feed it the script text        
            Pipeline pipeline = runspace.CreatePipeline();
            Command command = new Command(SCRIPT_PATH);
            //foreach (var file in filesToMerge)         
            //{            
            //  command.Parameters.Add(null, file);       
            //}         
            command.Parameters.Add(null, serverspath);
            command.Parameters.Add(null, resultspath);
            // command.Parameters.Add(null, AdminAcctName);
            command.Parameters.Add(null, strAdminName);
            command.Parameters.Add(null, strPassword);
            pipeline.Commands.Add(command);
            Collection<PSObject> returnObjects = pipeline.Invoke();
            runspace.Close();

            return returnObjects;
        }
        #endregion

        #region btnAddAdminScript_Click
        /// <summary>
        /// event will fire when we click add admin button in the popup
        /// </summary>
        /// <author>Sudha Gubbala</author>
        /// <CreatedDate>15 Oct 2012</CreatedDate>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void btnAddAdminScript_Click(object sender, EventArgs e)
        {
            hdnServerNames.Value = "";
            foreach (GridViewRow gvRow in gvResults.Rows)
            {
                CheckBox chkAdmin = (CheckBox)gvRow.FindControl("chkAdmin");

                if (chkAdmin.Checked == true)
                {
                    //flag = 1;
                    Label lblServername = (Label)gvRow.FindControl("lblServername");
                    hdnServerNames.Value = hdnServerNames.Value + "&" + lblServername.Text;

                }


            }


            gvResults.DataSource = null;
            string[] lines = hdnServerNames.Value.Split('&');
            string serverspath = "";
            
           // string serverspath = ConfigurationManager.AppSettings["AddAdminServersPath"].ToString();

             string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
             string serviceRoot = null;
                //if (flag == "Tk5stosrv03")
                //{
                //    serverspath = ConfigurationManager.ConnectionStrings["Tk5stosrv03AddAdminServersPath"].ConnectionString;
                //    serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv03Orchestrator"].ToString();
                   
                //}
                //if (flag == "Tk3OrchSQL01")
                //{
                //    serverspath = ConfigurationManager.ConnectionStrings["Tk3orchsql01AddAdminServersPath"].ConnectionString;
                //    serviceRoot = ConfigurationManager.AppSettings["Tk3OrchSQL01Orchestrator"].ToString();
                 
                //}
                //if (flag == "Tk5stosrv01")
                //{
                //    serverspath = ConfigurationManager.ConnectionStrings["Tk5stosrv01AddAdminServersPath"].ConnectionString;
                //    serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv01Orchestrator"].ToString();

                //}
            // string textfilepath = HttpContext.Current.Server.MapPath(@"AddAdmin\Servers.txt");
             serverspath = ConfigurationManager.ConnectionStrings["AddAdminServersPath"].ConnectionString;
             // serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv03Orchestrator"].ToString();
                   
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(serverspath))
            {
                foreach (string line in lines)
                {
                    if (line != "\r" && line != "")
                    {
                        file.WriteLine(line);
                    }
                }
            }
            
            //string AddAdminServersLogPath = ConfigurationManager.AppSettings["AddAdminServersLogPath"].ToString();
            string strDestPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString();
          

                try
                {
                    Guid runbookid = new Guid();
                    uniqueGUID = Guid.NewGuid().ToString();
                    hdnAddAdmin.Value = uniqueGUID;
                    rs = new RunbookOperations();
                   // runbookid = new Guid(ConfigurationManager.AppSettings["AddAdminGUID"].ToString());
                    //string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
                    //string serviceRoot = null;

                    //if (flag == "Tk5stosrv03")
                    //{
                       
                    //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk5stosrv03AddAdminGUID"].ToString());
                    //}

                    //if (flag == "Tk3OrchSQL01")
                    //{
                        
                    //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk3OrchSQL01AddAdminGUID"].ToString());
                    //}
                    //if (flag == "Tk5stosrv01")
                    //{

                    //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk5stosrv03AddAdminGUID"].ToString());
                    //}
                    runbookid = new Guid(ConfigurationManager.AppSettings["AddAdminGUID"].ToString());
                    output = rs.StartValidateAddAdminRunbook(runbookid, serverspath, txtDomainAccName.Text, txtDomainAcctPwd.Text, txtLogPath.Text, txtAdmin.Text, txtPassword.Text, uniqueGUID,serviceRoot,"4");

                    System.Threading.Thread.Sleep(10000);
                    gvAddAdmin.Visible = true;
                    gvAddAdmin.DataSource = null;
                    gvResults.Visible = false;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallResult", "load();", true);
                
                    PrepareAddAdminGrid(hdnAddAdmin.Value);

                }
                catch (Exception ex)
                {

                    WriteError(ex);

                }

                finally
                {

                }
            

            //UsingPowerShell(HttpContext.Current.Server.MapPath("AddAdmin\\Servers.txt"), HttpContext.Current.Server.MapPath("AddAdmin\\results.txt"));

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallJS", "Disable();", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallResult", "load();", true);
            lblValue.Visible = true;
           // lnkResult.Visible = true;
           // gvAddAdmin.Visible = false;

        }

        public void PrepareAddAdminGrid(string AddAdminUniqueID)
        {
           
           
            //connectionString = ConfigurationManager.ConnectionStrings["TK5_ConnectionString"].ConnectionString;
            //SqlConnection sqlConnection1 = new SqlConnection(connectionString);
            //SqlCommand cmd = new SqlCommand();

            //cmd.CommandText = "select count(*) from dbo.AddAdmin (nolock) where Status is Null and UniqueID='" + hdnAddAdmin.Value + "'";
            //cmd.CommandType = CommandType.Text;
            //cmd.Connection = sqlConnection1;
            //sqlConnection1.Open();
            //int count = (int)cmd.ExecuteScalar();
            //// Data is accessible through the DataReader object here.
            //sqlConnection1.Close();
            //TimerAddAdminValues(hdnAddAdmin.Value);
            //if (count == 0)
            //{
            //    Timer1.Enabled = false;


            //}
            //else
            //{
            //    hdnTimer.Value = "AddAdmin";
            //    Timer1.Enabled = true;
            //}
            ////return 0;
            int count = 1;
            //System.Threading.Thread.Sleep(10000);
            TimerAddAdminValues(AddAdminUniqueID);
            while (count != 0)
            {
                
                 string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();

                 if (flag == "Tk5stosrv03")
                 {
                     connectionString = ConfigurationManager.ConnectionStrings["TK5_ConnectionString"].ConnectionString;
                 }
                 if (flag == "Tk3OrchSQL01")
                 {
                     connectionString = ConfigurationManager.ConnectionStrings["TK3_ConnectionString"].ConnectionString;

                 }
                 if (flag == "Tk5stosrv01")
                 {
                     connectionString = ConfigurationManager.ConnectionStrings["TK5stosrv01_ConnectionString"].ConnectionString;
                 }
               
                SqlConnection sqlConnection1 = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                //cmd.CommandText = "select count(*) from dbo.CheckAdminAccess (nolock) where (AdminAccess= " + "'Access'" + "or AdminAcess =" + "'NoAccess'" + " ) and UniqueID='" + UniqueID + "'";
                cmd.CommandText = "select count(*) from dbo.AddAdmin (nolock) where Status in ('InProgress','') and UniqueID='" + AddAdminUniqueID + "'";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;
                sqlConnection1.Open();
                count = (int)cmd.ExecuteScalar();
                // Data is accessible through the DataReader object here.
                sqlConnection1.Close();
            }
            if (count == 0)
            {
                // Timer1.Enabled = false;
                TimerAddAdminValues(AddAdminUniqueID);

            }
            else
            {
                hdnTimer.Value = "AddAdmin";
                //Timer1.Enabled = true;
            }
            

        }
        #endregion

        #region lnkResult_Click
        /// <summary>
        /// Event will fire when we click on see results here link in the popup after adding admin
        /// </summary>
        /// <author>Sudha Gubbala</author>
        /// <CreatedDate>15 Oct 2012</CreatedDate>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkResult_Click(object sender, EventArgs e)
        {
            //gvAddAdmin.Visible = true;
            //gvAddAdmin.DataSource = null;
            //gvResults.Visible = false;
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallResult", "load();", true);
            ////txtResult.Visible = true;
            //DataTable table = new DataTable();



            //table.Columns.Add("Result", typeof(string));           
            ////FileStream st1 = new FileStream(HttpContext.Current.Server.MapPath("AddAdmin\\results.txt"), FileMode.Open, FileAccess.Read);
            ////StreamReader SR1 = new StreamReader(st1);
            ////txtResult.Text = SR1.ReadToEnd();
            ////st1.Close();
            //PrepareAddAdminGrid();
        }
        #endregion

        #region Timer Tick event
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            string strStatus = string.Empty;
            string strPatchOption = string.Empty;

            if (hdnTimer.Value=="CheckAccess")
            {
                //TimerCheckAccessValues(hdnUniqueAccess.Value);
                path = ConfigurationManager.AppSettings["CheckAccessReportPath"].ToString();
                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
                string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                ReportViewer1.ServerReport.ReportServerUrl = new Uri(@url);
                ReportViewer1.ServerReport.ReportPath = ReportViewerPath + path;
                exereport = ReportViewer1.ServerReport.ReportPath;
                ReportViewer1.ShowParameterPrompts = false;
                ReportViewer1.ShowPrintButton = true;

                //Addded by Naveen Arigela for passing UniqueID parameter to SSRS report on 26th Oct 2012
                // ReportViewer1.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter(uniqueGUID));

                ReportViewer1.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", hdnUniqueAccess.Value));

                //ReportViewer1.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UserName", txtDomainAccName.Text));


                ReportViewer1.ServerReport.Refresh();
            }
            else if (hdnTimer.Value == "AddAdmin")
            {
                TimerAddAdminValues(hdnAddAdmin.Value);
            }
            else
            {

                try
                {
                    if (ddlPatchingOption.SelectedIndex == 1)
                    {
                        path = ConfigurationManager.AppSettings["MSNReportPath"].ToString();
                        //path = "/PatchingReport";
                        strPatchOption = "MSNPatch";
                    }
                    else if (ddlPatchingOption.SelectedIndex == 2)
                    {
                        path = ConfigurationManager.AppSettings["ODPReportPath"].ToString();
                        //path = "/OnDemandPatchingReport";
                        strPatchOption = "ODP";
                    }
                    else if (ddlPatchingOption.SelectedIndex == 3)
                    {
                        path = ConfigurationManager.AppSettings["SimpleUpdateReportPath"].ToString();
                        //path = "/Simple Update Report";
                        strPatchOption = "SimpleUpdate";
                    }
                    else if (ddlPatchingOption.SelectedIndex == 4)
                    {
                        path = ConfigurationManager.AppSettings["ChainingReportPath"].ToString();
                        //path = "/Chaining Patch Report";
                        strPatchOption = "Chaining";
                    }
                    
                    lblRVExecute.Text = "Report Name: " + hdnFileName.Value;
                    
                    //ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                    //ReportViewer1.ServerReport.ReportServerUrl = new Uri(@"http://tk5stosrv03/ReportServer");
                    // ReportViewer1.ServerReport.ReportPath = "/PatchingLogs_PreProd" + path;
                    //ReportViewer1.ShowParameterPrompts = true;
                    //ReportViewer1.ShowPrintButton = true;
                    //ReportViewer1.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", hdnExecute.Value));

                    //ReportViewer1.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UserName", "redmond\\stpatcha"));

                    

                    ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                    string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
                    string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                    ReportViewer1.ServerReport.ReportServerUrl = new Uri(@url);
                    ReportViewer1.ServerReport.ReportPath = ReportViewerPath + path;
                    exereport = ReportViewer1.ServerReport.ReportPath;
                    ReportViewer1.ShowParameterPrompts = false;
                    ReportViewer1.ShowPrintButton = true;

                    //Addded by Naveen Arigela for passing UniqueID parameter to SSRS report on 26th Oct 2012
                    // ReportViewer1.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter(uniqueGUID));

                    ReportViewer1.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", hdnExecute.Value));

                    ReportViewer1.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UserName", txtDomainAccName.Text));


                    ReportViewer1.ServerReport.Refresh();


                    strStatus = GetRunbookStatus(hdnExecute.Value, strPatchOption,"Normal");

                    if (strStatus == "Completed")
                    {
                        Timer1.Enabled = false;
                    }

 


                }
                catch (Exception ex)
                {
                    WriteError(ex);
                }
                finally
                {

                }
            }

        }
        #endregion

        #region CallValidateScript
        /// <summary>
        ///Verify the Kbs installed for servers listed in the excel file 
        /// </summary>
        /// <authot>sudha gubbala</authot>
        /// <Createddate>16Oct2012</Createddate>
        /// <param name="servername"></param>
        /// <returns></returns>

        //Collection<PSObject> VerifyPatchScript(string strText)
        //{
        //    string checkSCRIPT = HttpContext.Current.Server.MapPath("AddAdmin\\VerifyPatch.ps1");
        //    Runspace runspace = RunspaceFactory.CreateRunspace();
        //    runspace.Open();
        //    RunspaceInvoke runSpaceInvoker = new RunspaceInvoke(runspace);
        //    runSpaceInvoker.Invoke("Set-ExecutionPolicy Unrestricted");
        //    // create a pipeline and feed it the script text        
        //    Pipeline pipeline = runspace.CreatePipeline();

        //    Command command = new Command(checkSCRIPT);
        //    //foreach (var file in filesToMerge)         
        //    //{            
        //    //  command.Parameters.Add(null, file);       
        //    //}         

        //    command.Parameters.Add(null, strText);
        //    command.Parameters.Add(null, HttpContext.Current.Server.MapPath("AddAdmin\\ValidateServers.txt"));
        //    command.Parameters.Add(null, HttpContext.Current.Server.MapPath("AddAdmin\\ValidateResults.txt"));


        //    pipeline.Commands.Add(command);
        //    Collection<PSObject> returnObjects = pipeline.Invoke();
        //    runspace.Close();

        //    return returnObjects;
        //}

        public void VerifyPatchScript(string KBNumbers, string serversList)
        {
            try
            {
                Guid runbookid = new Guid();
                uniqueGUID = Guid.NewGuid().ToString();
                rs = new RunbookOperations();
                runbookid = new Guid(ConfigurationManager.AppSettings["VerifyPatchGUID"].ToString());
                string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
                string serviceRoot = null;
                //if (flag == "Tk5stosrv03")
                //{
                //    serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv03Orchestrator"].ToString();
                //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk5stosrv03VerifyPatchGUID"].ToString());
                //}

                //if (flag == "Tk3OrchSQL01")
                //{
                //    serviceRoot = ConfigurationManager.AppSettings["Tk3OrchSQL01Orchestrator"].ToString();
                //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk3OrchSQL01VerifyPatchGUID"].ToString());
                //}
                //if (flag == "Tk5stosrv01")
                //{
                //    serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv01Orchestrator"].ToString();
                //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk5stosrv01VerifyPatchGUID"].ToString());
                //}
               // runbookid = new Guid(ConfigurationManager.AppSettings["VerifyPatchGUID"].ToString());
                output = rs.StartValidateVerifyPatchRunbook(runbookid, serversList, txtDomainAccName.Text, txtDomainAcctPwd.Text, KBNumbers, uniqueGUID,serviceRoot);
                // rvValidate.Visible = false;


                if (output != null)
                {
                    System.Threading.Thread.Sleep(25000);
                    TimerValidate.Enabled = true;
                }
                else
                {

                }

            }
            catch (Exception ex)
            {

                WriteError(ex);

            }

            finally
            {

            }

        }





        #endregion

        #region btnValidate_Click

        /// <summary>
        /// Validates the Kbs installed on the servers listed in the excel file
        /// </summary>
        /// <author>sudha gubbala</author>
        /// <createddate>17Oct2012</createddate>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        string strValidateExcel = string.Empty;



   
        protected void btnValidate_Click(object sender, EventArgs e)
        {
            if (!IsPageRefresh)
            {
               // string strDestPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString();
                string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
                string serviceRoot = null;
                string strDestPath = null;
                //if (flag == "Tk5stosrv03")
                //{
                //   // serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv03Orchestrator"].ToString();
                //   // runbookid = new Guid(ConfigurationManager.AppSettings["Tk5stosrv03PatchingGUID"].ToString());
                //    strDestPath = ConfigurationManager.AppSettings["Tk5stoSrv03ExcelFilesPath"].ToString();
                //}

                //if (flag == "Tk3OrchSQL01")
                //{
                //    //serviceRoot = ConfigurationManager.AppSettings["Tk3OrchSQL01Orchestrator"].ToString();
                //   // runbookid = new Guid(ConfigurationManager.AppSettings["Tk3OrchSQL01PatchingGUID"].ToString());
                //    strDestPath = ConfigurationManager.AppSettings["Tk3orchsql01ExcelFilesPath"].ToString();
                //}
                //if (flag == "Tk5stosrv01")
                //{
                //   // serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv01Orchestrator"].ToString();
                //   // runbookid = new Guid(ConfigurationManager.AppSettings["Tk5stosrv01PatchingGUID"].ToString());
                //    strDestPath = ConfigurationManager.AppSettings["Tk5stosrv01ExcelFilesPath"].ToString();
                //}
                strDestPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString();
                rvValidate.Visible = false;
                uniqueGUID = Guid.NewGuid().ToString();
                // hdnExcelPath.Value=@"\\TK5STOSRV03\Sudha\TestAccessbe0a9dfd-59d2-4ce5-b731-950537d2af58.xlsx";
                string strName = "";
                string strFileName = "";
                if (fupExcel.HasFile)
                {


                    if (fupExcel.FileName.Contains(".xlsx"))
                    {
                        strName = fupExcel.FileName.Replace(".xlsx", uniqueGUID);
                        strFileName = strName + ".xlsx";

                    }
                    else if (fupExcel.FileName.Contains(".xls"))
                    {
                        strName = fupExcel.FileName.Replace(".xls", uniqueGUID);
                        strFileName = strName + ".xls";
                    }
                    else
                    {
                    }
                    strValidateExcel = strDestPath + strFileName;
                    hdnExcelPath.Value = strValidateExcel;
                    fupExcel.SaveAs(strValidateExcel);
                    // hdnFileName.Value = fupExcel.FileName;

                    string strheader = "";
                    if (fupExcel.FileName.Contains(".xlsx"))
                    {
                        strheader = fupExcel.FileName.Replace(".xlsx", "");
                    }
                    else if (fupExcel.FileName.Contains(".xls"))
                    {
                        strheader = fupExcel.FileName.Replace(".xls", "");
                    }

                    hdnFileName.Value = strheader;

                }

            //    string url = "ValidatePatch.aspx";


              //  ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>openNewWin('" + url + "')</script>");


                if (cbNumbers.Checked == true)
                {
                  
                    lblRvValidate.Text = "";
                   


                    ValidatePopup.Attributes.Add("class", "ValidatePopup");
                    gvValidateResult.Visible = true;
                    rvValidate.Visible = false;
                   
                    VerifyPatchScript(txtKbValue.Text, hdnExcelPath.Value);
                   

                    PrepareValidateGrid(hdnExcelPath.Value);



                }

                if (cbSimpleUpdate.Checked == true)
                {
                    ValidatePopup.Attributes.Add("class", "rvValidatePopup");
                    rvValidate.Visible = true;
                    gvValidateResult.Visible = false;
                    lblRvValidate.Text = hdnFileName.Value;
                    try
                    {
                        Guid runbookid = new Guid();
                        uniqueGUID = Guid.NewGuid().ToString();
                        hdnCbSimpleUpdate.Value = uniqueGUID;
                        rs = new RunbookOperations();

                       runbookid = new Guid(ConfigurationManager.AppSettings["PatchingGUID"].ToString());
                      //  string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
                       // string serviceRoot = null;
                        //if (flag == "Tk5stosrv03")
                        //{
                        //    serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv03Orchestrator"].ToString();
                        //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk5stosrv03PatchingGUID"].ToString());
                        //}

                        //if (flag == "Tk3OrchSQL01")
                        //{
                        //    serviceRoot = ConfigurationManager.AppSettings["Tk3OrchSQL01Orchestrator"].ToString();
                        //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk3OrchSQL01PatchingGUID"].ToString());
                        //}
                        //if (flag == "Tk5stosrv01")
                        //{
                        //    serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv01Orchestrator"].ToString();
                        //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk5stosrv01PatchingGUID"].ToString());
                        //}
                        output = rs.StartRunbookWithParameters(runbookid, hdnExcelPath.Value, txtDomainAccName.Text, txtDomainAcctPwd.Text, "3", txtLogPath.Text, "0", "nope", "Preview", uniqueGUID, hdnFileName.Value);

                        // rvValidate.Visible = false;
                        rvValidate.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                        //  path = "/PatchingReport";     

                        rvValidate.ServerReport.ReportServerUrl = new Uri(@"http://tk5stosrv03/ReportServer");
                        rvValidate.ServerReport.ReportPath = "/PatchingLogs_PreProd/SimpleUpdateValidationReport";
                        rvValidate.ShowParameterPrompts = false;
                        rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", uniqueGUID));

                        // rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UserName", txtDomainAccName.Text));

                        rvValidate.ShowPrintButton = true;
                        rvValidate.ServerReport.Refresh();
                        //TimerValidate.Interval = Convert.ToInt32(ddlRefresh.SelectedValue.ToString());
                        TimerValidate.Enabled = true;
                        TimerValidate.Interval = Convert.ToInt32(ddlValidateRefresh.SelectedValue.ToString());


                    }
                    catch (Exception ex)
                    {

                        WriteError(ex);

                    }

                    finally
                    {

                    }

                }
                if (cbMSNPatch.Checked == true)
                {
                    ValidatePopup.Attributes.Add("class", "rvValidatePopup");
                    rvValidate.Visible = true;
                    gvValidateResult.Visible = false;
                    lblRvValidate.Text = hdnFileName.Value;
                    try
                    {
                        Guid runbookid = new Guid();
                        uniqueGUID = Guid.NewGuid().ToString();
                        hdnCbMSNPatch.Value = uniqueGUID;
                        rs = new RunbookOperations();
                       runbookid = new Guid(ConfigurationManager.AppSettings["ValidateGUID"].ToString());
                       // string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
                       // string serviceRoot = null;
                        //if (flag == "Tk5stosrv03")
                        //{
                        //    serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv03Orchestrator"].ToString();
                        //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk5stosrv03ValidateGUID"].ToString());
                        //}

                        //if (flag == "Tk3OrchSQL01")
                        //{
                        //    serviceRoot = ConfigurationManager.AppSettings["Tk3OrchSQL01Orchestrator"].ToString();
                        //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk3OrchSQL01ValidateGUID"].ToString());
                        //}
                        //if (flag == "Tk5stosrv01")
                        //{
                        //    serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv01Orchestrator"].ToString();
                        //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk5stosrv01PatchingGUID"].ToString());
                        //}
                        output = rs.StartValidateRunbook(runbookid, hdnExcelPath.Value, txtDomainAccName.Text, txtDomainAcctPwd.Text, txtLogPath.Text, uniqueGUID,serviceRoot);

                        // rvValidate.Visible = false;
                        rvValidate.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                        //  path = "/PatchingReport";     
                        rvValidate.ServerReport.ReportServerUrl = new Uri(@"http://tk5stosrv03/ReportServer");
                        rvValidate.ServerReport.ReportPath = "/PatchingLogs_PreProd/" + "MSNPatchValidationReport";
                        rvValidate.ShowParameterPrompts = false;
                        rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", uniqueGUID));

                        //  rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UserName", "redmond\\stpatcha"));
                        rvValidate.ShowPrintButton = true;
                        rvValidate.ServerReport.Refresh();
                        //TimerValidate.Interval = Convert.ToInt32(ddlRefresh.SelectedValue.ToString());
                        TimerValidate.Enabled = true;
                        TimerValidate.Interval = Convert.ToInt32(ddlValidateRefresh.SelectedValue.ToString());



                    }
                    catch (Exception ex)
                    {

                        WriteError(ex);

                    }

                    finally
                    {

                    }


                }




                if (cbODP.Checked == true)
                {
                    ValidatePopup.Attributes.Add("class", "rvValidatePopup");
                    rvValidate.Visible = true;
                    gvValidateResult.Visible = false;
                    lblRvValidate.Text = hdnFileName.Value;
                    try
                    {
                        Guid runbookid = new Guid();
                        uniqueGUID = Guid.NewGuid().ToString();
                        hdnCbODP.Value = uniqueGUID;
                        rs = new RunbookOperations();                      
                       runbookid = new Guid(ConfigurationManager.AppSettings["PatchingGUID"].ToString());
                       // string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
                       // string serviceRoot = null;
                        //if (flag == "Tk5stosrv03")
                        //{
                        //    serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv03Orchestrator"].ToString();
                        //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk5stosrv03PatchingGUID"].ToString());
                        //}

                        //if (flag == "Tk3OrchSQL01")
                        //{
                        //    serviceRoot = ConfigurationManager.AppSettings["Tk3OrchSQL01Orchestrator"].ToString();
                        //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk3OrchSQL01PatchingGUID"].ToString());
                        //}
                        //if (flag == "Tk5stosrv01")
                        //{
                        //    serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv01Orchestrator"].ToString();
                        //    runbookid = new Guid(ConfigurationManager.AppSettings["Tk5stosrv01PatchingGUID"].ToString());
                        //}
                        output = rs.StartRunbookWithParameters(runbookid, hdnExcelPath.Value, txtDomainAccName.Text, txtDomainAcctPwd.Text, "2", txtLogPath.Text, "0", "nope", "Scan", uniqueGUID, hdnFileName.Value);
                        // rvValidate.Visible = false;
                        rvValidate.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                        //  path = "/PatchingReport";     
                        rvValidate.ServerReport.ReportServerUrl = new Uri(@"http://tk5stosrv03/ReportServer");
                        rvValidate.ServerReport.ReportPath = "/PatchingLogs_PreProd/" + "ODPValidationReport";
                        rvValidate.ShowParameterPrompts = false;
                        rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", uniqueGUID));

                        // rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UserName", "redmond\\stpatcha"));
                        rvValidate.ShowPrintButton = true;
                        rvValidate.ServerReport.Refresh();
                        //TimerValidate.Interval = Convert.ToInt32(ddlRefresh.SelectedValue.ToString());
                        TimerValidate.Enabled = true;
                        TimerValidate.Interval = Convert.ToInt32(ddlValidateRefresh.SelectedValue.ToString());



                    }
                    catch (Exception ex)
                    {

                        WriteError(ex);

                    }

                    finally
                    {

                    }


                }

                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallResult", "loadValidate();", true);

            }
            else
            {
            }


        }

        /// <summary>
        /// writes the data to text file
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        public void WriteDataToTextFile(DataTable dt, string filePath)
        {
            int i = 0;
            if (File.Exists(filePath))
            {
            }
            else
            {
                // File.CreateText(filePath);
                using (StreamWriter sr = File.CreateText(filePath))
                {
                    sr.Close();
                }


            }
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(filePath, false);
                foreach (DataRow row in dt.Rows)
                {
                    if (row[1].ToString() != "")
                    {
                        object[] array = row.ItemArray;
                        for (i = 0; i < array.Length - 1; i++)
                        {
                            if (array[0].ToString() != "")
                            {
                                sw.Write(array[0].ToString());

                            }
                        }

                        sw.WriteLine();
                    }
                } sw.Close();

            }
            catch (Exception ex)
            {
                WriteError(ex);
            }


        }

        #endregion

        #region ValidateGrid

        /// <summary>
        /// Dynamically loading grid based on entered Kb values in validate popup
        /// <author>Sudha Gubbala</author>
        /// <CreatedDate>17Oct2012</CreatedDate>
        /// </summary>

        public void PrepareValidateGrid(string path)
        {
            string resultpath = String.Empty;
            if (path.Contains(".xlsx"))
            {
                resultpath = path.Replace(".xlsx", "Results.txt");
            }
            else if (path.Contains(".xls"))
            {
                resultpath = path.Replace(".xls", "Results.txt");
            }
            string serverspath = String.Empty;
            if (path.Contains(".xlsx"))
            {
                serverspath = path.Replace(".xlsx", ".txt");
            }
            else if (path.Contains(".xls"))
            {
                serverspath = path.Replace(".xls", ".txt");
            }
            string[] values = txtKbValue.Text.Split(',');
            int count = values.Length;
            DataTable dtValidate = new DataTable();
            dtValidate.Columns.Add("ServerName", Type.GetType("System.String"));
            for (int i = 0; i < count; i++)
            {
                dtValidate.Columns.Add(values[i].ToString(), Type.GetType("System.String"));
            }
            dtValidate.Columns.Add("Rebooted", Type.GetType("System.String"));
            DataRow dr = null;
            using (FileStream fs = new FileStream(resultpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {

                StreamReader server = new StreamReader(serverspath);
                StreamReader result = new StreamReader(fs);
                int serversCount = 0;
                int totalCount = 0;
                string servers = String.Empty;
                string total = String.Empty;
                while ((servers = server.ReadLine()) != null)
                {
                    serversCount++;
                }
                while (serversCount != totalCount)
                {
                    while ((total = result.ReadLine()) != null)
                    {
                        totalCount++;
                    }
                }

                using (StreamReader sr = new StreamReader(new FileStream(resultpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                {
                    String line = sr.ReadToEnd();

                    string[] val = line.Split('\n');

                    foreach (string str in val)
                    {
                        if (str != "\r" && str != "")
                        {
                            dr = dtValidate.NewRow();
                            if (str.Contains("@"))
                            {
                                string[] final = str.Split('@');
                                for (int j = 0; j < final.Length; j++)
                                {
                                    if (j == 0 || j == final.Length - 1)
                                    {
                                        dr[j] = final[j];
                                    }
                                    else
                                    {
                                        if (final[j].Contains("&"))
                                        {
                                            string[] flag = final[j].Split('&');

                                            dr[j] = flag[1];
                                        }
                                    }
                                    //dr[1] = final[1];
                                    //dr[2] = final[2];
                                    //dr[3] = final[3];
                                }
                                //for (int i = 0; i < count; i++)
                                //{
                                //    //dtValidate.Columns.Add(values[i].ToString(), Type.GetType("System.String"));
                                //    if (values[i].ToString() == final[1].ToString())
                                //    {
                                //        dr[values[i].ToString()] = final[2];
                                //    }

                                //}

                                dtValidate.Rows.Add(dr);

                            }
                        }
                    }
                }

                gvValidateResult.DataSource = dtValidate;
                gvValidateResult.DataBind();
            }
        }



        #endregion      

        #region Validate Timer Tick event
        protected void TimerValidate_Tick(object sender, EventArgs e)
        {
            string strValidationstatus = string.Empty;
            string strPatchOption = string.Empty;
            string strUniqueID = string.Empty;


            try
            {
                if (cbMSNPatch.Checked == true)
                {
                    rvValidate.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                    rvValidate.ServerReport.ReportServerUrl = new Uri(@"http://tk5stosrv03/ReportServer");
                    rvValidate.ServerReport.ReportPath = "/PatchingLogs_PreProd/MSNPatchValidationReport";
                    rvValidate.ShowParameterPrompts = false;
                    rvValidate.ShowPrintButton = true;
                    rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", hdnCbMSNPatch.Value));

                   // rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UserName", "redmond\\stpatcha"));
                    rvValidate.ServerReport.Refresh();
                    strPatchOption = "MSNPatch";
                    strUniqueID = hdnCbMSNPatch.Value;

                }
                if(cbSimpleUpdate.Checked==true)
                {
                    rvValidate.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;                      
                    rvValidate.ServerReport.ReportServerUrl = new Uri(@"http://tk5stosrv03/ReportServer");
                    rvValidate.ServerReport.ReportPath = "/PatchingLogs_PreProd/SimpleUpdateValidationReport";
                    rvValidate.ShowParameterPrompts = false;
                    rvValidate.ShowPrintButton = true;
                    rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", hdnCbSimpleUpdate.Value));

                   // rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UserName", "redmond\\stpatcha"));
                    rvValidate.ServerReport.Refresh();
                    strPatchOption = "SimpleUpdate";
                    strUniqueID = hdnCbSimpleUpdate.Value;

                }
                if (cbODP.Checked == true)
                {
                    rvValidate.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                    rvValidate.ServerReport.ReportServerUrl = new Uri(@"http://tk5stosrv03/ReportServer");
                    rvValidate.ServerReport.ReportPath = "/PatchingLogs_PreProd/ODPValidationReport";
                    rvValidate.ShowParameterPrompts = false;
                    rvValidate.ShowPrintButton = true;
                    rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", hdnCbODP.Value));

                   // rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UserName", "redmond\\stpatcha"));
                    rvValidate.ServerReport.Refresh();
                    strPatchOption = "ODP";
                    strUniqueID = hdnCbODP.Value;
                }
                strValidationstatus = GetRunbookStatus(strUniqueID, strPatchOption,"Validate");

                if (strValidationstatus == "Completed")
                {
                    TimerValidate.Enabled = false;
                }
                 

            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
            finally
            {

            }

        }
        #endregion


        public string GetRunbookStatus(string strUniqueID, string Patchingoption,string type)
        {
            SqlConnection conn = null;
            string strconnectionString = string.Empty;
            SqlCommand command = null;
            string strRunbookStatus = string.Empty;
            try
            {
                string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();

                if (flag == "Tk5stosrv03")
                {
                    strconnectionString = ConfigurationManager.ConnectionStrings["TK5_ConnectionString"].ConnectionString;
                }
                if (flag == "Tk3OrchSQL01")
                {
                    strconnectionString = ConfigurationManager.ConnectionStrings["TK3_ConnectionString"].ConnectionString;

                }
                if (flag == "Tk5stosrv01")
                {
                    strconnectionString = ConfigurationManager.ConnectionStrings["TK5stosrv01_ConnectionString"].ConnectionString;
                }
                //strconnectionString = ConfigurationManager.ConnectionStrings["TK5_ConnectionString"].ConnectionString;

                conn = new SqlConnection(strconnectionString);
                conn.Open();
                command = new SqlCommand("usp_GetRunbookStatusNew", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UniqueID", strUniqueID);
                command.Parameters.AddWithValue("@PatchingOption", Patchingoption);
                command.Parameters.Add("@Status", SqlDbType.VarChar, 20);
                command.Parameters["@Status"].Direction = ParameterDirection.Output;
                command.Parameters.Add("@type",type);

                command.ExecuteNonQuery();

                strRunbookStatus = (string)command.Parameters["@Status"].Value;
                conn.Close();

            }
            catch (Exception ex)
            {
                WriteError(ex);
            }

            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();

                }
            }


            return strRunbookStatus;

        }


        public String GetValue(String name)
        {

            return name + "test";

        }

        protected void gvResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               // Stopped
                string Value = e.Row.Cells[1].Text;
                string sqlservice = e.Row.Cells[2].Text;
                string sqlreportingservice = e.Row.Cells[3].Text;



                if (Value.Contains("GB"))
                {
                    Value = Value.Replace("GB", "");
                    if (Convert.ToDouble(Value) < 3.00)
                    {
                        // Value = "Red";
                       // e.Row.BackColor = System.Drawing.Color.Red;
                        e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;

                    }
                }

                if (sqlservice=="Stopped")
                { 
                   
                        e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;

                   
                }
                if (sqlreportingservice == "Stopped")
                 {

                   e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;

                 }
                
            }
        }


        public System.Drawing.Color GetColor(string space)
        {
            string value=string.Empty;
            System.Drawing.Color colorval= System.Drawing.Color.Black;
            if (space.Contains("GB"))
            {
               value= space.Replace("GB", "");
            }
            if (Convert.ToInt32(value) < 10.00)
            {
                value = "Red";
                colorval = System.Drawing.Color.Red;
            }
     
            return colorval;
        }

      
        }


    }
