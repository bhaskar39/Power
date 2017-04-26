<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="PatchingUI.Report.Reports" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Power Patch</title>
    <link href="../Styles/datepicker.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <style type="text/css">
        .modal
        {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 0.8;
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }
        
        .loading
        {
            font-family: Arial;
            font-size: 10pt; /* border: 5px solid #67CFF5;*/
            border: 2px solid #1E90FF;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="30000">
    </asp:ScriptManager>
    <div class="page">
        <div class="header">
            <div class="title">
                <h1>
                    Power Patch
                </h1>
            </div>
            <div class="loginDisplay">
                <asp:Label ID="CurrentUserFullName" runat="server" Text="" CssClass="loginDisplay" />
            </div>
        </div>
        <div class="main">
            <div id="tabs_content_container">
                <div id="content_5" class="tab_content" runat="server">
                    <table>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-top: 20px;
                                padding-left: 80px; width: 50px;" align="right">
                                StartDate :<span id="startdate" style="color: red;">*</span>
                            </td>
                            <td style="padding-top: 20px; width: 160px;">
                                <asp:TextBox ID="txtReportsStartDate" runat="server" Style="margin-left: 5px"></asp:TextBox>
                                <img id="imgReportsStartDate" alt="StartDate" class="selectR1" src="../Images/Calendar.gif" />
                                <label id="lblReportsStartDate" style="font-family: Verdana; font-size: 12px; color: red">
                                </label>
                            </td>
                            <td style="width: 220px; margin-left: 10px; padding-top: 20px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-left: 80px;"
                                align="right">
                                EndDate : <span id="enddate" style="color: red;">*</span>
                            </td>
                            <td style="width: 150px;">
                                <asp:TextBox ID="txtReportsEndDate" runat="server" Style="margin-left: 5px"></asp:TextBox>
                                <img id="imgReportsEndDate" alt="EndDate" class="selectR2" src="../Images/Calendar.gif" />
                                <label id="lblReportsEndDate" style="font-family: Verdana; font-size: 12px; color: red">
                                </label>
                            </td>
                            <td style="width: 220px; margin-left: 10px; padding-top: 20px;" align="left">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-top: 10px; padding-left: 90px;" align="center">
                                <asp:Button ID="btnPlanSummary" Style="width: 120px;" TabIndex="10" Text="Plan Summary"
                                    runat="server" OnClick="btnPlanSummaryReports_Click" Font-Underline="False" CssClass="button" />
                                <asp:Button ID="btnExecuteReports" Style="width: 135px;" TabIndex="10" Text="Execution Summary"
                                    runat="server" OnClick="btnExecuteReports_Click" Font-Underline="False" CssClass="button" />
                                <asp:Button ID="btnOverallSummary" Style="width: 140px;" TabIndex="10" Text="E2E Plan Summary"
                                    runat="server" OnClick="btnOverallReports_Click" Font-Underline="False" CssClass="button" />
                            </td>
                            <td style="width: 220px; margin-left: 10px; padding-top: 20px;" align="left">
                                <label id="Label7" style="font-family: Verdana; font-size: 12px; color: red">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-top: 30px; padding-left: 390px;">
                                <div class="loading">
                                    Loading. Please wait.<br />
                                    <br />
                                    <img src="../Images/loading.gif" alt="loading" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <h3>
                                    <asp:Label ID="Label4" runat="server"></asp:Label></h3>
                                <div align="left" style="margin-top: 25px; margin-left: 50px; width: auto;">
                                    <rsweb:ReportViewer ID="rvMainReports" runat="server" Visible="true" Width="97%"
                                        Height="800px">
                                        <ServerReport Timeout="600000000" />
                                    </rsweb:ReportViewer>
                                    <%--  <rsweb:ReportViewer ID="rvDeployment" runat="server" Visible="false" Width="97%">
                                        <ServerReport Timeout="600000000" />
                                    </rsweb:ReportViewer>--%>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    <script type="text/javascript">


        function tabSwitch(new_tab, new_content) {
            //alert(new_tab);alert(new_content);
            document.getElementById('content_1').style.display = 'none';
            document.getElementById('content_2').style.display = 'none';
            document.getElementById('content_3').style.display = 'none';
            document.getElementById('content_4').style.display = 'none';
            document.getElementById('content_5').style.display = 'none';
            document.getElementById(new_content).style.display = 'block';
            document.getElementById('tab_1').className = '';
            document.getElementById('tab_2').className = '';
            document.getElementById('tab_3').className = '';
            document.getElementById('tab_4').className = '';
            document.getElementById('tab_5').className = '';
            document.getElementById(new_tab).className = 'selected';
        }

    </script>
    </form>
    <br />
    <div id="footer" style="margin-bottom: 0.1in; margin-top: -0.35in; text-align: center">
        <asp:Label ID="VersionNumber" runat="server" Text="56.0.0.0" ForeColor="#333333"></asp:Label>
    </div>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <script type="text/javascript">
       

        function ShowProgress() {
            setTimeout(function () {
                var loading = $(".loading");
                loading.show();

            }, 200);

        }
    </script>
    <script src="../Scripts/datepicker.js" type="text/javascript"></script>
    <!---Reports Tab-->
    <script type="text/javascript">


        $("#btnPlanSummary").click(function () {


            var flag = 0;
            lblReportsStartDate.innerHTML = "";
            lblReportsEndDate.innerHTML = "";
            if ($('#txtReportsStartDate').val() == '') {

                lblReportsStartDate.innerHTML = 'Please Enter StartDate';
                flag = 1;

            }
            if ($('#txtReportsEndDate').val() == '') {

                lblReportsEndDate.innerHTML = 'Please Enter EndDate';
                flag = 1;

            }
            if (flag == 1) {
                return false;
            }
            else {
                ShowProgress();
                return true;

            }
        });

        $("#btnExecuteReports").click(function () {

            var flag = 0;
            lblReportsStartDate.innerHTML = "";
            lblReportsEndDate.innerHTML = "";
            if ($('#txtReportsStartDate').val() == '') {

                lblReportsStartDate.innerHTML = 'Please Enter StartDate';
                flag = 1;

            }
            if ($('#txtReportsEndDate').val() == '') {

                lblReportsEndDate.innerHTML = 'Please Enter EndDate';
                flag = 1;

            }
            if (flag == 1) {
                return false;
            }
            else {
                ShowProgress();
                return true;

            }
        });

        $("#btnOverallSummary").click(function () {

            //            var flag = 0;
            //            lblServers.innerHTML = "";
            //            lblPatching.innerHTML = "";
            //            if ($('#txtReportsStartDate').val() == '') {

            //                lblReportsStartDate.innerHTML = 'Please Enter StartDate';
            //                flag = 1;

            //            }
            //            if ($('#txtReportsEndDate').val() == '') {

            //                lblReportsEndDate.innerHTML = 'Please Enter EndDate';
            //                flag = 1;

            //            }
            //            if (flag == 1) {
            //                return false;
            //            }
            //            else {
            ShowProgress();
            return true;

            // }
        });
       
       

    </script>
</body>
</html>
