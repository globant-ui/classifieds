using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Classifieds.MastersData.BusinessEntities;
using Classifieds.MastersData.BusinessServices;
using System.Collections.Generic;
using Classifieds.MastersDataAPI.Controllers;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Net;
using System.Web.Http;
using System.Web.Http.Routing;
using Classifieds.Common;
using Classifieds.Common.Repositories;

namespace Classifieds.MastersDataAPI.Tests
{
    [TestClass]
    [Ignore]
    public class MasterdataControllerTest
    {
        #region Unit Test Cases

        #region Class Variables

        private Mock<IMasterDataService> _mockService;
        private Mock<ILogger> _logger;
        private Mock<ICommonRepository> _mockAuthRepo;
        private readonly List<Category> _classifiedList = new List<Category>();
        private readonly List<string> _categoryList = new List<string>();
        private const string UrlLocation = "http://localhost/api/Category";
        private CategoryController _controller;
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _mockService = new Mock<IMasterDataService>();
            _logger = new Mock<ILogger>();
            _mockService = new Mock<IMasterDataService>();
            _controller = new CategoryController(_mockService.Object, _logger.Object, _mockAuthRepo.Object);
        }
        #endregion

        #region Setup Methods

        private void SetUpClassifiedsListing()
        {
            var lstcategory = GetCategoryDataObject();
            _classifiedList.Add(lstcategory);
            _categoryList.Add("Automotive");
        }

        #endregion

        #region GetAllCategoryTestCases


        /// <summary>
        /// test positive scenario for Get All category
        /// </summary>
        [TestMethod]
        public void GetAllCategoryTest()
        {
            SetUpClassifiedsListing();
            _mockService.Setup(x => x.GetAllCategory())
           .Returns(_classifiedList);
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));

            //Act
            var objList = new List<Category>();
            objList = _controller.GetAllCategory();

            //Assert
            Assert.AreEqual(objList.Count, 1);
            Assert.AreEqual(objList[0].SubCategory[0], "Car");
        }

        /// <summary>
        /// test for null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_GetCategory_ThrowsException()
        {
            _controller.GetAllCategory();
        }

        /// <summary>
        /// test negative scenario for empty category
        /// </summary>
        [TestMethod]
        public void GetAllCategory_EmptyCategoryTest()
        {
            _mockService.Setup(x => x.GetAllCategory())
            .Returns(new List<Category>());
            var result = _controller.GetAllCategory();

            //Assert
            Assert.AreEqual(result.Count, 0);
        }

        #endregion GetAllCategoryTestCases

        #region GetCategorySuggetionTest

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

            //Act
            var objList = new List<string>();
            objList = _controller.GetCategorySuggetion("Auto");

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
            _controller.GetCategorySuggetion(null);
        }

        #endregion GetCategorySuggetionTest

        #region PostMasterDataTestCases


        /// <summary>
        /// test positive scenario for Post category
        /// </summary>
        [TestMethod]
        public void Controller_PostCategoryTest()
        {
            // Arrange
            _mockService.Setup(x => x.CreateCategory(It.IsAny<Category>()))
            .Returns(GetCategoryDataObject());
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
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
                values: new HttpRouteValueDictionary { { "controller", "Category" } });

            // Act
            var listObj = GetCategoryDataObject();
            var response = _controller.Post(listObj);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        /// <summary>
        /// test for inserting null listing object throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_PostCategory_ThrowsException()
        {
            _controller.Post(null);
        }

        /// <summary>
        /// test positive scenario for PostCategory and verify response header location
        /// </summary>
        [TestMethod]
        public void Controller_PostcategoryTest_SetsLocationHeader_MockURLHelperVersion()
        {
            // This version uses a mock UrlHelper.
            // Arrange
            _mockService.Setup(x => x.CreateCategory(It.IsAny<Category>()))
            .Returns(GetCategoryDataObject());
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            string locationUrl = "http://localhost/Classifieds.MastersDataAPI/api/Category";

            // Create the mock and set up the Link method, which is used to create the Location header.
            // The mock version returns a fixed string.
            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            _controller.Url = mockUrlHelper.Object;

            // Act
            Category listObj = GetCategoryDataObject();
            var response = _controller.Post(listObj);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        /// <summary>
        /// for Get Data Object of Category
        /// </summary>
        private Category GetCategoryDataObject()
        {
            Category dataObject = new Category
            {
                _id = "9",
                ListingCategory = "Automotive",
                SubCategory = new string[] { "Car",
                                            "Motor Cycle",
                                            "Scooter",
                                            "Bicycle",
                                            "Accessories" },
                Image = "Automotive.png"

            };
            return dataObject;
        }

        #endregion PostMasterDataTestCases

        #region DeleteCategoryTestCases

        /// <summary>
        /// test positive scenario for deleting category
        /// </summary>
        [TestMethod]
        public void Controller_DeleteCategoryTest()
        {
            // Arrange
            var dataObject = GetCategoryDataObject();
            _mockService.Setup(x => x.DeleteCategory(It.IsAny<string>()));
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost/api/Category")
            };
            // Act                
            var response = _controller.Delete(dataObject._id);

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        /// <summary>
        /// test for deleting category object throws exception
        /// </summary>
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Controller_DeleteCatgory_ThrowsException()
        {
            _controller.Delete(null);
        }

        #endregion DeleteCategoryTestCases

        #region UpdateCategoryTestCases

        /// <summary>
        /// test positive scenario for updating Category
        /// </summary>
        [TestMethod]
        public void Controller_UpdateCategoryTest()
        {
            // Arrange
            _mockService.Setup(x => x.UpdateCategory(It.IsAny<string>(), It.IsAny<Category>()))
            .Returns(GetCategoryDataObject());
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("http://localhost/api/Category")
            };
            _controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            // Act     
            var dataObject = GetCategoryDataObject();
            var updatedProduct = new Category() { ListingCategory = dataObject.ListingCategory };
            var contentResult = _controller.Put(dataObject._id, updatedProduct);

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        /// <summary>
        ///  test for update listing with null Category id throws exception
        /// </summary>
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Controller_UpdateCategory_ThrowsException()
        {
            var updatedProduct = new Category() { ListingCategory = "Automotive", Image = "" };
            _controller.Put(null, updatedProduct);
        }

        #endregion UpdateCategoryTestCases

        #endregion
    }
}
