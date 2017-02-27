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
using Classifieds.Common.Repositories;

namespace Classifieds.ListingCommentsAPI.Tests
{
    [TestClass]
    [Ignore]
    public class ListingCommentsControllerTest
    {
        #region Unit Test Cases

        #region Class Variables
        private Mock<IListingCommentService> _mockService;
        private Mock<ICommonRepository> _mockAuthRepo;
        private Mock<ILogger> _logger;
        private readonly List<ListingComment> _classifiedList = new List<ListingComment>();
        private const string ListingId = "5873490a48bd151ef5d67a29";
        private const string UrlLocation = "http://localhost/api/ListingComments";
        private ListingCommentsController _controller;

        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _mockService = new Mock<IListingCommentService>();
            _logger = new Mock<ILogger>();
            _mockAuthRepo = new Mock<ICommonRepository>();
            _controller = new ListingCommentsController(_mockService.Object, _logger.Object, _mockAuthRepo.Object);
        }
        #endregion

        #region Setup Methods

        private void SetUpClassifiedsListingComments()
        {
            var lstListingcomments = GetlistingCommentsObject();
            _classifiedList.Add(lstListingcomments);
        }

        #endregion

        #region GetAllListingCommentTest

        /// <summary>
        /// test positive scenario for Get All ListingComments
        /// </summary>
        [TestMethod]
        public void GetAllListingCommentTest()
        {
            SetUpClassifiedsListingComments();
            _mockService.Setup(x => x.GetAllListingComment(ListingId))
                .Returns(_classifiedList);

            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));

            //Act
            var objList = _controller.GetAllListingComment(ListingId);

            //Assert
            Assert.AreEqual(objList.Count, 1);
            Assert.AreEqual(objList[0].SubmittedBy, "v.wadsamudrakar@globant.com");
        }

        /// <summary>
        /// test negative scenario for empty Listing Comment
        /// </summary>
        [TestMethod]
        public void GetAllListingComment_EmptyListingCommentsTest()
        {
            // Arrange
            _mockService.Setup(x => x.GetAllListingComment(null))
            .Returns(new List<ListingComment>() { new ListingComment() });

            //Act
            _mockService.Setup(x => x.GetAllListingComment(ListingId));

            //Assert
            Assert.IsTrue(true);
        }

        /// <summary>
        /// test for null exception for all category
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_GetListingComments_ThrowsException()
        {
            _controller.GetAllListingComment(null);
        }

        #endregion GetAllListingCommentTest

        #region PostListingCommentsTestcases

        /// <summary>
        /// test positive scenario for Post Listing Comments
        /// </summary>
        [TestMethod]
        public void Controller_PostListingCommentsTest()
        {
            // Arrange
            _mockService.Setup(x => x.CreateListingComment(It.IsAny<ListingComment>()))
            .Returns(GetlistingCommentsObject());
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
            };
            _controller.Configuration = new HttpConfiguration();
            _controller.Configuration.Routes.MapHttpRoute(
                name: "ListingComments",
                routeTemplate: "api/{controller}/{method}/{id}",
                defaults: new { id = RouteParameter.Optional });

            _controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "ListingComments" } });

            // Act
            var listObj = GetlistingCommentsObject();
            var response = _controller.Post(listObj);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        /// <summary>
        /// test positive scenario for Post ListingComments and verify response header location
        /// </summary>
        [TestMethod]
        public void Controller_PostListingCommentsTest_SetsLocationHeader_MockURLHelperVersion()
        {
            // This version uses a mock UrlHelper.
            // Arrange
            _mockService.Setup(x => x.CreateListingComment(It.IsAny<ListingComment>()))
            .Returns(GetlistingCommentsObject());
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            var locationUrl = "http://localhost/Classifieds.ListingCommentsAPI/api/ListingComments";

            // Create the mock and set up the Link method, which is used to create the Location header.
            // The mock version returns a fixed string.
            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            _controller.Url = mockUrlHelper.Object;

            // Act
            var listObj = GetlistingCommentsObject();
            var response = _controller.Post(listObj);

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
            _controller.Post(null);
        }

        #endregion PostListingCommentsTestcases

        #region UpdateListingCommentsTestCases

        /// <summary>
        /// test positive scenario for updating listing Comments
        /// </summary>
        [TestMethod]
        public void Controller_UpdateListingCommentsTest()
        {
            // Arrange
            _mockService.Setup(x => x.UpdateListingComment(It.IsAny<string>(), It.IsAny<ListingComment>()))
            .Returns(GetlistingCommentsObject());
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(UrlLocation)
            };

            _controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            // Act     
            var dataObject = GetlistingCommentsObject();
            var updatedProduct = new ListingComment() { UpdatedBy = dataObject.UpdatedBy };
            var contentResult = _controller.Put(dataObject._id, updatedProduct);

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }


        /// <summary>
        ///  test for updating listing with null listing id throws exception
        /// </summary>

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_UpdateListingComments_ThrowsException()
        {
            var updatedProduct = GetlistingCommentsObject();
            _controller.Put(null, updatedProduct);
        }


        /// <summary>
        /// for Get Data Object of ListingComments
        /// </summary>
        private ListingComment GetlistingCommentsObject()
        {
            var dataObject = new ListingComment
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

        /// <summary>
        /// test positive scenario for deleting ListingComments
        /// </summary>
        [TestMethod]
        public void Controller_DeleteListingCommentsTest()
        {
            // Arrange
            var dataObject = GetlistingCommentsObject();
            _mockService.Setup(x => x.DeleteListingComment(It.IsAny<string>()));
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(UrlLocation)
            };
            // Act                
            var response = _controller.Delete(dataObject._id);

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }
        /// <summary>
        /// test for deleting listing comments object throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_DeleteListingComments_ThrowsException()
        {
            _controller.Delete(null);
        }

        #endregion  DeleteListingCommentsTestCases        

        #endregion
    }
}
