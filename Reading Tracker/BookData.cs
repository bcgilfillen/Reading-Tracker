using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Reading_Tracker
{
    internal class BookData
    {
        string connectionString = "Data Source=BookDatabase.sqlite";

        public BookData() { }

        public void VerifyDatabase()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string healthCheck = "Select name from sqlite_master where type='table' and name='books'";
                using (SQLiteCommand command = new SQLiteCommand(healthCheck, connection))
                {
                    using (SQLiteDataReader r = command.ExecuteReader())
                    {
                        while (!r.Read())
                        {
                            if (!r.ToString().Equals("books"))
                            {
                                string bookTable = "Create Table books (name varchar(50), location varchar(50), description varchar(250))";

                                using (SQLiteCommand comm = new SQLiteCommand(bookTable, connection))
                                {
                                    comm.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }

                //string duplicateCheck = "Delete from books where rowid not in (select min(rowid) from books group by name)";
                //using (SQLiteCommand command1 = new SQLiteCommand(duplicateCheck, connection))
                //{
                //    command1.ExecuteNonQuery();
                //}

                connection.Close();
            }
        }

        public List<Book> LoadList()
        {
            var list = new List<Book>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand comm = connection.CreateCommand())
                {
                    comm.CommandText = "Select * from books";
                    comm.CommandType = System.Data.CommandType.Text;
                    using (SQLiteDataReader r = comm.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            Book book = new Book();
                            book.Name = r.GetString(0);
                            book.Location = r.GetString(1);
                            book.Description = r.GetString(2);
                            list.Add(book);
                        }
                    }
                }

                connection.Close();
            }

            return list;
        }

        public void AddItem(string name, string location, string description)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string checkDups = "select count(*) from books where name=@name and location=@location and description=@description";
                using(SQLiteCommand dupComm = new SQLiteCommand(checkDups, connection))
                {
                    dupComm.Parameters.Add(new SQLiteParameter("@name", name));
                    dupComm.Parameters.Add(new SQLiteParameter("@location", location));
                    dupComm.Parameters.Add(new SQLiteParameter("@description", description));

                    int count = Convert.ToInt32(dupComm.ExecuteScalar());

                    if(count == 0)
                    {
                        string addRecord = "Insert into books (name, location, description) values (@param1,@param2,@param3)";
                        using (SQLiteCommand comm = new SQLiteCommand(addRecord, connection))
                        {
                            comm.Parameters.Add(new SQLiteParameter("@param1", name));
                            comm.Parameters.Add(new SQLiteParameter("@param2", location));
                            comm.Parameters.Add(new SQLiteParameter("@param3", description));
                            comm.ExecuteNonQuery();
                        }
                    }
                }

                connection.Close();
            }
        }

        public void RemoveItem(string name, string location, string description)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string deleteRecord = "Delete from books where name=@name and location=@location and description=@description";
                using(SQLiteCommand comm = new SQLiteCommand(deleteRecord, connection))
                {
                    comm.Parameters.Add(new SQLiteParameter("@name", name));
                    comm.Parameters.Add(new SQLiteParameter("@location", location));
                    comm.Parameters.Add(new SQLiteParameter("@description", description));
                    comm.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
