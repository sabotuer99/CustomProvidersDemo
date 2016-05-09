using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomMembershipProvider.Provider;
using System.Web.Security;

namespace CustomMembershipProvider.Tests
{
    [TestFixture]
    public class InMemoryMembershipProviderTests
    {
        [Test]
        public void ctor_sanityCheck()
        {

            new InMemoryProvider();
        }

        [Test]
        public void CreateUser_PassValidInput_CreatesUser()
        {
            var sut = new InMemoryProvider();
            var status = MembershipCreateStatus.UserRejected;
            var troy = sut.CreateUser("troy", "password","email", null, null, true, "troy", out status);

            Assert.NotNull(troy);
        }

        [Test]
        public void FindUserByEmail_OneUserEmailMatches_RecordCountIsOne()
        {
            //Arrange
            var sut = new InMemoryProvider();
            MembershipCreateStatus status;
            var troy = sut.CreateUser("troy", "password", "email", null, null, true, "troy", out status);

            //Act
            int count;
            var result = sut.FindUsersByEmail("email", 0, 10, out count);

            //Assert
            Assert.AreEqual(1, count);
            Assert.NotNull(result["troy"]);
        }

        [Test]
        public void FindUserByName_OneUserEmailMatches_RecordCountIsOne()
        {
            //Arrange
            var sut = new InMemoryProvider();
            MembershipCreateStatus status;
            var troy = sut.CreateUser("troy", "password", "email", null, null, true, "troy", out status);

            //Act
            int count;
            var result = sut.FindUsersByName("troy", 0, 10, out count);

            //Assert
            Assert.AreEqual(1, count);
            Assert.NotNull(result["troy"]);
        }

        [Test]
        public void GetAllUsers_ThreeUsers_RecordCountIsThree()
        {
            //Arrange
            var sut = new InMemoryProvider();
            MembershipCreateStatus status;
            var troy1 = sut.CreateUser("troy1", "password", "email1", null, null, true, "troy1", out status);
            var troy2 = sut.CreateUser("troy2", "password", "email2", null, null, true, "troy2", out status);
            var troy3 = sut.CreateUser("troy3", "password", "email3", null, null, true, "troy3", out status);

            //Act
            int count;
            var result = sut.GetAllUsers(0, 10, out count);

            //Assert
            Assert.AreEqual(3, count);
            Assert.NotNull(result["troy1"]);
        }

        [Test]
        public void GetAllUsers_ThreeUsersPageToMiddle_RecordCountIsOne()
        {
            //Arrange
            var sut = new InMemoryProvider();
            MembershipCreateStatus status;
            var troy1 = sut.CreateUser("troy1", "password", "email1", null, null, true, "troy1", out status);
            var troy2 = sut.CreateUser("troy2", "password", "email2", null, null, true, "troy2", out status);
            var troy3 = sut.CreateUser("troy3", "password", "email3", null, null, true, "troy3", out status);

            //Act
            int count;
            var result = sut.GetAllUsers(1, 1, out count);

            //Assert
            Assert.AreEqual(1, count);
            Assert.NotNull(result["troy2"]);
        }

        [Test]
        public void ChangePassword_CorrectUserAndPassword_ReturnsTrue()
        {
            //Arrange
            var sut = new InMemoryProvider();
            MembershipCreateStatus status;
            var troy1 = sut.CreateUser("troy", "password", "email", null, null, true, "troy", out status);

            //Act
            var result1 = sut.ChangePassword("troy", "password", "newPassword");
            var result2 = sut.ChangePassword("troy", "newPassword", "anotherNew");

            //Assert
            Assert.True(result1);
            Assert.True(result2);   
        }
    }
}
