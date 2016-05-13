using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace CustomSimpleMembership.Provider
{
    public class CustomAdminMembershipProvider : SimpleMembershipProvider
    {
        // TODO: will do a better way
        private const string SELECT_ALL_USER_SCRIPT = "select * from [dbo].[UserProfile]private where UserName = '{0}'";

        private readonly IEncrypting _encryptor;

        private readonly SimpleSecurityContext _simpleSecurityContext;

        public CustomAdminMembershipProvider()
            : this(new Encryptor(), new SimpleSecurityContext("DefaultConnection"))
        {
        }

        public CustomAdminMembershipProvider(SimpleSecurityContext simpleSecurityContext)
            : this(new Encryptor(), new SimpleSecurityContext("DefaultConnection"))
        {
        }

        public CustomAdminMembershipProvider(IEncrypting encryptor, SimpleSecurityContext simpleSecurityContext)
        {
            _encryptor = encryptor;
            _simpleSecurityContext = simpleSecurityContext;
        }

        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Argument cannot be null or empty", "username");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Argument cannot be null or empty", "password");
            }

            var hash = _encryptor.Encode(password);

            using (_simpleSecurityContext)
            {
                var users =
                    _simpleSecurityContext.Users.SqlQuery(
                        string.Format(SELECT_ALL_USER_SCRIPT, username));

                if (users == null && !users.Any())
                {
                    return false;
                }

                return users.FirstOrDefault().Password == hash;
            }
        }
    }

    public class AdminRoleProvider : SimpleRoleProvider
    {

    }
}