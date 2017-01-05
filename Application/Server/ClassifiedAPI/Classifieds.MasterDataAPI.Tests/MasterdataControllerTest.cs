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

        [TestMethod]
        public void GetAllCategoryTest()
        {
            var mockService = new Mock<IMasterDataService>();
            var logger = new Mock<ILogger>();

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

            var controller = new CategoryController(mockService.Object, logger.Object);

            //Act
            List<Category> objList = new List<Category>();
            objList = controller.GetAllCategory();

            //Assert
            Assert.AreEqual(objList.Count, 1);
            Assert.AreEqual(objList[0].SubCategory[0], "Test1");
        }

        [TestMethod]
        public void GetAllCategory_EmptyCategoryTest()
        {
            var mockService = new Mock<IMasterDataService>();
            mockService.Setup(x => x.GetAllCategory())
             .Returns(new List<Category>() { new Category() });

            var logger = new Mock<ILogger>();
            var controller = new CategoryController(mockService.Object, logger.Object);
            var result = controller.GetAllCategory();
        }

        [TestMethod]
        public void Controller_PostMasterDataTest()
        {
            // Arrange
            var mockService = new Mock<IMasterDataService>();
            var logger = new Mock<ILogger>();
            mockService.Setup(x => x.CreateCategory(It.IsAny<Category>()))
                .Returns(GetDataObject());
            CategoryController controller = new CategoryController(mockService.Object, logger.Object);

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
            var mockService = new Mock<IMasterDataService>();
            var logger = new Mock<ILogger>();
            mockService.Setup(x => x.CreateCategory(It.IsAny<Category>()))
                .Returns(GetDataObject());
            CategoryController controller = new CategoryController(mockService.Object, logger.Object);
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
                ListingCategory = "test",
                SubCategory = new String[] { "Test1", "Test2", "Test3" },
                Image = "Automobile.png"

            };
            return dataObject;
        }

        [TestMethod]
        public void Controller_DeleteCategoryTest()
        {
            // Arrange
            var mockService = new Mock<IMasterDataService>();
            Category dataObject = GetDataObject();
            var logger = new Mock<ILogger>();
            mockService.Setup(x => x.DeleteCategory(It.IsAny<string>()));
            var controller = new CategoryController(mockService.Object, logger.Object);
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
            var logger = new Mock<ILogger>();
            var mockService = new Mock<IMasterDataService>();
            var controller = new CategoryController(mockService.Object, logger.Object);
            var result = controller.Delete(null);
        }

        [TestMethod]
        public void Controller_UpdateMasterDataTest()
        {
            // Arrange
            var mockService = new Mock<IMasterDataService>();
            var logger = new Mock<ILogger>();
            mockService.Setup(x => x.UpdateCategory(It.IsAny<string>(), It.IsAny<Category>()))
                .Returns(GetDataObject());
            CategoryController controller = new CategoryController(mockService.Object, logger.Object);

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
            var logger = new Mock<ILogger>();
            var mockService = new Mock<IMasterDataService>();
            var controller = new CategoryController(mockService.Object, logger.Object);
            var updatedProduct = new Category() { ListingCategory = "test" };
            var result = controller.Put(null, updatedProduct);
        }

        #endregion
    }
}
