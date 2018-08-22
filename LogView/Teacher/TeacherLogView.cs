using FISCA.Presentation.Controls;
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
    public partial class TeacherLogView : BaseForm
    {
        BackgroundWorker BGW;

        string dateTime1 { get; set; }
        string dateTime2 { get; set; }

        string TimeFormat = "yyyy/MM/dd HH:mm:ss";

        Dictionary<string, TeacRecord> TeacherDic { get; set; }

        public TeacherLogView()
        {
            InitializeComponent();
        }

        private void TeacherLogView_Load(object sender, EventArgs e)
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
            this.Text = "教師訊息歷程(資料下載中)";
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
            List<MessageLogRecord> messageList = DataManager.GetNoticeList(dateTime1, dateTime2, true);

            if (messageList.Count > 0)
            {
                //取得訊息給多少教師
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
                DataManager.SetTeacherCount(NoticeIDList, messageList);

                //設定訊息數
                SetSendCount(NoticeIDList, messageList);

            }
            else
            {
                e.Cancel = true;
            }

            e.Result = messageList;




        }

        private void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FormLoad = true;
            this.Text = "教師訊息歷程";
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
                        

                        row.Cells[colTeacherCount.Index].Value = record.teacherList.Count; //教師數

                        int countSend = 0;
                        int countRead = 0;

                        foreach (PostTeacher teac in record.CheckTeacherReadDic.Values)
                        {
                            if (teac.IsTeacherRead)
                            {
                                countRead++;
                            }

                            if (!string.IsNullOrEmpty(teac.loginName))
                            {
                                countSend++;
                            }
                        }

                        row.Cells[colSendCount.Index].Value = countSend; //發送數
                        row.Cells[colMessageCount.Index].Value = countRead; //已讀數
                        
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

        /// <summary>
        /// 設定已讀數 & 發送數
        /// </summary>
        private void SetSendCount(List<string> noticeIDList, List<MessageLogRecord> messageList)
        {
            //取得學生記錄
            TeacherDic = GetTeacher(messageList);

            foreach (MessageLogRecord _record in messageList)
            {
                if (_record.teacherList.Count > 0)
                {
                    //建立學生基本資料
                    foreach (string teac_id in _record.teacherList)
                    {
                        if (TeacherDic.ContainsKey(teac_id))
                        {
                            TeacRecord teac = TeacherDic[teac_id];

                            if (!_record.CheckTeacherReadDic.ContainsKey(teac_id))
                            {
                                _record.CheckTeacherReadDic.Add(teac_id, new PostTeacher(teac));

                            }
                        }
                    }

                    //取得教師讀取記錄
                    StringBuilder sb_sql = new StringBuilder();
                    sb_sql.Append("select ref_notice_id,ref_teacher_id,time from $ischool.1campus.notice_log notice_log ");
                    sb_sql.Append(string.Format("where ref_notice_id='{0}' ", _record.notice_id));
                    sb_sql.Append(string.Format("and ref_teacher_id in ('{0}') ", string.Join("','", _record.teacherList)));
                    //老師不是空值
                    sb_sql.Append("and ref_teacher_id is not null");

                    DataTable dt = tool._Q.Select(sb_sql.ToString());

                    foreach (DataRow row in dt.Rows)
                    {
                        string ref_teacher_id = "" + row["ref_teacher_id"];

                        if (_record.CheckTeacherReadDic.ContainsKey(ref_teacher_id))
                        {
                            _record.CheckTeacherReadDic[ref_teacher_id].IsTeacherRead = true;

                            string time = "" + row["time"];
                            _record.CheckTeacherReadDic[ref_teacher_id].IsTeacherReadTime = time;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 取得所有學生物件
        /// </summary>
        private Dictionary<string, TeacRecord> GetTeacher(List<MessageLogRecord> messageList)
        {
            Dictionary<string, TeacRecord> dic = new Dictionary<string, TeacRecord>();
            List<string> TeacherIDList = new List<string>();
            foreach (MessageLogRecord _record in messageList)
            {
                TeacherIDList.AddRange(_record.teacherList);
            }

            List<TeacRecord> studList = DataManager.GetTeacher(TeacherIDList);
            foreach (TeacRecord teac in studList)
            {
                if (!dic.ContainsKey(teac.ID))
                {
                    dic.Add(teac.ID, teac);
                }
            }
            return dic;
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewX1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (e.ColumnIndex <= 3)
                {
                    MessageLogRecord record = (MessageLogRecord)dataGridViewX1.SelectedRows[0].Tag;
                    LogViewForm view = new LogViewForm(record, "teacher");
                    view.ShowDialog();
                }
                else
                {
                    MessageLogRecord record = (MessageLogRecord)dataGridViewX1.SelectedRows[0].Tag;
                    LogTeacherCountForm view = new LogTeacherCountForm(record);
                    view.ShowDialog();
                }
            }
        }
    }
}
