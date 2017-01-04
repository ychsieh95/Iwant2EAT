using System;
using System.Collections.Generic;
using System.Web;

namespace Iwant2EAT.Services
{
    public class MemberService
    {
        public List<Models.Member> LoadAllMember()
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            var reader = new System.Data.SqlClient.SqlCommand(@"SELECT * FROM Member;", connection).ExecuteReader();

            List<Models.Member> members = new List<Models.Member>();
            while (reader.Read())
            {
                members.Add(new Models.Member()
                {
                    Username = reader["Username"].ToString(),
                    Password = reader["Password"].ToString(),
                    Email = reader["Email"].ToString(),
                    LastLogin =(DateTime)reader["LastLogin"],
                    LastIpAdr = reader["LastIpAdr"].ToString()
                });
            }
            connection.Close();
            return members;
        }

        public bool AddMember(Models.Member member)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();

            if (LoadAllMember().FindAll(x => x.Username.Equals(member.Username)).Count > 0)
            {
                return false;
            }
            else
            {
                return (new System.Data.SqlClient.SqlCommand(string.Format("INSERT INTO Member (Username, Password, Email, LastLogin, LastIpAdr) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');",
                                                                           member.Username, member.Password, member.Email, member.LastLogin.ToString("yyyy/MM/dd HH:mm:ss"), member.LastIpAdr), connection).ExecuteNonQuery() > 0);
            }
        }

        public bool UpdateMember(string setCommand, string whereCommand)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            return (new System.Data.SqlClient.SqlCommand(string.Format("UPDATE Member SET {0} WHERE {1};", setCommand, whereCommand), connection).ExecuteNonQuery() > 0);
        }

        public bool DeleteMember(string username)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            return (new System.Data.SqlClient.SqlCommand(string.Format("DELETE FROM Member WHERE Username='{0}';", username), connection).ExecuteNonQuery() > 0);
        }
    }
}
