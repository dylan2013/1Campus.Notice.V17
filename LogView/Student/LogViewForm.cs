using CefSharp;
using CefSharp.WinForms;
using FISCA.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1campus.notice.v17
{
    public partial class LogViewForm : BaseForm
    {
        MessageLogRecord _record { get; set; }

        ChromiumWebBrowser _myBrowser { get; set; }
        string _type { get; set; }

        public LogViewForm(MessageLogRecord record, string type)
        {
            InitializeComponent();

            _record = record;
            _type = type;

            lbTime.Text = "發送時間：" + _record.post_time;
            lbUser.Text = "發送人員：" + _record.display_sender;
            lbTitle.Text = "標　　題：" + _record.title;

            if (type == "teacher")
            {
                cbParent.Visible = false;
                cbStudent.Visible = false;
                lbSendToUser.Visible = false;
            }
            else if (type == "student")
            {
                cbParent.Checked = _record.parent_visible == "true" ? true : false;
                cbStudent.Checked = _record.student_visible == "true" ? true : false;
            }

            //取得目前的專案dll檔的 絕對路徑(若在使用者端 就會抓絕對的使用者電腦路徑)
            FileInfo projectDllURL = new FileInfo(Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", ""));
            //取得html 相對路徑
            string htmlRelativeURL = Path.Combine(projectDllURL.DirectoryName, "HTMLResouces\\html\\content.html"); ;

            //2017/8/15 穎驊註解， 下面為穎驊的開發機 Html 的絕對位子，在開發測試完畢後，將使用相對路徑 找到html 檔案
            //ChromiumWebBrowser myBrowser = new ChromiumWebBrowser(@"C:\Users\ED\Documents\ischool_github\1campus.notice.v17\HTMLResouces\html\content.html");
            ChromiumWebBrowser myBrowser = new ChromiumWebBrowser(htmlRelativeURL);

            //ChromiumWebBrowser myBrowser = new ChromiumWebBrowser(@"http://www.maps.google.com");
            _myBrowser = myBrowser;

            _myBrowser.LoadHtml(_record.message, "http://www.ischool.com.tw/1campus-app.html");
            this.panel1.Controls.Add(myBrowser);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void linkOnSend_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            //RePostNotice post = new RePostNotice(RePostNotice.Type.Student, _record);
            //post.ShowDialog();

        }
    }
}
