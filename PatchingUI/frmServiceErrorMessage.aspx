<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmServiceErrorMessage.aspx.cs" Inherits="PatchingUI.frmServiceErrorMessage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<body style="background-color:#1E90FF; border:0;">
    <form id="form1" runat="server">  
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="30000"
                    EnablePartialRendering="true" ScriptMode="Release">
                </asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="timerSrvcChk" EventName="Tick" />
                    </Triggers>
                    <ContentTemplate>                        
                        <asp:Label ID="lblServiceErrorMessage" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                        <asp:Timer ID="timerSrvcChk" OnTick="timerSrvcChk_Tick" runat="server" Interval="10000"
                            Enabled="true">
                        </asp:Timer>
                    </ContentTemplate>
                </asp:UpdatePanel>   
    </form>
</body>
</html>
