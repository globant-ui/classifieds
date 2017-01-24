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

namespace Classifieds.MasterDataAPI.Tests
{
    [TestClass]
    public class MasterdataControllerTest
    {
        #region Unit Test Cases

        #region Class Variables
        private Mock<IMasterDataService> mockService;
        private Mock<ILogger> logger;
        private Mock<ICommonDBRepository> mockAuthRepo;
        private readonly List<Category> classifiedList = new List<Category>();
        private readonly List<string> categoryList = new List<string>();
        private const string urlLocation = "http://localhost/api/listings";
        private CategoryController controller;
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IMasterDataService>();
            logger = new Mock<ILogger>();
            mockAuthRepo = new Mock<ICommonDBRepository>();
            controller = new CategoryController(mockService.Object, logger.Object, mockAuthRepo.Object);
        }
        #endregion

        #region Setup Methods

        private void SetUpClassifiedsListing()
        {
            var lstListing = GetDataObject();
            classifiedList.Add(lstListing);
            categoryList.Add("Automotive");
        }

        #endregion

        [TestMethod]
        public void GetAllCategoryTest()
        {
                 mockService.Setup(x => x.GetAllCategory())
                .Returns(
                new List<Category>
                { new Category
                   {
                         ListingCategory = "test",
                         SubCategory = new String[] { "Test1", "Test2", "Test3" },
                         Image="Automobile.png"
                    }
           });

            //Act
            List<Category> objList = new List<Category>();
            objList = controller.GetAllCategory();

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
            mockService.Setup(x => x.GetCategorySuggetion(It.IsAny<string>()))
              .Returns(categoryList);
            logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));

            //Act
            List<string> objList = new List<string>();
            objList = controller.GetCategorySuggetion("Auto");

            //Assert
            Assert.AreEqual(objList.Count, 1);
            Assert.AreEqual(objList[0].ToString(), "Automotive");
        }

        /// <summary>
        /// test for null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Controller_GetCategorySuggetion_ThrowsException()
        {
            controller.GetCategorySuggetion(null);
        }

        [TestMethod]
        public void GetAllCategory_EmptyCategoryTest()
        {
            mockService.Setup(x => x.GetAllCategory())
            .Returns(new List<Category>() { new Category() });
            var result = controller.GetAllCategory();
        }

         [TestMethod]
        public void Controller_PostMasterDataTest()
        {
            // Arrange
            mockService.Setup(x => x.CreateCategory(It.IsAny<Category>()))
            .Returns(GetDataObject());
            controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "Category",
                routeTemplate: "api/{controller}/{method}/{id}",
                defaults: new { id = RouteParameter.Optional });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "Category" } });

            // Act
            Category listObj = GetDataObject();
            var response = controller.Post(listObj);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void Controller_PostMasterDataTest_SetsLocationHeader_MockURLHelperVersion()
        {
            // This version uses a mock UrlHelper.
            // Arrange
            mockService.Setup(x => x.CreateCategory(It.IsAny<Category>()))
            .Returns(GetDataObject());
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            string locationUrl = "http://localhost/Classifieds.MasterDataAPI/api/Category";

            // Create the mock and set up the Link method, which is used to create the Location header.
            // The mock version returns a fixed string.
            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            controller.Url = mockUrlHelper.Object;

            // Act
            Category listObj = GetDataObject();
            var response = controller.Post(listObj);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        private Category GetDataObject()
        {
            Category dataObject = new Category
            {
                _id = "9",
                ListingCategory = "Automotive",
                SubCategory = new String[] { "Car",
                                            "Motor Cycle",
                                            "Scooter",
                                            "Bicycle",
                                            "Accessories" },
                Image = "Automotive.png"

            };
            return dataObject;
        }

        [TestMethod]
        public void Controller_DeleteCategoryTest()
        {
            // Arrange
            Category dataObject = GetDataObject();
            mockService.Setup(x => x.DeleteCategory(It.IsAny<string>()));
            controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost/api/Category")
            };
            // Act                
            var response = controller.Delete(dataObject._id);

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_DeleteList_ThrowsException()
        {
            var result = controller.Delete(null);
        }

        [TestMethod]
        public void Controller_UpdateMasterDataTest()
        {
            // Arrange
            mockService.Setup(x => x.UpdateCategory(It.IsAny<string>(), It.IsAny<Category>()))
            .Returns(GetDataObject());

            controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("http://localhost/api/Category")
            };
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            // Act     
            var dataObject = GetDataObject();
            var updatedProduct = new Category() { ListingCategory = dataObject.ListingCategory };
            var contentResult = controller.Put(dataObject._id, updatedProduct);

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
            var result = controller.Put(null, updatedProduct);
        }

        #endregion


    }
}
