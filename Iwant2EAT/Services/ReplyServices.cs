using System;
using System.Collections.Generic;
using System.Web;

namespace Iwant2EAT.Services
{
    public class ReplyServices
    {
        public List<Models.lReply> LoadAllReply()
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            var reader = new System.Data.SqlClient.SqlCommand(@"SELECT * FROM Reply ORDER BY RecordTime DESC;", connection).ExecuteReader();

            List<Models.lReply> reply = new List<Models.lReply>();
            while (reader.Read())
            {
                reply.Add(new Models.lReply()
                {
                    Creater = reader["Creater"].ToString(),
                    Context = reader["Context"].ToString(),
                    StoreGuid = reader["StoreGuid"].ToString(),
                    RecordTime = (DateTime)reader["RecordTime"]
                });
            }
            connection.Close();
            return reply;
        }

        public bool AddReply(Models.lReply reply)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            return (new System.Data.SqlClient.SqlCommand(string.Format("INSERT INTO Reply (Creater, Context, StoreGuid, Recordtime) VALUES ('{0}', '{1}', '{2}', '{3}');",
                                                                       reply.Creater, reply.Context, reply.StoreGuid, reply.RecordTime.ToString("yyyy/MM/dd hh:mm:ss")),
                                                         connection).ExecuteNonQuery() > 0);
        }

        public bool UpdateReply(Models.lReply newReply, Models.lReply oldReply)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            return (new System.Data.SqlClient.SqlCommand(string.Format("UPDATE Reply SET Context='{0}', RecordTime='{1}' WHERE Creater='{2}' AND Context='{3}' AND StoreGuid='{4}' AND RecordTime='{5}';",
                                                                       newReply.Context, newReply.RecordTime.ToString("yyyy/MM/dd hh:mm:ss"),
                                                                       oldReply.Creater, oldReply.Context, oldReply.StoreGuid, oldReply.RecordTime.ToString("yyyy/MM/dd hh:mm:ss")),
                                                         connection).ExecuteNonQuery() > 0);
        }

        public bool DeleteReply(Models.lReply reply)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            return (new System.Data.SqlClient.SqlCommand(string.Format("DELETE FROM Reply WHERE Creater='{0}' AND Context='{1}' AND StoreGuid='{2}' AND RecordTime='{3}';",
                                                                       reply.Creater, reply.Context, reply.StoreGuid, reply.RecordTime.ToString("yyyy/MM/dd hh:mm:ss")),
                                                         connection).ExecuteNonQuery() > 0);
        }
    }
}
