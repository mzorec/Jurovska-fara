using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Vortex.Services;

namespace Vortex.Tests.ServiceTests
{
    public class AuthenticationServiceTests
    {
        private IAuthenticationService authenticationService;

        [SetUp]
        public void SetUp()
        {
            authenticationService = new AuthenticationService();
        }

        [Test]
        public void SuccessFullCreateHashAndVerificationTest()
        {
            string salt;
            var hashedPassword = authenticationService.CreateHash("testPass", out salt);
            var isAuthenticated = authenticationService.ValidatePassword("testPass", salt, hashedPassword);
            Assert.IsTrue(isAuthenticated);
        }

        [Test]
        public void VerificationFailedWrongSaltTest()
        {
            string salt;
            var hashedPassword = authenticationService.CreateHash("testPass", out salt);
            salt = salt + "=";
            var isAuthenticated = authenticationService.ValidatePassword("testPass", salt, hashedPassword);
            Assert.IsFalse(isAuthenticated);
        }

        [TestCase("testpass", "testPass")]
        [TestCase("Testpass", "testPass")]
        [TestCase("testPass1", "testPass")]
        [TestCase("1testPass", "testPass")]
        [TestCase("ttestPass", "testPass")]
        [TestCase("a", "A")]
        public void WrongVerificationTest(string wrongPassword, string originalPassword)
        {
            string salt;
            var hashedPassword = authenticationService.CreateHash(originalPassword, out salt);
            var isAuthenticated = authenticationService.ValidatePassword(wrongPassword, salt, hashedPassword);
            Assert.IsFalse(isAuthenticated);
        }

        [Test]
        public void CreateHashSaltAreNotTheSameTest()
        {
            string salt, salt2;
            var hashedPassword = authenticationService.CreateHash("abc", out salt);
            var hashedPassword2 = authenticationService.CreateHash("abc", out salt2);
            Assert.AreNotEqual(salt, salt2);
            Assert.AreNotEqual(hashedPassword, hashedPassword2);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void EmptyPasswordCreateHashTest()
        {
            string salt;
            authenticationService.CreateHash(string.Empty, out salt);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void EmptyPasswordValidateTest()
        {
            string salt = "1";
            authenticationService.ValidatePassword(string.Empty, salt, "bla");
        }
    }
}
