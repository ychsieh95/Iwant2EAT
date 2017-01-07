using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Iwant2EAT.Models
{
    public class Reply
    {
        public List<lReply> Replys { get; set; }

        public lReply newReply { get; set; }
    }

    public class lReply
    {

        public string Creater { get; set; }
        public string Context { get; set; }

        public string StoreGuid { get; set; }

        public DateTime RecordTime { get; set; }

        public string CheckReplyFormat()
        {
            if (string.IsNullOrWhiteSpace(Context))
            {
                return ("<div class=\"alert alert-danger\" role=\"alert\">[Failure] 留言不可為空！</div>");
            }
            if (Context.Length > 50 || StringUTF8Bytes(Context) > 200)
            {
                return ("<div class=\"alert alert-danger\" role=\"alert\">[Failure] 留言字數過多！</div>");
            }
            if (string.IsNullOrEmpty(StoreGuid))
            {
                return ("<div class=\"alert alert-danger\" role=\"alert\">[Failure] 指定的店家不存在，無法留言！</div>");
            }
            return null;
        }

        /// <summary>
        /// 傳回字串 UTF-8 編碼長度
        /// </summary>
        /// <param name="myString"></param>
        /// <returns></returns>
        public int StringUTF8Bytes(string myString)
        {
            return (string.IsNullOrEmpty(myString) ? 0 : Encoding.GetEncoding("utf-8").GetBytes(myString).Length);
        }
    }
}
