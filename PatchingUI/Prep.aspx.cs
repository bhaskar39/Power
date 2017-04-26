﻿using System;
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

namespace PatchingUI
{
    public partial class Prep : System.Web.UI.Page
    {
        #region Variables
        string[] textData = null;
        string[] headers = null;
        string filename = string.Empty;
        StreamWriter sw = null;
        private Boolean IsPageRefresh = false;
        public string exereport = string.Empty;
        string connectionString = null;
        SqlConnection conn = null;
        SqlCommand command = null;
        SqlDataAdapter dataAdapter = null;
        string uniqueGUID = null;
        RunbookOperations rs = null;
        string output = null;
        string path = string.Empty;
        string txtLogPath = string.Empty;
        string strServerlist = string.Empty;
        string timerrefreshinterval = string.Empty;

        #endregion Variables

        protected void Page_Load(object sender, EventArgs e)
        {
            //Prep tab to replace txtLogPath- given key value as C:\Temp
            txtLogPath = ConfigurationManager.AppSettings["PrepLogPath"].ToString();
           
            if (!IsPostBack)
            {
                timerrefreshinterval = ConfigurationManager.AppSettings["TimerRefreshInterval"];
                timerPrep.Interval = Convert.ToInt32(timerrefreshinterval);
                //LoadControls();
                LoadGroupNames();                
            }
        }

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
                   // ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_2','content_2')</script>");
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
                //ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_2','content_2')</script>");

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
                objParams.LogsPath = txtLogPath;
                objParams.AdminAccountName = txtAdmin.Text;
                objParams.AdminAccountPwd = EncryptMethod(txtPassword.Text);
                objParams.uniqueGUID = hdnUniqueAccess.Value;
                objParams.InputFilename = hdnPrepInputFilename.Value;


                if (hdnPrepFileName.Value.Contains(".xlsx") || hdnPrepFileName.Value.Contains(".xls"))
                {
                    objParams.CheckExcelORText = "Excel";                    
                }
                else if (hdnPrepFileName.Value == "Group")
                {
                    objParams.CheckExcelORText = "Group";
                    objParams.GroupName = ddlPrepNames.SelectedValue.ToString();
                }
                else
                {
                    objParams.CheckExcelORText = "Text";                   
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
                //ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_2','content_2')</script>");

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
                objParams.LogsPath = txtLogPath;
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
                        dsData = ReadExcel(strServerlist);
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

                    string strTextPath = ConfigurationManager.AppSettings["PrepFilesPath"].ToString() + uniqueGUID + ".txt";
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
                //System.Threading.Thread.Sleep(20000);
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
                //ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_2','content_2')</script>");

                if (!IsPageRefresh)
                {
                    dtSmokeResult = new DataTable();
                    string strDestPath = ConfigurationManager.AppSettings["ExcelFilesPath"].ToString();
                    uniqueGUID = Guid.NewGuid().ToString();
                    hdnUniqueAccess.Value = uniqueGUID;
                    rs = new RunbookOperations();
                    string strRunbookPath = ConfigurationManager.AppSettings["SmokeTestPath"].ToString();
                    string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();

                    if (ddlPrepNames.SelectedValue != "0")
                    {
                        output = rs.StartSmokeTestRunbook(strRunbookPath, "", txtLogPath, uniqueGUID, "No", "Group", ddlPrepNames.SelectedItem.Text);


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

                            dsData = ReadExcel(strServerlist);

                            if (dsData.Tables[0].Columns[0].ToString() == "ServerName" && dsData.Tables[0].Columns[1].ToString() == "Priority")
                            {
                                output = rs.StartSmokeTestRunbook(strRunbookPath, strServerlist, txtLogPath, uniqueGUID, "No", "Excel", "");

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

                        output = rs.StartSmokeTestRunbook(strRunbookPath, strTextPath, txtLogPath, uniqueGUID, "No", "Text", "");


                    }
                    //System.Threading.Thread.Sleep(25000);
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
                }
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
           // ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_2','content_2')</script>");

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
                    

                    if (ddlPrepNames.SelectedValue != "0")
                    {
                        output = rs.StartFlashRunbook(strRunbookPath, "", uniqueGUID, hdnFileName.Value, "Group", ddlPrepNames.SelectedItem.Text);
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
                            dsData = ReadExcel(strServerlist);

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

                                output = rs.StartFlashRunbook(strRunbookPath, strServerlist, uniqueGUID, hdnFileName.Value, "Excel", "");


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
                        output = rs.StartFlashRunbook(strRunbookPath, strTextPath, uniqueGUID, hdnFileName.Value, "Text", "");
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
                //ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_2','content_2')</script>");
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
            //ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_2','content_2')</script>");
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

        public void LoadGroupNames()
        {
            try
            {


                string StartDate = string.Empty;
                string EndDate = string.Empty;


                int date = DateTime.Now.Day;
                if (date < 14)
                {
                    StartDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + "-14";
                    EndDate = DateTime.Now.ToString("yyyy-MM") + "-14";
                }

                else
                {
                    StartDate = DateTime.Now.ToString("yyyy-MM") + "-14";
                    EndDate = DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-14";
                }                              

                DataTable dtPrepGroupName = DataAccessLayer.GetGroupNames(StartDate, EndDate, null);                
                ddlPrepNames.DataTextField = "groupname";
                ddlPrepNames.DataValueField = "groupname";
                ddlPrepNames.DataSource = dtPrepGroupName;
                ddlPrepNames.DataBind();
                ddlPrepNames.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
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