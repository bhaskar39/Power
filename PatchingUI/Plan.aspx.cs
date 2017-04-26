using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.IO;

namespace PatchingUI
{
    public partial class Plan : System.Web.UI.Page
    {
        DataTable dtPlanData = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadControls();
                LoadGroupNames();              
            }
            if (rblDefer.Items.FindByValue("0").Selected)
            {
                txtDeferStartDate.Enabled = txtDeferEndDate.Enabled = true;             
                txtdefer.Enabled = false;            
            }

            else if (rblDefer.Items.FindByValue("1").Selected)
            {
                txtDeferStartDate.Enabled = txtDeferEndDate.Enabled = false;             
                txtdefer.Enabled = true;
            }

            else
            {
                txtDeferStartDate.Enabled = txtDeferEndDate.Enabled = false;              
                txtdefer.Enabled = false;
            }
            hdnDate.Value = DateTime.Now.ToString();
        }

        #region Plan Tab
        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                string strDestPath = ConfigurationManager.AppSettings["WGPath"].ToString();
                string uniqueGUID = Guid.NewGuid().ToString();
                string filename = string.Empty;
                if (fupPlanExcel.HasFile)
                {
                    string strName = "";
                    string strFileName = "";
                    string strheader = "";
                    string strTypeofServerlist = "";
                    if (fupPlanExcel.FileName.Contains(".xlsx"))
                    {
                        strName = fupPlanExcel.FileName.Replace(".xlsx", uniqueGUID);
                        strFileName = strName + ".xlsx";

                    }
                    else if (fupPlanExcel.FileName.Contains(".xls"))
                    {
                        strName = fupPlanExcel.FileName.Replace(".xls", uniqueGUID);
                        strFileName = strName + ".xls";
                    }
                    else
                    {
                    }
                    if (fupPlanExcel.FileName.Contains(".xlsx"))
                    {
                        strheader = fupPlanExcel.FileName.Replace(".xlsx", "");
                    }
                    else if (fupPlanExcel.FileName.Contains(".xls"))
                    {
                        strheader = fupPlanExcel.FileName.Replace(".xls", "");
                    }
                    if (fupPlanExcel.FileName.Contains("_Adoc") || fupPlanExcel.FileName.Contains("_adoc"))
                    {
                        strTypeofServerlist = "Adoc";
                    }
                    else if (fupPlanExcel.FileName.Contains("_MO") || fupPlanExcel.FileName.Contains("_mo"))
                    {
                        strTypeofServerlist = "MO";
                    }
                    else if (fupPlanExcel.FileName.Contains("_WG") || fupPlanExcel.FileName.Contains("_wg"))
                    {
                        strTypeofServerlist = "WG";
                    }
                    else if (fupPlanExcel.FileName.Contains("_LT") || fupPlanExcel.FileName.Contains("_lt"))
                    {
                        strTypeofServerlist = "LT";
                    }

                    else
                    {
                        strTypeofServerlist = "";
                    }
                    hdnPlanInputFileName.Value = strheader;
                    filename = strDestPath + strFileName;
                    fupPlanExcel.SaveAs(filename);

                    string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties=Excel 12.0;";

                    try
                    {
                        string query = String.Format("select * from [{0}]", "Sheet1$");
                        OleDbDataAdapter daData = new OleDbDataAdapter(query, connectionString);
                        DataSet dsData = new DataSet();
                        daData.Fill(dsData);
                        if (dsData.Tables[0].Columns.Count == 14)
                        {
                            ReadExcel(filename, uniqueGUID, strTypeofServerlist);
                            hdnUniqueID.Value = uniqueGUID;
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('Please Select Valid Excel');</script>");
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.WriteError(ex);
                        Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('ExcelFile Sheet Name sholud be Sheet1');</script>");
                    }

                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
            }

        }
        public int SetPatchingScheduledResults(DataTable dt, string strTypeofServerlist)
        {
            int rtn = 0;
            string connectionString = null;
            SqlConnection conn = null;
            try
            {
                //connectionString = ConfigurationManager.ConnectionStrings["TK5_ConnectionString"].ConnectionString;

                connectionString = ConfigurationManager.ConnectionStrings["DB_ConnectionString"].ConnectionString;
                conn = new SqlConnection(connectionString);
                conn.Open();

                foreach (DataRow dr in dt.Rows)
                {

                    SqlCommand command = new SqlCommand("usp_SetPlanSchedule", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    // Checks for Blank values, if exists continue next iteration : 02-05-2013 Start
                    if (dr[0].ToString().Trim().Equals(""))
                    {
                        continue;
                    }
                    // Checks for Blank values, if exists continue next iteration : 02-05-2013 End
                    command.Parameters.AddWithValue("@ServerName", dr[0].ToString());
                    command.Parameters.AddWithValue("@ScheduledWindow", dr[1].ToString());
                    command.Parameters.AddWithValue("@Env", dr[2].ToString());
                    command.Parameters.AddWithValue("@Org1", dr[3].ToString());
                    command.Parameters.AddWithValue("@Org2", dr[4].ToString());
                    command.Parameters.AddWithValue("@Org3", dr[5].ToString());
                    command.Parameters.AddWithValue("@Org4", dr[6].ToString());
                    command.Parameters.AddWithValue("@Applications", dr[7].ToString());
                    command.Parameters.AddWithValue("@PatchWeek", dr[8].ToString());
                    command.Parameters.AddWithValue("@PatchMonth", dr[9].ToString());
                    command.Parameters.AddWithValue("@PatchingScenario", dr[10].ToString());
                    command.Parameters.AddWithValue("@UniqueID", dr[14].ToString());
                    command.Parameters.AddWithValue("@InitiatedBy", dr[15].ToString());
                    command.Parameters.AddWithValue("@DateTime", dr[16].ToString());
                    command.Parameters.AddWithValue("@InputFileName", hdnPlanInputFileName.Value);
                    command.Parameters.AddWithValue("@Mode", dr[11].ToString());
                    command.Parameters.AddWithValue("@Status", dr[12].ToString());
                    command.Parameters.AddWithValue("@Comments", dr[13].ToString());

                    command.Parameters.AddWithValue("@TypeofServerList", strTypeofServerlist);
                    rtn = command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
            }
            finally
            {
                conn.Close();
            }
            return rtn;
        }
        public void LoadControls()
        {
            try
            {
                DataSet dsPlanControlsData = DataAccessLayer.GetPlanControlsData();

                ddlOrgName.DataTextField = "org1";
                ddlOrgName.DataValueField = "org1";
                ddlOrgName.DataSource = dsPlanControlsData.Tables[0];
                ddlOrgName.DataBind();
                ddlOrgName.Items.Insert(0, new ListItem("--All--", "0"));

                ddlServerlistType.DataTextField = "TypeofServerlist";
                ddlServerlistType.DataValueField = "TypeofServerlist";
                ddlServerlistType.DataSource = dsPlanControlsData.Tables[3];
                ddlServerlistType.DataBind();
                ddlServerlistType.Items.Insert(0, new ListItem("--All--", "0"));

                ddlPatchMonth.DataTextField = "PatchMonth";
                ddlPatchMonth.DataValueField = "PatchDate";
                ddlPatchMonth.DataSource = dsPlanControlsData.Tables[5];
                ddlPatchMonth.DataBind();
                string strPatchMonth = GetPatchMonth();
                ddlPatchMonth.Items.FindByText(strPatchMonth).Selected = true;
                ddlSubBPU.Items.Insert(0, new ListItem("--All--", "0"));
                ddlApplication.DataTextField = "Applications";
                ddlApplication.DataValueField = "Applications";
                ddlApplication.DataSource = dsPlanControlsData.Tables[7];
                ddlApplication.DataBind();
                ddlApplication.Items.Insert(0, new ListItem("--All--", "0"));


                ddlEnv.DataTextField = "Env";
                ddlEnv.DataValueField = "Env";
                ddlEnv.DataSource = dsPlanControlsData.Tables[8];
                ddlEnv.DataBind();
                ddlEnv.Items.Insert(0, new ListItem("--All--", "0"));



            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
            }
        }

        #region ddlBPU_SelectedIndexChanged
        protected void ddlBPU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlOrgName.SelectedItem.Text == "--All--")
                {
                    ddlSubBPU.Items.Insert(0, new ListItem("--All--", "0"));
                }
                else
                {
                    DataTable dtSubBPU = DataAccessLayer.GetSubBPU(ddlOrgName.SelectedItem.Text);
                    ddlSubBPU.DataTextField = "org2";
                    ddlSubBPU.DataValueField = "org2";
                    ddlSubBPU.DataSource = dtSubBPU;
                    ddlSubBPU.DataBind();
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
            }
        }
        #endregion
        public void GetPatchingScheduledResults(string uniqueID)
        {

            try
            {
                string strOrgFilter = string.Empty;
                string strScenarioFilter = string.Empty;
                string strModeFilter = string.Empty;
                string strTypeFilter = string.Empty;
                string strWeekFilter = string.Empty;
                string strMonthFilter = string.Empty;
                string strSubBPUFilter = string.Empty;
                string strAppFilter = string.Empty;
                string strEnvFilter = string.Empty;

                FilterGridData();
                gvResults.Visible = true;

                if (dtPlanData.Rows.Count == 0)
                {
                    dtPlanData.Rows.Add(dtPlanData.NewRow());
                    gvResults.DataSource = dtPlanData;

                    gvResults.DataBind();

                    int columncount = gvResults.Rows[0].Cells.Count;

                    gvResults.Rows[0].Cells.Clear();

                    gvResults.Rows[0].Cells.Add(new TableCell());

                    gvResults.Rows[0].Cells[0].ColumnSpan = columncount;

                    gvResults.Rows[0].Cells[0].Text = "No Records Found";

                    trSave.Visible = false;
                    trSaveBottom.Visible = false;
                }

                else
                {
                    gvResults.DataSource = dtPlanData;
                    gvResults.DataBind();

                    int rowcount = dtPlanData.Rows.Count;
                    GetPageServerCount(rowcount);
                    lblServerCount.Visible = true;

                    trSave.Visible = true;
                    trSaveBottom.Visible = true;
                    txtdefer.Visible = true;
                    PPMode.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
            }
            finally
            {

            }
        }

        private void FilterGridData()
        {
            string strFilter = string.Empty;
            string strSelectedDate = ddlPatchMonth.SelectedValue.ToString();
            string[] strResult = strSelectedDate.Split('-');
            if (strResult.Length > 1)
            {
                strFilter = " where MONTH(p.PatchMonth)= '" + strResult[0] + "' AND YEAR(p.PatchMonth) = '" + strResult[1] + "'";

                if (ddlOrgName.SelectedValue != "0")
                    strFilter = strFilter + "and p.Org1='" + ddlOrgName.SelectedValue + "'";

                if (ddlPatchScenario.SelectedValue != "0")
                    strFilter = strFilter + " and LOWER(p.PatchingScenario)='" + ddlPatchScenario.SelectedItem.Text.ToLower() + "'";

                if (ddlMode.SelectedValue != "0")
                    strFilter = strFilter + " and p. Mode='" + ddlMode.SelectedValue + "'";

                if (ddlServerlistType.SelectedValue != "0")
                    strFilter = strFilter + " and p.TypeofServerList='" + ddlServerlistType.SelectedValue + "'";

                if (ddlPatchWeek.SelectedValue != "0")
                    strFilter = strFilter + " and p.PatchWeek='" + ddlPatchWeek.SelectedValue + "'";

                if (ddlSubBPU.SelectedValue != "0")
                    strFilter = strFilter + " and p.Org2='" + ddlSubBPU.SelectedValue + "'";

                if (ddlApplication.SelectedValue != "0")
                    strFilter = strFilter + " and p.Applications='" + ddlApplication.SelectedValue + "'";

                if (ddlEnv.SelectedValue != "0")
                    strFilter = strFilter + " and p.Env='" + ddlEnv.SelectedValue + "'";

                if (ddlGroupName.SelectedValue != "0")
                    strFilter = strFilter + " and p.GroupName like '%" + ddlGroupName.SelectedValue + "%'";

                dtPlanData = DataAccessLayer.GetPlanData(strFilter);
                DataColumn dcGroup = new DataColumn("IsGroupSelected", typeof(bool));
                dtPlanData.Columns.Add(dcGroup);
                Session["dtPlanData"] = dtPlanData;
            }

        }

        public void ReadExcel(string strExcelPath, string uniqueID, string strTypeOfServerlist)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strExcelPath + ";Extended Properties=Excel 12.0;";
            try
            {
                string query = String.Format("select * from [{0}]", "Sheet1$");
                OleDbDataAdapter daData = new OleDbDataAdapter(query, connectionString);
                DataSet dsData = new DataSet();
                daData.Fill(dsData);
                if (dsData.Tables[0].Columns.Count == 14)
                {
                    dsData.Tables[0].Columns.Add("UniqueID", Type.GetType("System.String"));
                    dsData.Tables[0].Columns.Add("InitiatedBy", Type.GetType("System.String"));
                    dsData.Tables[0].Columns.Add("DateTime", Type.GetType("System.DateTime"));
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                        dr["UniqueID"] = uniqueID;
                        dr["InitiatedBy"] = HttpContext.Current.User.Identity.Name;
                        dr["DateTime"] = DateTime.Now;
                    }
                    dsData.AcceptChanges();
                    SetPatchingScheduledResults(dsData.Tables[0], strTypeOfServerlist);
                    GetPatchingScheduledResults(uniqueID);
                    LoadControls();

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Incorrect Excel Format')", true);
                }
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Message1", "<script type='text/jscript'>alert('ExcelFile Sheet Name sholud be Sheet1');</script>");

            }

        }


        protected void btnSaveGroup_Click(object sender, EventArgs e)
        {

            try
            {
                gvResults_PageIndexChanging(sender, new GridViewPageEventArgs(gvResults.PageIndex));
                DataTable dtDetails = new DataTable();
                dtDetails.Columns.Add("ServerName", Type.GetType("System.String"));
                dtDetails.Columns.Add("PatchMonth", Type.GetType("System.String"));
                dtDetails.Columns.Add("UniqueID", Type.GetType("System.String"));
                dtDetails.Columns.Add("PatchWeek", Type.GetType("System.String"));

                if (Session["dtPlanData"] != null)
                {
                    DataTable dt = (DataTable)Session["dtPlanData"];
                    DataRow[] rows = dt.Select("IsGroupSelected='true'");

                    foreach (DataRow row in rows)
                    {
                        DataRow drGroup = dtDetails.NewRow();
                        drGroup[0] = row["ServerName"];
                        drGroup[1] = row["PatchMonth"];
                        drGroup[3] = row["PatchWeek"];
                        dtDetails.Rows.Add(drGroup);
                    }
                }
                if (dtDetails.Rows.Count > 0)
                {
                    int count = DataAccessLayer.UpdateGroupName(dtDetails, txtGroupName.Text, hdnUniqueID.Value);
                    if (count == 2)
                    {
                        lblGroupMsg.Text = "GroupName Already Exists";
                        lblGroupMsg.ForeColor = System.Drawing.Color.Red;

                    }
                    else if (count == 1)
                    {
                        lblGroupMsg.Text = "Successfully created the Group for the selected Servers";
                        lblGroupMsg.ForeColor = System.Drawing.Color.Green;
                    }
                }
                LoadGroupNames();
                GetPatchingScheduledResults(hdnUniqueID.Value);
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
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

                DataTable dtGroupName = DataAccessLayer.GetGroupNames(StartDate, EndDate, ddlPatchScenario.SelectedItem.Text);
                ddlGroupName.DataTextField = "groupname";
                ddlGroupName.DataValueField = "groupname";
                ddlGroupName.DataSource = dtGroupName;
                ddlGroupName.DataBind();
                ddlGroupName.Items.Insert(0, new ListItem("--All--", "0"));
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
            }
        }

        public string GetPatchMonth()
        {
            string monthName = string.Empty;
            string StartDate = string.Empty;
            string EndDate = string.Empty;
            try
            {


                int date = DateTime.Now.Day;
                if (date < 14)
                {
                    monthName = DateTime.Now.AddMonths(-1).ToString("MMM") + "-14";
                    StartDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + "-14";
                    EndDate = DateTime.Now.ToString("yyyy-MM") + "-14";
                }

                else
                {
                    monthName = DateTime.Now.ToString("MMM") + "-14";
                    StartDate = DateTime.Now.ToString("yyyy-MM") + "-14";
                    EndDate = DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-14";
                }

            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
            }
            return monthName;
        }
        protected void btnOverride_Click(object sender, EventArgs e)
        {

        }

        protected void btnGetData_Click(object sender, EventArgs e)
        {
            txtGroupName.Text = "";
            txtDeferStartDate.Text = "";
            txtDeferStartDate.Enabled = false;
            txtDeferEndDate.Text = "";
            txtDeferEndDate.Enabled = false;
            txtdefer.Text = "";
            txtdefer.Enabled = false;
            PPMode.SelectedIndex = 0;
            chkScheduleConfirmation.Checked = false;
            rblDefer.ClearSelection();
            GetPatchingScheduledResults(hdnUniqueID.Value);
            lblScheduleMsg.Text = "";
            lblGroupMsg.Text = "";
        }

        public bool GetCheckedValue(string Value)
        {
            bool strResult = false;
            if (Value == "True")
                strResult = true;
            return strResult;
        }


        protected void gvResults_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkScheduled = (CheckBox)e.Row.FindControl("chkScheduled");
                    TextBox txtStartDate = (TextBox)e.Row.FindControl("txtScheduleStartDate");
                    TextBox txtEndDate = (TextBox)e.Row.FindControl("txtScheduleEndDate");
                   
                    if (chkScheduled.Checked)
                    {
                        txtStartDate.Enabled = true;
                        txtEndDate.Enabled = true;
                      


                    }
                    else
                    {
                        txtStartDate.Enabled = false;
                        txtEndDate.Enabled = false;
                        txtStartDate.Text = txtEndDate.Text = "";                   

                    }
                }
            }

            catch (Exception ex)
            {
                Helper.WriteError(ex);
            }
        }


        //--flag scheduleall-

        //--old code
        //protected void btnSaveSchedule_Click(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        int scheduleCount = 0;
        //        hdnUniqueID.Value = Guid.NewGuid().ToString();
        //        gvResults_PageIndexChanging(sender, new GridViewPageEventArgs(gvResults.PageIndex));
        //        DataTable dtScheduleDetails = new DataTable();
        //        dtScheduleDetails.Columns.Add("ServerName", Type.GetType("System.String"));
        //        dtScheduleDetails.Columns.Add("UniqueID", Type.GetType("System.String"));
        //        dtScheduleDetails.Columns.Add("StartDateTime", Type.GetType("System.String"));
        //        dtScheduleDetails.Columns.Add("EndDateTime", Type.GetType("System.String"));
        //        dtScheduleDetails.Columns.Add("IsScheduled", typeof(bool));
        //        dtScheduleDetails.Columns.Add("patchMonth", typeof(DateTime));
        //        dtScheduleDetails.Columns.Add("Defer", typeof(int));
        //        dtScheduleDetails.Columns.Add("DeferStartDateTime", Type.GetType("System.String"));
        //        dtScheduleDetails.Columns.Add("DeferEndDateTime", Type.GetType("System.String"));
        //        dtScheduleDetails.Columns.Add("PatchScenario", Type.GetType("System.String"));
        //        if (Session["dtPlanData"] != null)
        //        {
        //            DataTable dt = (DataTable)Session["dtPlanData"];

        //            foreach (DataRow row in dt.Rows)
        //            {
        //                DataRow drSchedule = dtScheduleDetails.NewRow();
        //                drSchedule[0] = row["ServerName"].ToString();
        //                drSchedule[1] = hdnUniqueID.Value;
        //                drSchedule[5] = row["PatchMonthValue"];

        //                if (chkScheduleConfirmation.Checked == true)
        //                {
        //                    string[] strdate = new string[2];
        //                    strdate[0] = row[1].ToString();
        //                    strdate[1] = row[2].ToString();
        //                    drSchedule[2] = strdate[0].Replace("AM", "").Replace("PM", "").Replace("PT", "");
        //                    drSchedule[3] = strdate[1].Replace("AM", "").Replace("PM", "").Replace("PT", "");
        //                    drSchedule[4] = true;
        //                    drSchedule[9] = PPMode.SelectedItem.Value;
        //                }
        //                else
        //                {
        //                    if (row["isscheduled"] != null && !string.IsNullOrWhiteSpace(row["isscheduled"].ToString()) && Convert.ToBoolean(row["isscheduled"]) == true)
        //                    {
        //                        scheduleCount = 1;
        //                        drSchedule[9] = PPMode.SelectedItem.Value;

        //                    }
        //                    else
        //                    {
        //                        drSchedule[4] = "false";
        //                        drSchedule[9] = "NULL";
        //                    }

        //                    drSchedule[4] = row["isscheduled"];
        //                    drSchedule[2] = row["startdatetime"];
        //                    drSchedule[3] = row["enddatetime"];
        //                }

        //                if (rblDefer.SelectedValue == "1")
        //                {
        //                    //drSchedule[6] = Convert.ToInt32(ddlDefer.SelectedValue);
        //                    drSchedule[6] = Convert.ToInt32(txtdefer.Text);
        //                    drSchedule[7] = row["startdatetime"];
        //                    drSchedule[8] = row["enddatetime"];
        //                    drSchedule[9] = PPMode.SelectedItem.Value;
        //                }
        //                else if (rblDefer.SelectedValue == "0")
        //                {
        //                    drSchedule[6] = 0;
        //                    drSchedule[7] = txtDeferStartDate.Text;
        //                    drSchedule[8] = txtDeferEndDate.Text;
        //                    drSchedule[9] = PPMode.SelectedItem.Value;
        //                }

        //                else
        //                {
        //                    drSchedule[6] = 0;
        //                    drSchedule[7] = row["startdatetime"];
        //                    drSchedule[8] = row["enddatetime"];
        //                }
        //                dtScheduleDetails.Rows.Add(drSchedule);
        //            }

        //            if (dtScheduleDetails.Rows.Count > 0)
        //            {
        //                int result = DataAccessLayer.UpdateSchedule(dtScheduleDetails);
        //                if (scheduleCount > 0)
        //                {
        //                    if (result == 1 || result == 2)
        //                    {
        //                        lblScheduleMsg.Text = "Selected servers scheduled Successfully";
        //                        lblScheduleMsg.ForeColor = System.Drawing.Color.Green;
        //                    }
        //                    else
        //                    {
        //                        lblScheduleMsg.Text = "Problem Occured While Scheduling the Selected Servers";
        //                        lblScheduleMsg.ForeColor = System.Drawing.Color.Red;
        //                    }
        //                }
        //            }
        //        }
        //        //LoadControls();  
        //        GetPatchingScheduledResults(hdnUniqueID.Value);
        //    }
        //    catch (Exception ex)
        //    {
        //        Helper.WriteError(ex);
        //    }
        //}

        //--New CODE
        protected void btnSaveSchedule_Click(object sender, EventArgs e)
        {

            try
            {
                int scheduleCount = 0;
                hdnUniqueID.Value = Guid.NewGuid().ToString();
                gvResults_PageIndexChanging(sender, new GridViewPageEventArgs(gvResults.PageIndex));
                DataTable dtScheduleDetails = new DataTable();
                dtScheduleDetails.Columns.Add("ServerName", Type.GetType("System.String"));
                dtScheduleDetails.Columns.Add("UniqueID", Type.GetType("System.String"));
                dtScheduleDetails.Columns.Add("StartDateTime", Type.GetType("System.String"));
                dtScheduleDetails.Columns.Add("EndDateTime", Type.GetType("System.String"));
                dtScheduleDetails.Columns.Add("IsScheduled", typeof(bool));
                dtScheduleDetails.Columns.Add("patchMonth", typeof(DateTime));
                dtScheduleDetails.Columns.Add("Defer", typeof(int));
                dtScheduleDetails.Columns.Add("DeferStartDateTime", Type.GetType("System.String"));
                dtScheduleDetails.Columns.Add("DeferEndDateTime", Type.GetType("System.String"));
                dtScheduleDetails.Columns.Add("PatchScenario", Type.GetType("System.String"));
                if (Session["dtPlanData"] != null)
                {
                    DataTable dt = (DataTable)Session["dtPlanData"];
                    foreach (DataRow row in dt.Rows)
                    {
                        DataRow drSchedule = dtScheduleDetails.NewRow();
                        drSchedule[0] = row["ServerName"].ToString();
                        drSchedule[1] = hdnUniqueID.Value;
                        drSchedule[5] = row["PatchMonthValue"];

                        if (chkScheduleConfirmation.Checked == true)
                        {
                            string[] strdate = new string[2];
                            strdate[0] = row[1].ToString();
                            strdate[1] = row[2].ToString();
                            drSchedule[2] = strdate[0].Replace("AM", "").Replace("PM", "").Replace("PT", "");
                            drSchedule[3] = strdate[1].Replace("AM", "").Replace("PM", "").Replace("PT", "");
                            drSchedule[4] = true;
                            drSchedule[9] = PPMode.SelectedItem.Value;
                        }
                        else
                        {
                            foreach (GridViewRow row1 in gvResults.Rows)
                            {
                                CheckBox chkScheduled = (CheckBox)row1.FindControl("chkScheduled");

                                if (chkScheduled.Checked == true)
                                {
                                    drSchedule[4] = false;
                                }
                                else
                                {
                                    drSchedule[4] = row["isscheduled"];
                                }
                            }

                            if (row["isscheduled"] != null && !string.IsNullOrWhiteSpace(row["isscheduled"].ToString()) && Convert.ToBoolean(row["isscheduled"]) == true)
                            {
                                scheduleCount = 1;
                                drSchedule[4] = true;
                                drSchedule[9] = PPMode.SelectedItem.Value;

                            }
                            else
                            {
                                //drSchedule[4] = "false";
                                drSchedule[9] = "NULL";
                            }
                            //drSchedule[4] = row["isscheduled"];
                            drSchedule[2] = row["startdatetime"];
                            drSchedule[3] = row["enddatetime"];
                        }

                        if (rblDefer.SelectedValue == "1")
                        {
                            //drSchedule[6] = Convert.ToInt32(ddlDefer.SelectedValue);
                            drSchedule[6] = Convert.ToInt32(txtdefer.Text);
                            drSchedule[7] = row["startdatetime"];
                            drSchedule[8] = row["enddatetime"];
                            drSchedule[9] = PPMode.SelectedItem.Value;
                        }
                        else if (rblDefer.SelectedValue == "0")
                        {
                            drSchedule[6] = 0;
                            drSchedule[7] = txtDeferStartDate.Text;
                            drSchedule[8] = txtDeferEndDate.Text;
                            drSchedule[9] = PPMode.SelectedItem.Value;
                        }
                        else
                        {
                            drSchedule[6] = 0;
                            drSchedule[7] = row["startdatetime"];
                            drSchedule[8] = row["enddatetime"];

                        }
                        dtScheduleDetails.Rows.Add(drSchedule);
                    }

                    if (dtScheduleDetails.Rows.Count > 0)
                    {
                        int result = DataAccessLayer.UpdateSchedule(dtScheduleDetails);
                        if (scheduleCount > 0)
                        {
                            if (result == 1 || result == 2)
                            {
                                lblScheduleMsg.Text = "Selected servers scheduled Successfully";
                                lblScheduleMsg.ForeColor = System.Drawing.Color.Green;
                            }
                            else
                            {
                                lblScheduleMsg.Text = "Problem Occured While Scheduling the Selected Servers";
                                lblScheduleMsg.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }
                }
                //LoadControls();  
                GetPatchingScheduledResults(hdnUniqueID.Value);
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
            }
        }

        #endregion

        protected void ddlOrgName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSubBPU.Items.Clear();
            if (ddlOrgName.SelectedItem.Text != "--All--")
            {
                DataSet dsSubBPUs = DataAccessLayer.GetSubBPUs(ddlOrgName.SelectedItem.Text);
                ddlSubBPU.DataTextField = "org2";
                ddlSubBPU.DataValueField = "org2";
                ddlSubBPU.DataSource = dsSubBPUs.Tables[0];
                ddlSubBPU.DataBind();
            }
            ddlSubBPU.Items.Insert(0, new ListItem("--All--", "0"));
            gvResults.Visible = false;
            trSave.Visible = false;
            trSaveBottom.Visible = false;
            //btnGetData_Click(sender, new EventArgs());
        }

        protected void gvResults_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            DataTable dt = (DataTable)Session["dtPlanData"];
            DataRow[] dr;

            foreach (GridViewRow row in gvResults.Rows)
            {
                string strServerName = "";
                Label lblServerName = (Label)row.FindControl("lblServerName");
                strServerName = lblServerName.Text;
                Label lblScheduledWindow = (Label)row.FindControl("lblScheduledWindow");
                string[] arrSchedule = lblScheduledWindow.Text.Split('-');
                CheckBox chkScheduled = (CheckBox)row.FindControl("chkScheduled");
                CheckBox chkGroup = (CheckBox)row.FindControl("chkGroup");
                TextBox txtStartDate = (TextBox)row.FindControl("txtScheduleStartDate");
                TextBox txtEndDate = (TextBox)row.FindControl("txtScheduleEndDate");
                Label lblPatchMonth = (Label)row.FindControl("lblPatchMonth");
                HiddenField hdnMonth = (HiddenField)row.FindControl("hdnMonth");
                Label lblID = (Label)row.FindControl("lblUniqueId");

                dr = dt.Select("ID='" + lblID.Text + "'");

                if (chkScheduled.Checked == true)
                {
                    if (txtStartDate.Text == string.Empty && arrSchedule.Length > 0 && arrSchedule[0] != string.Empty)
                        dr[0]["startdatetime"] = arrSchedule[0].Replace("PM", "").Replace("AM", "").Replace("PT", "").Trim();
                    else
                        dr[0]["startdatetime"] = txtStartDate.Text;

                    if (txtEndDate.Text == string.Empty && arrSchedule.Length > 1 && arrSchedule[1] != string.Empty)
                        dr[0]["enddatetime"] = arrSchedule[1].Replace("PM", "").Replace("AM", "").Replace("PT", "").Trim();
                    else
                        dr[0]["enddatetime"] = txtEndDate.Text;
                }

                dr[0]["isscheduled"] = chkScheduled.Checked;
                dr[0]["IsGroupSelected"] = chkGroup.Checked;
            }

            gvResults.PageIndex = e.NewPageIndex;
            gvResults.DataSource = dt;
            gvResults.DataBind();

            int rowcount = dt.Rows.Count;
            GetPageServerCount(rowcount);

            trSave.Visible = true;
            trSaveBottom.Visible = true;
            PPMode.Visible = true;
            Session["dtPlanData"] = dt;
        }

        private void GetPageServerCount(int totalrowcount)
        {

            int pagecount = gvResults.PageCount;
            int pgno = gvResults.PageIndex;

            if (pgno == 0)
            {
                if (totalrowcount < 25)
                {
                    lblServerCount.Text = "Server Count: " + Convert.ToString(totalrowcount) + " of " + Convert.ToString(totalrowcount);
                }
                else
                {
                    lblServerCount.Text = "Server Count: " + Convert.ToString((((pgno) * 25) + 1) + "-" + (((pgno + 1) * 25))) + " of " + Convert.ToString(totalrowcount);
                }
            }
            else if (pgno == (pagecount - 1))
            {

                lblServerCount.Text = "Server Count: " + Convert.ToString((((pgno) * 25) + 1) + "-" + totalrowcount) + " of " + Convert.ToString(totalrowcount);
            }
            else
            {
                lblServerCount.Text = "Server Count: " + Convert.ToString((((pgno) * 25) + 1) + "-" + (((pgno + 1) * 25))) + " of " + Convert.ToString(totalrowcount);
            }
        }

        //protected void ddlPatchScenario_SelectedIndexChange(object sender, EventArgs e)
        //{
        //    string StartDate = DateTime.Now.ToString();
        //    string EndDate = DateTime.Now.ToString();

        //    string strSelectedDate = ddlPatchMonth.SelectedValue.ToString();
        //    string[] strResult = strSelectedDate.Split('-');

        //    DataTable dtGroupName = DataAccessLayer.GetGroupNames(StartDate, EndDate, ddlPatchScenario.SelectedItem.Text);
        //    ddlGroupName.DataTextField = "groupname";
        //    ddlGroupName.DataValueField = "groupname";
        //    ddlGroupName.DataSource = dtGroupName;
        //    ddlGroupName.DataBind();
        //    ddlGroupName.Items.Insert(0, new ListItem("--All--", "0"));
        //}

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