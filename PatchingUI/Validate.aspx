<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="Validate.aspx.cs" Inherits="PatchingUI.Validate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--<%@ OutputCache CacheProfile="AppCache1" VaryByParam="*" %>--%>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent"> 
 <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <%--<style type="text/css">
.Executeloading
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

                    <table width="100%">
                       
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-top: 20px;
                                margin-right: 10px; width: 340px;" align="right" class="Label">
                                Path :
                            </td>
                            <td style="color: Red; padding-top: 20px;">
                                *
                            </td>
                            <td style="padding-top: 20px; width: 400px">
                                <asp:FileUpload ID="fupValidateExcel" ClientIDMode="Static" runat="server" Width="250px" onchange="EnableValidate()" />
                                <label id="lblValidateExcel" style="font-family: Verdana; font-size: 12px; color: red">
                                </label>
                            </td>
                            <td style="padding-top: 20px; padding-left: 20px" align="right">
                                <asp:HyperLink ID="hlValidate" ClientIDMode="Static" runat="server" Target="_blank" NavigateUrl="http://sharepoint/sites/ST/Shared%20Documents/Forms/AllItems.aspx?RootFolder=/sites/ST/Shared%20Documents/Standard%20Practices/Automation%20Folder/Automation%20OnBoarding/PowerPatch%204.0/Documents/Input%20Excel%20Templates">Input Template </asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-left: 60px; width: 340px;" align="right" class="Label">
                                Select GroupName :
                            </td>
                            <td style="color: Red;">&nbsp;</td>
                            <td>
                                <asp:DropDownList ID="ddlValidateNames" ClientIDMode="Static" runat="server" Width="247px" Height="20px" TabIndex="6" Font-Bold="False" Font-Size="12px">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="padding-top: 20px; width: 150px">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; margin-right: 10px;
                                width: 340px;" align="right" class="Label">
                                Server Names :
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td style="">
                                <asp:TextBox ID="txtValidateServerNames" ClientIDMode="Static" runat="server" Width="244px"></asp:TextBox>
                            </td>
                            <td style="width: 300px">
                                <label id="lblValidateServerNames" style="font-family: Verdana; font-size: 12px;
                                    color: red">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; margin-right: 10px;
                                width: 340px;" align="right" class="Label">
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td style="">
                                <label id="Label8" style="font-family: Verdana; font-weight: bold; font-size: 10px;">
                                    Enter Servernames with , separated
                                </label>
                            </td>
                            <td style="width: 150px">
                            </td>
                        </tr>
                       
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 340px;"
                                align="right">
                                <asp:CheckBox ID="cbMSNPatch" runat="server" Text="" ClientIDMode="Static" />
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td style="margin: 5px; font-size: 12px; font-weight: bold; font-family: Verdana;"
                                align="left">
                                <span>MSN Patch Preview</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; margin-right: 10px;
                                width: 340px;" align="right">
                                <asp:CheckBox ID="cbSimpleUpdate" ClientIDMode="Static" runat="server" Text="" />
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td style="margin: 5px; font-size: 12px; font-weight: bold; font-family: Verdana;"
                                align="left">
                                <span>Simple Update Preview</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; margin-right: 10px;
                                width: 340px;" align="right">
                                <asp:CheckBox ID="cbODP" ClientIDMode="Static" runat="server" Text="" />
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td style="margin: 5px; font-size: 12px; font-weight: bold; font-family: Verdana;"
                                align="left">
                                <span>ODP Preview</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; margin-right: 10px;
                                width: 340px;" align="right">
                                <asp:CheckBox ID="chkISERSCAN" ClientIDMode="Static" runat="server" Text="" Enabled="false" />
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td style="margin: 5px; font-size: 12px; font-weight: bold; font-family: Verdana;"
                                align="left">
                                <span>ISER SCAN </span>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; margin-right: 10px;
                                width: 340px;" align="right">
                                <asp:CheckBox ID="cbNumbers" ClientIDMode="Static" runat="server" Text="" />
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td style="margin: 5px; font-size: 12px; font-family: Verdana;" align="left">
                                <span style="font-weight: bold">KB Numbers
                                    <asp:TextBox ID="txtKbValue" ClientIDMode="Static" runat="server" Width="400px" Enabled="false"></asp:TextBox>
                                </span>
                                <label id="lblTextValidate" style="font-family: Verdana; font-size: 12px; color: red">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; margin-right: 10px;
                                width: 340px;" align="right">
                                <asp:CheckBox ID="cbInstalledUpdates" ClientIDMode="Static" runat="server" Text="" />
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td style="margin: 9px; font-size: 12px; font-weight: bold; font-family: Verdana;"
                                align="left">
                                <span>Find Installed Updates &nbsp;&nbsp;From
                                    <asp:TextBox ID="txtStartDate"  ClientIDMode="Static" runat="server" Width="70px"></asp:TextBox>
                                    <img id="imgCal1" alt="StartDate"  class="select1" src="Images/Calendar.gif" />&nbsp;To
                                    <asp:TextBox ID="txtEndDate" ClientIDMode="Static" runat="server" Width="50px"></asp:TextBox>
                                    <img id="img2" alt="EndDate" class="select2" src="Images/Calendar.gif" />
                                </span>
                            </td>
                            <td style="margin-left: 10px" align="left">
                                <label id="lblInstalledUpdates" style="font-family: Verdana; font-size: 12px; color: red">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center" style="padding-right: 95px;">
                                <label id="lblCheckPatch" style="font-family: Verdana; font-size: 12px; color: red">
                                </label>
                            </td>
                        </tr>
                       
                        
                        
                        <tr>
                            <td colspan="3" align="center" style="height: 40px; margin-left: 20px">
                                <asp:Button ID="btnValidatePatch"  ClientIDMode="Static" Style="width: 80px; margin-left: 10px" TabIndex="10"
                                    Text="Validate" runat="server" OnClick="btnValidate_Click" Font-Underline="False"
                                    CssClass="button" />
                                <asp:Button ID="btnPostSmokeTest" ClientIDMode="Static" Text="SmokeTest" Width="100px" Height="25px" runat="server"
                                    OnClick="btnPostSmokeTest_Click" />
                                <asp:Button ID="btnCompareTest" ClientIDMode="Static" Text="CompareSmokeTestLog" OnClick="btnCompareTest_Click"
                                    Width="155px" Height="25px" runat="server" Visible="false" />
                                <asp:GridView ID="GridView1" ClientIDMode="Static" runat="server" AutoGenerateColumns="true" CellPadding="4"
                                    Style="margin-left: 80px; height: auto;" ForeColor="#333333" GridLines="None">
                                    <AlternatingRowStyle BackColor="White" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                </asp:GridView>
                            </td>
                        </tr>
                         <tr><td colspan="3">
                        
                          <div class="loading" style="margin-left:430px;margin-top:20px;">
                        Loading. Please wait.<br />
                        <br />
                        <img src="Images/loading.gif" alt="loading" />
                    </div>
                        
                        </td></tr>
                        <tr>
                            <td colspan="3">
                                <asp:TextBox ID="txtResult" ClientIDMode="Static" runat="server" Visible="false" TextMode="MultiLine" Width="900px"
                                    Height="100px" Style="margin-right: 10px;"></asp:TextBox>
                                <asp:Panel ID="pnlValidateResult" ClientIDMode="Static" runat="server" align="center" Style="margin-top: 20px;
                                    margin-left: 20px;" Height="400px" ScrollBars="Vertical" Visible="false">
                                    <asp:GridView ID="gvValidateResult" ClientIDMode="Static" runat="server" AutoGenerateColumns="true" CellPadding="4"
                                        Style="margin-left: 80px; height: auto;" ForeColor="#333333" GridLines="None">
                                        <AlternatingRowStyle BackColor="White" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#EFF3FB" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <h3>
                                    <asp:Label ID="lblRvValidate" runat="server"></asp:Label></h3>
                                <div align="center" style="margin-top: 25px; width: auto;">
                                    <asp:Timer ID="TimerValidate" ClientIDMode="Static" OnTick="TimerValidate_Tick" runat="server" 
                                        Enabled="False">
                                    </asp:Timer>
                                    <asp:UpdatePanel ID="upValidate" ClientIDMode="Static" UpdateMode="Conditional" runat="server">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="TimerValidate" EventName="Tick" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <rsweb:ReportViewer ID="rvValidate" ClientIDMode="Static" runat="server" Visible="false" Width="950px">
                                                <ServerReport Timeout="600000000" />
                                            </rsweb:ReportViewer>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                
                    </table>
                   
             
      <asp:HiddenField ID="hdnExcelPath" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCbValidate" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCbMSNPatch" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCbSimpleUpdate" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCbODP" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnFileName" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnKBUniqueValidate" runat="server" Value="" ClientIDMode="Static" />

    
   <%-- <script type="text/javascript" src="Scripts/jquery.datetimepicker.js"></script>--%>
    <script src="Scripts/datepicker.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>

    <!-- Validate Tab-->
  <script type="text/javascript">

      function ShowProgress() {

          setTimeout(function () {
              var loading = $(".loading");

              loading.show();

          }, 200);

      }

      $(".select1").click(function () {
          // alert('startDate');
          var txtStartDate = document.getElementById("txtStartDate");
          $("#<%=txtStartDate.ClientID%>").datepicker({ dateFormat: 'mm-dd-yy' });
          txtStartDate.focus();
      });

      $(".select2").click(function () {
          var txtEndDate = document.getElementById("txtEndDate");
          $("#<%=txtEndDate.ClientID%>").datepicker({ dateFormat: 'mm-dd-yy' });
          txtEndDate.focus();
      });

      $('#txtValidateServerNames').change(function () {
          $('#btnValidatePatch').removeAttr("disabled");
      });
      $('#txtValidateServerNames').keypress(function (event) {
          var Upload_file = document.getElementById('<%= fupValidateExcel.ClientID %>');
          var myfile = Upload_file.value;
          if (myfile != '') {
              $('#txtValidateServerNames').attr("disabled", "disabled");
              lblValidateServerNames.innerHTML = "Only One Option Avialble";
          }
          else {
              $('#txtValidateServerNames').removeAttr("disabled");
              $('#btnValidatePatch').removeAttr("disabled");

              lblValidateServerNames.innerHTML = "";
          }
      });

      $("#btnPostSmokeTest").click(function () {

          var flag1 = 0;
          lblServers.innerHTML = "";
          var Upload_file = document.getElementById('<%= fupValidateExcel.ClientID %>');

          document.getElementById('<%= hdnFileName.ClientID %>').value = myfile;
          var myfile = Upload_file.value;

          var path = myfile;
          var pos = path.lastIndexOf(path.charAt(path.indexOf(":") + 1));
          var filename = path.substring(pos + 1);
          var RegEx = /^([a-zA-Z0-9_\.])+$/;


          if (myfile.indexOf("xlsx") > 0) {

              if (!(filename.match(RegEx))) {
                  flag1 = 1;
                  lblServernames.innerHTML = 'Filename should not contain spaces and special characters ';

              }

          }
          else if (myfile.indexOf("xls") > 0) {
              if (!(filename.match(RegEx))) {
                  flag1 = 1;
                  lblServernames.innerHTML = 'Filename should not contain spaces and special characters ';

              }
          }

          else {

              //flag1 = 1;
              // lblValidateExcel.innerHTML = 'Please Select Excel File ';
              if ($('#txtValidateServerNames').val() == '') {
                  flag1 = 1;
                  lblValidateServerNames.innerHTML = 'Please Select Excel File or Enter ServerNames ';
              }

          }
          if (flag1 == 1) {
              return false;
          }
          else {
              ShowProgress();
              return true;

          }

      });





      $("#cbSimpleUpdate").click(function () {
          if ($(this).is(':checked')) {
              $("#cbNumbers").attr("checked", false);
              $("#cbMSNPatch").attr("checked", false);
              $("#cbODP").attr("checked", false);


              $('#gvValidateResult').hide();
              // $('#rvValidate').hide();
              $('#txtKbValue').attr("disabled", "disabled");
              $("#cbInstalledUpdates").attr("checked", false);
              //                $('#txtStartDate').attr("disabled", "disabled");
              //                $('#txtEndDate').attr("disabled", "disabled");


          }
      });


      $('#cbMSNPatch').click(function () {

          if ($(this).is(':checked')) {
              $("#cbNumbers").attr("checked", false);
              $("#cbSimpleUpdate").attr("checked", false);
              $("#cbODP").attr("checked", false);
              $('#gvValidateResult').hide();
              //$('#rvValidate').hide();
              $('#txtKbValue').attr("disabled", "disabled");
              $("#cbInstalledUpdates").attr("checked", false);
              //                $('#txtStartDate').attr("disabled", "disabled");
              //                $('#txtEndDate').attr("disabled", "disabled");

          }
      });

      $('#cbODP').click(function () {

          if ($(this).is(':checked')) {
              $("#cbNumbers").attr("checked", false);
              $("#cbSimpleUpdate").attr("checked", false);
              $("#cbMSNPatch").attr("checked", false);
              $('#gvValidateResult').hide();
              //$('#rvValidate').hide();
              $('#txtKbValue').attr("disabled", "disabled");
              $("#cbInstalledUpdates").attr("checked", false);
              //                $('#txtStartDate').attr("disabled", "disabled");
              //                $('#txtEndDate').attr("disabled", "disabled");


          }
      });
      $('#cbNumbers').click(function () {

          if ($(this).is(':checked')) {
              $("#ValidatePopup").addClass("ValidatePopup");
              $("#cbSimpleUpdate").attr("checked", false);
              $("#cbODP").attr("checked", false);
              $('#rvValidate').hide();
              $('#txtKbValue').removeAttr("disabled");
              $("#cbInstalledUpdates").attr("checked", false);
              //                $('#txtStartDate').attr("disabled", "disabled");
              //                $('#txtEndDate').attr("disabled", "disabled");
          }
          else {
              $('#txtKbValue').attr("disabled", "disabled");
              $("#ValidatePopup").addClass("rvValidatePopup");

          }
      });

      $('#cbInstalledUpdates').click(function () {

          if ($(this).is(':checked')) {
              $("#ValidatePopup").addClass("ValidatePopup");
              $("#cbMSNPatch").attr("checked", false);
              $("#cbSimpleUpdate").attr("checked", false);
              $("#cbODP").attr("checked", false);
              $("#cbNumbers").attr("checked", false);
              $('#txtKbValue').attr("disabled", "disabled");
              $('#rvValidate').hide();


          }
          else {

          }
      });


      $("#btnValidatePatch").click(function () {

          // alert('s');
          //$('#btnExecute').removeAttr("disabled");
          var flag = 0;
          lblTextValidate.innerHTML = "";
          lblCheckPatch.innerHTML = "";
          lblValidateServerNames.innerHTML = "";

          var Upload_file = document.getElementById('<%= fupValidateExcel.ClientID %>');
          var myfile = Upload_file.value;

          var path = myfile;
          var pos = path.lastIndexOf(path.charAt(path.indexOf(":") + 1));
          var filename = path.substring(pos + 1);
          var RegEx = /^([a-zA-Z0-9_\.])+$/;


          if (myfile.indexOf("xlsx") > 0) {

              if (!(filename.match(RegEx))) {
                  flag = 1;
                  lblValidateExcel.innerHTML = 'Filename should not contain spaces and special characters ';

              }

          }
          else if (myfile.indexOf("xls") > 0) {
              if (!(filename.match(RegEx))) {
                  flag = 1;
                  lblValidateExcel.innerHTML = 'Filename should not contain spaces and special characters ';

              }
          }

          else {
              if ($('#txtValidateServerNames').val() == '' && $('#ddlValidateNames').val() == '0') {
                  flag = 1;
                  lblValidateServerNames.innerHTML = 'Please Select Excel File or GroupName or Enter ServerNames ';
              }
              // flag = 1;
              // lblValidateExcel.innerHTML = 'Please Select Excel File ';

          }
          var cbMSNPatch = document.getElementById("cbMSNPatch");
          var cbODP = document.getElementById("cbODP");
          var cbFindUpdates = document.getElementById("cbInstalledUpdates");
          if (cbMSNPatch.checked) {
              //return confirm("execute");
          }
          var cbSimpleUpdate = document.getElementById("cbSimpleUpdate");
          if (cbSimpleUpdate.checked) {
              //return confirm("execute");
          }
          var cbNumbers = document.getElementById("cbNumbers");
          if (cbNumbers.checked) {

              if ($('#txtKbValue').val() == '') {

                  lblTextValidate.innerHTML = 'Please Enter Kb Values for Validate';
                  flag = 1;

              }

          }
          if (cbFindUpdates.checked) {
              var txtStartDate = document.getElementById("txtStartDate");
              var txtEndDate = document.getElementById("txtEndDate");
              if ($('#txtStartDate').val() == '' || $('#txtEndDate').val() == '') {
                  lblInstalledUpdates.innerHTML = "Please select Dates";
                  flag = 1;
              }
              if (flag == 1) {
                  return false;
              }
              else {

                  return true;
              }

          }
          if (cbMSNPatch.checked == false && cbNumbers.checked == false && cbSimpleUpdate.checked == false && cbODP.checked == false && cbFindUpdates.checked == false) {
              lblCheckPatch.innerHTML = 'Please select atleast one checkbox';
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
      function EnableValidate() {


          $('#txtValidateServerNames').removeAttr("disabled");
          $('#txtValidateServerNames').val('');

      }

      //End of Validate Tab
    </script>
</asp:Content>

