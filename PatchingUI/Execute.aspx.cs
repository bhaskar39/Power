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

namespace PatchingUI
{
    public partial class Execute : System.Web.UI.Page
    {
        #region Variables
        private Boolean IsPageRefresh = false;
        RunbookOperations rs = null;
        string uniqueGUID = null;
        string strServerlist = string.Empty;
        string output = null;
        string timerrefreshinterval = string.Empty;
        #endregion variables

        protected void Page_Load(object sender, EventArgs e)  
        {

            ucMSCSHLBGrid.chkFailbackClick += new EventHandler(ucMSCSHLBGrid_chkFailbackClick);
            ucMSCSHLBGrid.GridOnRowDataBound += new GridViewRowEventHandler(ucMSCSHLBGrid_GridOnRowDataBound);
            if (!IsPostBack)
            {
                timerrefreshinterval = ConfigurationManager.AppSettings["TimerRefreshInterval"];
                ExecuteTimer.Interval = Convert.ToInt32(timerrefreshinterval);
                ExecuteCompletedTimer.Interval = Convert.ToInt32(timerrefreshinterval);
                   
                    LoadGroupNames();
                    //Code to maintain unique session id for each tab
                    Random r = new Random(DateTime.Now.Millisecond + DateTime.Now.Second * 1000 + DateTime.Now.Minute * 60000 + DateTime.Now.Minute * 3600000);
                    PageID.Value = r.Next().ToString();
                    ViewState[PageID.Value] = System.Guid.NewGuid().ToString();
                    Session[PageID.Value] = System.Guid.NewGuid().ToString();                   

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

                DataTable dtExecuteGroupName = DataAccessLayer.GetGroupNames(StartDate, EndDate, null);
                ddlExecuteNames.DataTextField = "groupname";
                ddlExecuteNames.DataValueField = "groupname";
                ddlExecuteNames.DataSource = dtExecuteGroupName;
                ddlExecuteNames.DataBind();
                ddlExecuteNames.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
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

        protected void ucMSCSHLBGrid_chkFailbackClick(object sender, EventArgs e)
        {
            ChkFailBack_Clicked(sender, e);
        }

        protected void ucMSCSHLBGrid_GridOnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            gvClusterExecute_OnRowDataBound(sender, e);
        }

        #region ChkForceStandalone_Clicked
        //public void ChkForceStandalone_Clicked(Object sender, EventArgs e)
        //{
        //    try
        //    {
        //        CheckBox chkForceStandalone = (CheckBox)sender;
        //        GridViewRow row = (GridViewRow)chkForceStandalone.NamingContainer;
        //        string Nodename = string.Empty;
        //        string PauseFlag = "0";
        //        string PauseValue = string.Empty;
        //        string FailbackFlag = "0";
        //        int result = 0;
        //        string ForceStandaloneFlag = "0";
        //        if (row != null)
        //        {

        //            bool Value = ((CheckBox)(row.FindControl("chkForceStandalone"))).Checked;

        //            Label lblServerName = (Label)row.FindControl("lblServerName");
        //            Nodename = lblServerName.Text;
        //            DropDownList ddlPause = (DropDownList)row.FindControl("ddlPause");
        //            DropDownList ddlFailOverNode = (DropDownList)row.FindControl("ddlFailOverNode");
        //            //PauseValue = ddlPause.SelectedItem.Text;
        //            PauseValue = ddlPause.SelectedValue;
        //            CheckBox ChkPauseNode = (CheckBox)row.FindControl("ChkPauseNode");
        //            // PauseFlag = ChkPauseNode.Checked.ToString();
        //            CheckBox ChkFailBack = (CheckBox)row.FindControl("ChkFailBack");
        //            // FailbackFlag = ChkFailBack.Checked.ToString();
        //            // var isChecked = $(checked).attr('checked') ? true : false;
        //            PauseFlag = ChkPauseNode.Checked ? "1" : "0";
        //            FailbackFlag = ChkFailBack.Checked ? "1" : "0";
        //            // CheckBox chkForceStandalone = (CheckBox)row.FindControl("chkForceStandalone");
        //            ForceStandaloneFlag = chkForceStandalone.Checked ? "1" : "0";
        //            //ChkPauseNode.Enabled = ChkFailBack.Enabled = ddlPause.Enabled = chkForceStandalone.Enabled = false;

        //            //result = DataAccessLayer.UpdateNodeInfo(hdnClusterExecute.Value, Nodename, PauseFlag, PauseValue, FailbackFlag, ForceStandaloneFlag,ddlFailOverNode.SelectedValue);
        //            Label lblNodeType = (Label)row.FindControl("lblNodeType");
        //            Label lblClusterType = (Label)row.FindControl("lblClusterType");

        //            if (((lblClusterType.Text.Contains("MSCS - 2")) && lblNodeType.Text.ToLower() == "active"))
        //            {
        //                DataSet dsBackUpNodelist = DataAccessLayer.GetBackUpNodesList(hdnClusterExecute.Value, lblServerName.Text);
        //                hdnBackUpNode.Value = dsBackUpNodelist.Tables[0].Rows[0]["NodeName"].ToString();
        //                result = DataAccessLayer.UpdateNodeInfo(hdnClusterExecute.Value, Nodename, PauseFlag, PauseValue, FailbackFlag, ForceStandaloneFlag, hdnBackUpNode.Value);

        //            }
        //            else if (lblClusterType.Text.Contains("Standalone"))
        //            {
        //            }
        //            else
        //            {

        //                result = DataAccessLayer.UpdateNodeInfo(hdnClusterExecute.Value, Nodename, PauseFlag, PauseValue, FailbackFlag, ForceStandaloneFlag, ddlFailOverNode.SelectedValue);
        //            }


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteError(ex);
        //    }

        //}
        #endregion

        #region ChkFailBack_Clicked
        /// <summary>
        /// failback to original state” checkbox checked for all nodes of the cluster if any of the nodes under the cluster is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ChkFailBack_Clicked
        /// <summary>
        /// failback to original state” checkbox checked for all nodes of the cluster if any of the nodes under the cluster is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void CheckFailBack(Object sender, GridView gvLocal)
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
                        foreach (GridViewRow gvr in gvLocal.Rows)
                        {
                            Label ClusterName = (Label)gvr.FindControl("lblClusterName");
                            CheckBox FailBack = (CheckBox)gvr.FindControl("ChkFailBack");
                            if (lblClusterName.Text == ClusterName.Text)
                                FailBack.Checked = true;
                        }
                    }
                    else
                    {
                        Label lblClusterName = (Label)row.FindControl("lblClusterName");
                        foreach (GridViewRow gvr in gvLocal.Rows)
                        {
                            Label ClusterName = (Label)gvr.FindControl("lblClusterName");
                            CheckBox FailBack = (CheckBox)gvr.FindControl("ChkFailBack");
                            if (lblClusterName.Text == ClusterName.Text)
                                FailBack.Checked = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }

        public void ChkFailBack_Clicked(Object sender, EventArgs e)
        {
            CheckFailBack(sender, gvClusterExecute);
        }
        #endregion
     
        #endregion

    
        #region btnDisplayRefresh_Click
        protected void btnDisplayRefresh_Click(object sender, EventArgs e)
        {
            try
            {              
                if (ddlFilterBy.SelectedItem.Value != "0")
                {
                    DataTable dtFilterOutput = DataAccessLayer.GetSUExecutionSummary(hdnClusterExecute.Value, ddlFilterBy.SelectedItem.Value, ddlFilterBy.SelectedItem.Text);
                    gvClusterExecute.DataSource = dtFilterOutput;
                    gvClusterExecute.DataBind();
                }
                else
                {
                    DataTable dtFilterOutput = DataAccessLayer.GetSUExecutionSummary(hdnClusterExecute.Value);
                    gvClusterExecute.DataSource = dtFilterOutput;
                    gvClusterExecute.DataBind();
                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }

        #endregion

        #region ddlDisplayType_Click
        protected void ddlDisplayType_Click(object sender, EventArgs e)
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
            try
            {
                int Count = DataAccessLayer.GetExecutionStatus(hdnClusterExecute.Value);
                lblDisplayCount.Text = Convert.ToString(Count);
                //DataTable dtExecutionDetails = DataAccessLayer.GetSUExecutionSummary(hdnClusterExecute.Value);
                GridBind();
                
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

            //ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
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

                //strRunbookPath = ConfigurationManager.AppSettings["PatchingPathComplete"].ToString();
              if (ddlScenario.SelectedValue.ToString() == "1" || ddlScenario.SelectedValue.ToString() == "3")
                {
                    strRunbookPath = ConfigurationManager.AppSettings["PatchingPath_New"].ToString();
                }
                else if (ddlScenario.SelectedValue.ToString() == "2")
                {
                    strRunbookPath = ConfigurationManager.AppSettings["PatchingPath_HLB"].ToString();
                }

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

                //System.Threading.Thread.Sleep(60000);
                divClusterExecute.Visible = true;

               // int intRet = DataAccessLayer.GetHLBMSCStandaloneData(hdnClusterExecute.Value);
                //ddlScenario.SelectedValue = intRet.ToString();

                GridBind();

                ExecuteTimer.Enabled = true;
                //ExecuteTimer.Interval = 60000;
                //label processing visible
                lblprocessing.Visible = false;

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

        private void ShowHideControls(bool Visible)
        {
            lblProcessUnderWay.Visible = Visible;
            lblInProgress.Visible = Visible;

            lblProcessCompleted.Visible = Visible;
            lblcompleted.Visible = Visible;

            if (Visible == true)
            {
                btnDisplayRefresh.Visible = false;
                btnExecuteRefresh.Visible = true;
            }
            else
            {
                btnDisplayRefresh.Visible = true;
                btnExecuteRefresh.Visible = false;
            }
        }

        public void GridBind()
        {
            try
            {
                ShowHideControls(false);

                int intTotalGivenServersCount = DataAccessLayer.GetGivenTotalServerCount(hdnClusterExecute.Value);
                int intMSCSHLBCount = DataAccessLayer.GetMSCSHLBExecutionStatus(hdnClusterExecute.Value);

                if (intTotalGivenServersCount > 0 && intTotalGivenServersCount == intMSCSHLBCount)
                {
                    ExecuteTimer.Enabled = false;
                    btnClusterExecute.Visible = true;
                }

                lblTotalServers.Text = Convert.ToString(intMSCSHLBCount);
                DataTable dtExecutionDetails = DataAccessLayer.GetSUExecutionSummary(hdnClusterExecute.Value);
                ucMSCSHLBGrid.DataSource = dtExecutionDetails;
                ucMSCSHLBGrid.DataBind();
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

        public string GetFormatedData(string value)
        {
            return value.Replace("_", " ").Replace(",", ", ");
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
           
            btnExportToExcel.Enabled = true;
            btnClusterExecute.Enabled = false;
            hdnClusterFlag.Value = "1";
            string strStatus = string.Empty;
            string strPatchOption = string.Empty; 
            ShowHideControls(true);
            if (lblTotalServers.Text == lblcompleted.Text)
            {
                ExecuteCompletedTimer.Enabled = false;
                btnClusterExecute.Visible = false;
            }
            ucMSCSHLBGrid.Persist_gvClusterExecute_State(hdnClusterExecute.Value);
            lblTotalServers.Text = Convert.ToString(DataAccessLayer.GetExecutionTotalCountForMSCSAndHLB(hdnClusterExecute.Value));
            lblInProgress.Text = Convert.ToString(DataAccessLayer.GetSUExecutionInProgressCountForMSCSAndHLB(hdnClusterExecute.Value)); //+ DataAccessLayer.GetSUExecutionYetToStartCount(hdnClusterExecute.Value));
            lblcompleted.Text = Convert.ToString(DataAccessLayer.GetSUExecutionCompletedCountForMSCSAndHLB(hdnClusterExecute.Value));

            DataTable dtExecutionDetails = DataAccessLayer.GetSUExecutionSummary(hdnClusterExecute.Value);
            ViewState["ListingInfo"] = dtExecutionDetails;
            ucMSCSHLBGrid.DataSource = dtExecutionDetails;
            ucMSCSHLBGrid.DataBind();
        }
        #endregion

        #region Execute Timer Tick event

        private void Persist_gvClusterExecute_State()
        {
            GridView tempGridView = new GridView();
            string RunValidationFlag = "0";
            string CheckCheckBox = "0";
            string ForceStandaloneFlag = "0";
            string Nodename = string.Empty;
            string PauseFlag = "0";
            string PauseValue = string.Empty;
            string FailbackFlag = "0";
            string BackupNode = string.Empty;
            int result = 0;

            try
            {
                if (ddlScenario.SelectedValue.ToString() == "1")
                {
                    foreach (GridViewRow row in gvClusterExecute.Rows)
                    {
                        CheckBox chkRunValidation = (CheckBox)row.FindControl("chkRunValidation");
                        if (chkRunValidation != null)
                            RunValidationFlag = chkRunValidation.Checked ? "1" : "0";


                        CheckBox chkForceStandalone = (CheckBox)row.FindControl("chkForceStandalone");
                        if (chkForceStandalone != null)
                            ForceStandaloneFlag = chkForceStandalone.Checked ? "1" : "0";



                        Label lblServerName = (Label)row.FindControl("lblServerName");
                        if (lblServerName != null)
                            Nodename = lblServerName.Text;

                        CheckBox ChkPauseNode = (CheckBox)row.FindControl("ChkPauseNode");
                        if (ChkPauseNode != null)
                            PauseFlag = ChkPauseNode.Checked ? "1" : "0";

                        DropDownList ddlPause = (DropDownList)row.FindControl("ddlPause");
                        if (ddlPause != null)
                            PauseValue = ddlPause.SelectedValue;

                        CheckBox ChkFailBack = (CheckBox)row.FindControl("ChkFailBack");
                        if (ChkFailBack != null)
                            FailbackFlag = ChkFailBack.Checked ? "1" : "0";

                        DropDownList ddlFailOverNode = (DropDownList)row.FindControl("ddlFailOverNode");
                        if (ddlFailOverNode != null)
                            BackupNode = ddlFailOverNode.SelectedValue;

                        if (!string.IsNullOrWhiteSpace(Nodename))
                            result = DataAccessLayer.UpdateNodeInfo(hdnClusterExecute.Value, Nodename, PauseFlag, PauseValue, FailbackFlag, ForceStandaloneFlag, BackupNode, RunValidationFlag);
                    }
                }
                else if (ddlScenario.SelectedValue.ToString() == "2")
                {
                    foreach (GridViewRow row in gvHLBExecute.Rows)
                    {
                        CheckBox chkCheck = (CheckBox)row.FindControl("ChkValue");
                        if (chkCheck != null)
                            CheckCheckBox = chkCheck.Checked ? "1" : "0";

                        CheckBox chkRunValidation = (CheckBox)row.FindControl("chkRunValidationHLB");
                        if (chkRunValidation != null)
                            RunValidationFlag = chkRunValidation.Checked ? "1" : "0";

                        DropDownList ddlPause = (DropDownList)row.FindControl("ddlPause");
                        if (ddlPause != null)
                            PauseValue = ddlPause.SelectedValue;

                        Label lblServerName = (Label)row.FindControl("lblServerName");
                        if (lblServerName != null)
                            Nodename = lblServerName.Text;

                        if (!string.IsNullOrWhiteSpace(Nodename))
                            result = DataAccessLayer.UpdateHLBCheckValue(hdnClusterExecute.Value, Nodename, CheckCheckBox, PauseValue, RunValidationFlag);
                    }
                }
            }

            catch (Exception ex)
            {
                WriteError(ex);
            }
        }

        protected void ExecuteTimer_Tick(object sender, EventArgs e)
        {
           
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


        #region UpdateRunValidationFlag

        public void UpdateRunValidationFlag()
        {
            string PatchingScenario = ddlScenario.SelectedValue.ToString();
            string Nodename = string.Empty;
            string RunValidationFlag = "0";
            try
            {

                if (PatchingScenario == "1" || PatchingScenario == "3")
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

        
        #region Report Timer Tick event

        
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
               // ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
                mpeClusterExecute.Hide();
                pnlDisplay.Visible = false;
                btnClusterExecute.Enabled = false;
                //pnlCounts.Visible = true;
                //ClusterExecute();
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }





        }

        //#region ClusterExecute
        //public void ClusterExecute()
        //{

        //    try
        //    {
        //        pnlCounts.Visible = true;
        //        pnlDisplay.Visible = false;
        //        if (ddlScenario.SelectedValue.ToString() == "1" || ddlScenario.SelectedValue == "3")
        //        {
        //            gvHLBExecute.Visible = false;
        //            gvClusterExecute.Visible = true;
        //            lblExeceEnv.Visible = false;
        //            ddlEnvironment.Visible = false;
        //            ddlExeClusterType.Visible = true;
        //            lblExeClustype.Visible = true;
        //            hdnClusterFlag.Value = "1";
        //            Persist_gvClusterExecute_State();
        //            //UpdateRunValidationFlag();

        //            //string Nodename = string.Empty;
        //            //string PauseFlag = "0";
        //            //string PauseValue = string.Empty;
        //            //string FailbackFlag = "0";
        //            //string ForceStandaloneFlag = "0";

        //            //int result = 0;

        //            //foreach (GridViewRow row in gvClusterExecute.Rows)
        //            //{

        //            //    Label lblServerName = (Label)row.FindControl("lblServerName");
        //            //    Nodename = lblServerName.Text;
        //            //    DropDownList ddlPause = (DropDownList)row.FindControl("ddlPause");
        //            //    DropDownList ddlFailOverNode = (DropDownList)row.FindControl("ddlFailOverNode");

        //            //    PauseValue = ddlPause.SelectedValue;
        //            //    CheckBox ChkPauseNode = (CheckBox)row.FindControl("ChkPauseNode");
        //            //    CheckBox ChkFailBack = (CheckBox)row.FindControl("ChkFailBack");
        //            //    PauseFlag = ChkPauseNode.Checked ? "1" : "0";
        //            //    FailbackFlag = ChkFailBack.Checked ? "1" : "0";
        //            //    CheckBox chkForceStandalone = (CheckBox)row.FindControl("chkForceStandalone");
        //            //    ForceStandaloneFlag = chkForceStandalone.Checked ? "1" : "0";
        //            //    ChkPauseNode.Enabled = ChkFailBack.Enabled = ddlPause.Enabled = ddlFailOverNode.Enabled = false;

        //            //    Label lblNodeType = (Label)row.FindControl("lblNodeType");
        //            //    Label lblClusterType = (Label)row.FindControl("lblClusterType");
        //            //    Label lblNodeClusterName = (Label)row.FindControl("lblNodeClusterName");





        //            //    if (((lblClusterType.Text.Contains("MSCS - 2")) && lblNodeType.Text.ToLower() == "active"))
        //            //    {
        //            //        DataSet dsBackUpNodelist = DataAccessLayer.GetBackUpNodesList(hdnClusterExecute.Value, lblServerName.Text);
        //            //        if (dsBackUpNodelist.Tables[0].Rows.Count > 0)
        //            //        {
        //            //            hdnBackUpNode.Value = dsBackUpNodelist.Tables[0].Rows[0]["NodeName"].ToString();
        //            //            // result = DataAccessLayer.UpdateNodeInfo(hdnClusterExecute.Value, Nodename, PauseFlag, PauseValue, FailbackFlag, ForceStandaloneFlag, hdnBackUpNode.Value);
        //            //        }
        //            //        else
        //            //            hdnBackUpNode.Value = "";
        //            //        result = DataAccessLayer.UpdateNodeInfo(hdnClusterExecute.Value, Nodename, PauseFlag, PauseValue, FailbackFlag, ForceStandaloneFlag, hdnBackUpNode.Value);
        //            //    }
        //            //    else if (lblClusterType.Text.Contains("Standalone"))
        //            //    {
        //            //    }
        //            //    else
        //            //    {

        //            //        result = DataAccessLayer.UpdateNodeInfo(hdnClusterExecute.Value, Nodename, PauseFlag, PauseValue, FailbackFlag, ForceStandaloneFlag, ddlFailOverNode.SelectedValue);
        //            //    }

        //            //}


        //            rs = new RunbookOperations();
        //            string strRunbookPath = ConfigurationManager.AppSettings["PatchingPath"].ToString();
        //            try
        //            {
        //                output = rs.StartNewRunbookWithParameters(strRunbookPath, ddlPatchingOption.SelectedIndex.ToString(),
        //                    txtLogPath.Text, ddlODPOption.SelectedIndex.ToString(), "nope", ddlSimpleUpdateOption.SelectedValue, hdnClusterExecute.Value,
        //                    hdnExecuteFileName.Value, ddlReboot.SelectedValue, txtOnlyQFE.Text, txtExcludeQFE.Text, rblBPU.SelectedValue.ToString());
        //            }

        //            catch (Exception ex)
        //            {
        //                if (ex.Message == "Error starting runbook.")
        //                {

        //                    output = rs.StartNewRunbookWithParameters(strRunbookPath, ddlPatchingOption.SelectedIndex.ToString(),
        //                    txtLogPath.Text, ddlODPOption.SelectedIndex.ToString(), "nope", ddlSimpleUpdateOption.SelectedValue, hdnClusterExecute.Value,
        //                     hdnExecuteFileName.Value, ddlReboot.SelectedValue, txtOnlyQFE.Text, txtExcludeQFE.Text, rblBPU.SelectedValue.ToString());
        //                }
        //                else
        //                {
        //                    WriteError(ex);
        //                }


        //            }

        //            ExecuteCompletedTimer.Enabled = true;
        //            lblTotalServers.Text = Convert.ToString(DataAccessLayer.GetSUExecutionTotalCount(hdnClusterExecute.Value));
        //            lblInProgress.Text = Convert.ToString(DataAccessLayer.GetSUExecutionInProgressCount(hdnClusterExecute.Value) + DataAccessLayer.GetSUExecutionYetToStartCount(hdnClusterExecute.Value));
        //            lblcompleted.Text = Convert.ToString(DataAccessLayer.GetSUExecutionCompletedCount(hdnClusterExecute.Value));

        //            DataTable dtExecutionDetails = DataAccessLayer.GetSUExecutionSummary(hdnClusterExecute.Value, ddlExeClusterType.SelectedValue.ToString());
        //            ViewState["ListingInfo"] = dtExecutionDetails;
        //            gvClusterExecute.DataSource = dtExecutionDetails;
        //            gvClusterExecute.DataBind();
        //        }
        //        else if (ddlScenario.SelectedValue.ToString() == "2")
        //        {

        //            gvClusterExecute.Visible = false;
        //            gvHLBExecute.Visible = true;
        //            lblExeceEnv.Visible = true;
        //            ddlEnvironment.Visible = true;
        //            ddlExeClusterType.Visible = false;
        //            lblExeClustype.Visible = false;
        //            hdnClusterFlag.Value = "1";

        //            //UpdateRunValidationFlag();
        //            Persist_gvClusterExecute_State();

        //            rs = new RunbookOperations();
        //            string strRunbookPath = ConfigurationManager.AppSettings["HLBExecutePath"].ToString();
        //            RunbookParams objParams = new RunbookParams();

        //            try
        //            {
        //                objParams.RunbookPath = strRunbookPath;
        //                objParams.InputFilename = "";
        //                objParams.CheckExcelORText = "Text";
        //                objParams.uniqueGUID = hdnClusterExecute.Value;
        //                objParams.LogsPath = txtLogPath.Text;
        //                objParams.Ipak = "nope";
        //                objParams.OnlyQFE = txtOnlyQFE.Text;
        //                objParams.BPUOption = rblBPU.SelectedValue.ToString();
        //                objParams.simpleUpdateOption = ddlSimpleUpdateOption.SelectedValue;
        //                objParams.MSNRebootFlag = ddlReboot.SelectedValue;
        //                objParams.ScanOrPatch = ddlODPOption.SelectedIndex.ToString();
        //                objParams.ExcludeQFE = txtExcludeQFE.Text;
        //                objParams.PatchingOption = ddlPatchingOption.SelectedIndex.ToString();

        //                output = rs.StartHLBExecuteRunbook(objParams);
        //            }

        //            catch (Exception ex)
        //            {
        //                if (ex.Message == "Error starting runbook.")
        //                {

        //                    output = rs.StartHLBExecuteRunbook(objParams);
        //                }
        //                else
        //                {
        //                    WriteError(ex);
        //                }
        //            }
        //            ExecuteCompletedTimer.Enabled = true;
        //            lblTotalServers.Text = Convert.ToString(DataAccessLayer.GetHLBExecutionTotalCount(hdnClusterExecute.Value));
        //            //lblInProgress.Text = Convert.ToString(DataAccessLayer.GetSUExecutionInProgressCount(hdnClusterExecute.Value) + DataAccessLayer.GetHLBSUExecutionYetToStartCount(hdnClusterExecute.Value));
        //            lblInProgress.Text = Convert.ToString(DataAccessLayer.GetHLBExecutionYetToStartCount(hdnClusterExecute.Value));
        //            lblcompleted.Text = Convert.ToString(DataAccessLayer.GetHLBExecutionCompletedCount(hdnClusterExecute.Value));

        //            DataTable dtExecutionDetails = DataAccessLayer.GetHLBExecutionSummary(hdnClusterExecute.Value, ddlEnvironment.SelectedValue);
        //            ViewState["ListingInfo"] = dtExecutionDetails;
        //            gvHLBExecute.DataSource = dtExecutionDetails;
        //            gvHLBExecute.DataBind();

        //            foreach (GridViewRow row in gvHLBExecute.Rows)
        //            {

        //                DropDownList ddlPause = (DropDownList)row.FindControl("ddlPause");
        //                CheckBox ChkValue = (CheckBox)row.FindControl("ChkValue");
        //                ChkValue.Enabled = ddlPause.Enabled = false;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        WriteError(ex);

        //    }

        //}

        //#endregion

        protected void btnClusterExecute_Click(object sender, EventArgs e)
        {
            try
            {
                //uprogressExecute.Visible = true;
                //ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
                ExecuteTimer.Enabled = false;
                string Nodename = string.Empty;
                int result = 0;
                string ChkFlagValue = "0";
                //string ChkVIPValue = "0";
                if (ddlScenario.SelectedValue.ToString() == "1" || ddlScenario.SelectedValue == "3")
                {
                   // ddlEnvironment.Visible = false;
                    mpeClusterExecute.Hide();
                    pnlDisplay.Visible = false;
                    btnClusterExecute.Enabled = false;
                    //pnlCounts.Visible = true;
                    //ClusterExecute();

                }
                else if (ddlScenario.SelectedValue.ToString() == "2")
                {
                    btnYes.Visible = true;
                    btnNo.Text = "Cancel";  

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
                        //pnlCounts.Visible = true;
                        //ClusterExecute();
                        //ddlEnvironment.Visible = true;
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

     

        public void GridResumeButton(object sender, GridView gvStandaloneMSCSHLB)
        {
            RunbookOperations rs = null;
            string output = string.Empty;
            string strPatchScenario = string.Empty;

            try
            {
                Button btn = (Button)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                int index = gvr.RowIndex;
                GridViewRow row = gvStandaloneMSCSHLB.Rows[index];
                Button btnResume = (Button)row.FindControl("btnResume");
                btnResume.Enabled = false;

                Label lblServerName = (Label)row.FindControl("lblServerName");
                Label lblClusterName = (Label)row.FindControl("lblClusterName");
                Label lblVIPIP = (Label)row.FindControl("lblVIPIP");
                Label lblNodeIP = (Label)row.FindControl("lblNodeIP");
                Label lblPort = (Label)row.FindControl("lblPort");

                strPatchScenario = DataAccessLayer.GetPatchScenario(hdnClusterExecute.Value, lblServerName.Text);
                rs = new RunbookOperations();
                RunbookParams objParams = new RunbookParams();

                if (strPatchScenario == "HLB")
                {
                    int pausedstatus = DataAccessLayer.UpdateHLBPausedStatus(hdnClusterExecute.Value, lblServerName.Text);
                    string strRunbookPath = ConfigurationManager.AppSettings["HLBResumePath"].ToString();

                    try
                    {
                        objParams.RunbookPath = strRunbookPath;
                        objParams.uniqueGUID = hdnClusterExecute.Value;
                        objParams.VIP = lblVIPIP.Text;
                        objParams.NodeName = lblServerName.Text;
                        objParams.NodeIP = lblNodeIP.Text;
                        objParams.Port = lblPort.Text;

                        output = rs.StartResumeNodeRunbook(objParams);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "Error starting runbook.")
                        {
                            output = rs.StartResumeNodeRunbook(objParams);
                        }
                    }
                }
                else
                {
                    int pausedstatus = DataAccessLayer.UpdatePausedStatus(hdnClusterExecute.Value, lblServerName.Text);
                    string strRunbookPath = ConfigurationManager.AppSettings["ResumePath"].ToString();

                    try
                    {
                        objParams.RunbookPath = strRunbookPath;
                        objParams.ServerName = lblClusterName.Text;
                        objParams.InputFilename = hdnExecuteFileName.Value;
                        objParams.CheckExcelORText = hdnInputType.Value;
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
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }

        #endregion



        protected void ddlScenario_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "Execute.aspx");
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
            //ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>tabSwitch('tab_3','content_3')</script>");
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
    }
}