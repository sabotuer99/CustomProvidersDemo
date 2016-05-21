using CustomMembershipProvider.Provider;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace CustomMembershipProvider.Tests
{
    [TestFixture]
    class InMemoryRoleProviderTests
    {
        [Test]
        public void Role_ctor_sanitycheck()
        {
            var sut = new InMemoryRoleProvider();
        }

        [Test]
        public void Role_AddUsersToRoles_RolesExist_AddsUsernames()
        {
            //Arrange
            var sut = getSut();
            var roles = new string[] {"User", "Nerd"};
            var users = new string[] {"Steve", "Bob"};

            //Act
            sut.AddUsersToRoles(users, roles);

            //Assert
            Assert.AreEqual(4, sut.UserRoles.Count());
            Assert.AreEqual(2, sut.UserRoles["Steve"].Count());
            Assert.AreEqual(2, sut.UserRoles["Bob"].Count());
        }

        [Test]
        [ExpectedException(typeof(ProviderException))]
        public void Role_AddUsersToRoles_RolesDoesNotExist_ThrowsProviderException()
        {
            //Arrange
            var sut = getSut();
            var roles = new string[] { "User", "SuperNerd" };
            var users = new string[] { "Steve", "Bob" };

            //Act
            sut.AddUsersToRoles(users, roles);

            //Assert
            //Should throw exception
        }

        [Test]
        public void Role_CreateRole_succeeds()
        {
            //Arrange
            var sut = getSut();

            //Act
            sut.CreateRole("SuperNerd");

            //Assert
            Assert.Contains("SuperNerd", sut.Roles);
        }

        [Test]
        [ExpectedException(typeof(ProviderException))]
        public void Role_DeleteRole_PopulatedRoleThrowEnabled_ThrowProviderException()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.DeleteRole("User", true);

            //Assert
            //should throw exception
        }

        [Test]
        public void Role_DeleteRole_PopulatedRoleThrowDisabled_DeletesAnyway()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.DeleteRole("User", false);

            //Assert
            Assert.True(result);
            Assert.AreEqual(2, sut.Roles.Count());
            Assert.False(sut.Roles.Contains("User"));
        }

        [Test]
        public void Role_DeleteRole_RoleDoesNotExist_ReturnsFalse()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.DeleteRole("NotInThere", false);

            //Assert
            Assert.False(result);
            Assert.AreEqual(3, sut.Roles.Count());
        }

        [Test]
        public void Role_DeleteRole_UnpopulatedRole_DeletesRole()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.DeleteRole("Nerd", false);

            //Assert
            Assert.True(result);
            Assert.AreEqual(2, sut.Roles.Count());
            Assert.False(sut.Roles.Contains("Nerd"));
        }


        [Test]
        [ExpectedException(typeof(ProviderException))]
        public void Role_FindUsersInRole_RoleDoesNotExist_ThrowProviderException()
        {
            //Arrange
            var sut = getSut();

            //Act
            sut.FindUsersInRole("NotInThere", "Bob");

            //Assert
            //Should throw exception
        }

        [Test]
        public void Role_FindUsersInRole_RoleExistsNoMatch_ReturnsNull()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.FindUsersInRole("User", "Bob");

            //Assert
            Assert.Null(result);
        }

        [Test]
        public void Role_FindUsersInRole_RoleExistsWithMatch_ReturnsArrayOfStrings()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.FindUsersInRole("User", "User");

            //Assert
            Assert.AreEqual(2, result.Length);
            Assert.Contains("TestUser", result);
        }

        [Test]
        public void Role_FindUsersInRole_RoleExistsUserNameBlank_ReturnsAllUsersInRole()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.FindUsersInRole("User", "");

            //Assert
            Assert.AreEqual(2, result.Length);
            Assert.Contains("TestUser", result);
        }

        [Test]
        public void Role_GetAllRoles_Succeeds()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.GetAllRoles();

            //Assert
            Assert.AreEqual(3, result.Length);
            Assert.Contains("User", result);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Role_GetRolesForUser_NullUserName_ThrowsArgumentException()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.GetRolesForUser(null);

            //Assert
            //Should throw exception
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Role_GetRolesForUser_EmptyUserName_ThrowsArgumentException()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.GetRolesForUser("");

            //Assert
            //Should throw exception
        }

        [Test]
        public void Role_GetRolesForUser_GoodUserName_ReturnsRoles()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.GetRolesForUser("AdminUser");

            //Assert
            Assert.AreEqual(2, result.Length);
        }

        [Test]
        public void Role_GetRolesForUser_UsernameNotInData_ReturnsEmptyArray()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.GetRolesForUser("Nobody");

            //Assert
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        [ExpectedException(typeof(ProviderException))]
        public void Role_GetUsersInRole_RoleDoesNotExist_ThrowProviderException()
        {
            //Arrange
            var sut = getSut();

            //Act
            sut.GetUsersInRole("NotARole");

            //Assert
            //Should throw exception
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Role_GetUsersInRole_RoleNameNull_ThrowArgumentException()
        {
            //Arrange
            var sut = getSut();

            //Act
            sut.GetUsersInRole(null);

            //Assert
            //Should throw exception
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Role_GetUsersInRole_RoleNameEmpty_ThrowArgumentException()
        {
            //Arrange
            var sut = getSut();

            //Act
            sut.GetUsersInRole("");

            //Assert
            //Should throw exception
        }

        [Test]
        public void Role_GetUsersInRole_RoleExist_ReturnUsers()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.GetUsersInRole("User");

            //Assert
            Assert.AreEqual(2, result.Length);
        }

        [Test]
        public void Role_GetUsersInRole_RoleExist_ReturnEmptyArray()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.GetUsersInRole("Nerd");

            //Assert
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Role_IsUserInRole_RolenameNull_ThrowArgumentException()
        {
            //Arrange
            var sut = getSut();

            //Act
            sut.IsUserInRole("Bob", null);

            //Assert
            //Throws exception
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Role_IsUserInRole_RoleNameEmpty_ThrowArgumentException()
        {
            //Arrange
            var sut = getSut();

            //Act
            sut.IsUserInRole("Bob", "");

            //Assert
            //Throws exception
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Role_IsUserInRole_UserNameNull_ThrowArgumentException()
        {
            //Arrange
            var sut = getSut();

            //Act
            sut.IsUserInRole(null, "User");

            //Assert
            //Throws exception
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Role_IsUserInRole_UserNameEmpty_ThrowArgumentException()
        {
            //Arrange
            var sut = getSut();

            //Act
            sut.IsUserInRole("", "User");

            //Assert
            //Throws exception
        }

        [Test]
        [ExpectedException(typeof(ProviderException))]
        public void Role_IsUserInRole_UserNameNotInDictionary_ThrowProviderException()
        {
            //Arrange
            var sut = getSut();

            //Act
            sut.IsUserInRole("NotARealUser", "User");

            //Assert
            //Throws exception
        }

        [Test]
        [ExpectedException(typeof(ProviderException))]
        public void Role_IsUserInRole_RoleDoesNotExist_ThrowProviderException()
        {
            //Arrange
            var sut = getSut();

            //Act
            sut.IsUserInRole("TestUser", "NotARealRole");

            //Assert
            //Throws exception
        }

        [Test]
        public void Role_IsUserInRole_UserIsInRole_ReturnsTrue()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.IsUserInRole("TestUser", "User");

            //Assert
            Assert.True(result);
        }

        [Test]
        public void Role_IsUserInRole_UserIsNotInRole_ReturnsFalse()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.IsUserInRole("TestUser", "Nerd");

            //Assert
            Assert.False(result);
        }

        [Test]
        [ExpectedException(typeof(ProviderException))]
        public void Role_RemoveUsersFromRoles_RolesThatDoNotExistInArray_ThrowProviderException()
        {
            //Arrange
            var sut = getSut();
            var users = new string[] { "TestUser", "AdminUser" };
            var roles = new string[] { "Dummy", "User" };

            //Act
            sut.RemoveUsersFromRoles(users, roles);

            //Assert
            //Should throw exception
        }

        [Test]
        public void Role_RemoveUsersFromRoles_RolesAreGood_RemovesRoles()
        {
            //Arrange
            var sut = getSut();
            var users = new string[] { "TestUser", "AdminUser" };
            var roles = new string[] { "User" };

            //Act
            sut.RemoveUsersFromRoles(users, roles);

            //Assert
            Assert.AreEqual(1, sut.UserRoles["AdminUser"].Count());
            Assert.AreEqual(0, sut.UserRoles["TestUser"].Count());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Role_RoleExists_NullPassed_ThrowArgumentException()
        {
            //Arrange
            var sut = getSut();

            //Act
            sut.RoleExists(null);

            //Assert
            //Should throw exception
        }
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Role_RoleExists_EmptyStringPassed_ThrowsArgumentException()
        {
            //Arrange
            var sut = getSut();

            //Act
            sut.RoleExists("");

            //Assert
            //Should throw exception
        }

        [Test]
        public void Role_RoleExists_RoleDoesNotExist_ReturnFalse()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.RoleExists("Blah");

            //Assert
            Assert.False(result);
        }

        [Test]
        public void Role_RoleExists_RoleDoesExist_ReturnTrue()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result = sut.RoleExists("User");

            //Assert
            Assert.True(result);
        }


        private InMemoryRoleProvider getSut()
        {
            var userRoles = new Dictionary<string, List<string>>();

            userRoles.Add("AdminUser", new List<string>() { "Admin", "User" });
            userRoles.Add("TestUser", new List<string>() { "User" });

            var roles = new List<string>() { "Admin", "User", "Nerd" };

            var sut = new InMemoryRoleProvider(userRoles, roles);
            var section = (RoleManagerSection)WebConfigurationManager.GetSection("system.web/roleManager");

            sut.Initialize("InMemoryRoleProvider", section.Providers["InMemoryRoleProvider"].Parameters);
            return sut;
        }
    }
}
