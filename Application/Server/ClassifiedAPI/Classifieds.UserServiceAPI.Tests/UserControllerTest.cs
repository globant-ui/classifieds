using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Net;
using System.Web.Http;
using System.Web.Http.Routing;
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

        private Subscription GetSubscription()
        {
            Subscription subscription = new Subscription
            {
                _id = "1",
                UserEmail = "v.wadsamudrakar@globant.com",
                SubmittedDate = new DateTime(2018, 02, 03)
            };
            return subscription;
        }
        #endregion
        
        #region RegisterUserTest
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
        #endregion RegisterUserTest

        #region AddSubscriptionTest

        /// <summary>
        /// test positive scenario for Post Subscription
        /// </summary>
        [TestMethod]
        public void Controller_AddSubscriptionTest()
        {
            // Arrange
            _mockService.Setup(x => x.AddSubscription(It.IsAny<Subscription>()))
            .Returns(GetSubscription());
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
            };
            _controller.Configuration = new HttpConfiguration();
            _controller.Configuration.Routes.MapHttpRoute(
                name: "Category",
                routeTemplate: "api/{controller}/{method}/{id}",
                defaults: new { id = RouteParameter.Optional });

            _controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "Subscription" } });

            // Act
            var listObj = GetSubscription();
            var response = _controller.AddSubscription(listObj);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        /// <summary>
        /// test for inserting null Subscription object throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Controller_PostSubscription_ThrowsException()
        {
            _controller.AddSubscription(null);
        }

        /// <summary>
        /// test positive scenario for PostCategory and verify response header location
        /// </summary>
        [TestMethod]
        public void Controller_PostcategoryTest_SetsLocationHeader_MockURLHelperVersion()
        {
            // This version uses a mock UrlHelper.
            // Arrange
            _mockService.Setup(x => x.AddSubscription(It.IsAny<Subscription>()))
            .Returns(GetSubscription);
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            string locationUrl = "http://localhost/UserAPI/api/user";

            // Create the mock and set up the Link method, which is used to create the Location header.
            // The mock version returns a fixed string.
            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Url = mockUrlHelper.Object;

            // Act
            Subscription subObj = GetSubscription();
            var response = _controller.AddSubscription(subObj);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        #endregion AddSubscriptionTest

        #region DeleteSubscriptionTestCases

        /// <summary>
        /// test positive scenario for deleting Subscription
        /// </summary>
        [TestMethod]
        public void Controller_DeleteSubscriptionTest()
        {
            // Arrange
            var dataObject = GetSubscription();
            _mockService.Setup(x => x.DeleteSubscription(It.IsAny<string>()));
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost/api/User")
            };
            // Act                
            var response = _controller.DeleteSubscription(dataObject._id);

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        /// <summary>
        /// test for deleting Subscription object throws exception
        /// </summary>
        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void Controller_DeleteSubscription_ThrowsException()
        {
            _controller.DeleteSubscription(null);
        }

        #endregion DeleteSubscriptionTestCases

        #region GetUserProfileTest
        /// <summary>
        /// Test for GetUserProfile valid Scenario
        /// </summary>
        [TestMethod]
        public void Controller_GetUserProfileTest_Valid()
        {
           _mockService.Setup(x => x.GetUserProfile(It.IsAny<string>()))
                .Returns(new ClassifiedsUser()
                {
                    UserEmail = "amol.pawar@globant.com",
                    UserName = "Amol Pawar"
                }
               );
           _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
           _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");

            //Act
            var result = _controller.GetUserProfile("amol.pawar@globant.com");

            //Assert
            Assert.AreEqual(result.UserName, "Amol Pawar");
        }

        /// <summary>
        ///  GetUserProfile Empty result Test.
        /// </summary>
        [TestMethod]
        public void Controller_GetUserProfileTes_EmptyResult()
        {
            _mockService.Setup(x => x.GetUserProfile(It.IsAny<string>()))
                .Returns(new ClassifiedsUser()
                {
                    UserEmail = "",
                    UserName = ""
                });
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");

            //Act
            var result = _controller.GetUserProfile(string.Empty);
            //Assert
            Assert.AreEqual(result.UserName, "");
        }
        /// <summary>
        /// Controller_FreeTextSearch_ThrowsException Test Exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_GetUserProfileTest_ThrowsException()
        {
            _mockService.Setup(x => x.GetUserProfile(It.IsAny<string>()))
                .Returns(new ClassifiedsUser()
                {
                    UserEmail = "",
                    UserName = ""
                });
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");

            //Act
            _controller.GetUserProfile(null);
         
        }

        #endregion GetUserProfileTest

        #region UpdateUserProfileTest
        /// <summary>
        /// Test for Update UserProfile valid Scenario
        /// </summary>
        [TestMethod]
        public void Controller_UpdateUserProfileTest_Valid()
        {
            _mockService.Setup(x => x.UpdateUserProfile(It.IsAny<ClassifiedsUser>()))
                .Returns(
                    new ClassifiedsUser()
                    {
                        UserEmail = "amol.pawar@globant.com",
                        UserName = "Amol Pawar"
                    }
                );
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");

            //Act
            var result = _controller.UpdateUserProfile(new ClassifiedsUser()
            {
                UserEmail = "amol.pawar@globant.com",
                UserName = "Amol Pawar"
            });

            //Assert
            Assert.AreEqual(result.UserName, "Amol Pawar");
        }
        /// <summary>
        /// Test for Update UserProfile Exception Scenario
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_UpdateUserProfileTest_ThrowsException()
        {
            _mockService.Setup(x => x.UpdateUserProfile(It.IsAny<ClassifiedsUser>()))
                .Returns(
                    new ClassifiedsUser()
                    {
                        UserEmail = "amol.pawar@globant.com",
                        UserName = "Amol Pawar"
                    }
                );
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");

            //Act
            _controller.UpdateUserProfile(null);
          
        }
        #endregion UpdateUserProfileTest

        #region TagUnitTest
        /// <summary>
        /// test positive scenario for Post UserTag
        /// </summary>
        [TestMethod]
        public void Controller_AddTagTest()
        {
            // Arrange
            _mockService.Setup(x => x.AddTag(It.IsAny<string>(),It.IsAny<Tags>()))
            .Returns(new Tags()
                    {
                        SubCategory = new string[] { "SubCategory1", "SubCategory2" },
                        Location = new string[] { "Location1", "Location2" }
                    }
                );
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
            };
            _controller.Configuration = new HttpConfiguration();
            _controller.Configuration.Routes.MapHttpRoute(
                name: "UserServiceAPI",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });

            _controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "User" } });

            // Act
           var result = _controller.AddTag("amol.pawar@globant.com",new Tags()
            {
                SubCategory = new string[] { "SubCategory1", "SubCategory2" },
                Location = new string[] { "Location1", "Location2" }
            });
            
            // Assert
            Assert.IsNotNull(result.SubCategory);
            Assert.AreEqual(result.Location.Length,2);
        }

        /// <summary>
        /// test exception scenario for Post UserTag
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_AddTag_ExceptionTest()
        {
            var result = _controller.AddTag(null, new Tags()
            {
                SubCategory = new string[] { "SubCategory1", "SubCategory2" },
                Location = new string[] { "Location1", "Location2" }
            });
        }
        /// <summary>
        /// test positive scenario for deleting User Tag
        /// </summary>
        [TestMethod]
        public void Controller_DeleteTag()
        {
            // Arrange
            _mockService.Setup(x => x.DeleteTag(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost/UserAPI/api/User")
            };
            // Act                
            var result = _controller.DeleteTag("amol.pawar@globant.com","Cars");

            //Assert
            Assert.IsTrue(result);
       }
        /// <summary>
        /// test positive scenario for deleting User Tag
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Controller_DeleteTag_Exception()
        {
            // Arrange
            _mockService.Setup(x => x.DeleteTag(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
           
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost/UserAPI/api/User")
            };
            // Act                
            _controller.DeleteTag("amol.pawar@globant.com", "Cars");
           
        }

        #endregion TagUnitTest

        #region AlertUnitTest
        /// <summary>
        /// test positive scenario for Post UserAlert
        /// </summary>
        [TestMethod]
        public void Controller_AddAlertTest()
        {
            // Arrange
            _mockService.Setup(x => x.AddAlert(It.IsAny<string>(), It.IsAny<Alert>()))
            .Returns(true);
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");

            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
            };

            _controller.Configuration = new HttpConfiguration();
            _controller.Configuration.Routes.MapHttpRoute(
                name: "UserServiceAPI",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });

            _controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "User" } });

            // Act
            var result = _controller.AddAlert("amol.pawar@globant.com", new Alert()
            {
                Category= "Category",
                SubCategory = "SubCategory",
                IsEmail = true,
                IsSms= true
            });

            // Assert
            Assert.IsTrue(result);
            
        }

        /// <summary>
        /// test exception scenario for Post UserAlert
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Controller_AddAlert_ExceptionTest()
        {
            var result = _controller.AddAlert(null, new Alert()
            {
                Category = "Category",
                SubCategory = "SubCategory",
                IsEmail = true,
                IsSms = true
            });
        }
        /// <summary>
        /// test positive scenario for deleting User Alert
        /// </summary>
        [TestMethod]
        public void Controller_DeleteAlert()
        {
            // Arrange
            _mockService.Setup(x => x.DeleteAlert(It.IsAny<string>(), It.IsAny<Alert>())).Returns(true);
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            
            // Act                
            var result = _controller.DeleteAlert("amol.pawar@globant.com", new Alert()
            {
                Category = "Category",
                SubCategory = "SubCategory",
                IsEmail = true,
                IsSms = true
            });

            //Assert
            Assert.IsTrue(result);
        }
        /// <summary>
        /// test positive scenario for deleting User Tag
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Controller_DeleteAlert_Exception()
        {
            // Arrange
           _mockService.Setup(x => x.DeleteTag(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
           _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
           _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost/api/User")
            };
            // Act                
            _controller.DeleteTag("amol.pawar@globant.com", "Cars");
        }
        #endregion AlertUnitTest

        #region UserWishListUnitTest
        [TestMethod]
        public void Controller_GetUserWishListTest_Valid()
        {
            _mockService.Setup(x => x.GetUserWishList(It.IsAny<string>()))
                 .Returns(new string[]
                 {
                     "ListingId1","ListingId2","ListingId3"
                 }
                );
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");

            //Act
            var result = _controller.GetUserWishList("amol.pawar@globant.com");

            //Assert
            Assert.AreEqual(result.Length, 3);
            Assert.IsNotNull(result);
       }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Controller_GetUserWishListTest_Exception()
        {
            _mockService.Setup(x => x.GetUserWishList(It.IsAny<string>()))
                 .Returns(new string[]
                 {
                     "ListingId1","ListingId2","ListingId3"
                 }
                );
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
          
            //Act
            var result = _controller.GetUserWishList("amol.pawar@globant.com");

            //Assert
            Assert.AreEqual(result.Length, 3);
            Assert.IsNotNull(result);
        }
        #endregion UserWishListUnitTest
        #region UserRecommondedTagUnitTest
        [TestMethod]
        public void Controller_GetRecommondedTagListTest_Valid()
        {
            _mockService.Setup(x => x.GetRecommondedTagList(It.IsAny<string>()))
                 .Returns(new Tags()
                 {
                     SubCategory = new string[] { "SubCategory1", "SubCategory2" },
                     Location = new string[] { "Location1", "Location2" }
                 }
                );
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");

            //Act
            var result = _controller.GetRecommondedTagList("amol.pawar@globant.com");

            //Assert
            Assert.IsNotNull(result.SubCategory);
            Assert.AreEqual(result.Location.Length, 2);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Controller_GetUserTagListTest_Exception()
        {
            _mockService.Setup(x => x.GetRecommondedTagList(It.IsAny<string>()))
                 .Returns(new Tags()
                 {
                     SubCategory = new string[] { "SubCategory1", "SubCategory2" },
                     Location = new string[] { "Location1", "Location2" }
                 }
                );
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));

            //Act
            _controller.GetRecommondedTagList("amol.pawar@globant.com");
        }
        #endregion UserRecommondedTagUnitTest
    }
}
