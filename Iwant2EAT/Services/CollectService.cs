using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iwant2EAT.Services
{
    public class CollectService
    {
        public List<Models.Collect> LoadAllCollect()
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            var reader = new System.Data.SqlClient.SqlCommand(@"SELECT * FROM Collect;", connection).ExecuteReader();

            List<Models.Collect> likes = new List<Models.Collect>();
            while (reader.Read())
            {
                likes.Add(new Models.Collect()
                {
                    Username = reader["Username"].ToString(),
                    Guid = reader["Guid"].ToString()
                });
            }
            connection.Close();
            return likes;
        }

        public bool AddCollect(Models.Collect collect)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            if (LoadAllCollect().Any(x => x.Username.Equals(collect.Username) && x.Guid.Equals(collect.Guid)))
            {
                return false;
            }
            else
            {
                return (new System.Data.SqlClient.SqlCommand(string.Format("INSERT INTO Collect (Username, Guid) VALUES ('{0}', '{1}');",
                                                                           collect.Username, collect.Guid),
                                                             connection).ExecuteNonQuery() > 0);
            }
        }

        public bool DeleteCollect(Models.Collect collect)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            return (new System.Data.SqlClient.SqlCommand(string.Format("DELETE FROM Collect WHERE Username='{0}' AND Guid='{1}';",
                                                                       collect.Username, collect.Guid),
                                                         connection).ExecuteNonQuery() > 0);
        }
    }
}
