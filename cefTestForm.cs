using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CefSharp;
using CefSharp.WinForms;

namespace _1campus.notice.v17
{
    public partial class cefTestForm : Form
    {
        public cefTestForm()
        {
            InitializeComponent();
        }

        private void cefTestForm_Load(object sender, EventArgs e)
        {
            Cef.Initialize();

            ChromiumWebBrowser _myBrowser;

            //ChromiumWebBrowser myBrowser = new ChromiumWebBrowser(@"c:\users\ed\documents\visual studio 2017\Projects\cefSharp_test\cefSharp_test\HTMLResouces\html\content.html");

            ChromiumWebBrowser myBrowser = new ChromiumWebBrowser(@"http://www.maps.google.com");

            _myBrowser = myBrowser;

            this.Controls.Add(myBrowser);

        }
    }
}
