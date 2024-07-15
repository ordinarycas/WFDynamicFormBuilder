<%@ Page Title="管理者後台-表單管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormManage.aspx.cs" Inherits="DynamicFormFW.FormManage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %></h2>
        <asp:GridView ID="GridViewFormList" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="table table-bordered" OnRowCommand="GridViewFormList_RowCommand">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Button ID="btnInsert" runat="server" OnClick="btnInsert_Click" Text="新增" CssClass="btn btn-primary" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>' Text="編輯" CssClass="btn btn-outline-secondary" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="儲存" CssClass="btn btn-primary" />
                        |
                        <asp:Button ID="btnCancelSave" runat="server" OnClick="btnCancelSave_Click" Text="取消" CssClass="btn btn-outline-secondary" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="代號">
                    <ItemTemplate>
                        <asp:Label ID="gvLblFormId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="名稱">
                    <ItemTemplate>
                        <asp:Label ID="gvLblFormName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox Id="gvTbFormName" runat="server" Text="表單名稱"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="說明">
                    <ItemTemplate>
                        <asp:Label ID="gvLblFormDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox Id="gvTbFormDescription" runat="server" Text=""></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Version" HeaderText="版本" ReadOnly="True" />
                <asp:BoundField DataField="AtCreatDateTime" HeaderText="新增日期" ReadOnly="True" />
                <asp:BoundField DataField="EmpNo" HeaderText="新增人" ReadOnly="True" />
            </Columns>
        </asp:GridView>
    </main>
</asp:Content>
