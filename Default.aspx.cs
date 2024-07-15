using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DynamicFormFW
{
    public partial class _Default : Page
    {
        // 讀取配置文件中的連線字串
        string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string formId = "1";

                string queryDrawerBlock = @"
                    SELECT [FormId],[Id],[Title],[ColCount],[Version],[AtCreatDateTime],[AtLastDateTime]
                    FROM [DynamicForm].[dbo].[FormBlock]
                    WHERE FormId = @FormId";

                SqlParameter[] parametersDrawerBlock = new SqlParameter[] { new SqlParameter("@FormId", formId) };
                DataTable dataTableDrawerBlock = ExecuteQuery(queryDrawerBlock, parametersDrawerBlock);

                if (dataTableDrawerBlock.Rows.Count <= 0)
                {
                    Submit.Visible = false;
                    Label1.Visible = true;
                }
                else
                {
                    Submit.Visible = true;
                    Label1.Visible = false;
                }

                foreach (DataRow row in dataTableDrawerBlock.Rows)
                {
                    int blockId = Convert.ToInt32(row["Id"]);
                    string title = row["Title"].ToString();
                    int colCount = Convert.ToInt32(row["ColCount"]);

                    AddDrawerBlock(blockId, colCount, title);
                }

                string queryDrawerControl = @"
                    SELECT
                        [FormId]
                        ,[Version]
                        ,[BlockId]
                        ,[BlockColIndex]
                        ,[ControlId]
                        ,[ControlType]
                        ,[ControlTitle]
                        ,[AtCreatDateTime]
                        ,[AtLastDateTime]
                    FROM [DynamicForm].[dbo].[FormControl]
                    WHERE FormId = @FormId";

                SqlParameter[] parametersDrawerControl = new SqlParameter[] { new SqlParameter("@FormId", formId) };
                DataTable dataTableDrawerControl = ExecuteQuery(queryDrawerControl, parametersDrawerControl);

                foreach (DataRow row in dataTableDrawerControl.Rows)
                {
                    int blockId = Convert.ToInt32(row["BlockId"]);
                    int blockColIndex = Convert.ToInt32(row["BlockColIndex"]);
                    int controlId = Convert.ToInt32(row["ControlId"]);
                    string controlTitle = row["ControlTitle"].ToString();
                    string controlType = row["ControlType"].ToString();

                    AddDrawerControl(blockId, blockColIndex, controlId, controlType, controlTitle);
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 要做些事
            }
        }

        private void AddDrawerBlock(int blockId, int colCount, string title)
        {
            HtmlGenericControl newDrawerBlock = CreateDrawerBlock(blockId, colCount, title);

            PlaceHolder ph = new PlaceHolder();
            ph.Controls.Add(newDrawerBlock);

            UpdatePanel up = new UpdatePanel();
            up.ContentTemplateContainer.Controls.Add(ph);

            phFormBuildArea.Controls.Add(up);
            upFormBuild.Update();
        }

        private void AddDrawerControl(int blockId, int colIndex, int controlId, string controlType, string controlTitle)
        {
            string drawerColumnPanelId = $"drawerBlock{blockId}ColumnPanel{colIndex}";
            UpdatePanel updatePanel = (UpdatePanel)upFormBuild.FindControl(drawerColumnPanelId);
            PlaceHolder placeHolder = (PlaceHolder)updatePanel.FindControl($"drawerBlock{blockId}ColumnPanelHolder{colIndex}");

            // 找到 PlaceHolder 中的现有 <table> 元素
            // Find existing HtmlTable or create a new one if not exists
            HtmlTable existingTable = FindOrCreateHtmlTable(placeHolder);

            // Create new table row
            HtmlTableRow tableRow = new HtmlTableRow();
            tableRow.Controls.Add(CreateDrawerControlTitle(controlTitle));
            tableRow.Controls.Add(CreateDrawerControlTdHtml(CreateDrawerControl(controlId.ToString(), controlType), controlType));

            existingTable.Rows.Add(tableRow);
            // Update the panel to reflect changes
            updatePanel.Update();
        }

        private HtmlTable FindOrCreateHtmlTable(PlaceHolder placeHolder)
        {
            // Check if an HtmlTable exists in the PlaceHolder
            foreach (Control control in placeHolder.Controls)
            {
                if (control is HtmlTable existingTable)
                {
                    return existingTable; // Return the existing table if found
                }
            }

            // If no existing HtmlTable found, create a new one
            HtmlTable newTable = new HtmlTable();
            newTable.Attributes["class"] = "table-wraper__table";
            placeHolder.Controls.Add(newTable);
            return newTable;
        }

        private HtmlTableCell CreateDrawerControlTitle(string controlTitle)
        {
            HtmlTableCell tableTdControlTitle = new HtmlTableCell("td");
            tableTdControlTitle.Attributes["class"] = "th";
            tableTdControlTitle.InnerHtml = $"<span>{controlTitle}</span>"; // Replace with your dynamic content
            return tableTdControlTitle;
        }

        private HtmlTableCell CreateDrawerControlTdHtml(Control drawerControl, string controlType)
        {
            HtmlTableCell tableTdControlContainer = new HtmlTableCell("td");
            HtmlGenericControl divControlContainer = new HtmlGenericControl("div");
            switch (controlType.ToLower())
            {
                case "label":
                    tableTdControlContainer.Attributes["class"] = "tc bg-disabled text-dark";
                    divControlContainer.Attributes["class"] = "label-wrap--50 my-auto px-3";
                    break;
                default:
                    tableTdControlContainer.Attributes["class"] = "tc";
                    divControlContainer.Attributes["class"] = "input-wrap";
                    break;
            }
            divControlContainer.Controls.Add((Control)drawerControl);
            tableTdControlContainer.Controls.Add((HtmlGenericControl)divControlContainer);
            return tableTdControlContainer;
        }

        private HtmlGenericControl CreateDrawerBlock(int blockId, int colCount, string title)
        {
            HtmlGenericControl drawerDivBlock = new HtmlGenericControl("div");
            drawerDivBlock.Attributes["class"] = "drawer drawer--shown";
            drawerDivBlock.Attributes["data-drawer-id"] = blockId.ToString();
            drawerDivBlock.ID = $"drawerBlock{blockId}";

            HtmlGenericControl drawerTitle = new HtmlGenericControl("div");
            drawerTitle.Attributes["class"] = "drawer__title drawer__title--shown";

            HtmlGenericControl group1 = new HtmlGenericControl("div");
            group1.Attributes["class"] = "group";
            group1.InnerHtml = $"<span class='title'>{title}</span>";

            HtmlGenericControl group2 = new HtmlGenericControl("div");
            group2.Attributes["class"] = "group";
            group2.InnerHtml = "<div class='toggle-drawer-button'><i class='fas fa-minus'></i>收起</div>";

            drawerTitle.Controls.Add(group1);
            drawerTitle.Controls.Add(group2);
            drawerDivBlock.Controls.Add(drawerTitle);

            /* drawer 內容 html tag 處理 */
            HtmlGenericControl drawerContent = new HtmlGenericControl("div");
            drawerContent.Attributes["class"] = "drawer__content";
            drawerContent.Style["display"] = "block";

            /* drawer__content > row */
            HtmlGenericControl drawerContentRow = new HtmlGenericControl("div");
            drawerContentRow.Attributes["class"] = "row";

            /* Bootstrap 5.2 Columns */
            int colWidth = 12 / colCount;
            for (int colIndex = 1; colIndex <= colCount; colIndex++)
            {
                HtmlGenericControl drawerContentCol = new HtmlGenericControl("div");
                drawerContentCol.Attributes["class"] = $"col-{colWidth}";

                // Create UpdatePanel and PlaceHolder for each column - STRAT
                UpdatePanel updatePanel = new UpdatePanel();
                updatePanel.ID = $"drawerBlock{blockId}ColumnPanel{colIndex}";
                updatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional;

                PlaceHolder placeHolder = new PlaceHolder();
                placeHolder.ID = $"drawerBlock{blockId}ColumnPanelHolder{colIndex}";

                // Add tableFormWrap to the PlaceHolder
                //placeHolder.Controls.Add(tableFormWrap);
                // Create UpdatePanel and PlaceHolder for each column - END

                HtmlGenericControl tableFormWrap = new HtmlGenericControl("div");
                tableFormWrap.Attributes["class"] = "table_form-wrap";

                HtmlGenericControl formWraper = new HtmlGenericControl("div");
                formWraper.Attributes["class"] = "form-wraper";

                HtmlGenericControl mainForm = new HtmlGenericControl("div");
                mainForm.Attributes["class"] = "main-form";

                HtmlGenericControl tableWraper = new HtmlGenericControl("div");
                tableWraper.Attributes["class"] = "table-wraper";

                HtmlTable table = new HtmlTable();
                table.Attributes["class"] = "table-wraper__table";

                // 無添加 < tbody > 若要添加使用 LiteralControl

                /* 組合、套崁 HTML - 開始*/
                // 無法將ASP Panel Add to table HTML 語意不通，故反制
                placeHolder.Controls.Add(table);
                // Add PlaceHolder to the UpdatePanel
                updatePanel.ContentTemplateContainer.Controls.Add(placeHolder);
                tableWraper.Controls.Add(updatePanel);
                mainForm.Controls.Add(tableWraper);
                formWraper.Controls.Add(mainForm);
                tableFormWrap.Controls.Add(formWraper);
                drawerContentCol.Controls.Add(tableFormWrap);
                drawerContentRow.Controls.Add(drawerContentCol);
            }

            drawerContent.Controls.Add(drawerContentRow);

            HtmlGenericControl row2 = new HtmlGenericControl("div");
            row2.Attributes["class"] = "row";

            HtmlGenericControl textCenter = new HtmlGenericControl("div");
            textCenter.Attributes["class"] = "text-center";

            HtmlGenericControl toggleDrawerButton = new HtmlGenericControl("div");
            toggleDrawerButton.Attributes["class"] = "toggle-drawer-button toggle-drawer-button--content";
            toggleDrawerButton.InnerHtml = "<i class='fas fa-minus'></i>收起";

            textCenter.Controls.Add(toggleDrawerButton);
            row2.Controls.Add(textCenter);
            drawerContent.Controls.Add(row2);

            drawerDivBlock.Controls.Add(drawerContent);

            return drawerDivBlock;
        }

        private Control CreateDrawerControl(string id, string type)
        {
            Control drawerControl;
            switch (type.ToLower())
            {
                case "textbox":
                    drawerControl = new TextBox();
                    ((TextBox)drawerControl).CssClass = "form-control";
                    ((TextBox)drawerControl).Attributes["data-required"] = "false";
                    ((TextBox)drawerControl).MaxLength = 100;
                    break;
                case "dropdownlist":
                    drawerControl = new DropDownList();
                    ((DropDownList)drawerControl).CssClass = "form-control";
                    ((DropDownList)drawerControl).DataSource = new List<string> { "Option 1", "Option 2", "Option 3" };
                    ((DropDownList)drawerControl).DataBind();
                    break;
                case "checkbox":
                    drawerControl = new CheckBox();
                    break;
                case "radiobutton":
                    drawerControl = new RadioButton();
                    break;
                case "label":
                    drawerControl = new Label();
                    ((Label)drawerControl).Text = "未設定";
                    break;
                default:
                    throw new ArgumentException("Invalid control type");
            }
            drawerControl.ID = id;
            return drawerControl;
        }


        protected void Submit_Click(object sender, EventArgs e)
        {
            //    TextBox dynameicTextBox_1 = (TextBox)phLabels.FindControl("dynameicTextBox_1");
            //    if (dynameicTextBox_1 != null)
            //    {
            //        name = ((TextBox)phLabels.FindControl("dynameicTextBox_1")).Text;
            //    }
        }

        // 函数：执行查询并返回 DataTable
        private DataTable ExecuteQuery(string query, SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(parameters);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"資料庫操作錯誤：{ex.Message}");
                }
            }
            return dataTable;
        }
    }
}