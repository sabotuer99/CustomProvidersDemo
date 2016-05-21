using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomMembershipProvider.Provider;
using System.Web.Security;
using System.Web.Configuration;
using System.Collections.Specialized;

namespace CustomMembershipProvider.Tests
{
    
    [TestFixture]
    public class InMemoryMembershipProviderTests
    {
        [Test]
        public void Memb_ctor_sanityCheck()
        {

            new InMemoryProvider();
        }

        [Test]
        public void Memb_CreateUser_PassValidInput_CreatesUser()
        {
            var sut = new InMemoryProvider();
            var status = MembershipCreateStatus.UserRejected;
            var troy = sut.CreateUser("troy", "password","email", null, null, true, "troy", out status);

            Assert.NotNull(troy);
        }

        [Test]
        public void Memb_FindUserByEmail_OneUserEmailMatches_RecordCountIsOne()
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
        public void Memb_FindUserByName_OneUserEmailMatches_RecordCountIsOne()
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
        public void Memb_GetAllUsers_ThreeUsers_RecordCountIsThree()
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
        public void Memb_GetAllUsers_ThreeUsersPageToMiddle_RecordCountIsOne()
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
        public void Memb_ChangePassword_CorrectUserAndPassword_ReturnsTrue()
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
        public void Memb_DeleteUser_UserExists_ReturnsTrue()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result1 = sut.DeleteUser("troy", false);

            //Assert
            Assert.True(result1);
        }

        [Test]
        public void Memb_DeleteUser_UserDoesNotExists_ReturnsFalse()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result1 = sut.DeleteUser("blarg", false);

            //Assert
            Assert.False(result1);
        }

        [Test]
        public void Memb_GetUsernameByEmail_UserExists_ReturnsUsername()
        {
            //Arrange
            var sut = getSut();

            //Act
            var result1 = sut.GetUserNameByEmail("email");

            //Assert
            Assert.AreEqual("troy", result1);
        }



        [Test]
        public void Memb_getters_ValuesInConfigFile_ReturnsCorrectValues()
        {
            //Arrange
            var sut = getSut();

            //Act
            var mrwl = sut.MinRequiredPasswordLength;
            var mrnc = sut.MinRequiredNonAlphanumericCharacters;
            var rqa = sut.RequiresQuestionAndAnswer;
            var mipa = sut.MaxInvalidPasswordAttempts;
            var paw = sut.PasswordAttemptWindow;

            //Assert
            Assert.AreEqual(6, mrwl);
            Assert.AreEqual(0, mrnc);
            Assert.False(rqa);
            Assert.AreEqual(3, mipa);
            Assert.AreEqual(15, paw);
        }

        [Test]
        public void Memb_getters_ValuesNotInConfigFile_ReturnsDefaultValues()
        {
            //Arrange
            var sut = getSut();

            //Act
            var appn = sut.ApplicationName;
            var epres = sut.EnablePasswordReset;
            var epret = sut.EnablePasswordRetrieval;
            var rue = sut.RequiresUniqueEmail;

            //Assert
            Assert.Null(appn);
            Assert.True(epres);
            Assert.True(epret);
            Assert.True(rue);
        }


        private InMemoryProvider getSut()
        {
            var sut = new InMemoryProvider();
            var section = (MembershipSection)WebConfigurationManager.GetSection("system.web/membership");
            var config = new NameValueCollection();

            sut.Initialize("InMemoryProvider", section.Providers["InMemoryProvider"].Parameters);
            MembershipCreateStatus status;
            sut.CreateUser("troy", "password", "email", null, null, true, "troy", out status);

            return sut;
        }
    }
}
