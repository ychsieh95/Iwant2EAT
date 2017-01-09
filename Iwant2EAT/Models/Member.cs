using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Iwant2EAT.Models
{
    public class Member 
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public DateTime LastLogin { get; set; }
        public string LastIpAdr { get; set; }

        public string Rurl { get; set; }

        /// <summary>
        /// 檢查是否為正確的 Member 格式，正確則回傳 null，錯誤則回傳 HTML 標籤
        /// </summary>
        /// <returns></returns>
        public string CheckMemberFormat()
        {
            if (!(IsNumandEG(Username) && IsNumandEG(Password)))
            {
                return ("<div class=\"alert alert-danger\" role=\"alert\">[Failure] 帳號或密碼僅能由 A-Z, a-z, 0-9 組合！</div>");
            }
            if (Username.Length > 50)
            {
                return ("<div class=\"alert alert-danger\" role=\"alert\">[Failure] 帳號長度上限為 50 個字元！</div>");
            }
            if (Password.Length > 50)
            {
                return ("<div class=\"alert alert-danger\" role=\"alert\">[Failure] 密碼長度上限為 50 個字元！</div>");
            }
            if (!string.IsNullOrEmpty(Email) && Email.Length > 50)
            {
                return ("<div class=\"alert alert-danger\" role=\"alert\">[Failure] 信箱長度上限為 50 個字元！</div>");
            }
            return null;
        }

        /// <summary>
        /// 檢查字串是否僅由英文與數字組成
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool IsNumandEG(string word)
        {
            Regex NumandEG = new Regex("[^A-Za-z0-9]");
            return !NumandEG.IsMatch(word);
        }
    }
}
