using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using IdentityProvider.Models;
using System.Linq;
using static IdentityModel.OidcConstants;

namespace IdentityProvider.Configs
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            using (IDbConnection db = new SqlConnection("Server=localhost\\sqlexpress;Database=IDC;User ID=sa;Password=1qaz2wsx;"))
            {

                var user = db.Query<Users>("select * from users where username=@username and password=@password", 
                    new { UserName = context.UserName, password = context.Password }).SingleOrDefault<Users>();

                if (user == null)
                {
                    context.Result = new GrantValidationResult(TokenErrors.InvalidRequest, "user name or passwor is incorrext");
                    return Task.FromResult(0);
                }

                context.Result = new GrantValidationResult(user.sysid.ToString(), "password  error");
                return Task.FromResult(0);


            }
        }
    }
}
