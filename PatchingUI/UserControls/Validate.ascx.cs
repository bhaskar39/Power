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
    public partial class Validate : System.Web.UI.UserControl
    {
        #region Variables
        string connectionString = null;
        SqlConnection conn = null;
        SqlCommand command = null;
        SqlDataAdapter dataAdapter = null;
        string txtLogPath;
        string uniqueGUID = null;
        private Boolean IsPageRefresh = false;
        string output = null;
        string filename = string.Empty;
        string[] textData = null;
        StreamWriter sw = null;
        string path = string.Empty;
        string[] headers = null;
        RunbookOperations rs = null;
        string strServerlist = string.Empty;
        string txtOnlyQFE = string.Empty;
        string txtExcludeQFE = string.Empty;
        string rblBPU = string.Empty;
        #endregion variables

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        #region ValidateTab

        protected void btnPostSmokeTest_Click(Object sender, EventArgs e)
        {
            txtLogPath = ConfigurationManager.AppSettings["PrepLogPath"].ToString();
            try
            {
                DataTable dtSmokeResult = null;
                txtResult.Visible = false;
                rvValidate.Visible = false;
                lblRvValidate.Text = "";

                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_4','content_4')</script>");

                if (!IsPageRefresh)
                {
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


                                    output = rs.StartSmokeTestRunbook(strRunbookPath, strServerlist, txtLogPath, uniqueGUID, "Yes", "Excel","");
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


                        }

                        output = rs.StartSmokeTestRunbook(strRunbookPath, strTextPath, txtLogPath, uniqueGUID, "Yes", "Excel","");
                        //uniqueGUID = "abcd01";
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

                }
                else
                {
                }
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
            Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_4','content_4')</script>");
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


                output = rs.StartCompareSmokeTestlogRunbook(strRunbookPath, txtLogPath, uniqueGUID);
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
            txtOnlyQFE = Session["txtOnlyQFE"].ToString();
            txtExcludeQFE = Session["txtExcludeQFE"].ToString();
            rblBPU = Session["rblBPU"].ToString();

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
                    
                    if (fupValidateExcel.HasFile)
                    {
                        lblRvValidate.Text = "Report Name: " + hdnFileName.Value;
                        output = rs.StartValidateRunbook(strRunbookPath, hdnExcelPath.Value, txtLogPath, uniqueGUID, hdnFileName.Value, "Excel");
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

                            output = rs.StartValidateRunbook(strRunbookPath, strTextPath, txtLogPath, uniqueGUID, hdnFileName.Value, "Text");
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
                        output = rs.StartRunbookWithParameters(strRunbookPath, hdnExcelPath.Value, "2", txtLogPath, "0", "nope", "Scan", uniqueGUID, hdnFileName.Value, "", "Excel", txtOnlyQFE, txtExcludeQFE, rblBPU);
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

                            output = rs.StartRunbookWithParameters(strRunbookPath, strTextPath, "2", txtLogPath, "0", "nope", "Scan", uniqueGUID, hdnFileName.Value, "", "Excel", txtOnlyQFE, txtExcludeQFE, rblBPU);
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
            Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_4','content_4')</script>");
            try
            {
                if (!IsPageRefresh)
                {
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

                }
                else
                {

                }
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


        #region Validate Timer Tick event
        protected void TimerValidate_Tick(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
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