using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Net;
using System.Web.Http;
using Classifieds.Common;
using Classifieds.Common.Entities;
using Classifieds.Common.Repositories;
using Classifieds.UserServiceAPI.Controllers;
using Classifieds.UserService.BusinessEntities;
using Classifieds.UserService.BusinessServices;

namespace Classifieds.UserServiceAPI.Tests
{
    [TestClass]
    public class UserControllerTest
    {
        #region Class Variables
        private Mock<IUserService> _mockService;
        private Mock<ILogger> _logger;
        private Mock<ICommonRepository> _mockAuthRepo;
        private UserController _controller;
        private ClassifiedsUser _user;
        private UserToken _token;
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _mockService = new Mock<IUserService>();
            _logger = new Mock<ILogger>();
            _mockAuthRepo = new Mock<ICommonRepository>();
            _controller = new UserController(_mockService.Object, _logger.Object, _mockAuthRepo.Object);
            _controller.Request = new HttpRequestMessage();
            _controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
        }
        #endregion

        #region Setup Methods
        private void SetUpClassifiedsUsers()
        {
            _user = GetUserObject();
            _token = GetUserToken();
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

        /// <summary>
        /// Test for successful user registration
        /// </summary>
        [TestMethod]
        public void RegisterUserTest_ValidInput()
        {
            SetUpClassifiedsUsers();
            _mockService.Setup(x => x.RegisterUser(_user)).Returns("Success");
            _mockAuthRepo.Setup(x => x.SaveToken(_token)).Returns(_token);
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));

            //Act
            var response = _controller.RegisterUser(_user);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        /// <summary>
        /// Test for exception management in Register
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void RegisterUserTest_ThrowsException()
        {
            _controller.RegisterUser(null);
        }
    }
}
