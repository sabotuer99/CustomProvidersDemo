using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CustomMembershipProvider.Provider
{
    public class InMemoryProvider : MembershipProvider
    {
        //Initiallize a dictionary that we'll user for our users;
        Dictionary<string, InMemoryUser> _users = new Dictionary<string, InMemoryUser>();

        public InMemoryProvider()
        {
            //var status = MembershipCreateStatus.UserRejected;
            //var troy = CreateUser("troy", "password","email", null, null, true, "troy", out status);  
        }


        private string _applicationName;
        public override string ApplicationName
        {
            get
            {
                return _applicationName;
            }
            set
            {
                _applicationName = value;
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var user = _users[username];

            if (user == null || user.password != oldPassword) //username not found or password no match
                return false;

            user.password = newPassword;
            return true;
  
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            var user = new InMemoryUser(
                "InMemoryProvider",
                username,
                providerUserKey,
                email,
                passwordQuestion,
                "Comment",
                isApproved,
                false,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now
                );

            user.password = password;
            user.passwordAnswer = passwordAnswer;

            _users.Add(username, user);
            status = MembershipCreateStatus.Success;
            return user;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        private bool _enablePasswordReset;
        public override bool EnablePasswordReset
        {
            get { return _enablePasswordReset; }
        }

        private bool _enablePasswordRetrieval;
        public override bool EnablePasswordRetrieval
        {
            get { return _enablePasswordRetrieval; }
        }


        private MembershipUserCollection toMuc(IEnumerable<MembershipUser> users)
        {
            var muc = new MembershipUserCollection();
            foreach (var user in users)
                muc.Add(user);
            return muc;
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            int startIndex = pageIndex * pageSize;
            var users = _users.Values.Where(x => x.Email.Equals(emailToMatch)).Skip(startIndex).Take(pageSize);
            totalRecords = users.Count();
            return toMuc(users);
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            int startIndex = pageIndex * pageSize;
            var users = _users.Values.Where(x => x.UserName.Equals(usernameToMatch)).Skip(startIndex).Take(pageSize);
            totalRecords = users.Count();
            return toMuc(users);
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            int startIndex = pageIndex * pageSize;
            var users = _users.Values.Skip(startIndex).Take(pageSize);
            totalRecords = users.Count();
            return toMuc(users);
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            throw new NotImplementedException();
        }

    }

    public class InMemoryUser : MembershipUser
    {
        public string password;
        public string passwordAnswer;

        public InMemoryUser(
            string providerName, 
            string username, 
            object providerUserKey, 
            string email, 
            string passwordQuestion,
            string comment, 
            bool isApproved,
            bool isLockedOut,
            DateTime creationDate,
            DateTime lastLoginDate,
            DateTime lastActivityDate,
            DateTime lastPasswordChangedDate,
            DateTime lastLockoutDate) :
            base(providerName, username, providerUserKey, email, passwordQuestion, comment, isApproved, isLockedOut,
                 creationDate, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockoutDate)
        {}
    }
}