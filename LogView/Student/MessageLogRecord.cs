using K12.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1campus.notice.v17
{
    public class MessageLogRecord
    {
        public MessageLogRecord(DataRow row)
        {
            notice_id = "" + row["notice_id"];
            display_sender = "" + row["display_sender"];
            message = "" + row["message"];
            post_time = "" + row["post_time"];
            title = "" + row["title"];
            parent_visible = "" + row["parent_visible"];
            student_visible = "" + row["student_visible"];

            studentList = new List<string>();
            parentList = new List<string>();
            teacherList = new List<string>();

            //學生ID：學生讀取資訊
            CheckStudentReadDic = new Dictionary<string, PostStudent>();

            //學生ID：家長ID：家長物件
            CheckParentReadDic = new Dictionary<string, Dictionary<string, PostParent>>();

            //教師ID：教師物件
            CheckTeacherReadDic = new Dictionary<string, PostTeacher>();
        }

        /// <summary>
        /// 訊息系統編號
        /// </summary>
        public string notice_id { get; set; }

        /// <summary>
        /// 發送日期
        /// </summary>
        public string post_time { get; set; }

        /// <summary>
        /// 發送單位
        /// </summary>
        public string display_sender { get; set; }

        ///// <summary>
        ///// 全域訊息
        ///// </summary>
        //public string global_notice { get; set; }

        /// <summary>
        /// 訊息內容
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 家長是否可見
        /// </summary>
        public string parent_visible { get; set; }

        /// <summary>
        /// 學生是否可以見
        /// </summary>
        public string student_visible { get; set; }

        /// <summary>
        /// 學生系統編號清單
        /// </summary>
        public List<string> studentList { get; set; }

        /// <summary>
        /// 家長系統編號清單
        /// </summary>
        public List<string> parentList { get; set; }

        /// <summary>
        /// 老師系統編號清單
        /// </summary>
        public List<string> teacherList { get; set; }

        //教師檢查點
        public Dictionary<string, PostTeacher> CheckTeacherReadDic { get; set; }

        //學生檢查點
        public Dictionary<string, PostStudent> CheckStudentReadDic { get; set; }

        //學生的家長檢查點
        public Dictionary<string, Dictionary<string, PostParent>> CheckParentReadDic { get; set; }

    }

    public class PostStudent
    {
        public PostStudent(StudRecord stud)
        {
            IsStudentRead = false;

            _stud = stud;
            className = stud.ClassName;
            seatNo = stud.SeatNo;
            StudentName = stud.StudentName;
            SALoginName = stud.loginName;
        }

        public StudRecord _stud { get; set; }

        public string className { get; set; }
        public string seatNo { get; set; }
        public string StudentName { get; set; }
        public string ref_student_id { get; set; }
        public bool IsStudentRead { get; set; }
        public string IsStudentReadTime { get; set; }
        public string SALoginName { get; set; }
    }

    public class PostParent
    {

        public PostParent(DataRow row, StudRecord stud)
        {
            parent_id = "" + row["id"];
            account = "" + row["account"];
            ref_student_id = "" + row["ref_student_id"];
            relationship = "" + row["relationship"];

            _stud = stud;
            className = stud.ClassName;
            seatNo = stud.SeatNo;
            StudentName = stud.StudentName;
        }

        public StudRecord _stud { get; set; }

        public string className { get; set; }
        public string seatNo { get; set; }
        public string StudentName { get; set; }

        public string parent_id { get; set; }

        public bool IsParentRead { get; set; } //家長

        public string IsParentReadTime { get; set; } //已讀時間

        //家長帳號
        public string account { get; set; }
        public string ref_student_id { get; set; }
        public string relationship { get; set; }
    }

    public class PostTeacher
    {
        public PostTeacher(TeacRecord teac)
        {
            id = teac.ID;
            loginName = teac.loginName;
            TeacherName = teac.TeacherName;

        }

        public bool IsTeacherRead { get; set; } //老師已讀

        public string IsTeacherReadTime { get; set; } //已讀時間

        public string id { get; set; }

        public string loginName { get; set; }

        public string TeacherName { get; set; }



    }

    public class StudRecord
    {
        public StudRecord(DataRow each)
        {
            ID = "" + each["id"];
            StudentName = "" + each["name"];
            ClassName = "" + each["class_name"];
            SeatNo = "" + each["seat_no"];
            loginName = "" + each["sa_login_name"];
        }
        public string ID { get; set; }
        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public string SeatNo { get; set; }
        public string loginName { get; set; }
    }

    public class TeacRecord
    {
        public TeacRecord(DataRow each)
        {
            ID = "" + each["id"];
            TeacherName = "" + each["teacher_name"];
            loginName = "" + each["st_login_name"];
        }
        public string ID { get; set; }
        public string TeacherName { get; set; }
        public string loginName { get; set; }
        
    }
}
