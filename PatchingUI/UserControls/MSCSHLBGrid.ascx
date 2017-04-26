<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MSCSHLBGrid.ascx.cs" Inherits="PatchingUI.UserControls.MSCSHLBGrid" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Panel ID="panelContainer" runat="server" Width="100%" ScrollBars="Auto">
    <asp:GridView ID="gvStandaloneMSCSHLB" runat="server" AutoGenerateColumns="false" CellPadding="4"
    ShowHeaderWhenEmpty="true" ForeColor="#333333" GridLines="None" Font-Size="10px" OnRowDataBound="gvStandaloneMSCSHLB_OnRowDataBound">
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
        <asp:TemplateField Visible="false">
            <ItemTemplate>
                <asp:Label ID="lblUniqueID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UniqueID")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="" ControlStyle-Width="10px">
            <HeaderTemplate>
                <asp:CheckBox ID="chkRunValidationAll" runat="server" Text="Run Validations" />
            </HeaderTemplate>
            <HeaderStyle HorizontalAlign="Left" Width="10px" />
            <ItemTemplate>
                <asp:CheckBox ID="chkRunValidation" runat="server" Checked='<%# GetCheckedResult(Eval("RunValidationFlag").ToString()) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="" ControlStyle-Width="10px">
            <HeaderTemplate>
                <asp:Label ID="lblHeadForceSa" runat="server" ToolTip="Force Standalone" Text="Force SA"></asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkForceStandalone" runat="server" Checked='<%# GetCheckedResult(Eval("ForceStandaloneFlag").ToString()) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Cluster Name">
            <ItemTemplate>
                <asp:Label ID="lblVIPIP" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClusterName")%>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
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
        
        <asp:TemplateField HeaderText="" ControlStyle-Width="10px">
            <HeaderTemplate>
                <asp:CheckBox ID="ChkPauseAll" runat="server" Text="Pause Node Before Patching" />
            </HeaderTemplate>
            <HeaderStyle HorizontalAlign="Left" Width="10px" />
            <ItemTemplate>
                <asp:CheckBox ID="ChkPauseNode" runat="server" Checked='<%# GetCheckedResult(Eval("PauseNodeBeforePatching").ToString()) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Pause During Execution" ControlStyle-Width="60px">
            <ItemTemplate>
                <asp:Label ID="lblPauseDuringExec" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "PauseNodeDuringExecution")%>'></asp:Label>
                <asp:DropDownList ID="ddlPause" runat="server" Font-Size="10px" Width="150px">
                    <asp:ListItem Text="" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Pause after Patching Only" Value="1"></asp:ListItem>
                    <asp:ListItem Enabled="false" Text="Pause after Patching+failover Only" Value="2"></asp:ListItem>
                    <asp:ListItem Enabled="false" Text="Pause after patching and pause after failover" Value="3"></asp:ListItem>
                </asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="">
            <HeaderTemplate>
                <asp:CheckBox ID="ChkFailBackAll" runat="server" Text="Failback To Original Cluster State" />
            </HeaderTemplate>
            <HeaderStyle HorizontalAlign="Left" />
            <ItemTemplate>
                <asp:CheckBox ID="ChkFailBack" runat="server"  OnCheckedChanged="ChkFailBack_Clicked" Checked='<%# GetCheckedResult(Eval("FailbackToOriginalState").ToString()) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Select the node for Failover" ControlStyle-Width="60px">
            <ItemTemplate>
                <asp:Label ID="lblNodeClusterName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClusterName")%>' Visible="false"></asp:Label>
                <asp:Label ID="lblBackUpNode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BackupNode")%>' Visible="false"></asp:Label>
                <asp:DropDownList ID="ddlFailOverNode" runat="server" Font-Size="10px" Width="150px">
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
                <asp:Label ID="lblExtension" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "IsExtensionServer")%>'
                    ForeColor='<%# GetExtensionColor(Eval("IsExtensionServer").ToString()) %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Patch Result (T/S/F/R)">
            <ItemTemplate>
                <asp:HyperLink ID="hlPatchResult" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchResult")%>'></asp:HyperLink>
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
                <asp:Label ID="lblPatchStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PatchStatus")%>'
                    ForeColor='<%# GetColor(Eval("PatchStatus").ToString()) %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="RunStatus">
            <ItemTemplate>
                <asp:Label ID="lblrunStatus" runat="server" Text='<%# GetFormatedData(Eval("RunStatus").ToString())%>' ForeColor='<%# GetColor(Eval("RunStatus").ToString()) %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Resume">
            <ItemTemplate>
                <asp:Label ID="lblClusterName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClusterName")%>' Visible="false"></asp:Label>
                <asp:Button ID="btnResume" runat="server" Text="Resume" Font-Size="10px" OnClick="btnResume_Click"
                    CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' Enabled='<%# GetCheckedResult(Eval("Flag").ToString()) %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</asp:Panel>
