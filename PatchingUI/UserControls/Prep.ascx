<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Prep.ascx.cs" Inherits="PatchingUI.UserControls.Prep" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
  <div>
  <table>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-left: 60px;
                                padding-top: 20px;" align="right" class="Label">
                                Patch Scenario :
                            </td>
                            <td style="color: Red; padding-top: 20px;">
                            </td>
                            <td style="margin: 5px; padding-top: 20px;" align="left">
                                <asp:DropDownList ID="ddlPrepScenario" Width="225px" Height="20px" runat="server"
                                    TabIndex="6" Font-Bold="False" Font-Size="12px" AutoPostBack="true" OnSelectedIndexChanged="ddlPrepScenario_SelectedIndexChanged">
                                    <asp:ListItem Value="1">Standalone</asp:ListItem>
                                     <asp:ListItem Value="3">MSCS Servers</asp:ListItem>
                                    <asp:ListItem Value="2">HLB Cluster</asp:ListItem>
                                    <asp:ListItem Enabled="false">Scenario3</asp:ListItem>
                                    <asp:ListItem Enabled="false">Scenario4</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px; margin-left: 10px" align="left">
                            </td>
                            <td style="padding-top: 20px; padding-left: 20px; width: 200px;" align="right">
                                <asp:HyperLink ID="hlPrep" runat="server" Target="_blank" NavigateUrl="http://sharepoint/sites/ST/Shared%20Documents/Forms/AllItems.aspx?RootFolder=/sites/ST/Shared%20Documents/Standard%20Practices/Automation%20Folder/Automation%20OnBoarding/PowerPatch%204.0/Documents/Input%20Excel%20Templates">Input Template </asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-left: 60px"
                                align="right" class="Label">
                                ServersList Path :
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td>
                                <asp:FileUpload ID="fupPreExceute" runat="server" Width="280px" onchange="EnablePreCheck()" />
                            </td>
                            <td style="padding-top: 20px; width: 150px">
                                <label id="lblPreExecute" style="font-family: Verdana; font-size: 12px; color: red">
                                </label>
                            </td>
                            </tr>
                            <tr >

                             <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-left: 60px"
                                align="right" class="Label">
                                Select GroupName :
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td>
                               <asp:DropDownList ID="ddlPrepNames" runat="server" Width="225px" Height="20px" 
                                    TabIndex="6" Font-Bold="False" Font-Size="12px">
                               <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                               </asp:DropDownList>
                            </td>
                            <td style="padding-top: 20px; width: 150px">
                                <%--<label id="lblddl" style="font-family: Verdana; font-size: 12px; color: red">
                                </label>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-left: 60px"
                                align="right" class="Label">
                                Server Names :
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td style="">
                                <asp:TextBox ID="txtServerNames" runat="server" Width="274px"></asp:TextBox>
                            </td>
                            <td style="width: 150px">
                                <label id="lblServernames" style="font-family: Verdana; font-size: 12px; color: red">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-left: 60px"
                                align="right" class="Label">
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td style="">
                                <label id="Label9" style="font-family: Verdana; font-weight: bold; font-size: 10px;">
                                    Enter Servernames with , separated
                                </label>
                            </td>
                            <td style="width: 150px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center" style="height: 40px; padding-left: 180px;">
                                <asp:Button ID="btnCheck" Style="width: 100px; margin-left: 10px" TabIndex="10" Text="Pre-Check"
                                    CssClass="button" runat="server" OnClick="btnCheck_Click" ToolTip="CheckDiskSpace,AdminAccess" />
                                <asp:Button ID="btnPreSmokeTest" Text="SmokeTest" Width="100px" Height="25px" Style="margin-left: 10px"
                                    runat="server" OnClick="btnPreSmokeTest_Click" />
                                <asp:Button ID="btnUnistall" Style="width: 100px; margin-left: 10px" TabIndex="10"
                                    Text="FlashKiller" CssClass="button" runat="server" OnClick="btnUninstall_Click"
                                    ToolTip="UnInstall Flash" />
                            </td>
                        </tr>

                        <tr><td>
                        
                          <div class="loading" style="margin-left:430px;margin-top:20px;">
                        Loading. Please wait.<br />
                        <br />
                        <img src="Images/loading.gif" alt="loading" />
                    </div>
                        </td></tr>
                    </table>
                    
                    <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnAddAdminScript">
                        <div id="divAddAdmin" visible="false" runat="server">
                            <h3>
                                <label id="Label2" style="font-family: Verdana; font-weight: bold; font-size: 10px;">
                                    Enter Existing Admin Details For adding Redmond\Stpatcha Account
                                </label>
                            </h3>
                            <table>
                                <tr>
                                    <td style="font-size: 12px; font-weight: bold; font-family: Verdana; margin-right: 10px;
                                        width: 340px;" align="right">
                                        Admin Account Name :
                                    </td>
                                    <td style="color: Red;">
                                        *
                                    </td>
                                    <td style="margin: 5px;" align="left">
                                        <asp:TextBox ID="txtAdmin" runat="server" Style="width: 190px;" ClientIDMode="Static"
                                            Height="20px"></asp:TextBox>
                                    </td>
                                    <td style="margin-left: 10px" align="left">
                                        <label id="lblAdminName" style="font-family: Verdana; font-size: 12px; color: red">
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-size: 12px; font-weight: bold; font-family: Verdana; margin-right: 10px"
                                        align="right">
                                        Admin Account Password :
                                    </td>
                                    <td style="color: Red;">
                                        *
                                    </td>
                                    <td style="margin: 5px;" align="left">
                                        <asp:TextBox ID="txtPassword" runat="server" Style="width: 190px;" TextMode="Password"
                                            Height="20px"></asp:TextBox>
                                    </td>
                                    <td style="margin-left: 10px" align="left">
                                        <label id="lblPassword" style="font-family: Verdana; font-size: 12px; color: red">
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center" style="height: 40px; padding-left: 180px">
                                        <asp:Button ID="btnAddAdminScript" Style="width: 100px; margin-left: 10px" TabIndex="10"
                                            Text="Add Admin" runat="server" OnClick="btnAddAdminScript_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlAddAdmin" runat="server" align="center" Style="margin-top: 20px;
                        margin-left: 20px;" Height="400px" ScrollBars="Vertical" Visible="false">
                        <asp:GridView ID="gvAddAdmin" runat="server" CellPadding="4" ForeColor="#333333"
                            Height="400px" GridLines="None">
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
                    <asp:Timer ID="timerPrep" OnTick="timerPrep_Tick" runat="server" Interval="10000"
                        Enabled="False">
                    </asp:Timer>
                    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="timerPrep" EventName="Tick" />
                        </Triggers>
                        <ContentTemplate>
                            <div align="center" style="margin-top: 20px; margin-left: 0px;">
                                <rsweb:ReportViewer ID="rvPrep" runat="server" Height="300px" Width="1000px" Visible="true">
                                    <ServerReport Timeout="600000000" />
                                </rsweb:ReportViewer>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Button ID="btnMitigation" Style="width: 100px; margin-left: 450px; margin-top: 20px;"
                        TabIndex="10" Visible="false" Text="Mitigate" CssClass="button" runat="server"
                        OnClick="btnMitigation_Click" ToolTip="Admin Access,Clear C DriveSpace" />
                           
                            <%-- <asp:HiddenField ID="hdnUniqueFindUpdates" runat="server" Value="" />--%>
                                <asp:HiddenField ID="hdnUniqueAccess" runat="server" Value="" />
                                <asp:HiddenField ID="hdnCheck" runat="server" Value="" />
                                <asp:HiddenField ID="hdnPrepFileName" runat="server" Value="" />
                                <asp:HiddenField ID="hdnPrepInputFilename" runat="server" Value="" />
                                 <asp:HiddenField ID="hdnExecute" runat="server" Value="" />
                                 <asp:HiddenField ID="hdnServerNames" runat="server" Value="" />
                                 <asp:HiddenField ID="hdnFileName" runat="server" Value="" />
 </div>
          
           <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
  <!--PrepTab-->
    <script type="text/javascript">

        function ShowProgress() {

            setTimeout(function () {
               var loading = $(".loading");

                loading.show();                   

            }, 200);

        }


        $('#ddlPrepNames').change(function () {
            if ($('#ddlPrepNames').find('option:selected').text() == 'Select') {
                $('#txtServerNames').removeAttr("disabled");
                $('#btnCheck').removeAttr("disabled");
            }
            else {
                $('#txtServerNames').attr("disabled", "disabled");
            }
        });

        $('#txtServerNames').change(function () {
            $('#btnCheck').removeAttr("disabled");
        });
        $('#txtServerNames').keypress(function (event) {

            if ($('#ddlPrepNames').find('option:selected').text() != 'Select') {

                $('#txtServerNames').attr("disabled", "disabled");
                lblServernames.innerHTML = "Only One Option Avialble";
            }
            else {
                $('#txtServerNames').removeAttr("disabled");
                $('#btnCheck').removeAttr("disabled");

                lblServernames.innerHTML = "";
            }
        });

        $("#btnCheck").click(function () {


            var flag1 = 0;
            lblServers.innerHTML = "";


            //            var myfile = Upload_file.value;

            //            var path = myfile;
            //            var pos = path.lastIndexOf(path.charAt(path.indexOf(":") + 1));
            //            var filename = path.substring(pos + 1);
            //            var RegEx = /^([a-zA-Z0-9_\.])+$/;


            //            if (myfile.indexOf("xlsx") > 0) {

            //                if (!(filename.match(RegEx))) {
            //                    flag1 = 1;
            //                    lblServernames.innerHTML = 'Filename should not contain spaces and special characters ';

            //                }

            //            }
            //            else if (myfile.indexOf("xls") > 0) {
            //                if (!(filename.match(RegEx))) {
            //                    flag1 = 1;
            //                    lblServernames.innerHTML = 'Filename should not contain spaces and special characters ';

            //                }
            //            }

            if ($('#ddlPrepNames').find('option:selected').text() == 'Select') {


                if ($('#txtServerNames').val() == '') {
                    flag1 = 1;
                    lblServernames.innerHTML = 'Please Select Group Name or Enter ServerNames ';
                }
                //lblPreExecute.innerHTML = 'Please Select Excel File ';

            }
            if (flag1 == 1) {
                return false;
            }
            else {
                ShowProgress();
                return true;

            }
        });

        $("#btnUnistall").click(function () {


            var flag1 = 0;
            lblServers.innerHTML = "";



            // document.getElementById('<%= hdnFileName.ClientID %>').value = myfile;
            //            var myfile = Upload_file.value;

            //            var path = myfile;
            //            var pos = path.lastIndexOf(path.charAt(path.indexOf(":") + 1));
            //            var filename = path.substring(pos + 1);
            //            var RegEx = /^([a-zA-Z0-9_\.])+$/;


            //            if (myfile.indexOf("xlsx") > 0) {

            //                if (!(filename.match(RegEx))) {
            //                    flag1 = 1;
            //                    lblServernames.innerHTML = 'Filename should not contain spaces and special characters ';

            //                }

            //            }
            //            else if (myfile.indexOf("xls") > 0) {
            //                if (!(filename.match(RegEx))) {
            //                    flag1 = 1;
            //                    lblServernames.innerHTML = 'Filename should not contain spaces and special characters ';

            //                }
            //            }
            if ($('#ddlPrepNames').find('option:selected').text() == 'Select') {




                //  flag1 = 1;

                if ($('#txtServerNames').val() == '') {
                    flag1 = 1;
                    lblServernames.innerHTML = 'Please Select Excel File or Enter ServerNames ';
                }
                //lblPreExecute.innerHTML = 'Please Select Excel File ';

            }
            if (flag1 == 1) {
                return false;
            }
            else {
                ShowProgress();
                return true;

            }
        });

        $("#btnPreSmokeTest").click(function () {

            var flag1 = 0;
            lblServers.innerHTML = "";
            if ($('#ddlPrepNames').find('option:selected').text() == 'Select') {

                // flag1 = 1;
                //lblPreExecute.innerHTML = 'Please Select Excel File ';
                if ($('#txtServerNames').val() == '') {
                    flag1 = 1;
                    lblServernames.innerHTML = 'Please Select Excel File or Enter ServerNames ';
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

        $("#btnAddAdminScript").click(function () {

            $('#btnExecute').removeAttr("disabled");
            var flag = 0;
            lblAdminName.innerHTML = "";
            lblPassword.innerHTML = "";

            if ($('#txtAdmin').val() == '') {

                lblAdminName.innerHTML = 'Please Enter Admin Account Name';
                flag = 1;

            }
            if ($('#txtPassword').val() == '') {

                lblPassword.innerHTML = 'Please Enter Admin Account Password';
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
        function EnablePreCheck() {
            $('#btnCheck').removeAttr("disabled");
            $('#txtServerNames').removeAttr("disabled");
            $('#txtServerNames').val('');
        }
        function Enable() {
                    $('#btnAddAdmin').removeAttr("disabled");
        }
        function Disable() {
                     $('#btnAddAdmin').attr("disabled", "disabled");
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