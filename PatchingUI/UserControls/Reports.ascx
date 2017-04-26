<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Reports.ascx.cs" Inherits="PatchingUI.UserControls.Reports" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

    <div>
                    <table border="0">
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-top: 20px;
                                padding-left: 80px;" align="right">
                                StartDate :<span id="startdate" style="color: red;">*</span>
                            </td>
                            <td style="padding-top: 20px;">
                                <asp:TextBox ID="txtReportsStartDate" runat="server" Width="120px" Style="margin-left: 5px"></asp:TextBox>
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
                            <td>
                                <asp:TextBox ID="txtReportsEndDate" runat="server" Width="120px" Style="margin-left: 5px"></asp:TextBox>
                                <img id="imgReportsEndDate" alt="EndDate" class="selectR2" src="../Images/Calendar.gif" />
                                <label id="lblReportsEndDate" style="font-family: Verdana; font-size: 12px; color: red">
                                </label>
                            </td>
                            <td style="width: 220px; margin-left: 10px; padding-top: 20px;" align="left">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-top: 10px; padding-left: 290px;">
                                <asp:Button ID="btnPlanSummary" Style="width: 120px;" TabIndex="10" Text="Plan Summary"
                                    runat="server" OnClick="btnPlanSummaryReports_Click" Font-Underline="False" CssClass="button" />
                                <asp:Button ID="btnExecuteReports" Style="width: 135px;" TabIndex="10" Text="Execution Summary"
                                    runat="server" OnClick="btnExecuteReports_Click" Font-Underline="False" CssClass="button" />
                                <asp:Button ID="btnOverallSummary" Style="width: 120px;" TabIndex="10" Text="Overall Summary"
                                    runat="server" OnClick="btnOverallReports_Click" Font-Underline="False" CssClass="button" />
                                <asp:Button ID="btnValidationReports" Style="width: 120px;" TabIndex="10" Text="Validation Report"
                                    runat="server" OnClick="btnValidationReports_Click" Font-Underline="False" CssClass="button" />
                            </td>
                            <td style="width: 220px; margin-left: 10px; padding-top: 20px;" align="left">
                                <label id="Label7" style="font-family: Verdana; font-size: 12px; color: red">
                                </label>
                            </td>
                        </tr>
                        <tr>
                        <td colspan="3" style="padding-top: 30px; padding-left: 390px;">
                        
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
                                    <rsweb:ReportViewer ID="rvMainReports" runat="server" Visible="true" Width="97%" Height="800px">
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

    <script type="text/javascript">

                    function ShowProgress() {

                        setTimeout(function () {
                            var loading = $(".loading");
                            loading.show();
                        }, 200);

                    }
                    
                    function ShowExecuteProgress() {
                        setTimeout(function () {
                            var loading = $(".Executeloading");

                            loading.show();
                        }, 200);

                    }
    </script>

    
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

    <!---Reports Tab-->
    <script type="text/javascript">

        $(".selectR1").click(function () {
            //alert('startDate');
            var txtReportsStartDate = document.getElementById("txtReportsStartDate");
            $("#<%=txtReportsStartDate.ClientID%>").datepicker({ dateFormat: 'mm-dd-yy' });
            txtReportsStartDate.focus();


        });

        $(".selectR2").click(function () {
            // alert('enddate');


            var txtReportsEndDate = document.getElementById("txtReportsEndDate");
            $("#<%=txtReportsEndDate.ClientID%>").datepicker({ dateFormat: 'mm-dd-yy' });
            txtReportsEndDate.focus();
        });

        $("#btnValidationReports").click(function () {
            var flag = 0;
            lblServers.innerHTML = "";
            lblPatching.innerHTML = "";
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

        $("#btnPlanSummary").click(function () {

            var flag = 0;
            lblServers.innerHTML = "";
            lblPatching.innerHTML = "";
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
            lblServers.innerHTML = "";
            lblPatching.innerHTML = "";
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
