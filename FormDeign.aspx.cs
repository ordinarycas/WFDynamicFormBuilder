using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DynamicFormFW
{
    public partial class FormDeign : Page
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private DataTable _dataTable = new DataTable();
        #region Form JSON Data
        /* Form JSON Data */
        //private dynamic formDeignJsonData = new { drawers = new List<Drawer>() }; // 初始化为一个空的 drawers 列表

        /* Form JSON Data Object */
        /*
        public class Drawer
        {
            public int id { get; set; }
            public string title { get; set; }
            public int columns { get; set; }
            public List<DrawerControl> controls { get; set; }
        }

        public class DrawerControl
        {
            public string id { get; set; }
            public Dictionary<string, string> title { get; set; }
            public string type { get; set; }

        }
        */
        #endregion


        /// <summary>
        /// DrawerBlockId, drawerColCount, 
        /// </summary>
        private List<Tuple<int, int, string>> _listTupleDrawerBlocks
        {
            get
            {
                if (ViewState["listTupleDrawerBlocks"] == null)
                {
                    ViewState["listTupleDrawerBlocks"] = new List<Tuple<int, int, string>>();
                }
                return (List<Tuple<int, int, string>>)ViewState["listTupleDrawerBlocks"];
            }
            set
            {
                ViewState["listTupleDrawerBlocks"] = value;
            }
        }

        private List<Tuple<int, int, int, string, string>> _listTupleDrawerControls
        {
            get
            {
                if (ViewState["listTupleDrawerControls"] == null)
                {
                    ViewState["listTupleDrawerControls"] = new List<Tuple<int, int, int, string,string>>();
                }
                return (List<Tuple<int, int, int, string, string>>)ViewState["listTupleDrawerControls"];
            }
            set
            {
                ViewState["listTupleDrawerControls"] = value;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }

            //if(this._listTupleDrawerBlocks.Count <= 0)
            //{
            //    LoadDrawerBlocks();
            //}

            //if(this._listTupleDrawerControls.Count <= 0)
            //{
            //    LoadDrawerControls();
            //}
        }

        private void LoadDrawerBlocks()
        {
            string formId = Request.QueryString["id"];
            // 构建 SQL 查询语句
            string queryDrawerBlock = @"
                    SELECT [FormId],[Id],[Title],[ColCount],[Version],[AtCreatDateTime],[AtLastDateTime]
                    FROM [DynamicForm].[dbo].[FormBlock]
                    WHERE FormId = @FormId"; // 使用参数化查询

            SqlParameter[] parametersDrawerBlock = new SqlParameter[] { new SqlParameter("@FormId", formId) };
            DataTable dataTableDrawerBlock = ExecuteQuery(queryDrawerBlock, parametersDrawerBlock);

            foreach (DataRow row in dataTableDrawerBlock.Rows)
            {
                int blockId = Convert.ToInt32(row["Id"]);
                string title = row["Title"].ToString();
                int colCount = Convert.ToInt32(row["ColCount"]);

                _listTupleDrawerBlocks.Add(new Tuple<int, int, string>(blockId, colCount, title));
                // AddDrawerBlock(blockId, colCount, title);
            }
            //RecreateDrawerBlocks();

            ddlSelectDrawerId.Items.Clear();
            foreach (var item in _listTupleDrawerBlocks)
            {
                ddlSelectDrawerId.Items.Add(new ListItem(item.Item3, item.Item1.ToString()));
            }
        }

        private void LoadDrawerControls()
        {
            string formId = Request.QueryString["id"];

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
                    WHERE FormId = @FormId"; // 使用参数化查询

            SqlParameter[] parametersDrawerControl = new SqlParameter[] { new SqlParameter("@FormId", formId) };
            DataTable dataTableDrawerControl = ExecuteQuery(queryDrawerControl, parametersDrawerControl);

            foreach (DataRow row in dataTableDrawerControl.Rows)
            {
                int blockId = Convert.ToInt32(row["BlockId"]);
                int blockColIndex = Convert.ToInt32(row["BlockColIndex"]);
                int controlId = Convert.ToInt32(row["ControlId"]);
                string controlTitle = row["ControlTitle"].ToString();
                string controlType = row["ControlType"].ToString();

                _listTupleDrawerControls.Add(new Tuple<int, int, int, string, string>(blockId, blockColIndex, controlId, controlType, controlTitle));
                // AddDrawerControl(blockId, blockColIndex, controlId, controlType, controlTitle);
            }
            //RecreateDrawerControls();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initialControls();
                LoadDrawerBlocks();
                LoadDrawerControls();
            }
            // Recreate dynamic controls on postback
            RecreateDrawerBlocks();
            RecreateDrawerControls();
        }

        protected void btnSelectDrawer_Click(object sender, EventArgs e)
        {
            string drawerBlockId = ddlSelectDrawerId.SelectedValue.ToString();
            string drawerColIndex = ddlSelectDrawerColIndex.SelectedValue.ToString();

            string drawerColumnPanelId = $"drawerBlock{drawerBlockId}ColumnPanel{drawerColIndex}";
            UpdatePanel updatePanel = (UpdatePanel)upFormDeign.FindControl(drawerColumnPanelId);
            updatePanel.Attributes["style"] = "border: 6px dashed red; min-height: 46px;";
            updatePanel.Update();
        }

        protected void ddlSelectDrawerId_Change(object sender, EventArgs e)
        {
            ddlSelectDrawerColIndex.Items.Clear();
            int drawerId;
            if (int.TryParse(ddlSelectDrawerId.SelectedValue, out drawerId))
            {
                Tuple<int, int, string> selectedTuple = _listTupleDrawerBlocks.FirstOrDefault(tuple => tuple.Item1 == drawerId);

                if (selectedTuple != null)
                {
                    for (int colIndex = 1; colIndex <= selectedTuple.Item2; colIndex++)
                    {
                        ddlSelectDrawerColIndex.Items.Add(new ListItem($"第{colIndex}欄", colIndex.ToString()));
                    }
                }
                else
                {
                    // Handle the case where no matching tuple is found
                    // This could be a warning message or some other logic
                }
            }
            else
            {
                // Handle the case where parsing fails
                // This could be a warning message or some other logic
            }
        }

        protected void btnAddDrawerBlock_Click(object sender, EventArgs e)
        {
            string drawerTitle = tbDrawerTitle.Text;
            int drawerColCount = int.Parse(ddlDrawerColCount.SelectedValue);
            int drawerBlockId = _listTupleDrawerBlocks.Count + 1;

            AddDrawerBlock(drawerBlockId, drawerColCount, drawerTitle);
            _listTupleDrawerBlocks.Add(new Tuple<int, int, string>(drawerBlockId, drawerColCount, drawerTitle));

            ddlSelectDrawerId.Items.Clear();
            foreach (var item in _listTupleDrawerBlocks)
            {
                ddlSelectDrawerId.Items.Add(new ListItem(item.Item3, item.Item1.ToString()));
            }
        }

        protected void btnAddControlType_Click(object sender, EventArgs e)
        {
            // Add a new control to the specified drawer block and column
            int selectDrawerBlockId = int.Parse(ddlSelectDrawerId.SelectedValue);
            int selectDrawerBlockColIndex = int.Parse(ddlSelectDrawerColIndex.SelectedValue);
            string controlTitle = tbDrawerControlTitleCn.Text.ToString();
            string controlType = ddlControlType.SelectedValue.ToString();
            int drawerControlId = _listTupleDrawerControls.Count + 1;

            AddDrawerControl(selectDrawerBlockId, selectDrawerBlockColIndex, drawerControlId, controlType, controlTitle);
            _listTupleDrawerControls.Add(new Tuple<int, int, int, string, string>(selectDrawerBlockId, selectDrawerBlockColIndex, drawerControlId, controlType, controlTitle));
        }
        private void RecreateDrawerBlocks()
        {
            foreach (Tuple<int, int, string> drawerBlock in _listTupleDrawerBlocks)
            {
                AddDrawerBlock(drawerBlock.Item1, drawerBlock.Item2, drawerBlock.Item3);
            }
        }
        private void RecreateDrawerControls()
        {
            foreach (var controlInfo in _listTupleDrawerControls)
            {
                AddDrawerControl(controlInfo.Item1, controlInfo.Item2, controlInfo.Item3, controlInfo.Item4 ,controlInfo.Item5);
            }
        }
        private void AddDrawerBlock(int blockId, int colCount, string title)
        {
            HtmlGenericControl newDrawerBlock = CreateDrawerBlock(blockId, colCount, title);

            PlaceHolder ph = new PlaceHolder();
            ph.Controls.Add(newDrawerBlock);

            UpdatePanel up = new UpdatePanel();
            up.ContentTemplateContainer.Controls.Add(ph);

            phFormDeignWorkArea.Controls.Add(up);
            upFormDeign.Update();
        }

        private void AddDrawerControl(int blockId, int colIndex, int controlId, string controlType, string controlTitle)
        {
            string drawerColumnPanelId = $"drawerBlock{blockId}ColumnPanel{colIndex}";
            UpdatePanel updatePanel = (UpdatePanel)upFormDeign.FindControl(drawerColumnPanelId);
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


        #region Function
        private void initialControls()
        {
            string formId = Request.QueryString["id"];
            string query = "SELECT TOP 1 [Id], [Name], [Description], [Version], [AtCreatDateTime], [EmpNo] FROM [DynamicForm].[dbo].[FormBasicInfo] WHERE [Id] = @Id";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", formId)
            };

            DataTable dataTable = ExecuteQuery(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                // 可以根据需要处理查询结果，例如将数据绑定到控件
                LabTitle.Text = "正在編輯的表單名稱：" + dataTable.Rows[0]["Name"].ToString();
            }
        }
        #endregion

        #region DataBase Function
        /// <summary>
        /// 資料庫查詢
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
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
        #endregion

        private void ExecuteNonQuery(string query, SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(parameters);
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"資料庫操作錯誤：{ex.Message}");
                    // 可以考慮將錯誤記錄到文件或其他日志系統
                }
            }
        }

        protected void btnSaveDesign_Click(object sender, EventArgs e)
        {
            string formId = Request.QueryString["id"];

            string queryInsertBlock = @"INSERT INTO [DynamicForm].[dbo].[FormBlock]([FormId], [Id], [Title], [ColCount], [Version])
                SELECT @FormId, @Id, @Title, @ColCount, @Version
                WHERE NOT EXISTS (
                    SELECT 1 
                    FROM [DynamicForm].[dbo].[FormBlock] 
                    WHERE [FormId] = @FormId 
                    AND [Id] = @Id);";

            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (var block in _listTupleDrawerBlocks)
            {
                parameters.Clear();
                parameters.Add(new SqlParameter("@FormId", formId));
                parameters.Add(new SqlParameter("@Id", block.Item1)); // Block ID
                parameters.Add(new SqlParameter("@Title", block.Item3)); // Title
                parameters.Add(new SqlParameter("@ColCount", block.Item2)); // Column count
                parameters.Add(new SqlParameter("@Version", 1)); // Version (adjust as needed)
                ExecuteNonQuery(queryInsertBlock, parameters.ToArray());
            }



            string queryInsertControl = @"INSERT INTO [DynamicForm].[dbo].[FormControl]
                ([FormId],[Version],[BlockId],[BlockColIndex],[ControlId],[ControlType],[ControlTitle])
                SELECT @FormId, @Version, @BlockId, @BlockColIndex, @ControlId, @ControlType, @ControlTitle
                WHERE NOT EXISTS (
                    SELECT 1 
                    FROM [DynamicForm].[dbo].[FormControl] 
                    WHERE [FormId] = @FormId 
                    AND [BlockId] = @BlockId
                    AND [ControlId] = @ControlId
                );";

            List<SqlParameter> parametersControl = new List<SqlParameter>();

            foreach (var block in _listTupleDrawerControls)
            {
                parametersControl.Clear();
                parametersControl.Add(new SqlParameter("@FormId", formId));
                parametersControl.Add(new SqlParameter("@BlockId", block.Item1)); // Block ID
                parametersControl.Add(new SqlParameter("@BlockColIndex", block.Item2)); // Column count
                parametersControl.Add(new SqlParameter("@ControlId", block.Item3)); 
                parametersControl.Add(new SqlParameter("@ControlType", block.Item4));
                parametersControl.Add(new SqlParameter("@ControlTitle", block.Item5));
                parametersControl.Add(new SqlParameter("@Version", 1)); // Version (adjust as needed)
                ExecuteNonQuery(queryInsertControl, parametersControl.ToArray());
            }
        }
    }
}