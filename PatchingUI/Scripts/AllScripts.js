//General
//function dateselect(sender, args) {
//    var d = sender._selectedDate;
//    var now = new Date();
//    sender.get_element().value = d.format("MM/dd/yyyy") + " " + now.format("HH:mm")
//}

//function ShowProgress() {

//    setTimeout(function () {
//        var loading = $(".loading");

//        loading.show();

//    }, 200);

//}

//End of General

///* Plan tab */



//$("#btnSaveGroup").click(function () {
//    $("#<%= lblGroupMsg.ClientID %>").text("");
//    $("#<%= lblScheduleMsg.ClientID %>").text("");
//});

//$("#btnSave").click(function () {
//    $("#<%= lblGroupMsg.ClientID %>").text("");
//    $("#<%= lblScheduleMsg.ClientID %>").text("");


//    var count = 0;
//    $(".gridView tr").each(function (e) {
//        var checkBox = $(this).find("input[id*='chkScheduled']:checkbox");
//        var textBox = $(this).find("input[id*='txtScheduleStartDate']");
//        var txtEndDate = $(this).find("input[id*='txtScheduleEndDate']");
//        var lblScheduledWindow = $(this).find("span[id*='lblScheduledWindow']");
//        // alert(lblScheduledWindow.text());


//        if (checkBox.is(':checked')) {
//            if ((textBox.val().length === 0) && (lblScheduledWindow.text() == '')) {

//                count = 1;
//            }
//            if ((txtEndDate.val().length === 0) && (lblScheduledWindow.text() == '')) {
//                count = 1;
//            }
//            //                if (lblScheduledWindow.text() == '') {
//            //                    count = 1;
//            //                }
//        }
//    });

//    if (count == 1) {
//        $("#<%= lblScheduleMsg.ClientID %>").text("Please Select Start and End Dates for the Selected Scheduled Server");
//        $("#<%= lblScheduleMsg.ClientID %>").css("color", 'red');

//        return false;
//    }
//    else {
//        return true;
//    }

//});

//$("#btnImport").click(function () {

//    var i = 0;
//    var flag = 0;
//    lblPlanServers.innerHTML = "";
//    var Upload_file = document.getElementById('<%= fupPlanExcel.ClientID %>');
//    var myfile = Upload_file.value;

//    var path = myfile;
//    var pos = path.lastIndexOf(path.charAt(path.indexOf(":") + 1));
//    var filename = path.substring(pos + 1);
//    var RegEx = /^([a-zA-Z0-9_\.])+$/;


//    if (myfile.indexOf("xlsx") > 0) {

//        if (!(filename.match(RegEx))) {
//            i = 1;
//            flag = 1;
//            lblPlanServers.innerHTML = 'Filename should not contain spaces and special characters ';

//        }

//    }
//    else if (myfile.indexOf("xls") > 0) {
//        if (!(filename.match(RegEx))) {
//            i = 1;
//            flag = 1;
//            lblPlanServers.innerHTML = 'Filename should not contain spaces and special characters ';

//        }
//    }
//    else {
//        i = 1;
//        flag = 1;
//        lblPlanServers.innerHTML = 'Please Select Excel File ';

//    }


//    if (flag == 1) {
//        return false;
//    }
//    else {
//        ShowProgress();
//        return true;
//    }

//});

//$('.datetime').datetimepicker();
//var logic = function (currentDateTime) {
//    if (currentDateTime.getDay() == 6) {
//        this.setOptions({
//            minTime: '11:00'
//        });
//    } else
//        this.setOptions({
//            minTime: '8:00'
//        });
//};
///* end of Plan tab */


///* Prep tab */




//$('#ddlPrepNames').change(function () {
//    if ($('#ddlPrepNames').find('option:selected').text() == 'Select') {
//        $('#txtServerNames').removeAttr("disabled");
//        $('#btnCheck').removeAttr("disabled");
//    }
//    else {
//        $('#txtServerNames').attr("disabled", "disabled");
//    }
//});

//$('#txtServerNames').change(function () {
//    $('#btnCheck').removeAttr("disabled");
//});
//$('#txtServerNames').keypress(function (event) {

//    if ($('#ddlPrepNames').find('option:selected').text() != 'Select') {

//        $('#txtServerNames').attr("disabled", "disabled");
//        lblServernames.innerHTML = "Only One Option Avialble";
//    }
//    else {
//        $('#txtServerNames').removeAttr("disabled");
//        $('#btnCheck').removeAttr("disabled");

//        lblServernames.innerHTML = "";
//    }
//});

//$("#btnCheck").click(function () {


//    var flag1 = 0;
//    lblServers.innerHTML = "";
//    if ($('#ddlPrepNames').find('option:selected').text() == 'Select') {
//        if ($('#txtServerNames').val() == '') {
//            flag1 = 1;
//            lblServernames.innerHTML = 'Please Select Group Name or Enter ServerNames ';
//        }
     
//    }
//    if (flag1 == 1) {
//        return false;
//    }
//    else {
//        ShowProgress();
//        return true;

//    }
//});

//$("#btnUnistall").click(function () {
//    var flag1 = 0;
//    lblServers.innerHTML = "";
//    if ($('#ddlPrepNames').find('option:selected').text() == 'Select') {

//        if ($('#txtServerNames').val() == '') {
//            flag1 = 1;
//            lblServernames.innerHTML = 'Please Select Excel File or Enter ServerNames ';
//        }      
//    }
//      if (flag1 == 1)
//        {
//        return false;
//        }
//      else
//      {
//        ShowProgress();
//        return true;

//    }
//});

//$("#btnPreSmokeTest").click(function () {

//    var flag1 = 0;
//    lblServers.innerHTML = "";
//    if ($('#ddlPrepNames').find('option:selected').text() == 'Select') {

//        if ($('#txtServerNames').val() == '') {
//            flag1 = 1;
//            lblServernames.innerHTML = 'Please Select Excel File or Enter ServerNames ';
//        }

//    }
//    if (flag1 == 1) {
//        return false;
//    }
//    else {
//        ShowProgress();
//        return true;
//    }

//});

//$("#btnAddAdminScript").click(function () {

//    $('#btnExecute').removeAttr("disabled");
//    var flag = 0;
//    lblAdminName.innerHTML = "";
//    lblPassword.innerHTML = "";

//    if ($('#txtAdmin').val() == '') {

//        lblAdminName.innerHTML = 'Please Enter Admin Account Name';
//        flag = 1;

//    }
//    if ($('#txtPassword').val() == '') {

//        lblPassword.innerHTML = 'Please Enter Admin Account Password';
//        flag = 1;

//    }
//    if (flag == 1) {
//        return false;
//    }
//    else {

//        ShowProgress();

//        return true;
//    }
//});
//function EnablePreCheck() {
//    $('#btnCheck').removeAttr("disabled");
//    $('#txtServerNames').removeAttr("disabled");
//    $('#txtServerNames').val('');
//}
//function Enable() {
//    $('#btnAddAdmin').removeAttr("disabled");
//}
//function Disable() {
//    $('#btnAddAdmin').attr("disabled", "disabled");
//}


///* end of Prep tab */

///* Execute Tab */

//function IsValid() {
//    if ($('#ddlScenario').find('option:selected').text() == 'HLB Cluster') {
//        var checkedCheckboxes = $("#<%=gvHLBExecute.ClientID%> input[id*='ChkValue']:checkbox:checked").size();
//        if (checkedCheckboxes == 0) {
//            alert("Please select atleast One checkbox to Execute");
//            return false;
//        }

//    }
//}
//function InIEvent() {

//    $("#btnClusterExecute").click(IsValid);

//}

//function ShowExecuteProgress() {
//    setTimeout(function () {
//        var loading = $(".Executeloading");

//        loading.show();
//    }, 200);

//}

//function LoadOptions() {

//    if ($('#ddlPatchingOption').find('option:selected').text() == 'OnDemandPatch') {
//        $("#trODPOption").show();
//        $("#trSimpleUpdateOption").hide();
//        $("#trReboot").hide();
//        $("#trOnlyQFE").hide();
//        $("#trExcludeQFE").hide();
//    }
//    else if ($('#ddlPatchingOption').find('option:selected').text() == 'SimpleUpdate' || $('#ddlPatchingOption').find('option:selected').text() == 'SU-2x') {
//        $("#trODPOption").hide();
//        $("#trSimpleUpdateOption").show();
//        $("#trReboot").hide();
//        $("#trOnlyQFE").hide();
//        $("#trExcludeQFE").hide();
//    }
//    else if ($('#ddlPatchingOption').find('option:selected').text() == 'Chaining (ODP-SU)') {
//        $("#trODPOption").show();
//        $("#trSimpleUpdateOption").show();
//        $("#trReboot").hide();
//        $("#trOnlyQFE").hide();
//        $("#trExcludeQFE").hide();
//    }


//    else if ($('#ddlPatchingOption').find('option:selected').text() == 'MSNPatch' || $('#ddlPatchingOption').find('option:selected').text() == 'Select' || $('#ddlPatchingOption').find('option:selected').text() == 'Chaining(ODP-SU-MSNPATCH)') {


//        $("#trODPOption").hide();
//        $("#trSimpleUpdateOption").hide();
//        if ($('#ddlPatchingOption').find('option:selected').text() == 'MSNPatch') {
//            $("#trReboot").show();
//            $("#trOnlyQFE").show();
//            $("#trExcludeQFE").show();
//        }
//        else {

//            $("#trReboot").hide();
//            $("#trOnlyQFE").hide();
//            $("#trExcludeQFE").hide();
//        }
//    }


//}

//$('#txtExecuteServerNames').change(function () {
//    $('#btnExecute').removeAttr("disabled");
//});

//$('#txtExecuteServerNames').keypress(function (event) {
//    var Upload_file = document.getElementById('<%= fupExcel.ClientID %>');
//    var myfile = Upload_file.value;
//    if (myfile != '') {
//        $('#txtExecuteServerNames').attr("disabled", "disabled");
//        lblExecuteServerNames.innerHTML = "Only One Option Avialble";
//    }
//    else {

//        $('#txtExecuteServerNames').removeAttr("disabled");
//        $('#btnExecute').removeAttr("disabled");
//        lblExecuteServerNames.innerHTML = "";
//    }
//});


//$("#btnExecute").click(function () {
//    var flag = 0;
//    lblServers.innerHTML = "";
//    lblPatching.innerHTML = "";
//    if ($('#txtDomainAccName').val() == '') {

//        lblAdminAccountName.innerHTML = 'Please Enter Domain Account Name';
//        flag = 1;

//    }
//    if ($('#txtDomainAcctPwd').val() == '') {

//        lblAdminAcctPwd.innerHTML = 'Please Enter Domain Account Pwd';
//        flag = 1;

//    }
//    if ($('#txtLogPath').val() == '') {

//        lblLogPath.innerHTML = 'Please Enter LogPath';
//        flag = 1;

//    }

//    var Upload_file = document.getElementById('<%= fupExcel.ClientID %>');
//    var myfile = Upload_file.value;

//    var path = myfile;
//    var pos = path.lastIndexOf(path.charAt(path.indexOf(":") + 1));
//    var filename = path.substring(pos + 1);
//    var RegEx = /^([a-zA-Z0-9_\.])+$/;


//    if (myfile.indexOf("xlsx") > 0) {

//        if (!(filename.match(RegEx))) {
//            flag = 1;
//            lblExecuteServerNames.innerHTML = 'Filename should not contain spaces and special characters ';

//        }

//    }
//    else if (myfile.indexOf("xls") > 0) {
//        if (!(filename.match(RegEx))) {
//            flag = 1;
//            lblExecuteServerNames.innerHTML = 'Filename should not contain spaces and special characters ';

//        }
//    }

//    else {
//        if ($('#txtExecuteServerNames').val() == '') {
//            flag = 1;
//            lblExecuteServerNames.innerHTML = 'Please Select Excel File or Enter ServerNames ';
//        }

//    }


//    if ($('#ddlPatchingOption').find('option:selected').text() == 'Select') {
//        lblPatching.innerHTML = 'Please Select Tool Options';
//        flag = 1;
//    }
//    else {
//        lblPatching.innerHTML = '';
//    }
//    if ($('#ddlPatchingOption').find('option:selected').text() == 'Chaining(ODP-SU-MSNPATCH)') {
//        lblPatching.innerHTML = 'This Tool Option not avilable.';
//        flag = 1;
//    }


//    if (flag == 1) {
//        return false;
//    }
//    else {
//        ShowProgress();
//        return true;
//    }

//});


//$("#ddlScenario").change(function (e) {
//    if ($('#ddlScenario').find('option:selected').text() == 'HLB Cluster') {      
//        lblServerName.innerHTML = "VIP :";
//        lblServerlistPath.innerHTML = "VIPList Path :";
//        lblServersText.innerHTML = "Enter VIPs with , separated";
//    }
//    else {
//        lblServerName.innerHTML = "Server Names :";
//        lblServersText.innerHTML = "Enter Servernames with , separated";
//        lblServerlistPath.innerHTML = "ServersList Path :";
//    }
//});

//$("#ddlPatchingOption").change(function (e) {
//    LoadOptions();

//});
//function CheckEnable() {
//    $('#btnExecute').removeAttr("disabled");  
//    $('#txtExecuteServerNames').removeAttr("disabled");
//    $('#txtExecuteServerNames').val('');
//}

///* end of execute tab */


/////* Validate tab */

////$('#txtValidateServerNames').change(function () {
////    $('#btnValidatePatch').removeAttr("disabled");
////});
////$('#txtValidateServerNames').keypress(function (event) {
////    var Upload_file = document.getElementById('<%= fupValidateExcel.ClientID %>');
////    var myfile = Upload_file.value;
////    if (myfile != '') {
////        $('#txtValidateServerNames').attr("disabled", "disabled");
////        lblValidateServerNames.innerHTML = "Only One Option Avialble";
////    }
////    else {
////        $('#txtValidateServerNames').removeAttr("disabled");
////        $('#btnValidatePatch').removeAttr("disabled");

////        lblValidateServerNames.innerHTML = "";
////    }
////});

////$("#btnPostSmokeTest").click(function () {

////    var flag1 = 0;
////    lblServers.innerHTML = "";
////    var Upload_file = document.getElementById('<%= fupValidateExcel.ClientID %>');

////    document.getElementById('<%= hdnFileName.ClientID %>').value = myfile;
////    var myfile = Upload_file.value;

////    var path = myfile;
////    var pos = path.lastIndexOf(path.charAt(path.indexOf(":") + 1));
////    var filename = path.substring(pos + 1);
////    var RegEx = /^([a-zA-Z0-9_\.])+$/;


////    if (myfile.indexOf("xlsx") > 0) {

////        if (!(filename.match(RegEx))) {
////            flag1 = 1;
////            lblServernames.innerHTML = 'Filename should not contain spaces and special characters ';

////        }

////    }
////    else if (myfile.indexOf("xls") > 0) {
////        if (!(filename.match(RegEx))) {
////            flag1 = 1;
////            lblServernames.innerHTML = 'Filename should not contain spaces and special characters ';

////        }
////    }

////    else {

////        //flag1 = 1;
////        // lblValidateExcel.innerHTML = 'Please Select Excel File ';
////        if ($('#txtValidateServerNames').val() == '') {
////            flag1 = 1;
////            lblValidateServerNames.innerHTML = 'Please Select Excel File or Enter ServerNames ';
////        }

////    }
////    if (flag1 == 1) {
////        return false;
////    }
////    else {
////        ShowProgress();
////        return true;

////    }

////});





////$("#cbSimpleUpdate").click(function () {
////    if ($(this).is(':checked')) {
////        $("#cbNumbers").attr("checked", false);
////        $("#cbMSNPatch").attr("checked", false);
////        $("#cbODP").attr("checked", false);


////        $('#gvValidateResult').hide();
////        // $('#rvValidate').hide();
////        $('#txtKbValue').attr("disabled", "disabled");
////        $("#cbInstalledUpdates").attr("checked", false);
////        //                $('#txtStartDate').attr("disabled", "disabled");
////        //                $('#txtEndDate').attr("disabled", "disabled");


////    }
////});


////$('#cbMSNPatch').click(function () {

////    if ($(this).is(':checked')) {
////        $("#cbNumbers").attr("checked", false);
////        $("#cbSimpleUpdate").attr("checked", false);
////        $("#cbODP").attr("checked", false);
////        $('#gvValidateResult').hide();
////        //$('#rvValidate').hide();
////        $('#txtKbValue').attr("disabled", "disabled");
////        $("#cbInstalledUpdates").attr("checked", false);
////        //                $('#txtStartDate').attr("disabled", "disabled");
////        //                $('#txtEndDate').attr("disabled", "disabled");

////    }
////});

////$('#cbODP').click(function () {

////    if ($(this).is(':checked')) {
////        $("#cbNumbers").attr("checked", false);
////        $("#cbSimpleUpdate").attr("checked", false);
////        $("#cbMSNPatch").attr("checked", false);
////        $('#gvValidateResult').hide();
////        //$('#rvValidate').hide();
////        $('#txtKbValue').attr("disabled", "disabled");
////        $("#cbInstalledUpdates").attr("checked", false);
////        //                $('#txtStartDate').attr("disabled", "disabled");
////        //                $('#txtEndDate').attr("disabled", "disabled");


////    }
////});
////$('#cbNumbers').click(function () {

////    if ($(this).is(':checked')) {
////        $("#ValidatePopup").addClass("ValidatePopup");
////        $("#cbSimpleUpdate").attr("checked", false);
////        $("#cbODP").attr("checked", false);
////        $('#rvValidate').hide();
////        $('#txtKbValue').removeAttr("disabled");
////        $("#cbInstalledUpdates").attr("checked", false);
////        //                $('#txtStartDate').attr("disabled", "disabled");
////        //                $('#txtEndDate').attr("disabled", "disabled");
////    }
////    else {
////        $('#txtKbValue').attr("disabled", "disabled");
////        $("#ValidatePopup").addClass("rvValidatePopup");

////    }
////});

////$('#cbInstalledUpdates').click(function () {

////    if ($(this).is(':checked')) {
////        $("#ValidatePopup").addClass("ValidatePopup");
////        $("#cbMSNPatch").attr("checked", false);
////        $("#cbSimpleUpdate").attr("checked", false);
////        $("#cbODP").attr("checked", false);
////        $("#cbNumbers").attr("checked", false);
////        $('#txtKbValue').attr("disabled", "disabled");
////        $('#rvValidate').hide();
////        //                                $('#txtStartDate').removeAttr("disabled");
////        //                                $('#txtEndDate').removeAttr("disabled");

////    }
////    else {
////        //                                $('#txtStartDate').attr("disabled", "disabled");
////        //                                $('#txtEndDate').attr("disabled", "disabled");

////    }
////});


////$("#btnValidatePatch").click(function () {

////    // alert('s');
////    //$('#btnExecute').removeAttr("disabled");
////    var flag = 0;
////    lblTextValidate.innerHTML = "";
////    lblCheckPatch.innerHTML = "";
////    lblValidateServerNames.innerHTML = "";

////    var Upload_file = document.getElementById('<%= fupValidateExcel.ClientID %>');
////    var myfile = Upload_file.value;

////    var path = myfile;
////    var pos = path.lastIndexOf(path.charAt(path.indexOf(":") + 1));
////    var filename = path.substring(pos + 1);
////    var RegEx = /^([a-zA-Z0-9_\.])+$/;


////    if (myfile.indexOf("xlsx") > 0) {

////        if (!(filename.match(RegEx))) {
////            flag = 1;
////            lblValidateExcel.innerHTML = 'Filename should not contain spaces and special characters ';

////        }

////    }
////    else if (myfile.indexOf("xls") > 0) {
////        if (!(filename.match(RegEx))) {
////            flag = 1;
////            lblValidateExcel.innerHTML = 'Filename should not contain spaces and special characters ';

////        }
////    }

////    else {
////        if ($('#txtValidateServerNames').val() == '') {
////            flag = 1;
////            lblValidateServerNames.innerHTML = 'Please Select Excel File or Enter ServerNames ';
////        }
////        // flag = 1;
////        // lblValidateExcel.innerHTML = 'Please Select Excel File ';

////    }
////    var cbMSNPatch = document.getElementById("cbMSNPatch");
////    var cbODP = document.getElementById("cbODP");
////    var cbFindUpdates = document.getElementById("cbInstalledUpdates");
////    if (cbMSNPatch.checked) {
////        //return confirm("execute");
////    }
////    var cbSimpleUpdate = document.getElementById("cbSimpleUpdate");
////    if (cbSimpleUpdate.checked) {
////        //return confirm("execute");
////    }
////    var cbNumbers = document.getElementById("cbNumbers");
////    if (cbNumbers.checked) {

////        if ($('#txtKbValue').val() == '') {

////            lblTextValidate.innerHTML = 'Please Enter Kb Values for Validate';
////            flag = 1;

////        }

////    }
////    if (cbFindUpdates.checked) {
////        var txtStartDate = document.getElementById("txtStartDate");
////        var txtEndDate = document.getElementById("txtEndDate");
////        if ($('#txtStartDate').val() == '' || $('#txtEndDate').val() == '') {
////            lblInstalledUpdates.innerHTML = "Please select Dates";
////            flag = 1;
////        }
////        if (flag == 1) {
////            return false;
////        }
////        else {

////            return true;
////        }

////    }
////    if (cbMSNPatch.checked == false && cbNumbers.checked == false && cbSimpleUpdate.checked == false && cbODP.checked == false && cbFindUpdates.checked == false) {
////        lblCheckPatch.innerHTML = 'Please select atleast one checkbox';
////        flag = 1;
////    }
////    if (flag == 1) {
////        return false;
////    }
////    else {
////        ShowProgress();
////        return true;
////    }

////});
////function EnableValidate() {


////    $('#txtValidateServerNames').removeAttr("disabled");
////    $('#txtValidateServerNames').val('');

////}

/////*end of  Validate tab */


/////* Reports tab */



//$(".selectR1").click(function () {
//    var txtReportsStartDate = document.getElementById("txtReportsStartDate");
//    $("#<%=txtReportsStartDate.ClientID%>").datepicker({ dateFormat: 'mm-dd-yy' });
//    txtReportsStartDate.focus();
//});

//$(".selectR2").click(function () {
//    var txtReportsEndDate = document.getElementById("txtReportsEndDate");
//    $("#<%=txtReportsEndDate.ClientID%>").datepicker({ dateFormat: 'mm-dd-yy' });
//    txtReportsEndDate.focus();
//});

//$("#btnValidationReports").click(function () {
//    var flag = 0;  
//    if ($('#txtReportsStartDate').val() == '') {
//        lblReportsStartDate.innerHTML = 'Please Enter StartDate';
//        flag = 1;
//    }
//    if ($('#txtReportsEndDate').val() == '') {

//        lblReportsEndDate.innerHTML = 'Please Enter EndDate';
//        flag = 1;
//    }
//    if (flag == 1) {
//        return false;
//    }
//    else {
//        ShowProgress();
//        return true;
//    }
//});

//$("#btnPlanSummary").click(function () {

//    var flag = 0;
//    
//    if ($('#txtReportsStartDate').val() == '') {

//        lblReportsStartDate.innerHTML = 'Please Enter StartDate';
//        flag = 1;

//    }
//    if ($('#txtReportsEndDate').val() == '') {

//        lblReportsEndDate.innerHTML = 'Please Enter EndDate';
//        flag = 1;

//    }
//    if (flag == 1) {
//        return false;
//    }
//    else {
//        ShowProgress();
//        return true;

//    }
//});

//$("#btnExecuteReports").click(function () {

//    var flag = 0;
//   
//    if ($('#txtReportsStartDate').val() == '') {

//        lblReportsStartDate.innerHTML = 'Please Enter StartDate';
//        flag = 1;

//    }
//    if ($('#txtReportsEndDate').val() == '') {

//        lblReportsEndDate.innerHTML = 'Please Enter EndDate';
//        flag = 1;

//    }
//    if (flag == 1) {
//        return false;
//    }
//    else {
//        ShowProgress();
//        return true;

//    }
//});

//$("#btnOverallSummary").click(function () {
//    ShowProgress();
//    return true;
//});

/////*end of Reports tab */