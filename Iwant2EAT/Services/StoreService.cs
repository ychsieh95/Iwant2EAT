using System;
using System.Collections.Generic;
using System.Linq;

namespace Iwant2EAT.Services
{
    public class StoreService 
    {
        public List<Models.Store> LoadAllStore(string Username = "")
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            var reader = new System.Data.SqlClient.SqlCommand(@"SELECT * FROM Store;", connection).ExecuteReader();

            List<Models.Store> stores = new List<Models.Store>();
            List<Models.Collect> collects = new Services.CollectService().LoadAllCollect();
            List<Models.lReply> replys = new Services.ReplyServices().LoadAllReply();
            while (reader.Read())
            {
                stores.Add(new Models.Store()
                {
                    Name = reader["Name"].ToString(),
                    Branch = reader["Branch"].ToString(),
                    Type = reader["Type"].ToString(),
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
                    Saturday = !reader["DayOff"].ToString().Contains("6;"),

                    IsCollect = !string.IsNullOrEmpty(Username) && (collects.Find(x => x.Username.Equals(Username) && x.Guid.Equals(reader["Guid"].ToString())) != null),
                    CollectCount = collects.FindAll(x => x.Guid.Equals(reader["Guid"].ToString())).Count,
                    IsReply = !string.IsNullOrEmpty(Username) && (replys.Find(x => x.Creater.Equals(Username) && x.StoreGuid.Equals(reader["Guid"].ToString())) != null),
                    ReplyCount = replys.FindAll(x => x.StoreGuid.Equals(reader["Guid"].ToString())).Count
                });
            }
            connection.Close();
            return stores;
        }

        public bool AddStore(Models.Store store)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            if (LoadAllStore().Any(x => x.Name.Equals(store.Name) && x.Branch.Equals(store.Branch)))
            {
                return false;
            }
            else
            {
                return (new System.Data.SqlClient.SqlCommand(string.Format("INSERT INTO Store ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}) VALUES ('{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}');",
                                                                           "Name", "Branch", "Type", "Phone", "DayOff", "OpeningTime", "ClosingTime", "Address", "Introduction", "ImageUrl", "Creater", "Guid",
                                                                           store.Name, store.Branch, store.Type, store.Phone, store.DayOff, store.OpeningTime, store.ClosingTime, store.Address, store.Introduction, store.ImageUrl, store.Creater, store.Guid),
                                                             connection).ExecuteNonQuery() > 0);
            }
        }

        public bool UpdateStore(Models.Store store)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            return (new System.Data.SqlClient.SqlCommand(string.Format("UPDATE Store SET {1}='{11}', {2}='{12}', {3}='{13}', {4}='{14}', {5}='{15}', {6}='{16}', {7}='{17}', {8}='{18}', {9}='{19}', {10}='{20}' WHERE Guid='{0}';",
                                                                       store.Guid,
                                                                       "Name", "Branch", "Type", "Phone", "DayOff", "OpeningTime", "ClosingTime", "Address", "Introduction", "ImageUrl",
                                                                       store.Name, store.Branch, store.Type, store.Phone, store.DayOff, store.OpeningTime, store.ClosingTime, store.Address, store.Introduction, store.ImageUrl),
                                                         connection).ExecuteNonQuery() > 0);

        }

        public bool DeleteStore(string Guid)
        {
            var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=.; Initial Catalog=Iwant2EAT; Integrated Security=True");
            connection.Open();
            return (new System.Data.SqlClient.SqlCommand(string.Format("DELETE FROM Store WHERE Guid='{0}';", Guid),
                                                         connection).ExecuteNonQuery() > 0);
        }
    }
}
