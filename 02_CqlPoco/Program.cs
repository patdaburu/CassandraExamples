using Cassandra;
using Cassandra.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_CqlPoco
{
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
            // Now that we have a session, we can construct a "mapper".
            IMapper mapper = new Mapper(session);

            // Define the mappings.
            MappingConfiguration.Global.Define<MyMappings>();
            Console.WriteLine("The connection succeeded. Hit ENTER key to see all the records.");
            Console.ReadLine();

            // Get all the records.
            #region A note about mappings...
            /**
             * If you want to see an example of setting up mappings using 
             * class attributes, replace references to the "User" class with
             * "DecoratedUser".
             */
            #endregion
            var users = mapper.Fetch<User>("SELECT username, email, encrypted_password FROM users");
            foreach(var user in users)
            {
                Console.WriteLine($"username='{user.UserName}', email='{user.Email}'");
            }
            Console.WriteLine("The connection succeeded. Hit ENTER key to insert a new record.");
            Console.ReadLine();

            // Create a new record.
            // Insert a new user.
            string fakeUserName = DateTime.Now.Ticks.ToString();
            string fakeEmailAddress = $"{fakeUserName}@hotmail.com";
            var newUser = new User
            {
                UserName = fakeUserName,
                Email = fakeEmailAddress
            };
            mapper.Insert(newUser);
            Console.WriteLine($"Inserted record for user '{fakeUserName}'.  Hit ENTER to update the record.");
            Console.ReadLine();

            // Update the user.
            newUser.Email = $"{fakeUserName}@gmail.com";
            mapper.Update(newUser);

            

            // Optionally, delete the current record.
            Console.WriteLine("Do you want to delete the record you just inserted?");
            Console.WriteLine("Type 'Y' to delete the record you just inserted, or any other key to move along.");
            var ckInfoDeleteRecord = Console.ReadKey(false);
            if (ckInfoDeleteRecord.Key == ConsoleKey.Y)
            {
                mapper.Delete(newUser);
                Console.WriteLine($"Deleted record for '{fakeUserName}'.");
            }
            else
            {
                Console.WriteLine($"The record for '{fakeUserName}' was left in the database");
            }

            Console.WriteLine("Hit ENTER to see all the records.");
            Console.ReadLine();

            // Get all the records.
            users = mapper.Fetch<User>("SELECT username, email, encrypted_password FROM users");
            foreach (var user in users)
            {
                Console.WriteLine($"username='{user.UserName}', email='{user.Email}'");
            }

            // That's all.
            Console.WriteLine("All done. Hit ENTER to continue.");
            Console.ReadLine();
        }
    }
}
