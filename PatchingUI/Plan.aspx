<%@ Page Language="C#" Title="Home Page" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="Plan.aspx.cs" Inherits="PatchingUI.Plan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<%@ OutputCache CacheProfile="AppCache1" VaryByParam="*" %>--%>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table>
        <tr>
            <td style="font-size: 12px; font-weight: bold; font-family: Verdana; padding-left: 60px;
                padding-top: 20px;" align="right" class="Label">
                Patch Plan Excel Path :
            </td>
            <td style="color: Red; padding-top: 20px; width: 10px;">
                *
            </td>
            <td style="padding-top: 20px; width: 80px;">
                <asp:FileUpload ID="fupPlanExcel" runat="server" Width="280px" ClientIDMode="Static" />
            </td>
            <td style="padding-top: 20px;">
                <asp:Button ID="btnImport" Style="width: 100px;" Text="Import" CssClass="button"
                    ClientIDMode="Static" OnClick="btnImport_Click" runat="server" />
            </td>
            <td style="padding-top: 20px; padding-left: 20px; width: 270px;" align="right">
                <asp:HyperLink ID="hlPlan" ClientIDMode="Static" runat="server" Target="_blank" NavigateUrl="http://sharepoint/sites/ST/Shared%20Documents/Forms/AllItems.aspx?RootFolder=/sites/ST/Shared%20Documents/Standard%20Practices/Automation%20Folder/Automation%20OnBoarding/PowerPatch%204.0/Documents/Input%20Excel%20Templates">Input Template </asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center" style="">
                <label id="lblPlanServers" style="font-family: Verdana; font-size: 12px; color: red">
                </label>
                <div class="loading" style="margin-left: 420px;">
                    Loading. Please wait.<br />
                    <br />
                    <img src="Images/loading.gif" alt="loading" />
                </div>
            </td>
        </tr>
    </table>
    <div align="center" style="margin-top: 20px; margin-left: 8px;">
        <asp:HiddenField ID="hdnUniqueID" runat="server" Value="" />
        <%--<rsweb:ReportViewer ID="rvPlan" runat="server" Visible="true" Width="100%">
                            <ServerReport Timeout="600000000" />   
                        </rsweb:ReportViewer>--%>
        <asp:Panel ID="PnlPlan" runat="server">
            <table border="0" cellpadding="3" cellspacing="0">
                <tr>
                    <td align="right">
                        Org Name:&nbsp;
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlOrgName" runat="server" Font-Size="10px" Width="150px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlOrgName_SelectedIndexChanged" ClientIDMode="Static">
                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        SubBPU:&nbsp;
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlSubBPU" runat="server" Font-Size="10px" Width="150px" AutoPostBack="false"
                            ClientIDMode="Static">
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        Application Name:&nbsp;
                    </td>
                    <td align="right">
                        <asp:DropDownList ID="ddlApplication" runat="server" Font-Size="10px" Width="150px"
                            ClientIDMode="Static" AutoPostBack="false">
                            <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        Environment:&nbsp;
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlEnv" runat="server" Font-Size="10px" Width="150px" AutoPostBack="false"
                            ClientIDMode="Static">
                            <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="display: none">
                        UniqueID
                    </td>
                    <td style="display: none">
                        <asp:TextBox ID="txtUniqueID" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        PatchScenarios:&nbsp;
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlPatchScenario" runat="server" Font-Size="10px" Width="150px"
                            ClientIDMode="Static">
                            <%--AutoPostBack="true" OnSelectedIndexChanged="ddlPatchScenario_SelectedIndexChange">--%>
                            <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Standalone" Value="1"></asp:ListItem>
                            <asp:ListItem Text="MSCS" Value="2"></asp:ListItem>
                            <asp:ListItem Text="HLB" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        Mode:&nbsp;
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlMode" runat="server" Font-Size="10px" Width="150px" AutoPostBack="false"
                            ClientIDMode="Static">
                            <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Manual" Value="Manual"></asp:ListItem>
                            <asp:ListItem Text="Auto" Value="Auto"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        Type Of Serverlist:&nbsp;
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlServerlistType" runat="server" Font-Size="10px" Width="150px"
                            ClientIDMode="Static" AutoPostBack="false">
                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        Patch Month:&nbsp;
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlPatchMonth" runat="server" Font-Size="10px" Width="150px"
                            ClientIDMode="Static" AutoPostBack="false">
                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Patch Week:&nbsp;
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlPatchWeek" runat="server" Font-Size="10px" Width="150px"
                            ClientIDMode="Static" AutoPostBack="false">
                            <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        Group Name:&nbsp;
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlGroupName" runat="server" Font-Size="10px" Width="150px"
                            ClientIDMode="Static">
                            <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 20px;">
                        <asp:Button ID="btnGetData" Style="width: 100px;" Text="View Data" CssClass="button"
                            ClientIDMode="Static" OnClick="btnGetData_Click" runat="server" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />      
    
      <!-- new design -->
   <table border="0" width="88%" cellpadding="0" cellspacing="0">
        <tr id="trSave" runat="server" visible="false">
            <td style="width:330px;" align="left">
                <table border="0" cellpadding="2" cellspacing="2">
                    <tr>
                        <td nowrap="nowrap" style="height:28px;">
                            <span id="trGroup" style="display: none">
                                <asp:TextBox ID="txtGroupName" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                <asp:Button ID="btnSaveGroup" Style="width: 100px;" Text="Save Group" CssClass="button" ClientIDMode="Static" OnClick="btnSaveGroup_Click" runat="server" />
                            </span>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td nowrap="nowrap">
                            <asp:Label runat="server" ID="lblPPModeText" Text="PP Mode: "></asp:Label>&nbsp;
                            <asp:DropDownList ID="PPMode" runat="server" ClientIDMode="Static" Visible="false">          
                                <asp:ListItem Text="Install To Reboot" Value="reboottosuccess" />
                                <asp:ListItem Text="Install" Value="install"/>
                                <asp:ListItem Text="Preview" Value="preview"/>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap="nowrap">
                            <asp:Label ID="lblServerCount" runat="server" Visible="false" Font-Bold="true"></asp:Label> 
                        </td> 
                    </tr>
                </table>
            </td>
            <td rowspan="4" style="width:auto;">
                <fieldset>
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td  colspan="2" align="right" valign="top">
                                <asp:RadioButtonList ID="rblDefer" runat="server" RepeatDirection="horizontal" ClientIDMode="Static" CellPadding="0" Height="10px" Width="500px">
                                    <asp:ListItem Text="Defer Schedule" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Defer by Hours" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td valign="top">
                                <asp:CheckBox ID="chkScheduleConfirmation" runat="server" Text="Schedule All" ClientIDMode="Static" />                        
                            </td>
                        </tr>
                        <tr>
                            <td style="width:420px;" valign="top">
                                <asp:Label ID="startLabel" runat="server" Text="Start Date"></asp:Label>
                                <asp:TextBox ID="txtDeferStartDate" runat="server" Font-Size="10px" Text=""></asp:TextBox>
                                <asp:ImageButton ID="StartDeferButton" runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/calendar.gif" />
                                <ajaxToolkit:CalendarExtender ID="ceDeferStartDate" runat="server" TargetControlID="txtDeferStartDate"
                                    PopupButtonID="StartDeferButton" Format="MM'/'dd'/'yyyy HH:mm" OnClientDateSelectionChanged="dateselect">
                                </ajaxToolkit:CalendarExtender>                                                   
 			                    &nbsp;&nbsp;
                                 <asp:Label ID="EndLabel" runat="server" Text="End Date"></asp:Label>
                                <asp:TextBox ID="txtDeferEndDate" runat="server" Font-Size="10px" Text=""></asp:TextBox>
                                <asp:ImageButton ID="EndDeferButton" runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/calendar.gif" />
                                <ajaxToolkit:CalendarExtender ID="ceDeferEndDate" runat="server" TargetControlID="txtDeferEndDate"
                                    PopupButtonID="EndDeferButton" Format="MM'/'dd'/'yyyy HH:mm" OnClientDateSelectionChanged="dateselect">
                                </ajaxToolkit:CalendarExtender>  
                            </td>
                            <td valign="top">
                            	    <asp:TextBox ID="txtdefer" runat="server" ClientIDMode="Static" 
                                   ToolTip="Please enter only numbers" MaxLength="5" onBlur="return validatenumber(this.value)"
                                    Width="30px"></asp:TextBox>
                            </td>
                            <td style="width:129px;" align="center">                                                  
                                <asp:Button ID="btnSave" Style="width: 120px;" Text="Save Schedule" CssClass="button" OnClick="btnSaveSchedule_Click" ClientIDMode="Static" runat="server"/>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
     <!--end of  new design -->

        <asp:HiddenField ID="hdnOverride" runat="server" />
        <asp:Label ID="lblGroupMsg" runat="server" ForeColor="Red"></asp:Label>
        <asp:Label ID="lblScheduleMsg" runat="server" ForeColor="Red"></asp:Label>
         <asp:Label ID="lbldefervalidate" runat="server" ForeColor="Red" />
        <br />
        <asp:Panel ID="pnlResults" runat="server" ScrollBars="Auto" Width="1100px" ClientIDMode="Static">
            <asp:GridView ID="gvResults" runat="server" ClientIDMode="Static" AutoGenerateColumns="false"
                CellPadding="2" CssClass="gridView" ForeColor="#333333" GridLines="None" Font-Size="10px"
                OnRowDataBound="gvResults_OnRowDataBound" AllowPaging="True" OnPageIndexChanging="gvResults_PageIndexChanging"
                OnSelectedIndexChanged="gvResults_SelectedIndexChanged" PageSize="25">
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
                    <asp:TemplateField HeaderText="Group">
                        <HeaderTemplate>
                            <asp:CheckBox ID="ChkGroupAll" runat="server" Text="Group" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkGroup" runat="server" ClientIDMode="Static" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ServerName">
                        <ItemTemplate>
                            <asp:Label ID="lblServerName" runat="server" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "ServerName")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PatchWindow">
                        <ItemTemplate>
                            <asp:Label ID="lblScheduledWindow" runat="server" ClientIDMode="Static" Width="120px"
                                Text='<%# Eval("StartDateTime") + " - " + Eval("EndDateTime")%>'></asp:Label>
                            <asp:HiddenField ID="hdnSchedule" runat="server" ClientIDMode="Static" Value='<%# Eval("StartDateTime") + " - " + Eval("EndDateTime")%>' />
                            </label>
                        </ItemTemplate>
                        <ItemStyle Width="120px" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Env">
                        <ItemTemplate>
                            <asp:Label ID="lblEnv" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Env")%>'
                                ClientIDMode="Static"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Org1">
                        <ItemTemplate>
                            <asp:Label ID="lblOrg1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Org1")%>'
                                ClientIDMode="Static"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Org2">
                        <ItemTemplate>
                            <asp:Label ID="lblOrg2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Org2")%>'
                                ClientIDMode="Static"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Org3">
                        <ItemTemplate>
                            <asp:Label ID="lblOrg3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Org3")%>'
                                ClientIDMode="Static"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Org4">
                        <ItemTemplate>
                            <asp:Label ID="lblOrg4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Org4")%>'
                                ClientIDMode="Static"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Applcations">
                        <ItemTemplate>
                            <asp:Label ID="lblApplcations" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Applications")%>'
                                ClientIDMode="Static"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PatchWeek">
                        <ItemTemplate>
                            <asp:Label ID="lblPatchWeek" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchWeek")%>'
                                ClientIDMode="Static"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PatchMonth">
                        <ItemTemplate>
                            <%-- '<%# Eval("DOB","{0:d}") %>'--%>
                            <%--<asp:Label ID="lblPatchMonth" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchMonth")%>'></asp:Label>--%>
                            <asp:Label ID="lblPatchMonth" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchMonth")%>'
                                ClientIDMode="Static"></asp:Label>
                            <asp:HiddenField ID="hdnMonth" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "PatchMonthValue")%>'
                                ClientIDMode="Static" />
                            <%-- <asp:Label ID="lblDate" runat="server" Text='<%# Eval("PatchMonth","{0:dd/MM/YYYY}") %>' />--%>
                            <%-- Text='<%# Bind("Failure_date","{0:dd/MM/YYYY}") %>'--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PatchingScenario">
                        <ItemTemplate>
                            <asp:Label ID="lblPatchingScenario" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchingScenario")%>'
                                ClientIDMode="Static"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Schedule">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkScheduleAll" runat="server" Text="Schedule" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkScheduled" runat="server" Checked='<%# GetCheckedValue(Eval("isscheduled").ToString()) %>'
                                ClientIDMode="Static" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="StartDate">
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>
                            <asp:TextBox ID="txtScheduleStartDate" runat="server" Font-Size="10px"
                             Text='<%# Eval("Deferstartdatetime","{0:MM/dd/yyyy HH:mm}") %>'
                                ClientIDMode="Static"></asp:TextBox>
                            <asp:ImageButton ID="StartImageButton" runat="server" AlternateText="Click to show calendar"
                               ImageUrl="~/Images/calendar.gif" ClientIDMode="Static" />
                            <ajaxToolkit:CalendarExtender ID="ceStartDate" runat="server" TargetControlID="txtScheduleStartDate"
                                PopupButtonID="StartImageButton" Format="MM'/'dd'/'yyyy HH:mm" OnClientDateSelectionChanged="dateselect" ClientIDMode="Static"> 
                            </ajaxToolkit:CalendarExtender>
                            <%--<asp:Calendar ID="Calendar1" runat="server" BackColor="#FF6600" BorderColor="#336600" BorderStyle="Solid" BorderWidth="2px" Caption="Select date and month" 
   DayNameFormat="Full" OnSelectionChanged="Calendar1_SelectionChanged" ShowGridLines="True" Width="50px"></asp:Calendar>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="EndDate">
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>
                            <asp:TextBox ID="txtScheduleEndDate" runat="server" Font-Size="10px" ClientIDMode="Static"
                                Text='<%# Eval("Deferenddatetime", "{0:MM/dd/yyyy HH:mm}")%>'></asp:TextBox>
                            <asp:ImageButton ID="EndImageButton" runat="server" AlternateText="Click to show calendar"
                                ImageUrl="~/Images/calendar.gif" ClientIDMode="Static" />
                            <ajaxToolkit:CalendarExtender ID="ceEndDate" runat="server" TargetControlID="txtScheduleEndDate"
                                PopupButtonID="EndImageButton" Format="MM'/'dd'/'yyyy HH:mm" OnClientDateSelectionChanged="dateselect" ClientIDMode="Static">
                            </ajaxToolkit:CalendarExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UniqueID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblUniqueId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                                ClientIDMode="Static"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:Panel>
        <table border="0" width="88%" cellpadding="0" cellspacing="0">
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr id="trSaveBottom" runat="server" visible="false">
            <td align="right">
                <asp:Button ID="btnSaveBottom" Style="width: 120px;"   Text="Save Schedule" CssClass="button" OnClick="btnSaveSchedule_Click" ClientIDMode="Static" runat="server" />
            </td>
        </tr>
    </table>
        <%-- <ajaxToolkit:NumericUpDownExtender ID="NUD1" runat="server"
    TargetControlID="TextBox1" 
    Width="100"
    RefValues="January;February;March;April"
    TargetButtonDownID="Button1"
    TargetButtonUpID="Button2"
    ServiceDownPath="WebService1.asmx"
    ServiceDownMethod="PrevValue"
    ServiceUpPath="WebService1.asmx"
    ServiceUpMethod="NextValue"
    Tag="1" />--%>
        <%--</div>--%>
    </div>
    <asp:HiddenField ID="hdnPlanInputFileName" runat="server" Value="" />
    <asp:HiddenField ID="hdnDate" runat="server" Value="" />
    <!---Plan Tab-->
    <script type="text/javascript" src="Scripts/jquery.datetimepicker.js"></script>
    <script src="Scripts/datepicker.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
   
    <script type="text/javascript">

      function dateselect(sender, args) {
        var d = sender._selectedDate;
        var now = new Date();
        sender.get_element().value = d.format("MM/dd/yyyy") + " " + now.format("HH:mm")
    }

     function Shown(sender, args) {
        // var CalendarExt1 = $find('CalendarExtBehavior1'); // Note: CalendarExtBehavior1 is the BehaviorID I gave to the CalendarExtender for date 1
        var now = new Date();
        sender.set_visibleDate(now.format("MM/dd/yyyy HH:mm"));
    }

    function ShowProgress() {

        setTimeout(function () {

            var loading = $(".loading");

            loading.show();

        }, 200);

    }


    //   flag Defer TextBox
function validatenumber(val) { 
var count=0;
   var regex= /^(\+|-)?[0-9]\d*(\.\d*)?$/; 
  if( !regex.exec(val) ) {
   //alert("Please enter real numbers only");
       count=1;      
       document.getElementById('<%=txtdefer.ClientID %>').value="";
       document.getElementById('<%=txtdefer.ClientID %>').focus();
      
     }  
 else {
       $("#<%= lbldefervalidate.ClientID %>").text("");
       return true;
      }    
        if (count == 1) {
            $("#<%= lbldefervalidate.ClientID %>").text("Please enter real numbers only");
            $("#<%= lbldefervalidate.ClientID %>").css("color", 'red');   
             return false;        
        }  
}
//  End of section flag Defer TextBox


    $(document).ready(function () {

        $('#rblDefer').change(function () {

            if ($('#rblDefer').find('input:checked').val() == "0") {
                $("#<%=txtDeferStartDate.ClientID%>").attr("disabled", false);
                $("#<%=txtDeferEndDate.ClientID%>").attr("disabled", false);             
                  $("#<%=txtdefer.ClientID%>").attr("disabled", "disabled");
            }
            else if ($('#rblDefer').find('input:checked').val() == "1") {

                $("#<%=txtDeferStartDate.ClientID%>").attr("disabled", "disabled");
                $("#<%=txtDeferEndDate.ClientID%>").attr("disabled", "disabled");
                 $("#<%=txtdefer.ClientID%>").attr("disabled", false);             
            }

            else {
            }
        });
        var totalCheckboxes = $("#<%=gvResults.ClientID%> input[id*='chkScheduled']:checkbox").size();
        var checkedCheckboxes = $("#<%=gvResults.ClientID%> input[id*='chkScheduled']:checkbox:checked").size();
        var chkCheck = false;
        if (totalCheckboxes == checkedCheckboxes && totalCheckboxes != 0)
            chkCheck = true;

        $("#<%=gvResults.ClientID%> input[id*='chkScheduleAll']:checkbox").attr('checked', chkCheck);

        $("input[type=checkbox][id*=chkScheduled]").click(function () {
            if (this.checked) {
                $(this).closest("tr").find("input[type=text][id*=txtScheduleStartDate]").attr("disabled", false);
                $(this).closest("tr").find("input[type=text][id*=txtScheduleEndDate]").attr("disabled", false);

            }
            else {
                $(this).closest("tr").find("input[type=text][id*=txtScheduleStartDate]").attr("disabled", true);
                $(this).closest("tr").find("input[type=text][id*=txtScheduleEndDate]").attr("disabled", true);
                $(this).closest("tr").find("input[type=text][id*=txtScheduleStartDate]").val("");
                $(this).closest("tr").find("input[type=text][id*=txtScheduleEndDate]").val("");
            }
        });

        $("#<%=gvResults.ClientID%> input[id*='chkScheduled']:checkbox").click(function () {
            var totalCheckboxes = $("#<%=gvResults.ClientID%> input[id*='chkScheduled']:checkbox").size();
            var checkedCheckboxes = $("#<%=gvResults.ClientID%> input[id*='chkScheduled']:checkbox:checked").size();
            $("#<%=gvResults.ClientID%> input[id*='chkScheduleAll']:checkbox").attr('checked', totalCheckboxes == checkedCheckboxes);

            if (this.checked) {
                var startdate = $(this).closest("tr").find("input[type=text][id*=txtScheduleStartDate]");
                var enddate = $(this).closest("tr").find("input[type=text][id*=txtScheduleEndDate]");
                var lblScheduledWindow = $(this).closest("tr").find("span[id*='lblScheduledWindow']");
                var hdnSchedule = $(this).closest("tr").find("input[type=hidden][id*=hdnSchedule]");                
                if (hdnSchedule.val().indexOf("-") != "-1") {
                    var result = hdnSchedule.val().split("-");
                    var startval = result[0].replace('PM', '').replace('AM', '').replace('PT', '');
                    var endval = result[1].replace('PM', '').replace('AM', '').replace('PT', '');
                    startdate.val(startval);
                    enddate.val(endval);
                }
            }
        });

        $("#<%=gvResults.ClientID%> input[id*='chkGroup']:checkbox").click(function () {
            var totalCheckboxes = $("#<%=gvResults.ClientID%> input[id*='chkGroup']:checkbox").size();
            var checkedCheckboxes = $("#<%=gvResults.ClientID%> input[id*='chkGroup']:checkbox:checked").size();
            $("#<%=gvResults.ClientID%> input[id*='ChkGroupAll']:checkbox").attr('checked', totalCheckboxes == checkedCheckboxes);
            if (checkedCheckboxes > 0)
                $("#trGroup").show();

            else
                $("#trGroup").hide();
        });



        $("[id*=chkScheduleAll]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[id*='chkScheduled']", grid).each(function () {
                var chk = $(this);

                if (chkHeader.is(":checked")) {
                    if (chk.is(":disabled")) {
                        $(this).attr("checked", false);
                    }
                    else {
                        $(this).attr("checked", true);
                        $(this).closest("tr").find("input[type=text][id*=txtScheduleStartDate]").attr("disabled", false);
                        $(this).closest("tr").find("input[type=text][id*=txtScheduleEndDate]").attr("disabled", false);


                        if (this.checked) {
                            var startdate = $(this).closest("tr").find("input[type=text][id*=txtScheduleStartDate]");
                            var enddate = $(this).closest("tr").find("input[type=text][id*=txtScheduleEndDate]");
                            var lblScheduledWindow = $(this).closest("tr").find("span[id*='lblScheduledWindow']");
                            var hdnSchedule = $(this).closest("tr").find("input[type=hidden][id*=hdnSchedule]");                           
                            if (hdnSchedule.val().indexOf("-") != "-1") {
                                var result = hdnSchedule.val().split("-");
                                var startval = result[0].replace('PM', '').replace('AM', '').replace('PT', '');
                                var endval = result[1].replace('PM', '').replace('AM', '').replace('PT', '');
                                startdate.val(startval);
                                enddate.val(endval);
                            }
                        }                     
                    }
                }
                else {
                    $(this).attr("checked", false);
                    $(this).closest("tr").find("input[type=text][id*=txtScheduleStartDate]").attr("disabled", true);
                    $(this).closest("tr").find("input[type=text][id*=txtScheduleEndDate]").attr("disabled", true);
                    $(this).closest("tr").find("input[type=text][id*=txtScheduleStartDate]").val("");
                    $(this).closest("tr").find("input[type=text][id*=txtScheduleEndDate]").val("");
                }
            });
        });

        $("[id*=ChkGroupAll]").live("click", function () {

            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[id*='chkGroup']", grid).each(function () {
                var chk = $(this);

                if (chkHeader.is(":checked")) {
                    if (chk.is(":disabled")) {

                        $(this).attr("checked", false);
                    }
                    else {
                        $(this).attr("checked", true);
                        $("#trGroup").show();
                    }

                }
                else {
                    $(this).attr("checked", false);
                    $("#trGroup").hide();
                }
            });
        });
    });

    $("#btnSaveGroup").click(function () {
        $("#<%= lblGroupMsg.ClientID %>").text("");
        $("#<%= lblScheduleMsg.ClientID %>").text("");
        ShowProgress();
    });


      <%-- --flag scheduleAll--%>  
        function SaveSchedule() {
        $("#<%= lblGroupMsg.ClientID %>").text("");
        $("#<%= lblScheduleMsg.ClientID %>").text("");

        var chk = document.getElementById("chkScheduleConfirmation");       
        if (chk != null && chk.checked != null) {
            if (chk.checked == true) {
                var r = confirm("Please review all the schedules before clicking ok");
                if (r == true) {
                    //return true;
                    var count = 0;
                    $(".gridView tr").each(function (e) {
                        var checkBox = $(this).find("input[id*='chkScheduled']:checkbox");
                        var textBox = $(this).find("input[id*='txtScheduleStartDate']");
                        var txtEndDate = $(this).find("input[id*='txtScheduleEndDate']");
                        var lblScheduledWindow = $(this).find("span[id*='lblScheduledWindow']");
                       
                       if (checkBox.is(':checked')) {

                if ($('#rblDefer').find('input:checked').val() == "0") {

                    var textBox = $("#<%=txtDeferStartDate.ClientID%>");
                    var txtEndDate = $("#<%=txtDeferEndDate.ClientID%>");
                    var CurrentDate = new Date(document.getElementById('<%= hdnDate.ClientID %>').value);
                    var StartDate = new Date(textBox.val());
                    var EndDate = new Date(txtEndDate.val());
                    if (textBox.val().length === 0) {

                        count = 1;
                    }
                    else if (txtEndDate.val().length === 0) {
                        count = 1;
                    }

                    else if (CurrentDate >= StartDate) {
                        count = 2;
                    }
                    else if (StartDate > EndDate) {
                        count = 3;
                    }
                    else {
                        count = 4;
                    }
                }

                else {
                    var CurrentDate = new Date(document.getElementById('<%= hdnDate.ClientID %>').value);
                    var StartDate = new Date(textBox.val());
                    var EndDate = new Date(txtEndDate.val());

                    if ((textBox.val().length === 0) && (lblScheduledWindow.text() == '')) {

                        count = 1;
                    }
                    else if ((txtEndDate.val().length === 0) && (lblScheduledWindow.text() == '')) {
                        count = 1;
                    }

                    else if (CurrentDate >= StartDate) {
                        count = 2;
                    }
                    else if (StartDate > EndDate) {
                        count = 3;
                    }
                    else {
                        count = 4;
                    }
                }

            }
    });

                    if (count == 1) {
                        $("#<%= lblScheduleMsg.ClientID %>").text("Please Select Start and End Dates for the Selected Scheduled Server");
                        $("#<%= lblScheduleMsg.ClientID %>").css("color", 'red');

                        return false;
                    }
                    if (count == 2) {
                        $("#<%= lblScheduleMsg.ClientID %>").text("Start Date Must Be Greater than Current Date");
                        $("#<%= lblScheduleMsg.ClientID %>").css("color", 'red');

                        return false;
                    }
                    if (count == 3) {
                        $("#<%= lblScheduleMsg.ClientID %>").text("End Date Must Be Greater than Start Date");
                        $("#<%= lblScheduleMsg.ClientID %>").css("color", 'red');

                        return false;
                    }
                    else {
                        ShowProgress();
                        return true;
                    }

                }
                else {
                    return false;
                }
            }
       else {     
       var count=0;
        $(".gridView tr").each(function (e) {
            var checkBox = $(this).find("input[id*='chkScheduled']:checkbox");
            var textBox = $(this).find("input[id*='txtScheduleStartDate']");
            var txtEndDate = $(this).find("input[id*='txtScheduleEndDate']");
            var lblScheduledWindow = $(this).find("span[id*='lblScheduledWindow']");
            if (checkBox.is(':checked')) {

                if ($('#rblDefer').find('input:checked').val() == "0") {

                    var textBox = $("#<%=txtDeferStartDate.ClientID%>");
                    var txtEndDate = $("#<%=txtDeferEndDate.ClientID%>");
                    var CurrentDate = new Date(document.getElementById('<%= hdnDate.ClientID %>').value);
                    var StartDate = new Date(textBox.val());
                    var EndDate = new Date(txtEndDate.val());
                    if (textBox.val().length === 0) {

                        count = 1;
                    }
                    else if (txtEndDate.val().length === 0) {
                        count = 1;
                    }

                    else if (CurrentDate >= StartDate) {
                        count = 2;
                    }
                    else if (StartDate > EndDate) {
                        count = 3;
                    }
                    else {
                        count = 4;
                    }
                }

                else {
                    var CurrentDate = new Date(document.getElementById('<%= hdnDate.ClientID %>').value);
                    var StartDate = new Date(textBox.val());
                    var EndDate = new Date(txtEndDate.val());

                    if ((textBox.val().length === 0) && (lblScheduledWindow.text() == '')) {

                        count = 1;
                    }
                    else if ((txtEndDate.val().length === 0) && (lblScheduledWindow.text() == '')) {
                        count = 1;
                    }

                    else if (CurrentDate >= StartDate) {
                        count = 2;
                    }
                    else if (StartDate > EndDate) {
                        count = 3;
                    }
                    else {
                        count = 4;
                    }
                }

            }
        });


        if (count == 0) {
            $("#<%= lblScheduleMsg.ClientID %>").text("Please Select at least one Server");
            $("#<%= lblScheduleMsg.ClientID %>").css("color", 'red');

            return false;
        }

        if (count == 1) {
            $("#<%= lblScheduleMsg.ClientID %>").text("Please Select Start and End Dates");
            $("#<%= lblScheduleMsg.ClientID %>").css("color", 'red');

            return false;
        }
        if (count == 2) {
            $("#<%= lblScheduleMsg.ClientID %>").text("Start Date Must Be Greater than Current Date");
            $("#<%= lblScheduleMsg.ClientID %>").css("color", 'red');

            return false;
        }
        if (count == 3) {
            $("#<%= lblScheduleMsg.ClientID %>").text("End Date Must Be Greater than Start Date");
            $("#<%= lblScheduleMsg.ClientID %>").css("color", 'red');

            return false;
        }
        else {
            ShowProgress();
            return true;
        }
      }

}
        else {
            return false;
        }
}
      
    $("#btnSave").click(function () {
        return SaveSchedule();
    });
    $("#btnSaveBottom").click(function () {
        return SaveSchedule();
    });       
  <%-- end of flag scheduleAll--%>  


    $("#btnGetData").click(function () {
        ShowProgress();
        return true;
    });

    $("#btnImport").click(function () {

        var i = 0;
        var flag = 0;
        lblPlanServers.innerHTML = "";
        var Upload_file = document.getElementById('<%= fupPlanExcel.ClientID %>');
        var myfile = Upload_file.value;

        var path = myfile;
        var pos = path.lastIndexOf(path.charAt(path.indexOf(":") + 1));
        var filename = path.substring(pos + 1);
        var RegEx = /^([a-zA-Z0-9_\.])+$/;


        if (myfile.indexOf("xlsx") > 0) {

            if (!(filename.match(RegEx))) {
                i = 1;
                flag = 1;
                lblPlanServers.innerHTML = 'Filename should not contain spaces and special characters ';

            }

        }
        else if (myfile.indexOf("xls") > 0) {
            if (!(filename.match(RegEx))) {
                i = 1;
                flag = 1;
                lblPlanServers.innerHTML = 'Filename should not contain spaces and special characters ';

            }
        }
        else {
            i = 1;
            flag = 1;
            lblPlanServers.innerHTML = 'Please Select Excel File ';

        }


        if (flag == 1) {
            return false;
        }
        else {
            ShowProgress();
            return true;
        }

    });

    //End of Plan Tab

    </script>
</asp:Content>
