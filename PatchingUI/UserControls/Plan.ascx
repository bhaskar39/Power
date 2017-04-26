<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Plan.ascx.cs" Inherits="PatchingUI.UserControls.Plan" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />
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
            <asp:FileUpload ID="fupPlanExcel" runat="server" Width="280px" />
        </td>
        <td style="padding-top: 20px;">
            <asp:Button ID="btnImport" Style="width: 100px;" Text="Import" CssClass="button"
                ClientIDMode="Static" OnClick="btnImport_Click" runat="server" />
        </td>
        <td style="padding-top: 20px; padding-left: 20px; width: 270px;" align="right">
            <asp:HyperLink ID="hlPlan" runat="server" Target="_blank" NavigateUrl="http://sharepoint/sites/ST/Shared%20Documents/Forms/AllItems.aspx?RootFolder=/sites/ST/Shared%20Documents/Standard%20Practices/Automation%20Folder/Automation%20OnBoarding/PowerPatch%204.0/Documents/Input%20Excel%20Templates">Input Template </asp:HyperLink>
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
                        OnSelectedIndexChanged="ddlOrgName_SelectedIndexChanged">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td align="right">
                    SubBPU:&nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlSubBPU" runat="server" Font-Size="10px" Width="150px" AutoPostBack="false">
                    </asp:DropDownList>
                </td>
                <td align="right">
                    Application Name:&nbsp;
                </td>
                <td align="right">
                    <asp:DropDownList ID="ddlApplication" runat="server" Font-Size="10px" Width="150px"
                        AutoPostBack="false">
                        <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td align="right">
                    Environment:&nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlEnv" runat="server" Font-Size="10px" Width="150px" AutoPostBack="false">
                        <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="display: none">
                    UniqueID
                </td>
                <td style="display: none">
                    <asp:TextBox ID="txtUniqueID" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    PatchScenarios:&nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlPatchScenario" runat="server" Font-Size="10px" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlPatchScenario_SelectedIndexChange">
                        <asp:ListItem Text="Standalone" Value="1"></asp:ListItem>
                        <asp:ListItem Text="MSCS" Value="2"></asp:ListItem>
                        <asp:ListItem Text="HLB" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td align="right">
                    Mode:&nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlMode" runat="server" Font-Size="10px" Width="150px" AutoPostBack="false">
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
                        AutoPostBack="false">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td align="right">
                    Patch Month:&nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlPatchMonth" runat="server" Font-Size="10px" Width="150px"
                        AutoPostBack="false">
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
                        AutoPostBack="false">
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
                    <asp:DropDownList ID="ddlGroupName" runat="server" Font-Size="10px" Width="150px">
                        <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="padding-left: 20px;">
                    <asp:Button ID="btnGetData" Style="width: 100px;" Text="View Data" CssClass="button"
                        ClientIDMode="Static" OnClick="btnGetData_Click" runat="server"/>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <%-- <div class="loading" style="margin-left:430px;margin-top:20px;">
                        Loading. Please wait.<br />
                        <br />
                        <img src="Images/loading.gif" alt="loading" />
                             </div>--%>
    <table border="0" width="92%">
        <tr id="trSave" runat="server" visible="false">
            <td style="width:60%" align="left">
                <span id="trGroup" style="display: none">
                    <asp:TextBox ID="txtGroupName" runat="server" Width="274px" MaxLength="15"></asp:TextBox>
                    <asp:Button ID="btnSaveGroup" Style="width: 100px;" Text="Save Group" CssClass="button" ClientIDMode="Static" OnClick="btnSaveGroup_Click" runat="server" />
                </span>
            </td>
            <td align="right">
                <asp:CheckBox ID="ChkDefer" runat="server" Text="Defer"/>&nbsp;
                <ajaxToolkit:NumericUpDownExtender ID="nupDefer" runat="server" Width="50" Minimum="-12" Maximum="12" Step="1" TargetControlID="txtDefer"></ajaxToolkit:NumericUpDownExtender>
                <asp:TextBox ID="txtDefer" runat="server" Width="10px" Text="0"></asp:TextBox>&nbsp;
                <asp:Button ID="btnSave" Style="width: 120px;" Text="Save Schedule" CssClass="button" OnClick="btnSaveSchedule_Click" ClientIDMode="Static" runat="server" />
            </td>
        </tr>
<%--        <tr id="trGroup" runat="server" clientidmode="Static" style="display: none">
            <td>
                <asp:TextBox ID="txtGroupName" runat="server" Width="274px" MaxLength="15"></asp:TextBox>
                <asp:Button ID="btnSaveGroup" Style="width: 100px;" Text="Save Group" CssClass="button"
                    ClientIDMode="Static" OnClick="btnSaveGroup_Click" runat="server" />
            </td>
        </tr>--%>
    </table>
    <asp:HiddenField ID="hdnOverride" runat="server" />
    <asp:Label ID="lblGroupMsg" runat="server" ForeColor="Red"></asp:Label>
    <asp:Label ID="lblScheduleMsg" runat="server" ForeColor="Red"></asp:Label>
    <br />
    <%--    <div style="margin-top:20px;" align="center">--%>
   
     <%--<INPUT TYPE="NUMBER" MIN="0" MAX="10" STEP="2" VALUE="6" SIZE="6" runat="server">--%>
     
    <%--<table>
        <tr runat="server" visible="false" id="trDefer">
           
        </tr>
    </table>--%>
    <asp:Panel ID="pnlResults" runat="server" ScrollBars="Auto" Width="1100px">
        <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="false" CellPadding="2"
            CssClass="gridView" ForeColor="#333333" GridLines="None" Font-Size="10px" OnRowDataBound="gvResults_OnRowDataBound"
            AllowPaging="True" OnPageIndexChanging="gvResults_PageIndexChanging" OnSelectedIndexChanged="gvResults_SelectedIndexChanged"
            PageSize="25">
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
                        <asp:CheckBox ID="ChkGroupAll" runat="server" Text="Group" />
                    </HeaderTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:CheckBox ID="chkGroup" runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ServerName">
                    <ItemTemplate>
                        <asp:Label ID="lblServerName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ServerName")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PatchWindow">
                    <ItemTemplate>
                        <asp:Label ID="lblScheduledWindow" runat="server" ClientIDMode="Static" Width="120px"
                            Text='<%# DataBinder.Eval(Container.DataItem, "ScheduledWindow")%>'></asp:Label>
                        <asp:HiddenField ID="hdnSchedule" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "ScheduledWindow")%>' />
                        </label>
                    </ItemTemplate>
                    <ItemStyle Width="120px" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Env">
                    <ItemTemplate>
                        <asp:Label ID="lblEnv" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Env")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Org1">
                    <ItemTemplate>
                        <asp:Label ID="lblOrg1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Org1")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Org2">
                    <ItemTemplate>
                        <asp:Label ID="lblOrg2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Org2")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Org3">
                    <ItemTemplate>
                        <asp:Label ID="lblOrg3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Org3")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Org4">
                    <ItemTemplate>
                        <asp:Label ID="lblOrg4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Org4")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Applcations">
                    <ItemTemplate>
                        <asp:Label ID="lblApplcations" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Applications")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PatchWeek">
                    <ItemTemplate>
                        <asp:Label ID="lblPatchWeek" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchWeek")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PatchMonth">
                    <ItemTemplate>
                        <%-- '<%# Eval("DOB","{0:d}") %>'--%>
                        <%--<asp:Label ID="lblPatchMonth" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchMonth")%>'></asp:Label>--%>
                        <asp:Label ID="lblPatchMonth" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchMonth")%>'></asp:Label>
                        <asp:HiddenField ID="hdnMonth" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "PatchMonthValue")%>' />
                        <%-- <asp:Label ID="lblDate" runat="server" Text='<%# Eval("PatchMonth","{0:dd/MM/YYYY}") %>' />--%>
                        <%-- Text='<%# Bind("Failure_date","{0:dd/MM/YYYY}") %>'--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PatchingScenario">
                    <ItemTemplate>
                        <asp:Label ID="lblPatchingScenario" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchingScenario")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Schedule">
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkScheduleAll" runat="server" Text="Schedule" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkScheduled" runat="server" Checked='<%# GetCheckedValue(Eval("isscheduled").ToString()) %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="StartDate">
                  <ItemStyle Wrap="false"/>
                    <ItemTemplate>
                        
                        <asp:TextBox ID="txtScheduleStartDate"  runat="server" 
                            Font-Size="10px" Text='<%# Eval("startdatetime","{0:MM/dd/yyyy HH:mm}") %>'></asp:TextBox>
                                            <asp:ImageButton ID="StartImageButton" runat="server"
                                                AlternateText="Click to show calendar"
                                                ImageUrl="~/Images/calendar.gif" />
                                          <ajaxToolkit:CalendarExtender ID="ceStartDate" runat="server" TargetControlID="txtScheduleStartDate" 
                                                PopupButtonID="StartImageButton" Format="MM'/'dd'/'yyyy HH:mm"  OnClientDateSelectionChanged="dateselect">
                                            </ajaxToolkit:CalendarExtender>

   <%--<asp:Calendar ID="Calendar1" runat="server" BackColor="#FF6600" BorderColor="#336600" BorderStyle="Solid" BorderWidth="2px" Caption="Select date and month" 
   DayNameFormat="Full" OnSelectionChanged="Calendar1_SelectionChanged" ShowGridLines="True" Width="50px"></asp:Calendar>--%>

                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="EndDate">
                <ItemStyle Wrap="false"/>
                    <ItemTemplate>
                        <asp:TextBox ID="txtScheduleEndDate" runat="server" 
                            Font-Size="10px" Text='<%# Eval("enddatetime", "{0:MM/dd/yyyy HH:mm}")%>'></asp:TextBox>
                            <asp:ImageButton ID="EndImageButton" runat="server"
                                                AlternateText="Click to show calendar"
                                                ImageUrl="~/Images/calendar.gif" />
                                          <ajaxToolkit:CalendarExtender ID="ceEndDate" runat="server" TargetControlID="txtScheduleEndDate" 
                                                PopupButtonID="EndImageButton" Format="MM'/'dd'/'yyyy HH:mm"  OnClientDateSelectionChanged="dateselect" >
                                            </ajaxToolkit:CalendarExtender>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UniqueID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblUniqueId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ID")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>

    <table border="0" width="92%">
        <tr id="trSaveBottom" runat="server" visible="false">
            <td align="right">
                <asp:Button ID="btnSaveBottom" Style="width: 120px;" Text="Save Schedule" CssClass="button" OnClick="btnSaveSchedule_Click" ClientIDMode="Static" runat="server" />
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
<script type="text/javascript">
//    function dateselect(ev) {alert(ev);
//        var calendarBehavior1 = $find("CalendarExtender1");
//        var d = calendarBehavior1._selectedDate;
//        var now = new Date();
//        alert(d);
//        calendarBehavior1.get_element().value = d.format("yyyy/MM/dd") + " " + now.format("HH:mm:ss")
//    }
    //    $("#txtScheduleStartDate").blur(function () {

    //        var CurrentDate = new Date();
    //        var StartDate = new Date($('[id$=txtScheduleStartDate]').val());
    //        var EndDate = new Date($('[id$=txtScheduleEndDate]').val());
    //        if (CurrentDate >= StartDate) {
    //            alert("StartDate Must Be Greater Than Current Date");
    //            $("#txtScheduleStartDate").focus();            
    //        }
    //        if (StartDate >= EndDate) {
    //            alert("EndDate Must Be Greater Than Start Date");
    //            $("#txtScheduleEndDate").focus();
    //        }
    //      });
    //    $("#txtScheduleEndDate").blur(function () {

    //        var CurrentDate = new Date();
    //        var StartDate = new Date($('[id$=txtScheduleStartDate]').val());
    //        var EndDate = new Date($('[id$=txtScheduleEndDate]').val());
    //        if (CurrentDate >= StartDate) {
    //            alert("StartDate Must Be Greater Than Current Date");
    //            $("#txtScheduleStartDate").focus();          
    //        }
    //        if (StartDate >= EndDate) {
    //            alert("EndDate Must Be Greater Than Start Date");
    //            $("#txtScheduleEndDate").focus();
    //        }


    //    });
    function ShowProgress() {

        setTimeout(function () {

            var loading = $(".loading");

            loading.show();

        }, 200);

    }
</script>
<script type="text/javascript">


    $(document).ready(function () {
        var totalCheckboxes = $("#<%=gvResults.ClientID%> input[id*='chkScheduled']:checkbox").size();
        var checkedCheckboxes = $("#<%=gvResults.ClientID%> input[id*='chkScheduled']:checkbox:checked").size();
        var chkCheck = false;
        if (totalCheckboxes == checkedCheckboxes && totalCheckboxes != 0)
            chkCheck = true;

        $("#<%=gvResults.ClientID%> input[id*='chkScheduleAll']:checkbox").attr('checked', chkCheck);

//        var count = 0;
//        $(".gridView tr").each(function (e) {
//            var checkBox = $(this).find("input[id*='chkScheduled']:checkbox");
//            if (checkBox.is(':checked')) {
//                count = 1;
//            }
//        });

//        if (count == 1) {
//            $("#trSave").show();
//            $("#trSaveBottom").show();
//        }
//        else {
//            $("#trSave").hide();
//            $("#trSaveBottom").hide();
//        }

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
//            if (checkedCheckboxes > 0) {
//                $("#trSave").show();
//                $("#trSaveBottom").show();

//            }
//            else {
//                $("#trSave").hide();
//                $("#trSaveBottom").hide();
//            }

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
//                        $("#trSave").show();
//                        $("#trSaveBottom").show();
                    }
                }
                else {
                        $(this).attr("checked", false);
                        $(this).closest("tr").find("input[type=text][id*=txtScheduleStartDate]").attr("disabled", true);
                        $(this).closest("tr").find("input[type=text][id*=txtScheduleEndDate]").attr("disabled", true);
                        $(this).closest("tr").find("input[type=text][id*=txtScheduleStartDate]").val("");
                        $(this).closest("tr").find("input[type=text][id*=txtScheduleEndDate]").val("");
//                    $("#trSave").hide();
//                    $("#trSaveBottom").hide();
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

    $("#btnSave").click(function () {
        $("#<%= lblGroupMsg.ClientID %>").text("");
        $("#<%= lblScheduleMsg.ClientID %>").text("");


        var count = 0;
        $(".gridView tr").each(function (e) {
            var checkBox = $(this).find("input[id*='chkScheduled']:checkbox");
            var textBox = $(this).find("input[id*='txtScheduleStartDate']");
            var txtEndDate = $(this).find("input[id*='txtScheduleEndDate']");
            var lblScheduledWindow = $(this).find("span[id*='lblScheduledWindow']");
            // alert(lblScheduledWindow.text());


            if (checkBox.is(':checked')) {
                if ((textBox.val().length === 0) && (lblScheduledWindow.text() == '')) {

                    count = 1;
                }
                if ((txtEndDate.val().length === 0) && (lblScheduledWindow.text() == '')) {
                    count = 1;
                }
                var CurrentDate = new Date(document.getElementById('<%= hdnDate.ClientID %>').value);
             //   alert(CurrentDate);
              //  var CurrentDate1 = new Date();
                //alert(CurrentDate1);
                var StartDate = new Date(textBox.val());
                var EndDate = new Date(txtEndDate.val());
                //alert(StartDate);
                //alert(EndDate);
                if (CurrentDate >= StartDate) {
                    count = 2;
                    // textBox.focus();

                }
                if (StartDate > EndDate) {
                    count = 3;
                    //  txtEndDate.focus();
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

    });


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
