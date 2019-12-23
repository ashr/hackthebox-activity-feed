using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace hackthebox_activity_feed
{
    public class HackTheBoxDatabase{
        public static bool Update(List<ActivityModel> activities){
            bool success = false;

            if (!DatabaseCreated()){
                CreateDatabase();
            }

            foreach(ActivityModel activity in activities){
                if (!ContentIsAlreadyInDatabase(activity)){
                    InsertActivity(activity);
                }
            }

            return success;
        }

        public static string CreateSHA256Hash(ActivityModel activity){
            byte[] hashBytes = System.Security.Cryptography.SHA256.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(activity.content));
            StringBuilder builder = new StringBuilder();  
            for (int i = 0; i < hashBytes.Length; i++)  
            {  
                builder.Append(hashBytes[i].ToString("x2"));  
            }
            return builder.ToString();    
        }

        private static bool InsertActivity(ActivityModel activity){
            //Console.Write("InsertActivity:");
            bool ret = false;
            using (var connection = GetConnection()){
                activity.activityHash = CreateSHA256Hash(activity);

                connection.Open();
                
                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = "INSERT INTO htb_activity(activityId,authorName,authorPicUrl,content,postDate,contenthash)" +
                    "VALUES ('" +
                        activity.activityId.Replace("'","''").Replace("\"","\"\"") + "','" +
                        activity.authorName.Replace("'","''").Replace("\"","\"\"") + "','" +
                        activity.authorPicUrl.Replace("'","''").Replace("\"","\"\"") + "','" +
                        activity.content.Replace("'","''").Replace("\"","\"\"") + "','" +
                        activity.postDate.Replace("'","''").Replace("\"","\"\"") + "','" +
                        activity.activityHash.Replace("'","''").Replace("\"","\"\"") + "'" +
                    ");";

                insertCmd.ExecuteNonQuery();
                ret = true;
                connection.Close();
            }
            //Console.WriteLine(ret);

            Console.WriteLine("[+] " + activity.content);
            return ret;
        }

        private static bool ContentIsAlreadyInDatabase(ActivityModel activity){
            //Console.Write("ContentIsAlreadyInDatabase:");
            bool ret = false;
            using (var connection = GetConnection()){
                activity.activityHash = CreateSHA256Hash(activity);

                connection.Open();
                
                var checkHashCmd = connection.CreateCommand();
                checkHashCmd.CommandText = "SELECT count(*) FROM htb_activity WHERE contenthash = '" + activity.activityHash + "';";
                var reader = checkHashCmd.ExecuteReader();
                reader.Read();
                if (reader[0].ToString() == "0"){
                    ret = false;
                }
                else{
                    ret = true;
                }

                connection.Close();
            }
            //Console.WriteLine(ret);
            return ret;
        }

        private static bool CreateDatabase(){
            Console.Write("Creating database:");
            bool ret = false;
            using (var connection = GetConnection()){
                connection.Open();
                
                var delTableCmd = connection.CreateCommand();
                delTableCmd.CommandText = "DROP TABLE IF EXISTS htb_activity";
                delTableCmd.ExecuteNonQuery();

                var crTableCmd = connection.CreateCommand();
                crTableCmd.CommandText = "CREATE TABLE htb_activity (activityId VARCHAR(500), authorName VARCHAR(500), authorPicURL VARCHAR(500), content VARCHAR(500), postDate VARCHAR(500), contenthash VARCHAR(64));";
                crTableCmd.ExecuteNonQuery();

                connection.Close();
                ret = true;
            }
            Console.WriteLine(ret);
            return ret;
        }

        private static bool DatabaseCreated(){
            Console.Write("Checking if database exists:");
            bool ret = false;

            using (var connection = GetConnection()){
                connection.Open();

                var checkTableCmd = connection.CreateCommand();
                checkTableCmd.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'htb_activity';";
                var reader = checkTableCmd.ExecuteReader();
                if (reader.HasRows && reader.Read()){
                    if (reader[0].ToString() == "htb_activity"){
                        ret = true;
                    }
                }

                connection.Close();
            }
            Console.WriteLine(ret);
            return ret;
        }

        private static SqliteConnection GetConnection(){
            return new SqliteConnection("Data Source=./hackthebox.db;");
        }
    }
}