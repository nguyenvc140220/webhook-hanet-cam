using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Xml.Linq;
using System.Data.Common;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private DatabaseConnection databaseConnection;

        public Form1()
        {
            InitializeComponent();
            databaseConnection = new DatabaseConnection(@"Server=localhost\MSSQLSERVER01;Database=HRM;Database=master;Trusted_Connection=True;");
            this.comboBox1_SelectedIndexInitdata();
            this.comboBox2_SelectedIndexInitdata();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            databaseConnection = new DatabaseConnection(@"Server=localhost\MSSQLSERVER01;Database=HRM;Database=master;Trusted_Connection=True;");
            databaseConnection.Open();
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
                "<br /> Website: <a target=\"_blank\" style=\"color: #34b5b8; text-decoration: none;\">https://test.com</a></td></tr>\r\n</table>\r\n</body>\r\n</html>";
            if (this.comboBox2.SelectedValue != null) {                
                DataTable table = databaseConnection.QueryData("Select E.BusinessEmail, E.FullName From dbo.Employees E Where E.UserId = " + this.comboBox2.SelectedValue);
                DataTable tableDetail = databaseConnection.QueryData("Select E.Luong From dbo.H1_IncomeEmployees E Where E.UserId = " + this.comboBox2.SelectedValue);
                contentStr.Replace("{Employees}", table.Rows[0][1].ToString());
                contentStr.Replace("{Salary}", tableDetail.Rows[0][0].ToString());
                this.SendEmail(table.Rows[0][0].ToString(), contentStr);
            }
            else
            {
                if (this.comboBox1.SelectedValue != null)
                {
                    DataTable table = databaseConnection
                    .QueryData("Select DE.DepartmentId, DE.UserId userId, E.FullName , E.BusinessEmail, IE.Luong " +
                    "From dbo.H0_DepartmentEmployee DE " +
                    "INNER JOIN dbo.Employees E ON DE.UserId = E.UserId And DE.DepartmentId = " + this.comboBox1.SelectedValue +
                    "JOIN dbo.H1_IncomeEmployees IE ON DE.UserId = IE.UserId");
                    
                    // Iterate through the DataTable and add each row to the List
                    foreach (DataRow row in table.Rows)
                    {
                        contentStr.Replace("{Employees}", row["FullName"].ToString());
                        contentStr.Replace("{Salary}", row["Luong"].ToString());
                        this.SendEmail(table.Rows[0][0].ToString(), contentStr);
                    }
                }
            }
            databaseConnection.Close();

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBox2_SelectedIndexInitdata();
        }

        private void comboBox1_SelectedIndexInitdata()
        {
            this.comboBox1.DataSource = null;
            databaseConnection = new DatabaseConnection(@"Server=localhost\MSSQLSERVER01;Database=HRM;Database=master;Trusted_Connection=True;");
            databaseConnection.Open();
            DataTable table = databaseConnection.QueryData("Select d.DepartmentId id,d.DepartmentFullName value From dbo.H0_Departments d");
            // Bind the data to the form's controls
            this.comboBox1.DataSource = table;
            databaseConnection.Close();
        }

        private void comboBox2_SelectedIndexInitdata()
        {
            if(this.comboBox1.SelectedValue != null)
            {
                databaseConnection = new DatabaseConnection(@"Server=localhost\MSSQLSERVER01;Database=HRM;Database=master;Trusted_Connection=True;");
                databaseConnection.Open();
                DataTable table = databaseConnection
                    .QueryData("Select DE.DepartmentId, DE.UserId userId, E.FullName fullName " +
                    "From dbo.H0_DepartmentEmployee DE " +
                    "INNER JOIN dbo.Employees E ON DE.UserId = E.UserId And DE.DepartmentId = " + this.comboBox1.SelectedValue);
                // Bind the data to the form's controls
                this.comboBox2.DataSource = table;
                databaseConnection.Close();
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
        }



    }
}
