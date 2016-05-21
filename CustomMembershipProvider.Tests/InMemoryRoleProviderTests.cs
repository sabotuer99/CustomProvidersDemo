using CustomMembershipProvider.Provider;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void Role_AddUsersToRoles_()
        {
            Assert.Fail();
        }

        [Test]
        public void Role_CreateRole()
        {
            Assert.Fail();
        }

        [Test]
        public void Role_DeleteRole()
        {
            Assert.Fail();
        }

        [Test]
        public void Role_FindUsersInRole()
        {
            Assert.Fail();
        }

        [Test]
        public void Role_GetAllRoles()
        {
            Assert.Fail();
        }

        [Test]
        public void Role_GetRolesForUser()
        {
            Assert.Fail();
        }

        [Test]
        public void Role_GetUsersInRole()
        {
            Assert.Fail();
        }

        [Test]
        public void Role_IsUserInRole()
        {
            Assert.Fail();
        }

        [Test]
        public void Role_RemoveUsersFromRoles()
        {
            Assert.Fail();
        }

        [Test]
        public void Role_RoleExists()
        {
            Assert.Fail();
        }

        private InMemoryRoleProvider getSut()
        {
            return new InMemoryRoleProvider();
        }
    }
}
