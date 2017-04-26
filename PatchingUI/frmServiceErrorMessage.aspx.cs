using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Diagnostics;
using System.Web.UI.WebControls;
using System.ServiceProcess;
using System.Configuration;

namespace PatchingUI
{
    public partial class frmServiceErrorMessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region timerSrvcChk_Tick

        /*protected void timerSrvcChk_Tick(object sender, EventArgs e)
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
                    lblServiceErrorMessage.Text = "Service is Restarting, Please try after sometime.";
                }
                else if (srvorunprogram.Status.Equals(ServiceControllerStatus.Running))
                {
                    lblServiceErrorMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("PP Service Error Message", ex.Message, EventLogEntryType.Error);
            }
        }
        */
        #endregion

    }


}