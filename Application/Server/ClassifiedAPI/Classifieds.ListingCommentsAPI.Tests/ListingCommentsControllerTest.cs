using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Classifieds.ListingComments.BusinessEntities;
using Classifieds.ListingComments.BusinessServices;
using System.Collections.Generic;
using Classifieds.ListingCommentsAPI.Controllers;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Net;
using System.Web.Http;
using System.Web.Http.Routing;
using Classifieds.Common;

namespace Classifieds.ListingCommentsAPI.Tests
{
    [TestClass]
    public class ListingCommentsControllerTest
    {
        #region Unit Test Cases

        #region Class Variables
        private Mock<IListingCommentService> mockService;
        private Mock<ILogger> logger;
        private readonly List<ListingComment> classifiedList = new List<ListingComment>();
        private const string urlLocation = "http://localhost/api/ListingComments";
        private const string listingId = "5873490a48bd151ef5d67a29";

        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IListingCommentService>();
            logger = new Mock<ILogger>();
        }
        #endregion

        #region Setup Methods

        private void SetUpClassifiedsListingComments()
        {
            var lstListingcomments = GetlistingCommentsObject();
            classifiedList.Add(lstListingcomments);
        }

        #endregion

        #region GetAllListingCommentTest

        [TestMethod]
        /// <summary>
        /// test positive scenario for Get All ListingComments
        /// </summary>
        public void GetAllListingCommentTest()
        {
            SetUpClassifiedsListingComments();
            mockService.Setup(x => x.GetAllListingComment(listingId))
                .Returns(classifiedList);

            logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            var controller = new ListingCommentsController(mockService.Object, logger.Object);

            //Act
            var objList = controller.GetAllListingComment(listingId);

            //Assert
            Assert.AreEqual(objList.Count, 1);
            Assert.AreEqual(objList[0].SubmittedBy, "v.wadsamudrakar@globant.com");
        }

        [TestMethod]
        /// <summary>
        /// test negative scenario for empty Listing Comment
        /// </summary>
        public void GetAllListingComment_EmptyListingCommentsTest()
        {
            mockService.Setup(x => x.GetAllListingComment(null))
            .Returns(new List<ListingComment>() { new ListingComment() });
            var controller = new ListingCommentsController(mockService.Object, logger.Object);
        }

        /// <summary>
        /// test for null exception for all category
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_GetListingComments_ThrowsException()
        {
            var controller = new ListingCommentsController(mockService.Object, logger.Object);
            controller.GetAllListingComment(null);
        }

        #endregion GetAllListingCommentTest

        #region PostListingCommentsTestcases

        [TestMethod]
        /// <summary>
        /// test positive scenario for Post Listing Comments
        /// </summary>
        public void Controller_PostListingCommentsTest()
        {
            // Arrange
            mockService.Setup(x => x.CreateListingComment(It.IsAny<ListingComment>()))
            .Returns(GetlistingCommentsObject());
            logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            ListingCommentsController controller = new ListingCommentsController(mockService.Object, logger.Object);
            controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "ListingComments",
                routeTemplate: "api/{controller}/{method}/{id}",
                defaults: new { id = RouteParameter.Optional });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "ListingComments" } });

            // Act
            ListingComment listObj = GetlistingCommentsObject();
            var response = controller.Post(listObj);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        [TestMethod]
        /// <summary>
        /// test positive scenario for Post ListingComments and verify response header location
        /// </summary>
        public void Controller_PostListingCommentsTest_SetsLocationHeader_MockURLHelperVersion()
        {
            // This version uses a mock UrlHelper.
            // Arrange
            mockService.Setup(x => x.CreateListingComment(It.IsAny<ListingComment>()))
            .Returns(GetlistingCommentsObject());
            logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            ListingCommentsController controller = new ListingCommentsController(mockService.Object, logger.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            string locationUrl = "http://localhost/Classifieds.ListingCommentsAPI/api/ListingComments";

            // Create the mock and set up the Link method, which is used to create the Location header.
            // The mock version returns a fixed string.
            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            controller.Url = mockUrlHelper.Object;

            // Act
            ListingComment listObj = GetlistingCommentsObject();
            var response = controller.Post(listObj);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        /// <summary>
        /// test for inserting null ListingComment object throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_PostListingComment_ThrowsException()
        {
            var controller = new ListingCommentsController(mockService.Object, logger.Object);
            controller.Post(null);
        }

        #endregion PostListingCommentsTestcases

        #region UpdateListingCommentsTestCases

        [TestMethod]
        /// <summary>
        /// test positive scenario for updating listing Comments
        /// </summary>
        public void Controller_UpdateListingCommentsTest()
        {
            // Arrange
            mockService.Setup(x => x.UpdateListingComment(It.IsAny<string>(), It.IsAny<ListingComment>()))
            .Returns(GetlistingCommentsObject());
            logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            ListingCommentsController controller = new ListingCommentsController(mockService.Object, logger.Object);

            controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("http://localhost/api/listingComments")
            };
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            // Act     
            var dataObject = GetlistingCommentsObject();
            var updatedProduct = new ListingComment() { UpdatedBy = dataObject.UpdatedBy };
            var contentResult = controller.Put(dataObject._id, updatedProduct);

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        /// <summary>
        ///  test for updating listing with null listing id throws exception
        /// </summary>
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_UpdateListingComments_ThrowsException()
        {
            var controller = new ListingCommentsController(mockService.Object, logger.Object);
            var updatedProduct = new ListingComment() { UpdatedBy = "amol.pawar@globant.com", UpdatedDate = "10/01/2017", Comments = "is this available", ListingId = "5873490a48bd151ef5d67a29", SubmittedDate = "03/01/2017", SubmittedBy = "v.wadsamudrakar@globant.com" };
            var result = controller.Put(null, updatedProduct);
        }


        /// <summary>
        /// for Get Data Object of ListingComments
        /// </summary>
        private ListingComment GetlistingCommentsObject(string ListingId = "5873490a48bd151ef5d67a29")
        {
            ListingComment dataObject = new ListingComment
            {
                _id = "58786bf761f1b82779e0c5b5",
                ListingId = "5873490a48bd151ef5d67a29",
                SubmittedDate = "03/01/2017",
                SubmittedBy = "v.wadsamudrakar@globant.com",
                Verified = true,
                Comments = "Please share you contact details i wanna buy..",
                UpdatedDate = "16/01/2017",
                UpdatedBy = "amol.pawar@globant.com",
            };
            return dataObject;
        }

        #endregion UpdateListingCommentsTestCases

        #region DeleteListingCommentsTestCases

        [TestMethod]
        /// <summary>
        /// test positive scenario for deleting ListingComments
        /// </summary>
        public void Controller_DeleteListingCommentsTest()
        {
            // Arrange
            ListingComment dataObject = GetlistingCommentsObject();
            mockService.Setup(x => x.DeleteListingComment(It.IsAny<string>()));
            logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            var controller = new ListingCommentsController(mockService.Object, logger.Object);
            controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost/api/listingComments")
            };
            // Act                
            var response = controller.Delete(dataObject._id);

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        [TestMethod]
        /// <summary>
        /// test for deleting listing comments object throws exception
        /// </summary>
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_DeleteListingComments_ThrowsException()
        {
            var controller = new ListingCommentsController(mockService.Object, logger.Object);
            var result = controller.Delete(null);
        }

        #endregion  DeleteListingCommentsTestCases        

        #endregion
    }
}
