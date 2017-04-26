<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="Execute.aspx.cs"
    Inherits="PatchingUI.Execute" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc" TagName="ExecuteGrid" Src="~/UserControls/MSCSHLBGrid.ascx" %>
<%--<%@ OutputCache CacheProfile="AppCache1" VaryByParam="*" %>--%>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>  
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table align="center" border="0" width="100%">
        <tr style="display: none">
            <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right"
                class="Label">
                Domain Account Name :
            </td>
            <td style="color: Red; padding-top: 20px;">
                *
            </td>
            <td align="left">
                <asp:TextBox ID="txtDomainAccName" ClientIDMode="Static" Width="295px" Style="margin-left: 5px"
                    Height="20px" TextMode="Password" Text="redmond\stpatcha" runat="server" TabIndex="3"></asp:TextBox>
            </td>
            <td align="left" colspan="2">
                <label id="lblAdminAccountName" style="font-family: Verdana; font-size: 12px; color: red">
                </label>
            </td>
        </tr>
        <tr style="display: none">
            <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right"
                class="Label">
                Domain Account password :
            </td>
            <td style="color: Red;">
                *
            </td>
            <td style="margin: 5px;" align="left">
                <asp:TextBox ID="txtDomainAcctPwd" ClientIDMode="Static" TextMode="Password" Width="295px"
                    Style="margin-left: 5px" Text="#EDCcde3" Height="20px" runat="server" TabIndex="4"></asp:TextBox>
            </td>
            <td align="left" colspan="2">
                <label id="lblAdminAcctPwd" style="font-family: Verdana; font-size: 12px; color: red">
                </label>
            </td>
        </tr>
        <tr style="display: none">
            <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right"
                class="Label">
                LogPath :
            </td>
            <td style="color: Red;">
                *
            </td>
            <td style="margin: 5px;" align="left">
                <asp:TextBox ID="txtLogPath" Width="295px" ClientIDMode="Static" Style="margin-left: 5px"
                    Height="20px" Text="C:\Temp" ReadOnly="true" runat="server" TabIndex="5"></asp:TextBox>
            </td>
            <td align="left" colspan="2">
                <label id="lblLogPath" style="font-family: Verdana; font-size: 12px; color: red">
                </label>
            </td>
        </tr>
        <tr style="display: none">
            <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right"
                class="Label">
                Patch Scenario :
            </td>
            <td style="color: Red; padding-top: 20px;">
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlScenario" ClientIDMode="Static" Width="280px" Height="20px"
                    runat="server" TabIndex="6" Font-Bold="False" Font-Size="12px" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlScenario_SelectedIndexChanged">
                    <asp:ListItem Value="0" Enabled="false">Select</asp:ListItem>
                    <asp:ListItem Value="1">Standalone/MSCS Servers</asp:ListItem>
                    <asp:ListItem Value="2">HLB Cluster</asp:ListItem>
                    <asp:ListItem Enabled="false">Scenario3</asp:ListItem>
                    <asp:ListItem Enabled="false">Scenario4</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td align="left">
                <label id="lblScenario" style="font-family: Verdana; font-size: 12px; color: red">
                </label>
            </td>
            <td>
                &nbsp;
                <%--   <asp:HyperLink ID="hlExecution" ClientIDMode="Static" runat="server" Target="_blank" NavigateUrl="http://sharepoint/sites/ST/Shared%20Documents/Forms/AllItems.aspx?RootFolder=/sites/ST/Shared%20Documents/Standard%20Practices/Automation%20Folder/Automation%20OnBoarding/PowerPatch%204.0/Documents/Input%20Excel%20Templates">Input Template </asp:HyperLink>--%>
            </td>
        </tr>
        <tr style="display: none">
            <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right"
                class="Label">
                BPU :
            </td>
            <td style="color: Red;">
            </td>
            <td style="margin: 5px;" align="left">
                <asp:RadioButtonList ID="rblBPU" ClientIDMode="Static" Width="225px" Height="20px"
                    runat="server" TabIndex="6" RepeatDirection="Horizontal" RepeatLayout="Flow"
                    TextAlign="Right" Font-Bold="false" Font-Names="Verdana" Font-Size="12px">
                    <asp:ListItem Value="1" Selected="True">Non-ECIT Servers</asp:ListItem>
                    <asp:ListItem Value="2">ECIT Servers</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td align="left" colspan="2">
            </td>
        </tr>
        <tr>
            <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right"
                class="Label">
                Option :
            </td>
            <td style="color: Red;">
                *
            </td>
            <td>
                <asp:DropDownList ID="ddlPatchingOption" ClientIDMode="Static" Width="280px" Height="20px"
                    TabIndex="7" runat="server" AutoPostBack="false" Font-Bold="False" Font-Size="12px">
                    <asp:ListItem Enabled="false">Select</asp:ListItem>
                    <asp:ListItem>MSNPatch</asp:ListItem>
                    <asp:ListItem>OnDemandPatch</asp:ListItem>
                    <asp:ListItem Selected="True">SimpleUpdate</asp:ListItem>
                    <asp:ListItem>Chaining (ODP-SU)</asp:ListItem>
                    <asp:ListItem Value="5">Chaining(ODP-SU-MSNPATCH)</asp:ListItem>
                    <asp:ListItem>SU-2x</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td align="left">
                <label id="lblPatching" style="font-family: Verdana; font-size: 12px; color: red">
                </label>
            </td>
            <td nowrap="nowrap">
                <asp:HyperLink ID="HyperLink1" runat="server" ClientIDMode="Static" Target="_blank"
                    NavigateUrl="http://sharepoint/sites/ST/Shared%20Documents/Forms/AllItems.aspx?RootFolder=/sites/ST/Shared%20Documents/Standard%20Practices/Automation%20Folder/Automation%20OnBoarding/PowerPatch%204.0/Documents/Input%20Excel%20Templates">Input Template </asp:HyperLink>
            </td>
        </tr>
        <tr id="trReboot" runat="server" style="display: none" clientidmode="Static">
            <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right"
                class="Label">
                Reboot Option :
            </td>
            <td style="color: Red;">
                *
            </td>
            <td>
                <asp:DropDownList ID="ddlReboot" ClientIDMode="Static" Width="280px" Height="20px"
                    TabIndex="7" runat="server" AutoPostBack="false" Font-Bold="False" Font-Size="12px">
                    <asp:ListItem Value="0">Force Reboot</asp:ListItem>
                    <asp:ListItem Value="1">NoForce Reboot</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td align="left" colspan="2">
                <label id="Label5" style="font-family: Verdana; font-size: 12px; color: red">
                </label>
            </td>
        </tr>
        <tr runat="server" style="display: none" clientidmode="Static" id="trOnlyQFE">
            <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right"
                class="Label">
                OnlyQFE :
            </td>
            <td style="color: Red;">
            </td>
            <td style="">
                <asp:TextBox ID="txtOnlyQFE" ClientIDMode="Static" Width="277px" Style="" Height="20px"
                    Text="" runat="server"></asp:TextBox>
            </td>
            <td align="left" colspan="2">
            </td>
        </tr>
        <tr runat="server" style="display: none" clientidmode="Static" id="trExcludeQFE">
            <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right"
                class="Label">
                ExcludeQFE :
            </td>
            <td style="color: Red;">
            </td>
            <td style="">
                <asp:TextBox ID="txtExcludeQFE" ClientIDMode="Static" Width="277px" Style="" Height="20px"
                    Text="" runat="server"></asp:TextBox>
            </td>
            <td align="left" colspan="2">
            </td>
        </tr>
        <tr id="trODPOption" runat="server" style="display: none" clientidmode="Static">
            <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right"
                class="Label">
                OnDemandPatch Option :
            </td>
            <td style="color: Red;">
                *
            </td>
            <td>
                <asp:DropDownList ID="ddlODPOption" ClientIDMode="Static" Width="280px" Height="20px"
                    runat="server" TabIndex="8" Font-Bold="False" Font-Size="12px">
                    <asp:ListItem>Patch</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td align="left" colspan="2">
            </td>
        </tr>
        <tr id="trSimpleUpdateOption" runat="server" clientidmode="Static">
            <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right"
                class="Label">
                SimpleUpdate Option :
            </td>
            <td style="color: Red;">
                *
            </td>
            <td>
                <asp:DropDownList ID="ddlSimpleUpdateOption" ClientIDMode="Static" Width="280px"
                    Height="20px" runat="server" TabIndex="9" Font-Bold="False" Font-Size="12px"
                    AutoPostBack="false">
                    <asp:ListItem Text="Install To Success" Value="reboottosuccess"></asp:ListItem>
                    <asp:ListItem Text="Reboot" Value="reboot"></asp:ListItem>
                    <asp:ListItem Text="Install" Value="install"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 300px;">
            </td>
            <td align="left">
            </td>
        </tr>
        <tr>
            <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right"
                class="Label">
                <label id="lblServerlistPath" style="font-family: Verdana; font-size: 12px;">
                    ServersList Path :
                </label>
            </td>
            <td style="color: Red;">
                *
            </td>
            <td>
                <asp:FileUpload ID="fupExcel" runat="server" ClientIDMode="Static" onchange="CheckEnable()"
                    Width="280px" />
            </td>
            <td colspan="2" align="left">
                <label id="lblServers" style="font-family: Verdana; font-size: 12px; color: red">
                </label>
            </td>
        </tr>
        <tr>
            <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right">
                Select GroupName :
            </td>
            <td style="color: Red;">
            </td>
            <td>
                <asp:DropDownList ID="ddlExecuteNames" ClientIDMode="Static" runat="server" Width="280px"
                    Height="20px" TabIndex="6" Font-Bold="False" Font-Size="12px">
                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td colspan="2">
                <%--<label id="lblddl" style="font-family: Verdana; font-size: 12px; color: red">
                                    </label>--%>
            </td>
        </tr>
        <tr>
            <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right"
                class="Label">
                <label id="lblServerName" style="font-family: Verdana; font-weight: bold;">
                    Server Names :
                </label>
                <%--  Server Names :--%>
            </td>
            <td style="color: Red;">
            </td>
            <td style="">
                <asp:TextBox ID="txtExecuteServerNames" ClientIDMode="Static" runat="server" Width="277px"></asp:TextBox>
            </td>
            <td colspan="2" align="left">
                <label id="lblExecuteServerNames" style="font-family: Verdana; font-size: 12px; color: red">
                </label>
            </td>
        </tr>
        <tr>
            <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right"
                class="Label">
            </td>
            <td style="color: Red;">
            </td>
            <td style="">
                <label id="lblServersText" style="font-family: Verdana; font-weight: bold; font-size: 10px;">
                    Enter Servernames with , separated
                </label>
            </td>
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <div class="loading" style="margin-left: 430px; margin-top: 20px;">
                    Loading. Please wait.<br />
                    <br />
                    <img src="Images/loading.gif" alt="loading" />
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="5">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="5" align="center">
                <asp:Button ID="btnExecute" ClientIDMode="Static" Width="80px" Height="25px" runat="server"
                    TabIndex="11" CssClass="button" Text="Display" OnClick="btnExecute_Click" />
                <asp:Button ID="btnExportToExcel" ClientIDMode="Static" Width="150px" Height="25px"
                    runat="server" Enabled="false" TabIndex="12" CssClass="button" Text="Export to Excel"
                    OnClick="btnExportToExcel_Click" />
            </td>
        </tr>
        <!--label processing-->
        <tr>
        <td>
          <asp:label id="lblprocessing" runat="server" style="font-family: Verdana; display:none; font-size: 20px; color: red;" text="Processing...." ></asp:label>
        </td>
        </tr>
        <!--end of label processing-->
    </table>
    <div style="margin-top: 25px; margin-right: 0px; padding-right: 30px; height: auto;"
        runat="server" id="divClusterExecute" visible="false">
        <asp:Timer ID="ExecuteTimer" ClientIDMode="Static" OnTick="ExecuteTimer_Tick" runat="server"
            Enabled="False">
        </asp:Timer>
        <asp:Timer ID="ExecuteCompletedTimer" ClientIDMode="Static" OnTick="ExecuteCompletedTimer_Tick"
            runat="server" Enabled="False">
        </asp:Timer>
        <asp:UpdateProgress ID="uprogressExecute" runat="server" AssociatedUpdatePanelID="upExecute">
            <ProgressTemplate>
                <div class="overlay" />
                <div class="overlayContent">
                    <h2>
                        Loading...</h2>
                    <img src="Images/ajax-loader.gif" alt="Loading" border="1" />
                </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="upExecute" ClientIDMode="Static" UpdateMode="Conditional" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ExecuteTimer" EventName="Tick" />
                <asp:AsyncPostBackTrigger ControlID="ExecuteCompletedTimer" EventName="Tick" />
                <%--    <asp:AsyncPostBackTrigger ControlID="btnYes" EventName="Click" />--%>
            </Triggers>
            <ContentTemplate>
                 <asp:Panel ID="pnlDisplay" runat="server">
                                <table width="100%" style="font-size: 12px; font-weight: bold; font-family: Verdana;" border="0" cellspacing="0" cellpadding="2">
                                    <tr>
                                        <td width="20%">
                                            <asp:Label ID="lblDisplayCount" runat="server" Text="Total Servers Count :"></asp:Label>&nbsp;<asp:Label ID="lblTotalServers" runat="server" ></asp:Label>
                                        </td>
                                        <td width="20%">
                                            <asp:Label ID="lblProcessUnderWay" runat="server" Text="Process Underway :" Visible="false"></asp:Label>&nbsp;<asp:Label ID="lblInProgress" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td width="20%">
                                            <asp:Label ID="lblProcessCompleted" runat="server" Text="Process Completed :" Visible="false"></asp:Label>&nbsp;<asp:label id="lblcompleted" runat="server" Visible="false"></asp:label> 
                                        </td>
                                        <td align="right" width="20%">
                                            <asp:Label ID="lblFilterBy" runat="server" Text="Filter By: "></asp:Label>&nbsp;
                                            <asp:DropDownList ID="ddlFilterBy" runat="server" OnSelectedIndexChanged="btnDisplayRefresh_Click" Font-Bold="False" Font-Size="12px" AutoPostBack="true">
                                                <asp:ListItem Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="HLB" Value="ClusterType1"></asp:ListItem>
                                                <asp:ListItem Text="Active" Value="NodeType1"></asp:ListItem>
                                                <asp:ListItem Text="Passive" Value="NodeType2"></asp:ListItem>
                                                <asp:ListItem Text="Extension" Value="IsExtensionServer"></asp:ListItem>
                                                <asp:ListItem Text="Standalone" Value="ClusterType2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="right" width="20%">
                                            <asp:Button ID="btnDisplayRefresh" runat="server" Text="Refresh" OnClick="ExecuteTimer_Tick" ></asp:Button>
                                            <asp:Button ID="btnExecuteRefresh" runat="server" Text="Refresh" OnClick="ExecuteCompletedTimer_Tick" Visible="false"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
               
                <asp:GridView ID="gvClusterExecute" ClientIDMode="Static" runat="server" AutoGenerateColumns="False"
                    CellPadding="4" ShowHeaderWhenEmpty="true" ForeColor="#333333" GridLines="None"
                    Font-Size="10px" OnRowDataBound="gvClusterExecute_OnRowDataBound" OnRowCommand="gvClusterExecute_RowCommand">
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
                    <Columns>
                        <asp:TemplateField HeaderText="" ControlStyle-Width="10px">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkRunValidationAll" runat="server" Text="Run Validations" />
                            </HeaderTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="10px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkRunValidation" ClientIDMode="Static" runat="server" Checked='<%# GetCheckedResult(Eval("RunValidationFlag").ToString()) %>' />
                                <%-- OnCheckedChanged="ChkRunValidation_Clicked"--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblHeadForceSa" runat="server" ToolTip="Force Standalone" Text="Force SA"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkForceStandalone" runat="server" Checked='<%# GetCheckedResult(Eval("ForceStandaloneFlag").ToString()) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ServerName">
                            <ItemTemplate>
                                <asp:Label ID="lblServerName" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NodeName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ClusterType">
                            <ItemTemplate>
                                <asp:Label ID="lblClusterType" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClusterType")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NodeType">
                            <ItemTemplate>
                                <asp:Label ID="lblNodeType" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NodeType")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ControlStyle-Width="10px">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ChkPauseAll" ClientIDMode="Static" runat="server" Text="PauseNode Before Patching" />
                            </HeaderTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="10px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="ChkPauseNode" ClientIDMode="Static" runat="server" Checked='<%# GetCheckedResult(Eval("PauseNodeBeforePatching").ToString()) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pause During Execution" ControlStyle-Width="60px">
                            <ItemTemplate>
                                <asp:Label ID="lblPauseDuringExec" runat="server" ClientIDMode="Static" Visible="false"
                                    Text='<%# DataBinder.Eval(Container.DataItem, "PauseNodeDuringExecution")%>'></asp:Label>
                                <asp:DropDownList ID="ddlPause" runat="server" Font-Size="10px" Width="150px" AutoPostBack="false">
                                    <asp:ListItem Text="" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Pause after Patching Only" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Pause after Patching+failover Only" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Pause after patching and pause after failover" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ChkFailBackAll" ClientIDMode="Static" runat="server" Text="Failback To Original ClusterState" />
                            </HeaderTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:CheckBox ID="ChkFailBack" ClientIDMode="Static" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="ChkFailBack_Clicked" Checked='<%# GetCheckedResult(Eval("FailbackToOriginalState").ToString()) %>' />
                            </ItemTemplate>
                            <%--   <ItemStyle HorizontalAlign="Center" />--%>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select the node for Failover" ControlStyle-Width="60px">
                            <ItemTemplate>
                                <asp:Label ID="lblNodeClusterName" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClusterName")%>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblBackUpNode" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BackupNode")%>'
                                    Visible="false"></asp:Label>
                                <asp:DropDownList ID="ddlFailOverNode" runat="server" Font-Size="10px" Width="150px"
                                    AutoPostBack="false">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Patch Tool">
                            <ItemTemplate>
                                <asp:Label ID="lblPatchTool" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchTool")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Extension">
                            <ItemTemplate>
                                <asp:Label ID="lblExtension" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "IsExtensionServer")%>'
                                    ForeColor='<%# GetExtensionColor(Eval("IsExtensionServer").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Patch Result (T/S/F/R)">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlPatchResult" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchResult")%>'></asp:HyperLink>
                                <asp:Panel runat="server" ID="mpPatchOutPut" ClientIDMode="Static" BackColor="LemonChiffon"
                                    Style="display: none">
                                    <asp:GridView ID="gvPatchOutput" ClientIDMode="Static" runat="server" AutoGenerateColumns="False"
                                        CellPadding="4" ShowHeaderWhenEmpty="true" ForeColor="#333333" GridLines="None"
                                        Font-Size="10px">
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
                                        <Columns>
                                            <asp:TemplateField HeaderText="ServerName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServerName" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ServerName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PatchOutput">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatchOutput" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UpdateInstallationResults")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:LinkButton runat="server" ClientIDMode="Static" ID="lnkCancel" Text="Cancel" />
                                </asp:Panel>
                                <ajaxToolkit:ModalPopupExtender runat="server" ClientIDMode="Static" ID="mpePatchOutput"
                                    TargetControlID="hlPatchResult" PopupControlID="mpPatchOutPut" OkControlID=""
                                    CancelControlID="lnkCancel" BackgroundCssClass="modalBackground">
                                </ajaxToolkit:ModalPopupExtender>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Server Online">
                            <ItemTemplate>
                                <asp:Label ID="lblServerOnline" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ServerOnline")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Server UpTime DD:HH:MM">
                            <ItemTemplate>
                                <asp:Label ID="lblServerUpTime" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ServerUpTime")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PatchStatus">
                            <ItemTemplate>
                                <asp:Label ID="lblPatchStatus" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchStatus")%>'
                                    ForeColor='<%# GetColor(Eval("PatchStatus").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RunStatus">
                            <ItemTemplate>
                                <asp:Label ID="lblrunStatus" ClientIDMode="Static" runat="server" Text='<%# GetFormatedData(Eval("RunStatus").ToString())%>'
                                    ForeColor='<%# GetColor(Eval("RunStatus").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Label ID="lblClusterName" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClusterName")%>'
                                    Visible="false"></asp:Label>
                                <asp:Button ID="btnResume" ClientIDMode="Static" runat="server" Text="Resume" Font-Size="10px"
                                    OnClick="btnResume_Click" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                    Enabled='<%# GetCheckedResult(Eval("Flag").ToString()) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Button ID="btnStop" ClientIDMode="Static" runat="server" Text="Stop" Font-Size="10px"
                                    Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div style="margin-left: 7px;">
                    <asp:GridView ID="gvHLBExecute" ClientIDMode="Static" runat="server" AutoGenerateColumns="False"
                        CellPadding="3" ShowHeaderWhenEmpty="true" OnRowDataBound="gvHLBClusterExecute_OnRowDataBound"
                        ForeColor="#333333" GridLines="None" Font-Size="12px">
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
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkValue" runat="server" ClientIDMode="Static" AutoPostBack="true"
                                        Checked='<%# GetCheckedResult(Eval("ChkValue").ToString()) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ControlStyle-Width="10px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkRunValidationAllHLB" runat="server" Text="Run Validations" />
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="10px" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRunValidationHLB" runat="server" Checked='<%# GetCheckedResult(Eval("RunValidationFlag").ToString()) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="VIP Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblVIPIP" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "VIP")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ServerName">
                                <ItemTemplate>
                                    <asp:Label ID="lblServerName" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Servername")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ClusterType">
                                <ItemTemplate>
                                    <asp:Label ID="lblClusterType" ClientIDMode="Static" runat="server" Text='<%# GetPatchScenario() %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ENV">
                                <ItemTemplate>
                                    <asp:Label ID="lblEnv" runat="server" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "Env")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="NodeIP">
                                <ItemTemplate>
                                    <asp:Label ID="lblNodeIP" runat="server" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "NodeIP")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pause During Execution" ControlStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblPauseDuringExec" runat="server" ClientIDMode="Static" Visible="false"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "IsPaused")%>'></asp:Label>
                                    <asp:DropDownList ID="ddlPause" runat="server" ClientIDMode="Static" Font-Size="10px"
                                        Width="150px">
                                        <asp:ListItem Text="" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Pause after Patching" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PatchTool">
                                <ItemTemplate>
                                    <asp:Label ID="lblPatchTool" runat="server" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "PatchTool")%>'></asp:Label>
                                    <asp:Label ID="lblPort" runat="server" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "Port")%>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PatchResult(T/S/F/R)">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlPatchResult" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchResult")%>'></asp:HyperLink>
                                    <asp:Panel runat="server" ID="mpPatchOutPut" ClientIDMode="Static" BackColor="LemonChiffon"
                                        Style="display: none">
                                        <asp:GridView ID="gvPatchOutput" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            ShowHeaderWhenEmpty="true" ForeColor="#333333" GridLines="None" Font-Size="10px">
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
                                        PopupControlID="mpPatchOutPut" OkControlID="" CancelControlID="lnkCancel" BackgroundCssClass="modalBackground">
                                    </ajaxToolkit:ModalPopupExtender>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CIState">
                                <ItemTemplate>
                                    <asp:Label ID="lblServerOnline" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ISOnline")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ServerUpTime DD:HH:MM">
                                <ItemTemplate>
                                    <asp:Label ID="lblServerUpTime" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ServerUpTime")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PatchStatus">
                                <ItemTemplate>
                                    <asp:Label ID="lblPatchStatus" ClientIDMode="Static" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchStatus")%>'
                                        ForeColor='<%# GetColor(Eval("PatchStatus").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RunStatus">
                                <ItemTemplate>
                                    <asp:Label ID="lblrunStatus" ClientIDMode="Static" runat="server" Text='<%# Eval("RunStatus")%>'
                                        ForeColor='<%# GetColor(Eval("RunStatus").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:Label ID="lblVIPName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "VIP")%>'
                                        Visible="false"></asp:Label>
                                    <asp:Button ID="btnResume" runat="server" Text="Resume" Font-Size="10px" OnClick="btnHLBResume_Click"
                                        CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' Enabled='<%# GetCheckedResult(Eval("resumeflag").ToString())%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                  <uc:ExecuteGrid runat="server" ID="ucMSCSHLBGrid" OnchkFailbackClick="ucMSCSHLBGrid_chkFailbackClick" OngridbtnResumeClick="ucMSCSHLBGrid_gridbtnResumeClick" OnGridOnRowDataBound="ucMSCSHLBGrid_GridOnRowDataBound" />
                  <br />
                <asp:Label ID="lblText" runat="server" Visible="false" Style="margin-left: 50px;"
                    Text="T = Total Updates Found , S = Updates Installed Successfully , F = Updates Installation Failed , R= Reboot Required"></asp:Label>
                <asp:Button ID="btnClusterExecute" Width="80px" Height="25px" runat="server" TabIndex="11"
                    Style="margin-left: 450px" CssClass="button" Text="Execute" OnClick="btnClusterExecute_Click"
                    Visible="false" />
                <asp:HiddenField ID="hdnClusterFlag" runat="server" Value="0" />
                <div class="Executeloading" style="margin-left: 430px; margin-top: 20px;">
                    Loading. Please wait.<br />
                    <br />
                    <img src="Images/loading.gif" alt="loading" />
                </div>
                <input type="hidden" runat="server" id="hdnNext" />
                <ajaxToolkit:ModalPopupExtender ID="mpeClusterExecute" ClientIDMode="Static" runat="server"
                    PopupControlID="pnlPopup" TargetControlID="hdnNext" CancelControlID="btnNo" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlPopup" runat="server" ClientIDMode="Static" CssClass="modalPopup"
                    Style="display: none; width: 300px">
                    <div class="header" style="width: 300px; height: 20px; padding-bottom: 10px;">
                        Confirmation
                    </div>
                    <div class="body" style="width: 300px">
                        <asp:Label ID="lblPopUpMsg" runat="server" ClientIDMode="Static" Text="One of the ServerName is part of multiple VIPs,Do you Still want to Proceed?"
                            CssClass="clsWrap" />
                    </div>
                    <div class="footer" align="right">
                        <asp:Button ID="btnYes" runat="server" ClientIDMode="Static" Text="OK" OnClick="btnClusterOk_Click" />
                        <asp:Button ID="btnNo" runat="server" ClientIDMode="Static" Text="Cancel" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:HiddenField ID="PageID" runat="server" />
    <asp:HiddenField ID="hdnClusterExecute" runat="server" Value="" />
    <asp:HiddenField ID="hdnExecuteFileName" runat="server" Value="" />
    <asp:HiddenField ID="hdnBackUpNode" runat="server" Value="0" />
    <asp:HiddenField ID="hdnFileName" runat="server" Value="" />
    <asp:HiddenField ID="hdnExecute" runat="server" Value="" />
     <asp:HiddenField ID="hdnInputType" runat="server" Value="0" />
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
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
    <!-- Execute Tab-->
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InIEvent);
    </script>
    <script type="text/javascript">
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

            }
            else if ($('#ddlPatchingOption').find('option:selected').text() == 'Chaining (ODP-SU)') {
                $("#trODPOption").show();
                $("#trSimpleUpdateOption").show();
                $("#trReboot").hide();
                $("#trOnlyQFE").hide();
                $("#trExcludeQFE").hide();

            }


            else if ($('#ddlPatchingOption').find('option:selected').text() == 'MSNPatch' || $('#ddlPatchingOption').find('option:selected').text() == 'Select' || $('#ddlPatchingOption').find('option:selected').text() == 'Chaining(ODP-SU-MSNPATCH)') {


                $("#trODPOption").hide();
                $("#trSimpleUpdateOption").hide();
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

        });

        $("#ddlExecuteNames").change(function (e) {
            $('#btnExecute').removeAttr("disabled");
        });
        $('#txtExecuteServerNames').change(function () {
            $('#btnExecute').removeAttr("disabled");

        });

        $('#txtExecuteServerNames').keypress(function (event) {
            var Upload_file = document.getElementById('<%= fupExcel.ClientID %>');
            var myfile = Upload_file.value;
            if ($('#ddlExecuteNames').find('option:selected').text() != 'Select') {
                $('#txtExecuteServerNames').attr("disabled", "disabled");
                lblServernames.innerHTML = "Only One Option Avialble";
            }
            else if (myfile != '') {
                $('#txtExecuteServerNames').attr("disabled", "disabled");
                lblExecuteServerNames.innerHTML = "Only One Option Avialble";
            }
            else {

                $('#txtExecuteServerNames').removeAttr("disabled");
                $('#btnExecute').removeAttr("disabled");
                lblExecuteServerNames.innerHTML = "";
            }
        });



        $("#btnExecute").click(function () {
            var flag = 0;
            lblServers.innerHTML = "";
            lblPatching.innerHTML = "";
            if ($('#txtLogPath').val() == '') {

                lblLogPath.innerHTML = 'Please Enter LogPath';
                flag = 1;

            }

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
                if ($('#ddlExecuteNames').find('option:selected').text() == 'Select') {
                    if ($('#txtExecuteServerNames').val() == '') {
                        flag = 1;
                        lblExecuteServerNames.innerHTML = 'Please Select Excel File or GroupName or  Enter ServerNames ';
                    }
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
                //ShowProgress();
                $("[id*=lblprocessing]").show();
                return true;
            }

        });


        $("#ddlScenario").change(function (e) {
            if ($('#ddlScenario').find('option:selected').text() == 'HLB Cluster') {
                lblServerName.innerHTML = "VIP :";
                lblServerlistPath.innerHTML = "VIPList Path :";
                lblServersText.innerHTML = "Enter VIPs with , separated";
            }
            else {

                lblServerName.innerHTML = "Server Names :";
                lblServersText.innerHTML = "Enter Servernames with , separated";
                lblServerlistPath.innerHTML = "ServersList Path :";
            }
        });

        $("#ddlPatchingOption").change(function (e) {
            LoadOptions();

        });
        function CheckEnable() {
            $('#btnExecute').removeAttr("disabled");
            $('#txtExecuteServerNames').removeAttr("disabled");
            $('#txtExecuteServerNames').val('');
        }

       
    </script>
</asp:Content>
