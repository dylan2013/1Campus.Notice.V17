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
    public partial class LogCountForm : BaseForm
    {
        MessageLogRecord _record { get; set; }

        public LogCountForm(MessageLogRecord record)
        {
            InitializeComponent();

            _record = record;

            Run();
        }

        private void Run()
        {


            List<PostStudent> ShowStudentList = CheckStudent(_record.CheckStudentReadDic);

            //學生已讀清單
            foreach (PostStudent stud in ShowStudentList)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridViewX1);

                row.Cells[colClassName.Index].Value = stud.className; //班級
                row.Cells[colSeatNo.Index].Value = stud.seatNo; //座號
                row.Cells[colStudentName.Index].Value = stud.StudentName; //姓名
                row.Cells[colAccount.Index].Value = stud.SALoginName; //帳號

                row.Cells[colTime.Index].Value = stud.IsStudentReadTime; //讀取時間

                //是否已讀
                if (stud.IsStudentRead)
                {
                    row.Cells[colISRead.Index].Value = "是";
                    row.Cells[colISRead.Index].Style.BackColor = Color.LightCyan;
                    row.Cells[colClassName.Index].Style.BackColor = Color.LightCyan;
                    row.Cells[colSeatNo.Index].Style.BackColor = Color.LightCyan;
                    row.Cells[colStudentName.Index].Style.BackColor = Color.LightCyan;
                    row.Cells[colAccount.Index].Style.BackColor = Color.LightCyan;
                    row.Cells[colTime.Index].Style.BackColor = Color.LightCyan;
                }

                dataGridViewX1.Rows.Add(row);
            }


            //家長已讀清單
            List<PostParent> ShowParentList = CheckParent(_record.CheckParentReadDic);
            foreach (PostParent parent in ShowParentList)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridViewX2);

                row.Cells[colClassNameParent.Index].Value = parent.className; //班級
                row.Cells[colSeatNoParent.Index].Value = parent.seatNo; //座號
                row.Cells[colStudentNameParent.Index].Value = parent.StudentName; //姓名

                row.Cells[colParentName.Index].Value = parent.relationship; //稱謂
                row.Cells[colAccountParent.Index].Value = parent.account; //帳號
                row.Cells[colReadTime.Index].Value = parent.IsParentReadTime; //讀取時間

                //是否已讀
                if (parent.IsParentRead)
                {
                    row.Cells[colIsReadParent.Index].Value = "是";
                    row.Cells[colIsReadParent.Index].Style.BackColor = Color.LightCyan;
                    row.Cells[colClassNameParent.Index].Style.BackColor = Color.LightCyan;
                    row.Cells[colSeatNoParent.Index].Style.BackColor = Color.LightCyan;
                    row.Cells[colStudentNameParent.Index].Style.BackColor = Color.LightCyan;
                    row.Cells[colParentName.Index].Style.BackColor = Color.LightCyan;
                    row.Cells[colAccountParent.Index].Style.BackColor = Color.LightCyan;
                    row.Cells[colReadTime.Index].Style.BackColor = Color.LightCyan;
                }
                dataGridViewX2.Rows.Add(row);
            }
        }

        //排序家長清單
        private List<PostParent> CheckParent(Dictionary<string, Dictionary<string, PostParent>> checkParentReadDic)
        {
            List<PostParent> list = new List<PostParent>();
            foreach (Dictionary<string, PostParent> a in checkParentReadDic.Values)
            {
                list.AddRange(a.Values.ToList());
            }
            list.Sort(SortParent);
            return list;
        }

        private int SortParent(PostParent parent1, PostParent parent2)
        {
            string parent1_string = "";
            parent1_string += parent1.IsParentRead ? "00000" : "11111";
            parent1_string += !string.IsNullOrEmpty(parent1.account) ? "00000" : "11111";
            parent1_string += parent1.className.PadLeft(10, '0');
            parent1_string += parent1.seatNo.PadLeft(10, '0');
            parent1_string += parent1.StudentName.PadLeft(10, '0');

            string parent2_string = "";
            parent2_string += parent2.IsParentRead ? "00000" : "11111";
            parent2_string += !string.IsNullOrEmpty(parent2.account) ? "00000" : "11111";
            parent2_string += parent2.className.PadLeft(10, '0');
            parent2_string += parent2.seatNo.PadLeft(10, '0');
            parent2_string += parent2.StudentName.PadLeft(10, '0');

            return parent1_string.CompareTo(parent2_string);
        }

        //排序學生清單
        //依據1.已讀
        //2.班級座號姓名等資訊排序
        private List<PostStudent> CheckStudent(Dictionary<string, PostStudent> checkStudentReadDic)
        {
            List<PostStudent> list = checkStudentReadDic.Values.ToList();
            list.Sort(SortStudent);
            return list;
        }

        private int SortStudent(PostStudent stud1, PostStudent stud2)
        {
            string stud1_string = "";
            stud1_string += stud1.IsStudentRead ? "00000" : "11111";
            stud1_string += !string.IsNullOrEmpty(stud1.SALoginName) ? "00000" : "11111";
            stud1_string += stud1.className.PadLeft(10, '0');
            stud1_string += stud1.seatNo.PadLeft(10, '0');
            stud1_string += stud1.StudentName.PadLeft(10, '0');

            string stud2_string = "";
            stud2_string += stud2.IsStudentRead ? "00000" : "11111";
            stud2_string += !string.IsNullOrEmpty(stud2.SALoginName) ? "00000" : "11111";
            stud2_string += stud2.className.PadLeft(10, '0');
            stud2_string += stud2.seatNo.PadLeft(10, '0');
            stud2_string += stud2.StudentName.PadLeft(10, '0');

            return stud1_string.CompareTo(stud2_string);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {




        }
    }
}
