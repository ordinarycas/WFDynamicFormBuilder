using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace DynamicFormFW
{
    public partial class FormManage : Page
    {
        // 讀取配置文件中的連線字串
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private DataTable _dataTable = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initialControls();
            }
        }


        #region Function
        /// <summary>
        /// 控制項初始化
        /// </summary>
        private void initialControls()
        {
            _dataTable.Clear();
            _dataTable = this.ExecuteQuery("SELECT[Id],[Name],[Description],[Version],[AtCreatDateTime],[EmpNo]" +
                "FROM [DynamicForm].[dbo].[FormBasicInfo]");

            // Check if the DataTable is empty
            if (_dataTable.Rows.Count == 0)
            {
                // Create a new DataRow and add it to the DataTable
                DataRow defaultFormListRow = _dataTable.NewRow();
                _dataTable.Rows.Add(defaultFormListRow);

                // Optionally, you can set default values for the columns
                defaultFormListRow["Id"] = 0;
                defaultFormListRow["Name"] = "請先按新增";
                defaultFormListRow["Description"] = "-";
                defaultFormListRow["Version"] = 0;
                defaultFormListRow["AtCreatDateTime"] = DateTime.Now;
                defaultFormListRow["EmpNo"] = "-";
            }

            GridViewFormList.DataSource = _dataTable;
            GridViewFormList.DataBind();
        }

        #endregion

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            GridViewFormList.FooterRow.Visible = true;
        }

        protected void GridViewFormList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                // 获取行索引
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                // 使用 DataKeys 获取 ID
                string id = GridViewFormList.DataKeys[rowIndex].Value.ToString();
                Response.Redirect(string.Format("FormDeign.aspx?id={0}", id));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            String gvTbFormName, gvTbFormDescription;
            gvTbFormName = ((TextBox)GridViewFormList.FooterRow.Cells[0].FindControl("gvTbFormName")).Text;
            gvTbFormDescription = ((TextBox)GridViewFormList.FooterRow.Cells[0].FindControl("gvTbFormDescription")).Text;

            /* 寫入資料 */
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // 假设您有一个 Users 表格，包含 ID、Username 和 Password 字段
                string insertSql = "INSERT INTO [DynamicForm].[dbo].[FormBasicInfo] ([Name], [Description],[EmpNo]) VALUES (@Name, @Description, @EmpNo)";

                using (SqlCommand command = new SqlCommand(insertSql, connection))
                {
                    command.Parameters.AddWithValue("@Name", gvTbFormName);
                    command.Parameters.AddWithValue("@Description", gvTbFormDescription);
                    command.Parameters.AddWithValue("@EmpNo", "0000000001");

                    // 打开数据库连接并执行插入操作
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    // 检查是否成功插入数据
                    if (rowsAffected > 0)
                    {
                        // 插入成功的处理逻辑
                        initialControls();
                    }
                    else
                    {
                        // 插入失败的处理逻辑
                    }
                }
            }

        }

        protected void btnCancelSave_Click(object sender, EventArgs e)
        {
            GridViewFormList.FooterRow.Visible = false;
        }

        #region
        /// <summary>
        /// 資料庫查詢
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // 可以在此处处理异常
                    Console.WriteLine($"数据库操作错误：{ex.Message}");
                }
            }
            return dataTable;
        }
        #endregion
    }
}