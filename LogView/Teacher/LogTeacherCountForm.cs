using FISCA.Presentation.Controls;
using K12.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1campus.notice.v17
{
    public partial class LogTeacherCountForm : BaseForm
    {
        MessageLogRecord _record { get; set; }

        public LogTeacherCountForm(MessageLogRecord record)
        {
            InitializeComponent();

            _record = record;

            Run();
        }

        private void Run()
        {
            //學生已讀清單
            foreach (PostTeacher teac in _record.CheckTeacherReadDic.Values)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridViewX1);

                row.Cells[colTeacherName.Index].Value = teac.TeacherName; //班級
                row.Cells[colAccount.Index].Value = teac.loginName; //帳號
                row.Cells[colTime.Index].Value = teac.IsTeacherReadTime; //讀取時間

                //是否已讀
                if (teac.IsTeacherRead)
                {
                    row.Cells[colISRead.Index].Value = "是";
                    row.Cells[colISRead.Index].Style.BackColor = Color.LightCyan;
                    row.Cells[colTeacherName.Index].Style.BackColor = Color.LightCyan;
                    row.Cells[colAccount.Index].Style.BackColor = Color.LightCyan;
                    row.Cells[colTime.Index].Style.BackColor = Color.LightCyan;
                }

                dataGridViewX1.Rows.Add(row);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
