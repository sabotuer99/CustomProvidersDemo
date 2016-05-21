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
        public void Role_FindUsersInRole()
        {
            //Arrange

            //Act

            //Assert
            Assert.Fail();
        }

        [Test]
        public void Role_GetAllRoles()
        {
            //Arrange

            //Act

            //Assert
            Assert.Fail();
        }

        [Test]
        public void Role_GetRolesForUser()
        {
            //Arrange

            //Act

            //Assert
            Assert.Fail();
        }

        [Test]
        public void Role_GetUsersInRole()
        {
            //Arrange

            //Act

            //Assert
            Assert.Fail();
        }

        [Test]
        public void Role_IsUserInRole()
        {
            //Arrange

            //Act

            //Assert
            Assert.Fail();
        }

        [Test]
        public void Role_RemoveUsersFromRoles()
        {
            //Arrange

            //Act

            //Assert
            Assert.Fail();
        }

        [Test]
        public void Role_RoleExists()
        {
            //Arrange

            //Act

            //Assert
            Assert.Fail();
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
