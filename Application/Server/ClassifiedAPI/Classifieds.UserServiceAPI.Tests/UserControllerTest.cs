using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Net;
using System.Web.Http;
using System.Web.Http.Routing;
using Classifieds.Common;
using Classifieds.UserService;
using Classifieds.UserServiceAPI.Controllers;
using Classifieds.UserService.BusinessEntities;
using Classifieds.UserService.BusinessServices;

namespace Classifieds.UserServiceAPI.Tests
{
    [TestClass]
    public class UserControllerTest
    {
        #region Class Variables
        private Mock<IUserService> mockService;
        private Mock<ILogger> logger;
        private Mock<ICommonDBRepository> mockAuthRepo;
        private Mock<HttpRequestMessage> mockRequest;
        //private readonly List<ClassifiedsUser> classifiedUserList = new List<ClassifiedsUser>();
        private const string urlLocation = "http://localhost/UserServiceAPI/api/";
        private UserController controller;
        private ClassifiedsUser user;
        private UserToken token;
        private HttpResponseMessage response;
        private HttpStatusCode statusCode;
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IUserService>();
            logger = new Mock<ILogger>();
            mockAuthRepo = new Mock<ICommonDBRepository>();
            controller = new UserController(mockService.Object, logger.Object, mockAuthRepo.Object);
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            response = new HttpResponseMessage(HttpStatusCode.Created);
        }
        #endregion

        #region Setup Methods
        private void SetUpClassifiedsUsers()
        {
            user = GetUserObject();
            //classifiedUserList.Add(user);
            token = GetUserToken();
            statusCode = HttpStatusCode.Created;
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
        public void RegisterTest()
        {
            SetUpClassifiedsUsers();
            mockService.Setup(x => x.RegisterUser(user)).Returns("Success");
            mockService.Setup(x => x.SaveToken(token)).Returns(token);
            //mockRequest.Setup(x => x.CreateResponse<UserToken>(statusCode, token)).Returns(this.response);
            logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));

            //Act
            var response = controller.Register(user);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RegisterTest_ThrowsException()
        {
            controller.Register(null);
        }
    }
}
