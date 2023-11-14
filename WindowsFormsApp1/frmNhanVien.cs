using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class frmNhanVien : Form
    {
        private DatabaseConnection databaseConnection;
        private string DbConnectionString = @"Server=NGUYEN\MSSQLSERVER01;Database=HRM;Trusted_Connection=True;";
        public frmNhanVien()
        {
            InitializeComponent();
            this.initDataCbxPhongBan();
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {

        }

        private void onLoad()
        {

        }

        private void cbxPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbxPhongBan.SelectedValue != null)
            {
                databaseConnection = new DatabaseConnection(DbConnectionString);
                databaseConnection.Open();
                DataTable table = databaseConnection.QueryData("Select E.FullName Ten,D.DepartmentFullName TenPhongBan , E.BusinessEmail Email, IE.Luong Luong" +
                    " From dbo.H0_DepartmentEmployee DE " +
                    " INNER JOIN dbo.Employees E ON DE.UserId = E.UserId And DE.DepartmentId = " + this.cbxPhongBan.SelectedValue +
                    " JOIN dbo.H1_IncomeEmployees IE ON DE.UserId = IE.UserId " +
                    " JOIN dbo.H0_Departments D ON D.DepartmentId = DE.DepartmentId ");
                // Bind the data to the form's controls

                dvgData.Rows.Clear();
                foreach (DataRow dataRow in table.Rows)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    ToolStrip toolStrip1 = new ToolStrip();
           
                    row.Cells.Add(new DataGridViewTextBoxCell() { Value = dataRow["Ten"] });
                    row.Cells.Add(new DataGridViewTextBoxCell() { Value = dataRow["TenPhongBan"] });
                    row.Cells.Add(new DataGridViewTextBoxCell() { Value = dataRow["Email"] });
                    row.Cells.Add(new DataGridViewTextBoxCell() { Value = dataRow["Luong"] });
                 
                    dvgData.Rows.Add(row);
                }
                dvgData.Refresh();
                databaseConnection.Close();
            }
        }

        private void initDataCbxPhongBan ()
        {
            this.cbxPhongBan.DataSource = null;
            databaseConnection = new DatabaseConnection(DbConnectionString);
            databaseConnection.Open();
            DataTable table = databaseConnection.QueryData("Select d.DepartmentId id,d.DepartmentFullName value From dbo.H0_Departments d");
            // Bind the data to the form's controls
            this.cbxPhongBan.DataSource = table;
            databaseConnection.Close();
        }

        private void dvgData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dvgData.Columns[e.ColumnIndex].Name == "Action")
            {
                if(MessageBox.Show("Bạn có muốn gửi email cho nhân viên này không ?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var contentStr = "" +
                    "<!DOCTYPE html><html><head>" +
                    "<title>Welcome Email</title></head>" +
                    "<body><br /><table width=\"50%\">" +
                    "<tr><td align=\"left\" style=\"color: white; border-radius:10px; background-color:#34b5b8; padding:18px 0 0 10px;text-align: center;\">" +
                    "<label style=\"font-size:22px;\"> Thông báo nhận lương tháng 11 </label>" +
                    "<br />" +
                    "<br />" +
                    "</td> </tr><tr align=\"left\"> <td><br />Kính chào, <strong>{Employees}</strong>" +
                    "<br /><br />Lương tháng 11 của bạn là : <strong>{Salary}</strong> đã được thanh toán." +
                    "<br /> " +
                    "<br />" +
                    " <a href=\"{DomainName}/account/login\" target=\"_blank\" style=\"text-align: center;display: block;\">" +
                    "<button style=\"padding: 10px;font-size: 15px;background-color: #34b5b8;border-radius: 10px;border: none;color: white;text-align: center;cursor: pointer\">Truy cập hệ thống</button> </a>" +
                    " <br /><br /> <b>Trân trọng,</b><br /><br /><b>Công ty CP Dịch vụ công nghệ</b>" +
                    "<br /> Website: <a target=\"_blank\" style=\"color: #34b5b8; text-decoration: none;\">https://test.com</a></td></tr>" +
                    "</table>" +
                    "</body>" +
                    "</html>";

                    contentStr.Replace("{Employees}", dvgData.Rows[e.ColumnIndex].Cells[0].Value.ToString());
                    contentStr.Replace("{Salary}", dvgData.Rows[e.ColumnIndex].Cells[3].Value.ToString());
                    this.SendEmail(dvgData.Rows[e.ColumnIndex].Cells[2].Value.ToString(), contentStr);
                }
            }
        }

        public void SendEmail(string to, string body)
        {
            string email = "vunguyen140220@gmail.com";
            string app_pasword = "kfcq tpaf qrta cgov";
            var client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                //UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials =
                    new NetworkCredential(
                        email,
                        app_pasword),
                EnableSsl = true,
            };
            var fromAddress = new MailAddress("vunguyen140220@gmail.com", "Company - Notification");
            var toAddress = new MailAddress(to);
            var mail = new MailMessage(fromAddress, toAddress)
            {
                Subject = "Thông báo lương tháng 11",
                Body = body,
                IsBodyHtml = true,
            };
            client.Send(mail);
            MessageBox.Show("Mail gửi thành công!");
        }
    }
}
