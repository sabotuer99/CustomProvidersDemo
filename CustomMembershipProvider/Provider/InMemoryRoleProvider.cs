﻿using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CustomMembershipProvider.Provider
{
    public class InMemoryRoleProvider : RoleProvider
    {
        private static Dictionary<string, List<string>> _userRoles = new Dictionary<string, List<string>>();
        private static List<string> _roles = new List<string>();

        public InMemoryRoleProvider() { }


        public InMemoryRoleProvider(Dictionary<string, List<string>> userRoles, List<string> roles)
        {
            _userRoles = userRoles;
            _roles = roles;
        }

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
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                //gulp
            }

            _applicationName = GetConfigValue(config["applicationName"],
                                      System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        }


        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            if (!roleNames.All(x => Roles.Contains(x)))
                throw new ProviderException("Some roles to not exist!");


            foreach (string username in usernames)
            {
                foreach (string role in roleNames)
                {
                    if (!UserRoles.ContainsKey(username))
                        UserRoles.Add(username, new List<string>());

                    UserRoles[username].Add(role);
                }
            }
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

        public override void CreateRole(string roleName)
        {
            Roles.Add(roleName);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (throwOnPopulatedRole && UserRoles.Values.Any(x => x.Contains(roleName)))
                throw new ProviderException("Cannot delete populated role");

            return Roles.Remove(roleName);

        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            if (!Roles.Contains(roleName))
                throw new ProviderException("Role does not exist!");

            var users = UserRoles.Where(x => x.Value.Contains(roleName) && x.Key.Contains(usernameToMatch)).Select(x => x.Key).ToArray();

            return users.Length == 0 ? null : users;
        }

        public override string[] GetAllRoles()
        {
            return Roles.ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            if(username == null || username == "")
                throw new ArgumentException("Username cannot be null or empty");

            return UserRoles.ContainsKey(username) ? UserRoles[username].ToArray() : new string[0];
        }

        public override string[] GetUsersInRole(string roleName)
        {
            if (roleName == null || roleName == "")
                throw new ArgumentException("Role name cannot be null or empty");

            if (!Roles.Contains(roleName))
                throw new ProviderException("Role does not exist!");


            return UserRoles.Where(x => x.Value.Contains(roleName)).Select(x => x.Key).ToArray();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (roleName == null || roleName == "")
                throw new ArgumentException("Role name cannot be null or empty");

            if (username == null || username == "")
                throw new ArgumentException("Username cannot be null or empty");

            if (!UserRoles.ContainsKey(username))
                throw new ProviderException("Username not found");

            if (!Roles.Contains(roleName))
                throw new ProviderException("Role name not found");


            return UserRoles[username].Contains(roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            if (roleNames.Any(x => !Roles.Contains(x)))
                throw new ProviderException("Some roles do not exist");

            foreach (string username in usernames)
            {
                if (UserRoles.ContainsKey(username))
                    UserRoles[username].RemoveAll(x => roleNames.Contains(x));
            }
        }

        public override bool RoleExists(string roleName)
        {
            if (roleName == null || roleName == "")
                throw new ArgumentException("Role name cannot be null or empty");

            return Roles.Contains(roleName);
        }

        public Dictionary<string, List<string>> UserRoles
        {
            get {
                return InMemoryRoleProvider._userRoles;
            }          
        }

        public List<string> Roles
        {
            get
            {
                return InMemoryRoleProvider._roles;
            }
        }
    }
}