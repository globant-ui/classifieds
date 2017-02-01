using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Classifieds.MastersData.BusinessEntities;
using Classifieds.MastersData.BusinessServices;
using System.Collections.Generic;
using Classifieds.MasterDataAPI.Controllers;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Net;
using System.Web.Http;
using System.Web.Http.Routing;
using Classifieds.Common;
using Classifieds.Common.Repositories;

namespace Classifieds.MasterDataAPI.Tests
{
    [TestClass]
    public class MasterdataControllerTest
    {
        #region Unit Test Cases

        #region Class Variables
        private Mock<IMasterDataService> _mockService;
        private Mock<ILogger> _logger;
        private Mock<ICommonRepository> _mockAuthRepo;
        private readonly List<string> _categoryList = new List<string>();
        private CategoryController _controller;
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _mockService = new Mock<IMasterDataService>();
            _logger = new Mock<ILogger>();
            _mockAuthRepo = new Mock<ICommonRepository>();
            _controller = new CategoryController(_mockService.Object, _logger.Object, _mockAuthRepo.Object);
        }
        #endregion

        #region Setup Methods

        private void SetUpClassifiedsListing()
        {
            _categoryList.Add("Automotive");
        }

        #endregion

        [TestMethod]
        public void GetAllCategoryTest()
        {
                 _mockService.Setup(x => x.GetAllCategory())
                .Returns(
                new List<Category>
                { new Category
                   {
                         ListingCategory = "test",
                         SubCategory = new [] { "Test1", "Test2", "Test3" },
                         Image="Automobile.png"
                    }
           });
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");

            //Act
            List<Category> objList = _controller.GetAllCategory();

            //Assert
            Assert.AreEqual(objList.Count, 1);
            Assert.AreEqual(objList[0].SubCategory[0], "Test1");
        }

        /// <summary>
        ///test positive scenario for Get Category list for maching input
        /// </summary>
        [TestMethod]
        public void GetCategorySuggetionTest()
        {
            SetUpClassifiedsListing();
            _mockService.Setup(x => x.GetCategorySuggetion(It.IsAny<string>()))
              .Returns(_categoryList);
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");

            //Act
            List<string> objList = _controller.GetCategorySuggetion("Auto");

            //Assert
            Assert.AreEqual(objList.Count, 1);
            Assert.AreEqual(objList[0], "Automotive");
        }

        /// <summary>
        /// test for null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_GetCategorySuggetion_ThrowsException()
        {
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.GetCategorySuggetion(null);
        }

        [TestMethod]
        public void GetAllCategory_EmptyCategoryTest()
        {
            _mockService.Setup(x => x.GetAllCategory())
            .Returns(new List<Category>() { new Category() });
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.GetAllCategory();
        }

         [TestMethod]
        public void Controller_PostMasterDataTest()
        {
            // Arrange
            _mockService.Setup(x => x.CreateCategory(It.IsAny<Category>()))
            .Returns(GetDataObject());
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
            };
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Configuration = new HttpConfiguration();
            _controller.Configuration.Routes.MapHttpRoute(
                name: "Category",
                routeTemplate: "api/{controller}/{method}/{id}",
                defaults: new { id = RouteParameter.Optional });

            _controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "Category" } });

            // Act
            Category listObj = GetDataObject();
            var response = _controller.Post(listObj);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void Controller_PostMasterDataTest_SetsLocationHeader_MockURLHelperVersion()
        {
            // This version uses a mock UrlHelper.
            // Arrange
            _mockService.Setup(x => x.CreateCategory(It.IsAny<Category>()))
            .Returns(GetDataObject());
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            string locationUrl = "http://localhost/Classifieds.MasterDataAPI/api/Category";

            // Create the mock and set up the Link method, which is used to create the Location header.
            // The mock version returns a fixed string.
            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            _controller.Url = mockUrlHelper.Object;

            // Act
            Category listObj = GetDataObject();
            var response = _controller.Post(listObj);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void Controller_DeleteCategoryTest()
        {
            // Arrange
            Category dataObject = GetDataObject();
            _mockService.Setup(x => x.DeleteCategory(It.IsAny<string>()));
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost/api/Category")
            };
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            // Act                
            var response = _controller.Delete(dataObject._id);

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_DeleteList_ThrowsException()
        {
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Delete(null);
        }

        [TestMethod]
        public void Controller_UpdateMasterDataTest()
        {
            // Arrange
            _mockService.Setup(x => x.UpdateCategory(It.IsAny<string>(), It.IsAny<Category>()))
            .Returns(GetDataObject());
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("http://localhost/api/Category")
            };
            _controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            // Act     
            var dataObject = GetDataObject();
            var updatedProduct = new Category() { ListingCategory = dataObject.ListingCategory };
            var contentResult = _controller.Put(dataObject._id, updatedProduct);

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_UpdateMasterData_ThrowsException()
        {
            var updatedProduct = new Category() { ListingCategory = "test", Image = "" };
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Put(null, updatedProduct);
        }

        #endregion

        #region private methods
        private Category GetDataObject()
        {
            Category dataObject = new Category
            {
                _id = "9",
                ListingCategory = "Automotive",
                SubCategory = new [] { "Car",
                                            "Motor Cycle",
                                            "Scooter",
                                            "Bicycle",
                                            "Accessories" },
                Image = "Automotive.png"

            };
            return dataObject;
        }
        #endregion
    }
}
