<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PatchTool.aspx.cs" Inherits="PatchingUI.PatchTool" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
    <%@ Register TagPrefix="uc" TagName="Plan" Src="~/UserControls/Plan.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Power Patch</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="Styles/datepicker.css" rel="stylesheet" type="text/css" />
   
    <style type="text/css">
        
       .clsWrap
        {
            word-break: break-all;
        }
        /* <![CDATA[ */
        /*Modal Popup*/
.modalBackground {
	background-color:Gray;
	filter:alpha(opacity=70);
	opacity:0.7;
}

.modalPopup {
	background-color:#ffffdd;
	border-width:3px;
	border-style:solid;
	border-color:Gray;
	padding:3px;
	width:250px;
}
        .progressbaron
        {
            background-image: url("Images/loading.gif");
            background-position: center top;
            background-repeat: no-repeat;
            height: 26px;
            width: 193px;
        }
        
        .progressbaroff
        {
            height: 6px;
            width: 193px;
        }
        #NavigationMenu img.icon
        {
            border-style: none;
            vertical-align: middle;
        }
        #NavigationMenu img.separator
        {
            border-style: none;
            display: block;
        }
        #NavigationMenu img.horizontal-separator
        {
            border-style: none;
            vertical-align: middle;
        }
        #NavigationMenu ul
        {
            list-style: none;
            margin: 0;
            padding: 0;
            width: auto;
        }
        #NavigationMenu ul.dynamic
        {
            z-index: 1;
        }
        #NavigationMenu a
        {
            text-decoration: none;
            white-space: nowrap;
            display: block;
        }
        #NavigationMenu a
        {
            border-style: None;
            padding: 5px 10px 5px 10px;
            text-decoration: none;
        }
        /*  #NavigationMenu a.popout
        {
            background-image: url("/CustomerPortal/WebResource.axd?d=XVGOII1qoPZIpOoBrBytoEEXbWI2mOFKtvpcWaR1YxqcL9NKyx8tZ-lzwlLEPB7sBm1k1XKivSUCa4j-9dlQ9VbhkUWkGB8OZ7fTbsWBRgE1&t=634752949420453670");
            background-repeat: no-repeat;
            background-position: right center;
            padding-right: 14px;
        }*/
        #NavigationMenu a.selected
        {
            background-color: Black;
            border-color: #EDBD24;
            border-width: 0px;
            border-style: Outset;
            text-decoration: none;
        }
        
        /* ]]> */
    </style>

    <style type="text/css">
        .modal
        {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }
        
        
        
        .loading
        {
            font-family: Arial;
            font-size: 10pt;
           /* border: 5px solid #67CFF5;*/
           border: 2px solid #1E90FF;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }
         .Executeloading
        {
            font-family: Arial;
            font-size: 10pt;
           /* border: 5px solid #67CFF5;*/
           border: 2px solid #1E90FF;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }
         
    </style>

    <style type="text/css">
        .overlay
        {
          position: fixed;
          z-index: 98;
          top: 0px;
          left: 0px;
          right: 0px;
          bottom: 0px;
          background-color: #aaa;
          filter: alpha(opacity=80);
          opacity: 0.8;
        }
        .overlayContent
        {
          z-index: 99;
          margin: 250px auto;
          width: 80px;
          height: 80px;
        }
        .overlayContent h2
        {
            font-size: 14px;
            font-weight: bold;
            color: #000;
        }
        .overlayContent img
        {
          width: 40px;
          height: 40px;
        }
        span.tab{
            padding: 0 80px; 
        }
    </style>
  

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="30000" ScriptMode="Release">
    <Scripts>
    <asp:ScriptReference Path="~/Scripts/jquery-1.4.1.min.js" />
    </Scripts>
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
            <div id="" class="clear hideSkiplink">
                <div class="menu" id="NavigationMenu">
                    <ul id="tabs" class="level1">
                        <li><a id="tab_1" class="selected" href="javascript:tabSwitch('tab_1', 'content_1');">
                            Plan</a></li>
                        <li><a id="tab_2" class="" href="javascript:tabSwitch('tab_2', 'content_2');">Prep</a></li>
                        <li><a id="tab_3" class="" href="javascript:tabSwitch('tab_3', 'content_3');">Execute</a></li>
                        <li><a id="tab_4" class="" href="javascript:tabSwitch('tab_4', 'content_4');">Validate</a></li>
                        <li><a id="tab_5" class="" href="javascript:tabSwitch('tab_5', 'content_5');">Reports</a></li>
                    </ul>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1"  runat="server">    
                    <ContentTemplate>             
                        <asp:Timer ID="timerSrvcChk" OnTick="timerSrvcChk_Tick" runat="server" Interval="10000" Enabled="true"></asp:Timer>
                        <span class="tab"></span> 
                        <asp:Label ID="lblServiceErrorMessage" runat="server" Text="" ForeColor="Red" Font-Bold="true" ></asp:Label>
                    </ContentTemplate>    
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="main">
            <div id="tabs_content_container">
                <div id="content_1" class="tab_content current" style="display: block;" runat="server">
                    <uc:Plan id="ucPlan"  runat="server" MinValue="1" MaxValue="10" />
                </div>
                <div id="content_2" class="tab_content current" style="display: none;" runat="server">
                    <table width="100%">
                        <%--<tr>
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
                                    <asp:ListItem Value="2" Enabled="false">HLB Cluster</asp:ListItem>
                                    <asp:ListItem Enabled="false">Scenario3</asp:ListItem>
                                    <asp:ListItem Enabled="false">Scenario4</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px; margin-left: 10px" align="left">
                            </td>
                            <td style="padding-top: 20px; padding-left: 20px; width: 200px;" align="right">
                                <asp:HyperLink ID="hlPrep" runat="server" Target="_blank" NavigateUrl="http://sharepoint/sites/ST/Shared%20Documents/Forms/AllItems.aspx?RootFolder=/sites/ST/Shared%20Documents/Standard%20Practices/Automation%20Folder/Automation%20OnBoarding/PowerPatch%204.0/Documents/Input%20Excel%20Templates">Input Template </asp:HyperLink>
                            </td>
                        </tr>--%>
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
                            <td align="right" nowrap="nowrap">
                                <asp:HyperLink ID="hlPrep" runat="server" Target="_blank" NavigateUrl="http://sharepoint/sites/ST/Shared%20Documents/Forms/AllItems.aspx?RootFolder=/sites/ST/Shared%20Documents/Standard%20Practices/Automation%20Folder/Automation%20OnBoarding/PowerPatch%204.0/Documents/Input%20Excel%20Templates">Input Template </asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-left: 60px"
                                align="right" class="Label">
                                Select GroupName :
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td>
                               <asp:DropDownList ID="ddlPrepNames" runat="server" Width="280px" Height="20px" 
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
                                <asp:TextBox ID="txtServerNames" runat="server" Width="277px"></asp:TextBox>
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
                </div>
                <div id="content_3" class="tab_content" runat="server" style="display: none;">
                    <div id="divMain">
                        <table align="center" border="0">
                            <tr style="display: none">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right" class="Label">
                                    Domain Account Name :
                                </td>
                                <td style="color: Red; padding-top: 20px;">
                                    *
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDomainAccName" Width="295px" Style="margin-left: 5px" Height="20px" TextMode="Password"
                                        Text="redmond\stpatcha" runat="server" TabIndex="3"></asp:TextBox>
                                </td>
                                <td align="left" colspan="2">
                                    <label id="lblAdminAccountName" style="font-family: Verdana; font-size: 12px; color: red">
                                    </label>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right" class="Label">
                                    Domain Account password :
                                </td>
                                <td style="color: Red;">
                                    *
                                </td>
                                <td style="margin: 5px;" align="left">
                                    <asp:TextBox ID="txtDomainAcctPwd" TextMode="Password" Width="295px" Style="margin-left: 5px"
                                        Text="#EDCcde3" Height="20px" runat="server" TabIndex="4"></asp:TextBox>
                                </td>
                                <td align="left" colspan="2">
                                    <label id="lblAdminAcctPwd" style="font-family: Verdana; font-size: 12px; color: red">
                                    </label>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right" class="Label">
                                    LogPath :
                                </td>
                                <td style="color: Red;">
                                    *
                                </td>
                                <td style="margin: 5px;" align="left">
                                    <asp:TextBox ID="txtLogPath" Width="295px" Style="margin-left: 5px" Height="20px"
                                        Text="C:\Temp" ReadOnly="true" runat="server" TabIndex="5"></asp:TextBox>
                                </td>
                                <td align="left" colspan="2">
                                    <label id="lblLogPath" style="font-family: Verdana; font-size: 12px; color: red">
                                    </label>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right" class="Label">
                                    Patch Scenario :
                                </td>
                                <td style="color: Red; padding-top: 20px;">
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlScenario" Width="225px" Height="20px" runat="server" TabIndex="6" 
                                        Font-Bold="False" Font-Size="12px" AutoPostBack="true" OnSelectedIndexChanged="ddlScenario_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
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
                                    <asp:HyperLink ID="hlExecution" runat="server" Target="_blank" NavigateUrl="http://sharepoint/sites/ST/Shared%20Documents/Forms/AllItems.aspx?RootFolder=/sites/ST/Shared%20Documents/Standard%20Practices/Automation%20Folder/Automation%20OnBoarding/PowerPatch%204.0/Documents/Input%20Excel%20Templates">Input Template </asp:HyperLink>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right" class="Label">
                                    BPU :
                                </td>
                                <td style="color: Red;">
                                </td>
                                <td style="margin: 5px;" align="left">
                                    <asp:RadioButtonList ID="rblBPU" Width="225px" Height="20px" runat="server" TabIndex="6"  RepeatDirection="Horizontal" 
                                            RepeatLayout="Flow"  TextAlign="Right"
                                        Font-Bold="false" Font-Names="Verdana" Font-Size="12px">                                        
                                        <asp:ListItem Value="1" Selected="True">Non-ECIT Servers</asp:ListItem>
                                        <asp:ListItem Value="2">ECIT Servers</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td align="left" colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right" class="Label">
                                    Option :
                                </td>
                                <td style="color: Red;">
                                    *
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPatchingOption" Width="280px" Height="20px" TabIndex="7"
                                        runat="server" AutoPostBack="false" Font-Bold="False" Font-Size="12px">
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
                                    <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl="http://sharepoint/sites/ST/Shared%20Documents/Forms/AllItems.aspx?RootFolder=/sites/ST/Shared%20Documents/Standard%20Practices/Automation%20Folder/Automation%20OnBoarding/PowerPatch%204.0/Documents/Input%20Excel%20Templates">Input Template </asp:HyperLink>
                                </td>
                            </tr>
                            <tr id="trReboot" runat="server" style="display: none" clientidmode="Static">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right" class="Label">
                                    Reboot Option :
                                </td>
                                <td style="color: Red;">
                                    *
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlReboot" Width="280px" Height="20px" TabIndex="7" runat="server"
                                        AutoPostBack="false" Font-Bold="False" Font-Size="12px">
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
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right" class="Label">
                                    OnlyQFE :
                                </td>
                                <td style="color: Red;">
                                </td>
                                <td style="">
                                    <asp:TextBox ID="txtOnlyQFE" Width="277px" Style="" Height="20px" Text="" runat="server"></asp:TextBox>
                                </td>
                                <td align="left" colspan="2">
                                </td>
                            </tr>
                            <tr runat="server" style="display: none" clientidmode="Static" id="trExcludeQFE">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right" class="Label">
                                    ExcludeQFE :
                                </td>
                                <td style="color: Red;">
                                </td>
                                <td style="">
                                    <asp:TextBox ID="txtExcludeQFE" Width="277px" Style="" Height="20px" Text="" runat="server"></asp:TextBox>
                                </td>
                                <td align="left" colspan="2">
                                </td>
                            </tr>
                            <tr id="trODPOption" runat="server" style="display: none" clientidmode="Static">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right" class="Label">
                                    OnDemandPatch Option :
                                </td>
                                <td style="color: Red;">
                                    *
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlODPOption" Width="280px" Height="20px" runat="server" TabIndex="8"
                                        Font-Bold="False" Font-Size="12px">
                                        <asp:ListItem>Patch</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td align="left" colspan="2">
                                </td>
                            </tr>
                            <tr id="trSimpleUpdateOption" runat="server" clientidmode="Static">
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right" class="Label">
                                    SimpleUpdate Option :
                                </td>
                                <td style="color: Red;">*</td>
                                <td>
                                    <asp:DropDownList ID="ddlSimpleUpdateOption" Width="280px" Height="20px" runat="server" TabIndex="9" Font-Bold="False" Font-Size="12px" AutoPostBack="false">
                                        <asp:ListItem Text="Install To Success" Value="reboottosuccess"></asp:ListItem>
                                        <asp:ListItem Text="Reboot" Value="reboot"></asp:ListItem>
                                        <asp:ListItem Text="Install" Value="install"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width:350px;"></td>
                                <td align="left"></td>
                            </tr>
                            <tr>
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right" class="Label">
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
                                   <asp:DropDownList ID="ddlExecuteNames" runat="server" Width="280px" Height="20px" 
                                        TabIndex="6" Font-Bold="False" Font-Size="12px">
                                   <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                   </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <%--<label id="lblddl" style="font-family: Verdana; font-size: 12px; color: red">
                                    </label>--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right" class="Label">
                                    <label id="lblServerName" style="font-family: Verdana; font-weight: bold;">
                                        Server Names :
                                    </label>
                                 
                                  <%--  Server Names :--%>
                                </td>
                                <td style="color: Red;">
                                </td>
                                <td style="">
                                    <asp:TextBox ID="txtExecuteServerNames" runat="server" Width="277px"></asp:TextBox>
                                </td>
                                <td colspan="2" align="left">
                                    <label id="lblExecuteServerNames" style="font-family: Verdana; font-size: 12px; color: red">
                                    </label>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size: 12px; font-weight: bold; font-family: Verdana;" align="right" class="Label">
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
                                    <div class="loading" style="margin-left:430px;margin-top:20px;">
                                        Loading. Please wait.<br />
                                        <br />
                                        <img src="Images/loading.gif" alt="loading" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="5" align="center">
                                    <asp:Button ID="btnExecute" Width="80px" Height="25px" runat="server" TabIndex="11"
                                        CssClass="button" Text="Display" OnClick="btnExecute_Click" />
                                    <asp:Button ID="btnExportToExcel" Width="150px" Height="25px" runat="server"  Enabled="false" TabIndex="12" CssClass="button" Text="Export to Excel" OnClick="btnExportToExcel_Click" />
                                </td>
                            </tr>
                        </table>                      
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
                                  <asp:Button ID="btnDisplayRefresh" runat="server" Text="Refresh" OnClick="ExecuteTimer_Tick" ></asp:Button>
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
                                 <asp:TemplateField HeaderText="Patch Tool">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatchTool" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchTool")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Extension">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExtension" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "IsExtensionServer")%>'  ForeColor='<%# GetExtensionColor(Eval("IsExtensionServer").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Patch Result (T/S/F/R)">
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
                                 <asp:TemplateField HeaderText="Server Online">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServerOnline" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ServerOnline")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Server UpTime DD:HH:MM">
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
                <div id="content_4" class="tab_content" runat="server" style="display: none;">
                    <table>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-top: 20px;
                                margin-right: 10px; width: 340px;" align="right" class="Label">
                                Path :
                            </td>
                            <td style="color: Red; padding-top: 20px;">
                                *
                            </td>
                            <td style="padding-top: 20px; width: 400px">
                                <asp:FileUpload ID="fupValidateExcel" runat="server" Width="250px" onchange="EnableValidate()" />
                                <label id="lblValidateExcel" style="font-family: Verdana; font-size: 12px; color: red">
                                </label>
                            </td>
                            <td style="padding-top: 20px; padding-left: 20px" align="right">
                                <asp:HyperLink ID="hlValidate" runat="server" Target="_blank" NavigateUrl="http://sharepoint/sites/ST/Shared%20Documents/Forms/AllItems.aspx?RootFolder=/sites/ST/Shared%20Documents/Standard%20Practices/Automation%20Folder/Automation%20OnBoarding/PowerPatch%204.0/Documents/Input%20Excel%20Templates">Input Template </asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-left: 60px; width: 340px;" align="right" class="Label">
                                Select GroupName :
                            </td>
                            <td style="color: Red;">&nbsp;</td>
                            <td>
                                <asp:DropDownList ID="ddlValidateNames" runat="server" Width="247px" Height="20px" TabIndex="6" Font-Bold="False" Font-Size="12px">
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
                                <asp:TextBox ID="txtValidateServerNames" runat="server" Width="244px"></asp:TextBox>
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
                        <%-- <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; margin-right: 10px;
                                width: 340px;" align="right">
                                Report Refresh Time (Minutes) :
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlValidateRefresh" Width="225px" Height="20px" runat="server"
                                    TabIndex="9" Font-Bold="False" Font-Size="12px">
                                    <asp:ListItem Text="5" Value="300000"></asp:ListItem>
                                    <asp:ListItem Text="2" Value="120000"></asp:ListItem>
                                    <asp:ListItem Text="1" Value="60000"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 250px; margin-left: 10px" align="left">
                                <label id="Label3" style="font-family: Verdana; font-size: 12px; color: red">
                                </label>
                            </td>
                        </tr>--%>
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
                                <asp:CheckBox ID="cbSimpleUpdate" runat="server" Text="" />
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
                                <asp:CheckBox ID="cbODP" runat="server" Text="" />
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
                                <asp:CheckBox ID="chkISERSCAN" runat="server" Text="" Enabled="false" />
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
                                <asp:CheckBox ID="cbNumbers" runat="server" Text="" />
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td style="margin: 5px; font-size: 12px; font-family: Verdana;" align="left">
                                <span style="font-weight: bold">KB Numbers
                                    <asp:TextBox ID="txtKbValue" runat="server" Width="400px" Enabled="false"></asp:TextBox>
                                </span>
                                <label id="lblTextValidate" style="font-family: Verdana; font-size: 12px; color: red">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; margin-right: 10px;
                                width: 340px;" align="right">
                                <asp:CheckBox ID="cbInstalledUpdates" runat="server" Text="" />
                            </td>
                            <td style="color: Red;">
                            </td>
                            <td style="margin: 9px; font-size: 12px; font-weight: bold; font-family: Verdana;"
                                align="left">
                                <span>Find Installed Updates &nbsp;&nbsp;From
                                    <asp:TextBox ID="txtStartDate" runat="server" Width="70px"></asp:TextBox>
                                    <img id="imgCal1" alt="StartDate" class="select1" src="Images/Calendar.gif" />&nbsp;To
                                    <asp:TextBox ID="txtEndDate" runat="server" Width="50px"></asp:TextBox>
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
                                <asp:Button ID="btnValidatePatch" Style="width: 80px; margin-left: 10px" TabIndex="10"
                                    Text="Validate" runat="server" OnClick="btnValidate_Click" Font-Underline="False"
                                    CssClass="button" />
                                <asp:Button ID="btnPostSmokeTest" Text="SmokeTest" Width="100px" Height="25px" runat="server"
                                    OnClick="btnPostSmokeTest_Click" />
                                <asp:Button ID="btnCompareTest" Text="CompareSmokeTestLog" OnClick="btnCompareTest_Click"
                                    Width="155px" Height="25px" runat="server" Visible="false" />
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" CellPadding="4"
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
                                <asp:TextBox ID="txtResult" runat="server" Visible="false" TextMode="MultiLine" Width="900px"
                                    Height="100px" Style="margin-right: 10px;"></asp:TextBox>
                                <asp:Panel ID="pnlValidateResult" runat="server" align="center" Style="margin-top: 20px;
                                    margin-left: 20px;" Height="400px" ScrollBars="Vertical" Visible="false">
                                    <asp:GridView ID="gvValidateResult" runat="server" AutoGenerateColumns="true" CellPadding="4"
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
                                    <asp:Timer ID="TimerValidate" OnTick="TimerValidate_Tick" runat="server" Interval="10000"
                                        Enabled="False">
                                    </asp:Timer>
                                    <asp:UpdatePanel ID="upValidate" UpdateMode="Conditional" runat="server">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="TimerValidate" EventName="Tick" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <rsweb:ReportViewer ID="rvValidate" runat="server" Visible="false" Width="950px">
                                                <ServerReport Timeout="600000000" />
                                            </rsweb:ReportViewer>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                    </table>
                   
                </div>
                <div id="content_5" class="tab_content" runat="server" style="display: none;">
                    <table border="0">
                        <tr>
                            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-top: 20px;
                                padding-left: 80px;" align="right">
                                StartDate :<span id="startdate" style="color: red;">*</span>
                            </td>
                            <td style="padding-top: 20px;">
                                <asp:TextBox ID="txtReportsStartDate" runat="server" Width="120px" Style="margin-left: 5px"></asp:TextBox>
                                <img id="imgReportsStartDate" alt="StartDate" class="selectR1" src="Images/Calendar.gif" />
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
                                <img id="imgReportsEndDate" alt="EndDate" class="selectR2" src="Images/Calendar.gif" />
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
                        <img src="Images/loading.gif" alt="loading" />
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
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
 <%--   <asp:HiddenField ID="hdnExecutionFlag" runat="server" Value="" />--%>
    <asp:HiddenField ID="hdnClusterExecute" runat="server" Value="" />
    <asp:HiddenField ID="hdnFileName" runat="server" Value="" />
    <asp:HiddenField ID="hdnExecute" runat="server" Value="" />
    <asp:HiddenField ID="hdnTimer" runat="server" Value="0" />
    <asp:HiddenField ID="hdnExcelPath" runat="server" Value="" />
    <asp:HiddenField ID="hdnCbValidate" runat="server" Value="" />
    <asp:HiddenField ID="hdnCbMSNPatch" runat="server" Value="" />
    <asp:HiddenField ID="hdnCbSimpleUpdate" runat="server" Value="" />
    <asp:HiddenField ID="hdnCbODP" runat="server" Value="" />
    <asp:HiddenField ID="hdnUniqueAccess" runat="server" Value="" />
    <asp:HiddenField ID="hdnServerNames" runat="server" Value="" />
    <asp:HiddenField ID="hdnAddAdmin" runat="server" Value="" />
    <asp:HiddenField ID="PageID" runat="server" />
    <asp:HiddenField ID="hdnCheck" runat="server" Value="" />
    <asp:HiddenField ID="hdnPrepFileName" runat="server" Value="" />
    <asp:HiddenField ID="hdnPrepInputFilename" runat="server" Value="" />
    <asp:HiddenField ID="hdnPlanInputFileName" runat="server" Value="" />
    <asp:HiddenField ID="hdnKBUniqueValidate" runat="server" Value="" />
    <asp:HiddenField ID="hdnUniqueFindUpdates" runat="server" Value="" />
    <asp:HiddenField ID="hdnExecuteFileName" runat="server" Value="" />     
    <asp:HiddenField ID="hdnResumeFlag" runat="server" Value="0" />
     <asp:HiddenField ID="hdnNodeCount" runat="server" Value="0" />
      <asp:HiddenField ID="hdnBackUpNode" runat="server" Value="0" />
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
    </form>
    <br />
    <div id="footer" style="margin-bottom: 0.1in; margin-top: -0.35in; text-align: center">
        <asp:Label ID="VersionNumber" runat="server" Text="9.0.0.0" ForeColor="#333333"></asp:Label>
    </div>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
    function Shown(sender, args) {
        // var CalendarExt1 = $find('CalendarExtBehavior1'); // Note: CalendarExtBehavior1 is the BehaviorID I gave to the CalendarExtender for date 1
        var now = new Date();
     
        sender.set_visibleDate(now.format("MM/dd/yyyy HH:mm"));
    }
   function dateselect(sender,args)
   {   
        var d = sender._selectedDate;
        var now = new Date();      
        sender.get_element().value = d.format("MM/dd/yyyy") + " " + now.format("HH:mm")     
    }
    function ShowProgress() {

        setTimeout(function () {

            // var modal = $('<div />');

            // modal.addClass("modal");

            // $('body').append(modal);

            var loading = $(".loading");

            loading.show();

            //                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);

            //                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);

            //                loading.css({ top: top, left: left });                

        }, 200);

    }

    //        $('form').live("submit", function () {

    //            ShowProgress();

    //        });

    function ShowExecuteProgress() {
        setTimeout(function () {
            var loading = $(".Executeloading");

            loading.show();
        }, 200);

    }
    </script>
     <script  type="text/javascript" src="Scripts/datepicker.js"></script>  
        <script  type="text/javascript" src="Scripts/jquery.datetimepicker.js"></script>  

<script type="text/javascript">
    $('.datetime').datetimepicker();
    var logic = function (currentDateTime) {
        if (currentDateTime.getDay() == 6) {
            this.setOptions({
                minTime: '11:00'
            });
        } else
            this.setOptions({
                minTime: '8:00'
            });
    };

</script>

  
    <!--PrepTab-->
     <script type="text/javascript">
         //Prep Tab

         $('#txtServerNames').change(function () {
             $('#btnCheck').removeAttr("disabled");
         });
         $("#ddlPrepNames").change(function (e) {
             $('#btnCheck').removeAttr("disabled");
         });
         $('#txtServerNames').keypress(function (event) {
             var Upload_file = document.getElementById('<%= fupPreExceute.ClientID %>');
             var myfile = Upload_file.value;
             var ddlPrepNames = document.getElementById('<%= ddlPrepNames.ClientID %>');
             if ($('#ddlPrepNames').find('option:selected').text() != 'Select') {
                 $('#txtServerNames').attr("disabled", "disabled");
                 lblServernames.innerHTML = "Only One Option Avialble";
             }            
            else if (myfile != '') {
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
             var Upload_file = document.getElementById('<%= fupPreExceute.ClientID %>');
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

                 if ($('#ddlPrepNames').find('option:selected').text() == 'Select') {
                     if ($('#txtServerNames').val() == '') {
                         flag1 = 1;
                         lblServernames.innerHTML = 'Please Select Excel File or GroupName or Enter ServerNames ';
                     }
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

         $("#btnUnistall").click(function () {


             var flag1 = 0;
             lblServers.innerHTML = "";
             var Upload_file = document.getElementById('<%= fupPreExceute.ClientID %>');


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

                 if ($('#ddlPrepNames').find('option:selected').text() == 'Select') {

                     if ($('#txtServerNames').val() == '') {
                         flag1 = 1;
                         lblServernames.innerHTML = 'Please Select Excel File or GroupName or Enter ServerNames ';
                     }
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
             var Upload_file = document.getElementById('<%= fupPreExceute.ClientID %>');
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

                 if ($('#ddlPrepNames').find('option:selected').text() == 'Select') {
                     if ($('#txtServerNames').val() == '') {
                         flag1 = 1;
                         lblServernames.innerHTML = 'Please Select Excel File or GroupName or Enter ServerNames ';
                     }
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
             //  $('#btnAddAdmin').Attr("disabled");

             $('#btnAddAdmin').attr("disabled", "disabled");


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

            //  LoadOptions();


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


            $(".select1").click(function () {
                // alert('startDate');
                var txtStartDate = document.getElementById("txtStartDate");
                $("#<%=txtStartDate.ClientID%>").datepicker({ dateFormat: 'mm-dd-yy' });
                txtStartDate.focus();


            });

            $(".select2").click(function () {
                //alert('enddate');


                var txtEndDate = document.getElementById("txtEndDate");
                $("#<%=txtEndDate.ClientID%>").datepicker({ dateFormat: 'mm-dd-yy' });
                txtEndDate.focus();
            });
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
//            if ($('#txtDomainAccName').val() == '') {

//                lblAdminAccountName.innerHTML = 'Please Enter Domain Account Name';
//                flag = 1;

//            }
//            if ($('#txtDomainAcctPwd').val() == '') {

//                lblAdminAcctPwd.innerHTML = 'Please Enter Domain Account Pwd';
//                flag = 1;

//            }
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
                ShowProgress();
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

    <!-- Validate Tab-->
    <script type="text/javascript">

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
    <!---Reports Tab-->
    <script type="text/javascript">
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
</body>
</html>


