using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Classifieds.Common;
using Classifieds.UserService;
using Classifieds.UserService.Repository;
using Classifieds.UserService.BusinessEntities;
using Classifieds.UserService.BusinessServices;
using Moq;

namespace Classifieds.UserServiceAPI.Tests
{
    [TestClass]
    public class UserServiceTest
    {
        #region Class Variables
        private Mock<IUserRepository> _moqAppManager;
        private IUserService _service;
        string returnString = string.Empty;
        private UserToken userToken;
        private ClassifiedsUser user;
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _moqAppManager = new Mock<IUserRepository>();
            _service = new UserService.BusinessServices.UserService(_moqAppManager.Object);
        }
        #endregion

        #region Setup
        private void SetUpClassifiedsUsers()
        {
            var user = GetUserObject();
            returnString = "200 OK";
            user = GetUserObject();
            userToken = GetUserToken();
        }

        private ClassifiedsUser GetUserObject()
        {
            ClassifiedsUser user = new ClassifiedsUser
            {
                _id = "7",
                UserEmail = "ashish.kulkarni@globant.com",
                UserName = "Ashish Kulkarni"
            };
            return user;
        }

        private UserToken GetUserToken()
        {
            UserToken token = new UserToken
            {
                _id = 5.ToString(),
                AccessToken = Guid.NewGuid().ToString(),
                UserEmail = "ashish.kulkarni@globant.com"
            };
            return token;
        }
        #endregion
        [TestMethod]
        public void RegisterUserTest()
        {
            SetUpClassifiedsUsers();
            //string returnString = "200 OK";
            _moqAppManager.Setup(x => x.RegisterUser(this.user)).Returns(returnString);

            //Act
            var result = _service.RegisterUser(user);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RegisterUserTest_ThrowsException()
        {
            _service.RegisterUser(null);
        }

        [TestMethod]
        public void SaveTokenTest()
        {
            SetUpClassifiedsUsers();
            _moqAppManager.Setup(x => x.SaveToken(this.userToken)).Returns(userToken);

            //Act
            var result = _service.SaveToken(userToken);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UserToken));
        }

    }
}
