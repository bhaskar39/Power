<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="Reports.aspx.cs" Inherits="PatchingUI.Reports" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--<%@ OutputCache CacheProfile="AppCache1" VaryByParam="*" %>--%>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <%--<style type="text/css">
       
        .loading
        {
            font-family: Arial;
            font-size: 10pt;          
            border: 2px solid #1E90FF;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }  
         
</style>--%>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
                    <table border="0" width="100%">
                       
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-top: 20px;
                                padding-left: 80px;" align="right">
                                StartDate :<span id="startdate" style="color: red;">*</span>
                            </td>
                            <td style="padding-top: 20px;">
                                <asp:TextBox ID="txtReportsStartDate" runat="server" Width="120px" ClientIDMode="Static" Style="margin-left: 5px" ></asp:TextBox>
                                <img id="imgReportsStartDate" alt="StartDate" class="selectR1" src="Images/Calendar.gif" />
                                <label id="lblReportsStartDate"  style="font-family: Verdana; font-size: 12px; color: red">
                                </label>
                            </td>                            
                        </tr>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-left: 80px;"
                                align="right">
                                EndDate : <span id="enddate" style="color: red;">*</span>
                            </td>
                            <td  style="padding-top: 20px;">
                                <asp:TextBox ID="txtReportsEndDate" runat="server" ClientIDMode="Static" Width="120px" Style="margin-left: 5px"></asp:TextBox>
                                <img id="imgReportsEndDate" alt="EndDate" class="selectR2" src="Images/Calendar.gif" />
                                <label id="lblReportsEndDate" style="font-family: Verdana; font-size: 12px; color: red">
                                </label>
                            </td>                            
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-top: 10px; padding-left: 290px;">
                                <asp:Button ID="btnPlanSummary" ClientIDMode="Static" Style="width: 120px;" TabIndex="10" Text="Plan Summary"
                                    runat="server" OnClick="btnPlanSummaryReports_Click" Font-Underline="False" CssClass="button" />
                                <asp:Button ID="btnExecuteReports" ClientIDMode="Static" Style="width: 135px;" TabIndex="10" Text="Execution Summary"
                                    runat="server" OnClick="btnExecuteReports_Click" Font-Underline="False" CssClass="button" />
                                <asp:Button ID="btnOverallSummary" ClientIDMode="Static" Style="width: 120px;" TabIndex="10" Text="Overall Summary"
                                    runat="server" OnClick="btnOverallReports_Click" Font-Underline="False" CssClass="button" />
                                <asp:Button ID="btnValidationReports" ClientIDMode="Static" Style="width: 120px;" TabIndex="10" Text="Validation Report"
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
                        <img src="Images/loading.gif" alt="loading" />
                    </div>
                        </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <h3>
                                    <asp:Label ID="Label4" runat="server" ClientIDMode="Static"></asp:Label></h3>
                                <div align="left" style="margin-top: 25px; margin-left: 50px; width: auto;">
                                    <rsweb:ReportViewer ID="rvMainReports" ClientIDMode="Static" runat="server" Visible="false" Width="97%" Height="800px">
                                        <ServerReport Timeout="600000000" />
                                    </rsweb:ReportViewer>                                 
                                </div>
                            </td>
                        </tr>
               
                    </table>            

      <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>      
    <script type="text/javascript" src="Scripts/jquery.datetimepicker.js"></script>
    <script src="Scripts/datepicker.js" type="text/javascript"></script> 
          
  <script type="text/javascript">

      function ShowProgress() {

          setTimeout(function () {
              var loading = $(".loading");

              loading.show();

          }, 200);

      }

      $(".selectR1").click(function () {
          var txtReportsStartDate = document.getElementById("txtReportsStartDate");
          $("#<%=txtReportsStartDate.ClientID%>").datepicker({ dateFormat: 'mm-dd-yy' });
          txtReportsStartDate.focus();
      });

      $(".selectR2").click(function () {
          var txtReportsEndDate = document.getElementById("txtReportsEndDate");
          $("#<%=txtReportsEndDate.ClientID%>").datepicker({ dateFormat: 'mm-dd-yy' });
          txtReportsEndDate.focus();
      });

      $("#btnValidationReports").click(function () {
          var flag = 0;
          lblReportsStartDate.innerHTML = "";
          lblReportsEndDate.innerHTML = "";
          var reportsStartDate = $('#txtReportsStartDate').val();
          var reportsEndDate = $('#txtReportsEndDate').val();

          if ($('#txtReportsStartDate').val() == '') {
              lblReportsStartDate.innerHTML = 'Please Enter StartDate';
              flag = 1;
          }
          if ($('#txtReportsEndDate').val() == '') {

              lblReportsEndDate.innerHTML = 'Please Enter EndDate';
              flag = 1;
          }
          if (reportsStartDate > reportsEndDate) {
              lblReportsStartDate.innerHTML = 'End Date Must Be Greater than Start Date';
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
          lblReportsStartDate.innerHTML = "";
          lblReportsEndDate.innerHTML = "";       
          var reportsStartDate = $('#txtReportsStartDate').val();
          var reportsEndDate = $('#txtReportsEndDate').val();

          if ($('#txtReportsStartDate').val() == '') {

              lblReportsStartDate.innerHTML = 'Please Enter StartDate';
              flag = 1;

          }
          if ($('#txtReportsEndDate').val() == '') {

              lblReportsEndDate.innerHTML = 'Please Enter EndDate';
              flag = 1;

          }

          if (reportsStartDate > reportsEndDate) {
              lblReportsStartDate.innerHTML = 'End Date Must Be Greater than Start Date';
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
          var reportsStartDate = $('#txtReportsStartDate').val();
          var reportsEndDate = $('#txtReportsEndDate').val();

          if ($('#txtReportsStartDate').val() == '') {

              lblReportsStartDate.innerHTML = 'Please Enter StartDate';
              flag = 1;

          }
          if ($('#txtReportsEndDate').val() == '') {

              lblReportsEndDate.innerHTML = 'Please Enter EndDate';
              flag = 1;

          }

          if (reportsStartDate > reportsEndDate) {
              lblReportsStartDate.innerHTML = 'End Date Must Be Greater than Start Date';
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
          ShowProgress();
          return true;
      });

  
  </script>
</asp:Content>
