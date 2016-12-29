using System;
using System.Collections.Generic;

namespace Iwant2EAT.Service
{
    public class StoreService 
    {
        public List<Models.Store> LoadAllStore()
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            var reader = new System.Data.SqlClient.SqlCommand(@"SELECT * FROM Store", connection).ExecuteReader();

            List<Models.Store> stores = new List<Models.Store>();
            while (reader.Read())
            {
                stores.Add(new Models.Store()
                {
                    PicPath = reader["PicPath"].ToString(),
                    Title = reader["Title"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Address = reader["Address"].ToString(),
                    Introduction = reader["Introduction"].ToString(),
                    OpeningTime = DateTime.Parse(reader["OpeningTime"].ToString()),
                    CloseTime = DateTime.Parse(reader["CloseTime"].ToString()),
                });
            }
            connection.Close();
            return stores;
        }
    }
}
