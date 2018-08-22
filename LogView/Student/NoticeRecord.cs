using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1campus.notice.v17
{
    public class NoticeRecord
    {
        public NoticeRecord(DataRow row)
        {
            notice_uid = "" + row["notice_uid"];
            post_time = "" + row["post_time"];
            //read_time = "" + row["read_time"];

            grade_year = "" + row["grade_year"];
            class_name = "" + row["class_name"];
            student_id = "" + row["student_id"];
            seat_no = "" + row["seat_no"];
            student_number = "" + row["student_number"];
            student_name = "" + row["student_name"];
            parent_account = "" + row["parent_account"];
            //relationship = "" + row["relationship"];
            //last_name = "" + row["last_name"];
            //first_name = "" + row["first_name"];
            //parent_id = "" + row["parent_id"];

            //student_mail = "" + row["student_mail"];
        }


        /// <summary>
        /// 訊息系統編號
        /// </summary>
        public string notice_uid { get; set; }
        public string post_time { get; set; }

        public string parent_id { get; set; }

        public string student_mail { get; set; }

        public string grade_year { get; set; }
        public string class_name { get; set; }
        public string student_id { get; set; }
        public string seat_no { get; set; }
        public string student_number { get; set; }
        public string student_name { get; set; }
        public string parent_account { get; set; }
        public string relationship { get; set; }
        public string last_name { get; set; }
        public string first_name { get; set; }

        public string read_time { get; set; }

    }
}
