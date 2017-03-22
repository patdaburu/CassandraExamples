using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_CqlPoco
{
    /// <summary>
    /// This class models a record in the "users" table.
    /// </summary>
    class User
    {

        public string UserName { get; set; }
        public string Email { get; set; }
        public object EncryptedPassword { get; set; }

    }
}
