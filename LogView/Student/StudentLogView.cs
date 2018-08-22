using FISCA.Presentation.Controls;
using K12.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1campus.notice.v17
{
    public partial class StudentLogView : BaseForm
    {

        BackgroundWorker BGW;

        string dateTime1 { get; set; }
        string dateTime2 { get; set; }

        string TimeFormat = "yyyy/MM/dd HH:mm:ss";

        Dictionary<string, StudRecord> StudentDic { get; set; }

        string totalCount = "";
        int totalSuperCount = 0;

        public StudentLogView()
        {
            InitializeComponent();
        }

        private void MessageLovView_Load(object sender, EventArgs e)
        {
            BGW = new BackgroundWorker();

            BGW.DoWork += BGW_DoWork;
            BGW.RunWorkerCompleted += BGW_RunWorkerCompleted;

            //預設七天之資料
            dateTimeInput1.Value = DateTime.Today.AddDays(-7);
            dateTimeInput2.Value = DateTime.Today;

            Run();

        }

        private void Run()
        {
            this.Text = "家長學生訊息歷程(資料下載中)";
            FormLoad = false;

            dateTime1 = dateTimeInput1.Value.ToString(TimeFormat);

            //加一天減一秒
            dateTime2 = dateTimeInput2.Value.AddDays(1).AddSeconds(-1).ToString(TimeFormat);


            //開始取得資料
            BGW.RunWorkerAsync();
        }

        private void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            //取得主要Log記錄
            //(發送時間/人員/標題/給家長/給學生)
            List<MessageLogRecord> messageList = DataManager.GetNoticeList(dateTime1, dateTime2, false);

            if (messageList.Count > 0)
            {
                //取得訊息給多少學生/又有多少家長
                List<string> NoticeIDList = new List<string>();
                Dictionary<string, MessageLogRecord> NoticeList = new Dictionary<string, MessageLogRecord>();
                foreach (MessageLogRecord each in messageList)
                {
                    if (!NoticeIDList.Contains(each.notice_id))
                    {
                        NoticeIDList.Add(each.notice_id);
                    }
                }

                //設定學生數 & 家長數
                DataManager.SetStudentCount(NoticeIDList, messageList);

                //設定訊息數
                SetSendCount(NoticeIDList, messageList);
            }
            else
            {
                e.Cancel = true;
            }

            e.Result = messageList;
        }

        /// <summary>
        /// 設定已讀數 & 發送數
        /// </summary>
        private void SetSendCount(List<string> noticeIDList, List<MessageLogRecord> messageList)
        {
            //取得學生記錄
            StudentDic = GetStudent(messageList);

            totalCount = DataManager.TotalCount();

            foreach (MessageLogRecord _record in messageList)
            {
                if (_record.studentList.Count > 0)
                {
                    //建立學生基本資料
                    foreach (string stud_id in _record.studentList)
                    {
                        if (StudentDic.ContainsKey(stud_id))
                        {
                            StudRecord stud = StudentDic[stud_id];

                            if (!_record.CheckStudentReadDic.ContainsKey(stud_id))
                            {
                                _record.CheckStudentReadDic.Add(stud_id, new PostStudent(stud));

                            }
                        }
                    }

                    //取得學生讀取記錄
                    StringBuilder sb_sql = new StringBuilder();
                    sb_sql.Append("select ref_notice_id,ref_student_id,ref_parent_id,time from $ischool.1campus.notice_log notice_log ");
                    sb_sql.Append(string.Format("where ref_notice_id='{0}' ", _record.notice_id));
                    sb_sql.Append(string.Format("and ref_student_id in ('{0}') ", string.Join("','", _record.studentList)));
                    //家長是空值 , 老師是空值
                    sb_sql.Append("and ref_parent_id is null and ref_teacher_id is null");

                    DataTable dt = tool._Q.Select(sb_sql.ToString());

                    foreach (DataRow row in dt.Rows)
                    {
                        string ref_student_id = "" + row["ref_student_id"];

                        if (_record.CheckStudentReadDic.ContainsKey(ref_student_id))
                        {
                            _record.CheckStudentReadDic[ref_student_id].IsStudentRead = true;

                            string time = "" + row["time"];
                            _record.CheckStudentReadDic[ref_student_id].IsStudentReadTime = time;
                        }
                    }
                }

                //需有家長才取得家長讀取記錄
                if (_record.parentList.Count > 0)
                {
                    //取得目前學生所關連之家長個人資料
                    StringBuilder sb_sParent_sql = new StringBuilder();
                    sb_sParent_sql.Append(string.Format("select * from student_parent where id in ('{0}')", string.Join("','", _record.parentList)));

                    DataTable dtsParent = tool._Q.Select(sb_sParent_sql.ToString());
                    foreach (DataRow row in dtsParent.Rows)
                    {
                        string ref_student_id = "" + row["ref_student_id"];

                        if (!_record.CheckParentReadDic.ContainsKey(ref_student_id))
                        {
                            _record.CheckParentReadDic.Add(ref_student_id, new Dictionary<string, PostParent>());
                        }

                        if (StudentDic.ContainsKey(ref_student_id))
                        {
                            PostParent parent = new PostParent(row, StudentDic[ref_student_id]);
                            _record.CheckParentReadDic[ref_student_id].Add(parent.parent_id, parent);
                        }
                    }

                    //取得家長讀取記錄
                    StringBuilder sb_parent_sql = new StringBuilder();
                    sb_parent_sql.Append("select ref_notice_id,ref_student_id,ref_parent_id,time from $ischool.1campus.notice_log notice_log ");
                    sb_parent_sql.Append(string.Format("where notice_log.ref_notice_id='{0}' ", _record.notice_id));
                    sb_parent_sql.Append(string.Format("and notice_log.ref_parent_id in ('{0}') ", string.Join("','", _record.parentList)));
                    sb_parent_sql.Append("and ref_parent_id is not null and ref_teacher_id is null ");
                    sb_parent_sql.Append("order by time");

                    DataTable dt_parent = tool._Q.Select(sb_parent_sql.ToString());
                    foreach (DataRow row in dt_parent.Rows)
                    {
                        string ref_parent_id = "" + row["ref_parent_id"];
                        string ref_student_id = "" + row["ref_student_id"];
                        string time = "" + row["time"];
                        if (_record.CheckParentReadDic.ContainsKey(ref_student_id))
                        {
                            if (_record.CheckParentReadDic[ref_student_id].ContainsKey(ref_parent_id))
                            {
                                _record.CheckParentReadDic[ref_student_id][ref_parent_id].IsParentRead = true;

                                if (string.IsNullOrEmpty(_record.CheckParentReadDic[ref_student_id][ref_parent_id].IsParentReadTime))
                                {
                                    _record.CheckParentReadDic[ref_student_id][ref_parent_id].IsParentReadTime = time;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 取得所有學生物件
        /// </summary>
        private Dictionary<string, StudRecord> GetStudent(List<MessageLogRecord> messageList)
        {
            Dictionary<string, StudRecord> dic = new Dictionary<string, StudRecord>();
            List<string> StudentIDList = new List<string>();
            foreach (MessageLogRecord _record in messageList)
            {
                StudentIDList.AddRange(_record.studentList);
            }

            List<StudRecord> studList = DataManager.GetStudent(StudentIDList);
            foreach (StudRecord stud in studList)
            {
                if (!dic.ContainsKey(stud.ID))
                {
                    dic.Add(stud.ID, stud);
                }
            }
            return dic;
        }

        private void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FormLoad = true;
            this.Text = "家長學生訊息歷程";
            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    dataGridViewX1.Rows.Clear();
                    List<MessageLogRecord> messageList = (List<MessageLogRecord>)e.Result;
                    foreach (MessageLogRecord record in messageList)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(dataGridViewX1);

                        //訊息系統編號
                        row.Cells[colNoticeID.Index].Value = record.notice_id;

                        row.Cells[colSendTime.Index].Value = record.post_time;
                        row.Cells[colSender.Index].Value = record.display_sender;
                        row.Cells[colTitle.Index].Value = record.title;

                        //內文
                        //row.Cells[colMessage.Index].Value = StripHTML(record.message);

                        row.Cells[colSendParent.Index].Value = record.parent_visible == "true" ? "是" : "";
                        row.Cells[colSendStudent.Index].Value = record.student_visible == "true" ? "是" : "";

                        row.Cells[colStudentCount.Index].Value = record.studentList.Count; //學生數
                        row.Cells[colParentCount.Index].Value = record.parentList.Count; //家長數

                        int countSend = 0;
                        int countRead = 0;

                        foreach (PostStudent stud in record.CheckStudentReadDic.Values)
                        {
                            if (stud.IsStudentRead)
                            {
                                countRead++;
                            }
                            else
                            {
                                //若學生沒有已讀
                                //去查詢家長是否已讀過
                                if (record.CheckParentReadDic.ContainsKey(stud._stud.ID))
                                {
                                    foreach (PostParent parent in record.CheckParentReadDic[stud._stud.ID].Values)
                                    {
                                        if (parent.IsParentRead)
                                        {
                                            countRead++;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(stud.SALoginName))
                            {
                                countSend++;
                            }
                        }

                        foreach (string studID in record.CheckParentReadDic.Keys)
                        {
                            foreach (PostParent parent in record.CheckParentReadDic[studID].Values)
                            {
                                //若此訊息只發給家長,則要從此邏輯處計數
                                if (record.student_visible == "false" && record.parent_visible == "true")
                                {
                                    if (parent.IsParentRead)
                                    {
                                        countRead++;
                                    }
                                }

                                if (!string.IsNullOrEmpty(parent.account))
                                {
                                    countSend++;
                                }
                            }
                        }

                        row.Cells[colSendCount.Index].Value = countSend; //發送數
                        row.Cells[colMessageCount.Index].Value = countRead; //已讀數
                        totalSuperCount += countRead;

                        row.ReadOnly = true;
                        row.Tag = record;
                        dataGridViewX1.Rows.Add(row);
                    }

                }
                else
                {
                    MsgBox.Show("發生錯誤:\n" + e.Error.Message);
                }
            }
            else
            {
                MsgBox.Show("無訊息內容");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnViewContent_Click(object sender, EventArgs e)
        {
            if (dataGridViewX1.SelectedRows.Count == 1)
            {

                MessageLogRecord record = (MessageLogRecord)dataGridViewX1.SelectedRows[0].Tag;

                LogViewForm view = new LogViewForm(record, "student");
                view.ShowDialog();
            }
            else
            {
                MsgBox.Show("請選擇一筆資料");
            }
        }


        private void dataGridViewX1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (e.ColumnIndex <= 3)
                {
                    MessageLogRecord record = (MessageLogRecord)dataGridViewX1.SelectedRows[0].Tag;
                    LogViewForm view = new LogViewForm(record, "student");
                    view.ShowDialog();
                }
                else
                {
                    MessageLogRecord record = (MessageLogRecord)dataGridViewX1.SelectedRows[0].Tag;
                    LogCountForm view = new LogCountForm(record);
                    view.ShowDialog();
                }
            }
        }

        private void btnViewCount_Click(object sender, EventArgs e)
        {
            //顯示已讀清單

            if (dataGridViewX1.SelectedRows.Count == 1)
            {
                MessageLogRecord record = (MessageLogRecord)dataGridViewX1.SelectedRows[0].Tag;

                LogCountForm lcf = new LogCountForm(record);
                lcf.ShowDialog();
            }
            else
            {
                MsgBox.Show("請選擇一筆資料");
            }


        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dateTimeInput1.Text != "" && dateTimeInput2.Text != "")
            {
                if (!BGW.IsBusy)
                {
                    Run();
                }
                else
                {
                    MsgBox.Show("系統忙碌中,稍後再試");
                }
            }
            else
            {
                MsgBox.Show("請選擇時間區間");
            }
        }

        private bool FormLoad
        {
            set
            {
                dataGridViewX1.Enabled = value;
                btnSelect.Enabled = value;
                dateTimeInput1.Enabled = value;
                dateTimeInput2.Enabled = value;
                btnViewContent.Enabled = value;
                btnViewCount.Enabled = value;

            }
        }
    }
}
