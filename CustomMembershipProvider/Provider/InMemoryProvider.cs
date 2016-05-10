using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Configuration;

namespace CustomMembershipProvider.Provider
{
    public class InMemoryProvider : MembershipProvider
    {
        //Initiallize a dictionary that we'll user for our users;
        Dictionary<string, InMemoryUser> _users = new Dictionary<string, InMemoryUser>();

        public InMemoryProvider()
        {
        }

        // 
        // A helper function to retrieve config values from the configuration file. 
        // 

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            //base.Initialize(name, config);
            try
            {
                base.Initialize(name, null);
            }
            catch(Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                //gulp
            }

            _applicationName = GetConfigValue(config["applicationName"],
                                      System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            _maxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            _passwordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            _minRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonalphanumericCharacters"], "1"));
            _minRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            _passwordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            _enablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            _enablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            _requiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            _requiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
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
            return _users.Remove(username);
        }

        private bool _enablePasswordReset = true;
        public override bool EnablePasswordReset
        {
            get { return _enablePasswordReset; }
        }

        private bool _enablePasswordRetrieval = true;
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
            return _users.Values.Where(x => x.LastLoginDate > DateTime.Now - TimeSpan.FromMinutes(30)).Count();
        }

        public override string GetPassword(string username, string answer)
        {
            return _users.Values.Where(x => x.passwordAnswer == answer).FirstOrDefault().password;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return _users[username];
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return _users.Values.Where(x => x.ProviderUserKey == providerUserKey).FirstOrDefault();
        }

        public override string GetUserNameByEmail(string email)
        {
            return _users.Values.Where(x => x.Email == email).FirstOrDefault().UserName;
        }

        private int _maxInvalidPasswordAttempts;
        public override int MaxInvalidPasswordAttempts
        {
            get { return _maxInvalidPasswordAttempts; }
        }

        private int _minRequiredNonAlphanumericCharacters;
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return _minRequiredNonAlphanumericCharacters; }
        }

        private int _minRequiredPasswordLength;
        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }

        private int _passwordAttemptWindow;
        public override int PasswordAttemptWindow
        {
            get { return _passwordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Clear; }
        }

        private string _passwordStrengthRegularExpression;
        public override string PasswordStrengthRegularExpression
        {
            get { return _passwordStrengthRegularExpression; }
        }

        private bool _requiresQuestionAndAnswer;
        public override bool RequiresQuestionAndAnswer
        {
            get { return _requiresQuestionAndAnswer; }
        }

        private bool _requiresUniqueEmail;
        public override bool RequiresUniqueEmail
        {
            get { return _requiresUniqueEmail; }
        }

        public override string ResetPassword(string username, string answer)
        {
            var user = _users[username];
            if (user.passwordAnswer == answer)
            {
                if (user.ChangePassword(user.password, "password"))
                    return "password";
                else
                    throw new Exception("Couldn't change password for some reason...");
            }
            else
            {
                throw new MembershipPasswordException("Your answer was incorrect, boo!");
            }
        }

        public override bool UnlockUser(string userName)
        {
            return GetUser(userName, false).UnlockUser();
        }

        public override void UpdateUser(MembershipUser user)
        {
            _users[user.UserName] = user as InMemoryUser;
        }

        public override bool ValidateUser(string username, string password)
        {
            if (_users[username] == null || _users[username].password != password)
                return false;
            else
                return true;
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