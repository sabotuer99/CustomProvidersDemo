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
            var sut = getSut();

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
            var sut = getSut();

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
            var sut = getSut();
            MembershipCreateStatus status;
            var troy2 = sut.CreateUser("troy2", "password", "email2", null, null, true, "troy2", out status);
            var troy3 = sut.CreateUser("troy3", "password", "email3", null, null, true, "troy3", out status);

            //Act
            int count;
            var result = sut.GetAllUsers(0, 10, out count);

            //Assert
            Assert.AreEqual(3, count);
            Assert.NotNull(result["troy2"]);
        }

        [Test]
        public void GetAllUsers_ThreeUsersPageToMiddle_RecordCountIsOne()
        {
            //Arrange
            var sut = getSut();
            MembershipCreateStatus status;
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
            var sut = getSut();

            //Act
            var result1 = sut.ChangePassword("troy", "password", "newPassword");
            var result2 = sut.ChangePassword("troy", "newPassword", "anotherNew");

            //Assert
            Assert.True(result1);
            Assert.True(result2);   
        }

        [Test]
        public void DeleteUser_UserExists_ReturnsTrue()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result1 = sut.DeleteUser("troy", false);

            //Assert
            Assert.True(result1);
        }

        [Test]
        public void DeleteUser_UserDoesNotExists_ReturnsFalse()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result1 = sut.DeleteUser("blarg", false);

            //Assert
            Assert.False(result1);
        }

        [Test]
        public void GetUsernameByEmail_UserExists_ReturnsUsername()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result1 = sut.GetUserNameByEmail("email");

            //Assert
            Assert.AreEqual("troy", result1);
        }


        private InMemoryProvider getSut()
        {
            var sut = new InMemoryProvider();
            MembershipCreateStatus status;
            sut.CreateUser("troy", "password", "email", null, null, true, "troy", out status);

            return sut;
        }
    }
}
