using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1campus.notice.v17
{
    class ParentRecord
    {

        public ParentRecord(DataRow row)
        {

            ref_notice_id = "" + row["ref_notice_id"];
            read_time = "" + row["read_time"];
            student_name = "" + row["student_name"];
            account = "" + row["account"];
            relationship = "" + row["relationship"];
            ref_parent_id = "" + row["ref_parent_id"];
        }

        //家長 or 學生已讀記錄
        public string ref_notice_id { get; set; }
        public string read_time { get; set; }
        public string student_name { get; set; }
        public string account { get; set; }
        public string relationship { get; set; }
        public string ref_parent_id { get; set; }



    }
}
