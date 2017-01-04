using System;
using System.Text;
using System.Web;

namespace Iwant2EAT.Models
{
    public class Store
    {
        public string Name { get; set; }
        public string Branch { get; set; }

        public string Type { get; set; }

        public string Phone { get; set; }

        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public string DayOff { get; set; }

        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }

        public string Address { get; set; }

        public string Introduction { get; set; }

        public string ImageUrl { get; set; }

        public string Creater { get; set; }

        public string Guid { get; set; }

        public bool Collect { get; set; }
        public int CollectCount { get; set; }


        public string CheckStoreFormat()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return ("<div class=\"alert alert-danger\" role=\"alert\">[Failure] 店家名稱不可空白！</div>");
            }
            if (StringUTF8Bytes(Name) > 50)
            {
                return ("<div class=\"alert alert-danger\" role=\"alert\">[Failure] 店家名稱不可超過 100 字元（中文字佔 2 字元）！</div>");
            }
            if (StringUTF8Bytes(Branch) > 50)
            {
                return ("<div class=\"alert alert-danger\" role=\"alert\">[Failure] 分店名稱不可超過 100 字元（中文字佔 2 字元）！</div>");
            }
            if (StringUTF8Bytes(Type) > 50)
            {
                return ("<div class=\"alert alert-danger\" role=\"alert\">[Failure] 用餐類別不可超過 100 字元（中文字佔 2 字元）！</div>");
            }
            if (StringUTF8Bytes(Phone) > 50)
            {
                return ("<div class=\"alert alert-danger\" role=\"alert\">[Failure] 電話號碼不可超過 100 字元（中文字佔 2 字元）！</div>");
            }
            if (StringUTF8Bytes(Address) > 100)
            {
                return ("<div class=\"alert alert-danger\" role=\"alert\">[Failure] 店家地址不可超過 100 字元（中文字佔 2 字元）！</div>");
            }
            if (StringUTF8Bytes(Introduction) > 500)
            {
                return ("<div class=\"alert alert-danger\" role=\"alert\">[Failure] 店家介紹不可超過 100 字元（中文字佔 2 字元）！</div>");
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
