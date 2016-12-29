using System;
using System.Collections.Generic;
using System.Web;

namespace Iwant2EAT.Service
{
    public class MemberService
    {
        public List<Models.Member> LoadAllMember()
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            var reader = new System.Data.SqlClient.SqlCommand(@"SELECT * FROM Member", connection).ExecuteReader();

            List<Models.Member> members = new List<Models.Member>();
            while (reader.Read())
            {
                members.Add(new Models.Member()
                {
                    Username = reader["Username"].ToString(),
                    Password = reader["Password"].ToString(),
                    Email = reader["Email"].ToString()
                });
            }
            connection.Close();
            return members;
        }

        public bool AddMember(Models.Member member)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            return (new System.Data.SqlClient.SqlCommand(string.Format("INSERT INTO Member (Username, Password, Email) VALUES ('{0}', '{1}', '{2}')", member.Username, member.Password, member.Email), connection).ExecuteNonQuery() > 0);
        }

        public bool ChangePassword(string username, string password)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            return (new System.Data.SqlClient.SqlCommand(string.Format("UPDATE Member SET Password='{0}' WHERE Username='{1}'", password, username), connection).ExecuteNonQuery() > 0);
        }

        public bool ChangeEmail(string username, string email)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            return (new System.Data.SqlClient.SqlCommand(string.Format("UPDATE Member SET Email='{0}' WHERE Username='{1}'", email, username), connection).ExecuteNonQuery() > 0);
        }

        public bool DeleteMember(string username)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            return (new System.Data.SqlClient.SqlCommand(string.Format("DELETE FROM Member WHERE Username='{0}'", username), connection).ExecuteNonQuery() > 0);
        }
    }
}
