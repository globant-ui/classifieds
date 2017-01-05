using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Classifieds.Listings.BusinessEntities;
using Classifieds.Listings.BusinessServices;
using Classifieds.Listings.Repository;
using System.Collections.Generic;
using Classifieds.ListingsAPI.Controllers;
using System.Collections;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Net;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Classifieds.ListingsAPI.Tests
{
    [TestClass]
    public class ListingControllerTest
    {
        #region Unit Test Cases
        [TestMethod]
        public void GetListingByIdTest()
        {
            var mockService = new Mock<IListingService>();
            mockService.Setup(x => x.GetListingById(It.IsAny<string>()))
                .Returns(
                new List<Listing>
                { new Listing
                   {
                        ListingType = "test",
                        ListingCategory = "test",
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
                        Photos = "test"
                   }
                });

            var controller = new ListingsController(mockService.Object);

            //Act
            List<Listing> objList = new List<Listing>();
            objList = controller.GetListingById("123");

            //Assert
            Assert.AreEqual(objList.Count, 1);
            Assert.AreEqual(objList[0].Title, "test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_GetListingById_ThrowsException()
        {
            var mockService = new Mock<IListingService>();
            var controller = new ListingsController(mockService.Object);
            var result = controller.GetListingById(null);
        }

        [TestMethod]
        public void GetListingsByCategory_ReturnsList()
        {
            //Arrange
            var listingServiceMock = new Mock<IListingService>();
            List<Listing> listings = new List<Listing>();
            Listing testListing = new Listing();
            testListing.ContactName = "Raju";
            testListing.ContactNo = "Main road facing";
            testListing.ExpiryDate = "13/02/2017";
            testListing.Furnished = "Non";
            testListing.ListingCategory = "Housing";
            testListing.ListingType = "Sell";
            testListing.Price = 4250000;
            testListing.Status = "Active";
            testListing.SubCategory = "2BHK";
            testListing.Submittedby = "Ashish";
            testListing.SubmittedDate = DateTime.Now.ToShortDateString();
            testListing.Title = "2BHK Flat";
            listings.Add(testListing);

            //Act
            listingServiceMock.Setup(service => service.GetListingsByCategory(It.IsAny<string>())).Returns(listings);
            var controller = new ListingsController(listingServiceMock.Object);
            var values = controller.GetListingsByCategory("Housing");

            //Assert
            Assert.AreEqual(values.Count, 1);
            Assert.AreEqual(values[0], testListing);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetListingByCategory_ThrowsException()
        {
            var mockService = new Mock<IListingService>();
            var controller = new ListingsController(mockService.Object);
            var result = controller.GetListingsByCategory(null);
        }

        [TestMethod]
        public void Controller_PostListTest_SetsLocationHeader()
        {
            // Arrange
            var mockService = new Mock<IListingService>();
            mockService.Setup(x => x.CreateListing(It.IsAny<Listing>()))
                .Returns(GetListObject());
            ListingsController controller = new ListingsController(mockService.Object);

            controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/api/listings")
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
            Assert.AreEqual("http://localhost/api/listings/9", response.Headers.Location.AbsoluteUri);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void Controller_PostListTest_SetsLocationHeader_MockURLHelperVersion()
        {
            // This version uses a mock UrlHelper.
            // Arrange
            var mockService = new Mock<IListingService>();
            mockService.Setup(x => x.CreateListing(It.IsAny<Listing>()))
                .Returns(GetListObject());
            ListingsController controller = new ListingsController(mockService.Object);
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

        private Listing GetListObject()
        {
            Listing listObject = new Listing
            {
                _id = "9",
                ListingType = "test",
                ListingCategory = "test",
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
                Photos = "test"
            };
            return listObject;


        }

        [TestMethod]
        public void Controller_DeleteListTest()
        {
            // Arrange
            var mockService = new Mock<IListingService>();
            Listing listObject = GetListObject();
            mockService.Setup(x => x.DeleteListing(It.IsAny<string>()));//.Returns(GetListObject());
            var controller = new ListingsController(mockService.Object);
            controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost/api/listings")
            };
            // Act                
            var response = controller.Delete(listObject._id);

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_DeleteList_ThrowsException()
        {
            var mockService = new Mock<IListingService>();
            var controller = new ListingsController(mockService.Object);
            var result = controller.Delete(null);
        }

        [TestMethod]
        public void Controller_UpdateListTest()
        {
            // Arrange
            var mockService = new Mock<IListingService>();
            mockService.Setup(x => x.UpdateListing(It.IsAny<string>(), It.IsAny<Listing>()))
                .Returns(GetListObject());
            ListingsController controller = new ListingsController(mockService.Object);

            controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("http://localhost/api/listings")
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_UpdateList_ThrowsException()
        {
            var mockService = new Mock<IListingService>();
            var controller = new ListingsController(mockService.Object);
            var updatedProduct = new Listing() { Title = "test", ListingType = "test" };
            var result = controller.Put(null, updatedProduct);
        }

        #endregion
    }
}
