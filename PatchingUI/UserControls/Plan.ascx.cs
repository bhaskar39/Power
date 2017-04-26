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

namespace PatchingUI.UserControls
{
    public partial class Plan : System.Web.UI.UserControl
    {
        DataTable dtPlanData = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadControls();
                LoadGroupNames();
                // tbxFromDate.Attributes.Add("readonly", "readonly");
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
                            //if (dsData.Tables[0].Columns[0].ToString() == "ServerName" && dsData.Tables[0].Columns[1].ToString() == "Priority")
                            //{
                            ReadExcel(filename, uniqueGUID, strTypeofServerlist);
                            hdnUniqueID.Value = uniqueGUID;
                            //}

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
               // ddlOrgName.Items.Insert(1, new ListItem("UNK", "unk"));

                //ddlPatchScenario.DataTextField = "PatchingScenario";
                //ddlPatchScenario.DataValueField = "PatchingScenario";
                //ddlPatchScenario.DataSource = dsPlanControlsData.Tables[1];
                //ddlPatchScenario.DataBind();
                //ddlPatchScenario.Items.Insert(0, new ListItem("--All--", "0"));

                //ddlMode.DataTextField = "Mode";
                //ddlMode.DataValueField = "Mode";
                //ddlMode.DataSource = dsPlanControlsData.Tables[2];
                //ddlMode.DataBind();
                //ddlMode.Items.Insert(0, new ListItem("--All--", "0"));

                ddlServerlistType.DataTextField = "TypeofServerlist";
                ddlServerlistType.DataValueField = "TypeofServerlist";
                ddlServerlistType.DataSource = dsPlanControlsData.Tables[3];
                ddlServerlistType.DataBind();
                ddlServerlistType.Items.Insert(0, new ListItem("--All--", "0"));

                //ddlPatchWeek.DataTextField = "Patchweek";
                //ddlPatchWeek.DataValueField = "Patchweek";
                //ddlPatchWeek.DataSource = dsPlanControlsData.Tables[4];
                //ddlPatchWeek.DataBind();
                //ddlPatchWeek.Items.Insert(0, new ListItem("--All--", "0"));

                ddlPatchMonth.DataTextField = "PatchMonth";
                ddlPatchMonth.DataValueField = "PatchDate";
                ddlPatchMonth.DataSource = dsPlanControlsData.Tables[5];
                ddlPatchMonth.DataBind();
                string strPatchMonth = GetPatchMonth();
               // ddlPatchMonth.Items.Insert(0, new ListItem("--All--", "0"));

              //  ddlPatchMonth.Items.FindByValue(strPatchMonth).Selected = true;
                ddlPatchMonth.Items.FindByText(strPatchMonth).Selected = true;

                //ddlSubBPU.DataTextField = "org2";
                //ddlSubBPU.DataValueField = "org2";
                //ddlSubBPU.DataSource = dsPlanControlsData.Tables[6];
                //ddlSubBPU.DataBind();
                ddlSubBPU.Items.Insert(0, new ListItem("--All--", "0"));
                //ddlSubBPU.Items.Insert(1, new ListItem("UNK", "unk"));


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
                //ddlSubBPU.Items.Insert(0, new ListItem("--All--", "0"));
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
                //DataTable dtPlanData = new DataTable();
                //DateTime dtSelectedPAtch = new DateTime();
                //dtSelectedPAtch = Convert.ToDateTime(ddlPatchMonth.SelectedValue.ToString());

                FilterGridData();

                    if (dtPlanData.Rows.Count == 0)
                    {
                        dtPlanData.Rows.Add(dtPlanData.NewRow());
                        // ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());

                        gvResults.DataSource = dtPlanData;

                        gvResults.DataBind();

                        int columncount = gvResults.Rows[0].Cells.Count;

                        gvResults.Rows[0].Cells.Clear();

                        gvResults.Rows[0].Cells.Add(new TableCell());

                        gvResults.Rows[0].Cells[0].ColumnSpan = columncount;

                        gvResults.Rows[0].Cells[0].Text = "No Records Found";

                        trSave.Visible = false;
                        trSaveBottom.Visible = false;
                      //  trDefer.Visible = false;
                        // trSave.Attributes.Add("style", "display:none");
                    }

                    else
                    {

                        gvResults.DataSource = dtPlanData;
                        gvResults.DataBind();

                        trSave.Visible = true;
                        trSaveBottom.Visible = true;
                        //ChkDefer.Visible = true;
                       // ddlDefer.Visible = true;

                      //  trDefer.Visible = true;
                        // trSave.Attributes.Add("style", "display:block");


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

                //strFilter = " Order By ServerName";
                dtPlanData = DataAccessLayer.GetPlanData(strFilter);

                DataColumn dcGroup = new DataColumn("IsGroupSelected", typeof(bool));
                dtPlanData.Columns.Add(dcGroup);

                //DataColumn dcSchedule = new DataColumn("IsScheduleSelected", typeof(bool));
                //dtPlanData.Columns.Add(dcSchedule);

                //DataColumn dcStartDate = new DataColumn("StartDate");
                //dtPlanData.Columns.Add(dcStartDate);

                //DataColumn dcEndDate = new DataColumn("EndDate");
                //dtPlanData.Columns.Add(dcEndDate);

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

                //foreach (GridViewRow row in gvResults.Rows)
                //{
                //    string strServerName = "";
                //    string strPatchMonth = "";
                //    CheckBox chkGroup = (CheckBox)row.FindControl("chkGroup");
                //    Label lblServerName = (Label)row.FindControl("lblServerName");
                //    strServerName = lblServerName.Text;
                //    if (chkGroup.Checked)
                //    {
                //        DataRow dr = dtDetails.NewRow();

                //        Label lblPatchMonth = (Label)row.FindControl("lblPatchMonth");
                //        HiddenField hdnMonth = (HiddenField)row.FindControl("hdnMonth");
                //        Label lblPatchWeek = (Label)row.FindControl("lblPatchWeek");
                        
                //        strPatchMonth = lblPatchMonth.Text;
                //      //  strPatchMonth = hdnMonth.Value;
                //        dr[0] = strServerName;
                //        dr[1] = strPatchMonth;
                //        dr[3] = lblPatchWeek.Text;
                //        dtDetails.Rows.Add(dr);
                //    }
                   
                //}

                if (Session["dtPlanData"] != null)
                {
                    DataTable dt = (DataTable)Session["dtPlanData"];
                    DataRow[] rows = dt.Select("IsGroupSelected='true'");

                    foreach (DataRow row in rows)
                    {
                        DataRow drGroup = dtDetails.NewRow();
                        drGroup[0] = row["ServerName"];
                        drGroup[1]=row["PatchMonth"];
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
               
              //  LoadControls();               
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

                DropDownList ddlScenario = (DropDownList)Parent.FindControl("ddlScenario");
                DropDownList ddlPrepScenario = (DropDownList)Parent.FindControl("ddlPrepScenario");

                DataTable dtPrepGroupName = DataAccessLayer.GetGroupNames(StartDate, EndDate, null);
                DropDownList ddlPrepNames=(DropDownList) Parent.FindControl("ddlPrepNames");
                ddlPrepNames.DataTextField = "groupname";
                ddlPrepNames.DataValueField = "groupname";
                ddlPrepNames.DataSource = dtPrepGroupName;
                ddlPrepNames.DataBind();
                ddlPrepNames.Items.Insert(0, new ListItem("Select", "0"));

                DataTable dtExecuteGroupName = DataAccessLayer.GetGroupNames(StartDate, EndDate, null);
                DropDownList ddlExecuteNames = (DropDownList)Parent.FindControl("ddlExecuteNames");
                ddlExecuteNames.DataTextField = "groupname";
                ddlExecuteNames.DataValueField = "groupname";
                ddlExecuteNames.DataSource = dtExecuteGroupName;
                ddlExecuteNames.DataBind();
                ddlExecuteNames.Items.Insert(0, new ListItem("Select", "0"));

                DataTable dtValidateGroupName = DataAccessLayer.GetGroupNames(StartDate, EndDate, null);
                DropDownList ddlValidateNames = (DropDownList)Parent.FindControl("ddlValidateNames");
                ddlValidateNames.DataTextField = "groupname";
                ddlValidateNames.DataValueField = "groupname";
                ddlValidateNames.DataSource = dtValidateGroupName;
                ddlValidateNames.DataBind();
                ddlValidateNames.Items.Insert(0, new ListItem("Select", "0"));

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
                    //monthName = DateTime.Now.AddMonths(-1).ToString("MMM-yy");
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
           // int count = 0;
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
                        //count=1;
                    }
                    else
                    {
                        txtStartDate.Enabled = false;
                        txtEndDate.Enabled = false;
                        txtStartDate.Text = txtEndDate.Text = "";
                    }
                }
                //if (count == 1)
                //{
                //    trSave.Attributes.Add("style", "display:block");
                //}
                //else
                //{
                //    trSave.Attributes.Add("style", "display:none");
                //}
            }

            catch (Exception ex)
            {
                Helper.WriteError(ex);
            }
        }



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
                //dtScheduleDetails.Columns.Add("StartDateTime", typeof(DateTime));
                //dtScheduleDetails.Columns.Add("EndDateTime", typeof(DateTime));
                dtScheduleDetails.Columns.Add("StartDateTime", Type.GetType("System.String"));
                dtScheduleDetails.Columns.Add("EndDateTime", Type.GetType("System.String"));                
               dtScheduleDetails.Columns.Add("IsScheduled", typeof(bool));            
                dtScheduleDetails.Columns.Add("patchMonth", typeof(DateTime));
                //dtScheduleDetails.Columns.Add("Defer", Type.GetType("System.String"));
                dtScheduleDetails.Columns.Add("Defer", typeof(int));
                if (Session["dtPlanData"] != null)
                {
                    DataTable dt = (DataTable)Session["dtPlanData"];
                    // DataRow[] rows = dt.Select("isscheduled='true'");
                    DateTime Startdate=new DateTime();
                    DateTime Enddate = new DateTime();
                    foreach (DataRow row in dt.Rows)
                    {
                        DataRow drSchedule = dtScheduleDetails.NewRow();
                        if (row["isscheduled"]!=null && !string.IsNullOrWhiteSpace(row["isscheduled"].ToString()) && Convert.ToBoolean(row["isscheduled"]) == true)
                        {
                            scheduleCount = 1;
                            
                        }
                        else
                            drSchedule[4] = "false";

                        drSchedule[4] = row["isscheduled"];
                        drSchedule[0] = row["ServerName"].ToString();
                        drSchedule[1] = hdnUniqueID.Value;
                        //if (ChkDefer.Checked && !string.IsNullOrWhiteSpace(row["startdatetime"].ToString()) && !string.IsNullOrWhiteSpace(row["enddatetime"].ToString()))
                        //{
                        //    Startdate = (DateTime)row["startdatetime"];
                        //    Startdate = Startdate.AddHours(Convert.ToDouble(txtDefer.Text));
                        //    drSchedule[2] = Startdate;
                        //    Enddate = (DateTime)row["enddatetime"];
                        //    Enddate = Enddate.AddHours(Convert.ToDouble(txtDefer.Text));
                        //    drSchedule[3] = Enddate;
                        //}
                        //else
                        //{
                        //    drSchedule[2] = row["startdatetime"];
                        //    drSchedule[3] = row["enddatetime"];
                        //}

                       
                        drSchedule[2] = row["startdatetime"];
                        drSchedule[3] = row["enddatetime"];                       
                        drSchedule[5] = row["PatchMonthValue"];
                        if (ChkDefer.Checked)

                            drSchedule[6] = Convert.ToInt32(txtDefer.Text);
                        else
                            drSchedule[6] = 0;
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
          //  ddlSubBPU.Items.Insert(1, new ListItem("UNK", "unk"));
           // btnGetData_Click(sender, new EventArgs());
        }

        protected void gvResults_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>ShowProgress()</script>");
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
                Label lblID= (Label)row.FindControl("lblUniqueId");

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
            trSave.Visible = true;
            trSaveBottom.Visible = true;
            Session["dtPlanData"] = dt;
        }

        protected void ddlPatchScenario_SelectedIndexChange(object sender, EventArgs e)
        {
            string StartDate = DateTime.Now.ToString();
            string EndDate = DateTime.Now.ToString();

            string strSelectedDate = ddlPatchMonth.SelectedValue.ToString();
            string[] strResult = strSelectedDate.Split('-');

            DataTable dtGroupName = DataAccessLayer.GetGroupNames(StartDate, EndDate, ddlPatchScenario.SelectedItem.Text);
            ddlGroupName.DataTextField = "groupname";
            ddlGroupName.DataValueField = "groupname";
            ddlGroupName.DataSource = dtGroupName;
            ddlGroupName.DataBind();
            ddlGroupName.Items.Insert(0, new ListItem("--All--", "0"));
        }
    }
}
