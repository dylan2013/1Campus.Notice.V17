using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1campus.notice.v17
{
    static class DataManager
    {

        /// <summary>
        /// 取得目前簡訊歷程
        /// 
        /// </summary>
        static public List<MessageLogRecord> GetNoticeList(string dateTime1, string dateTime2, bool IsTeacher)
        {
            List<MessageLogRecord> list = new List<MessageLogRecord>();

            StringBuilder sb_sql = new StringBuilder();
            sb_sql.Append("select notice.uid as notice_id,notice.message,notice.display_sender,notice.post_time,");
            sb_sql.Append("notice.title,notice.parent_visible,notice.student_visible ");
            sb_sql.Append("from $ischool.1campus.notice notice ");

            //是否為老師訊息
            if (IsTeacher)
            {
                sb_sql.Append("join (select notice.uid as notice_id ");
                sb_sql.Append("from $ischool.1campus.notice notice ");
                sb_sql.Append("left join $ischool.1campus.notice_target notice_target on notice.uid=notice_target.ref_notice_id ");
                sb_sql.Append("where notice_target.ref_teacher_id is not null ");
                sb_sql.Append("group by notice.uid ) teacher_notice on teacher_notice.notice_id=notice.uid ");

                sb_sql.Append("where notice.post_time >='{0}' and notice.post_time <='{1}'");
            }
            else
            {
                sb_sql.Append("where not notice.uid in (select notice.uid as notice_id ");
                sb_sql.Append("from $ischool.1campus.notice notice  ");
                sb_sql.Append("left join $ischool.1campus.notice_target notice_target on notice.uid=notice_target.ref_notice_id ");
                sb_sql.Append("where notice_target.ref_teacher_id is not null ");
                sb_sql.Append("group by notice.uid  ");
                sb_sql.Append(") and  notice.post_time >='{0}' and notice.post_time <='{1}'");
            }

            sb_sql.Append("order by notice.post_time desc");

            DataTable dt = tool._Q.Select(string.Format(sb_sql.ToString(), dateTime1, dateTime2));
            foreach (DataRow row in dt.Rows)
            {
                MessageLogRecord mesgR = new MessageLogRecord(row);

                list.Add(mesgR);
            }

            return list;
        }

        /// <summary>
        /// 統計學生數
        /// </summary>
        static public void SetStudentCount(List<string> noticeIDList, List<MessageLogRecord> messageList)
        {
            Dictionary<string, MessageLogRecord> MessageDic = new Dictionary<string, MessageLogRecord>();
            foreach (MessageLogRecord log in messageList)
            {
                if (!MessageDic.ContainsKey(log.notice_id))
                {
                    MessageDic.Add(log.notice_id, log);
                }
            }

            StringBuilder sb_sql = new StringBuilder();
            sb_sql.Append("select notice_target.ref_notice_id,student.id as student_id,student.name as student_name,");
            sb_sql.Append("student_parent.id as parent_id,student_parent.relationship,student_parent.account ");
            sb_sql.Append("from $ischool.1campus.notice_target notice_target ");
            sb_sql.Append("left join student on student.id= notice_target.ref_student_id ");
            sb_sql.Append("left join student_parent on student.id=student_parent.ref_student_id ");
            sb_sql.Append(string.Format("where notice_target.ref_notice_id in ('{0}') and notice_target.ref_teacher_id is null  and notice_target.ref_student_id is not null ", string.Join("','", noticeIDList)));

            DataTable dt = tool._Q.Select(sb_sql.ToString());
            foreach (DataRow row in dt.Rows)
            {
                string notice_id = "" + row["ref_notice_id"];
                string student_id = "" + row["student_id"];
                string parent_id = "" + row["parent_id"];

                if (MessageDic.ContainsKey(notice_id))
                {
                    MessageLogRecord log = MessageDic[notice_id];
                    if (!log.studentList.Contains(student_id))
                    {
                        log.studentList.Add(student_id);
                    }

                    if (parent_id != "")
                    {
                        if (!log.parentList.Contains(parent_id))
                        {
                            log.parentList.Add(parent_id);
                        }
                    }
                }
            }
        }

        public static void SetTeacherCount(List<string> noticeIDList, List<MessageLogRecord> messageList)
        {
            Dictionary<string, MessageLogRecord> MessageDic = new Dictionary<string, MessageLogRecord>();
            foreach (MessageLogRecord log in messageList)
            {
                if (!MessageDic.ContainsKey(log.notice_id))
                {
                    MessageDic.Add(log.notice_id, log);
                }
            }

            StringBuilder sb_sql = new StringBuilder();
            sb_sql.Append("select notice_target.ref_notice_id,teacher.id as teacher_id,teacher.teacher_name ");
            sb_sql.Append("from $ischool.1campus.notice_target notice_target ");
            sb_sql.Append("left join teacher on teacher.id= notice_target.ref_teacher_id ");
            sb_sql.Append(string.Format("where notice_target.ref_notice_id in ('{0}') and notice_target.ref_teacher_id is not null  and notice_target.ref_student_id is null ", string.Join("','", noticeIDList)));

            DataTable dt = tool._Q.Select(sb_sql.ToString());
            foreach (DataRow row in dt.Rows)
            {
                string notice_id = "" + row["ref_notice_id"];
                string student_id = "" + row["teacher_id"];

                if (MessageDic.ContainsKey(notice_id))
                {
                    MessageLogRecord log = MessageDic[notice_id];
                    if (!log.teacherList.Contains(student_id))
                    {
                        log.teacherList.Add(student_id);
                    }
                }
            }

        }

        public static string TotalCount()
        {

            StringBuilder sb_sql = new StringBuilder();
            sb_sql.Append("select count(*) from ( select ref_notice_id,ref_student_id,ref_parent_id,ref_teacher_id ");
            sb_sql.Append("from $ischool.1campus.notice_log notice_log ");
            sb_sql.Append("group by ref_notice_id,ref_student_id,ref_parent_id,ref_teacher_id ) total");

            DataTable dt = tool._Q.Select(sb_sql.ToString());
            return "" + dt.Rows[0]["count"];

        }

        /// <summary>
        /// 取得目前已讀狀態資料
        /// </summary>
        static public List<ParentRecord> GetLogRead(string notice_id)
        {
            List<ParentRecord> list = new List<ParentRecord>();
            StringBuilder sb_sql = new StringBuilder();
            sb_sql.Append("select notice_log.ref_notice_id,notice_log.time as read_time,student.name as student_name,");
            sb_sql.Append("student_parent.account,student_parent.relationship,ref_parent_id ");
            sb_sql.Append("from $ischool.1campus.notice_log notice_log ");
            sb_sql.Append("join $ischool.1campus.notice_target target on target.ref_notice_id=notice_log.ref_notice_id ");
            sb_sql.Append("join student on student.id=target.ref_student_id ");
            sb_sql.Append("join student_parent on student_parent.ref_student_id=student.id ");
            sb_sql.Append(string.Format("where notice_log.ref_notice_id={0} ", notice_id));
            sb_sql.Append("order by notice_log.time desc");

            DataTable dt = tool._Q.Select(sb_sql.ToString());

            foreach (DataRow row in dt.Rows)
            {
                ParentRecord mesgR = new ParentRecord(row);

                list.Add(mesgR);
            }
            return list;
        }

        /// <summary>
        /// 取得目前系統之簡訊歷程
        /// </summary>
        static public List<string> GetNoticeIDList()
        {
            List<string> list = new List<string>();

            StringBuilder sb_sql = new StringBuilder();
            //取得的訊息記錄不要有老師編號
            sb_sql.Append("select uid from $ischool.1campus.notice WHERE ref_teacher_id is null");

            DataTable dt = tool._Q.Select(sb_sql.ToString());

            foreach (DataRow row in dt.Rows)
            {
                string uid = "" + row["uid"];
                list.Add(uid);
            }

            return list;
        }

        /// <summary>
        /// 依清單取得訊息資料
        /// </summary>
        static public List<NoticeRecord> GetNotices(List<string> noticeIDList)
        {
            StringBuilder sb_sql = new StringBuilder();
            sb_sql.Append("SELECT DISTINCT notice_uid,post_time,grade_year,class_name,student_id,seat_no,student_number,student_name,parent_account,parent_id FROM( ");
            sb_sql.Append("SELECT notice.uid as notice_uid,notice.post_time,notice_log.time as read_time,class.grade_year, class.class_name,student.id as student_id, student.seat_no,");
            sb_sql.Append("student.student_number, student.name as student_name,student_parent.account as parent_account, student_parent.relationship, student_parent.last_name,");
            sb_sql.Append("student_parent.first_name,student_parent.id as parent_id,notice_log.time,student.email as student_mail,parent_read.parent_read FROM  $ischool.1campus.notice notice ");
            sb_sql.Append("LEFT OUTER JOIN ");
            sb_sql.Append("(");
            sb_sql.Append("SELECT DISTINCT union_list.ref_student_id, union_list.ref_notice_id FROM ");
            sb_sql.Append("(");
            sb_sql.Append("SELECT student.id as ref_student_id,notice_target.ref_notice_id FROM $ischool.1campus.notice_target notice_target ");
            sb_sql.Append("LEFT OUTER JOIN ");
            sb_sql.Append("(");
            sb_sql.Append("SELECT 'group' as kind, groupx.uid as uid, sg_attend.seat_no as seat_no, sg_attend.ref_student_id as ref_student_id FROM $group groupx JOIN $sg_attend sg_attend on sg_attend.ref_group_id = groupx.uid ");
            sb_sql.Append("UNION ALL ");
            sb_sql.Append("SELECT 'class' as kind, class.group_id as uid, student.seat_no as seat_no, student.id as ref_student_id FROM class join student on student.ref_class_id = class.id ");
            sb_sql.Append("UNION ALL ");
            sb_sql.Append("SELECT 'course' as kind, course.group_id as uid, null as seat_no, sc_attend.ref_student_id as ref_student_id FROM course join sc_attend on sc_attend.ref_course_id = course.id ");
            sb_sql.Append(") as groupx on groupx.uid=notice_target.ref_group_id ");
            sb_sql.Append("LEFT OUTER JOIN ");
            sb_sql.Append("student on student.id = groupx.ref_student_id WHERE student.id is not null AND student.status in  (1 ,2) ");
            sb_sql.Append("UNION ALL SELECT notice_target.ref_student_id as ref_student_id, notice_target.ref_notice_id FROM $ischool.1campus.notice_target notice_target ");
            sb_sql.Append("WHERE notice_target.ref_student_id is not null ");
            sb_sql.Append(") as union_list ");
            sb_sql.Append(") as student_list on student_list.ref_notice_id=notice.uid ");
            sb_sql.Append("LEFT OUTER JOIN student on student.id = student_list.ref_student_id ");
            sb_sql.Append("LEFT OUTER JOIN class on class.id = student.ref_class_id ");
            sb_sql.Append("LEFT OUTER JOIN $ischool.1campus.notice_log notice_log on notice_log.ref_notice_id = notice.uid AND notice_log.ref_student_id = student.id ");
            sb_sql.Append("LEFT OUTER JOIN student_parent on student_parent.id = notice_log.ref_parent_id ");
            //=========讀取數的資料整理====parent_read有值時,會有ID資料====
            sb_sql.Append("LEFT OUTER JOIN ");
            sb_sql.Append("(select notice.uid,student.id as student_id,student_parent.id as parent_read ");
            sb_sql.Append("from $ischool.1campus.notice notice ");
            sb_sql.Append("join $ischool.1campus.notice_target target on target.ref_notice_id=notice.uid ");
            sb_sql.Append("join student on student.id=target.ref_student_id ");
            sb_sql.Append("join student_parent on student_parent.ref_student_id=student.id ");
            sb_sql.Append("join $ischool.1campus.notice_log notice_log on notice_log.ref_notice_id=notice.uid ");
            sb_sql.Append("group by notice.uid,student.id,student_parent.id) parent_read on notice.uid=parent_read.uid ");
            //==============
            sb_sql.Append("WHERE notice.uid in ('{0}') ");
            sb_sql.Append("ORDER BY class.grade_year, class.display_order, class.class_name, student.seat_no, student.id, notice_log.time desc ");
            sb_sql.Append(") as all_table");
            List<NoticeRecord> list = new List<NoticeRecord>();
            DataTable dt = tool._Q.Select(string.Format(sb_sql.ToString(), string.Join("','", noticeIDList)));

            foreach (DataRow row in dt.Rows)
            {
                NoticeRecord notice = new NoticeRecord(row);
                list.Add(notice);
            }

            return list;
        }

        static public List<StudRecord> GetStudent(List<string> studentIDList)
        {
            List<StudRecord> list = new List<StudRecord>();

            StringBuilder sb_sql = new StringBuilder();
            sb_sql.Append("select student.id,student.name,student.seat_no,class.class_name,sa_login_name ");
            sb_sql.Append("from student left join class on class.id=student.ref_class_id ");
            sb_sql.Append(string.Format("where student.id in ('{0}')", string.Join("','", studentIDList)));

            DataTable dt = tool._Q.Select(sb_sql.ToString());
            foreach (DataRow each in dt.Rows)
            {
                StudRecord stud = new StudRecord(each);
                list.Add(stud);

            }

            return list;
        }

        static public List<TeacRecord> GetTeacher(List<string> teacherIDList)
        {
            List<TeacRecord> list = new List<TeacRecord>();

            StringBuilder sb_sql = new StringBuilder();
            sb_sql.Append("select teacher.id,teacher.teacher_name,teacher.st_login_name ");
            sb_sql.Append("from teacher ");
            sb_sql.Append(string.Format("where teacher.id in ('{0}')", string.Join("','", teacherIDList)));

            DataTable dt = tool._Q.Select(sb_sql.ToString());
            foreach (DataRow each in dt.Rows)
            {
                TeacRecord teac = new TeacRecord(each);
                list.Add(teac);
            }
            return list;
        }

        static public List<string> GetDisPlaySender()
        {
            List<string> list = new List<string>();

            StringBuilder sb_sql = new StringBuilder();
            sb_sql.Append("select display_sender from $ischool.1campus.notice ");
            sb_sql.Append("group by display_sender ");
            sb_sql.Append("order by display_sender");

            DataTable dt = tool._Q.Select(sb_sql.ToString());

            foreach (DataRow row in dt.Rows)
            {
                if (!list.Contains("" + row["display_sender"]))
                {
                    list.Add("" + row["display_sender"]);
                }
            }

            return list;
        }
    }
}
