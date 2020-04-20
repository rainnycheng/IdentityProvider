using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProvider.Models
{
    public class Users
    {

		public int sysid { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public int sys_CreateDate { get; set; }

		public string sys_CreateUser { get; set; }

		public DateTime sys_UpdateDate { get; set; }

		public string sys_UpdateUser { get; set; }



	}
}
