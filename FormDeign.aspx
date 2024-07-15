<%@ Page Title="表單設計" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormDeign.aspx.cs" Inherits="DynamicFormFW.FormDeign" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="container-fluid">
        <div class="row">
            <div class="col">
                <asp:Label ID="LabTitle" runat="server" CssClass="fs-2 fw-semibold"></asp:Label>
                <hr>
            </div>
        </div>
        <div class="row">
            <div class="col-3">
                <p class="fs-3 fw-semibold">元件區</p>
                
                <div class="border-top my-3"></div>
                
                <div>
                    <p class="fs-5">區塊元件</p>

                    <div class="mb-3">
                        <label for="tbDrawerTitle" class="form-label">名稱</label>
                        <asp:TextBox ID="tbDrawerTitle" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="ddlDrawerColCount" class="form-label">欄位數</label>
                        <asp:DropDownList ID="ddlDrawerColCount" runat="server" class="form-select" >
                            <asp:ListItem Text="1欄，純內容" Value="1"></asp:ListItem>
                            <asp:ListItem Text="2欄，左、右" Value="2"></asp:ListItem>
                            <asp:ListItem Text="3欄，左中右" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:Button ID="btnAddDrawerBlock" runat="server" Text="新增區塊" OnClick="btnAddDrawerBlock_Click" CssClass="btn btn-primary" />
                </div>
                
                <div class="border-top my-3"></div>
                
                <div>
                    <p class="fs-5">選擇編輯區塊</p>

                    <div class="mb-3">
                        <label for="ddlSelectDrawerId" class="form-label">選擇編輯區塊</label>
                        <asp:DropDownList ID="ddlSelectDrawerId" runat="server" OnSelectedIndexChanged="ddlSelectDrawerId_Change" AutoPostBack="true" class="form-select" >
                            <asp:ListItem Text="代號，標題名稱" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="mb-3">
                        <label for="ddlSelectDrawerColIndex" class="form-label">選擇編輯區塊欄位</label>
                        <asp:DropDownList ID="ddlSelectDrawerColIndex" runat="server" class="form-select" >
                            <asp:ListItem Text="第1欄" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:Button ID="btnSelectDrawer" runat="server" Text="選取區塊" CssClass="btn btn-primary" OnClick="btnSelectDrawer_Click" />
                </div>

                <div>
                    <p class="fs-5">新增填寫欄位</p>

                    <div class="mb-3">
                        <label for="tbDrawerControlTitleCn" class="form-label">中文名稱</label>
                        <asp:TextBox ID="tbDrawerControlTitleCn" runat="server" CssClass="form-control" Text="欄位標題"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="tbDrawerControlTitleId" class="form-label">英文名稱(程式)</label>
                        <asp:TextBox ID="tbDrawerControlTitleId" runat="server" CssClass="form-control" Text="(未開放)"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="ddlControlType" class="form-label">欄位類型</label>
                        <asp:DropDownList ID="ddlControlType" runat="server" class="form-select" >
                            <asp:ListItem Text="文字顯示框" Value="label" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="文字輸入框" Value="textBox" ></asp:ListItem>
                            <asp:ListItem Text="下拉選單"  Value="dropDownList"></asp:ListItem>
                            <asp:ListItem Text="單選方塊" Value="radioButton"></asp:ListItem>
                            <asp:ListItem Text="多選方塊" Value="checkBox"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:Button ID="btnAddControlType" runat="server" OnClick="btnAddControlType_Click" Text="新增控制項" CssClass="btn btn-primary" />
                </div>
            </div>
            <div class="col-9">
                <asp:UpdatePanel ID="upFormDeign" runat="server" UpdateMode="Conditional">
                     <ContentTemplate>
                        <asp:PlaceHolder ID="phFormDeignWorkArea" runat="server"></asp:PlaceHolder>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <hr>
                <asp:Button CssClass="btn btn-primary w-100" ID="btnSaveDesign" runat="server" Text="儲存當前設計" OnClick="btnSaveDesign_Click" />
            </div>
        </div>
    </main>
</asp:Content>