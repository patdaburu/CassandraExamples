using Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_ConnectCrud
{
    /// <summary>
    /// A simple console application that connects to the local host, then
    /// performs some simple CRUD operations.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Preamble.
            Console.WriteLine("This example assumes Cassandra is running on the local host.");
            Console.WriteLine("It also assumes a keyspace called \"demo\" and a table called \"users\".");
            Console.WriteLine("If this isn't the case, run the following on your Cassandra server.");
            Console.WriteLine("CREATE KEYSPACE \"demo\" WITH REPLICATION = {\n\t'class': 'SimpleStrategy', 'replication_factor': 1\n};");
            Console.WriteLine("CREATE TABLE \"users\" (\n\t\"username\" text PRIMARY KEY,\n\t\"email\" text,\n\t\"encrypted_password\" blob\n); ");
            Console.WriteLine("Hit ENTER to continue");
            Console.ReadLine();

            // Connect to the demo keyspace on our cluster running on the
            // local host.
            Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
            ISession session = cluster.Connect("demo");
            Console.WriteLine("The connection succeeded. Hit ENTER key to perform an insert.");
            Console.ReadLine();

            // Insert a user.
            string fakeUserName = DateTime.Now.Ticks.ToString();
            string fakeEmailAddress = $"{fakeUserName}@hotmail.com";
            var insertPs = session.Prepare("insert into users (username, email) values (?, ?)");
            var insertStatement = insertPs.Bind(fakeUserName, fakeEmailAddress);
            session.Execute(insertStatement);
            Console.WriteLine($"Inserted record for user '{fakeUserName}'.  Hit ENTER to retrieve the record.");
            Console.ReadLine();

            // Retrieve the record.
            var selectPs = session.Prepare("select * from users where username=?");
            var selectStatement = selectPs.Bind(fakeUserName);
            Row insResult = session.Execute(selectStatement).First();
            Console.WriteLine($"The email address for user {insResult["username"]} is {insResult["email"]}.  Hit ENTER to update the record.");
            Console.ReadLine();

            // Update the record.
            var newFakeEmailAddress = $"{fakeUserName}@gmail.com";
            // Prepare the statement.
            var updatePs = session.Prepare("update users set email=? where username=?");
            var updateStatement = updatePs.Bind(newFakeEmailAddress, fakeUserName);
            session.Execute(updateStatement);
            Console.WriteLine($"Updated the email address for '{fakeUserName}' to '{newFakeEmailAddress}'.  Hit ENTER to see all the records.");
            Console.ReadLine();

            // Select all the records.
            RowSet rows = session.Execute("select * from users");
            foreach(Row row in rows)
            {
                Console.WriteLine($"{row["username"]}, {row["email"]}");
            }

            // Optionally, delete the current record.
            Console.WriteLine("Do you want to delete the record you just inserted?");
            Console.WriteLine("Type 'Y' to delete the record you just inserted, or any other key to move along.");
            var ckInfoDeleteRecord = Console.ReadKey(false);
            if(ckInfoDeleteRecord.Key == ConsoleKey.Y)
            {
                var deletePs = session.Prepare("delete from users where username=?");
                var deleteStatement = deletePs.Bind(fakeUserName);
                session.Execute(deleteStatement);
                Console.WriteLine($"Deleted record for '{fakeUserName}'.");
            }
            else
            {
                Console.WriteLine($"The record for '{fakeUserName}' was left in the database");
            }

            // Optionally, delete all the records.
            Console.WriteLine("Do you want to delete *all* the records in the table?");
            Console.WriteLine("Type 'Y' to delete *all* the records, or any other key to move along.");
            var ckInfoDeleteAll = Console.ReadKey(false);
            if(ckInfoDeleteAll.Key == ConsoleKey.Y)
            {
                session.Execute("truncate users");
                Console.WriteLine("Truncated the table.");
            }
            else
            {
                Console.WriteLine("Did not truncate the table.");
            }

            // That's all.
            Console.WriteLine("All done. Hit ENTER to continue.");
            Console.ReadLine();
        }
    }
}
