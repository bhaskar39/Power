using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace PatchingUI
{
    public static class DataAccessLayer
    {


        public static SqlConnection OpenConn()
        {
            string SQLConnString = ConfigurationManager.ConnectionStrings["DB_ConnectionString"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(SQLConnString);
            sqlCon.Open();
            return sqlCon;

        }
        public static SqlConnection OpenClearCacheConn()
        {
            string SQLConnString = string.Empty;
            string strEnv = ConfigurationManager.AppSettings["Env"].ToString();
            if (strEnv.ToLower() == "prod")
                SQLConnString = ConfigurationManager.ConnectionStrings["PROD_DB_ConnectionString"].ConnectionString;
            else if (strEnv.ToLower() == "uat")
                SQLConnString = ConfigurationManager.ConnectionStrings["UAT_DB_ConnectionString"].ConnectionString;
            else
                SQLConnString = ConfigurationManager.ConnectionStrings["DEV_DB_ConnectionString"].ConnectionString;

            SqlConnection sqlCon = new SqlConnection(SQLConnString);
            sqlCon.Open();
            return sqlCon;

        }


        #region Plan

        public static DataTable GetSubBPU(string strBPU)
        {
            try
            {

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("GetSubBPUList", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;

                        sqlcmd.Parameters.AddWithValue("@BPU", strBPU);

                        SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
                        DataTable dtDetails = new DataTable();
                        daDetails.Fill(dtDetails);
                        return dtDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetSubBPU\n\rErrorMessage:\n" + ex.Message);

            }
        }

        public static DataTable GetPlanData(string strFilter)
        {
            try
            {

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("GetPlanData_PatchWindow", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;

                        sqlcmd.Parameters.AddWithValue("@Filter", strFilter);

                        SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
                        DataTable dtDetails = new DataTable();
                        daDetails.Fill(dtDetails);
                        return dtDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetPlanData\n\rErrorMessage:\n" + ex.Message);

            }
        }
        //public static DataTable GetPlanData(PlanParams objParams)
        //{
        //    try
        //    {

        //        using (SqlConnection sqlcon = OpenConn())
        //        {
        //            using (SqlCommand sqlcmd = new SqlCommand("GetPlanData_New", sqlcon))
        //            {
        //                sqlcmd.CommandType = CommandType.StoredProcedure;

        //                sqlcmd.Parameters.AddWithValue("@UniqueID", objParams.UniqueID);
        //                sqlcmd.Parameters.AddWithValue("@OrgName", objParams.OrgName);
        //                sqlcmd.Parameters.AddWithValue("@PatchScenario", objParams.PatchScenario);
        //                sqlcmd.Parameters.AddWithValue("@Mode", objParams.Mode);
        //                sqlcmd.Parameters.AddWithValue("@Type", objParams.Type);
        //                sqlcmd.Parameters.AddWithValue("@PatchWeek", objParams.PatchWeek);
        //                sqlcmd.Parameters.AddWithValue("@PatchMonth", objParams.PatchMonth);
        //                sqlcmd.Parameters.AddWithValue("@SubBPU", objParams.SubBPU);
        //                sqlcmd.Parameters.AddWithValue("@App", objParams.Application);
        //                sqlcmd.Parameters.AddWithValue("@Env", objParams.Env);
        //                SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
        //                DataTable dtDetails = new DataTable();
        //                daDetails.Fill(dtDetails);
        //                return dtDetails;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Helper.WriteError(ex);
        //        throw new Exception("ErrorSource:\n" + ex.Source + "\nGetPlanData\n\rErrorMessage:\n" + ex.Message);

        //    }
        //}

        // public static int UpdateGroupName(string strServerName,string strPatchMonth,string strFlag,string strGoupName)
        public static int UpdateGroupName(DataTable dtDetails, string strGroupName, string strUniqueID)
        {
            int Count = 0;
            try
            {


                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("UpdatePlanGroupName", sqlcon))
                    //using (SqlCommand sqlcmd = new SqlCommand("UpdatePlanGroupName_New", sqlcon))
                    {

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@Details", dtDetails);
                        sqlcmd.Parameters.AddWithValue("@GroupName", strGroupName);
                        sqlcmd.Parameters.AddWithValue("@UniqueID", strUniqueID);
                        //sqlcmd.Parameters.AddWithValue("@IsScheduled", true);
                        //sqlcmd.Parameters.AddWithValue("@StartDate", DateTime.Now);
                        //sqlcmd.Parameters.AddWithValue("@EndDate", da);
                        sqlcmd.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 20);
                        sqlcmd.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;

                        //sqlcmd.ExecuteNonQuery();
                        sqlcmd.ExecuteScalar();
                        Count = (int)sqlcmd.Parameters["@RETURN_VALUE"].Value;

                        return Count;



                        //cmdProc.Parameters.AddWithValue("@Type", "InsertDetails");
                        //cmdProc.Parameters.AddWithValue("@Details", dtDetails);
                        //cmdProc.ExecuteNonQuery();



                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nUpdateGroupName\n\rErrorMessage:\n" + ex.Message);

            }
        }


        public static int UpdateSchedule(DataTable dtScheduleDetails)
        {
            int Count = 0;
            try
            {


                using (SqlConnection sqlcon = OpenConn())
                {

                    using (SqlCommand sqlcmd = new SqlCommand("UpdateSchedule", sqlcon))
                    {

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@Details", dtScheduleDetails);

                        sqlcmd.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 20);
                        sqlcmd.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;

                        //sqlcmd.ExecuteNonQuery();
                        sqlcmd.ExecuteScalar();
                        Count = (int)sqlcmd.Parameters["@RETURN_VALUE"].Value;

                        return Count;

                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nUpdateSchedule\n\rErrorMessage:\n" + ex.Message);

            }
        }

        public static DataSet GetPlanControlsData()
        {
            try
            {

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("GetPlanControlsData", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
                        DataSet dsDetails = new DataSet();
                        daDetails.Fill(dsDetails);
                        return dsDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetPlanControlsData\n\rErrorMessage:\n" + ex.Message);

            }
        }

        public static DataSet GetSubBPUs(string BPU)
        {
            try
            {
                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("GetSubBPUs", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@BPU", BPU);
                        SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
                        DataSet dsDetails = new DataSet();
                        daDetails.Fill(dsDetails);
                        return dsDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetSubBPUs\n\rErrorMessage:\n" + ex.Message);
            }
        }

        public static DataSet GetAllSubBPUs()
        {
            try
            {
                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("GetAllSubBPUs", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
                        DataSet dsDetails = new DataSet();
                        daDetails.Fill(dsDetails);
                        return dsDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetAllSubBPUs\n\rErrorMessage:\n" + ex.Message);
            }
        }

        public static DataTable GetGroupNames(string strStartDate, string strEndDate, string strPatchScenario)
        {
            try
            {

                using (SqlConnection sqlcon = OpenConn())
                {
                    //using (SqlCommand sqlcmd = new SqlCommand("GetGroupNameList", sqlcon))
                    using (SqlCommand sqlcmd = new SqlCommand("GetGroupNameList", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@StartDate", strStartDate);
                        sqlcmd.Parameters.AddWithValue("@EndDate", strEndDate);
                        sqlcmd.Parameters.AddWithValue("@PatchScenario", strPatchScenario);
                        SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
                        DataTable dtDetails = new DataTable();
                        daDetails.Fill(dtDetails);
                        return dtDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetGroupNames\n\rErrorMessage:\n" + ex.Message);

            }
        }


        #endregion
        public static DataTable GetHLBExecutionDetails(string UniqueID)
        {
            try
            {

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("GetHLBExecutionDetails", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
                        DataTable dtDetails = new DataTable();
                        daDetails.Fill(dtDetails);
                        return dtDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetExecutionDetails\n\rErrorMessage:\n" + ex.Message);

            }
        }
        public static int UpdateHLBPausedStatus(string UniqueID, string NodeName)
        {
            try
            {

                int result = 0;
                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("UpdateHLBPausedStatus", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.AddWithValue("@NodeName", NodeName);
                        result = sqlcmd.ExecuteNonQuery();
                        return result;

                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nSetPausedStatus\n\rErrorMessage:\n" + ex.Message);

            }
        }

        public static DataSet GetBackUpNodesList(string UniqueID, string ClusterName)
        {
            try
            {

                using (SqlConnection sqlcon = OpenConn())
                {

                    using (SqlCommand sqlcmd = new SqlCommand("Get_BackUpNodeList_New", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@Uniqueid", UniqueID);
                        //sqlcmd.Parameters.AddWithValue("@ClusterName", ClusterName);
                        sqlcmd.Parameters.AddWithValue("@NodeName", ClusterName);

                        SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
                        DataSet dsDetails = new DataSet();
                        daDetails.Fill(dsDetails);
                        return dsDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetBackUpNodesList\n\rErrorMessage:\n" + ex.Message);

            }
        }

        public static int GetAllCheckedVips(string UniqueID)
        {
            try
            {
                int vipcount = 0;
                using (SqlConnection sqlcon = OpenConn())
                {
                    // using (SqlCommand sqlcmd = new SqlCommand("Get_HLBExecutionSummary", sqlcon))
                    using (SqlCommand sqlcmd = new SqlCommand("sp_GetAllCheckedVips", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
                        DataTable dtDetails = new DataTable();
                        daDetails.Fill(dtDetails);
                        vipcount = dtDetails.Rows.Count;
                        //daDetails.Fill(dtDetails);
                        return vipcount;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetAllCheckedVips\n\rErrorMessage:\n" + ex.Message);

            }
        }

        public static DataTable GetServerCount(string UniqueID)
        {
            try
            {

                using (SqlConnection sqlcon = OpenConn())
                {

                    using (SqlCommand sqlcmd = new SqlCommand("GetServerCount", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);

                        SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
                        DataTable dtDetails = new DataTable();
                        daDetails.Fill(dtDetails);
                        return dtDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetServerCount\n\rErrorMessage:\n" + ex.Message);

            }
        }

        //public static int UpdateHLBCheckValue(string UniqueID, string NodeName, string CheckValue,string IsPaused)
        //{
        //    try
        //    {

        //        int result = 0;
        //        using (SqlConnection sqlcon = OpenConn())
        //        {
        //            using (SqlCommand sqlcmd = new SqlCommand("UpdateHLBCheckValue_New", sqlcon))
        //            {
        //                sqlcmd.CommandType = CommandType.StoredProcedure;
        //                sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
        //                sqlcmd.Parameters.AddWithValue("@NodeName", NodeName);
        //                sqlcmd.Parameters.AddWithValue("@ChkValue", CheckValue);
        //                sqlcmd.Parameters.AddWithValue("@IsPaused", IsPaused);
        //                result = sqlcmd.ExecuteNonQuery();
        //                return result;

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Helper.WriteError(ex);
        //        throw new Exception("ErrorSource:\n" + ex.Source + "\nUpdateHLBCheckValue\n\rErrorMessage:\n" + ex.Message);

        //    }
        //}

        public static int UpdateHLBCheckValue(string UniqueID, string NodeName, string CheckValue, string IsPaused, string IsRunValidation)
        {
            try
            {
                int result = 0;
                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("UpdateHLBCheckValue_New", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.AddWithValue("@NodeName", NodeName);
                        sqlcmd.Parameters.AddWithValue("@ChkValue", CheckValue);
                        sqlcmd.Parameters.AddWithValue("@IsPaused", IsPaused);
                        sqlcmd.Parameters.AddWithValue("@IsRunValadition", IsRunValidation);
                        result = sqlcmd.ExecuteNonQuery();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nUpdateHLBCheckValue\n\rErrorMessage:\n" + ex.Message);
            }
        }

        public static DataTable GetHLBExecutionSummary(string UniqueID, string Environment)
        {
            try
            {

                using (SqlConnection sqlcon = OpenConn())
                {
                    // using (SqlCommand sqlcmd = new SqlCommand("Get_HLBExecutionSummary", sqlcon))
                    using (SqlCommand sqlcmd = new SqlCommand("Get_HLBExecutionSummary_New", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.AddWithValue("@Env", Environment);
                        SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
                        DataTable dtDetails = new DataTable();
                        daDetails.Fill(dtDetails);
                        return dtDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetHLBExecutionSummary\n\rErrorMessage:\n" + ex.Message);

            }
        }


        public static int GetHLBExecutionCompletedCount(string UniqueID)
        {
            try
            {
                int Count = 0;

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("sp_GetCountofHLBCompletedNodes", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);

                        sqlcmd.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 20);
                        sqlcmd.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;

                        //SqlParameter paramReturnValue = new SqlParameter();
                        //paramReturnValue.ParameterName = "@RETURN_VALUE";
                        //paramReturnValue.Direction = ParameterDirection.Output;

                        sqlcmd.ExecuteScalar();
                        Count = (int)sqlcmd.Parameters["@RETURN_VALUE"].Value;
                        // Count=Convert.ToInt32(sqlcmd.ExecuteScalar());

                        return Count;

                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetHLBExecutionCompletedCount\n\rErrorMessage:\n" + ex.Message);

            }
        }
        public static int GetHLBExecutionYetToStartCount(string UniqueID)
        {
            try
            {
                int Count = 0;

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("sp_GetCountofHLBInProgressNodes", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 20);
                        sqlcmd.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;

                        //SqlParameter paramReturnValue = new SqlParameter();
                        //paramReturnValue.ParameterName = "@RETURN_VALUE";
                        //paramReturnValue.Direction = ParameterDirection.Output;

                        sqlcmd.ExecuteScalar();
                        Count = (int)sqlcmd.Parameters["@RETURN_VALUE"].Value;
                        // Count=Convert.ToInt32(sqlcmd.ExecuteScalar());

                        return Count;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetHLBSUExecutionYetToStartCount\n\rErrorMessage:\n" + ex.Message);

            }
        }
        public static int GetHLBExecutionInProgressCount(string UniqueID)
        {
            try
            {
                int Count = 0;

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("sp_GetCountofHLBInProgressNodes", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);

                        sqlcmd.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 20);
                        sqlcmd.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;

                        //SqlParameter paramReturnValue = new SqlParameter();
                        //paramReturnValue.ParameterName = "@RETURN_VALUE";
                        //paramReturnValue.Direction = ParameterDirection.Output;

                        sqlcmd.ExecuteScalar();
                        Count = (int)sqlcmd.Parameters["@RETURN_VALUE"].Value;
                        // Count=Convert.ToInt32(sqlcmd.ExecuteScalar());

                        return Count;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetHLBExecutionInProgressCount\n\rErrorMessage:\n" + ex.Message);

            }
        }
        public static int GetHLBExecutionTotalCount(string UniqueID)
        {
            try
            {
                int Count = 0;

                using (SqlConnection sqlcon = OpenConn())
                {
                    //using (SqlCommand sqlcmd = new SqlCommand("sp_GetHLBCountofTotalServers", sqlcon))
                    using (SqlCommand sqlcmd = new SqlCommand("sp_GetCountofHLBTotalServers", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 20);
                        sqlcmd.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;
                        sqlcmd.ExecuteScalar();
                        Count = (int)sqlcmd.Parameters["@RETURN_VALUE"].Value;
                        return Count;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetHLBSUExecutionTotalCount\n\rErrorMessage:\n" + ex.Message);

            }
        }
        public static int GetHLBExecutionStatus(string UniqueID)
        {
            try
            {
                string status = string.Empty;
                int Count = 0;

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("GetHLBExecutionStatus", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.Add("@Count", SqlDbType.Int);
                        sqlcmd.Parameters["@Count"].Direction = ParameterDirection.Output;

                        sqlcmd.ExecuteNonQuery();
                        //status = (string)sqlcmd.Parameters["@Status"].Value;
                        Count = (int)sqlcmd.Parameters["@Count"].Value;
                        return Count;

                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetHLBExecutionStatus\n\rErrorMessage:\n" + ex.Message);

            }
        }

        public static DataTable GetExecutionDetails(string UniqueID)
        {
            try
            {

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("GetExecutionDetails", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
                        DataTable dtDetails = new DataTable();
                        daDetails.Fill(dtDetails);
                        return dtDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetExecutionDetails\n\rErrorMessage:\n" + ex.Message);

            }
        }

        public static int GetMSCSHLBExecutionStatus(string UniqueID)
        {
            try
            {
                string status = string.Empty;
                int Count = 0;
                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("GetMSCSHLBExecutionStatus", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.Add("@Count", SqlDbType.Int);
                        sqlcmd.Parameters["@Count"].Direction = ParameterDirection.Output;
                        sqlcmd.ExecuteNonQuery();
                        Count = (int)sqlcmd.Parameters["@Count"].Value;
                        return Count;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetExecutionStatus\n\rErrorMessage:\n" + ex.Message);
            }
        }

        public static int GetExecutionStatus(string UniqueID)
        {
            try
            {
                string status = string.Empty;
                int Count = 0;

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("GetExecutionStatus", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.Add("@Status", SqlDbType.VarChar, 20);
                        sqlcmd.Parameters["@Status"].Direction = ParameterDirection.Output;
                        sqlcmd.Parameters.Add("@Count", SqlDbType.Int);
                        sqlcmd.Parameters["@Count"].Direction = ParameterDirection.Output;

                        sqlcmd.ExecuteNonQuery();
                        //status = (string)sqlcmd.Parameters["@Status"].Value;
                        Count = (int)sqlcmd.Parameters["@Count"].Value;
                        return Count;

                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetExecutionStatus\n\rErrorMessage:\n" + ex.Message);

            }
        }
        public static DataTable GetSUExecutionSummary(string UniqueID, string ClusterType)
        {
            try
            {

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("Get_SUExecutionSummary_New", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.AddWithValue("@ClusterType", ClusterType);
                        SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
                        DataTable dtDetails = new DataTable();
                        daDetails.Fill(dtDetails);
                        return dtDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetSUExecutionSummary\n\rErrorMessage:\n" + ex.Message);

            }
        }

        public static DataTable GetSUExecutionSummary(string UniqueID)
        {
            try
            {
                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("Get_ExecutionSummary", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
                        DataTable dtDetails = new DataTable();
                        daDetails.Fill(dtDetails);
                        return dtDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetExecutionSummary\n\rErrorMessage:\n" + ex.Message);
            }
        }

        public static DataTable GetSUExecutionSummary(string UniqueID, string FilterColumn, string FilterValue)
        {
            try
            {
                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("Get_ExecutionSummary1", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.AddWithValue("@FilterColumn", FilterColumn);
                        sqlcmd.Parameters.AddWithValue("@FilterValue", FilterValue);
                        SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
                        DataTable dtDetails = new DataTable();
                        daDetails.Fill(dtDetails);
                        return dtDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetExecutionSummary\n\rErrorMessage:\n" + ex.Message);
            }
        }

        public static DataTable GetSUExecutionOuput(string UniqueID, string NodeName)
        {
            try
            {

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("GetPatchOutput", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.AddWithValue("@ServerName", NodeName);
                        SqlDataAdapter daDetails = new SqlDataAdapter(sqlcmd);
                        DataTable dtDetails = new DataTable();
                        daDetails.Fill(dtDetails);
                        return dtDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetSUExecutionOuput\n\rErrorMessage:\n" + ex.Message);

            }
        }

        public static int GetSUExecutionCompletedCount(string UniqueID)
        {
            try
            {
                int Count = 0;

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("sp_GetCountofCompletedNodes", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);

                        sqlcmd.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 20);
                        sqlcmd.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;

                        //SqlParameter paramReturnValue = new SqlParameter();
                        //paramReturnValue.ParameterName = "@RETURN_VALUE";
                        //paramReturnValue.Direction = ParameterDirection.Output;

                        sqlcmd.ExecuteScalar();
                        Count = (int)sqlcmd.Parameters["@RETURN_VALUE"].Value;
                        // Count=Convert.ToInt32(sqlcmd.ExecuteScalar());

                        return Count;

                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetSUExecutionCompletedCount\n\rErrorMessage:\n" + ex.Message);

            }
        }

        public static int GetSUExecutionCompletedCountForMSCSAndHLB(string UniqueID)
        {
            try
            {
                int Count = 0;

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("sp_GetCountofCompletedNodesForMSCSAndHLB", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 20);
                        sqlcmd.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;
                        sqlcmd.ExecuteScalar();
                        Count = (int)sqlcmd.Parameters["@RETURN_VALUE"].Value;
                        return Count;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetSUExecutionCompletedCount\n\rErrorMessage:\n" + ex.Message);
            }
        }

        public static int GetHLBMSCStandaloneData(string UniqueID)
        {
            try
            {
                int Count = 0;
                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("sp_Get_HLBMSCStandaloneData", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);

                        sqlcmd.Parameters.Add("@Return", SqlDbType.Int, 2);
                        sqlcmd.Parameters["@Return"].Direction = ParameterDirection.ReturnValue;

                        sqlcmd.ExecuteScalar();
                        Count = (int)sqlcmd.Parameters["@Return"].Value;

                        return Count;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetHLBMSCStandaloneData\n\rErrorMessage:\n" + ex.Message);
            }
        }

        public static int GetSUExecutionInProgressCount(string UniqueID)
        {
            try
            {
                int Count = 0;

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("sp_GetCountofInProgressNodes", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);

                        sqlcmd.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 20);
                        sqlcmd.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;

                        //SqlParameter paramReturnValue = new SqlParameter();
                        //paramReturnValue.ParameterName = "@RETURN_VALUE";
                        //paramReturnValue.Direction = ParameterDirection.Output;

                        sqlcmd.ExecuteScalar();
                        Count = (int)sqlcmd.Parameters["@RETURN_VALUE"].Value;
                        // Count=Convert.ToInt32(sqlcmd.ExecuteScalar());

                        return Count;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetSUExecutionInProgressCount\n\rErrorMessage:\n" + ex.Message);

            }
        }

        public static int GetSUExecutionInProgressCountForMSCSAndHLB(string UniqueID)
        {
            try
            {
                int Count = 0;

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("sp_GetCountofInProgressNodesForMSCSAndHLB", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 20);
                        sqlcmd.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;
                        sqlcmd.ExecuteScalar();
                        Count = (int)sqlcmd.Parameters["@RETURN_VALUE"].Value;
                        return Count;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetSUExecutionInProgressCount\n\rErrorMessage:\n" + ex.Message);
            }
        }

        public static int GetSUExecutionYetToStartCount(string UniqueID)
        {
            try
            {
                int Count = 0;

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("sp_GetCountofnodesYettoStart", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 20);
                        sqlcmd.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;

                        //SqlParameter paramReturnValue = new SqlParameter();
                        //paramReturnValue.ParameterName = "@RETURN_VALUE";
                        //paramReturnValue.Direction = ParameterDirection.Output;

                        sqlcmd.ExecuteScalar();
                        Count = (int)sqlcmd.Parameters["@RETURN_VALUE"].Value;
                        // Count=Convert.ToInt32(sqlcmd.ExecuteScalar());

                        return Count;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetSUExecutionYetToStartCount\n\rErrorMessage:\n" + ex.Message);

            }
        }

        public static int GetSUExecutionTotalCount(string UniqueID)
        {
            try
            {
                int Count = 0;

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("sp_GetCountofTotalServers", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 20);
                        sqlcmd.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;

                        //SqlParameter paramReturnValue = new SqlParameter();
                        //paramReturnValue.ParameterName = "@RETURN_VALUE";
                        //paramReturnValue.Direction = ParameterDirection.Output;

                        sqlcmd.ExecuteScalar();
                        Count = (int)sqlcmd.Parameters["@RETURN_VALUE"].Value;
                        // Count=Convert.ToInt32(sqlcmd.ExecuteScalar());

                        return Count;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetSUExecutionTotalCount\n\rErrorMessage:\n" + ex.Message);

            }
        }

        public static int GetExecutionTotalCountForMSCSAndHLB(string UniqueID)
        {
            try
            {
                int Count = 0;

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("sp_GetCountofTotalServersForMSCSAndHLB", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 20);
                        sqlcmd.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;
                        sqlcmd.ExecuteScalar();
                        Count = (int)sqlcmd.Parameters["@RETURN_VALUE"].Value;
                        return Count;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetSUExecutionTotalCount\n\rErrorMessage:\n" + ex.Message);
            }
        }

        public static int GetGivenTotalServerCount(string UniqueID)
        {
            try
            {
                int Count = 0;

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("Get_TotalGivenServersCount", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 20);
                        sqlcmd.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;
                        sqlcmd.ExecuteScalar();
                        Count = (int)sqlcmd.Parameters["@RETURN_VALUE"].Value;
                        return Count;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetSUExecutionTotalCount\n\rErrorMessage:\n" + ex.Message);
            }
        }

        public static string GetPatchScenario(string UniqueID, string ServerName)
        {
            try
            {
                string PatchScenario = string.Empty;

                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("GetPatchScenario", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.AddWithValue("@ServerName", ServerName);
                        sqlcmd.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 10);
                        sqlcmd.Parameters["@RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;
                        sqlcmd.ExecuteScalar();
                        PatchScenario = sqlcmd.Parameters["@RETURN_VALUE"].Value.ToString();
                        return PatchScenario;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetPatchScenario\n\rErrorMessage:\n" + ex.Message);
            }
        }

        public static int UpdateNodeInfo(string UniqueID, string NodeName, string PauseFalg, string Pausevalue, string FailbackFlag, string ForceStandloneFlag, string BackUpNode, string RunValidationFlag)
        {
            try
            {
                int result = 0;
                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("UpdateNodeInfo_New", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.AddWithValue("@NodeName", NodeName);
                        sqlcmd.Parameters.AddWithValue("@PauseNodeBeforePatching", PauseFalg);
                        sqlcmd.Parameters.AddWithValue("@PauseNodeDuringExecution", Pausevalue);
                        sqlcmd.Parameters.AddWithValue("@FailbackToOriginalState", FailbackFlag);
                        sqlcmd.Parameters.AddWithValue("@ForceStanaloneFlag", ForceStandloneFlag);
                        sqlcmd.Parameters.AddWithValue("@BackUpNode", BackUpNode);
                        sqlcmd.Parameters.AddWithValue("@RunValidationFlag", RunValidationFlag);

                        result = sqlcmd.ExecuteNonQuery();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nUpdateNodeInfo\n\rErrorMessage:\n" + ex.Message);
            }
        }

        public static int UpdatePausedStatus(string UniqueID, string NodeName)
        {
            try
            {

                int result = 0;
                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("SetPausedStatus", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.AddWithValue("@NodeName", NodeName);
                        result = sqlcmd.ExecuteNonQuery();
                        return result;

                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nSetPausedStatus\n\rErrorMessage:\n" + ex.Message);

            }
        }

        public static int UpdateRunValidationFlag(string UniqueID, string NodeName, string RunvalidationFlag, string PatchingScenario)
        {
            try
            {

                int result = 0;
                using (SqlConnection sqlcon = OpenConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("usp_UpdateRunValidationFlag", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                        sqlcmd.Parameters.AddWithValue("@NodeName", NodeName);
                        sqlcmd.Parameters.AddWithValue("@PatchingScenario", PatchingScenario);
                        sqlcmd.Parameters.AddWithValue("@RunValidationFlag", RunvalidationFlag);
                        result = sqlcmd.ExecuteNonQuery();
                        return result;

                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nUpdateRunValidationFlag\n\rErrorMessage:\n" + ex.Message);

            }
        }
        public static string GetRunbookStatus(string strUniqueID, string Patchingoption, string type)
        {
            SqlConnection conn = null;
            string strconnectionString = string.Empty;
            SqlCommand command = null;
            string strRunbookStatus = string.Empty;
            try
            {


                strconnectionString = ConfigurationManager.ConnectionStrings["DB_ConnectionString"].ConnectionString;

                conn = new SqlConnection(strconnectionString);
                conn.Open();
                command = new SqlCommand("usp_GetRunbookStatusNew", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UniqueID", strUniqueID);
                command.Parameters.AddWithValue("@PatchingOption", Patchingoption);
                command.Parameters.Add("@Status", SqlDbType.VarChar, 20);
                command.Parameters["@Status"].Direction = ParameterDirection.Output;
                command.Parameters.Add("@type", type);

                command.ExecuteNonQuery();

                strRunbookStatus = (string)command.Parameters["@Status"].Value;
                conn.Close();

            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
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


        public static void GetClearCacheStatus()
        {
            try
            {
                string status = string.Empty;

                using (SqlConnection sqlcon = OpenClearCacheConn())
                {
                    using (SqlCommand sqlcmd = new SqlCommand("GetClearCacheStatus", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;


                        sqlcmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetClearCacheStatus\n\rErrorMessage:\n" + ex.Message);

            }
        }



        public static string GetExtensionlist(List<string> NodeList)
        {
            try
            {
                DataTable tblNodes = new DataTable("FilterNodes");
                tblNodes.Columns.Add("ServerName", typeof(string));
                NodeList.ForEach(x => tblNodes.Rows.Add(x));
                try
                {
                    using (SqlConnection sqlcon1 = OpenConn())
                    {
                        using (SqlCommand sqlcmd1 = new SqlCommand("USP_GetExtensionServerList_RealTime", sqlcon1))
                        {
                            sqlcmd1.CommandTimeout = 1;
                            sqlcmd1.CommandType = CommandType.StoredProcedure;
                            sqlcmd1.Parameters.AddWithValue("@NodeList", tblNodes);
                            //   SqlDataReader reader = new SqlDataReader();

                            SqlDataReader reader = sqlcmd1.ExecuteReader();
                            List<string> Serverlist = new List<string>();

                            while (reader.Read())
                            {
                                Serverlist.Add((string)reader["servername"]);
                            }

                            string serverlist = string.Join(",", Serverlist);
                            sqlcon1.Close();
                            return serverlist;

                        }

                    }

                }

                catch (Exception ex)
                {
                    Helper.WriteError(ex);
                    using (SqlConnection sqlcon = OpenConn())
                    {
                        using (SqlCommand sqlcmd = new SqlCommand("USP_GetExtensionServerList", sqlcon))
                        {

                            sqlcmd.CommandType = CommandType.StoredProcedure;
                            sqlcmd.Parameters.AddWithValue("@NodeList", tblNodes);
                            //   SqlDataReader reader = new SqlDataReader();
                            SqlDataReader reader = sqlcmd.ExecuteReader();
                            List<string> Serverlist = new List<string>();

                            while (reader.Read())
                            {
                                Serverlist.Add((string)reader["servername"]);
                            }

                            string serverlist = string.Join(",", Serverlist);
                            sqlcon.Close();
                            return serverlist;

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Helper.WriteError(ex);
                throw new Exception("ErrorSource:\n" + ex.Source + "\nGetExclusionlist\n\rErrorMessage:\n" + ex.Message);

            }

        }

    }
    public class PlanParams
    {
        public string UniqueID { get; set; }
        public string OrgName { get; set; }
        public string PatchScenario { get; set; }
        public string Mode { get; set; }
        public string Type { get; set; }
        public string PatchWeek { get; set; }
        public string PatchMonth { get; set; }
        public string SubBPU { get; set; }
        public string Application { get; set; }
        public string Env { get; set; }
    }
}