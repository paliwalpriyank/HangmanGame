using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ServiceModel;
using System.Runtime.Serialization;
using System.IO;
using System.Data.OleDb;
using System.Xml;


namespace MyRESTService
{
    [DataContract(Namespace="")]
    public class Product
    {
        [DataMember]
        public int MovieId { get; set; }
        [DataMember]
        public string FilmName { get; set; }
        [DataMember]
        public string Director { get; set; }
        [DataMember]
        public string LeadingActors { get; set; }
        [DataMember]
        public int ReleaseYear { get; set; }
        [DataMember]
        public int OscarsWon { get; set; }
        [DataMember]
        public string IMDBLink { get; set; }
        [DataMember]
        public string Country { get; set; }
    }
    [DataContract(Namespace = "")]
    public class IndividualScore
    {        
        [DataMember]
        public string score { get; set; }  
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public string user { get; set; }  
    }

    public partial class Products
    {
       private static readonly Products _instance = new Products();
       
       private Products() { } 
       
       public static Products Instance 
       { 
                get { return _instance; } 
       } 
        public List<Product> ProductList(string category)
        {
            List<Product> x = GetProducts(category);
            return x;

        }
        //private List<Product> products = new List<Product>()
        //{
        //    new Product() { ProductId = 1, Name = "Product 1", CategoryName = "Category 1", Price=10}, 
        //    new Product() { ProductId = 1, Name = "Product 2", CategoryName = "Category 2", Price=5}, 
        //    new Product() { ProductId = 1, Name = "Product 3", CategoryName = "Category 3", Price=15}, 
        //    new Product() { ProductId = 1, Name = "Product 4", CategoryName = "Category 1", Price=9} 
        
        //}; 
        // Get all the data based on category provided
        private List<Product> GetProducts(string category)
        {
            category = category.ToUpper();
            List<Product> values = new List<Product>();
            String constr = "Provider=Microsoft.Jet.OLEDB.4.0;" +"Data Source=C:\\da.xls;" +"Extended Properties=Excel 8.0;";
            using (OleDbConnection conn = new OleDbConnection(constr))
            {
                conn.Open();
                OleDbCommand command = new OleDbCommand("Select * from [" + category +"$]", conn);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {                         
                        Product x = new Product();                       
                        x.MovieId = Int32.Parse(reader[0].ToString());
                        x.FilmName = reader[1].ToString();
                        x.Director = reader[2].ToString();
                        x.LeadingActors = reader[3].ToString();
                        x.ReleaseYear = Int32.Parse(reader[4].ToString());
                        x.OscarsWon = Int32.Parse(reader[5].ToString());
                        x.IMDBLink = reader[6].ToString();
                        x.Country = reader[7].ToString();
                        values.Add(x);
                    }
                }
            }
            return values;
        }
        // Add the user score when user exit from application
        public bool AddScores(string score, string name)
        {
            List<string> users = new List<string>();
            List<string> scores = new List<string>();
            String constr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=C:\\da.xls;" + "Extended Properties=Excel 8.0;";
            using (OleDbConnection conn = new OleDbConnection(constr))
            {
                conn.Open();
                OleDbCommand command = new OleDbCommand("Select * from [Scores$]", conn);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        scores.Add(reader[0].ToString());
                        users.Add(reader[1].ToString());
                    }
                }
                conn.Close();
            }

            if (users.Contains(name))
            {
                int x = users.IndexOf(name);
                int currentScore = Int32.Parse(scores[x].ToString());
                int totalScore = Int32.Parse(score) + currentScore;
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    conn.Open();
                    OleDbCommand command = new OleDbCommand("Update [Scores$] set Score ='" + totalScore .ToString()+ "' where User='" + name + "'", conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                }
            }
            else
            {
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    conn.Open();
                    OleDbCommand command = new OleDbCommand("INSERT INTO [Scores$] ([Score],[User]) VALUES('" + score + "','" + name + "');", conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                }
            }

            using (OleDbConnection conn = new OleDbConnection(constr))
            {
                conn.Open();
                OleDbCommand command = new OleDbCommand("INSERT INTO [IndividualScores$] ([Score],[User],[Date]) VALUES('" + score + "','" + name + "','"+DateTime.Now.ToString()+"');", conn);
                command.ExecuteNonQuery();
                conn.Close();
            }
            return true;
        }
        // Get all the user's past scores.
        public List<IndividualScore> GetIndividualScores(string name)
        {
            List<IndividualScore> scores = new List<IndividualScore>();
            String constr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=C:\\da.xls;" + "Extended Properties=Excel 8.0;";
            using (OleDbConnection conn = new OleDbConnection(constr))
            {
                conn.Open();
                OleDbCommand command = new OleDbCommand("Select * from [IndividualScores$] where User='" + name + "';", conn);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        IndividualScore i = new IndividualScore();
                        i.score = reader[0].ToString();
                        i.user = reader[1].ToString();
                        i.date = reader[2].ToString();
                        scores.Add(i);
                    }
                }
                conn.Close();
            }
            return scores;
        }
        // Get Total user score. Score user has earned till now.
        public int GetScores(string name)
        {
            int score = new int();
            String constr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=C:\\da.xls;" + "Extended Properties=Excel 8.0;";
            using (OleDbConnection conn = new OleDbConnection(constr))
            {
                conn.Open();
                OleDbCommand command = new OleDbCommand("Select * from [Scores$] where User='"+name+"';", conn);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        score = Int32.Parse(reader[0].ToString());                     
                    }
                }
                conn.Close();
            }
            return score;
        }
        // This service creates a new user.
        public bool CreateUser(string name, string password)
        {
            List<string> users = new List<string>();

            String constr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=C:\\da.xls;" + "Extended Properties=Excel 8.0;";
            using (OleDbConnection conn = new OleDbConnection(constr))
            {
                conn.Open();
                OleDbCommand command = new OleDbCommand("Select * from [Users$]", conn);
                 OleDbDataReader reader = command.ExecuteReader();
                 if (reader.HasRows)
                 {
                     while(reader.Read())
                     {
                         users.Add(reader[0].ToString());
                     }
                 }
                 conn.Close();
            }

            if (users.Contains(name))
            {
                return false;
            }
            else
            {
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    conn.Open();
                    OleDbCommand command = new OleDbCommand("INSERT INTO [Users$]([UserName],[Password]) VALUES('"+name+"','"+password+"');", conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
        }
        // This service is called to authenticate the user.
        public int CheckUser(string name, string password)
        {
            List<string> users = new List<string>();
            List<string> passwords = new List<string>();
            String constr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=C:\\da.xls;" + "Extended Properties=Excel 8.0;";
            using (OleDbConnection conn = new OleDbConnection(constr))
            {
                conn.Open();
                OleDbCommand command = new OleDbCommand("Select * from [Users$]", conn);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        users.Add(reader[0].ToString());
                        passwords.Add(reader[1].ToString());
                    }
                }
                conn.Close();
            }
            
            if (users.Contains(name))
            {                
                int x = users.IndexOf(name);
                if(password == passwords[x].ToString())
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 3;
            }
            
        }
    }
}

