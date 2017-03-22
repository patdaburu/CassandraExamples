using Cassandra.Data.Linq;
using Cassandra.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CassandraColumn = Cassandra.Mapping.Attributes.ColumnAttribute;

namespace _02_CqlPoco
{
    /// <summary>
    /// This is an example of defining the mappings with attributes.
    /// </summary>
    [TableName("users")]
    [PrimaryKey("username")]
    public class DecoratedUser
    {
        [CassandraColumn("username")]
        public string UserName { get; set; }
        [CassandraColumn("email")]
        public string Email { get; set; }
        [CassandraColumn("encrypted_password")]
        public object EncryptedPassword { get; set; }
    }
}
