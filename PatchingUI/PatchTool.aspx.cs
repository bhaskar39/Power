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
using System.Xml;
using System.Diagnostics;
using System.IO;
using PatchingToolUI;
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
    public partial class PatchTool : System.Web.UI.Page
    {

        #region Variables
        private WindowsIdentity wiCurrentUser = WindowsIdentity.GetCurrent();
        string connectionString = null;
        SqlConnection conn = null;
        SqlCommand command = null;
        SqlDataAdapter dataAdapter = null;
        private Boolean IsPageRefresh = false;
        RunbookOperations rs = null;
        string uniqueGUID = null;
        string strServerlist = string.Empty;
        string output = null;
        string path = string.Empty;
        public string exereport = string.Empty;
        string[] textData = null;
        string filename = string.Empty;
        string[] headers = null;
        StreamWriter sw = null;

        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
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



                //txtDomainAcctPwd.Attributes.Add("value", ConfigurationManager.AppSettings["password"].ToString());
                //txtDomainAccName.Attributes.Add("value", ConfigurationManager.AppSettings["UserName"].ToString());

                // Page.ClientScript.RegisterStartupScript(this.GetType(), "load", "load();", true);
                if (!IsPostBack)
                {
                    //Code to maintain unique session id for each tab
                    Random r = new Random(DateTime.Now.Millisecond + DateTime.Now.Second * 1000 + DateTime.Now.Minute * 60000 + DateTime.Now.Minute * 3600000);
                    PageID.Value = r.Next().ToString();
                    ViewState[PageID.Value] = System.Guid.NewGuid().ToString();
                    Session[PageID.Value] = System.Guid.NewGuid().ToString();
                   // string strMonth = GetPatchMonth();
                    //LoadGroupNames(strMonth);

                    BrowserRefresh();
                }


                else
                {
                    BrowserRefresh();
                    trODPOption.Attributes.Add("style", "display:none");
                    trSimpleUpdateOption.Attributes.Add("style", "display:none");



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
            catch (Exception ex)
            {
                WriteError(ex);
            }

        }


   
        public void BrowserRefresh()
        {
            try
            {
                if (ViewState[PageID.Value].ToString() != Session[PageID.Value].ToString())
                {
                    IsPageRefresh = true;
                }
                Session[PageID.Value] = System.Guid.NewGuid().ToString();
                ViewState[PageID.Value] = Session[PageID.Value];
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }
        #endregion

        #region timerSrvcChk_Tick

        protected void timerSrvcChk_Tick(object sender, EventArgs e)
        {
            try
            {

                ServiceController srvorunprogram = new ServiceController("orunprogram", ConfigurationManager.AppSettings["OrchestratorServer"].ToString());
                ServiceController srvorunbook = new ServiceController("orunbook", ConfigurationManager.AppSettings["OrchestratorServer"].ToString());
                ServiceController srvoremoting = new ServiceController("oremoting", ConfigurationManager.AppSettings["OrchestratorServer"].ToString());
                ServiceController srvomonitor = new ServiceController("omonitor", ConfigurationManager.AppSettings["OrchestratorServer"].ToString());
                ServiceController srvomanagement = new ServiceController("omanagement", ConfigurationManager.AppSettings["OrchestratorServer"].ToString());

                if (srvorunprogram.Status.Equals(ServiceControllerStatus.Stopped) || srvorunbook.Status.Equals(ServiceControllerStatus.Stopped) ||
                   srvoremoting.Status.Equals(ServiceControllerStatus.Stopped) || srvomonitor.Status.Equals(ServiceControllerStatus.Stopped)
                    || srvomanagement.Status.Equals(ServiceControllerStatus.Stopped))
                {
                    lblServiceErrorMessage.Text = "Service is Restarting, Please try after sometime ";

                }

                else if (srvorunprogram.Status.Equals(ServiceControllerStatus.Running))
                {
                    lblServiceErrorMessage.Text = "";

                }
            }

            catch (Exception ex)
            {
                WriteError(ex);
            }
        }

        #endregion

        #region Prep Tab

        #region Mitigation
        protected void btnMitigation_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                connectionString = ConfigurationManager.ConnectionStrings["DB_ConnectionString"].ConnectionString;

                conn = new SqlConnection(connectionString);
                conn.Open();
                command = new SqlCommand("[usp_GetMitigationStatusNew]", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UniqueID", hdnUniqueAccess.Value);
                count = (int)command.ExecuteScalar();

                conn.Close();
                if (count > 0)
                {
                    divAddAdmin.Visible = true;
                    rvPrep.Visible = false;
                    btnMitigation.Visible = false;
                    ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_2','content_2')</script>");
                }
                else
                {
                    CallMitigate();
                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }


        }
        static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("ZeroCool");

        public string EncryptMethod(string originalString)
        {
            try
            {
                if (String.IsNullOrEmpty(originalString))
                {
                    // throw new ArgumentNullException
                    //("The string which needs to be encrypted can not be null.");
                    return "";
                }
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                    cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
                StreamWriter writer = new StreamWriter(cryptoStream);
                writer.Write(originalString);
                writer.Flush();
                cryptoStream.FlushFinalBlock();
                writer.Flush();
                return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
            catch (Exception ex)
            {
                WriteError(ex);
                return "";
            }


        }
        public void CallMitigate()
        {
            try
            {
                hdnCheck.Value = "Mitigate";
                rvPrep.Visible = true;
                ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_2','content_2')</script>");

                // Guid runbookid = new Guid();
                uniqueGUID = Guid.NewGuid().ToString();
                rs = new RunbookOperations();
                string strrunbookPath = string.Empty;
                //if (ddlPrepScenario.SelectedValue == "1" || ddlPrepScenario.SelectedValue == "3")
                //{
                    strrunbookPath = ConfigurationManager.AppSettings["MitigatePath"].ToString();
                    path = ConfigurationManager.AppSettings["MitigateReportPath"].ToString();
                    rvPrep.Style.Add("Width", "720px");
                /*}
                else
                {
                    strrunbookPath = ConfigurationManager.AppSettings["MitigatePath_HLB"].ToString();
                    path = ConfigurationManager.AppSettings["MitigateReportPath_HLB"].ToString();
                    rvPrep.Style.Add("Width", "950px");
                }*/
                //string strrunbookPath = ConfigurationManager.AppSettings["MitigatePath"].ToString();
                RunbookParams objParams = new RunbookParams();
                objParams.RunbookPath = strrunbookPath;
                objParams.LogsPath = txtLogPath.Text;
                objParams.AdminAccountName = txtAdmin.Text;
                objParams.AdminAccountPwd = EncryptMethod(txtPassword.Text);
                objParams.uniqueGUID = hdnUniqueAccess.Value;
                objParams.InputFilename = hdnPrepInputFilename.Value;
             

                if (hdnPrepFileName.Value.Contains(".xlsx") || hdnPrepFileName.Value.Contains(".xls"))
                {
                    objParams.CheckExcelORText = "Excel";
                    //try
                    //{
                    //    output = rs.StartValidateAddAdminRunbook(strrunbookPath, hdnPrepFileName.Value, txtLogPath.Text, txtAdmin.Text, EncryptMethod(txtPassword.Text), hdnUniqueAccess.Value, hdnPrepInputFilename.Value, "4", "Excel");
                    //}
                    //catch (Exception ex)
                    //{
                    //    if (ex.Message == "Error starting runbook.")
                    //    {

                    //        output = rs.StartValidateAddAdminRunbook(strrunbookPath, hdnPrepFileName.Value, txtLogPath.Text, txtAdmin.Text, EncryptMethod(txtPassword.Text), hdnUniqueAccess.Value, hdnPrepInputFilename.Value, "4", "Excel");
                    //    }
                    //    else
                    //    {
                    //        WriteError(ex);
                    //    }

                    //}
                }
                else if (hdnPrepFileName.Value == "Group")
                {
                    objParams.CheckExcelORText = "Group";
                    objParams.GroupName = ddlPrepNames.SelectedValue.ToString();
                }
                else
                {
                    objParams.CheckExcelORText = "Text";
                    //try
                    //{
                    //    output = rs.StartValidateAddAdminRunbook(strrunbookPath, hdnPrepFileName.Value, txtLogPath.Text, txtAdmin.Text, EncryptMethod(txtPassword.Text), hdnUniqueAccess.Value, hdnPrepInputFilename.Value, "4", "Text");
                    //}
                    //catch (Exception ex)
                    //{
                    //    if (ex.Message == "Error starting runbook.")
                    //    {

                    //        output = rs.StartValidateAddAdminRunbook(strrunbookPath, hdnPrepFileName.Value, txtLogPath.Text, txtAdmin.Text, EncryptMethod(txtPassword.Text), hdnUniqueAccess.Value, hdnPrepInputFilename.Value, "4", "Text");
                    //    }
                    //    else
                    //    {
                    //        WriteError(ex);
                    //    }

                    //}
                }

                try
                {
                    output = rs.StartMitigateRunbook(objParams);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Error starting runbook.")
                    {

                        output = rs.StartMitigateRunbook(objParams);
                    }
                    else
                    {
                        WriteError(ex);
                    }

                }
                //path = ConfigurationManager.AppSettings["MitigateReportPath"].ToString();
                rvPrep.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
                string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                rvPrep.ServerReport.ReportServerUrl = new Uri(@url);
                rvPrep.ServerReport.ReportPath = ReportViewerPath + path;
                exereport = rvPrep.ServerReport.ReportPath;
                rvPrep.ShowParameterPrompts = true;
                rvPrep.ShowPrintButton = true;
                rvPrep.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueId", hdnUniqueAccess.Value));

                rvPrep.ServerReport.Refresh();
                // Modification for Auto refresh in Mitigation : Start : 07/05/2013
                timerPrep.Enabled = true;
                // Modification for Auto refresh in Mitigation : End : 07/05/2013

                // rvPrep.Style.Add("Width", "500px");
                btnMitigation.Visible = false;
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }
        #endregion

        #region CheckAcess
        /// <summary>
        /// Click event to call a Check the access of servers in the serverlist path
        /// </summary>
        /// <author></author>
        /// <CreatedDate></CreatedDate>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            try
            {
                hdnCheck.Value = "Check";
                string strRunbookPath = string.Empty;
                rvPrep.Visible = true;
                divAddAdmin.Visible = false;
                pnlAddAdmin.Visible = false;
                gvAddAdmin.Visible = false;
                DataTable dt = new DataTable();
                RunbookParams objParams = new RunbookParams();
                ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_2','content_2')</script>");

                //if (!IsPageRefresh)
                //{

                    btnCheck.Enabled = false;

                    string strDestPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString();
                    string strName = "";
                    string strFileName = "";
                    string strheader = "";
                    uniqueGUID = Guid.NewGuid().ToString();
                    hdnUniqueAccess.Value = uniqueGUID;
                    rs = new RunbookOperations();
                    //if (ddlPrepScenario.SelectedValue == "1" || ddlPrepScenario.SelectedValue.ToString() == "3")
                        strRunbookPath = ConfigurationManager.AppSettings["CheckAccessPath"].ToString();
                    //else
                        //strRunbookPath = ConfigurationManager.AppSettings["CheckAccessPath_HLB"].ToString();


                    objParams.RunbookPath = strRunbookPath;                 
                    objParams.LogsPath = txtLogPath.Text;
                    objParams.uniqueGUID = uniqueGUID;
                    objParams.InputFilename = hdnPrepInputFilename.Value;                 
                    if (ddlPrepNames.SelectedValue != "0")
                    {
                        objParams.GroupName = ddlPrepNames.SelectedItem.Text;
                        objParams.CheckExcelORText = "Group";
                        hdnPrepFileName.Value = "Group";

                    }
                    else if (fupPreExceute.HasFile)
                    {

                        if (fupPreExceute.FileName.Contains(".xlsx"))
                        {
                            strName = fupPreExceute.FileName.Replace(".xlsx", uniqueGUID);

                            strFileName = strName + ".xlsx";
                            strheader = fupPreExceute.FileName.Replace(".xlsx", "");

                        }
                        else if (fupPreExceute.FileName.Contains(".xls"))
                        {
                            strName = fupPreExceute.FileName.Replace(".xls", uniqueGUID);
                            strFileName = strName + ".xls";
                            strheader = fupPreExceute.FileName.Replace(".xls", "");
                        }
                        else
                        {
                        }

                        strServerlist = strDestPath + strFileName;
                        fupPreExceute.SaveAs(strServerlist);
                        hdnPrepFileName.Value = strServerlist;
                        hdnPrepInputFilename.Value = strheader;
                       
                        try
                        {
                            
                            DataSet dsData = new DataSet();                           
                           dsData= ReadExcel(strServerlist);
                            if (dsData.Tables[0].Columns[0].ToString() == "ServerName" && dsData.Tables[0].Columns[1].ToString() == "Priority")
                            {

                                objParams.CheckExcelORText = "Excel";
                                objParams.ServerName = strServerlist;

                            }
                            else
                            {
                                btnMitigation.Visible = false;
                                rvPrep.Visible = false;
                                Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('Please Select Valid Excel');</script>");

                            }
                        }
                        catch
                        {
                            Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message2", "<script type='text/jscript'>alert('ExcelFile Sheet Name sholud be Sheet1');</script>");
                        }
                    }
                    else
                    {

                        string strTextPath =ConfigurationManager.AppSettings["PrepFilesPath"].ToString() + uniqueGUID + ".txt";
                        CreateTextFile(txtServerNames.Text, strTextPath);                        
                        objParams.ServerName = strTextPath;
                        objParams.CheckExcelORText = "Text";
                        hdnPrepFileName.Value = strTextPath;
                    }


                    try
                    {
                        output = rs.StartPrepRunbook(objParams);

                    }
                    catch (Exception ex)
                    {

                        if (ex.Message == "Error starting runbook.")
                        {

                            output = rs.StartPrepRunbook(objParams);
                        }
                        else
                        {
                            WriteError(ex);
                        }

                    }
                    System.Threading.Thread.Sleep(20000);
                    callPrepReport();
                //}

            }
            catch (Exception ex)
            {
                WriteError(ex);
            }

        }


        public DataSet ReadExcel(string strFilePath)
        {
            DataSet dsData = new DataSet();
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFilePath + ";Extended Properties=Excel 12.0;";
            try
            {
                string query = String.Format("select * from [{0}]", "Sheet1$");
                OleDbDataAdapter daData = new OleDbDataAdapter(query, connectionString);
                //DataSet dsData = new DataSet();
                daData.Fill(dsData);
                
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
            }
            return dsData;
        }
        public void CreateTextFile(string strValues, string strPath)
        {
            string[] values = strValues.Split(',');
            if (!File.Exists(strPath))
            {
                using (StreamWriter sr = File.CreateText(strPath))
                {
                    sr.Close();
                }
                StreamWriter sw = null;

                sw = new StreamWriter(strPath, false);

                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i].ToString() != "")
                    {
                        sw.Write(values[i].ToString());
                        sw.WriteLine();
                    }

                }
                sw.Close();
            }

        }
        public void callPrepReport()
        {
            try
            {
                DataTable dt = new DataTable();
                // btnMitigation.Visible = false;
                //if (ddlPrepScenario.SelectedValue == "1" || ddlPrepScenario.SelectedValue=="3")
                //{
                    path = ConfigurationManager.AppSettings["PrepReportPath"].ToString();
                    rvPrep.Style.Add("Width", "720px");
                /*}
                else
                {
                    path = ConfigurationManager.AppSettings["PrepReportPath_HLB"].ToString();
                    rvPrep.Style.Add("Width", "950px");
                }*/
                rvPrep.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
                string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                rvPrep.ServerReport.ReportServerUrl = new Uri(@url);
                rvPrep.ServerReport.ReportPath = ReportViewerPath + path;
                exereport = rvPrep.ServerReport.ReportPath;
                rvPrep.ShowParameterPrompts = true;
                rvPrep.ShowPrintButton = true;
                rvPrep.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", hdnUniqueAccess.Value));
                rvPrep.ServerReport.Refresh();
                //rvPrep.Style.Add("Width", "950px");
                //  System.Threading.Thread.Sleep(10000);

                timerPrep.Enabled = true;
                rvPrep.Visible = true;
                btnMitigation.Visible = true;
                try
                {
                    dt = GetPrepStatus(hdnUniqueAccess.Value);
                    //while (dt.Rows[0]["Status"].ToString() != "Completed")
                    if (dt.Rows[0]["Status"].ToString() != "Completed")
                    {

                        timerPrep.Enabled = true;

                    }
                    if (dt.Rows[0]["Status"].ToString() == "Completed")
                    {
                        //timerPrep.Enabled = true;
                        if (Convert.ToInt32(dt.Rows[0]["count"]) > 0)
                        {
                            btnMitigation.Visible = true;
                        }
                        else
                        {
                            // btnMitigation.Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteError(ex);
                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }

        }

        #endregion

        #region PreSmokeTest
        protected void btnPreSmokeTest_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtSmokeResult = null;
                rvPrep.Visible = false;
                btnMitigation.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_2','content_2')</script>");

                //if (!IsPageRefresh)
                //{
                    dtSmokeResult = new DataTable();
                    string strDestPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString();
                    uniqueGUID = Guid.NewGuid().ToString();
                    hdnUniqueAccess.Value = uniqueGUID;
                    rs = new RunbookOperations();
                    string strRunbookPath = ConfigurationManager.AppSettings["SmokeTestPath"].ToString();
                    string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
                    
                    if (ddlPrepNames.SelectedValue != "0")
                    {
                        output = rs.StartSmokeTestRunbook(strRunbookPath, "", txtLogPath.Text, uniqueGUID, "No", "Group", ddlPrepNames.SelectedItem.Text);

                        
                    }

                    else if (fupPreExceute.HasFile)
                    {

                        string strName = "";
                        string strFileName = "";
                        if (fupPreExceute.FileName.Contains(".xlsx"))
                        {
                            strName = fupPreExceute.FileName.Replace(".xlsx", uniqueGUID);
                            strFileName = strName + ".xlsx";

                        }
                        else if (fupPreExceute.FileName.Contains(".xls"))
                        {
                            strName = fupPreExceute.FileName.Replace(".xls", uniqueGUID);
                            strFileName = strName + ".xls";
                        }
                        else
                        {
                        }
                        strServerlist = strDestPath + strFileName;
                        fupPreExceute.SaveAs(strServerlist);
                      
                        try
                        {
                           
                            DataSet dsData = new DataSet();
                          
                            dsData=ReadExcel(strServerlist);
                            
                            if (dsData.Tables[0].Columns[0].ToString() == "ServerName" && dsData.Tables[0].Columns[1].ToString() == "Priority")
                            {
                                    output = rs.StartSmokeTestRunbook(strRunbookPath, strServerlist, txtLogPath.Text, uniqueGUID, "No", "Excel","");
                                 
                            }
                            else
                            {
                                pnlAddAdmin.Visible = false;
                                gvAddAdmin.Visible = false;
                                Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('Please Select Valid Excel');</script>");
                            }
                        }
                        catch
                        {

                            Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('ExcelFile Sheet Name sholud be Sheet1');</script>");


                        }
                    }
                    else
                    {
                        
                      
                        string strTextPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString() + uniqueGUID + ".txt";
                        CreateTextFile(txtServerNames.Text, strTextPath);                     

                        output = rs.StartSmokeTestRunbook(strRunbookPath, strTextPath, txtLogPath.Text, uniqueGUID, "No", "Text","");

                       
                    }
                    System.Threading.Thread.Sleep(25000);
                    string textfilepath = ConfigurationManager.AppSettings["SmokeTestLogPath"].ToString();
                    string[] filePaths = Directory.GetFiles(textfilepath, "SmokeTestResult_*_" + uniqueGUID + ".txt");
                    filename = textfilepath + "SmokeTestResult_Consolidated_" + uniqueGUID + ".txt";

                    //Read all the text files which has same unique id and prepares a datatable
                    dtSmokeResult = CreateDataTable(filePaths);

                    //Export smoke test result of all teh servers into a text file with unique id.
                    CreateSmokeTestLog(filename, dtSmokeResult);

                    //Set the DataSource of DataGridView to the DataTable
                    gvAddAdmin.DataSource = dtSmokeResult;

                    gvAddAdmin.DataBind();
                    if (dtSmokeResult.Rows.Count > 0)
                    {
                        pnlAddAdmin.Visible = true;
                        gvAddAdmin.Visible = true;

                    }
                //}
            }
            catch (Exception ex)
            {

                WriteError(ex);

            }
        }
        #endregion

        #region Unistall Falsh
        /// <summary>
        /// Click event to call a Uninstall Falshof servers in the serverlist path
        /// </summary>
        /// <author></author>
        /// <CreatedDate></CreatedDate>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUninstall_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_2','content_2')</script>");

            //if (!IsPageRefresh)
            //{
                try
                {
                   
                    rs = new RunbookOperations();
                    string strDestPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString();
                    uniqueGUID = Guid.NewGuid().ToString();
                    hdnExecute.Value = uniqueGUID;
                    string strRunbookPath = string.Empty;
                   
                    strRunbookPath = ConfigurationManager.AppSettings["FlashPath"].ToString();

                    string strheader = "";
                    #region Commented
                   
                    #endregion

                    if (ddlPrepNames.SelectedValue != "0")
                    {
                        output = rs.StartFlashRunbook(strRunbookPath,"", uniqueGUID, hdnFileName.Value, "Group",ddlPrepNames.SelectedItem.Text);
                    }
                    else if (fupPreExceute.HasFile)
                    {

                        string strName = "";
                        string strFileName = "";

                        if (fupPreExceute.FileName.Contains(".xlsx"))
                        {
                            strName = fupPreExceute.FileName.Replace(".xlsx", uniqueGUID);
                            strFileName = strName + ".xlsx";

                        }
                        else if (fupPreExceute.FileName.Contains(".xls"))
                        {
                            strName = fupPreExceute.FileName.Replace(".xls", uniqueGUID);
                            strFileName = strName + ".xls";
                        }
                        else
                        {
                        }

                        strServerlist = strDestPath + strFileName;
                        fupPreExceute.SaveAs(strServerlist);
                      
                        try
                        {
                           
                            DataSet dsData = new DataSet();
                            dsData=ReadExcel(strServerlist);
                           
                            if (dsData.Tables[0].Columns[0].ToString() == "ServerName" && dsData.Tables[0].Columns[1].ToString() == "Priority")
                            {
                                hdnFileName.Value = fupPreExceute.FileName;

                                if (fupPreExceute.FileName.Contains(".xlsx"))
                                {
                                    strheader = fupPreExceute.FileName.Replace(".xlsx", "");
                                }
                                else if (fupPreExceute.FileName.Contains(".xls"))
                                {
                                    strheader = fupPreExceute.FileName.Replace(".xls", "");
                                }

                                output = rs.StartFlashRunbook(strRunbookPath, strServerlist, uniqueGUID, hdnFileName.Value, "Excel","");


                            }
                            else
                            {
                                //rvReports.Visible = false;
                                Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('Please Select Valid Excel');</script>");
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("'Sheet1$' is not a valid name"))
                            {
                                Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('ExcelFile Sheet Name should be Sheet1');</script>");

                            }
                            else
                            {
                                WriteError(ex);
                            }
                        }
                       
                    }

                    else
                    {
                       
                        string strTextPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString() + uniqueGUID + ".txt";
                        CreateTextFile(txtServerNames.Text, strTextPath);     
                        output = rs.StartFlashRunbook(strRunbookPath, strTextPath, uniqueGUID, hdnFileName.Value, "Text","");
                    }



                }


                catch (Exception ex)
                {
                    WriteError(ex);

                }

                finally
                {

                }

           // }
        }
        #endregion

        #region AddAdmin
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
            try
            {
                divAddAdmin.Visible = false;
                rvPrep.Visible = true;
                // btnMitigation.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_2','content_2')</script>");
                hdnServerNames.Value = "";
                CallMitigate();
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }


        }
        #endregion
        #endregion

        #region PrepTimer Tick event
        protected void timerPrep_Tick(object sender, EventArgs e)
        {
            timerPrep.Interval = 10000;
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_2','content_2')</script>");
            string strStatus = string.Empty;
            string strPatchOption = string.Empty;
            DataTable dt = new DataTable();

            try
            {
                rvPrep.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
                string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                rvPrep.ServerReport.ReportServerUrl = new Uri(@url);
                //lblRVExecute.Text = "Report Name: " + hdnFileName.Value;
                if (hdnCheck.Value == "Mitigate")
                {
                    //if (ddlPrepScenario.SelectedValue == "1" || ddlPrepScenario.SelectedValue=="3")
                    //{
                        path = ConfigurationManager.AppSettings["MitigateReportPath"].ToString();
                        rvPrep.Style.Add("Width", "720px");
                        rvPrep.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueId", hdnUniqueAccess.Value));
                    /*}
                    else
                    {
                        path = ConfigurationManager.AppSettings["MitigateReportPath_HLB"].ToString();
                        rvPrep.Style.Add("Width", "950px");
                        rvPrep.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueId", hdnUniqueAccess.Value));
                    }*/
                    // path = ConfigurationManager.AppSettings["MitigateReportPath"].ToString();
                    rvPrep.ShowParameterPrompts = true;

                }
                if (hdnCheck.Value == "Check")
                {
                    //if (ddlPrepScenario.SelectedValue == "1" || ddlPrepScenario.SelectedValue=="3")
                    //{
                        path = ConfigurationManager.AppSettings["PrepReportPath"].ToString();
                        rvPrep.Style.Add("Width", "720px");
                        rvPrep.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", hdnUniqueAccess.Value));
                    /*}
                    else
                    {
                        path = ConfigurationManager.AppSettings["PrepReportPath_HLB"].ToString();
                        rvPrep.Style.Add("Width", "950px");
                        rvPrep.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", hdnUniqueAccess.Value));
                    }*/
                    // path = ConfigurationManager.AppSettings["PrepReportPath"].ToString();
                    rvPrep.ShowParameterPrompts = true;

                }

                rvPrep.ServerReport.ReportPath = ReportViewerPath + path;
                exereport = rvPrep.ServerReport.ReportPath;

                rvPrep.ShowPrintButton = true;



                rvPrep.ServerReport.Refresh();



                dt = GetPrepStatus(hdnUniqueAccess.Value);
                //while (dt.Rows[0]["Status"].ToString() != "Completed")
                if (dt.Rows[0]["Status"].ToString() != "Completed")
                {
                    dt = GetPrepStatus(hdnUniqueAccess.Value);
                    timerPrep.Enabled = true;

                }
                if (dt.Rows[0]["Status"].ToString() == "Completed")
                {
                    timerPrep.Enabled = false;
                    if (Convert.ToInt32(dt.Rows[0]["count"]) > 0)
                    {
                        btnMitigation.Visible = true;
                    }
                    else
                    {
                        // btnMitigation.Visible = false;
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



        }
        #endregion


        public DataTable GetPrepStatus(string strUniqueID)
        {
            SqlConnection conn = null;
            string strconnectionString = string.Empty;
            SqlCommand command = null;
            string strRunbookStatus = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                //strconnectionString = ConfigurationManager.ConnectionStrings["TK5_ConnectionString"].ConnectionString;

                strconnectionString = ConfigurationManager.ConnectionStrings["DB_ConnectionString"].ConnectionString;

                conn = new SqlConnection(strconnectionString);
                conn.Open();
                command = new SqlCommand("usp_GetPrepStatus_New", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UniqueID", strUniqueID);
                command.Parameters.Add("@Status", SqlDbType.VarChar, 20);
                command.Parameters["@Status"].Direction = ParameterDirection.Output;
                command.Parameters.Add("@count", SqlDbType.VarChar, 20);
                command.Parameters["@count"].Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();

                strRunbookStatus = (string)command.Parameters["@Status"].Value;
                dt.Columns.Add("Status", Type.GetType("System.String"));
                dt.Columns.Add("Count", Type.GetType("System.String"));
                DataRow dr = dt.NewRow();
                dr[0] = (string)command.Parameters["@Status"].Value;
                dr[1] = (string)command.Parameters["@count"].Value;

                dt.Rows.Add(dr);
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


            return dt;

        }

        #endregion

        #region Execute Tab

        #region gvClusterExecute_OnRowDataBound
        protected void gvClusterExecute_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    DropDownList ddlPause = (DropDownList)e.Row.FindControl("ddlPause");
                    Label lblPauseDuringExec = (Label)e.Row.FindControl("lblPauseDuringExec");
                    if (lblPauseDuringExec.Text == "")
                        ddlPause.Items.FindByValue("0").Selected = true;
                    else
                        ddlPause.Items.FindByValue(lblPauseDuringExec.Text).Selected = true;

                    // ddlPause.Items.FindByValue(lblPauseDuringExec.Text).Selected = true;
                    //foreach (GridViewRow row in gvClusterExecute.Rows)
                    //{
                    Label lblServerName = (Label)e.Row.FindControl("lblServerName");
                    // DropDownList ddlPause = (DropDownList)row.FindControl("ddlPause");
                    CheckBox ChkPauseNode = (CheckBox)e.Row.FindControl("ChkPauseNode");
                    CheckBox ChkFailBack = (CheckBox)e.Row.FindControl("ChkFailBack");
                    CheckBox chkForceStandalone = (CheckBox)e.Row.FindControl("chkForceStandalone");

                    Label lblNodeType = (Label)e.Row.FindControl("lblNodeType");

                    var ddlFailOverNode = (DropDownList)e.Row.FindControl("ddlFailOverNode");
                    DataTable dtExecutionOutput = DataAccessLayer.GetSUExecutionOuput(hdnClusterExecute.Value, lblServerName.Text);
                    GridView gvPatchOutput = (GridView)e.Row.FindControl("gvPatchOutput");
                    gvPatchOutput.DataSource = dtExecutionOutput;
                    gvPatchOutput.DataBind();
                    Label lblClusterType = (Label)e.Row.FindControl("lblClusterType");
                    if (lblClusterType.Text.Contains("MSCS") && hdnClusterFlag.Value == "0")
                    {

                        // if ((lblClusterType.Text.Contains("MSCS - 2")) || (lblClusterType.Text.Contains("Standalone")) ||lblNodeType.Text.ToLower()=="passive")   
                        if ((lblClusterType.Text.Contains("Standalone")) || lblNodeType.Text.ToLower() == "passive")
                            ddlFailOverNode.Enabled = false;
                        else
                            ddlFailOverNode.Enabled = true;
                        ChkPauseNode.Enabled = ChkFailBack.Enabled = ddlPause.Enabled = chkForceStandalone.Enabled = true;
                    }
                    else
                        ChkPauseNode.Enabled = ChkFailBack.Enabled = ddlPause.Enabled = ddlFailOverNode.Enabled = chkForceStandalone.Enabled = false;

                    //  Label lblNodeClusterName = (Label)e.Row.FindControl("lblNodeClusterName");

                    Label lblBackUpNode = (Label)e.Row.FindControl("lblBackUpNode");

                    DataSet dsBackUpNodelist = DataAccessLayer.GetBackUpNodesList(hdnClusterExecute.Value, lblServerName.Text);
                    ddlFailOverNode.Items.Clear();
                    //Me.DropDownList1.DataSource = txtValues.Text.Split(",")
                    if (dsBackUpNodelist.Tables[0].Rows.Count > 0)
                    {
                        string strBackupNodes = dsBackUpNodelist.Tables[0].Rows[0][0].ToString();
                        if (strBackupNodes.Contains(","))

                            ddlFailOverNode.DataSource = dsBackUpNodelist.Tables[0].Rows[0][0].ToString().Split(',');
                        else
                        // ddlFailOverNode.DataSource = dsBackUpNodelist.Tables[0].Rows[0][0].ToString();
                        {
                            ddlFailOverNode.DataSource = dsBackUpNodelist.Tables[0];
                            ddlFailOverNode.DataTextField = "NodeName";
                            ddlFailOverNode.DataValueField = "NodeName";
                        }

                        ddlFailOverNode.DataBind();
                        ddlFailOverNode.Items.Insert(0, new ListItem("--Select--", "0"));
                        if (lblBackUpNode.Text != null && lblBackUpNode.Text != "")
                            ddlFailOverNode.Items.FindByValue(lblBackUpNode.Text).Selected = true;
                    }

                   
                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }
        #endregion

        #region ChkForceStandalone_Clicked
        public void ChkForceStandalone_Clicked(Object sender, EventArgs e)
        {
            try
            {
                CheckBox chkForceStandalone = (CheckBox)sender;
                GridViewRow row = (GridViewRow)chkForceStandalone.NamingContainer;
                string Nodename = string.Empty;
                string PauseFlag = "0";
                string PauseValue = string.Empty;
                string FailbackFlag = "0";
                int result = 0;
                string ForceStandaloneFlag = "0";
                if (row != null)
                {

                    bool Value = ((CheckBox)(row.FindControl("chkForceStandalone"))).Checked;

                    Label lblServerName = (Label)row.FindControl("lblServerName");
                    Nodename = lblServerName.Text;
                    DropDownList ddlPause = (DropDownList)row.FindControl("ddlPause");
                    DropDownList ddlFailOverNode = (DropDownList)row.FindControl("ddlFailOverNode");
                    //PauseValue = ddlPause.SelectedItem.Text;
                    PauseValue = ddlPause.SelectedValue;
                    CheckBox ChkPauseNode = (CheckBox)row.FindControl("ChkPauseNode");
                    // PauseFlag = ChkPauseNode.Checked.ToString();
                    CheckBox ChkFailBack = (CheckBox)row.FindControl("ChkFailBack");
                    // FailbackFlag = ChkFailBack.Checked.ToString();
                    // var isChecked = $(checked).attr('checked') ? true : false;
                    PauseFlag = ChkPauseNode.Checked ? "1" : "0";
                    FailbackFlag = ChkFailBack.Checked ? "1" : "0";
                    // CheckBox chkForceStandalone = (CheckBox)row.FindControl("chkForceStandalone");
                    ForceStandaloneFlag = chkForceStandalone.Checked ? "1" : "0";
                    //ChkPauseNode.Enabled = ChkFailBack.Enabled = ddlPause.Enabled = chkForceStandalone.Enabled = false;

                    //result = DataAccessLayer.UpdateNodeInfo(hdnClusterExecute.Value, Nodename, PauseFlag, PauseValue, FailbackFlag, ForceStandaloneFlag,ddlFailOverNode.SelectedValue);
                    Label lblNodeType = (Label)row.FindControl("lblNodeType");
                    Label lblClusterType = (Label)row.FindControl("lblClusterType");

                    if (((lblClusterType.Text.Contains("MSCS - 2")) && lblNodeType.Text.ToLower() == "active"))
                    {
                        DataSet dsBackUpNodelist = DataAccessLayer.GetBackUpNodesList(hdnClusterExecute.Value, lblServerName.Text);
                        hdnBackUpNode.Value = dsBackUpNodelist.Tables[0].Rows[0]["NodeName"].ToString();
                        result = DataAccessLayer.UpdateNodeInfo(hdnClusterExecute.Value, Nodename, PauseFlag, PauseValue, FailbackFlag, ForceStandaloneFlag, hdnBackUpNode.Value);

                    }
                    else if (lblClusterType.Text.Contains("Standalone"))
                    {
                    }
                    else
                    {

                        result = DataAccessLayer.UpdateNodeInfo(hdnClusterExecute.Value, Nodename, PauseFlag, PauseValue, FailbackFlag, ForceStandaloneFlag, ddlFailOverNode.SelectedValue);
                    }


                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }

        }
        #endregion

        #region ChkFailBack_Clicked
        /// <summary>
        /// failback to original state” checkbox checked for all nodes of the cluster if any of the nodes under the cluster is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void ChkFailBack_Clicked(Object sender, EventArgs e)
        {

            try
            {
                CheckBox chkFailBack = (CheckBox)sender;
                GridViewRow row = (GridViewRow)chkFailBack.NamingContainer;
                if (row != null)
                {

                    bool Value = ((CheckBox)(row.FindControl("chkFailBack"))).Checked;
                    if (Value == true)
                    {
                        Label lblClusterName = (Label)row.FindControl("lblClusterName");

                        foreach (GridViewRow gvr in gvClusterExecute.Rows)
                        {
                            Label ClusterName = (Label)gvr.FindControl("lblClusterName");
                            CheckBox FailBack = (CheckBox)gvr.FindControl("ChkFailBack");
                            if (lblClusterName.Text == ClusterName.Text)
                            {
                                FailBack.Checked = true;
                            }
                            //else
                            //{
                            //    FailBack.Checked = false;
                            //}
                        }
                    }
                    else
                    {
                        Label lblClusterName = (Label)row.FindControl("lblClusterName");

                        foreach (GridViewRow gvr in gvClusterExecute.Rows)
                        {
                            Label ClusterName = (Label)gvr.FindControl("lblClusterName");
                            CheckBox FailBack = (CheckBox)gvr.FindControl("ChkFailBack");
                            if (lblClusterName.Text == ClusterName.Text)
                            {
                                FailBack.Checked = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                WriteError(ex);

            }
        }
        #endregion

        #region ddlPause_SelectedIndexChanged
        protected void ddlPause_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            try
            {

                DropDownList ddlPause = (DropDownList)sender;
                GridViewRow row = (GridViewRow)ddlPause.NamingContainer;
                string Nodename = string.Empty;
                string ChkFlagValue = string.Empty;
                if (row != null)
                {

                    bool Value = ((CheckBox)(row.FindControl("ChkValue"))).Checked;
                    Label lblServerName = (Label)row.FindControl("lblServerName");
                    Nodename = lblServerName.Text;
                    CheckBox ChkValue = (CheckBox)row.FindControl("ChkValue");
                    ChkFlagValue = ChkValue.Checked ? "1" : "0";
                    string strPaused = ddlPause.SelectedValue.ToString();
                    int result = DataAccessLayer.UpdateHLBCheckValue(hdnClusterExecute.Value, Nodename, ChkFlagValue, strPaused);



                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }

        }
        #endregion

        #region btnDisplayRefresh_Click

        public void GetDisplayRefershData()
        {
            try
            {
                pnlCounts.Visible = false; pnlDisplay.Visible = true;

                if (ddlScenario.SelectedValue.ToString() == "1" || ddlScenario.SelectedValue=="3")
                {

                    int MSCSCount = DataAccessLayer.GetExecutionStatus(hdnClusterExecute.Value);
                    lblDisplayCount.Text = Convert.ToString(MSCSCount);

                    lblDisplayEnv.Visible = false;
                    ddlDisplayEnv.Visible = false;
                    lblClusType.Visible = true;
                    ddlClustertype.Visible = true;
                    gvHLBExecute.Visible = false;
                    gvClusterExecute.Visible = true;
                    DataTable dtExecutionDetails = DataAccessLayer.GetSUExecutionSummary(hdnClusterExecute.Value, ddlClustertype.SelectedValue.ToString());
                    ViewState["ListingInfo"] = dtExecutionDetails;
                    gvClusterExecute.DataSource = dtExecutionDetails;
                    gvClusterExecute.DataBind();
                    if (dtExecutionDetails.Rows.Count > 0)
                    {
                        btnClusterExecute.Visible = true;
                    }
                    else
                    {
                        btnClusterExecute.Visible = false;
                    }
                }
                else if (ddlScenario.SelectedValue.ToString() == "2")
                {
                    int Count = DataAccessLayer.GetHLBExecutionStatus(hdnClusterExecute.Value);
                    lblDisplayCount.Text = Convert.ToString(Count);
                    gvHLBExecute.Visible = true;
                    gvClusterExecute.Visible = false;
                    lblDisplayEnv.Visible = true;
                    ddlDisplayEnv.Visible = true;
                    ddlClustertype.Visible = false;
                    lblClusType.Visible = false;

                    DataTable dtExecutionDetails = DataAccessLayer.GetHLBExecutionSummary(hdnClusterExecute.Value, ddlDisplayEnv.SelectedValue);
                    ViewState["ListingInfo"] = dtExecutionDetails;
                    //if (ViewState["ListingInfo"] != null)
                    //    gvHLBExecute.DataSource = ViewState["ListingInfo"];
                    //else
                    gvHLBExecute.DataSource = dtExecutionDetails;
                    gvHLBExecute.DataBind();
                    if (dtExecutionDetails.Rows.Count > 0)
                    {
                        btnClusterExecute.Visible = true;
                    }
                    else
                    {
                        btnClusterExecute.Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }
        protected void btnDisplayRefresh_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
            // ExecuteTimer.Enabled = false;

            try
            {
                GetDisplayRefershData();
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

        #region ddlDisplayType_Click
        protected void ddlDisplayType_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
            try
            {
                int Count = DataAccessLayer.GetExecutionStatus(hdnClusterExecute.Value);
                lblDisplayCount.Text = Convert.ToString(Count);
                //DataTable dtExecutionDetails = DataAccessLayer.GetSUExecutionSummary(hdnClusterExecute.Value);
                GridBind();
                //ViewState["ListingInfo"] = dtExecutionDetails;
                //gvClusterExecute.DataSource = dtExecutionDetails;
                //gvClusterExecute.DataBind();
                //if (dtExecutionDetails.Rows.Count > 0)
                //{
                //    btnClusterExecute.Visible = true;
                //}
                //else
                //{
                //    btnClusterExecute.Visible = false;
                //}
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
           
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
            hdnClusterExecute.Value = "";
            gvHLBExecute.DataSource = null;
            lblText.Visible = true;           
            btnExportToExcel.Enabled = true;
            btnExecute.Enabled = false;            
            string UniqueID = string.Empty;
            pnlDisplay.Visible = true;
            
            //if (!IsPageRefresh)
            //{
                try
                {


                    rs = new RunbookOperations();
                    string strDestPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString();
                    uniqueGUID = Guid.NewGuid().ToString();
                    hdnExecute.Value = uniqueGUID;
                    hdnClusterExecute.Value = uniqueGUID;                   
                    string strRunbookPath = string.Empty;

                    strRunbookPath = ConfigurationManager.AppSettings["PatchingPathComplete"].ToString();
                    /*if (ddlScenario.SelectedValue.ToString() == "1" || ddlScenario.SelectedValue.ToString() == "3")
                    {
                        strRunbookPath = ConfigurationManager.AppSettings["PatchingPath_New"].ToString();
                    }
                    else if (ddlScenario.SelectedValue.ToString() == "2")
                    {
                        strRunbookPath = ConfigurationManager.AppSettings["PatchingPath_HLB"].ToString();
                    }*/

                    string strheader = "";
                    RunbookParams objparams = new RunbookParams();
                    objparams.RunbookPath = strRunbookPath;
                    objparams.LogsPath = txtLogPath.Text;
                    objparams.uniqueGUID = hdnClusterExecute.Value;

                    #region Group
                    if (ddlExecuteNames.SelectedValue != "0")
                    {
                        objparams.GroupName = ddlExecuteNames.SelectedItem.Text;
                        objparams.CheckExcelORText = "Group";

                    }
                    #endregion

                    #region ExcelFile
                    else if (fupExcel.HasFile)
                    {
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
                            
                            DataSet dsData = new DataSet();
                            dsData = ReadExcel(strServerlist);                           
                            if (dsData.Tables[0].Columns[0].ToString() == "ServerName" && dsData.Tables[0].Columns[1].ToString() == "Priority")
                            {
                                hdnFileName.Value = fupExcel.FileName;

                                if (fupExcel.FileName.Contains(".xlsx"))
                                {
                                    strheader = fupExcel.FileName.Replace(".xlsx", "");
                                }
                                else if (fupExcel.FileName.Contains(".xls"))
                                {
                                    strheader = fupExcel.FileName.Replace(".xls", "");
                                }
                                hdnExecuteFileName.Value = strheader;                               
                               
                                objparams.ServerName = strServerlist;
                                objparams.CheckExcelORText = "Excel";
                                   
                            }
                            else
                            {
                                // rvReports.Visible = false;
                                Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('Please Select Valid Excel');</script>");
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("'Sheet1$' is not a valid name"))
                            {
                                Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('ExcelFile Sheet Name should be Sheet1');</script>");

                            }
                            else
                            {
                                WriteError(ex);
                            }
                        }

                    }
                    #endregion

                    #region TextFilePath
                    else
                    {
                        
                        string strTextPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString() + uniqueGUID + ".txt";
                        CreateTextFile(txtExecuteServerNames.Text, strTextPath);
                        objparams.ServerName = strTextPath;
                        objparams.CheckExcelORText = "Text";
                       
                    }
                    #endregion

                    try
                    {
                        output = rs.StartGroupDisplayRunbook(objparams);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "Error starting runbook.")
                        {

                            output = rs.StartGroupDisplayRunbook(objparams);
                        }
                        else
                        {
                           Helper.WriteError(ex);
                        }
                    }
                    
                    System.Threading.Thread.Sleep(60000);
                    divClusterExecute.Visible = true;

                    int intRet = DataAccessLayer.GetHLBMSCStandaloneData(hdnClusterExecute.Value);
                    ddlScenario.SelectedValue = intRet.ToString();
                    
                    GridBind();

                    ExecuteTimer.Enabled = true;
                    ExecuteTimer.Interval = 60000;
                   
                }

                catch (Exception ex)
                {
                    WriteError(ex);

                }

                finally
                {

                }

            //}


            //else
            //{

            //    // Page.ClientScript.RegisterStartupScript(typeof(Page), "Message1", "<script type='text/jscript'>alert('Please do not click  Refresh');</script>");
            //}





        }

        public void GridBind()
        {
            try
            {
                pnlDisplay.Visible = true;
                pnlCounts.Visible = false;

                if (ddlScenario.SelectedValue.ToString() == "1" || ddlScenario.SelectedValue == "3")
                {
                    int MSCSCount = DataAccessLayer.GetExecutionStatus(hdnClusterExecute.Value);
                    lblDisplayCount.Text = Convert.ToString(MSCSCount);
                    ddlEnvironment.Visible = false;
                    lblDisplayEnv.Visible = false;
                    ddlDisplayEnv.Visible = false;
                    lblClusType.Visible = true;
                    ddlClustertype.Visible = true;
                    gvHLBExecute.Visible = false;
                    gvClusterExecute.Visible = true;
                    DataTable dtExecutionDetails = DataAccessLayer.GetSUExecutionSummary(hdnClusterExecute.Value, ddlClustertype.SelectedValue.ToString());
                    ViewState["ListingInfo"] = dtExecutionDetails;
                    gvClusterExecute.DataSource = dtExecutionDetails;
                    gvClusterExecute.DataBind();
                    if (dtExecutionDetails.Rows.Count > 0)
                    {
                        btnClusterExecute.Visible = true;
                    }
                    else
                    {
                        btnClusterExecute.Visible = false;
                    }
                }
                else if (ddlScenario.SelectedValue.ToString() == "2")
                {
                    int Count = DataAccessLayer.GetHLBExecutionStatus(hdnClusterExecute.Value);
                    lblDisplayCount.Text = Convert.ToString(Count);
                    gvHLBExecute.Visible = true;
                    ddlDisplayEnv.Visible = true;
                    lblDisplayEnv.Visible = true;
                    ddlClustertype.Visible = false;
                    lblClusType.Visible = false;
                    //  btnDisplayRefresh.Visible = false;
                    // hdnClusterExecute.Value = "3504b276-8eb7-46cf-8f9e-c10f9a7fc811";
                    gvClusterExecute.Visible = false;
                    DataTable dtExecutionDetails = DataAccessLayer.GetHLBExecutionSummary(hdnClusterExecute.Value, ddlDisplayEnv.SelectedValue);
                    ViewState["ListingInfo"] = dtExecutionDetails;

                    gvHLBExecute.DataSource = dtExecutionDetails;
                    gvHLBExecute.DataBind();
                    if (dtExecutionDetails.Rows.Count > 0)
                    {
                        btnClusterExecute.Visible = true;
                        // ExecuteTimer.Enabled = false;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(5000);
                        GridBind();
                        btnClusterExecute.Visible = false;
                    }

                    // hdnClusterExecute.Value = "3504b276-8eb7-46cf-8f9e-c10f9a7fc811";

                }
                else if (ddlScenario.SelectedValue.ToString() == "0")
                { 
                    
                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }


        }

        public string GetClusterType(string strIsCluster)
        {
            string strResult = string.Empty;
            if (strIsCluster == "1")
                strResult = "MSCS";
            else
                strResult = "Standalone";
            return strResult;
        }
        public bool GetResult(string strIsCluster)
        {
            bool strResult = false;
            if (strIsCluster == "MSCS")
                strResult = true;
            return strResult;
        }

        public bool GetCheckedResult(string Value)
        {
            bool strResult = false;
            if (Value == "1")
                strResult = true;
            return strResult;
        }
        //public string GetServerStatus(string status)
        //{
        //    string strStatus = string.Empty;
        //    if (status == "1")
        //        strStatus = "MSCS";
        //    else
        //        strStatus = "Standalone";
        //    return strStatus;
        //}
        public string GetPatchTool()
        {

            return ddlPatchingOption.SelectedItem.ToString();
        }

        public string GetPatchScenario()
        {

            return ddlScenario.SelectedItem.ToString();
        }

        public System.Drawing.Color GetColor(string Value)
        {
            //if (Value == "Success")
            if (Value.ToLower().Contains("success"))
                return System.Drawing.Color.Green;
            else if (Value.ToLower().Contains("inprogress"))
                return System.Drawing.Color.Orange;
            else
                return System.Drawing.Color.Red;
        }
        public System.Drawing.Color GetExtensionColor(string Value)
        {
            //if (Value == "Success")
            if (Value.ToLower() == ("yes"))
                return System.Drawing.Color.Red;
            else
                return System.Drawing.Color.Green;
        }
        #endregion

        #region ExecuteCompletedTimer_Tick


        protected void ExecuteCompletedTimer_Tick(object sender, EventArgs e)
        {
            //uprogressExecute.Visible = true;
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
            System.Threading.Thread.Sleep(5000);
            btnExportToExcel.Enabled = true;
            btnClusterExecute.Enabled = false;
            hdnClusterFlag.Value = "1";
            string strStatus = string.Empty;
            string strPatchOption = string.Empty;
            pnlCounts.Visible = true;

            if (lblTotalServers.Text == lblcompleted.Text)
            {
                ExecuteCompletedTimer.Enabled = false;
            }
            if (ddlScenario.SelectedValue == "1" || ddlScenario.SelectedValue == "3")
            {

                try
                {
                    lblTotalServers.Text = Convert.ToString(DataAccessLayer.GetSUExecutionTotalCount(hdnClusterExecute.Value));
                    lblInProgress.Text = Convert.ToString(DataAccessLayer.GetSUExecutionInProgressCount(hdnClusterExecute.Value) + DataAccessLayer.GetSUExecutionYetToStartCount(hdnClusterExecute.Value));
                    lblcompleted.Text = Convert.ToString(DataAccessLayer.GetSUExecutionCompletedCount(hdnClusterExecute.Value));


                    DataTable dtExecutionDetails = DataAccessLayer.GetSUExecutionSummary(hdnClusterExecute.Value, ddlExeClusterType.SelectedValue.ToString());
                    ViewState["ListingInfo"] = dtExecutionDetails;
                    gvClusterExecute.DataSource = dtExecutionDetails;
                    gvClusterExecute.DataBind();

                    if (dtExecutionDetails.Rows.Count > 0)
                    {
                        btnClusterExecute.Visible = true;
                    }
                    else
                    {
                        btnClusterExecute.Visible = false;
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
            else if (ddlScenario.SelectedValue == "2")
            {
                try
                {

                    lblTotalServers.Text = Convert.ToString(DataAccessLayer.GetHLBExecutionTotalCount(hdnClusterExecute.Value));
                    lblInProgress.Text = Convert.ToString(DataAccessLayer.GetHLBExecutionYetToStartCount(hdnClusterExecute.Value));
                    lblcompleted.Text = Convert.ToString(DataAccessLayer.GetHLBExecutionCompletedCount(hdnClusterExecute.Value));

                    DataTable dtExecutionDetails = DataAccessLayer.GetHLBExecutionSummary(hdnClusterExecute.Value, ddlEnvironment.SelectedValue);
                    ViewState["ListingInfo"] = dtExecutionDetails;
                    gvHLBExecute.DataSource = dtExecutionDetails;
                    gvHLBExecute.DataBind();
                    foreach (GridViewRow row in gvHLBExecute.Rows)
                    {

                        DropDownList ddlPause = (DropDownList)row.FindControl("ddlPause");
                        CheckBox ChkValue = (CheckBox)row.FindControl("ChkValue");
                        ChkValue.Enabled = ddlPause.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    WriteError(ex);
                }

            }




            //ClusterExecute();


        }
        #endregion

        #region Execute Timer Tick event

        protected void ExecuteTimer_Tick(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
            //  hdnClusterExecute.Value = "328c38d6-e87b-4c62-9870-6bb7e30d8c28";
            //System.Threading.Thread.Sleep(10000);
            string strStatus = string.Empty;
            string strPatchOption = string.Empty;
            

            UpdateRunValidationFlag();
              try
              {            
                    
                 GridBind();  
               }

            catch (Exception ex)
            {
                WriteError(ex);
            }


        }

        
        #endregion

        #region ddlHLBPause_SelectedIndexChanged
        protected void ddlHLBPause_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // uprogressExecute.Visible = false;
                DropDownList ddlPause = (DropDownList)sender;
                GridViewRow row = (GridViewRow)ddlPause.NamingContainer;
                string Nodename = string.Empty;
                string ChkFlagValue = string.Empty;
                if (row != null)
                {

                    bool Value = ((CheckBox)(row.FindControl("ChkValue"))).Checked;
                    Label lblServerName = (Label)row.FindControl("lblServerName");
                    Nodename = lblServerName.Text;
                    CheckBox ChkValue = (CheckBox)row.FindControl("ChkValue");
                    ChkFlagValue = ChkValue.Checked ? "1" : "0";
                    //DropDownList ddlPause = (DropDownList)row.FindControl("ddlPause");
                    string strPaused = ddlPause.SelectedValue.ToString();

                    //   string strPaused = "";
                    int result = DataAccessLayer.UpdateHLBCheckValue(hdnClusterExecute.Value, Nodename, ChkFlagValue, strPaused);

                    DataTable dtExecutionDetails = DataAccessLayer.GetHLBExecutionSummary(hdnClusterExecute.Value, ddlEnvironment.SelectedValue);
                    ViewState["ListingInfo"] = dtExecutionDetails;
                    gvHLBExecute.DataSource = dtExecutionDetails;
                    gvHLBExecute.DataBind();

                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }

        }
        #endregion

        #region ChkVIPIP_Clicked
        /// <summary>
        /// checkbox checked for all nodes of the cluster if any of the nodes under the cluster is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void ChkVIPIP_Clicked(Object sender, EventArgs e)
        {
            try
            {
                //uprogressExecute.Visible = false;
                CheckBox ChkVIPIP = (CheckBox)sender;
                GridViewRow row = (GridViewRow)ChkVIPIP.NamingContainer;
                string Nodename = string.Empty;
                string ChkFlagValue = string.Empty;
                if (row != null)
                {

                    bool Value = ((CheckBox)(row.FindControl("ChkValue"))).Checked;
                    Label lblServerName = (Label)row.FindControl("lblServerName");
                    Nodename = lblServerName.Text;
                    CheckBox ChkValue = (CheckBox)row.FindControl("ChkValue");
                    ChkFlagValue = ChkValue.Checked ? "1" : "0";
                    DropDownList ddlPause = (DropDownList)row.FindControl("ddlPause");
                    string strPaused = ddlPause.SelectedValue.ToString();

                    //    string strPaused = "";
                    int result = DataAccessLayer.UpdateHLBCheckValue(hdnClusterExecute.Value, Nodename, ChkFlagValue, strPaused);
                    if (Value == true)
                    {
                        Label lblVIPIP = (Label)row.FindControl("lblVIPIP");

                        foreach (GridViewRow gvr in gvHLBExecute.Rows)
                        {
                            Label VIPIP = (Label)gvr.FindControl("lblVIPIP");
                            CheckBox cVIPIP = (CheckBox)gvr.FindControl("ChkValue");
                            Label lblNodeName = (Label)gvr.FindControl("lblServerName");
                            Nodename = lblNodeName.Text;
                            if (lblVIPIP.Text == VIPIP.Text)
                            {
                                cVIPIP.Checked = true;
                                ChkFlagValue = cVIPIP.Checked ? "1" : "0";
                                int value = DataAccessLayer.UpdateHLBCheckValue(hdnClusterExecute.Value, Nodename, ChkFlagValue, strPaused);
                            }

                        }
                    }
                    else
                    {
                        int value = DataAccessLayer.UpdateHLBCheckValue(hdnClusterExecute.Value, Nodename, ChkFlagValue, strPaused);
                    }

                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }

            // ViewState["ListingInfo"] = gvClusterExecute.DataSource;
        }
        #endregion
        #region UpdateRunValidationFlag

        public void UpdateRunValidationFlag()
        {
            string PatchingScenario = ddlScenario.SelectedValue.ToString();
            string Nodename = string.Empty;
            string RunValidationFlag = "0";
            try
            {

                if (PatchingScenario == "1" ||PatchingScenario=="3")
                {
                    foreach (GridViewRow row in gvClusterExecute.Rows)
                    {
                        Label lblServerName = (Label)row.FindControl("lblServerName");
                        Nodename = lblServerName.Text;
                        CheckBox chkRunValidation = (CheckBox)row.FindControl("chkRunValidation");
                        RunValidationFlag = chkRunValidation.Checked ? "1" : "0";
                        int result = DataAccessLayer.UpdateRunValidationFlag(hdnClusterExecute.Value, Nodename, RunValidationFlag, PatchingScenario);
                    }
                }

                else if (PatchingScenario == "2")
                {
                    foreach (GridViewRow row in gvHLBExecute.Rows)
                    {
                        Label lblServerName = (Label)row.FindControl("lblServerName");
                        Nodename = lblServerName.Text;
                        CheckBox chkRunValidation = (CheckBox)row.FindControl("chkRunValidationHLB");
                        RunValidationFlag = chkRunValidation.Checked ? "1" : "0";
                        int result = DataAccessLayer.UpdateRunValidationFlag(hdnClusterExecute.Value, Nodename, RunValidationFlag, PatchingScenario);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }

        #endregion

        #region CommentedRebootCode

        //#region btnReboot_Click

        //protected void btnReboot_Click(object sender, EventArgs e)
        //{
        //    hdnTimer.Value = "Reboot";
        //    ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
        //    btnExecute.Enabled = false;
        //    rvReports.Visible = true;
        //    btnReboot.Enabled = false;
        //    if (!IsPageRefresh)
        //    {
        //        try
        //        {
        //            //Guid runbookid = new Guid();
        //            rs = new RunbookOperations();
        //            string strDestPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString();
        //            uniqueGUID = Guid.NewGuid().ToString();
        //            hdnExecute.Value = uniqueGUID;
        //            string strRunbookPath = string.Empty;
        //            //  string strRunbookPath = ConfigurationManager.AppSettings["PatchingPath"].ToString();

        //            strRunbookPath = ConfigurationManager.AppSettings["Reboot_HLB"].ToString();

        //            string strheader = "";
        //            if (fupExcel.HasFile)
        //            {

        //                //string strName = fupExcel.FileName.Replace(".xlsx", uniqueGUID);
        //                //string strFileName = strName + ".xlsx";
        //                string strName = "";
        //                string strFileName = "";

        //                if (fupExcel.FileName.Contains(".xlsx"))
        //                {
        //                    strName = fupExcel.FileName.Replace(".xlsx", uniqueGUID);
        //                    strFileName = strName + ".xlsx";

        //                }
        //                else if (fupExcel.FileName.Contains(".xls"))
        //                {
        //                    strName = fupExcel.FileName.Replace(".xls", uniqueGUID);
        //                    strFileName = strName + ".xls";
        //                }
        //                else
        //                {
        //                }

        //                strServerlist = strDestPath + strFileName;
        //                fupExcel.SaveAs(strServerlist);
        //                string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strServerlist + ";Extended Properties=Excel 12.0;";
        //                try
        //                {
        //                    string query = String.Format("select * from [{0}]", "Sheet1$");
        //                    OleDbDataAdapter daData = new OleDbDataAdapter(query, connectionString);

        //                    DataSet dsData = new DataSet();
        //                    daData.Fill(dsData);
        //                    //if (dsData.Tables[0].Columns.Count == 2)
        //                    //{
        //                    if (dsData.Tables[0].Columns[0].ToString() == "ServerName" && dsData.Tables[0].Columns[1].ToString() == "Priority")
        //                    {
        //                        hdnFileName.Value = fupExcel.FileName;

        //                        if (fupExcel.FileName.Contains(".xlsx"))
        //                        {
        //                            strheader = fupExcel.FileName.Replace(".xlsx", "");
        //                        }
        //                        else if (fupExcel.FileName.Contains(".xls"))
        //                        {
        //                            strheader = fupExcel.FileName.Replace(".xls", "");
        //                        }

        //                        string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
        //                        // string serviceRoot = null;

        //                        output = rs.StartRebootRunbook(strRunbookPath, strServerlist, "", uniqueGUID, strheader, "Excel");


        //                    }
        //                    else
        //                    {
        //                        rvReports.Visible = false;
        //                        Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('Please Select Valid Excel');</script>");
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    if (ex.Message.Contains("'Sheet1$' is not a valid name"))
        //                    {
        //                        Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('ExcelFile Sheet Name should be Sheet1');</script>");

        //                    }
        //                    else
        //                    {
        //                        WriteError(ex);
        //                    }
        //                }
        //                //}
        //                //else
        //                //{
        //                //    rvReports.Visible = false;
        //                //    Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('Please Select Valid Excel');</script>");
        //                //}
        //            }
        //            else
        //            {
        //                lblRVExecute.Text = "";
        //                string[] values = txtExecuteServerNames.Text.Split(',');
        //                string strTextPath = ConfigurationSettings.AppSettings["ExcelFilesPath"].ToString() + uniqueGUID + ".txt";
        //                if (File.Exists(strTextPath))
        //                {
        //                }
        //                else
        //                {
        //                    // File.CreateText(filePath);
        //                    using (StreamWriter sr = File.CreateText(strTextPath))
        //                    {
        //                        sr.Close();
        //                    }
        //                    StreamWriter sw = null;
        //                    try
        //                    {
        //                        sw = new StreamWriter(strTextPath, false);

        //                        for (int i = 0; i < values.Length; i++)
        //                        {
        //                            if (values[i].ToString() != "")
        //                            {

        //                                sw.Write(values[i].ToString());
        //                                sw.WriteLine();


        //                            }



        //                        } sw.Close();

        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        //MessageBox.Show("Invalid Operation : \n" + ex.ToString(),  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);       
        //                    }


        //                }


        //                output = rs.StartRebootRunbook(strRunbookPath, strTextPath, "", uniqueGUID, "", "Text");
        //            }
        //            path = ConfigurationManager.AppSettings["RebootReportPath_HLB"].ToString();
        //            rvReports.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
        //            string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
        //            string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
        //            rvReports.ServerReport.ReportServerUrl = new Uri(@url);
        //            rvReports.ServerReport.ReportPath = ReportViewerPath + path;
        //            exereport = rvReports.ServerReport.ReportPath;
        //            rvReports.ShowParameterPrompts = false;
        //            rvReports.ShowPrintButton = true;
        //            rvReports.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", hdnExecute.Value));

        //            rvReports.ServerReport.Refresh();
        //            ReportTimer.Enabled = true;
        //            //ReportTimer.Interval = Convert.ToInt32(ddlRefresh.SelectedValue.ToString());
        //            ReportTimer.Interval = 300000;



        //        }


        //        catch (Exception ex)
        //        {
        //            WriteError(ex);

        //        }

        //        finally
        //        {

        //        }

        //    }


        //    else
        //    {

        //        // Page.ClientScript.RegisterStartupScript(typeof(Page), "Message1", "<script type='text/jscript'>alert('Please do not click  Refresh');</script>");
        //    }



        //}
        //#endregion
        #region Report Timer Tick event

        //public void GetRebootReport()
        //{
        //    //  rvReports.Visible = true;
        //    rvReports.Visible = true;
        //    try
        //    {
        //        path = ConfigurationManager.AppSettings["RebootReportPath_HLB"].ToString();
        //        rvReports.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
        //        string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
        //        string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
        //        rvReports.ServerReport.ReportServerUrl = new Uri(@url);
        //        rvReports.ServerReport.ReportPath = ReportViewerPath + path;
        //        exereport = rvReports.ServerReport.ReportPath;
        //        rvReports.ShowParameterPrompts = false;
        //        rvReports.ShowPrintButton = true;
        //        rvReports.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", hdnExecute.Value));

        //        rvReports.ServerReport.Refresh();
        //        ReportTimer.Enabled = true;
        //        //ReportTimer.Interval = Convert.ToInt32(ddlRefresh.SelectedValue.ToString());
        //        ReportTimer.Interval = 300000;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteError(ex);
        //    }

        //}
        //protected void ReportTimer_Tick(object sender, EventArgs e)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
        //    string strStatus = string.Empty;
        //    string strPatchOption = string.Empty;
        //    if (hdnTimer.Value == "Reboot")
        //    {
        //       // GetRebootReport();
        //    }
        //    else
        //    {

        //        try
        //        {

        //            if (ddlPatchingOption.SelectedIndex == 1)
        //            {
        //                path = ConfigurationManager.AppSettings["MSNReportPath"].ToString();
        //                //path = "/PatchingReport";
        //                strPatchOption = "MSNPatch";
        //            }
        //            else if (ddlPatchingOption.SelectedIndex == 2)
        //            {
        //                path = ConfigurationManager.AppSettings["ODPReportPath"].ToString();
        //                //path = "/OnDemandPatchingReport";
        //                strPatchOption = "ODP";
        //            }
        //            else if (ddlPatchingOption.SelectedIndex == 3 || ddlPatchingOption.SelectedIndex == 6)
        //            {
        //                path = ConfigurationManager.AppSettings["SimpleUpdateReportPath"].ToString();
        //                //path = "/Simple Update Report";
        //                strPatchOption = "SimpleUpdate";
        //            }
        //            else if (ddlPatchingOption.SelectedIndex == 4)
        //            {
        //                path = ConfigurationManager.AppSettings["ChainingReportPath"].ToString();
        //                //path = "/Chaining Patch Report";
        //                strPatchOption = "Chaining";
        //            }

        //            lblRVExecute.Text = "Report Name: " + hdnFileName.Value;

        //            rvReports.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
        //            string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
        //            string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
        //            rvReports.ServerReport.ReportServerUrl = new Uri(@url);
        //            rvReports.ServerReport.ReportPath = ReportViewerPath + path;
        //            exereport = rvReports.ServerReport.ReportPath;
        //            rvReports.ShowParameterPrompts = false;
        //            rvReports.ShowPrintButton = true;
        //            rvReports.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", hdnExecute.Value));

        //           // rvReports.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UserName", txtDomainAccName.Text));


        //            rvReports.ServerReport.Refresh();


        //            strStatus = DataAccessLayer.GetRunbookStatus(hdnExecute.Value, strPatchOption, "Normal");

        //            if (strStatus == "Completed")
        //            {
        //                ReportTimer.Enabled = false;
        //            }




        //        }
        //        catch (Exception ex)
        //        {
        //            WriteError(ex);
        //        }
        //        finally
        //        {

        //        }
        //    }



        //}
        #endregion
        #endregion

        #region gvClusterExecute_RowCommand
        /// <summary>
        ///  Click event for calling Cluster Runbooks
        /// </summary>
        /// <author>Sudha Gubbala</author>
        /// <CreatedDate>10/01/2013</CreatedDate>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvClusterExecute_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Resume")
                {
                    //// Retrieve the row index stored in the 
                    //// CommandArgument property.
                    //int index = Convert.ToInt32(e.CommandArgument);

                    //// Retrieve the row that contains the button 
                    //// from the Rows collection.
                    //GridViewRow row = gvClusterExecute.Rows[index];
                    // string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });

                    // string ServerName = commandArgs[0];
                    // string NodeName = commandArgs[1];
                    string NodeName = e.CommandArgument.ToString();
                    int pausedstatus = DataAccessLayer.UpdatePausedStatus(hdnClusterExecute.Value, NodeName);
                    //if (pausedstatus > 0)
                    //{

                    rs = new RunbookOperations();
                    string strRunbookPath = ConfigurationManager.AppSettings["ResumePath"].ToString();
                    RunbookParams objParams = new RunbookParams();

                    try
                    {
                        objParams.RunbookPath = strRunbookPath;
                        objParams.ServerName = NodeName;
                        objParams.InputFilename = "";
                        objParams.CheckExcelORText = "Text";
                        objParams.uniqueGUID = hdnClusterExecute.Value;
                        objParams.LogsPath = txtLogPath.Text;
                        objParams.Ipak = "nope";
                        objParams.OnlyQFE = txtOnlyQFE.Text;
                        objParams.BPUOption = rblBPU.SelectedValue.ToString();
                        objParams.simpleUpdateOption = ddlSimpleUpdateOption.SelectedValue;
                        objParams.MSNRebootFlag = ddlReboot.SelectedValue;
                        objParams.ScanOrPatch = ddlODPOption.SelectedIndex.ToString();
                        objParams.ExcludeQFE = txtExcludeQFE.Text;
                        objParams.PatchingOption = ddlPatchingOption.SelectedIndex.ToString();
                        // output = rs.StartResumeRunbook(strRunbookPath, NodeName, hdnClusterExecute.Value);

                        output = rs.StartResumeRunbook(objParams);
                    }

                    catch (Exception ex)
                    {
                        if (ex.Message == "Error starting runbook.")
                        {

                            output = rs.StartResumeRunbook(objParams);
                        }
                        else
                        {
                            WriteError(ex);
                        }


                    }
                    //}
                    //else
                    //{
                    //    Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('Unable to Resume the Node ');</script>");
                    //   // WriteError("not Updated in DB");
                    //}

                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }


        }
        #endregion

        #region Resume Click
        /// <summary>
        ///  Click event for calling Cluster Runbooks
        /// </summary>
        /// <author>Sudha Gubbala</author>
        /// <CreatedDate>10/01/2013</CreatedDate>
        /// <param name="sender"></param>
        /// <param name="e"></param>



        protected void btnResume_Click(object sender, EventArgs e)
        {
            try
            {
                //Get the button that raised the event        
                Button btn = (Button)sender;
                //Get the row that contains this button        
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                //Get rowindex        
                int index = gvr.RowIndex;

                GridViewRow row = gvClusterExecute.Rows[index];
                Button btnResume = (Button)row.FindControl("btnResume");
                btnResume.Enabled = false;
                Label lblClusterName = (Label)row.FindControl("lblClusterName");
                Label lblServerName = (Label)row.FindControl("lblServerName");

                int pausedstatus = DataAccessLayer.UpdatePausedStatus(hdnClusterExecute.Value, lblServerName.Text);
                //if (pausedstatus > 0)
                //{

                rs = new RunbookOperations();
                string strRunbookPath = ConfigurationManager.AppSettings["ResumePath"].ToString();
                RunbookParams objParams = new RunbookParams();

                try
                {
                    objParams.RunbookPath = strRunbookPath;
                    objParams.ServerName = lblClusterName.Text;
                    objParams.InputFilename = hdnExecuteFileName.Value;
                    objParams.CheckExcelORText = "Text";
                    objParams.uniqueGUID = hdnClusterExecute.Value;
                    objParams.LogsPath = txtLogPath.Text;
                    objParams.Ipak = "nope";
                    objParams.OnlyQFE = txtOnlyQFE.Text;
                    objParams.BPUOption = rblBPU.SelectedValue.ToString();
                    objParams.simpleUpdateOption = ddlSimpleUpdateOption.SelectedValue;
                    objParams.MSNRebootFlag = ddlReboot.SelectedValue;
                    objParams.ScanOrPatch = ddlODPOption.SelectedIndex.ToString();
                    objParams.ExcludeQFE = txtExcludeQFE.Text;
                    objParams.PatchingOption = ddlPatchingOption.SelectedIndex.ToString();
                    // output = rs.StartResumeRunbook(strRunbookPath, NodeName, hdnClusterExecute.Value);

                    output = rs.StartResumeRunbook(objParams);
                }

                catch (Exception ex)
                {

                    if (ex.Message == "Error starting runbook.")
                    {
                        try
                        {
                            output = rs.StartResumeRunbook(objParams);
                        }
                        catch (Exception exmsg)
                        {
                            WriteError(exmsg);
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Unable to Resume the Node,Please Proceed Manually')", true);
                        }
                    }
                    else if (ex.Message == "Exception has been thrown by the target of an invocation.")
                    {

                        DataAccessLayer.GetClearCacheStatus();
                        try
                        {
                            output = rs.StartResumeRunbook(objParams);
                        }
                        catch (Exception ex1)
                        {


                            if (ex1.Message == "Exception has been thrown by the target of an invocation.")
                            {
                                WriteError(ex1);
                                //Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('Unable to Resume the Node,Please Proceed Manually ');</script>");
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Unable to Resume the Node,Please Proceed Manually')", true);
                            }
                        }


                    }
                    else
                    {
                        WriteError(ex);
                    }


                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }


        }
        #endregion

        #region btnHLBResume_Click

        protected void btnHLBResume_Click(object sender, EventArgs e)
        {
            try
            {
                //Get the button that raised the event        
                Button btn = (Button)sender;
                //Get the row that contains this button        
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                //Get rowindex        
                int index = gvr.RowIndex;

                GridViewRow row = gvHLBExecute.Rows[index];
                Button btnResume = (Button)row.FindControl("btnResume");
                btnResume.Enabled = false;

                Label lblVIPIP = (Label)row.FindControl("lblVIPIP");
                Label lblServerName = (Label)row.FindControl("lblServerName");
                Label lblNodeIP = (Label)row.FindControl("lblNodeIP");
                Label lblPort = (Label)row.FindControl("lblPort");

                int pausedstatus = DataAccessLayer.UpdateHLBPausedStatus(hdnClusterExecute.Value, lblServerName.Text);
                rs = new RunbookOperations();
                string strRunbookPath = ConfigurationManager.AppSettings["HLBResumePath"].ToString();
                //RunbookParams objParams = new RunbookParams();
                var objParams = new RunbookParams { RunbookPath = strRunbookPath, uniqueGUID = hdnClusterExecute.Value, VIP = lblVIPIP.Text, NodeName = lblServerName.Text, NodeIP = lblNodeIP.Text, Port = lblPort.Text };
                try
                {
                    //objParams.RunbookPath = strRunbookPath;

                    //objParams.uniqueGUID = hdnClusterExecute.Value;
                    //objParams.VIP = lblVIPIP.Text;
                    //objParams.NodeName = lblServerName.Text;
                    //objParams.NodeIP = lblNodeIP.Text;
                    //objParams.Port = "";

                    output = rs.StartResumeNodeRunbook(objParams);
                }

                catch (Exception ex)
                {
                    if (ex.Message == "Error starting runbook.")
                    {

                        output = rs.StartResumeNodeRunbook(objParams);
                    }
                    else
                    {
                        WriteError(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }

        }

        #endregion

        #region btnClusterExecute_Click
        /// <summary>
        ///  Click event for calling Cluster Runbooks
        /// </summary>
        /// <author>Sudha Gubbala</author>
        /// <CreatedDate>9/23/2013</CreatedDate>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected void btnClusterOk_Click(object sender, EventArgs e)
        {
            try
            {
                //  uprogressExecute.Visible = true;
                ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
                mpeClusterExecute.Hide();
                pnlDisplay.Visible = false;
                btnClusterExecute.Enabled = false;
                pnlCounts.Visible = true;
                ClusterExecute();
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }





        }

        #region ClusterExecute
        public void ClusterExecute()
        {

            try
            {
                pnlCounts.Visible = true;
                pnlDisplay.Visible = false;
                if (ddlScenario.SelectedValue.ToString() == "1" || ddlScenario.SelectedValue == "3")
                {
                    gvHLBExecute.Visible = false;
                    gvClusterExecute.Visible = true;
                    lblExeceEnv.Visible = false;
                    ddlEnvironment.Visible = false;
                    ddlExeClusterType.Visible = true;
                    lblExeClustype.Visible = true;
                    hdnClusterFlag.Value = "1";

                    UpdateRunValidationFlag();

                    string Nodename = string.Empty;
                    string PauseFlag = "0";
                    string PauseValue = string.Empty;
                    string FailbackFlag = "0";
                    string ForceStandaloneFlag = "0";
                    
                    int result = 0;

                    foreach (GridViewRow row in gvClusterExecute.Rows)
                    {

                        Label lblServerName = (Label)row.FindControl("lblServerName");
                        Nodename = lblServerName.Text;
                        DropDownList ddlPause = (DropDownList)row.FindControl("ddlPause");
                        DropDownList ddlFailOverNode = (DropDownList)row.FindControl("ddlFailOverNode");

                        PauseValue = ddlPause.SelectedValue;
                        CheckBox ChkPauseNode = (CheckBox)row.FindControl("ChkPauseNode");
                        CheckBox ChkFailBack = (CheckBox)row.FindControl("ChkFailBack");
                        PauseFlag = ChkPauseNode.Checked ? "1" : "0";
                        FailbackFlag = ChkFailBack.Checked ? "1" : "0";
                        CheckBox chkForceStandalone = (CheckBox)row.FindControl("chkForceStandalone");
                        ForceStandaloneFlag = chkForceStandalone.Checked ? "1" : "0";
                        ChkPauseNode.Enabled = ChkFailBack.Enabled = ddlPause.Enabled = ddlFailOverNode.Enabled = false;

                        Label lblNodeType = (Label)row.FindControl("lblNodeType");
                        Label lblClusterType = (Label)row.FindControl("lblClusterType");
                        Label lblNodeClusterName = (Label)row.FindControl("lblNodeClusterName");





                        if (((lblClusterType.Text.Contains("MSCS - 2")) && lblNodeType.Text.ToLower() == "active"))
                        {
                            DataSet dsBackUpNodelist = DataAccessLayer.GetBackUpNodesList(hdnClusterExecute.Value, lblServerName.Text);
                            if (dsBackUpNodelist.Tables[0].Rows.Count > 0)
                            {
                                hdnBackUpNode.Value = dsBackUpNodelist.Tables[0].Rows[0]["NodeName"].ToString();
                                // result = DataAccessLayer.UpdateNodeInfo(hdnClusterExecute.Value, Nodename, PauseFlag, PauseValue, FailbackFlag, ForceStandaloneFlag, hdnBackUpNode.Value);
                            }
                            else
                                hdnBackUpNode.Value = "";
                            result = DataAccessLayer.UpdateNodeInfo(hdnClusterExecute.Value, Nodename, PauseFlag, PauseValue, FailbackFlag, ForceStandaloneFlag, hdnBackUpNode.Value);
                        }
                        else if (lblClusterType.Text.Contains("Standalone"))
                        {
                        }
                        else
                        {

                            result = DataAccessLayer.UpdateNodeInfo(hdnClusterExecute.Value, Nodename, PauseFlag, PauseValue, FailbackFlag, ForceStandaloneFlag, ddlFailOverNode.SelectedValue);
                        }

                    }


                    rs = new RunbookOperations();
                    string strRunbookPath = ConfigurationManager.AppSettings["PatchingPath"].ToString();
                    try
                    {
                        output = rs.StartNewRunbookWithParameters(strRunbookPath, ddlPatchingOption.SelectedIndex.ToString(),
                            txtLogPath.Text, ddlODPOption.SelectedIndex.ToString(), "nope", ddlSimpleUpdateOption.SelectedValue, hdnClusterExecute.Value,
                            hdnExecuteFileName.Value, ddlReboot.SelectedValue, txtOnlyQFE.Text, txtExcludeQFE.Text, rblBPU.SelectedValue.ToString());
                    }

                    catch (Exception ex)
                    {
                        if (ex.Message == "Error starting runbook.")
                        {

                            output = rs.StartNewRunbookWithParameters(strRunbookPath, ddlPatchingOption.SelectedIndex.ToString(),
                            txtLogPath.Text, ddlODPOption.SelectedIndex.ToString(), "nope", ddlSimpleUpdateOption.SelectedValue, hdnClusterExecute.Value,
                             hdnExecuteFileName.Value, ddlReboot.SelectedValue, txtOnlyQFE.Text, txtExcludeQFE.Text, rblBPU.SelectedValue.ToString());
                        }
                        else
                        {
                            WriteError(ex);
                        }


                    }

                    ExecuteCompletedTimer.Enabled = true;
                    lblTotalServers.Text = Convert.ToString(DataAccessLayer.GetSUExecutionTotalCount(hdnClusterExecute.Value));
                    lblInProgress.Text = Convert.ToString(DataAccessLayer.GetSUExecutionInProgressCount(hdnClusterExecute.Value) + DataAccessLayer.GetSUExecutionYetToStartCount(hdnClusterExecute.Value));
                    lblcompleted.Text = Convert.ToString(DataAccessLayer.GetSUExecutionCompletedCount(hdnClusterExecute.Value));

                    DataTable dtExecutionDetails = DataAccessLayer.GetSUExecutionSummary(hdnClusterExecute.Value, ddlExeClusterType.SelectedValue.ToString());
                    ViewState["ListingInfo"] = dtExecutionDetails;
                    gvClusterExecute.DataSource = dtExecutionDetails;
                    gvClusterExecute.DataBind();
                }
                else if (ddlScenario.SelectedValue.ToString() == "2")
                {

                    gvClusterExecute.Visible = false;
                    gvHLBExecute.Visible = true;
                    lblExeceEnv.Visible = true;
                    ddlEnvironment.Visible = true;
                    ddlExeClusterType.Visible = false;
                    lblExeClustype.Visible = false;
                    hdnClusterFlag.Value = "1";

                    UpdateRunValidationFlag();

                    rs = new RunbookOperations();
                    string strRunbookPath = ConfigurationManager.AppSettings["HLBExecutePath"].ToString();
                    RunbookParams objParams = new RunbookParams();

                    try
                    {
                        objParams.RunbookPath = strRunbookPath;
                        objParams.InputFilename = "";
                        objParams.CheckExcelORText = "Text";
                        objParams.uniqueGUID = hdnClusterExecute.Value;
                        objParams.LogsPath = txtLogPath.Text;
                        objParams.Ipak = "nope";
                        objParams.OnlyQFE = txtOnlyQFE.Text;
                        objParams.BPUOption = rblBPU.SelectedValue.ToString();
                        objParams.simpleUpdateOption = ddlSimpleUpdateOption.SelectedValue;
                        objParams.MSNRebootFlag = ddlReboot.SelectedValue;
                        objParams.ScanOrPatch = ddlODPOption.SelectedIndex.ToString();
                        objParams.ExcludeQFE = txtExcludeQFE.Text;
                        objParams.PatchingOption = ddlPatchingOption.SelectedIndex.ToString();

                        output = rs.StartHLBExecuteRunbook(objParams);
                    }

                    catch (Exception ex)
                    {
                        if (ex.Message == "Error starting runbook.")
                        {

                            output = rs.StartHLBExecuteRunbook(objParams);
                        }
                        else
                        {
                            WriteError(ex);
                        }
                    }
                    ExecuteCompletedTimer.Enabled = true;
                    lblTotalServers.Text = Convert.ToString(DataAccessLayer.GetHLBExecutionTotalCount(hdnClusterExecute.Value));
                    //lblInProgress.Text = Convert.ToString(DataAccessLayer.GetSUExecutionInProgressCount(hdnClusterExecute.Value) + DataAccessLayer.GetHLBSUExecutionYetToStartCount(hdnClusterExecute.Value));
                    lblInProgress.Text = Convert.ToString(DataAccessLayer.GetHLBExecutionYetToStartCount(hdnClusterExecute.Value));
                    lblcompleted.Text = Convert.ToString(DataAccessLayer.GetHLBExecutionCompletedCount(hdnClusterExecute.Value));

                    DataTable dtExecutionDetails = DataAccessLayer.GetHLBExecutionSummary(hdnClusterExecute.Value, ddlEnvironment.SelectedValue);
                    ViewState["ListingInfo"] = dtExecutionDetails;
                    gvHLBExecute.DataSource = dtExecutionDetails;
                    gvHLBExecute.DataBind();

                    foreach (GridViewRow row in gvHLBExecute.Rows)
                    {

                        DropDownList ddlPause = (DropDownList)row.FindControl("ddlPause");
                        CheckBox ChkValue = (CheckBox)row.FindControl("ChkValue");
                        ChkValue.Enabled = ddlPause.Enabled = false;
                    }

                }
            }
            catch (Exception ex)
            {

                WriteError(ex);

            }

        }

        #endregion

        protected void btnClusterExecute_Click(object sender, EventArgs e)
        {
            try
            {
                //uprogressExecute.Visible = true;
                ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
                ExecuteTimer.Enabled = false;
                string Nodename = string.Empty;
                int result = 0;
                string ChkFlagValue = "0";
                //string ChkVIPValue = "0";
                if (ddlScenario.SelectedValue.ToString() == "1" || ddlScenario.SelectedValue == "3")
                {                
                    ddlEnvironment.Visible = false;
                    mpeClusterExecute.Hide();
                    pnlDisplay.Visible = false;
                    btnClusterExecute.Enabled = false;
                    pnlCounts.Visible = true;
                    ClusterExecute();

                }
                else if (ddlScenario.SelectedValue.ToString() == "2")
                {
                    btnYes.Visible = true;
                    btnNo.Text = "Cancel";
                    foreach (GridViewRow row in gvHLBExecute.Rows)
                    {

                        Label lblServerName = (Label)row.FindControl("lblServerName");
                        Nodename = lblServerName.Text;
                        CheckBox ChkValue = (CheckBox)row.FindControl("ChkValue");
                        ChkFlagValue = ChkValue.Checked ? "1" : "0";

                        DropDownList ddlPause = (DropDownList)row.FindControl("ddlPause");
                        string strPaused = ddlPause.SelectedValue.ToString();
                        // string strPaused = "";
                        result = DataAccessLayer.UpdateHLBCheckValue(hdnClusterExecute.Value, Nodename, ChkFlagValue, strPaused);

                    }

                    DataTable dtServerCount = DataAccessLayer.GetServerCount(hdnClusterExecute.Value);
                    int ServerCount = 0;
                    foreach (DataRow row in dtServerCount.Rows)
                    {
                        int count = Convert.ToInt32(row["NodeCount"].ToString());
                        if (count > 1)
                            ServerCount = 1;

                    }
                    int NodesCount = DataAccessLayer.GetAllCheckedVips(hdnClusterExecute.Value);


                    if (NodesCount > 0)
                    {
                        mpeClusterExecute.Show();
                        lblPopUpMsg.Text = "All the Servers in same VIP are selected,Do you Still want to Proceed?";
                    }
                    else if (ServerCount == 1)
                    // else if (hdnNodeCount.Value == "1")
                    {
                        mpeClusterExecute.Show();
                        lblPopUpMsg.Text = "One of the ServerName is part of multiple VIPs,Do you Still want to Proceed?";
                    }
                    else
                    {
                        mpeClusterExecute.Hide();
                        pnlDisplay.Visible = false;
                        btnClusterExecute.Enabled = false;
                        pnlCounts.Visible = true;
                        ClusterExecute();
                        ddlEnvironment.Visible = true;
                    }


                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }


        }

        #endregion

        #region gvHLBClusterExecute_OnRowDataBound
        protected void gvHLBClusterExecute_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            // uprogressExecute.Visible = true;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    DropDownList ddlPause = (DropDownList)e.Row.FindControl("ddlPause");
                    Label lblPauseDuringExec = (Label)e.Row.FindControl("lblPauseDuringExec");
                    CheckBox ChkValue = (CheckBox)e.Row.FindControl("ChkValue");
                    if (lblPauseDuringExec.Text == "")
                        ddlPause.Items.FindByValue("0").Selected = true;
                    else
                        ddlPause.Items.FindByValue(lblPauseDuringExec.Text).Selected = true;

                    Label lblServerName = (Label)e.Row.FindControl("lblServerName");
                    DataTable dtExecutionOutput = DataAccessLayer.GetSUExecutionOuput(hdnClusterExecute.Value, lblServerName.Text);
                    GridView gvPatchOutput = (GridView)e.Row.FindControl("gvPatchOutput");
                    gvPatchOutput.DataSource = dtExecutionOutput;
                    gvPatchOutput.DataBind();
                    if (e.Row.DataItemIndex == 0)
                    {
                        e.Row.BackColor = System.Drawing.Color.White;
                        return;
                    }


                    var thisRow = e.Row;
                    var prevRow = gvHLBExecute.Rows[e.Row.DataItemIndex - 1];
                    DataRowView drv = (DataRowView)e.Row.DataItem;
                    // DataRowView drvPrevious = (DataRowView)gvHLBExecute.Rows[e.Row.DataItemIndex - 1].DataItem;
                    // string CurrentVIP = drv["VIP"].ToString();
                    // string PreviousVIP = drvPrevious["VIP"].ToString();
                    Label lblVIPIP = (Label)e.Row.FindControl("lblVIPIP");
                    Label lblPrevVIPIP = (Label)gvHLBExecute.Rows[e.Row.DataItemIndex - 1].FindControl("lblVIPIP");
                    if (lblVIPIP.Text == lblPrevVIPIP.Text)
                    {
                        //e.Row.BackColor = gvHLBExecute.Rows[e.Row.DataItemIndex - 1].Cells[1].BackColor;
                        e.Row.BackColor = gvHLBExecute.Rows[e.Row.DataItemIndex - 1].BackColor;

                    }
                    else
                    {
                        if (gvHLBExecute.Rows[e.Row.DataItemIndex - 1].BackColor == System.Drawing.Color.FromArgb(0xEFF3FB))
                            e.Row.BackColor = System.Drawing.Color.White;
                        else
                            e.Row.BackColor = System.Drawing.Color.FromArgb(0xEFF3FB);
                    }
                    //e.Row.Cells[1].BackColor = (lblVIPIP.Text == lblPrevVIPIP.Text) ? System.Drawing.Color.Red : System.Drawing.Color.Green;
                    //e.Row.Cells[1].BackColor = (thisRow.Cells[2].Text == prevRow.Cells[2].Text) ? System.Drawing.Color.Green : System.Drawing.Color.Red;



                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }
        #endregion

        #endregion

        #region ValidateTab

        protected void btnPostSmokeTest_Click(Object sender, EventArgs e)
        {
            try
            {
                DataTable dtSmokeResult = null;
                txtResult.Visible = false;
                rvValidate.Visible = false;
                lblRvValidate.Text = "";

                ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_4','content_4')</script>");

                //if (!IsPageRefresh)
                //{
                    dtSmokeResult = new DataTable();
                    string strDestPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString();
                    uniqueGUID = Guid.NewGuid().ToString();
                    if (fupValidateExcel.HasFile)
                    {

                        string strName = "";
                        string strFileName = "";
                        if (fupValidateExcel.FileName.Contains(".xlsx"))
                        {
                            strName = fupValidateExcel.FileName.Replace(".xlsx", uniqueGUID);
                            strFileName = strName + ".xlsx";

                        }
                        else if (fupValidateExcel.FileName.Contains(".xls"))
                        {
                            strName = fupValidateExcel.FileName.Replace(".xls", uniqueGUID);
                            strFileName = strName + ".xls";
                        }
                        else
                        {
                        }
                        strServerlist = strDestPath + strFileName;
                        fupValidateExcel.SaveAs(strServerlist);

                        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strServerlist + ";Extended Properties=Excel 12.0;";
                        try
                        {
                            string query = String.Format("select * from [{0}]", "Sheet1$");
                            OleDbDataAdapter daData = new OleDbDataAdapter(query, connectionString);
                            DataSet dsData = new DataSet();
                            daData.Fill(dsData);
                            //if (dsData.Tables[0].Columns.Count == 2)
                            //{
                            if (dsData.Tables[0].Columns[0].ToString() == "ServerName" && dsData.Tables[0].Columns[1].ToString() == "Priority")
                            {
                                try
                                {
                                    uniqueGUID = Guid.NewGuid().ToString();
                                    //   hdnUniqueAccess.Value = uniqueGUID;
                                    // hdnUniqueAccess.Value = uniqueGUID;
                                    rs = new RunbookOperations();
                                    string strRunbookPath = ConfigurationManager.AppSettings["SmokeTestPath"].ToString();


                                    output = rs.StartSmokeTestRunbook(strRunbookPath, strServerlist, txtLogPath.Text, uniqueGUID, "Yes", "Excel","");
                                    //uniqueGUID = "abcd01";
                                    System.Threading.Thread.Sleep(25000);



                                }
                                catch (Exception ex)
                                {

                                    WriteError(ex);

                                }

                                finally
                                {

                                }
                            }
                            else
                            {
                                pnlValidateResult.Visible = false;
                                gvValidateResult.Visible = false;
                                Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('Please Select Valid Excel');</script>");
                            }
                        }
                        catch
                        {

                            Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('ExcelFile Sheet Name sholud be Sheet1');</script>");


                        }


                    }

                    else
                    {
                        rs = new RunbookOperations();
                        string strRunbookPath = ConfigurationManager.AppSettings["SmokeTestPath"].ToString();
                        string[] values = txtValidateServerNames.Text.Split(',');
                        string strTextPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString() + uniqueGUID + ".txt";
                        if (!File.Exists(strTextPath))
                        {
                            using (StreamWriter sr = File.CreateText(strTextPath))
                            {
                                sr.Close();
                            }
                            StreamWriter sw = null;
                           
                                sw = new StreamWriter(strTextPath, false);

                                for (int i = 0; i < values.Length; i++)
                                {
                                    if (values[i].ToString() != "")
                                    {

                                        sw.Write(values[i].ToString());
                                        sw.WriteLine();

                                    }



                                } sw.Close();

                           

                        }

                       // output = rs.StartSmokeTestRunbook(strRunbookPath, strTextPath, txtLogPath.Text, uniqueGUID, "Yes", "Excel","");
                        output = rs.StartSmokeTestRunbook(strRunbookPath, strTextPath, txtLogPath.Text, uniqueGUID, "Yes", "Text", "");
                        System.Threading.Thread.Sleep(25000);
                    }

                    string textfilepath = ConfigurationManager.AppSettings["SmokeTestLogPath"].ToString();
                    string[] filePaths = Directory.GetFiles(textfilepath, "SmokeTestResult_*_" + uniqueGUID + ".txt");
                    filename = textfilepath + "SmokeTestResult_Consolidated_" + uniqueGUID + ".txt";

                    //Read all the text files which has same unique id and prepares a datatable
                    dtSmokeResult = CreateDataTable(filePaths);

                    //Export smoke test result of all teh servers into a text file with unique id.
                    CreateSmokeTestLog(filename, dtSmokeResult);




                    gvValidateResult.DataSource = dtSmokeResult;
                    //dataGridView1.AllowSorting = true;
                    gvValidateResult.DataBind();


                    if (dtSmokeResult.Rows.Count > 0)
                    {
                        pnlValidateResult.Visible = true;
                        gvValidateResult.Visible = true;
                        btnCompareTest.Visible = true;
                    }

                //}
                //else
                //{
                //}
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }



        }
        public DataTable CreateDataTable(string[] filePaths)
        {
            DataTable dtSmokeTest = null;
            try
            {
                dtSmokeTest = new DataTable();
                dtSmokeTest.Columns.Add("ServerName", Type.GetType("System.String"));

                dtSmokeTest.Columns.Add("ServiceName", Type.GetType("System.String"));
                dtSmokeTest.Columns.Add("Service Status", Type.GetType("System.String"));

                foreach (string file in filePaths)
                {
                    //filename = textfilepath + "SmokeTestResult_" + uniqueGUID + ".txt";
                    textData = System.IO.File.ReadAllLines(file);
                    headers = textData[0].Split('-');
                    int length = textData.Length;
                    for (int i = 0; i < length; i++)
                        dtSmokeTest.Rows.Add(textData[i].Split('-'));
                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }

            return dtSmokeTest;
        }


        public void CreateSmokeTestLog(string filename, DataTable dtSmokeTest)
        {
            try
            {
                sw = new StreamWriter(filename, false);

                foreach (DataRow row in dtSmokeTest.Rows)
                {
                    object[] array = row.ItemArray;
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (i == array.Length - 1)
                        {
                            sw.WriteLine(array[i].ToString());

                        }
                        else
                        {
                            sw.Write(array[i].ToString() + "-");
                        }
                    }

                }

                sw.Close();
            }
            catch (Exception ex)
            {
                WriteError(ex);
                sw.Close();
            }


        }

        protected void btnCompareTest_Click(Object sender, EventArgs e)
        {
            DataTable dtCompareTestResult = null;
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_4','content_4')</script>");
            rvValidate.Visible = false;
            txtResult.Visible = false;
            lblRvValidate.Text = "";

            try
            {
                dtCompareTestResult = new DataTable();
                btnPostSmokeTest.Visible = true;
                uniqueGUID = Guid.NewGuid().ToString();

                //  string uniqueGUID = hdnPostuniqueGUID.Value;
                rs = new RunbookOperations();
                string strRunbookPath = ConfigurationManager.AppSettings["CompareSmokeTestLogPath"].ToString();


                output = rs.StartCompareSmokeTestlogRunbook(strRunbookPath, txtLogPath.Text, uniqueGUID);
                //uniqueGUID = "abcd01";
                System.Threading.Thread.Sleep(10000);

                string textfilepath = ConfigurationManager.AppSettings["SmokeTestLogPath"].ToString();
                string filename = textfilepath + "LogComparisionResult_" + uniqueGUID + ".txt";


                string[] textData = System.IO.File.ReadAllLines(filename);
                string[] result = textData[0].Split(':');
                int length = textData.Length;
                DataTable dtValidate = new DataTable();
                dtValidate.Columns.Add(" ", Type.GetType("System.String"));

                dtValidate.Columns.Add("ServerName", Type.GetType("System.String"));

                dtValidate.Columns.Add("ServiceName", Type.GetType("System.String"));
                dtValidate.Columns.Add("Service Status", Type.GetType("System.String"));

                for (int i = 0; i < length; i++)
                {

                    string[] strcolumn = textData[i].Split(':');
                    dtValidate.Rows.Add();
                    //string[] strServers = strcolumn[1].Split('-');
                    if (strcolumn[1].Contains("-"))
                    {

                        string[] strServers = strcolumn[1].Split('-');

                        dtValidate.Rows[i][" "] = strcolumn[0];
                        dtValidate.Rows[i]["ServerName"] = strServers[0];
                        dtValidate.Rows[i]["ServiceName"] = strServers[1];
                        dtValidate.Rows[i]["Service Status"] = strServers[2];


                    }
                    else
                    {
                        dtValidate.Rows[i][" "] = strcolumn[0];
                        dtValidate.Rows[i]["ServerName"] = "No Changes";
                        dtValidate.Rows[i]["ServiceName"] = "No Changes";
                        dtValidate.Rows[i]["Service Status"] = "No Changes";

                    }



                }
                gvValidateResult.DataSource = dtValidate;
                gvValidateResult.DataBind();

                if (dtValidate.Rows.Count > 0)
                {
                    pnlValidateResult.Visible = true;
                    gvValidateResult.Visible = true;

                    if (dtValidate.Rows.Count < 10)
                    {
                        pnlValidateResult.Style.Add("Height", "250px");

                    }
                }
            }

            catch (Exception ex)
            {

                WriteError(ex);

            }
        }


        #region btnValidate_Click

        /// <summary>
        /// Validates the Kbs installed on the servers listed in the excel file
        /// </summary>
        /// <author>sudha gubbala</author>
        /// <createddate>17Oct2012</createddate>
        /// <param name="sender"></param>

        string strValidateExcel = string.Empty;
        public void CallValidateMethod()
        {
            if (cbNumbers.Checked == true)
            {

                lblRvValidate.Text = "";

                // ValidatePopup.Attributes.Add("class", "ValidatePopup");
                pnlValidateResult.Visible = true;
                gvValidateResult.Visible = true;
                rvValidate.Visible = false;
                txtResult.Visible = false;

                if (fupValidateExcel.HasFile)
                {
                    VerifyPatchScript(txtKbValue.Text, hdnExcelPath.Value, "Excel");
                    //PrepareValidateGrid(hdnExcelPath.Value);


                    //PrepareValidateGrid(hdnExcelPath.Value);
                }
                else if (ddlValidateNames.SelectedValue != "0")
                {
                    VerifyPatchScript(txtKbValue.Text, hdnExcelPath.Value, "Group", ddlValidateNames.SelectedItem.Text);
                }
                else
                {
                    string[] values = txtValidateServerNames.Text.Split(',');
                    string strTextPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString() + uniqueGUID + ".txt";
                    if (File.Exists(strTextPath))
                    {
                    }
                    else
                    {
                        // File.CreateText(filePath);
                        using (StreamWriter sr = File.CreateText(strTextPath))
                        {
                            sr.Close();
                        }
                        StreamWriter sw = null;
                        try
                        {
                            sw = new StreamWriter(strTextPath, false);

                            for (int i = 0; i < values.Length; i++)
                            {
                                if (values[i].ToString() != "")
                                {

                                    sw.Write(values[i].ToString());
                                    sw.WriteLine();

                                }



                            } sw.Close();

                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show("Invalid Operation : \n" + ex.ToString(),  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);       
                        }
                        VerifyPatchScript(txtKbValue.Text, strTextPath, "Text");

                        //  PrepareValidateGrid(strTextPath);
                        //  output = rs.StartRunbookWithParameters(strRunbookPath, strTextPath, txtDomainAccName.Text, txtDomainAcctPwd.Text, "3", txtLogPath.Text, "0", "nope", "Preview", uniqueGUID, hdnFileName.Value, "", "Text");
                    }
                }

                //VerifyPatchScript(txtKbValue.Text, hdnExcelPath.Value);

                //PrepareValidateGrid(hdnExcelPath.Value);



            }


            if (cbInstalledUpdates.Checked == true)
            {

                lblRvValidate.Text = "";
                pnlValidateResult.Visible = false;
                gvValidateResult.Visible = false;
                rvValidate.Visible = false;
                txtResult.Visible = true;
                if (fupValidateExcel.HasFile)
                {
                    FindInstalledUpdatesScript(txtStartDate.Text, txtEndDate.Text, hdnExcelPath.Value, "Excel");
                }
                else if (ddlValidateNames.SelectedValue != "0")
                {
                    FindInstalledUpdatesScript(txtStartDate.Text, txtEndDate.Text, hdnExcelPath.Value, "Group", ddlValidateNames.SelectedItem.Text);
                }
                else
                {
                    string[] values = txtValidateServerNames.Text.Split(',');
                    string strTextPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString() + uniqueGUID + ".txt";
                    if (File.Exists(strTextPath))
                    {
                    }
                    else
                    {
                        // File.CreateText(filePath);
                        using (StreamWriter sr = File.CreateText(strTextPath))
                        {
                            sr.Close();
                        }
                        StreamWriter sw = null;
                        try
                        {
                            sw = new StreamWriter(strTextPath, false);

                            for (int i = 0; i < values.Length; i++)
                            {
                                if (values[i].ToString() != "")
                                {

                                    sw.Write(values[i].ToString());
                                    sw.WriteLine();

                                }



                            } sw.Close();

                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show("Invalid Operation : \n" + ex.ToString(),  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);       
                        }
                        FindInstalledUpdatesScript(txtStartDate.Text, txtEndDate.Text, strTextPath, "Text");
                        //  output = rs.StartRunbookWithParameters(strRunbookPath, strTextPath, txtDomainAccName.Text, txtDomainAcctPwd.Text, "3", txtLogPath.Text, "0", "nope", "Preview", uniqueGUID, hdnFileName.Value, "", "Text");
                    }
                }

                //FindInstalledUpdatesScript(txtStartDate.Text, txtEndDate.Text, hdnExcelPath.Value);




            }

            if (cbSimpleUpdate.Checked == true)
            {
                // ValidatePopup.Attributes.Add("class", "rvValidatePopup");
                rvValidate.Visible = true;
                pnlValidateResult.Visible = false;
                gvValidateResult.Visible = false;
                txtResult.Visible = false;

                try
                {
                    //Guid runbookid = new Guid();
                    uniqueGUID = Guid.NewGuid().ToString();
                    hdnCbSimpleUpdate.Value = uniqueGUID;
                    rs = new RunbookOperations();

                    // string strRunbookPath = ConfigurationManager.AppSettings["PatchingPath"].ToString();
                    string strRunbookPath = ConfigurationManager.AppSettings["SUPreviewPath"].ToString();

                    string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
                    string serviceRoot = null;
                    if (fupValidateExcel.HasFile)
                    {
                        lblRvValidate.Text = "Report Name: " + hdnFileName.Value;
                        // output = rs.StartRunbookWithParameters(strRunbookPath, hdnExcelPath.Value, "3", txtLogPath.Text, "0", "nope", "preview", uniqueGUID, hdnFileName.Value, "", "Excel", txtOnlyQFE.Text, txtExcludeQFE.Text, rblBPU.SelectedValue.ToString());

                        output = rs.StartSimpleUpdatePreviewRunbook(strRunbookPath, hdnExcelPath.Value, DateTime.Now.ToString(), uniqueGUID, hdnFileName.Value);
                    }
                    else if (ddlValidateNames.SelectedValue != "0")
                    {
                        output = rs.StartSimpleUpdatePreviewRunbook(strRunbookPath, hdnExcelPath.Value, DateTime.Now.ToString(), uniqueGUID, hdnFileName.Value, "Group", ddlValidateNames.SelectedItem.Text);
                    }
                    else
                    {
                        string[] values = txtValidateServerNames.Text.Split(',');
                        string strTextPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString() + uniqueGUID + ".txt";
                        if (File.Exists(strTextPath))
                        {
                        }
                        else
                        {
                            // File.CreateText(filePath);
                            using (StreamWriter sr = File.CreateText(strTextPath))
                            {
                                sr.Close();
                            }
                            StreamWriter sw = null;
                            try
                            {
                                sw = new StreamWriter(strTextPath, false);

                                for (int i = 0; i < values.Length; i++)
                                {
                                    if (values[i].ToString() != "")
                                    {

                                        sw.Write(values[i].ToString());
                                        sw.WriteLine();

                                    }



                                } sw.Close();

                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show("Invalid Operation : \n" + ex.ToString(),  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);       
                            }

                            // output = rs.StartRunbookWithParameters(strRunbookPath, strTextPath, "3", txtLogPath.Text, "0", "nope", "preview", uniqueGUID, hdnFileName.Value, "", "Text", txtOnlyQFE.Text, txtExcludeQFE.Text, rblBPU.SelectedValue.ToString());
                            output = rs.StartSimpleUpdatePreviewRunbook(strRunbookPath, strTextPath, DateTime.Now.ToString(), uniqueGUID, hdnFileName.Value);
                        }
                    }
                    if (output != null)
                    {
                        // rvValidate.Visible = false;
                        rvValidate.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                        string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
                        string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                        path = ConfigurationManager.AppSettings["SimpleUpdateReportPath"].ToString();
                        rvValidate.ServerReport.ReportServerUrl = new Uri(@url);

                        rvValidate.ServerReport.ReportPath = ReportViewerPath + path;
                        rvValidate.ShowParameterPrompts = false;
                        rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", uniqueGUID));
                        rvValidate.ShowPrintButton = true;
                        rvValidate.ServerReport.Refresh();
                        //TimerValidate.Interval = Convert.ToInt32(ddlRefresh.SelectedValue.ToString());
                        TimerValidate.Enabled = true;
                        //TimerValidate.Interval = Convert.ToInt32(ddlValidateRefresh.SelectedValue.ToString());
                        TimerValidate.Interval = 300000;
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
            if (cbMSNPatch.Checked == true)
            {
                // ValidatePopup.Attributes.Add("class", "rvValidatePopup");
                rvValidate.Visible = true;
                pnlValidateResult.Visible = false;
                gvValidateResult.Visible = false;
                txtResult.Visible = false;

                try
                {
                    //Guid runbookid = new Guid();
                    uniqueGUID = Guid.NewGuid().ToString();
                    hdnCbMSNPatch.Value = uniqueGUID;
                    rs = new RunbookOperations();
                    string strRunbookPath = ConfigurationManager.AppSettings["ValidatePath"].ToString();
                    string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
                    string serviceRoot = null;

                    if (fupValidateExcel.HasFile)
                    {
                        lblRvValidate.Text = "Report Name: " + hdnFileName.Value;
                        output = rs.StartValidateRunbook(strRunbookPath, hdnExcelPath.Value, txtLogPath.Text, uniqueGUID, hdnFileName.Value, "Excel");
                    }
                    else if (ddlValidateNames.SelectedValue != "0")
                    {
                        output = rs.StartValidateRunbook(strRunbookPath, hdnExcelPath.Value, txtLogPath.Text, uniqueGUID, hdnFileName.Value, "Group", ddlValidateNames.SelectedItem.Text);
                    }
                    else
                    {

                        string[] values = txtValidateServerNames.Text.Split(',');
                        string strTextPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString() + uniqueGUID + ".txt";
                        if (File.Exists(strTextPath))
                        {
                        }
                        else
                        {
                            // File.CreateText(filePath);
                            using (StreamWriter sr = File.CreateText(strTextPath))
                            {
                                sr.Close();
                            }
                            StreamWriter sw = null;
                            try
                            {
                                sw = new StreamWriter(strTextPath, false);

                                for (int i = 0; i < values.Length; i++)
                                {
                                    if (values[i].ToString() != "")
                                    {

                                        sw.Write(values[i].ToString());
                                        sw.WriteLine();

                                    }



                                } sw.Close();

                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show("Invalid Operation : \n" + ex.ToString(),  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);       
                            }

                            output = rs.StartValidateRunbook(strRunbookPath, strTextPath, txtLogPath.Text, uniqueGUID, hdnFileName.Value, "Text");
                        }
                    }
                    // rvValidate.Visible = false;

                    if (output != null)
                    {
                        rvValidate.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                        string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
                        string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                        path = ConfigurationManager.AppSettings["MSNPatchValidationReportPath"].ToString();
                        rvValidate.ServerReport.ReportServerUrl = new Uri(@url);
                        rvValidate.ServerReport.ReportPath = ReportViewerPath + path;
                        rvValidate.ShowParameterPrompts = false;
                        rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", uniqueGUID));
                        rvValidate.ShowPrintButton = true;
                        rvValidate.ServerReport.Refresh();

                        TimerValidate.Enabled = true;
                        //TimerValidate.Interval = Convert.ToInt32(ddlValidateRefresh.SelectedValue.ToString());
                        TimerValidate.Interval = 300000;
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




            if (cbODP.Checked == true)
            {
                //  ValidatePopup.Attributes.Add("class", "rvValidatePopup");
                rvValidate.Visible = true;
                pnlValidateResult.Visible = false;
                gvValidateResult.Visible = false;

                txtResult.Visible = false;
                try
                {
                    //Guid runbookid = new Guid();
                    uniqueGUID = Guid.NewGuid().ToString();
                    hdnCbODP.Value = uniqueGUID;
                    rs = new RunbookOperations();
                    //runbookid = new Guid(ConfigurationManager.AppSettings["ValidateGUID"].ToString());
                    string strRunbookPath = ConfigurationManager.AppSettings["PatchingPath"].ToString();
                    string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
                    //  string serviceRoot = null;
                    if (fupValidateExcel.HasFile)
                    {
                        lblRvValidate.Text = "Report Name: " + hdnFileName.Value;
                        output = rs.StartRunbookWithParameters(strRunbookPath, hdnExcelPath.Value, "2", txtLogPath.Text, "0", "nope", "Scan", uniqueGUID, hdnFileName.Value, "", "Excel", txtOnlyQFE.Text, txtExcludeQFE.Text, rblBPU.SelectedValue.ToString());
                    }
                    else if (ddlValidateNames.SelectedValue != "0")
                    {
                        output = rs.StartRunbookWithParameters(strRunbookPath, hdnExcelPath.Value, "2", txtLogPath.Text, "0", "nope", "Scan", uniqueGUID, hdnFileName.Value, "", "Group", txtOnlyQFE.Text, txtExcludeQFE.Text, rblBPU.SelectedValue.ToString(), ddlValidateNames.SelectedItem.Text);
                    }
                    else
                    {
                        string[] values = txtValidateServerNames.Text.Split(',');
                        string strTextPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString() + uniqueGUID + ".txt";
                        
                        if (File.Exists(strTextPath))
                        {
                        }
                        else
                        {
                            // File.CreateText(filePath);
                            using (StreamWriter sr = File.CreateText(strTextPath))
                            {
                                sr.Close();
                            }
                            StreamWriter sw = null;
                            try
                            {
                                sw = new StreamWriter(strTextPath, false);

                                for (int i = 0; i < values.Length; i++)
                                {
                                    if (values[i].ToString() != "")
                                    {

                                        sw.Write(values[i].ToString());
                                        sw.WriteLine();

                                    }



                                } sw.Close();

                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show("Invalid Operation : \n" + ex.ToString(),  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);       
                            }

                            output = rs.StartRunbookWithParameters(strRunbookPath, strTextPath, "2", txtLogPath.Text, "0", "nope", "Scan", uniqueGUID, hdnFileName.Value, "", "Excel", txtOnlyQFE.Text, txtExcludeQFE.Text, rblBPU.SelectedValue.ToString());
                        }
                    }
                    if (output != null)
                    {
                        rvValidate.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                        string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
                        string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                        path = ConfigurationManager.AppSettings["ODPValidationReportPath"].ToString();
                        rvValidate.ServerReport.ReportServerUrl = new Uri(@url);
                        rvValidate.ServerReport.ReportPath = ReportViewerPath + path;
                        rvValidate.ShowParameterPrompts = false;
                        rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", uniqueGUID));
                        rvValidate.ShowPrintButton = true;
                        rvValidate.ServerReport.Refresh();

                        TimerValidate.Enabled = true;
                        //  TimerValidate.Interval = Convert.ToInt32(ddlValidateRefresh.SelectedValue.ToString());
                        TimerValidate.Interval = 300000;
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

        protected void btnValidate_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_4','content_4')</script>");
            try
            {
                //if (!IsPageRefresh)
                //{
                    string strDestPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString();
                    rvValidate.Visible = false;
                    uniqueGUID = Guid.NewGuid().ToString();
                    string strName = "";
                    string strFileName = "";

                    if (fupValidateExcel.HasFile)
                    {


                        if (fupValidateExcel.FileName.Contains(".xlsx"))
                        {
                            strName = fupValidateExcel.FileName.Replace(".xlsx", uniqueGUID);
                            strFileName = strName + ".xlsx";

                        }
                        else if (fupValidateExcel.FileName.Contains(".xls"))
                        {
                            strName = fupValidateExcel.FileName.Replace(".xls", uniqueGUID);
                            strFileName = strName + ".xls";
                        }
                        else
                        {
                        }
                        strValidateExcel = strDestPath + strFileName;
                        hdnExcelPath.Value = strValidateExcel;
                        fupValidateExcel.SaveAs(strValidateExcel);
                        // hdnFileName.Value = fupExcel.FileName;
                        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strValidateExcel + ";Extended Properties=Excel 12.0;";
                        try
                        {
                            string query = String.Format("select * from [{0}]", "Sheet1$");
                            OleDbDataAdapter daData = new OleDbDataAdapter(query, connectionString);
                            DataSet dsData = new DataSet();
                            daData.Fill(dsData);
                            //if (dsData.Tables[0].Columns.Count == 2)
                            //{
                            if (dsData.Tables[0].Columns[0].ToString() == "ServerName" && dsData.Tables[0].Columns[1].ToString() == "Priority")
                            {
                                string strheader = "";
                                if (fupValidateExcel.FileName.Contains(".xlsx"))
                                {
                                    strheader = fupValidateExcel.FileName.Replace(".xlsx", "");
                                }
                                else if (fupValidateExcel.FileName.Contains(".xls"))
                                {
                                    strheader = fupValidateExcel.FileName.Replace(".xls", "");
                                }

                                hdnFileName.Value = strheader;

                                CallValidateMethod();
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('Please Select Valid Excel');</script>");
                            }
                        }
                        catch
                        {

                            Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('ExcelFile Sheet Name sholud be Sheet1');</script>");


                        }
                    }
                    else
                    {
                        CallValidateMethod();
                    }
                    // Page.ClientScript.RegisterStartupScript(this.GetType(), "CallResult", "loadValidate();", true);

                //}
                //else
                //{

                //}
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


        public string GetValidateKbStatus(string strUniqueID)
        {
            SqlConnection conn = null;
            string strconnectionString = string.Empty;
            SqlCommand command = null;
            string strRunbookStatus = string.Empty;

            try
            {
                //strconnectionString = ConfigurationManager.ConnectionStrings["TK5_ConnectionString"].ConnectionString;

                strconnectionString = ConfigurationManager.ConnectionStrings["DB_ConnectionString"].ConnectionString;

                conn = new SqlConnection(strconnectionString);
                conn.Open();
                command = new SqlCommand("usp_GetValdiateKBStatus", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UniqueID", strUniqueID);
                command.Parameters.Add("@Status", SqlDbType.VarChar, 20);
                command.Parameters["@Status"].Direction = ParameterDirection.Output;


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

        public void PrepareValidateGrid(string strUniqueID)
        {
            #region Comment
            //string resultpath = String.Empty;
            //if (path.Contains(".xlsx"))
            //{
            //    resultpath = path.Replace(".xlsx", "Results.txt");
            //}
            //else if (path.Contains(".xls"))
            //{
            //    resultpath = path.Replace(".xls", "Results.txt");
            //}
            //string serverspath = String.Empty;
            //if (path.Contains(".xlsx"))
            //{
            //    serverspath = path.Replace(".xlsx", ".txt");
            //}
            //else if (path.Contains(".xls"))
            //{
            //    serverspath = path.Replace(".xls", ".txt");
            //}



            //if (path.Contains(".txt"))
            //{
            //    resultpath = path.Replace(".txt", "Results.txt");
            //}

            //if (path.Contains(".txt"))
            //{
            //    serverspath = path.Replace(".txt", ".txt");
            //}



            //string[] values = txtKbValue.Text.Split(',');
            //int count = values.Length;
            //DataTable dtValidate = new DataTable();
            //dtValidate.Columns.Add("ServerName", Type.GetType("System.String"));
            //for (int i = 0; i < count; i++)
            //{
            //    dtValidate.Columns.Add(values[i].ToString(), Type.GetType("System.String"));
            //}
            //dtValidate.Columns.Add("Rebooted", Type.GetType("System.String"));
            //DataRow dr = null;
            //using (FileStream fs = new FileStream(resultpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //{

            //    StreamReader server = new StreamReader(serverspath);
            //    StreamReader result = new StreamReader(fs);
            //    int serversCount = 0;
            //    int totalCount = 0;
            //    string servers = String.Empty;
            //    string total = String.Empty;
            //    while ((servers = server.ReadLine()) != null)
            //    {
            //        serversCount++;
            //    }
            //    while (serversCount != totalCount)
            //    {
            //        while ((total = result.ReadLine()) != null)
            //        {
            //            totalCount++;
            //        }
            //    }

            //    using (StreamReader sr = new StreamReader(new FileStream(resultpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            //    {
            //        String line = sr.ReadToEnd();

            //        string[] val = line.Split('\n');

            //        foreach (string str in val)
            //        {
            //            if (str != "\r" && str != "")
            //            {
            //                dr = dtValidate.NewRow();
            //                if (str.Contains("@"))
            //                {
            //                    string[] final = str.Split('@');
            //                    for (int j = 0; j < final.Length; j++)
            //                    {
            //                        if (j == 0 || j == final.Length - 1)
            //                        {
            //                            dr[j] = final[j];
            //                        }
            //                        else
            //                        {
            //                            if (final[j].Contains("&"))
            //                            {
            //                                string[] flag = final[j].Split('&');

            //                                dr[j] = flag[1];
            //                            }
            //                        }
            //                        //dr[1] = final[1];
            //                        //dr[2] = final[2];
            //                        //dr[3] = final[3];
            //                    }
            //                    //for (int i = 0; i < count; i++)
            //                    //{
            //                    //    //dtValidate.Columns.Add(values[i].ToString(), Type.GetType("System.String"));
            //                    //    if (values[i].ToString() == final[1].ToString())
            //                    //    {
            //                    //        dr[values[i].ToString()] = final[2];
            //                    //    }

            //                    //}

            //                    dtValidate.Rows.Add(dr);

            //                }
            //            }
            //        }
            //    }

            //    gvValidateResult.DataSource = dtValidate;
            //    gvValidateResult.DataBind();
            //}


            #endregion


            int count = 1;
            System.Threading.Thread.Sleep(10000);
            // TimerCheckAccessValues(UniqueID);
            string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
            connectionString = ConfigurationManager.ConnectionStrings["DB_ConnectionString"].ConnectionString;



            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = null;
            DataSet ds = new DataSet();
            while (count != 0)
            {


                connectionString = ConfigurationManager.ConnectionStrings["DB_ConnectionString"].ConnectionString;
                conn = new SqlConnection(connectionString);
                conn.Open();
                command = new SqlCommand("[usp_GetKBNumberStatus]", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UniqueID", strUniqueID);
                count = (int)command.ExecuteScalar();

                conn.Close();


            }
            if (count == 0)
            {

                sqlConnection.Open();
                command = new SqlCommand("usp_GetValdiateKBDetails", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UniqueID", strUniqueID);
                //dr = command.ExecuteReader();
                dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(ds);
                DataTable dtValidate = new DataTable();
                string[] strKBValues = txtKbValue.Text.Split(',');
                int KBcount = strKBValues.Length;
                dtValidate.Columns.Add("ServerName", Type.GetType("System.String"));
                for (int k = 0; k < KBcount; k++)
                {
                    dtValidate.Columns.Add(strKBValues[k].ToString(), Type.GetType("System.String"));
                }
                dtValidate.Columns.Add("Rebooted", Type.GetType("System.String"));
                if (ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string strKbDetails = ds.Tables[0].Rows[i][1].ToString();
                        string[] values = strKbDetails.Split('@');
                        DataRow dr = dtValidate.NewRow();
                        dr[0] = ds.Tables[0].Rows[i][0].ToString();
                        for (int j = 0; j < KBcount; j++)
                        {
                            for (int l = 0; l < values.Length; l++)
                            {
                                string[] kb = values[l].Split('&');
                                //  dr[j] = kb[1].ToString();
                                //  dr[j+1] = kb[1].ToString();
                                dr[strKBValues[j]] = kb[1].ToString();
                            }

                        }
                        //for (int j = 1; j < KBcount; j++)
                        //{

                        //for(int l=0;l<values.Length;l++)
                        //{
                        //    string[] kb=values[l].Split('&');
                        //    dr[j] = kb[1].ToString();
                        //}

                        //}
                        dr[KBcount + 1] = ds.Tables[0].Rows[i][2].ToString();
                        //dr[2] = ds.Tables[0].Rows[i][2].ToString();
                        //for (int j = 0; j < values.Length; j++) 
                        //{

                        //    //if (j == 0 || j == values.Length - 1)
                        //    //{
                        //    //    dr[j] = values[j];
                        //    //}
                        //    //else
                        //    //{
                        //        if (values[j].Contains("&"))
                        //        {
                        //            string[] flag1 = values[j].Split('&');

                        //            dr[j] = flag1[1];
                        //        }
                        //   // }

                        //}
                        //dr[values.Length + 1] = ds.Tables[0].Rows[i][0].ToString();
                        //dr[values.Length + 2] = ds.Tables[0].Rows[i][2].ToString();
                        dtValidate.Rows.Add(dr);


                    }



                    gvValidateResult.DataSource = dtValidate;

                    gvValidateResult.DataBind();
                }

                else
                {
                    gvValidateResult.DataSource = null;
                    gvValidateResult.DataBind();
                }
            }
            //string strValidationstatus = GetValidateKbStatus(strUniqueID);

            //if (strValidationstatus == "Completed")
            //{
            //    TimerValidate.Enabled = false;
            //}
            //else
            //{
            //    TimerValidate.Enabled = false;
            //}

        }



        #endregion

        public void VerifyPatchScript(string KBNumbers, string serversList, string check)
        {
            try
            {

                uniqueGUID = Guid.NewGuid().ToString();
                hdnKBUniqueValidate.Value = uniqueGUID;
                rs = new RunbookOperations();
                string strRunbookPath = ConfigurationManager.AppSettings["VerifyPatchPath"].ToString();
                string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();

                output = rs.StartValidateVerifyPatchRunbook(strRunbookPath, serversList, KBNumbers, uniqueGUID, hdnFileName.Value, check);

                if (output != null)
                {
                    System.Threading.Thread.Sleep(25000);
                    TimerValidate.Enabled = true;
                    //PrepareValidateGrid(serversList);
                    PrepareValidateGrid(hdnKBUniqueValidate.Value);
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

        public void VerifyPatchScript(string KBNumbers, string serversList, string check, string GroupName)
        {
            try
            {
                uniqueGUID = Guid.NewGuid().ToString();
                hdnKBUniqueValidate.Value = uniqueGUID;
                rs = new RunbookOperations();
                string strRunbookPath = ConfigurationManager.AppSettings["VerifyPatchPath"].ToString();
                string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();

                output = rs.StartValidateVerifyPatchRunbook(strRunbookPath, serversList, KBNumbers, uniqueGUID, hdnFileName.Value, check, GroupName);

                if (output != null)
                {
                    System.Threading.Thread.Sleep(25000);
                    TimerValidate.Enabled = true;
                    PrepareValidateGrid(hdnKBUniqueValidate.Value);
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

        #region ValidateFindUpdatesGrid

        /// <summary>
        /// Dynamically loading grid 
        /// <author>Sudha Gubbala</author>
        /// <CreatedDate>28Nov2012</CreatedDate>
        /// </summary>

        public void PrepareFindUpdatesGrid(string path, string strUniqueID)
        {
            try
            {
                string resultpath = String.Empty;
                int count = 1;
                System.Threading.Thread.Sleep(10000);

                while (count != 0)
                {

                    // connectionString = ConfigurationManager.ConnectionStrings["TK5_ConnectionString"].ConnectionString;

                    connectionString = ConfigurationManager.ConnectionStrings["DB_ConnectionString"].ConnectionString;

                    SqlConnection sqlConnection1 = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "select count(*) from dbo.FindInstalledUpdates (nolock) where Status in ('InProgress','') and UniqueID='" + strUniqueID + "'";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnection1;
                    sqlConnection1.Open();
                    count = (int)cmd.ExecuteScalar();
                    // Data is accessible through the DataReader object here.
                    sqlConnection1.Close();
                }



                if (path.Contains(".xlsx"))
                {
                    resultpath = path.Replace(".xlsx", "InstallUpdateResults.txt");
                }
                else if (path.Contains(".xls"))
                {
                    resultpath = path.Replace(".xls", "InstallUpdateResults.txt");
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


                if (path.Contains(".txt"))
                {
                    resultpath = path.Replace(".txt", "InstallUpdateResults.txt");
                }

                if (path.Contains(".txt"))
                {
                    serverspath = path.Replace(".txt", ".txt");
                }

                //string[] values = txtKbValue.Text.Split(',');
                // int count = values.Length;
                //DataTable dtValidate = new DataTable();
                //dtValidate.Columns.Add("ServerName", Type.GetType("System.String"));

                //dtValidate.Columns.Add("Description", Type.GetType("System.String"));
                //dtValidate.Columns.Add("HotFixID", Type.GetType("System.String"));
                //dtValidate.Columns.Add("InstalledBy", Type.GetType("System.String"));
                //dtValidate.Columns.Add("InstalledOn", Type.GetType("System.String"));
                //DataRow dr = null;


                using (StreamReader sr = new StreamReader(new FileStream(resultpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                {
                    String line = sr.ReadToEnd();
                    txtResult.Visible = true;
                    pnlValidateResult.Visible = false;
                    gvValidateResult.Visible = false;
                    txtResult.Text = line;



                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }


        }



        #endregion

        public void FindInstalledUpdatesScript(string strStartDate, string strEndDate, string serversList, string check)
        {
            try
            {
                //Guid runbookid = new Guid();

                uniqueGUID = Guid.NewGuid().ToString();
                rs = new RunbookOperations();
                string strRunbookPath = ConfigurationManager.AppSettings["FindUpdatesPath"].ToString();
                string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
                //string serviceRoot = null;
                //if (flag == "Tk5stosrv03")
                //{
                //    serviceRoot = ConfigurationManager.AppSettings["Tk5stosrv03Orchestrator"].ToString();
                //}

                //if (flag == "Tk3OrchSQL01")
                //{
                //    serviceRoot = ConfigurationManager.AppSettings["Tk3OrchSQL01Orchestrator"].ToString();
                //}

                output = rs.StartFindUpdatesRunbook(strRunbookPath, serversList, strStartDate, strEndDate, uniqueGUID, hdnFileName.Value, check);

                //  PrepareFindUpdatesGrid(hdnExcelPath.Value, uniqueGUID);


                if (output != null)
                {
                    System.Threading.Thread.Sleep(25000);
                    PrepareFindUpdatesGrid(serversList, uniqueGUID);
                    // TimerValidate.Enabled = true;
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

        public void FindInstalledUpdatesScript(string strStartDate, string strEndDate, string serversList, string check, string GroupName)
        {
            try
            {
                uniqueGUID = Guid.NewGuid().ToString();
                rs = new RunbookOperations();
                string strRunbookPath = ConfigurationManager.AppSettings["FindUpdatesPath"].ToString();
                string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();

                output = rs.StartFindUpdatesRunbook(strRunbookPath, serversList, strStartDate, strEndDate, uniqueGUID, hdnFileName.Value, check, GroupName);
                
                if (output != null)
                {
                    System.Threading.Thread.Sleep(25000);
                    PrepareFindUpdatesGrid(serversList, uniqueGUID);
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
        
        #region Validate Timer Tick event
        protected void TimerValidate_Tick(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
            string strValidationstatus = string.Empty;
            string strPatchOption = string.Empty;
            string strUniqueID = string.Empty;
            string strKBnumberStatus = string.Empty;


            try
            {
                if (cbMSNPatch.Checked == true)
                {



                    rvValidate.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                    //rvValidate.ServerReport.ReportServerUrl = new Uri(@"http://tk5stosrv03/ReportServer");
                    //rvValidate.ServerReport.ReportPath = "/PatchingLogs_PreProd/MSNPatchValidationReport";
                    string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
                    string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                    path = ConfigurationManager.AppSettings["MSNPatchValidationReportPath"].ToString();
                    rvValidate.ServerReport.ReportServerUrl = new Uri(@url);
                    rvValidate.ServerReport.ReportPath = ReportViewerPath + path;
                    rvValidate.ShowParameterPrompts = false;
                    rvValidate.ShowPrintButton = true;
                    rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", hdnCbMSNPatch.Value));


                    rvValidate.ServerReport.Refresh();
                    strPatchOption = "MSNPatch";
                    strUniqueID = hdnCbMSNPatch.Value;

                }
                if (cbSimpleUpdate.Checked == true)
                {
                    rvValidate.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                    //rvValidate.ServerReport.ReportServerUrl = new Uri(@"http://tk5stosrv03/ReportServer");
                    //rvValidate.ServerReport.ReportPath = "/PatchingLogs_PreProd/SimpleUpdateValidationReport";
                    string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
                    string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                    path = ConfigurationManager.AppSettings["SimpleUpdateReportPath"].ToString();
                    rvValidate.ServerReport.ReportServerUrl = new Uri(@url);
                    rvValidate.ServerReport.ReportPath = ReportViewerPath + path;
                    rvValidate.ShowParameterPrompts = false;
                    rvValidate.ShowPrintButton = true;
                    rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", hdnCbSimpleUpdate.Value));
                    rvValidate.ServerReport.Refresh();
                    strPatchOption = "SimpleUpdate";
                    strUniqueID = hdnCbSimpleUpdate.Value;

                }
                if (cbODP.Checked == true)
                {
                    rvValidate.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                    //rvValidate.ServerReport.ReportServerUrl = new Uri(@"http://tk5stosrv03/ReportServer");
                    //rvValidate.ServerReport.ReportPath = "/PatchingLogs_PreProd/ODPValidationReport";
                    string url = ConfigurationManager.AppSettings["ReportServerURL"].ToString();
                    string ReportViewerPath = ConfigurationManager.AppSettings["ReportViewerPath"].ToString();
                    path = ConfigurationManager.AppSettings["ODPValidationReportPath"].ToString();
                    rvValidate.ServerReport.ReportServerUrl = new Uri(@url);
                    rvValidate.ServerReport.ReportPath = ReportViewerPath + path;
                    rvValidate.ShowParameterPrompts = false;
                    rvValidate.ShowPrintButton = true;
                    rvValidate.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UniqueID", hdnCbODP.Value));
                    rvValidate.ServerReport.Refresh();
                    strPatchOption = "ODP";
                    strUniqueID = hdnCbODP.Value;
                }
                if (cbNumbers.Checked == true)
                {
                    //  PrepareValidateGrid(hdnKBUniqueValidate.Value);
                }
                strKBnumberStatus = GetValidateKbStatus(hdnKBUniqueValidate.Value);
                if (strKBnumberStatus == "Completed")
                {
                    TimerValidate.Enabled = false;
                }
                if (cbInstalledUpdates.Checked == true)
                {
                    // PrepareFindUpdatesGrid(hdnExcelPath.Value, uniqueGUID);
                }

                //  strValidationstatus = DataAccessLayer.get(strUniqueID, strPatchOption, "Validate");

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

        #endregion

        #region ReportsTab


        protected void btnPlanSummaryReports_Click(object sender, EventArgs e)
        {
            try
            {
                // rvDeployment.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_5','content_5')</script>");

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
                ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_5','content_5')</script>");
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
                ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_5','content_5')</script>");
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
                ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_5','content_5')</script>");
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

        #region ExportToexcel
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
            string strDestPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString();
            var outputFileName = strDestPath + "Report.xlsx";
            //  DataTable dtExecutionDetails = DataAccessLayer.GetSUExecutionSummary(hdnClusterExecute.Value);
            DataTable dtExecutionDetails = new DataTable();
            if (ViewState["ListingInfo"] != null)
            {
                dtExecutionDetails = (DataTable)ViewState["ListingInfo"];
                //  dtExecutionDetails = (DataTable)gvClusterExecute.DataSource;
                List<ListInfo> details = new List<ListInfo>();
                if (ddlScenario.SelectedValue.ToString() == "1" || ddlScenario.SelectedValue == "3")
                {
                    var convertedList = (from rw in dtExecutionDetails.AsEnumerable()
                                         select new ListInfo()
                                         {
                                             ServerName = Convert.ToString(rw["NodeName"]),
                                             ClusterType = Convert.ToString(rw["ClusterType"]),
                                             NodeType = Convert.ToString(rw["NodeType"]),
                                             PatchTool = Convert.ToString(rw["PatchTool"]),
                                             PatchResult = Convert.ToString(rw["PatchResult"]),
                                             ServerOnline = Convert.ToString(rw["ServerOnline"]),
                                             ServerUpTime = Convert.ToString(rw["ServerUpTime"]),
                                             PatchStatus = Convert.ToString(rw["PatchStatus"]),
                                             RunStatus = Convert.ToString(rw["RunStatus"])

                                         }).ToList();
                    ExportDataTable(convertedList, outputFileName);
                }
                else if (ddlScenario.SelectedValue.ToString() == "2")
                {
                    var convertedList = (from rw in dtExecutionDetails.AsEnumerable()
                                         select new ListInfo()
                                         {
                                             ServerName = Convert.ToString(rw["ServerName"]),
                                             ClusterType = Convert.ToString(rw["ClusterType"]),
                                             VIP = Convert.ToString(rw["VIP"]),
                                             NodeIP = Convert.ToString(rw["NodeIP"]),
                                             Port = Convert.ToString(rw["Port"]),
                                             PatchTool = Convert.ToString(rw["PatchTool"]),
                                             PatchResult = Convert.ToString(rw["PatchResult"]),
                                             ServerOnline = Convert.ToString(rw["ISOnline"]),
                                             ServerUpTime = Convert.ToString(rw["ServerUpTime"]),
                                             PatchStatus = Convert.ToString(rw["PatchStatus"]),
                                             RunStatus = Convert.ToString(rw["RunStatus"])

                                         }).ToList();
                    ExportHLBDataTable(convertedList, outputFileName);
                }
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", System.IO.Path.GetFileName(outputFileName)));
                Response.WriteFile(outputFileName);
                Response.Flush();
                Response.End();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('No data To Export');</script>");
            }


            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Customers.xls"));
            //Response.ContentType = "application/ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter htw = new HtmlTextWriter(sw);
            //gvClusterExecute.AllowPaging = false;
            //gvClusterExecute.DataBind();
            ////Change the Header Row back to white color
            //gvClusterExecute.HeaderRow.Style.Add("background-color", "#FFFFFF");
            ////Applying stlye to gridview header cells
            //for (int i = 0; i < gvClusterExecute.HeaderRow.Cells.Count; i++)
            //{
            //    gvClusterExecute.HeaderRow.Cells[i].Style.Add("background-color", "#507CD1");
            //}
            //int j = 1;
            ////This loop is used to apply stlye to cells based on particular row
            //foreach (GridViewRow gvrow in gvClusterExecute.Rows)
            //{
            //    //gvrow.BackColor = Color.White;
            //    if (j <= gvClusterExecute.Rows.Count)
            //    {
            //        if (j % 2 != 0)
            //        {
            //            for (int k = 0; k < gvrow.Cells.Count; k++)
            //            {
            //                gvrow.Cells[k].Style.Add("background-color", "#EFF3FB");
            //            }
            //        }
            //    }
            //    j++;
            //}

            //gvClusterExecute.RenderControl(htw);
            //Response.Write(sw.ToString());
            //Response.End();
        }
        [Serializable]
        public class ListInfo
        {

            public string ServerName { get; set; }
            public string ClusterType { get; set; }
            public string NodeType { get; set; }
            public string PatchTool { get; set; }
            public string PatchResult { get; set; }
            public string ServerOnline { get; set; }
            public string ServerUpTime { get; set; }
            public string PatchStatus { get; set; }
            public string RunStatus { get; set; }
            public string VIP { get; set; }
            public string NodeIP { get; set; }
            public string Port { get; set; }


        }
        public void ExportHLBDataTable(List<ListInfo> data, string exportFile)
        {
            //create the empty spreadsheet template and save the file
            //using the class generated by the Productivity tool
            var excelDocument = new Excel();
            excelDocument.CreatePackage(exportFile);

            //populate the data into the spreadsheet
            using (var spreadsheet = SpreadsheetDocument.Open(exportFile, true))
            {
                var workbook = spreadsheet.WorkbookPart;
                //create a reference to Sheet1
                var worksheet = workbook.WorksheetParts.Last();
                var sdata = worksheet.Worksheet.GetFirstChild<SheetData>();

                //add column names to the first row
                var header = new Row { RowIndex = (UInt32)1 };
                header.AppendChild(createTextCell(1, 1, "ServerName"));
                header.AppendChild(createTextCell(2, 1, "ClusterType"));
                header.AppendChild(createTextCell(3, 1, "VIP"));
                header.AppendChild(createTextCell(4, 1, "NodeIP"));
                header.AppendChild(createTextCell(5, 1, "Port"));
                header.AppendChild(createTextCell(6, 1, "PatchTool"));
                header.AppendChild(createTextCell(7, 1, "PatchResult"));
                header.AppendChild(createTextCell(8, 1, "ServerOnline"));
                header.AppendChild(createTextCell(9, 1, "ServerUpTime"));
                header.AppendChild(createTextCell(10, 1, "PatchStatus"));
                header.AppendChild(createTextCell(11, 1, "RunStatus"));


                sdata.AppendChild(header);
                //loop through each data row
                foreach (var item in data)
                {
                    sdata.AppendChild(createHLBContentRow(item, data.IndexOf(item) + 2));
                }
            }
        }
        public void ExportDataTable(List<ListInfo> data, string exportFile)
        {
            //create the empty spreadsheet template and save the file
            //using the class generated by the Productivity tool
            var excelDocument = new Excel();
            excelDocument.CreatePackage(exportFile);

            //populate the data into the spreadsheet
            using (var spreadsheet = SpreadsheetDocument.Open(exportFile, true))
            {
                var workbook = spreadsheet.WorkbookPart;
                //create a reference to Sheet1
                var worksheet = workbook.WorksheetParts.Last();
                var sdata = worksheet.Worksheet.GetFirstChild<SheetData>();

                //add column names to the first row
                var header = new Row { RowIndex = (UInt32)1 };
                header.AppendChild(createTextCell(1, 1, "ServerName"));
                header.AppendChild(createTextCell(2, 1, "ClusterType"));
                header.AppendChild(createTextCell(3, 1, "NodeType"));
                header.AppendChild(createTextCell(4, 1, "PatchTool"));
                header.AppendChild(createTextCell(5, 1, "PatchResult"));
                header.AppendChild(createTextCell(6, 1, "ServerOnline"));
                header.AppendChild(createTextCell(7, 1, "ServerUpTime"));
                header.AppendChild(createTextCell(8, 1, "PatchStatus"));
                header.AppendChild(createTextCell(9, 1, "RunStatus"));


                sdata.AppendChild(header);
                //loop through each data row
                foreach (var item in data)
                {
                    sdata.AppendChild(createContentRow(item, data.IndexOf(item) + 2));
                }
            }
        }

        private Cell createTextCell(int columnIndex, int rowIndex, object cellValue)
        {
            var cell = new Cell
            {
                DataType = CellValues.InlineString,
                CellReference = getColumnName(columnIndex) + rowIndex,


            };

            var inlineString = new InlineString();
            var t = new Text { Text = cellValue.ToString() };

            inlineString.AppendChild(t);
            cell.AppendChild(inlineString);


            return cell;
        }
        private string getColumnName(int columnIndex)
        {
            var dividend = columnIndex;
            var columnName = String.Empty;
            while (dividend > 0)
            {
                var modifier = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modifier) + columnName;
                dividend = (dividend - modifier) / 26;
            }
            return columnName;
        }
        private Row createContentRow(ListInfo item, int rowIndex)
        {
            var row = new Row

            {
                RowIndex = (UInt32)rowIndex
            };
            row.AppendChild(createTextCell(1, rowIndex, item.ServerName));
            row.AppendChild(createTextCell(2, rowIndex, item.ClusterType));
            row.AppendChild(createTextCell(3, rowIndex, item.NodeType));
            row.AppendChild(createTextCell(4, rowIndex, item.PatchTool));
            row.AppendChild(createTextCell(5, rowIndex, item.PatchResult));
            row.AppendChild(createTextCell(6, rowIndex, item.ServerOnline));
            row.AppendChild(createTextCell(7, rowIndex, item.ServerUpTime));
            row.AppendChild(createTextCell(8, rowIndex, item.PatchStatus));
            row.AppendChild(createTextCell(9, rowIndex, item.RunStatus));





            return row;
        }

        private Row createHLBContentRow(ListInfo item, int rowIndex)
        {
            var row = new Row

            {
                RowIndex = (UInt32)rowIndex
            };
            row.AppendChild(createTextCell(1, rowIndex, item.ServerName));
            row.AppendChild(createTextCell(2, rowIndex, item.ClusterType));
            row.AppendChild(createTextCell(3, rowIndex, item.VIP));
            row.AppendChild(createTextCell(4, rowIndex, item.NodeIP));
            row.AppendChild(createTextCell(5, rowIndex, item.Port));
            row.AppendChild(createTextCell(6, rowIndex, item.PatchTool));
            row.AppendChild(createTextCell(7, rowIndex, item.PatchResult));
            row.AppendChild(createTextCell(8, rowIndex, item.ServerOnline));
            row.AppendChild(createTextCell(9, rowIndex, item.ServerUpTime));
            row.AppendChild(createTextCell(10, rowIndex, item.PatchStatus));
            row.AppendChild(createTextCell(11, rowIndex, item.RunStatus));





            return row;
        }
        #endregion

        //protected void ddlPrepScenario_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_2','content_2')</script>");
        //   string strscenario=string.Empty;
        //   if (ddlPrepScenario.SelectedValue == "1")
        //       strscenario = "Standalone";
        //   else if (ddlPrepScenario.SelectedValue == "2")
        //       strscenario = "HLB";
        //   else
        //       strscenario = "MSCS";

        //   string StartDate = string.Empty;
        //   string EndDate = string.Empty;
        //   DataTable dtPrepGroupName = DataAccessLayer.GetGroupNames(StartDate, EndDate, strscenario);

        //   // DropDownList ddlPrepNames = (DropDownList)Parent.FindControl("ddlPrepNames");
        //    ddlPrepNames.DataTextField = "groupname";
        //    ddlPrepNames.DataValueField = "groupname";
        //    ddlPrepNames.DataSource = dtPrepGroupName;
        //    ddlPrepNames.DataBind();
        //    ddlPrepNames.Items.Insert(0, new ListItem("Select", "0"));
        //}
        protected void ddlScenario_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
            string strscenario = string.Empty;
            if (ddlScenario.SelectedValue == "1")
                strscenario = "Standalone";
            else if (ddlScenario.SelectedValue == "2")
                strscenario = "HLB";
            else
                strscenario = "MSCS";
            string StartDate = "";
            string EndDate = "";
            DataTable dtExecuteGroupName = DataAccessLayer.GetGroupNames(StartDate, EndDate, strscenario);
            //DropDownList ddlExecuteNames = (DropDownList)Parent.FindControl("ddlExecuteNames");
            ddlExecuteNames.DataTextField = "groupname";
            ddlExecuteNames.DataValueField = "groupname";
            ddlExecuteNames.DataSource = dtExecuteGroupName;
            ddlExecuteNames.DataBind();
            ddlExecuteNames.Items.Insert(0, new ListItem("Select", "0"));
        }
       

    }
}

