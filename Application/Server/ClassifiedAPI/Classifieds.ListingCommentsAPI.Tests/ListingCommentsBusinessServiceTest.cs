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
        #region Class Variables
        private Mock<IListingCommentsRepository> _moqAppManager;
        private IListingCommentService _service;
        private readonly List<ListingComment> classifiedcategory = new List<ListingComment>();
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _moqAppManager = new Mock<IListingCommentsRepository>();
            _service = new ListingCommentService(_moqAppManager.Object);
        }
        #endregion

        #region Setup
        private void SetUpClassifiedsListingComments()
        {
            var lstlistingcomments = GetListingCommentsObject();
            classifiedcategory.Add(lstlistingcomments);
        }

        private ListingComment GetListingCommentsObject(string ListingId = "5873490a48bd151ef5d67a29")
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

        #region Unit Test Cases

        #region GetAllListingCommentCases

        [TestMethod]
        /// <summary>
        /// test negative scenario for throw Exception
        /// </summary>
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetAllListingComment_ThrowsException()
        {
            var result = _service.GetAllListingComment(null);
        }

        /// <summary>
        /// tests the positive test criteria for Get All Listing Comments
        /// 
        /// </summary>
        [TestMethod]
        public void GetAllListingCommentTest()
        {
            // Arrange
            SetUpClassifiedsListingComments();
            _moqAppManager.Setup(x => x.GetAllListingComment(It.IsAny<string>())).Returns(classifiedcategory);

            //Act
            var result = _service.GetAllListingComment("5873490a48bd151ef5d67a29");

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsNotNull(result[0]);
        }

        /// <summary>
        /// tests for incorrect input giving empty result
        /// </summary>
        [TestMethod]
        public void GetAllCategory_EmptyResultTest()
        {
            // Arrange
            SetUpClassifiedsListingComments();
            _moqAppManager.Setup(x => x.GetAllListingComment(It.IsAny<string>())).Returns(new List<ListingComment>() { new ListingComment() });

            //Act
            var result = _service.GetAllListingComment("5873490a48bd151ef5d67a29");

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(ListingComment));
        }

        #endregion GetAllListingCommentCases

        #region PostListingTestCases
        [TestMethod]
        /// <summary>
        /// test positive scenario for Post Listing Comments
        /// </summary>
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
        public void PostCategoryTest_EmptyList()
        {
            var result = _service.CreateListingComment(null);
            Assert.IsNull(result, null);
        }

        #endregion PostListingTestCases        

        #region UpdateListingCommentsTestCases

        [TestMethod]
        /// <summary>
        /// test positive scenario for Update Listing Comment
        /// </summary>
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

        [TestMethod]
        /// <summary>
        /// test negative scenario for invalid DeleteList
        /// </summary>
        public void PutListingCommentTest_InvalidId()
        {
            var updatedData = new ListingComment() { UpdatedBy = "santosh.kale@globant.com", UpdatedDate = "10/01/2017", Comments = "is this available", ListingId = "5873490a48bd151ef5d67a29", SubmittedDate = "03/01/2017", SubmittedBy = "v.wadsamudrakar@globant.com" };
            var result = _service.UpdateListingComment(null, updatedData);
            Assert.IsNull(result);
        }

        #endregion UpdateListingCommentsTestCases

        #region DeleteListingTestCases

        [TestMethod]
        /// <summary>
        /// test positive scenario for Delete Listing Comments
        /// </summary>       
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

        [TestMethod]
        /// <summary>
        /// test for null Exception
        /// </summary>
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteListingComment_InvalidId_ThrowException()
        {
            ArgumentNullException exec = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _moqAppManager.Setup(x => x.DeleteListingComment(null)).Throws(exec);
            _service.DeleteListingComment(null);
        }

        #endregion DeleteListingTestCases

        #endregion
    }
}
