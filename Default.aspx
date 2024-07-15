<%@ Page Title="首頁" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DynamicFormFW._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="container-fluid">
        <div class="row">
            <div class="col">
                <asp:UpdatePanel ID="upFormBuild" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                     <ContentTemplate>
                        <asp:PlaceHolder ID="phFormBuildArea" runat="server"></asp:PlaceHolder>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row">
            <div class="col text-center">
                <asp:Label ID="Label1" runat="server" CssClass="fs-1 fw-semibold" Text="表單尚未設計，請至後台設計"></asp:Label>
                <asp:Button ID="Submit" CssClass="btn btn-primary" runat="server" Text="表單送出" OnClick="Submit_Click" />
            </div>
        </div>
    </main>
</asp:Content>
