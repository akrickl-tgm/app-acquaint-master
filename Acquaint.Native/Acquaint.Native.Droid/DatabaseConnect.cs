using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using Acquaint.Data;
using System.Data;

namespace Acquaint.Native.Droid
{
    public class DatabaseConnect
    {
        List<Animal> returnanimals; 
        MySqlConnection conn; 
        // var user = await Auth0.SDK;
        // MySqlConnection conn; 
        //MySqlConnectionStringBuilder constr = new MySqlConnectionStringBuilder(); 

        public bool openConnection()
        {
            new I18N.West.CP1250();
           // string constring = "Server= wi-gate.technikum-wien.at;Port=60673;database=lexicom;
             //   User Id=remote;Password=MDHfst4-;charset=utf8";
            // MySqlConnection constr;
            MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
            conn_string.Server = DBConnectionBase.servername;
            conn_string.UserID = DBConnectionBase.username;
            conn_string.Password = DBConnectionBase.pw;
            conn_string.Database = DBConnectionBase.dbname;
            conn_string.Port = DBConnectionBase.port; 

            conn = new MySqlConnection(conn_string.ToString());
            conn.Open();

            return true; 
        }

        public List<Animal> getAllAnimals()
        {
            //MySqlCommand sqlcmd = new MySqlCommand("select name from animal", conn);
            //String result = sqlcmd.ExecuteScalar().ToString();
            //string Text = result + " accounts in DB";
            //return Text;

            returnanimals = new List<Animal>(); 

            DataSet animals = new DataSet();
            string queryString = "select * from animal;";
            MySqlDataAdapter adapter = new MySqlDataAdapter(queryString, conn);
           

            DataTable table = new DataTable();
            adapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                returnanimals.Add(new Animal() {
                    id = Int32.Parse(row["id"].ToString()),
                    name = row["name"].ToString(),
                    kingdom = row["kingdom"].ToString(),
                    phylum = row["phylum"].ToString(),
                    classe = row["class"].ToString(),
                    order = row["order"].ToString(),
                    family = row["family"].ToString(),
                    genus = row["genus"].ToString(),
                    species = row["species"].ToString(),
                    origin = row["origin"].ToString(),
                    description = row["description"].ToString(),
                    PhotoURL = row["image"].ToString()
                });
            }

            return returnanimals; 

        }

        public Animal getItem(int id)
        {      
            foreach(Animal item in returnanimals)
            {
                if (item.id == id)
                    return item;  
            }
            return new Animal() { name="etwas ist schief gelaufen", kingdom = "ganz ganz schief"};
            //return returnanimals
        }

        public void closeConnection()
        {
            conn.Close(); 
        }
        

    }
}