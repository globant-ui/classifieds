using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Classifieds.ListingComments.BusinessEntities;
using Classifieds.ListingComments.Repository;
using Moq;
using Classifieds.ListingComments.BusinessServices;

namespace Classifieds.ListingCommentsAPI.Tests
{
    [TestClass]
    public class ListingCommentsBusinessServiceTest
    {
        #region Unit Test Cases

        #region Class Variables
        private Mock<IListingCommentsRepository<ListingComment>> _moqAppManager;
        private IListingCommentService _service;
        private readonly List<ListingComment> _classifiedlistingcomments = new List<ListingComment>();
        private const string ListingId = "5873490a48bd151ef5d67a29";
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _moqAppManager = new Mock<IListingCommentsRepository<ListingComment>>();
            _service = new ListingCommentService(_moqAppManager.Object);
        }
        #endregion

        #region Setup
        private void SetUpClassifiedsListingComments()
        {
            var lstlistingcomments = GetListingCommentsObject();
            _classifiedlistingcomments.Add(lstlistingcomments);
        }

        private ListingComment GetListingCommentsObject()
        {
            ListingComment dataObject = new ListingComment
            {
                _id = "9",
                UpdatedBy = "santosh.kale@globant.com",
                UpdatedDate = "10/01/2017",
                Comments = "is this available",
                ListingId = "5873490a48bd151ef5d67a29",
                SubmittedBy = "v.wadsamudrakar@globant.com",
                SubmittedDate = "03/01/2017",
                Verified = true
            };
            return dataObject;
        }
        #endregion

        #region GetAllListingCommentCases

        /// <summary>
        /// test negative scenario for throw Exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetAllListingComment_ThrowsException()
        {
            var result = _service.GetAllListingComment(null);
        }

        /// <summary>
        /// tests the positive test criteria for Get All Listing Comments
        /// </summary>
        [TestMethod]
        public void GetAllListingCommentTest()
        {
            // Arrange
            SetUpClassifiedsListingComments();
            _moqAppManager.Setup(x => x.GetAllListingComment(It.IsAny<string>())).Returns(_classifiedlistingcomments);

            //Act
            var result = _service.GetAllListingComment(ListingId);

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsNotNull(result[0]);
        }

        /// <summary>
        /// tests for incorrect input giving empty result
        /// </summary>
        [TestMethod]
        public void GetAllListingComments_EmptyResultTest()
        {
            // Arrange
            SetUpClassifiedsListingComments();
            _moqAppManager.Setup(x => x.GetAllListingComment(It.IsAny<string>())).Returns(new List<ListingComment>() { new ListingComment() });

            //Act
            var result = _service.GetAllListingComment(ListingId);

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(ListingComment));
        }

        #endregion GetAllListingCommentCases

        #region PostListingTestCases

        /// <summary>
        /// test positive scenario for Post Listing Comments
        /// </summary>
        [TestMethod]
        public void PostListingCommentTest()
        {
            //Arrange
            ListingComment lstObject = GetListingCommentsObject();
            _moqAppManager.Setup(x => x.CreateListingComment(It.IsAny<ListingComment>())).Returns(lstObject);

            //Act
            var result = _service.CreateListingComment(lstObject);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(ListingComment));
        }

        /// <summary>
        /// test for inserting empty Listing Comments object return null result
        /// </summary>
        [TestMethod]
        public void PostListingcommentsTest_EmptyList()
        {
            var result = _service.CreateListingComment(null);
            Assert.IsNull(result, null);
        }

        #endregion PostListingTestCases        

        #region UpdateListingCommentsTestCases


        /// <summary>
        /// test positive scenario for Update Listing Comment
        /// </summary>
        [TestMethod]
        public void PutListingCommentTest()
        {
            //Arrange
            ListingComment lstObject = GetListingCommentsObject();
            _moqAppManager.Setup(x => x.UpdateListingComment(It.IsAny<string>(), It.IsAny<ListingComment>())).Returns(lstObject);
            var updatedList = new ListingComment() { UpdatedBy = lstObject.UpdatedBy, UpdatedDate = lstObject.UpdatedDate };
            //Act
            var result = _service.UpdateListingComment(lstObject._id, updatedList);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(ListingComment));
        }


        /// <summary>
        /// test Case for Exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PutListingCommentTest_InvalidId_ThrowException()
        {
            ArgumentNullException ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            var updatedData = new ListingComment() { UpdatedBy = "santosh.kale@globant.com", UpdatedDate = "10/01/2017", Comments = "is this available", ListingId = "5873490a48bd151ef5d67a29", SubmittedDate = "03/01/2017", SubmittedBy = "v.wadsamudrakar@globant.com" };
            _moqAppManager.Setup(x => x.UpdateListingComment(It.IsAny<string>(), It.IsAny<ListingComment>())).Throws(ex);
            var result = _service.UpdateListingComment(null, updatedData);

        }

        #endregion UpdateListingCommentsTestCases

        #region DeleteListingTestCases


        /// <summary>
        /// test positive scenario for Delete Listing Comments
        /// </summary>  
        [TestMethod]
        public void DeleteListingCommentTest()
        {
            //Arrange
            ListingComment lstObject = GetListingCommentsObject();
            _moqAppManager.Setup(x => x.DeleteListingComment(It.IsAny<string>()));

            //Act
            _service.DeleteListingComment(lstObject._id);

            //Assert
            _moqAppManager.Verify(v => v.DeleteListingComment(lstObject._id), Times.Once());
        }


        /// <summary>
        /// test for null Exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteListingComment_InvalidId_ThrowException()
        {
            ArgumentNullException ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _moqAppManager.Setup(x => x.DeleteListingComment(null)).Throws(ex);
            _service.DeleteListingComment(null);
        }

        #endregion DeleteListingTestCases

        #endregion
    }
}
