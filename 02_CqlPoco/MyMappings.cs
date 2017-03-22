using Cassandra.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_CqlPoco
{
    public class MyMappings : Mappings
    {
        public MyMappings()
        {
            For<User>()
                .TableName("users")
                .PartitionKey(u => u.UserName)
                .Column(u => u.UserName, cm => cm.WithName("username"))
                .Column(u => u.Email, cm => cm.WithName("email"))
                .Column(u => u.EncryptedPassword, cm => cm.WithName("encrypted_password"));
        }
    }
}
