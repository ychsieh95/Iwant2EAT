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
            var reader = new System.Data.SqlClient.SqlCommand(@"SELECT * FROM Store;", connection).ExecuteReader();

            List<Models.Store> stores = new List<Models.Store>();
            while (reader.Read())
            {
                stores.Add(new Models.Store()
                {
                    Name = reader["Name"].ToString(),
                    Branch = reader["Branch"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    DayOff = reader["DayOff"].ToString(),
                    OpeningTime = (TimeSpan)reader["Openingtime"],
                    ClosingTime = (TimeSpan)reader["ClosingTime"],
                    Address = reader["Address"].ToString(),
                    Introduction = reader["Introduction"].ToString(),
                    ImageUrl = reader["ImageUrl"].ToString(),
                    Creater = reader["Creater"].ToString(),
                    Guid = reader["Guid"].ToString(),
                    Sunday = !reader["DayOff"].ToString().Contains("0;"),
                    Monday = !reader["DayOff"].ToString().Contains("1;"),
                    Tuesday = !reader["DayOff"].ToString().Contains("2;"),
                    Wednesday = !reader["DayOff"].ToString().Contains("3;"),
                    Thursday = !reader["DayOff"].ToString().Contains("4;"),
                    Friday = !reader["DayOff"].ToString().Contains("5;"),
                    Saturday = !reader["DayOff"].ToString().Contains("6;")
                });
            }
            connection.Close();
            return stores;
        }

        public bool AddStore(Models.Store store)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            return (new System.Data.SqlClient.SqlCommand(string.Format("INSERT INTO Store ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}) VALUES ('{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}');",
                                                                       "Name", "Branch", "Phone", "DayOff", "OpeningTime", "ClosingTime", "Address", "Introduction", "ImageUrl", "Creater", "Guid",
                                                                       store.Name, store.Branch, store.Phone, store.DayOff, store.OpeningTime, store.ClosingTime, store.Address, store.Introduction, store.ImageUrl, store.Creater, store.Guid),
                                                         connection).ExecuteNonQuery() > 0);

        }

        public bool UpdateStore(Models.Store store)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            return (new System.Data.SqlClient.SqlCommand(string.Format("UPDATE Store SET {0}='{10}', {1}='{11}', {2}='{12}', {3}='{13}', {4}='{14}', {5}='{15}', {6}='{16}', {7}='{17}', {8}='{18}' WHERE Guid='{9}';",
                                                                       "Name", "Branch", "Phone", "DayOff", "OpeningTime", "ClosingTime", "Address", "Introduction", "ImageUrl",
                                                                       store.Guid, store.Name, store.Branch, store.Phone, store.DayOff, store.OpeningTime, store.ClosingTime, store.Address, store.Introduction, store.ImageUrl),
                                                         connection).ExecuteNonQuery() > 0);

        }

        public bool DeleteStore(string storeGuid)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            return (new System.Data.SqlClient.SqlCommand(string.Format("DELETE FROM Store WHERE Guid='{0}';", storeGuid),
                                                         connection).ExecuteNonQuery() > 0);

        }
    }
}
