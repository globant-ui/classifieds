using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Classifieds.Listings.BusinessEntities;
using Classifieds.Listings.BusinessServices;
using System.Collections.Generic;
using Classifieds.ListingsAPI.Controllers;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Net;
using System.Web.Http;
using System.Web.Http.Routing;
using Classifieds.Common;

namespace Classifieds.ListingsAPI.Tests
{
    [TestClass]
    public class ListingControllerTest
    {
        #region Class Variables
        private Mock<IListingService> mockService;
        private Mock<ILogger> logger;
        private readonly List<Listing> classifiedList = new List<Listing>();
        private const string urlLocation = "http://localhost/api/listings";
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IListingService>();
            logger = new Mock<ILogger>();
        }
        #endregion

        #region Setup Methods
        private void SetUpClassifiedsListing()
        {
            var lstListing = GetListObject();
            classifiedList.Add(lstListing);
        }

        private Listing GetListObject()
        {
            Listing listObject = new Listing
            {
                _id = "9",
                ListingType = "test",
                ListingCategory = "Housing",
                SubCategory = "test",
                Title = "test",
                Address = "AAA",
                ContactNo = "1111",
                ContactName = "AAA AAA",
                Configuration = "NA",
                Details = "for rupees 20,000,000,000",
                Brand = "test",
                Price = 123,
                YearOfPurchase = 123,
                ExpiryDate = "test",
                Status = "test",
                Submittedby = "test",
                SubmittedDate = "test",
                IdealFor = "test",
                Furnished = "test",
                FuelType = "test",
                KmDriven = 123,
                YearofMake = 123,
                Dimensions = "test",
                TypeofUse = "test",
                Photos = new string[] { "/Photos/Merc2016.jpg", "/Photos/Merc2016.jpg"}
            };
            return listObject;


        }
        #endregion

        #region Unit Test Cases
        /// <summary>
        /// test positive scenario for Get Listing By Id  
        /// </summary>
        [TestMethod]
        public void GetListingByIdTest()
        {
            SetUpClassifiedsListing();
            mockService.Setup(x => x.GetListingById(It.IsAny<string>()))
                .Returns(classifiedList);

            logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            var controller = new ListingsController(mockService.Object, logger.Object);

            //Act           
            var objList = controller.GetListingById("123");

            //Assert
            Assert.AreEqual(objList.Count, 1);
            Assert.AreEqual(objList[0].Title, "test");
        }

        /// <summary>
        ///test positive scenario for Get Listings By SubCategory 
        /// </summary>
        [TestMethod]
        public void GetListingsBySubCategoryTest()
        {
            SetUpClassifiedsListing();
            mockService.Setup(x => x.GetListingsBySubCategory(It.IsAny<string>()))
                .Returns(classifiedList);
            logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            var controller = new ListingsController(mockService.Object, logger.Object);

            //Act            
            var objList = controller.GetListingsBySubCategory("test");

            //Assert
            Assert.AreEqual(objList.Count, 1);
            Assert.AreEqual(objList[0].SubCategory, "test");
        }

        /// <summary>
        /// test for null listing id giving exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Controller_GetListingById_ThrowsException()
        {           
            var controller = new ListingsController(mockService.Object, logger.Object);
            controller.GetListingById(null);
        }

        /// <summary>
        /// test for null subcategory giving exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Controller_GetListingsBySubCategory_ThrowsException()
        {           
            var controller = new ListingsController(mockService.Object, logger.Object);
            controller.GetListingsBySubCategory(null);
        }

        /// <summary>
        /// test positive scenario get listing collection by category
        /// </summary>
        [TestMethod]
        public void GetListingsByCategory_ReturnsList()
        {
            //Arrange            
            SetUpClassifiedsListing();

            //Act
            mockService.Setup(service => service.GetListingsByCategory(It.IsAny<string>())).Returns(classifiedList);
            logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            var controller = new ListingsController(mockService.Object, logger.Object);
            var values = controller.GetListingsByCategory("Housing");

            //Assert
            Assert.AreEqual(values.Count, 1);
            Assert.AreEqual(values[0], classifiedList[0]);
        }

        /// <summary>
        ///  test for null category giving exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetListingByCategory_ThrowsException()
        {           
            var controller = new ListingsController(mockService.Object, logger.Object);
            controller.GetListingsByCategory(null);
        }

        /// <summary>
        /// test positive scenario for PostList and verify response header location
        /// </summary>
        [TestMethod]
        public void Controller_PostListTest_SetsLocationHeader()
        {
            // Arrange
            mockService.Setup(x => x.CreateListing(It.IsAny<Listing>()))
                .Returns(GetListObject());
            logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            ListingsController controller = new ListingsController(mockService.Object, logger.Object);

            controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(urlLocation)
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "Listings",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "listings" } });

            // Act
            Listing listObj = GetListObject();
            var response = controller.Post(listObj);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
            Assert.AreEqual(urlLocation + "/9", response.Headers.Location.AbsoluteUri);
        }

        /// <summary>
        /// test positive scenario for postlist by using mock url helper
        /// </summary>
        [TestMethod]
        public void Controller_PostListTest_SetsLocationHeader_MockURLHelperVersion()
        {
            // This version uses a mock UrlHelper.
            // Arrange           
            mockService.Setup(x => x.CreateListing(It.IsAny<Listing>()))
                .Returns(GetListObject());
            logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            ListingsController controller = new ListingsController(mockService.Object, logger.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            string locationUrl = "http://localhost/ListingsAPI/api/listings";

            // Create the mock and set up the Link method, which is used to create the Location header.
            // The mock version returns a fixed string.
            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            controller.Url = mockUrlHelper.Object;

            // Act
            Listing listObj = GetListObject();
            var response = controller.Post(listObj);

            // Assert
            Assert.AreEqual(locationUrl, response.Headers.Location.AbsoluteUri);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        /// <summary>
        /// test for inserting null listing object throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_PostList_ThrowsException()
        {
            var controller = new ListingsController(mockService.Object, logger.Object);
            controller.Post(null);
        }

        /// <summary>
        /// test positive scenario of Delete listing
        /// </summary>     
        [TestMethod]
        public void Controller_DeleteListTest()
        {
            // Arrange
            Listing listObject = GetListObject();
            mockService.Setup(x => x.DeleteListing(It.IsAny<string>()));//.Returns(GetListObject());
            logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            var controller = new ListingsController(mockService.Object, logger.Object);
            controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(urlLocation)
            };
            // Act                
            var response = controller.Delete(listObject._id);

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        /// <summary>
        /// test for null listing id throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_DeleteList_ThrowsException()
        {
            var controller = new ListingsController(mockService.Object, logger.Object);
            controller.Delete(null);
        }

        /// <summary>
        /// test positive scenario for updating listing
        /// </summary>
        [TestMethod]
        public void Controller_UpdateListTest()
        {
            // Arrange
            mockService.Setup(x => x.UpdateListing(It.IsAny<string>(), It.IsAny<Listing>()))
                .Returns(GetListObject());
            logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            ListingsController controller = new ListingsController(mockService.Object, logger.Object);

            controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(urlLocation)
            };
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            // Act     
            var listObject = GetListObject();
            var updatedProduct = new Listing() { Title = listObject.Title, ListingType = listObject.ListingType };
            var contentResult = controller.Put(listObject._id, updatedProduct);

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
            //Assert.That(listObject._id, Is.EqualTo("9")); // hasn't changed
        }

        /// <summary>
        ///  test for updating listing with null listing id throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_UpdateList_ThrowsException()
        {
            var controller = new ListingsController(mockService.Object, logger.Object);
            var updatedProduct = new Listing() { Title = "test", ListingType = "test" };
            var result = controller.Put(null, updatedProduct);
        }

        /// <summary>
        /// test positive scenario for get top listing collection as per specyfied record count
        /// </summary>
        [TestMethod]
        public void GetTopListings_5RecordsTest()
        {
            //Arrange
            List<Listing> list = new List<Listing>();
            for (int i = 0; i < 5; i++) 
            {
                list.Add(GetListObject());
            }
            mockService.Setup(x => x.GetTopListings(It.IsAny<int>())).Returns(list);
            logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));

            var controller = new ListingsController(mockService.Object, logger.Object);

            //Act            
            var objList = controller.GetTopListings(5);

            //Assert
            Assert.AreEqual(objList.Count, 5);
        }

        /// <summary>
        /// test positive scenario for get top listing, by default returns 10 records
        /// </summary>
        [TestMethod]
        public void GetTopListings_Defualt_noOfRecords_Test()
        {
            //Arrange
            List<Listing> list = new List<Listing>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(GetListObject());
            }
            mockService.Setup(x => x.GetTopListings(It.IsAny<int>())).Returns(list);
            logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));

            var controller = new ListingsController(mockService.Object, logger.Object);

            //Act
            var objList = controller.GetTopListings();

            //Assert
            Assert.AreEqual(objList.Count, 10);
        }       
        #endregion
    }
}
