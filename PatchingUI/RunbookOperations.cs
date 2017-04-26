using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
//using System.Data.Services.Client;
using PatchingToolUI;
using System.Configuration;
//using PatchingUI.PatchingService;
using System.Data.Services.Client;
//using PatchingUI.TK3PatchService;
//using PatchingUI.TK5stosrv01PatchService;
using PatchingUI.OrchestratorService;

namespace PatchingToolUI
{
    public class RunbookOperations
    {
        // PatchingUI.TK3PatchService.OrchestratorContext TK3context = null;
        PatchingUI.OrchestratorService.OrchestratorContext context = null;



        public string GetVIPsforSuperUser(string strRunbookPath)
        {
            // Details of runbook that we are going to run.
            //Guid runbookId = id;
            //Guid obj = Guid.NewGuid();
            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;





            try
            {

                result = ExecuteRunbook(strRunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }







            return result;
        }
        public string StartHLBExecuteRunbook(RunbookParams objParams)
        {

            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;
            parameterValues.Add("ExcludeQFE", objParams.ExcludeQFE);
            parameterValues.Add("CheckExcelOrText", objParams.CheckExcelORText);
            parameterValues.Add("SimpleUpdateOption", objParams.simpleUpdateOption);
            parameterValues.Add("OnlyQFE", objParams.OnlyQFE);
            parameterValues.Add("ScanORPatch", objParams.ScanOrPatch);
            parameterValues.Add("BPUOption", objParams.BPUOption);
            parameterValues.Add("LogPath", objParams.LogsPath);
            parameterValues.Add("IPAK", objParams.Ipak);
            parameterValues.Add("InputFileName", objParams.InputFilename);
            parameterValues.Add("MSNRebootFlag", objParams.MSNRebootFlag);
            parameterValues.Add("UniqueID", objParams.uniqueGUID);
            parameterValues.Add("PatchingOption", objParams.PatchingOption);
            parameterValues.Add("InputSource", objParams.InputSource);


            try
            {

                result = ExecuteRunbook(objParams.RunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }


            return result;
        }
        public string StartSuspendNodeRunbook(string strRunbookPath, string strVip, string strPassword, string strUserName, string strNodeIP, string strPort)
        {
            // LogPath,uniqueid, inputfilename, serverlist, checkExcelorText


            //Guid obj = Guid.NewGuid();
            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;
            parameterValues.Add("VIP", strVip);
            //  parameterValues.Add("AccountPassword", strPassword);
            //  parameterValues.Add("AccountName", strUserName);
            parameterValues.Add("NodeIP", strNodeIP);
            parameterValues.Add("Port", strPort);

            try
            {

                result = ExecuteRunbook(strRunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }


            return result;
        }
        //public string StartResumeNodeRunbook(string strRunbookPath, string strVip, string strUniqueID, string strNodeName,string strNodeIP,string strPort)
        public string StartResumeNodeRunbook(RunbookParams objParams)
        {
            // LogPath,uniqueid, inputfilename, serverlist, checkExcelorText


            //Guid obj = Guid.NewGuid();
            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;
            parameterValues.Add("UniqueID", objParams.uniqueGUID);
            parameterValues.Add("VIP", objParams.VIP);
            parameterValues.Add("NodeName", objParams.NodeName);
            parameterValues.Add("NodeIP", objParams.NodeIP);
            parameterValues.Add("Port", objParams.Port);
            try
            {

                result = ExecuteRunbook(objParams.RunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }


            return result;
        }
        public string StartGetVipStatusRunbook(string strRunbookPath, string strVip, string strPassword, string strUserName)
        {
            // LogPath,uniqueid, inputfilename, serverlist, checkExcelorText


            //Guid obj = Guid.NewGuid();
            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;
            parameterValues.Add("VIP", strVip);
            // parameterValues.Add("AccountName", strUserName);
            //   parameterValues.Add("AccountPassword", strPassword);
            //  parameterValues.Add("AccountName", strUserName);




            try
            {

                result = ExecuteRunbook(strRunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }


            return result;
        }

        public string StartResumeRunbook(RunbookParams objParams)
        {
            // LogPath,uniqueid, inputfilename, serverlist, checkExcelorText


            //Guid obj = Guid.NewGuid();
            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;

            //parameterValues.Add("Services", "");
            //parameterValues.Add("FromUI", "1");
            //parameterValues.Add("UniqueId", uniqueGUID);
            //parameterValues.Add("ClusterName", "");
            //parameterValues.Add("PatchNode", PatchNode);
            //parameterValues.Add("Nodes", uniqueGUID);   
            parameterValues.Add("ClusterName", objParams.ServerName);
            parameterValues.Add("InputFileName", objParams.InputFilename);
            parameterValues.Add("CheckExcelOrText", objParams.CheckExcelORText);
            parameterValues.Add("UniqueId", objParams.uniqueGUID);
            parameterValues.Add("LogPath", objParams.LogsPath);
            parameterValues.Add("IPAK", objParams.Ipak);
            parameterValues.Add("OnlyQFE", objParams.OnlyQFE);
            parameterValues.Add("BPUOption", objParams.BPUOption);
            parameterValues.Add("SimpleUpdateOption", objParams.simpleUpdateOption);
            parameterValues.Add("MSNRebootFlag", objParams.MSNRebootFlag);
            parameterValues.Add("ScanORPatch", objParams.ScanOrPatch);
            parameterValues.Add("ExcludeQFE", objParams.ExcludeQFE);
            parameterValues.Add("PatchingOption", objParams.PatchingOption);
            try
            {

                result = ExecuteRunbook(objParams.RunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }


            return result;
        }

        public string StartExecuteRunbook(string strRunbookPath, string ServerList, string LogsPath, string uniqueGUID, string filename, string check)
        {
            // LogPath,uniqueid, inputfilename, serverlist, checkExcelorText


            //Guid obj = Guid.NewGuid();
            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;


            parameterValues.Add("ServerList", ServerList);
            parameterValues.Add("Log Path", LogsPath);
            parameterValues.Add("UniqueID", uniqueGUID);
            parameterValues.Add("InputFileName", filename);
            parameterValues.Add("CheckExcelORText", check);



            try
            {

                result = ExecuteRunbook(strRunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }


            return result;
        }

        public string StartNewRunbookWithParameters(string strRunbookPath, string PatchingOption, string LogsPath, string ScanOrPatch, string Ipak, string simpleUpdateOption, string uniqueGUID, string filename, string strRebootOption, string OnlyQFE, string ExcludeQFE, string BPUOption, string InputType, string InputSource)
        {
            // Details of runbook that we are going to run.

            //Guid obj = Guid.NewGuid();
            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;
            parameterValues.Add("Log Path", LogsPath);
            parameterValues.Add("ScanORPatch", ScanOrPatch);
            parameterValues.Add("SimpleUpdateOption", simpleUpdateOption);
            parameterValues.Add("PatchingOption", PatchingOption);
            parameterValues.Add("IPAK", Ipak);
            parameterValues.Add("UniqueID", uniqueGUID);
            parameterValues.Add("InputFileName", filename);
            parameterValues.Add("MSNRebootFlag", strRebootOption);
            parameterValues.Add("OnlyQFE", OnlyQFE);
            parameterValues.Add("ExcludeQFE", ExcludeQFE);
            parameterValues.Add("BPUOption", BPUOption);
            parameterValues.Add("InputType", InputType);
            parameterValues.Add("InputSource", InputSource);
            //parameterValues.Add("ServerList", ServerList);

            try
            {

                result = ExecuteRunbook(strRunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }


            return result;
        }


        public string StartValidateRunbook(string strRunbookPath, string ServerList, string LogsPath, string UniqueGUID, string FileName, string check)
        {
            // Details of runbook that we are going to run.
            //Guid runbookId = id;
            //Guid obj = Guid.NewGuid();
            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;

            parameterValues.Add("Log Path", LogsPath);
            //parameterValues.Add("Admin Account Name", DomainAcctNm); 
            //parameterValues.Add("Admin Account Password", DomainAcctPwd);
            parameterValues.Add("ServerList", ServerList);
            parameterValues.Add("UniqueID", UniqueGUID);
            parameterValues.Add("InputFileName", FileName);
            parameterValues.Add("CheckExcelORText", check);



            try
            {

                result = ExecuteRunbook(strRunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }







            return result;
        }

        public string StartValidateRunbook(string strRunbookPath, string ServerList, string LogsPath, string UniqueGUID, string FileName, string check, string GroupName)
        {
            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;

            parameterValues.Add("Log Path", LogsPath);
            parameterValues.Add("ServerList", ServerList);
            parameterValues.Add("UniqueID", UniqueGUID);
            parameterValues.Add("InputFileName", FileName);
            parameterValues.Add("CheckExcelORText", check);
            parameterValues.Add("GroupName", GroupName);

            try
            {
                result = ExecuteRunbook(strRunbookPath, context, parameterValues);
            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }
            return result;
        }

        public string ExecuteRunbook(string runbookPath, OrchestratorContext context, Hashtable parameterValues)
        {

            string result = string.Empty;
            string strServiceRoot = string.Empty;
            try
            {
                strServiceRoot = ConfigurationManager.AppSettings["OrchestratorServiceURL"].ToString();
                string flag = ConfigurationManager.AppSettings["OrchestratorServer"].ToString();
                context = new PatchingUI.OrchestratorService.OrchestratorContext(new Uri(strServiceRoot));
                // Set credentials to default or a specific user.
                if (flag.ToLower() == "tk5stosrv02")
                {
                    context.Credentials = new System.Net.NetworkCredential("orchestrator", "gcoauto@123", "tk5stosrv02");

                }
                else
                {
                    context.Credentials = System.Net.CredentialCache.DefaultCredentials;
                }
                Runbook runbook = context.Runbooks.Where(rbk => rbk.Path == runbookPath).FirstOrDefault();
                // Retrieve parameters for the runbook
                var runbookParams = context.RunbookParameters.Where(runbookParam => runbookParam.RunbookId == runbook.Id && runbookParam.Direction == "In");

                // Configure the XML for the parameters
                StringBuilder parametersXml = new StringBuilder();



                if (runbookParams != null)
                {
                    var s = runbookParams;
                    parametersXml.Append("<Data>");

                    foreach (var param in runbookParams)
                    {
                        parametersXml.AppendFormat("<Parameter><ID>{0}</ID><Value>{1}</Value></Parameter>", param.Id.ToString("B"), parameterValues[param.Name]);
                    }
                    parametersXml.Append("</Data>");
                }

                // Create new job and assign runbook Id and parameters.
                PatchingUI.OrchestratorService.Job job = new PatchingUI.OrchestratorService.Job();
                job.RunbookId = runbook.Id;
                job.Parameters = parametersXml.ToString();

                // Add newly created job.
                context.AddToJobs(job);
                context.SaveChanges();

                result = job.Id.ToString();
                Console.WriteLine("Successfully started runbook. Job ID: {0}", job.Id.ToString());





            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }

            return result;
        }

        public string StartValidateAddAdminRunbook(string strRunbookPath, string ServerList, string LogsPath, string AdminName, string Adminpassowrd, string uniqueGUID, string Filename, string Flag, string check)
        {
            // Details of runbook that we are going to run.

            //Guid obj = Guid.NewGuid();
            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;

            parameterValues.Add("Log Path", LogsPath);
            //parameterValues.Add("Admin Account Name", DomainAcctNm);
            //parameterValues.Add("Admin Account Password", DomainAcctPwd);
            parameterValues.Add("ServerList", ServerList);
            parameterValues.Add("ExistingAdminName", AdminName);
            parameterValues.Add("ExistingAdminPassword", Adminpassowrd);
            parameterValues.Add("UniqueID", uniqueGUID);
            parameterValues.Add("Flag", Flag);
            parameterValues.Add("InputFileName", Filename);
            parameterValues.Add("CheckExcelORText", check);

            try
            {

                result = ExecuteRunbook(strRunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }




            return result;
        }

        public string StartRunbookWithParameters(string strRunbookPath, string ServerList, string PatchingOption, string LogsPath, string ScanOrPatch, string Ipak, string simpleUpdateOption, string uniqueGUID, string filename, string strRebootOption, string check, string OnlyQFE, string ExcludeQFE, string BPUOption)
        {
            // Details of runbook that we are going to run.

            //Guid obj = Guid.NewGuid();
            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;


            parameterValues.Add("ServerList", ServerList);
            parameterValues.Add("Log Path", LogsPath);
            parameterValues.Add("ScanORPatch", ScanOrPatch);
            //parameterValues.Add("Admin Account Name", DomainAcctNm);
            parameterValues.Add("SimpleUpdateOption", simpleUpdateOption);
            parameterValues.Add("PatchingOption", PatchingOption);
            parameterValues.Add("IPAK", Ipak);
            //parameterValues.Add("Admin Account Password", DomainAcctPwd);
            parameterValues.Add("UniqueID", uniqueGUID);
            parameterValues.Add("InputFileName", filename);
            parameterValues.Add("MSNRebootFlag", strRebootOption);
            parameterValues.Add("CheckExcelOrText", check);
            parameterValues.Add("OnlyQFE", OnlyQFE);
            parameterValues.Add("ExcludeQFE", ExcludeQFE);
            parameterValues.Add("BPUOption", BPUOption);

            try
            {

                result = ExecuteRunbook(strRunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }


            return result;
        }

        public string StartRunbookWithParameters(string strRunbookPath, string ServerList, string PatchingOption, string LogsPath, string ScanOrPatch, string Ipak, string simpleUpdateOption, string uniqueGUID, string filename, string strRebootOption, string check, string OnlyQFE, string ExcludeQFE, string BPUOption, string GroupName)
        {
            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;

            parameterValues.Add("ServerList", ServerList);
            parameterValues.Add("Log Path", LogsPath);
            parameterValues.Add("ScanORPatch", ScanOrPatch);
            parameterValues.Add("SimpleUpdateOption", simpleUpdateOption);
            parameterValues.Add("PatchingOption", PatchingOption);
            parameterValues.Add("IPAK", Ipak);
            parameterValues.Add("UniqueID", uniqueGUID);
            parameterValues.Add("InputFileName", filename);
            parameterValues.Add("MSNRebootFlag", strRebootOption);
            parameterValues.Add("CheckExcelOrText", check);
            parameterValues.Add("OnlyQFE", OnlyQFE);
            parameterValues.Add("ExcludeQFE", ExcludeQFE);
            parameterValues.Add("BPUOption", BPUOption);
            parameterValues.Add("GroupName", GroupName);

            try
            {
                result = ExecuteRunbook(strRunbookPath, context, parameterValues);
            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }
            return result;
        }

        public string StartValidateVerifyPatchRunbook(string strRunbookPath, string ServerList, string KBNumbers, string uniqueGUID, string FileName, string Check)
        {
            // Details of runbook that we are going to run.
            //Guid obj = Guid.NewGuid();
            string result = null;
            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;


            //  parameterValues.Add("Admin Account Name", DomainAcctNm);
            //  parameterValues.Add("Admin Account Password", DomainAcctPwd);
            parameterValues.Add("ServerList", ServerList);
            parameterValues.Add("KBNumbers", KBNumbers);
            parameterValues.Add("UniqueID", uniqueGUID);
            parameterValues.Add("InputFileName", FileName);
            parameterValues.Add("CheckExcelORText", Check);

            try
            {

                result = ExecuteRunbook(strRunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }

            return result;
        }

        public string StartValidateVerifyPatchRunbook(string strRunbookPath, string ServerList, string KBNumbers, string uniqueGUID, string FileName, string Check, string GroupName)
        {
            string result = null;
            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;
            parameterValues.Add("ServerList", ServerList);
            parameterValues.Add("KBNumbers", KBNumbers);
            parameterValues.Add("UniqueID", uniqueGUID);
            parameterValues.Add("InputFileName", FileName);
            parameterValues.Add("CheckExcelORText", Check);
            parameterValues.Add("GroupName", GroupName);

            try
            {
                result = ExecuteRunbook(strRunbookPath, context, parameterValues);
            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }
            return result;
        }

        public string StartFindUpdatesRunbook(string strRunbookPath, string ServerList, string strStartDate, string strEndDate, string uniqueGUID, string FileName, string check)
        {
            // Details of runbook that we are going to run.

            //Guid obj = Guid.NewGuid();
            string result = null;
            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;


            // parameterValues.Add("Admin Account Name", DomainAcctNm);
            // parameterValues.Add("Admin Account Password", DomainAcctPwd);
            parameterValues.Add("ServerList", ServerList);
            parameterValues.Add("StartDate", strStartDate);
            parameterValues.Add("EndDate", strEndDate);
            parameterValues.Add("UniqueID", uniqueGUID);
            parameterValues.Add("InputFileName", FileName);
            parameterValues.Add("CheckExcelORText", check);
            try
            {

                result = ExecuteRunbook(strRunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }

            return result;
        }

        public string StartFindUpdatesRunbook(string strRunbookPath, string ServerList, string strStartDate, string strEndDate, string uniqueGUID, string FileName, string check, string GroupName)
        {
            string result = null;
            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;

            parameterValues.Add("ServerList", ServerList);
            parameterValues.Add("StartDate", strStartDate);
            parameterValues.Add("EndDate", strEndDate);
            parameterValues.Add("UniqueID", uniqueGUID);
            parameterValues.Add("InputFileName", FileName);
            parameterValues.Add("CheckExcelORText", check);
            parameterValues.Add("GroupName", GroupName);

            try
            {
                result = ExecuteRunbook(strRunbookPath, context, parameterValues);
            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }
            return result;
        }

        public string StartSmokeTestRunbook(string strRunbookPath, string ServerList, string LogsPath, string UniqueGUID, string LogComparision, string check, string GroupName)
        {
            // Details of runbook that we are going to run.

            //Guid obj = Guid.NewGuid();
            string result = null;
            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;


            parameterValues.Add("Log Path", LogsPath);
            //  parameterValues.Add("AdminAccountName", DomainAcctNm);
            // parameterValues.Add("AdminAccountPassword", DomainAcctPwd);
            parameterValues.Add("UniqueID", UniqueGUID);
            parameterValues.Add("ServerList", ServerList);
            parameterValues.Add("LogsComparision", LogComparision);
            parameterValues.Add("CheckExcelORText", check);
            parameterValues.Add("GroupName", GroupName);

            try
            {

                result = ExecuteRunbook(strRunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }
            return result;
        }
        public string StartCompareSmokeTestlogRunbook(string strRunbookPath, string LogsPath, string UniqueGUID)
        {
            // Details of runbook that we are going to run.

            //Guid obj = Guid.NewGuid();
            string result = null;
            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;


            parameterValues.Add("Log Path", LogsPath);
            // parameterValues.Add("AdminAccountName", DomainAcctNm);
            //   parameterValues.Add("AdminAccountPassword", DomainAcctPwd);
            parameterValues.Add("UniqueID", UniqueGUID);

            try
            {

                result = ExecuteRunbook(strRunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }

            return result;
        }


        public string StartRebootRunbook(string strRunbookPath, string ServerList, string strStartDate, string uniqueGUID, string FileName, string check)
        {
            // Details of runbook that we are going to run.

            //Guid obj = Guid.NewGuid();
            string result = null;
            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;


            // parameterValues.Add("Admin Account Name", DomainAcctNm);
            // parameterValues.Add("Admin Account Password", DomainAcctPwd);
            parameterValues.Add("ServersList", ServerList);
            parameterValues.Add("StartDate", strStartDate);

            parameterValues.Add("UniqueID", uniqueGUID);
            parameterValues.Add("InputFileName", FileName);
            parameterValues.Add("CheckExcelORText", check);
            try
            {

                result = ExecuteRunbook(strRunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }

            return result;
        }


        public string StartFlashRunbook(string strRunbookPath, string ServerList, string uniqueGUID, string FileName, string check, string GroupName)
        {
            // Details of runbook that we are going to run.

            //Guid obj = Guid.NewGuid();
            string result = null;
            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;


            // parameterValues.Add("Admin Account Name", DomainAcctNm);
            //  parameterValues.Add("Admin Account Password", DomainAcctPwd);
            parameterValues.Add("ServersList", ServerList);
            parameterValues.Add("UniqueID", uniqueGUID);
            parameterValues.Add("InputFileName", FileName);
            parameterValues.Add("CheckExcelORText", check);
            parameterValues.Add("GroupName", GroupName);
            try
            {

                result = ExecuteRunbook(strRunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }

            return result;
        }


        public string StartSimpleUpdatePreviewRunbook(string strRunbookPath, string ServerList, string strStartDate, string uniqueGUID, string FileName)
        {
            string result = null;
            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;

            parameterValues.Add("StartDate", strStartDate);

            parameterValues.Add("Serverlist", ServerList);
            parameterValues.Add("UniqueID", uniqueGUID);
            parameterValues.Add("InputFileName", FileName);
            parameterValues.Add("SimpleUpdateOptions", "preview");
            parameterValues.Add("Count", "1");
            try
            {

                result = ExecuteRunbook(strRunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }

            return result;
        }

        public string StartSimpleUpdatePreviewRunbook(string strRunbookPath, string ServerList, string strStartDate, string uniqueGUID, string FileName, string Check, string GroupName)
        {
            string result = null;
            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;

            parameterValues.Add("StartDate", strStartDate);
            parameterValues.Add("Serverlist", ServerList);
            parameterValues.Add("UniqueID", uniqueGUID);
            parameterValues.Add("InputFileName", FileName);
            parameterValues.Add("SimpleUpdateOptions", "preview");
            parameterValues.Add("Count", "1");
            parameterValues.Add("CheckExcelORText", Check);
            parameterValues.Add("GroupName", GroupName);

            try
            {
                result = ExecuteRunbook(strRunbookPath, context, parameterValues);
            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }
            return result;
        }

        #region Prep



        //public string StartMitigateRunbook(string strRunbookPath, string ServerList, string LogsPath, string AdminName, string Adminpassowrd, string uniqueGUID, string Filename, string Flag, string check)
        public string StartMitigateRunbook(RunbookParams objParams)
        {
            // Details of runbook that we are going to run.

            //Guid obj = Guid.NewGuid();
            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;

            parameterValues.Add("Log Path", objParams.LogsPath);
            //parameterValues.Add("Admin Account Name", DomainAcctNm);
            //parameterValues.Add("Admin Account Password", DomainAcctPwd);
            parameterValues.Add("ServerList", objParams.ServerName);
            parameterValues.Add("ExistingAdminName", objParams.AdminAccountName);
            parameterValues.Add("ExistingAdminPassword", objParams.AdminAccountPwd);
            parameterValues.Add("UniqueID", objParams.uniqueGUID);
            parameterValues.Add("Flag", "");
            parameterValues.Add("InputFileName", objParams.InputFilename);
            parameterValues.Add("CheckExcelORText", objParams.CheckExcelORText);
            parameterValues.Add("GroupName", objParams.GroupName);

            try
            {

                result = ExecuteRunbook(objParams.RunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }




            return result;
        }
        public string StartPrepRunbook(RunbookParams objPrepParams)
        {
            // Details of runbook that we are going to run.
            //Guid runbookId = id;
            //Guid obj = Guid.NewGuid();
            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;

            parameterValues.Add("Log Path", objPrepParams.LogsPath);
            parameterValues.Add("ServerList", objPrepParams.ServerName);
            parameterValues.Add("UniqueID", objPrepParams.uniqueGUID);
            parameterValues.Add("InputFileName", objPrepParams.InputFilename);
            parameterValues.Add("CheckExcelORText", objPrepParams.CheckExcelORText);
            parameterValues.Add("GroupName", objPrepParams.GroupName);

            try
            {
                result = ExecuteRunbook(objPrepParams.RunbookPath, context, parameterValues);
            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }







            return result;
        }
        #endregion

        #region Execute

        public string StartGroupDisplayRunbook(RunbookParams objDisplayParams)
        {
            string result = null;
            Hashtable parameterValues = new Hashtable();
            PatchingUI.OrchestratorService.OrchestratorContext context = null;
            parameterValues.Add("ServerList", objDisplayParams.ServerName);
            parameterValues.Add("Log Path", objDisplayParams.LogsPath);
            parameterValues.Add("UniqueID", objDisplayParams.uniqueGUID);
            parameterValues.Add("InputFileName", objDisplayParams.InputFilename);
            parameterValues.Add("CheckExcelORText", objDisplayParams.CheckExcelORText);
            //parameterValues.Add("GroupName", objDisplayParams.GroupName);
            parameterValues.Add("InputSource", objDisplayParams.InputSource);

            try
            {
                result = ExecuteRunbook(objDisplayParams.RunbookPath, context, parameterValues);
            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }
            return result;
        }

        public string StartMSCSGroupRunbook(RunbookParams objMSCSParams)
        {
            string result = null;
            Hashtable parameterValues = new Hashtable();
            PatchingUI.OrchestratorService.OrchestratorContext context = null;
            parameterValues.Add("Log Path", objMSCSParams.LogsPath);
            parameterValues.Add("ScanORPatch", objMSCSParams.ScanOrPatch);
            parameterValues.Add("SimpleUpdateOption", objMSCSParams.simpleUpdateOption);
            parameterValues.Add("PatchingOption", objMSCSParams.PatchingOption);
            parameterValues.Add("IPAK", objMSCSParams.Ipak);
            parameterValues.Add("UniqueID", objMSCSParams.uniqueGUID);
            parameterValues.Add("InputFileName", objMSCSParams.InputFilename);
            parameterValues.Add("MSNRebootFlag", objMSCSParams.MSNRebootFlag);
            parameterValues.Add("OnlyQFE", objMSCSParams.OnlyQFE);
            parameterValues.Add("ExcludeQFE", objMSCSParams.ExcludeQFE);
            parameterValues.Add("BPUOption", objMSCSParams.BPUOption);
            parameterValues.Add("GroupName", objMSCSParams.GroupName);
            try
            {
                result = ExecuteRunbook(objMSCSParams.RunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }
            return result;
        }


        public string StartHLBGroupRunbook(RunbookParams objParams)
        {

            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;
            parameterValues.Add("ExcludeQFE", objParams.ExcludeQFE);
            parameterValues.Add("CheckExcelOrText", objParams.CheckExcelORText);
            parameterValues.Add("SimpleUpdateOption", objParams.simpleUpdateOption);
            parameterValues.Add("OnlyQFE", objParams.OnlyQFE);
            parameterValues.Add("ScanORPatch", objParams.ScanOrPatch);
            parameterValues.Add("BPUOption", objParams.BPUOption);
            parameterValues.Add("LogPath", objParams.LogsPath);
            parameterValues.Add("IPAK", objParams.Ipak);
            parameterValues.Add("InputFileName", objParams.InputFilename);
            parameterValues.Add("MSNRebootFlag", objParams.MSNRebootFlag);
            parameterValues.Add("UniqueID", objParams.uniqueGUID);
            parameterValues.Add("PatchingOption", objParams.PatchingOption);
            parameterValues.Add("GroupName", objParams.GroupName);

            try
            {

                result = ExecuteRunbook(objParams.RunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }


            return result;
        }



        public string StartExecuteNewRunbook(RunbookParams objParams)
        {



            // Details of runbook that we are going to run.

            //Guid obj = Guid.NewGuid();
            string result = null;

            Hashtable parameterValues = new Hashtable();

            PatchingUI.OrchestratorService.OrchestratorContext context = null;
            parameterValues.Add("Log Path", objParams.LogsPath);
            parameterValues.Add("ScanORPatch", objParams.ScanOrPatch);
            parameterValues.Add("SimpleUpdateOption", objParams.simpleUpdateOption);
            parameterValues.Add("PatchingOption", objParams.PatchingOption);
            parameterValues.Add("IPAK", objParams.Ipak);
            parameterValues.Add("UniqueID", objParams.uniqueGUID);
            parameterValues.Add("InputFileName", objParams.InputFilename);
            parameterValues.Add("MSNRebootFlag", objParams.MSNRebootFlag);
            parameterValues.Add("OnlyQFE", objParams.OnlyQFE);
            parameterValues.Add("ExcludeQFE", objParams.ExcludeQFE);
            parameterValues.Add("BPUOption", objParams.BPUOption);
            parameterValues.Add("InputType", objParams.CheckExcelORText);
            parameterValues.Add("InputSource", objParams.InputSource);
            //parameterValues.Add("ServerList", ServerList);

            try
            {

                result = ExecuteRunbook(objParams.RunbookPath, context, parameterValues);

            }
            catch (DataServiceQueryException ex)
            {
                result = null;
                throw new ApplicationException("Error starting runbook.", ex);
            }


            return result;
        }

        #endregion

    }



    public class RunbookParams
    {
        public string RunbookPath { get; set; }
        public string ServerName { get; set; }
        public string InputFilename { get; set; }
        public string CheckExcelORText { get; set; }
        public string uniqueGUID { get; set; }
        public string LogsPath { get; set; }
        public string Ipak { get; set; }
        public string OnlyQFE { get; set; }
        public string BPUOption { get; set; }
        public string simpleUpdateOption { get; set; }
        public string MSNRebootFlag { get; set; }
        public string ScanOrPatch { get; set; }
        public string ExcludeQFE { get; set; }
        public string PatchingOption { get; set; }
        public string VIP { get; set; }
        public string NodeIP { get; set; }
        public string Port { get; set; }
        public string NodeName { get; set; }
        public string GroupName { get; set; }
        public string AdminAccountName { get; set; }
        public string AdminAccountPwd { get; set; }
        public string InputSource { get; set; }
    }
}
