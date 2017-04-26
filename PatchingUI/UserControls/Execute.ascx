<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Execute.ascx.cs" Inherits="PatchingUI.UserControls.Execute" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<div>
    <div>
                   <table align="center">
                            <tr style="display: none">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 340px;
                                    margin-right: 10px; padding-top: 20px;" align="right" class="Label">
                                    Domain Account Name :
                                </td>
                                <td style="color: Red; padding-top: 20px;">
                                    *
                                </td>
                                <td style="margin: 5px; padding-top: 20px;" align="left">
                                    <asp:TextBox ID="txtDomainAccName" Width="295px" Style="margin-left: 5px" Height="20px" TextMode="Password"
                                        Text="redmond\stpatcha" runat="server" TabIndex="3"></asp:TextBox>
                                </td>
                                <td style="width: 220px; margin-left: 10px; padding-top: 20px;" align="left">
                                    <label id="lblAdminAccountName" style="font-family: Verdana; font-size: 12px; color: red">
                                    </label>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 340px;
                                    margin-right: 10px;" align="right" class="Label">
                                    Domain Account password :
                                </td>
                                <td style="color: Red;">
                                    *
                                </td>
                                <td style="margin: 5px;" align="left">
                                    <asp:TextBox ID="txtDomainAcctPwd" TextMode="Password" Width="295px" Style="margin-left: 5px"
                                        Text="#EDCcde3" Height="20px" runat="server" TabIndex="4"></asp:TextBox>
                                </td>
                                <td style="width: 180px; margin-left: 10px" align="left">
                                    <label id="lblAdminAcctPwd" style="font-family: Verdana; font-size: 12px; color: red">
                                    </label>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 340px;
                                    margin-right: 10px;" align="right" class="Label">
                                    LogPath :
                                </td>
                                <td style="color: Red;">
                                    *
                                </td>
                                <td style="margin: 5px;" align="left">
                                    <asp:TextBox ID="txtLogPath" Width="295px" Style="margin-left: 5px" Height="20px"
                                        Text="C:\Temp" ReadOnly="true" runat="server" TabIndex="5"></asp:TextBox>
                                </td>
                                <td style="width: 180px; margin-left: 10px" align="left">
                                    <label id="lblLogPath" style="font-family: Verdana; font-size: 12px; color: red">
                                    </label>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 340px;
                                    margin-right: 10px; padding-top: 20px;" align="right" class="Label">
                                    Patch Scenario :
                                </td>
                                <td style="color: Red; padding-top: 20px;">
                                </td>
                                <td style="margin: 5px; padding-top: 20px;" align="left">
                                    <asp:DropDownList ID="ddlScenario" Width="225px" Height="20px" runat="server" TabIndex="6" 
                                        Font-Bold="False" Font-Size="12px" AutoPostBack="true" OnSelectedIndexChanged="ddlScenario_SelectedIndexChanged">
                                        <asp:ListItem Value="1">Standalone</asp:ListItem>
                                          <asp:ListItem Value="3">MSCS Servers</asp:ListItem>
                                        <asp:ListItem Value="2">HLB Cluster</asp:ListItem>
                                        <asp:ListItem Enabled="false">Scenario3</asp:ListItem>
                                        <asp:ListItem Enabled="false">Scenario4</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 180px; margin-left: 10px" align="left">
                                    <label id="lblScenario" style="font-family: Verdana; font-size: 12px; color: red">
                                    </label>
                                </td>
                                <td style="padding-top: 20px; padding-left: 20px">
                                    <asp:HyperLink ID="hlExecution" runat="server" Target="_blank" NavigateUrl="http://sharepoint/sites/ST/Shared%20Documents/Forms/AllItems.aspx?RootFolder=/sites/ST/Shared%20Documents/Standard%20Practices/Automation%20Folder/Automation%20OnBoarding/PowerPatch%204.0/Documents/Input%20Excel%20Templates">Input Template </asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 340px;
                                    margin-right: 10px;" align="right" class="Label">
                                    BPU :
                                </td>
                                <td style="color: Red;">
                                </td>
                                <td style="margin: 5px;" align="left">
                                   <%-- <asp:DropDownList ID="ddlBPU" Width="225px" Height="20px" runat="server" TabIndex="6"
                                        Font-Bold="False" Font-Size="12px">
                                        <asp:ListItem Value="1">Non-ECIT Servers</asp:ListItem>
                                        <asp:ListItem Value="2">ECIT Servers</asp:ListItem>
                                    </asp:DropDownList>--%>

                                    <asp:RadioButtonList ID="rblBPU" Width="225px" Height="20px" runat="server" TabIndex="6"  RepeatDirection="Horizontal" 
                                            RepeatLayout="Flow"  TextAlign="Right"
                                        Font-Bold="false" Font-Names="Verdana" Font-Size="12px">                                        
                                        <asp:ListItem Value="1" Selected="True">Non-ECIT Servers</asp:ListItem>
                                        <asp:ListItem Value="2">ECIT Servers</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td style="width: 180px; margin-left: 10px" align="left">
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 340px;
                                    margin-right: 10px;" align="right" class="Label">
                                    Patch Tool Option :
                                </td>
                                <td style="color: Red;">
                                    *
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPatchingOption" Width="225px" Height="20px" TabIndex="7"
                                        runat="server" AutoPostBack="false" Font-Bold="False" Font-Size="12px">
                                        <asp:ListItem>Select</asp:ListItem>
                                        <asp:ListItem>MSNPatch</asp:ListItem>
                                        <asp:ListItem>OnDemandPatch</asp:ListItem>
                                        <asp:ListItem>SimpleUpdate</asp:ListItem>
                                        <asp:ListItem>Chaining (ODP-SU)</asp:ListItem>
                                        <asp:ListItem Value="5">Chaining(ODP-SU-MSNPATCH)</asp:ListItem>
                                        <asp:ListItem>SU-2x</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 250px; margin-left: 10px" align="left">
                                    <label id="lblPatching" style="font-family: Verdana; font-size: 12px; color: red">
                                    </label>
                                </td>
                            </tr>
                            <tr id="trReboot" runat="server" style="display: none" clientidmode="Static">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 340px;
                                    margin-right: 10px;" align="right" class="Label">
                                    Reboot Option :
                                </td>
                                <td style="color: Red;">
                                    *
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlReboot" Width="225px" Height="20px" TabIndex="7" runat="server"
                                        AutoPostBack="false" Font-Bold="False" Font-Size="12px">
                                        <asp:ListItem Value="0">Force Reboot</asp:ListItem>
                                        <asp:ListItem Value="1">NoForce Reboot</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 250px; margin-left: 10px" align="left">
                                    <label id="Label5" style="font-family: Verdana; font-size: 12px; color: red">
                                    </label>
                                </td>
                            </tr>
                            <tr runat="server" style="display: none" clientidmode="Static" id="trOnlyQFE">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 340px;
                                    margin-right: 10px;" align="right" class="Label">
                                    OnlyQFE :
                                </td>
                                <td style="color: Red;">
                                </td>
                                <td style="">
                                    <asp:TextBox ID="txtOnlyQFE" Width="274px" Style="" Height="20px" Text="" runat="server"></asp:TextBox>
                                </td>
                                <td style="width: 180px; margin-left: 10px" align="left">
                                </td>
                            </tr>
                            <tr runat="server" style="display: none" clientidmode="Static" id="trExcludeQFE">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 340px;
                                    margin-right: 10px;" align="right" class="Label">
                                    ExcludeQFE :
                                </td>
                                <td style="color: Red;">
                                </td>
                                <td style="">
                                    <asp:TextBox ID="txtExcludeQFE" Width="274px" Style="" Height="20px" Text="" runat="server"></asp:TextBox>
                                </td>
                                <td style="width: 180px; margin-left: 10px" align="left">
                                </td>
                            </tr>
                            <tr id="trODPOption" runat="server" style="display: none" clientidmode="Static">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 340px;
                                    margin-right: 10px;" align="right" class="Label">
                                    OnDemandPatch Option :
                                </td>
                                <td style="color: Red;">
                                    *
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlODPOption" Width="225px" Height="20px" runat="server" TabIndex="8"
                                        Font-Bold="False" Font-Size="12px">
                                        <asp:ListItem>Patch</asp:ListItem>
                                        <%--  <asp:ListItem>Scan</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                               <%-- <td style="width: 250px; margin-left: 10px" align="left">
                                    <label id="lblScanorPatch" style="font-family: Verdana; font-size: 12px; color: red">
                                    </label>
                                </td>--%>
                                  <td style="width: 180px; margin-left: 10px" align="left">
                                </td>
                            </tr>
                            <tr id="trSimpleUpdateOption" runat="server" style="display: none" clientidmode="Static">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 340px;
                                    margin-right: 10px;" align="right" class="Label">
                                    SimpleUpdate Option :
                                </td>
                                <td style="color: Red;">
                                    *
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSimpleUpdateOption" Width="225px" Height="20px" runat="server"
                                        TabIndex="9" Font-Bold="False" Font-Size="12px" AutoPostBack="false">
                                        <asp:ListItem Text="Reboot" Value="reboot"></asp:ListItem>
                                        <asp:ListItem Text="Install" Value="install"></asp:ListItem>
                                        <%-- <asp:ListItem>Preview</asp:ListItem>--%>
                                    </asp:DropDownList>

                                   
                                </td>
                              <%--  <td style="width: 250px; margin-left: 10px" align="left">
                                    <label id="lblSimpleUpdate" style="font-family: Verdana; font-size: 12px; color: red">
                                    </label>
                                </td>--%>
                                  <td style="width: 180px; margin-left: 10px" align="left">
                                </td>
                            </tr>
                          
                            <tr>
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 340px;
                                    margin-right: 10px;" align="right" class="Label">
                                      <label id="lblServerlistPath" style="font-family: Verdana; font-size: 12px;">
                                       ServersList Path :
                                    </label>
                                   
                                </td>
                                <td style="color: Red;">
                                    *
                                </td>
                                <td>
                                    <asp:FileUpload ID="fupExcel" runat="server" onchange="CheckEnable()" Width="280px" />
                                </td>
                                <td>
                                    <label id="lblServers" style="font-family: Verdana; font-size: 12px; color: red">
                                    </label>
                                </td>
                            </tr>

                                <tr >

                             <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-left: 60pxwidth: 340px;"
                                align="right" class="Label">
                                Select GroupName :
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td>
                               <asp:DropDownList ID="ddlExecuteNames" runat="server" Width="225px" Height="20px" 
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
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 340px;
                                    margin-right: 10px;" align="right" class="Label">
                                    <label id="lblServerName" style="font-family: Verdana; font-weight: bold;">
                                        Server Names :
                                    </label>
                                 
                                  <%--  Server Names :--%>
                                </td>
                                <td style="color: Red;">
                                </td>
                                <td style="">
                                    <asp:TextBox ID="txtExecuteServerNames" runat="server" Width="274px"></asp:TextBox>
                                </td>
                                <td style="width: 300px">
                                    <label id="lblExecuteServerNames" style="font-family: Verdana; font-size: 12px; color: red">
                                    </label>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 340px;
                                    margin-right: 10px;" align="right" class="Label">
                                </td>
                                <td style="color: Red;">
                                </td>
                                <td style="">
                                    <label id="lblServersText" style="font-family: Verdana; font-weight: bold; font-size: 10px;">
                                        Enter Servernames with , separated
                                    </label>
                                </td>
                                <td style="width: 150px">
                                </td>
                            </tr>
                            <tr><td>
                          <div class="loading" style="margin-left:430px;margin-top:20px;">
                        Loading. Please wait.<br />
                        <br />
                        <img src="Images/loading.gif" alt="loading" />
                             </div>
                        </td></tr>
                            <tr>
                                <td colspan="3" align="center" style="height: 40px; padding-left: 190px;">
                                
                 
                                    <asp:Button ID="btnExecute" Width="80px" Height="25px" runat="server" TabIndex="11"
                                        CssClass="button" Text="Display" OnClick="btnExecute_Click" />
                                   <%-- <asp:Button ID="btnReboot" Width="80px" Height="25px" runat="server" TabIndex="11"
                                        CssClass="button" Text="Reboot" OnClick="btnReboot_Click" />--%>
                                       <%--  <asp:Button ID="btnHLBExecute" Width="100px" Height="25px" runat="server" Enabled="false"
                                        CssClass="button" Text="Execute" OnClick="btnHLBExecute_Click" />
                             --%>
                             <asp:Button ID="btnExportToExcel" Width="150px" Height="25px" runat="server"  Enabled="false" TabIndex="12" CssClass="button" Text="Export to Excel" OnClick="btnExportToExcel_Click" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:HyperLink ID="hlLogs1" runat="server" ForeColor="Blue" Target="_blank" Visible="false"
                                        Enabled="false">   Execution Summary Report </asp:HyperLink>
                                   
                                </td>
                            </tr>


                             
                            
                        </table>                           
                         
                         <asp:HiddenField ID="PageID" runat="server" />
                         <asp:HiddenField ID="hdnClusterExecute" runat="server" Value="" />
                         <asp:HiddenField ID="hdnExecute" runat="server" Value="" />
                        <asp:HiddenField ID="hdnBackUpNode" runat="server" Value="0" />
                          <asp:HiddenField ID="hdnFileName" runat="server" Value="" />
                           <asp:HiddenField ID="hdnExecuteFileName" runat="server" Value="" />
                    </div>
    <div style="margin-top: 25px; margin-right: 0px; padding-right:30px; height: auto;" runat="server" id="divClusterExecute" visible="false">               


                        <asp:Timer ID="ExecuteTimer" OnTick="ExecuteTimer_Tick" runat="server" Interval="60000"
                            Enabled="False">
                        </asp:Timer>

                         <asp:Timer ID="ExecuteCompletedTimer" OnTick="ExecuteCompletedTimer_Tick" runat="server" Interval="60000"
                            Enabled="False">
                        </asp:Timer>
                          
                     <%--   <asp:UpdateProgress ID="uprogressExecute" runat="server" AssociatedUpdatePanelID="upExecute">
                                 <ProgressTemplate>
                                <div class="overlay" />
                                    <div class="overlayContent">
                                        <h2>Loading...</h2>
                                        <img src="Images/ajax-loader.gif" alt="Loading" border="1" />
                                     </div>
                                     </div>

              
                             
                                     </ProgressTemplate>
                        </asp:UpdateProgress>--%>
                        <asp:UpdatePanel ID="upExecute" UpdateMode="Conditional" runat="server">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ExecuteTimer" EventName="Tick" />
                                 <asp:AsyncPostBackTrigger ControlID="ExecuteCompletedTimer" EventName="Tick" />
                               <%--    <asp:AsyncPostBackTrigger ControlID="btnYes" EventName="Click" />--%>
                            </Triggers>
                            <ContentTemplate>
                            <asp:Panel ID="pnlDisplay" runat="server" Visible="false">
                            
                            <table width="900px">
                             <%-- <td align="left">
                            
                            <asp:Button ID="btnDisplayRefresh" runat="server" Text="Refresh" OnClick="btnDisplayRefresh_Click" ></asp:Button> </td>--%>
                            <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; 
                                    margin-right: 10px; padding-top: 20px;" align="left">
                            <span>Total Servers Count :<asp:Label ID="lblDisplayCount" runat="server" ></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                 </span>
                             <td style="font-size: 12px; font-weight: bold; font-family: Verdana; 
                                    margin-right: 10px; padding-top: 20px;">
                                    <span style="font-weight:bold">
                                     <asp:Label ID="lblClusType" runat="server" Text="ClusterType :"></asp:Label>
                                       <asp:DropDownList ID="ddlClustertype" Width="90px" Height="20px" runat="server" OnSelectedIndexChanged="btnDisplayRefresh_Click"
                                         Font-Bold="False" Font-Size="12px" AutoPostBack="true">
                                         <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="2 Node" Value="2Node"></asp:ListItem>
                                        <asp:ListItem Text="3+ node" Value="3+Node"></asp:ListItem>
                                        <asp:ListItem Text="Standalone" Value="Standalone"></asp:ListItem>
                                         <%--     <asp:ListItem Text="Dev" Value="N"></asp:ListItem>--%>
                                        <%-- <asp:ListItem>Preview</asp:ListItem>--%>
                                    </asp:DropDownList>
                                    </span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <span><asp:Label ID="lblDisplayEnv" runat="server" Text="Environment : "></asp:Label>
                                   <asp:DropDownList ID="ddlDisplayEnv" Width="70px" Height="20px" runat="server" OnSelectedIndexChanged="btnDisplayRefresh_Click"
                                         Font-Bold="False" Font-Size="12px" AutoPostBack="true">
                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="Prod" Value="Prod"></asp:ListItem>
                                        <asp:ListItem Text="UAT" Value="UAT"></asp:ListItem>
                                        <asp:ListItem Text="Dev" Value="Dev"></asp:ListItem>
                                        <asp:ListItem Text="Test" Value="Test"></asp:ListItem>
                                        <asp:ListItem Text="Data Gap" Value="Data Gap"></asp:ListItem>
                                        <asp:ListItem Text="Spare" Value="Spare"></asp:ListItem>
                                        <asp:ListItem Text="SvcCont" Value="SvcCont"></asp:ListItem>
                                   <%--     <asp:ListItem Text="Dev" Value="N"></asp:ListItem>--%>
                                        <%-- <asp:ListItem>Preview</asp:ListItem>--%>
                                    </asp:DropDownList>
                                    </span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                  <asp:Button ID="btnDisplayRefresh" runat="server" Text="Refresh" OnClick="btnDisplayRefresh_Click" ></asp:Button>
                                    </td>
                          </tr>
                            </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlCounts" runat="server" Visible="false">
                            <table width="900px">
                           <%-- <tr>
                             <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 50px;
                                     padding-top: 20px;">
                            <span><asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="ExecuteCompletedTimer_Tick" ></asp:Button> </span>
                            </td>
                            </tr>--%>
                            <tr><td style="font-size: 12px; font-weight: bold; font-family: Verdana; width: 150px;
                                    margin-right: 10px; padding-top: 20px;">
                            <span>Total Servers Count :<asp:Label ID="lblTotalServers" runat="server" ></asp:Label> </span>
                            </td>
                           <td style="font-size: 12px; font-weight: bold; font-family: Verdana; width:600px;
                                    padding-top: 20px;">
                            <span>Process Underway :<asp:Label ID="lblInProgress" runat="server" ></asp:Label>&nbsp;&nbsp;
                            Process Completed :<asp:label id="lblcompleted" runat="server" ></asp:label> &nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblExeceEnv" runat="server" Text="Environment :"></asp:Label>
                            <asp:DropDownList ID="ddlEnvironment" Width="70px" Height="20px" runat="server"
                                        Font-Bold="False" Font-Size="12px" AutoPostBack="true" OnSelectedIndexChanged="ExecuteCompletedTimer_Tick">
                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="Prod" Value="Prod"></asp:ListItem>
                                        <asp:ListItem Text="UAT" Value="UAT"></asp:ListItem>
                                        <asp:ListItem Text="Dev" Value="Dev"></asp:ListItem>
                                        <asp:ListItem Text="Test" Value="Test"></asp:ListItem>
                                        <asp:ListItem Text="Data Gap" Value="Data Gap"></asp:ListItem>
                                        <asp:ListItem Text="Spare" Value="Spare"></asp:ListItem>
                                        <asp:ListItem Text="SvcCont" Value="SvcCont"></asp:ListItem>
                                       
                                    </asp:DropDownList>
                                   

                            </span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <span style="font-weight:bold">
                              <asp:Label ID="lblExeClustype" runat="server" Text="ClusterType :"></asp:Label>
                               <asp:DropDownList ID="ddlExeClusterType" Width="90px" Height="20px" runat="server" OnSelectedIndexChanged="ExecuteCompletedTimer_Tick"
                                         Font-Bold="False" Font-Size="12px" AutoPostBack="true">
                                              <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="2 Node" Value="2Node"></asp:ListItem>
                                        <asp:ListItem Text="3+ node" Value="3+Node"></asp:ListItem>
                                        <asp:ListItem Text="Standalone" Value="Standalone"></asp:ListItem>
                                         <%--     <asp:ListItem Text="Dev" Value="N"></asp:ListItem>--%>
                                        <%-- <asp:ListItem>Preview</asp:ListItem>--%>
                                    </asp:DropDownList> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;    
                                    
                                    </span>
                            </td
                           <td style="font-size: 12px; font-weight: bold; font-family: verdana; width: 350px;
                                    padding-top: 20px;">
                            <span> <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="ExecuteCompletedTimer_Tick" ></asp:Button>  </span>
                            </td>
                           
                            </tr>
                            </table></asp:Panel>
                                
                          <asp:GridView ID="gvClusterExecute" runat="server" AutoGenerateColumns="False" CellPadding="4" ShowHeaderWhenEmpty="true"
                            ForeColor="#333333" GridLines="None" Font-Size="10px" OnRowDataBound="gvClusterExecute_OnRowDataBound"
                            onrowcommand="gvClusterExecute_RowCommand">
                            <AlternatingRowStyle BackColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            <Columns>
                            <asp:TemplateField HeaderText="" ControlStyle-Width="10px">
                               <HeaderTemplate>
                                    <asp:CheckBox ID="chkRunValidationAll" runat="server" Text="Run Validations"/>
                                 </HeaderTemplate>
                                  <HeaderStyle HorizontalAlign="Left"  Width="10px"/>
                                     <ItemTemplate>
                                        <asp:CheckBox ID="chkRunValidation" runat="server"   
                                         Checked='<%# GetCheckedResult(Eval("RunValidationFlag").ToString()) %>'                                          
                                         />  <%-- OnCheckedChanged="ChkRunValidation_Clicked"--%>
                                     </ItemTemplate>
                                 </asp:TemplateField>

                             <asp:TemplateField HeaderText="Force Standalone">
                               
                                     <ItemTemplate>
                                        <asp:CheckBox ID="chkForceStandalone" runat="server"   AutoPostBack="true" OnCheckedChanged="ChkForceStandalone_Clicked"
                                         Checked='<%# GetCheckedResult(Eval("ForceStandaloneFlag").ToString()) %>'                                          
                                         />
                                     </ItemTemplate>
                                 </asp:TemplateField>
                             
                                <asp:TemplateField HeaderText="ServerName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServerName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NodeName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="ClusterType">

                                  
                                    <ItemTemplate>                                  
                                        <asp:Label ID="lblClusterType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClusterType")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="NodeType">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNodeType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NodeType")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="" ControlStyle-Width="10px">
                                 <HeaderTemplate>
                                    <asp:CheckBox ID="ChkPauseAll" runat="server" Text="PauseNode Before Patching"/>
                                 </HeaderTemplate>
                                  <HeaderStyle HorizontalAlign="Left"  Width="10px"/>
                                     <ItemTemplate>
                                         <asp:CheckBox ID="ChkPauseNode" runat="server" Checked='<%# GetCheckedResult(Eval("PauseNodeBeforePatching").ToString()) %>'  />
                                     </ItemTemplate>
                                    <%-- <EditItemTemplate>
                                         <asp:CheckBox ID="CheckBox1" runat="server" />
                                     </EditItemTemplate>--%>
                                   <%--  <ItemStyle HorizontalAlign="Center" />--%>
                                </asp:TemplateField>
                               
                                 <asp:TemplateField HeaderText="Pause During Execution" ControlStyle-Width="60px">
                                    <ItemTemplate>
                                    <asp:Label ID="lblPauseDuringExec" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "PauseNodeDuringExecution")%>'></asp:Label>
                                        <asp:DropDownList ID="ddlPause" runat="server" Font-Size="10px" Width="150px" AutoPostBack="false"
                                        >
                                        <asp:ListItem Text="" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Pause after Patching Only" Value="1"></asp:ListItem>
                                         <asp:ListItem Text="Pause after Patching+failover Only" Value="2"></asp:ListItem>
                                         <asp:ListItem Text="Pause after patching and pause after failover" Value="3"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="">
                                 <HeaderTemplate>
                                    <asp:CheckBox ID="ChkFailBackAll" runat="server"  Text="Failback To Original ClusterState"/>
                                 </HeaderTemplate>
                                  <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkFailBack" runat="server" AutoPostBack="true" OnCheckedChanged="ChkFailBack_Clicked" 
                                        Checked='<%# GetCheckedResult(Eval("FailbackToOriginalState").ToString()) %>'   />
                                    </ItemTemplate>
                                  <%--   <ItemStyle HorizontalAlign="Center" />--%>
                                </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Select the node for Failover" ControlStyle-Width="60px">
                                    <ItemTemplate>
                                    <asp:Label ID="lblNodeClusterName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClusterName")%>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblBackUpNode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BackupNode")%>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddlFailOverNode" runat="server" Font-Size="10px" Width="150px" AutoPostBack="false" 
                                        >
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                       
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="PatchTool">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatchTool" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchTool")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Extension">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExtension" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "IsExtensionServer")%>'  ForeColor='<%# GetExtensionColor(Eval("IsExtensionServer").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="PatchResult(T/S/F/R)">
                                    <ItemTemplate>
                                      
                                        <asp:HyperLink ID="hlPatchResult" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchResult")%>' ></asp:HyperLink>

                                          <asp:Panel runat="server" ID="mpPatchOutPut"  BackColor="LemonChiffon" Style="display: none">
                 
                 
                    <asp:GridView ID="gvPatchOutput" runat="server" AutoGenerateColumns="False" CellPadding="4" ShowHeaderWhenEmpty="true"
                            ForeColor="#333333" GridLines="None" Font-Size="10px">
                            <AlternatingRowStyle BackColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            <Columns>
                                <asp:TemplateField HeaderText="ServerName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServerName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ServerName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                  <asp:TemplateField HeaderText="PatchOutput">
                                    <ItemTemplate>
                                       <asp:Label ID="lblPatchOutput" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UpdateInstallationResults")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                    </asp:GridView>
                    
                    
                  <%--  <asp:Button runat="server" ID="Button6" Text="OK" />--%>
                    <asp:LinkButton runat="server" ID="lnkCancel" Text="Cancel" />
                </asp:Panel>
                <ajaxToolkit:ModalPopupExtender runat="server" ID="mpePatchOutput" TargetControlID="hlPatchResult"
                    PopupControlID="mpPatchOutPut" OkControlID="" CancelControlID="lnkCancel"
                    BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="ServerOnline">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServerOnline" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ServerOnline")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="ServerUpTime DD:HH:MM">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServerUpTime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ServerUpTime")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="PatchStatus">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatchStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchStatus")%>' ForeColor='<%# GetColor(Eval("PatchStatus").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="RunStatus">
                                    <ItemTemplate>
                                        <asp:Label ID="lblrunStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RunStatus")%>' ForeColor='<%# GetColor(Eval("RunStatus").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                      <asp:Label ID="lblClusterName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClusterName")%>' Visible="false"></asp:Label>
                                           <%--<asp:Button ID="btnResume" runat="server" Text="Resume" Font-Size="10px" 
                                           CommandName="Resume"  CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"

                                           CommandArgument='<%#Eval("uid") + ","+#Eval("cartid") %>' 
                                           Enabled='<%# GetCheckedResult(Eval("Flag").ToString()) %>'/>--%>
                                          <%-- <asp:Button ID="btnResume" runat="server" Text="Resume" Font-Size="10px" CommandName="Resume"
                                             CommandArgument='<%#Eval("ClusterName") %>' Enabled='<%# GetCheckedResult(Eval("Flag").ToString()) %>' />--%>
                                             <asp:Button ID="btnResume" runat="server" Text="Resume" Font-Size="10px" OnClick="btnResume_Click"
                                            CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' Enabled='<%# GetCheckedResult(Eval("Flag").ToString()) %>'
                                           />
                                       <%--      <asp:Label ID="lblResumeFlag" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Flag")%>'></asp:Label>--%>
                                    <%--       Enabled='<%# GetCheckedResult(Eval("Flag").ToString()) %>'--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Button ID="btnStop" runat="server" Text="Stop" Font-Size="10px" Enabled="false" />
                                    </ItemTemplate>
                                   
                                </asp:TemplateField>
                                
                                </Columns>
                                </asp:GridView>

                          <div style="margin-left:7px;">
                            <asp:GridView ID="gvHLBExecute" runat="server" AutoGenerateColumns="False" CellPadding="3" ShowHeaderWhenEmpty="true"
                            OnRowDataBound="gvHLBClusterExecute_OnRowDataBound"
                            ForeColor="#333333" GridLines="None" Font-Size="12px">
                            <AlternatingRowStyle BackColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            <Columns>
                               <asp:TemplateField HeaderText="">
                               
                                     <ItemTemplate>
                                         <asp:CheckBox ID="ChkValue" runat="server"  AutoPostBack="true" Checked='<%# GetCheckedResult(Eval("ChkValue").ToString()) %>' 
                                          OnCheckedChanged="ChkVIPIP_Clicked"
                                         />
                                     </ItemTemplate>
                                  
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="" ControlStyle-Width="10px">
                               <HeaderTemplate>
                                    <asp:CheckBox ID="chkRunValidationAllHLB" runat="server" Text="Run Validations"/>
                                 </HeaderTemplate>
                                  <HeaderStyle HorizontalAlign="Left"  Width="10px"/>
                                     <ItemTemplate>
                                        <asp:CheckBox ID="chkRunValidationHLB" runat="server"  
                                         Checked='<%# GetCheckedResult(Eval("RunValidationFlag").ToString()) %>'                                          
                                         /> <%--OnCheckedChanged="ChkRunValidationHLB_Clicked"--%>
                                     </ItemTemplate>
                                 </asp:TemplateField>

                                 <asp:TemplateField HeaderText="VIP Name">

                                  
                                    <ItemTemplate>                                  
                                        <asp:Label ID="lblVIPIP" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "VIP")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                           
                                <asp:TemplateField HeaderText="ServerName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServerName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Servername")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="ClusterType">

                                  
                                    <ItemTemplate>                                  
                                        <asp:Label ID="lblClusterType" runat="server" Text='<%# GetPatchScenario() %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="ENV">

                                  
                                    <ItemTemplate>                                  
                                        <asp:Label ID="lblEnv" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Env")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                          
                               
                                <asp:TemplateField HeaderText="NodeIP">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNodeIP" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NodeIP")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                              <asp:TemplateField HeaderText="Pause During Execution" ControlStyle-Width="100px">
                                    <ItemTemplate>
                                    <asp:Label ID="lblPauseDuringExec" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "IsPaused")%>'></asp:Label>
                                        <asp:DropDownList ID="ddlPause" runat="server" Font-Size="10px" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlPause_SelectedIndexChanged"
                                        
                                        >
                                        <asp:ListItem Text="" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Pause after Patching" Value="1"></asp:ListItem>

                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                                 <asp:TemplateField HeaderText="PatchTool">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatchTool" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchTool")%>'></asp:Label>
                                        <asp:Label ID="lblPort" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Port")%>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                              
                                 <asp:TemplateField HeaderText="PatchResult(T/S/F/R)">
                                    <ItemTemplate>
                                       
                                        <asp:HyperLink ID="hlPatchResult" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchResult")%>' ></asp:HyperLink>

                                          <asp:Panel runat="server" ID="mpPatchOutPut"  BackColor="LemonChiffon" Style="display: none">
                 
                
                                            <asp:GridView ID="gvPatchOutput" runat="server" AutoGenerateColumns="False" CellPadding="4" ShowHeaderWhenEmpty="true"
                            ForeColor="#333333" GridLines="None" Font-Size="10px">
                            <AlternatingRowStyle BackColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            <Columns>
                                <asp:TemplateField HeaderText="ServerName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServerName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ServerName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                  <asp:TemplateField HeaderText="PatchOutput">
                                    <ItemTemplate>
                                       <asp:Label ID="lblPatchOutput" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UpdateInstallationResults")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                    </asp:GridView>
                    
                    
                    <asp:LinkButton runat="server" ID="lnkCancel" Text="Cancel" />
                </asp:Panel>
                                            <ajaxToolkit:ModalPopupExtender runat="server" ID="mpePatchOutput" TargetControlID="hlPatchResult"
                    PopupControlID="mpPatchOutPut" OkControlID="" CancelControlID="lnkCancel"
                    BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="CIState">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServerOnline" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ISOnline")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="ServerUpTime DD:HH:MM">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServerUpTime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ServerUpTime")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="PatchStatus">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatchStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchStatus")%>' ForeColor='<%# GetColor(Eval("PatchStatus").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="RunStatus">
                                    <ItemTemplate>
                                        <asp:Label ID="lblrunStatus" runat="server" Text='<%# Eval("RunStatus")%>' ForeColor='<%# GetColor(Eval("RunStatus").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                      <asp:Label ID="lblVIPName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "VIP")%>' Visible="false"></asp:Label>
                                        
                                             <asp:Button ID="btnResume" runat="server" Text="Resume" Font-Size="10px" 
                                             OnClick="btnHLBResume_Click"
                                            CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' Enabled='<%# GetCheckedResult(Eval("resumeflag").ToString())%>'  />
                                           
                                    
                                    </ItemTemplate>
                                </asp:TemplateField>
                                </Columns>
                                </asp:GridView>
                                </div>
                                <br/>
                                <asp:Label ID="lblText" runat="server"  Visible="false"  style="margin-left:50px;" Text="
                                    T = Total Updates Found , S = Updates Installed Successfully , F = Updates Installation Failed , R= Reboot Required
                                    "></asp:Label>
                                  <asp:Button ID="btnClusterExecute" Width="80px" Height="25px" runat="server" TabIndex="11" style="margin-left:450px"
                                        CssClass="button" Text="Execute" OnClick="btnClusterExecute_Click"  Visible="false"/>
                                           <asp:HiddenField ID="hdnClusterFlag" runat="server" Value="0" />
                                            <div class="Executeloading" style="margin-left:430px;margin-top:20px;">
                        Loading. Please wait.<br />
                        <br />
                        <img src="Images/loading.gif" alt="loading" />
                             </div>
              


             <input type="hidden" runat="server" id="hdnNext" />

                 <ajaxToolkit:ModalPopupExtender ID="mpeClusterExecute" runat="server" PopupControlID="pnlPopup" TargetControlID="hdnNext" 

                    CancelControlID="btnNo" BackgroundCssClass="modalBackground">

                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none;width:300px" >

                    <div class="header" style="width:300px;height:20px;padding-bottom:10px;">

                        Confirmation

                    </div>

                    <div class="body" style="width:300px">

                    <asp:Label ID="lblPopUpMsg" runat="server" Text="One of the ServerName is part of multiple VIPs,Do you Still want to Proceed?" CssClass="clsWrap"/>
                       

                    </div>

                    <div class="footer" align="right">

                        <asp:Button ID="btnYes" runat="server" Text="OK"  OnClick="btnClusterOk_Click"/>

                        <asp:Button ID="btnNo" runat="server" Text="Cancel" />

                    </div>

                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                                
                         
                     </div>
</div>
               
 <script type="text/javascript">
          function IsValid() {
                       //alert('s');

                        if ($('#ddlScenario').find('option:selected').text() == 'HLB Cluster') {
                            var checkedCheckboxes = $("#<%=gvHLBExecute.ClientID%> input[id*='ChkValue']:checkbox:checked").size();
                            //  alert(checkedCheckboxes);
                            if (checkedCheckboxes == 0) {
                                alert("Please select atleast One checkbox to Execute");
                                return false;
                            }

                        }
                    }
         function InIEvent() {

                        $("#btnClusterExecute").click(IsValid);

                    }

                    $(document).ready(InIEvent);


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

     <!-- Ecexute Tab-->
      <script type="text/javascript">
          Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InIEvent);
    </script>
     
      <script type="text/javascript">

          //        $("#btnClusterExecute").click(function () {
          //            //alert('s');
          //            if ($('#ddlScenario').find('option:selected').text() == 'HLB Cluster') {
          //                var checkedCheckboxes = $("#<%=gvHLBExecute.ClientID%> input[id*='ChkValue']:checkbox:checked").size();
          //                alert(checkedCheckboxes);
          //                if (checkedCheckboxes == 0) {
          //                    alert("Please select atleast One checkbox to Execute");
          //                    return false;
          //                }
          //                else {
          //                    return true;
          //                }
          //            }
          //        });




          //        $(document).ready(function () {

          //            $("#btnDisplayRefresh").click(function () {
          //            });
          //            $("#btnClusterExecute").click(function () {
          //                //alert('s');
          //                if ($('#ddlScenario').find('option:selected').text() == 'HLB Cluster') {
          //                    var checkedCheckboxes = $("#<%=gvHLBExecute.ClientID%> input[id*='ChkValue']:checkbox:checked").size();
          //                    alert(checkedCheckboxes);
          //                    if (checkedCheckboxes == 0) {
          //                        alert("Please select atleast One checkbox to Execute");
          //                        return false;
          //                    }
          //                    else {
          //                        return true;
          //                    }
          //                }
          //            });
          //        });

          $(document).ready(function () {
              $("#<%=gvClusterExecute.ClientID%> input[id*='ChkPauseNode']:checkbox").click(function () {
                  var totalCheckboxes = $("#<%=gvClusterExecute.ClientID%> input[id*='ChkPauseNode']:checkbox").size();
                  var checkedCheckboxes = $("#<%=gvClusterExecute.ClientID%> input[id*='ChkPauseNode']:checkbox:checked").size();
                  $("#<%=gvClusterExecute.ClientID%> input[id*='ChkPauseAll']:checkbox").attr('checked', totalCheckboxes == checkedCheckboxes);
              });



              $("#<%=gvClusterExecute.ClientID%> input[id*='ChkFailBack']:checkbox").click(function () {
                  var totalCheckboxes = $("#<%=gvClusterExecute.ClientID%> input[id*='ChkFailBack']:checkbox").size();
                  var checkedCheckboxes = $("#<%=gvClusterExecute.ClientID%> input[id*='ChkFailBack']:checkbox:checked").size();
                  $("#<%=gvClusterExecute.ClientID%> input[id*='ChkFailBackAll']:checkbox").attr('checked', totalCheckboxes == checkedCheckboxes);
              });

              $("[id*=ChkFailBackAll]").live("click", function () {

                  var chkHeader = $(this);
                  var grid = $(this).closest("table");
                  $("input[id*='ChkFailBack']", grid).each(function () {
                      var chk = $(this);

                      if (chkHeader.is(":checked")) {
                          if (chk.is(":disabled")) {
                              // alert(chk);
                              $(this).attr("checked", false);
                          }
                          else {
                              $(this).attr("checked", true);
                          }

                      }
                      else {
                          $(this).attr("checked", false);
                      }
                  });
              });


              $("[id*=ChkPauseAll]").live("click", function () {

                  var chkHeader = $(this);
                  var grid = $(this).closest("table");
                  $("input[id*='ChkPauseNode']", grid).each(function () {
                      var chk = $(this);

                      if (chkHeader.is(":checked")) {
                          if (chk.is(":disabled")) {
                              // alert(chk);
                              $(this).attr("checked", false);
                          }
                          else {
                              $(this).attr("checked", true);
                          }

                      }
                      else {
                          $(this).attr("checked", false);
                      }
                  });
              });
              $("[id*=chkRunValidationAll]").live("click", function () {

                  var chkHeader = $(this);
                  var grid = $(this).closest("table");
                  $("input[id*='chkRunValidation']", grid).each(function () {
                      var chk = $(this);

                      if (chkHeader.is(":checked")) {
                          if (chk.is(":disabled")) {
                              // alert(chk);
                              $(this).attr("checked", false);
                          }
                          else {
                              $(this).attr("checked", true);
                          }

                      }
                      else {
                          $(this).attr("checked", false);
                      }
                  });
              });
              $("[id*=chkRunValidationAllHLB]").live("click", function () {

                  var chkHeader = $(this);
                  var grid = $(this).closest("table");
                  $("input[id*='chkRunValidationHLB']", grid).each(function () {
                      var chk = $(this);

                      if (chkHeader.is(":checked")) {
                          if (chk.is(":disabled")) {
                              // alert(chk);
                              $(this).attr("checked", false);
                          }
                          else {
                              $(this).attr("checked", true);
                          }

                      }
                      else {
                          $(this).attr("checked", false);
                      }
                  });
              });

          });
</script>

     <script type="text/javascript">


         //Execute Tab


         function LoadOptions() {

             if ($('#ddlPatchingOption').find('option:selected').text() == 'OnDemandPatch') {
                 $("#trODPOption").show();
                 $("#trSimpleUpdateOption").hide();
                 $("#trReboot").hide();
                 $("#trOnlyQFE").hide();
                 $("#trExcludeQFE").hide();
             }
             else if ($('#ddlPatchingOption').find('option:selected').text() == 'SimpleUpdate' || $('#ddlPatchingOption').find('option:selected').text() == 'SU-2x') {
                 $("#trODPOption").hide();
                 $("#trSimpleUpdateOption").show();
                 $("#trReboot").hide();
                 $("#trOnlyQFE").hide();
                 $("#trExcludeQFE").hide();
                 //                if ($('#ddlScenario').find('option:selected').text() == 'HLB Cluster') {
                 //                    $('#ddlSimpleUpdateOption >option').remove();
                 //                    $("#<%= ddlSimpleUpdateOption.ClientID %>").append("<option value='install'>" + "Install" + " </option>");
                 //                }
                 //                else {

                 //                    $('#ddlSimpleUpdateOption >option').remove();
                 //                    $("#<%= ddlSimpleUpdateOption.ClientID %>").append("<option value='reboot'>" + "Reboot" + " </option>");
                 //                    $("#<%= ddlSimpleUpdateOption.ClientID %>").append("<option value='install'>" + "Install" + " </option>");

                 //                }

             }
             else if ($('#ddlPatchingOption').find('option:selected').text() == 'Chaining (ODP-SU)') {
                 $("#trODPOption").show();
                 $("#trSimpleUpdateOption").show();
                 $("#trReboot").hide();
                 $("#trOnlyQFE").hide();
                 $("#trExcludeQFE").hide();
                 //                if ($('#ddlScenario').find('option:selected').text() == 'HLB Cluster') {
                 //                    $('#ddlSimpleUpdateOption >option').remove();
                 //                    $("#<%= ddlSimpleUpdateOption.ClientID %>").append("<option value='install'>" + "Install" + " </option>");
                 //                }
                 //                else {

                 //                    $('#ddlSimpleUpdateOption >option').remove();
                 //                    $("#<%= ddlSimpleUpdateOption.ClientID %>").append("<option value='reboot'>" + "Reboot" + " </option>");
                 //                    $("#<%= ddlSimpleUpdateOption.ClientID %>").append("<option value='install'>" + "Install" + " </option>");

                 //                }

             }


             else if ($('#ddlPatchingOption').find('option:selected').text() == 'MSNPatch' || $('#ddlPatchingOption').find('option:selected').text() == 'Select' || $('#ddlPatchingOption').find('option:selected').text() == 'Chaining(ODP-SU-MSNPATCH)') {


                 $("#trODPOption").hide();
                 $("#trSimpleUpdateOption").hide();
                 if ($('#ddlPatchingOption').find('option:selected').text() == 'MSNPatch') {
                     //                    if ($('#ddlScenario').find('option:selected').text() == 'HLB Cluster') {
                     //                        // $("#ddlReboot option[value='0']").attr("disabled", "disabled");
                     //                        $('#ddlReboot >option').remove();
                     //                        $("#<%= ddlReboot.ClientID %>").append("<option value='1'>" + "NoForce Reboot" + " </option>");
                     //                    }
                     //                    else {
                     //                        //   $("#ddlReboot option[value='0']").removeAttr("disabled");
                     //                        $('#ddlReboot >option').remove();
                     //                        $("#<%= ddlReboot.ClientID %>").append("<option value='0'>" + "Force Reboot" + " </option>");
                     //                        $("#<%= ddlReboot.ClientID %>").append("<option value='1'>" + "NoForce Reboot" + " </option>");

                     //                    }
                     $("#trReboot").show();
                     $("#trOnlyQFE").show();
                     $("#trExcludeQFE").show();
                 }
                 else {

                     $("#trReboot").hide();
                     $("#trOnlyQFE").hide();
                     $("#trExcludeQFE").hide();
                 }
             }


         }
         $(document).ready(function () {
             $("#ddlPatchingOption option[value='5']").attr("disabled", "disabled");
             if ($('#ddlScenario').find('option:selected').text() == 'HLB Cluster') {
                 // $('#btnReboot').removeAttr("disabled");
                 lblServerName.innerHTML = "VIP :";
                 lblServerlistPath.innerHTML = "VIPList Path :"
                 lblServersText.innerHTML = "Enter VIPs with , separated";

             }
             else {
                 // $('#btnReboot').attr("disabled", "disabled");
             }


             if ($('#ddlPatchingOption').find('option:selected').text() == 'MSNPatch') {

                 $("#trReboot").show();
                 $("#trOnlyQFE").show();
                 $("#trExcludeQFE").show();

             }
             else {

                 $("#trReboot").hide();
                 $("#trOnlyQFE").hide();
                 $("#trExcludeQFE").hide();

             }

             //  LoadOptions();
                        
         });

         $('#txtExecuteServerNames').change(function () {
             $('#btnExecute').removeAttr("disabled");
             // $('#btnReboot').removeAttr("disabled");
         });

         $('#txtExecuteServerNames').keypress(function (event) {
             var Upload_file = document.getElementById('<%= fupExcel.ClientID %>');
             var myfile = Upload_file.value;
             if (myfile != '') {
                 $('#txtExecuteServerNames').attr("disabled", "disabled");
                 lblExecuteServerNames.innerHTML = "Only One Option Avialble";
             }
             else {

                 $('#txtExecuteServerNames').removeAttr("disabled");
                 $('#btnExecute').removeAttr("disabled");
                 //  $('#btnReboot').removeAttr("disabled");


                 lblExecuteServerNames.innerHTML = "";
             }
         });

         //        $("#btnReboot").click(function () {

         //            var flag = 0;
         //            lblServers.innerHTML = "";
         //            var Upload_file = document.getElementById('<%= fupExcel.ClientID %>');
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

         //            else {

         //                // flag1 = 1;
         //                //lblPreExecute.innerHTML = 'Please Select Excel File ';
         //                if ($('#txtServerNames').val() == '') {
         //                    flag1 = 1;
         //                    lblServernames.innerHTML = 'Please Select Excel File or Enter ServerNames ';
         //                }

         //            }
         //            else {
         //               
         //                    flag = 1;
         //                    lblServers.innerHTML = 'Please Select Excel File';
         //                }


         //            if (flag == 1) {
         //                return false;
         //            }
         //            else {
         //           
         //                return confirm('Are you sure you want to Reboot?');
         //                ShowProgress();
         //                return true;
         //            }

         //        });




         //        $("#btnExportToExcel").click(function () {
         //            ShowProgress();
         //            return true;
         //        });

         //        $("#btnClusterExecute").click(function () {
         //            alert('s');
         //            ShowProgress();
         //            return true;

         //        });

         $("#btnExecute").click(function () {

             //alert( $("#ddlReboot option:selected").text());

             var flag = 0;
             lblServers.innerHTML = "";
             lblPatching.innerHTML = "";
             if ($('#txtDomainAccName').val() == '') {

                 lblAdminAccountName.innerHTML = 'Please Enter Domain Account Name';
                 flag = 1;

             }
             if ($('#txtDomainAcctPwd').val() == '') {

                 lblAdminAcctPwd.innerHTML = 'Please Enter Domain Account Pwd';
                 flag = 1;

             }
             if ($('#txtLogPath').val() == '') {

                 lblLogPath.innerHTML = 'Please Enter LogPath';
                 flag = 1;

             }
             //var hdnFilename = document.getElementById('<%= hdnFileName.ClientID %>').value;
             // if (hdnFilename == "") {
             var Upload_file = document.getElementById('<%= fupExcel.ClientID %>');
             var myfile = Upload_file.value;

             var path = myfile;
             var pos = path.lastIndexOf(path.charAt(path.indexOf(":") + 1));
             var filename = path.substring(pos + 1);
             var RegEx = /^([a-zA-Z0-9_\.])+$/;


             if (myfile.indexOf("xlsx") > 0) {

                 if (!(filename.match(RegEx))) {
                     flag = 1;
                     lblExecuteServerNames.innerHTML = 'Filename should not contain spaces and special characters ';

                 }

             }
             else if (myfile.indexOf("xls") > 0) {
                 if (!(filename.match(RegEx))) {
                     flag = 1;
                     lblExecuteServerNames.innerHTML = 'Filename should not contain spaces and special characters ';

                 }
             }

             else {
                 if ($('#txtExecuteServerNames').val() == '') {
                     flag = 1;
                     lblExecuteServerNames.innerHTML = 'Please Select Excel File or Enter ServerNames ';
                 }

             }


             if ($('#ddlPatchingOption').find('option:selected').text() == 'Select') {
                 lblPatching.innerHTML = 'Please Select Tool Options';
                 flag = 1;
             }
             else {
                 lblPatching.innerHTML = '';
             }
             if ($('#ddlPatchingOption').find('option:selected').text() == 'Chaining(ODP-SU-MSNPATCH)') {
                 lblPatching.innerHTML = 'This Tool Option not avilable.';
                 flag = 1;
             }


             if (flag == 1) {
                 return false;
             }
             else {

                 //   return confirm('Are you sure you want to execute?');
                 ShowProgress();
                 return true;
             }

         });


         $("#ddlScenario").change(function (e) {
             //            alert($('#ddlScenario').val());
             //            alert($('#ddlScenario').find('option:selected').text());
             if ($('#ddlScenario').find('option:selected').text() == 'HLB Cluster') {
                 //                $('#btnReboot').removeAttr("disabled");
                 //                $('#ddlReboot >option').remove();
                 //                $('#ddlSimpleUpdateOption >option').remove();
                 //                $("#<%= ddlReboot.ClientID %>").append("<option value='1'>" + "NoForce Reboot" + " </option>");
                 //                $("#<%= ddlSimpleUpdateOption.ClientID %>").append("<option value='install'>" + "Install" + " </option>");
                 lblServerName.innerHTML = "VIP :";
                 lblServerlistPath.innerHTML = "VIPList Path :";
                 lblServersText.innerHTML = "Enter VIPs with , separated";
             }
             else {
                 //                $('#btnReboot').attr("disabled", "disabled");
                 //                $('#ddlReboot >option').remove();
                 //                $('#ddlSimpleUpdateOption >option').remove();
                 //                $("#<%= ddlReboot.ClientID %>").append("<option value='0'>" + "Force Reboot" + " </option>");
                 //                $("#<%= ddlReboot.ClientID %>").append("<option value='1'>" + "NoForce Reboot" + " </option>");
                 //                $("#<%= ddlSimpleUpdateOption.ClientID %>").append("<option value='reboot'>" + "Reboot" + " </option>");
                 //                $("#<%= ddlSimpleUpdateOption.ClientID %>").append("<option value='install'>" + "Install" + " </option>");
                 lblServerName.innerHTML = "Server Names :";
                 lblServersText.innerHTML = "Enter Servernames with , separated";
                 lblServerlistPath.innerHTML = "ServersList Path :";
             }
         });

         $("#ddlPatchingOption").change(function (e) {
             LoadOptions();

         });
         function CheckEnable() {
             //  $('#btnCheck').removeAttr("disabled");
             $('#btnExecute').removeAttr("disabled");
             //$('#btnReboot').removeAttr("disabled");
             $('#txtExecuteServerNames').removeAttr("disabled");
             $('#txtExecuteServerNames').val('');
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