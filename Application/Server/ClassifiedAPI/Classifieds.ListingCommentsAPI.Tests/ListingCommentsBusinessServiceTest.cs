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
                Verified = "yes"
            };
            return dataObject;
        }
        #endregion

        #region Unit Test Cases

        [TestMethod]
        /// <summary>
        /// test negative scenario for throw Exception
        /// </summary>
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetAllListingComment_ThrowsException(string listingId)
        {
            var result = _service.GetAllListingComment(listingId);
        }


        /// <summary>
        /// tests the positive test criteria for Get All Listing Comments
        /// </summary>
        [TestMethod]
        public void GetAllListingCommentTest(string listingId)
        {
            // Arrange
            SetUpClassifiedsListingComments();
            _moqAppManager.Setup(x => x.GetAllListingComment(listingId)).Returns(classifiedcategory);

            //Act
            var result = _service.GetAllListingComment("5873490a48bd151ef5d67a29");

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsNotNull(result[0]);
        }

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
            Assert.IsTrue(true);
            _moqAppManager.Verify(v => v.DeleteListingComment(lstObject._id), Times.Once());
        }

        [TestMethod]
        public void DeleteListingCommentTest_InvalidId()
        {
            _service.DeleteListingComment(null);
            Assert.IsTrue(true);
        }

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
            var updatedData = new ListingComment() { UpdatedBy = "santosh.kale@globant.com", UpdatedDate = "10/01/2017", Comments= "is this available",ListingId= "5873490a48bd151ef5d67a29",SubmittedDate= "03/01/2017", SubmittedBy= "v.wadsamudrakar@globant.com" };
            var result = _service.UpdateListingComment(null, updatedData);
            Assert.IsNull(result);
        }
        #endregion
    }
}
