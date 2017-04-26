using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PatchingUI.VipService;
//using PatchingUI.MyVipsSuperUser;
namespace PatchingUI
{
    public class VipDetails
    {
        VipService.UserHLBMgmtSvc objService = null;
        // MyVipsSuperUser.UserHLBMgmtSvcSoap objSuperUser = null;
        #region GetVipStatus
        /// <summary>
        /// method to get VipStatus
        /// </summary>
        /// <author>Sudha</author>
        /// <CreatedDate>10/15/2013</CreatedDate>
        /// <returns></returns>
        public void GetVipStatus(string strAuthToken, string strVip)
        //   public void GetVipStatus()
        {
            objService = new UserHLBMgmtSvc();
            objService.GetVIPStatus(strAuthToken, "10.248.26.83");
            //objSuperUser = new UserHLBMgmtSvcSoap();
            // objSuperUser.GetVIPsForSuperUser();
        }

        #endregion

        #region EnableVipNode
        /// <summary>
        /// method to Enable Vip Node
        /// </summary>
        /// <author>Sudha</author>
        /// <CreatedDate>10/15/2013</CreatedDate>
        /// <returns></returns>
        public void EnableVipNode(string strAuthToken, string strVip, string strNodeIP, long port)
        {
            objService = new UserHLBMgmtSvc();
            objService.EnableVIPNode(strAuthToken, strVip, strNodeIP, port);
        }

        #endregion


        #region DisableVipNode
        /// <summary>
        /// method to Disable Vip Node
        /// </summary>
        /// <author>Sudha</author>
        /// <CreatedDate>10/15/2013</CreatedDate>
        /// <returns></returns>
        public void DisableVipNode(string strAuthToken, string strVip, string strNodeIP, long port)
        {
            objService = new UserHLBMgmtSvc();
            objService.DisableVIPNode(strAuthToken, strVip, strNodeIP, port);
        }

        #endregion

        #region AddNode
        /// <summary>
        /// method to AddNode
        /// </summary>
        /// <author>Sudha</author>
        /// <CreatedDate>10/15/2013</CreatedDate>
        /// <returns></returns>
        public void AddNode(string strAuthToken, string strVip, string strNodeIP)
        {
            objService = new UserHLBMgmtSvc();
            objService.AddNode(strAuthToken, strVip, strNodeIP);
        }

        #endregion

        //#region DisableNode
        ///// <summary>
        ///// method to DisableNode
        ///// </summary>
        ///// <author>Sudha</author>
        ///// <CreatedDate>10/15/2013</CreatedDate>
        ///// <returns></returns>
        //public void DisableNode(string strAuthToken, string strVip, string strNodeIP)
        //{
        //    objService = new UserHLBMgmtSvc();
        //   // objService.disab
        //}

        //#endregion
    }
}