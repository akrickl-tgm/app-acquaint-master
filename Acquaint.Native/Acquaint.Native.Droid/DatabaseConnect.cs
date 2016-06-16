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
        MySqlConnection conn = null; 
        // var user = await Auth0.SDK;
        // MySqlConnection conn; 
        //MySqlConnectionStringBuilder constr = new MySqlConnectionStringBuilder(); 

        public bool openConnection()
        {
            new I18N.West.CP1250();
           //build up connection string 
            MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
            conn_string.Server = DBConnectionBase.servername;
            conn_string.UserID = DBConnectionBase.username;
            conn_string.Password = DBConnectionBase.pw;
            conn_string.Database = DBConnectionBase.dbname;
            conn_string.Port = DBConnectionBase.port; 

            //open connection
            conn = new MySqlConnection(conn_string.ToString());
            conn.Open();

            return true; 
        }

        public List<Animal> getAllAnimals()
        {
            if (conn != null)
            {

                returnanimals = new List<Animal>();

                DataSet animals = new DataSet();
                string queryString = "select * from animal;";
                MySqlDataAdapter adapter = new MySqlDataAdapter(queryString, conn);


                DataTable table = new DataTable();
                adapter.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    returnanimals.Add(new Animal()
                    {
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
            return null; 
        }

        public Animal getItem(int id)
        {
            if (conn != null)
            {
                foreach (Animal item in returnanimals)
                {
                    if (item.id == id)
                        return item;
                }
            }
            return new Animal() { name="etwas ist schief gelaufen", kingdom = "ganz ganz schief"};
        }

        public bool addAnimal (Animal ani)
        {
            if (conn != null)
            {
                string query = String.Format("insert into animal " +
                    "(name, kingdom, origin, order, phylum, species, image, family, genus, class, description) "+
                    "values ({1}, {2}, {3}, {4}, {5},{6},{7},{8},{9},{10},{11});",
                    ani.name, ani.kingdom, ani.origin, ani.order, ani.phylum, ani.species, 
                    ani.PhotoURL, ani.family, ani.genus, ani.classe, ani.description);

                MySqlCommand cmd = new MySqlCommand();

                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                return true; 

            }
            return false; 
        }

        public void closeConnection()
        {
            //Close db connection 
            conn.Close();
            conn = null; 
        }
        

    }
}