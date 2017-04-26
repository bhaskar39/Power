using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using PatchingToolUI;

namespace PatchingUI.UserControls
{
    public partial class MSCSHLBGrid : System.Web.UI.UserControl
    {
        public event EventHandler chkFailbackClick;
        public event GridViewRowEventHandler GridOnRowDataBound;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public object DataSource
        {
            get
            {
                return gvStandaloneMSCSHLB.DataSource;
            }
            set
            {
                gvStandaloneMSCSHLB.DataSource = value;
            }
        }

        public void DataBind()
        {
            gvStandaloneMSCSHLB.DataBind();
        }

        public string GetFormatedData(string value)
        {
            return value.Replace("_", " ").Replace(",", ", ");
        }

        public bool GetCheckedResult(string Value)
        {
            bool strResult = false;
            if (Value == "1")
                strResult = true;
            return strResult;
        }

        public System.Drawing.Color GetColor(string Value)
        {
            if (Value.ToLower().Contains("success"))
                return System.Drawing.Color.Green;
            else if (Value.ToLower().Contains("inprogress"))
                return System.Drawing.Color.Orange;
            else
                return System.Drawing.Color.Red;
        }

        public System.Drawing.Color GetExtensionColor(string Value)
        {
            if (Value.ToLower() == ("yes"))
                return System.Drawing.Color.Red;
            else
                return System.Drawing.Color.Green;
        }

        public void ChkFailBack_Clicked(Object sender, EventArgs e)
        {
            //if (chkFailbackClick != null)
                //chkFailbackClick(sender, e);
            ((Execute)this.Page).CheckFailBack(sender, gvStandaloneMSCSHLB);
        }

        protected void btnResume_Click(object sender, EventArgs e)
        {
            ((Execute)this.Page).GridResumeButton(sender, gvStandaloneMSCSHLB);
        }

        protected void gvStandaloneMSCSHLB_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (GridOnRowDataBound != null)
                GridOnRowDataBound(sender, e);
        }

        public void Persist_gvClusterExecute_State(string UniqueID)
        {
            string RunValidationFlag = "0";
            string CheckCheckBox = "1";
            string ForceStandaloneFlag = "0";
            string Nodename = string.Empty;
            string PauseFlag = "0";
            string PauseValue = string.Empty;
            string FailbackFlag = "0";
            string BackupNode = string.Empty;
            string ClusterType = string.Empty;
            int result = 0;

            try
            {
                foreach (GridViewRow row in gvStandaloneMSCSHLB.Rows)
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

                    Label lblClusterType = (Label)row.FindControl("lblClusterType");
                    if (lblClusterType != null)
                        ClusterType = lblClusterType.Text;

                    if (!string.IsNullOrWhiteSpace(Nodename))
                    {
                        if (ClusterType != "HLB")
                            result = DataAccessLayer.UpdateNodeInfo(UniqueID, Nodename, PauseFlag, PauseValue, FailbackFlag, ForceStandaloneFlag, BackupNode, RunValidationFlag);
                        else
                            result = DataAccessLayer.UpdateHLBCheckValue(UniqueID, Nodename, CheckCheckBox, PauseValue, RunValidationFlag);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}