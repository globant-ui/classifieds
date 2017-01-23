using Classifieds.ListingComments.BusinessEntities;
using Classifieds.ListingComments.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Classifieds.ListingCommentsAPI.Tests
{
    [TestClass]
    public class ListingCommentsRepositoryTest
    {
        #region Class Variables
        private IListingCommentsRepository<ListingComment> _listingCommmentsRepo;
        private IDBRepository _dbRepository;
        private readonly List<ListingComment> _classifiedListingComments = new List<ListingComment>();
        private const string listingId = "5873490a48bd151ef5d67a29";
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _dbRepository = new DBRepository();
            _listingCommmentsRepo = new ListingCommentsRepository<ListingComment>(_dbRepository);

        }
        #endregion

        #region Setup
        private void SetUpClassifiedsCategory()
        {
            var lstMasterData = GetListingCommentObject();
            _classifiedListingComments.Add(lstMasterData);
        }

        private ListingComment GetListingCommentObject(string ListingId = "5873490a48bd151ef5d67a29")
        {
            ListingComment dataObject = new ListingComment
            {
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

        #region GetAllListingCommentsTestCases

        [TestMethod]
        // <summary>
        /// test positive scenario for Get listing comments By ListingId
        /// </summary>
        public void Repo_CreateListingCommentTest()
        {
            //Arrange
            ListingComment dataObject = GetListingCommentObject(listingId);

            //Act
            var result = _listingCommmentsRepo.CreateListingComment(dataObject);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(ListingComment));
        }

        /// <summary>
        /// tests for incorrect input giving empty result
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetRepo_GeAllListingComments_Invalid_OR_Null()
        {
            SetUpClassifiedsCategory();
            var nullResult = _listingCommmentsRepo.GetAllListingComment("vkw");
            Assert.AreEqual(0, nullResult.Count);
        }

        /// <summary>
        /// test for null input listing Comments returns null result
        /// </summary>
        [TestMethod]
        public void GetAllListingComment_EmptyResult()
        {
            var result = _listingCommmentsRepo.GetAllListingComment(null);
            Assert.IsNull(result);
        }

        /// <summary>
        /// test positive scenario for Get ListingComments By ListingId 
        /// </summary>
        [TestMethod]
        public void Repo_GetListingCommentsByIdTest()
        {
            /*In this test case we add one post and pass recently added post's Id as a parameter to Repo_GetListingCommentsById() method instead of passing hard coded value*/
            //Arrange
            ListingComment lstObject = GetListingCommentObject();

            //Act
            var result = _listingCommmentsRepo.CreateListingComment(lstObject);

            Assert.IsNotNull(result, null);

            var recentlyAddedRecord = _listingCommmentsRepo.GetListingCommentsById(result._id);

            //Assert
            Assert.AreEqual(recentlyAddedRecord.Count, 1);
        }

        #endregion GetAllListingCommentsTestCases

        #region PostListingCommentsTestCase

        /// <summary>
        /// test positive scenario for Create listing object into the database
        /// </summary>
        [TestMethod]
        public void Repo_CreateListCommentsTest()
        {
            //Arrange
            ListingComment lstObject = GetListingCommentObject();

            //Act
            var result = _listingCommmentsRepo.CreateListingComment(lstObject);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(ListingComment));
        }

        /// <summary>
        /// test for adding empty listing object returns null result
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Repo_CreateListingCommentsTest_EmptyList_ThrowException()
        {
            var result = _listingCommmentsRepo.CreateListingComment(null);
            Assert.IsNull(result, null);
        }

        #endregion PostListingCommentsTestCase        

        #region UpdateListingCommentsTestCase

        [TestMethod]
        /// <summary>
        /// test positive scenario for updating Listing Comments object
        /// </summary>
        public void Repo_UpdateListingCommentsTest()
        {
            //Arrange
            ListingComment lstObject = GetListingCommentObject();
            //Act
            var result = _listingCommmentsRepo.CreateListingComment(lstObject);
            Assert.IsNotNull(result._id);
            result.UpdatedBy = "santosh.kale@globant.com";

            var Updatedresult = _listingCommmentsRepo.UpdateListingComment(result._id, result);
            Assert.IsNotNull(Updatedresult);

            Assert.AreEqual(result.UpdatedBy, Updatedresult.UpdatedBy);
            Assert.IsInstanceOfType(result, typeof(ListingComment));
        }

        [TestMethod]
        /// <summary>
        /// test for Null id returns null result
        /// </summary>
        [ExpectedException(typeof(NullReferenceException))]
        public void Repo_UpdateListingCommentsTest_NullId_ThrowException()
        {
            ListingComment updatedListingComments = null;
            var result = _listingCommmentsRepo.UpdateListingComment(null, updatedListingComments);
            Assert.IsNull(result);
        }

        #endregion UpdateListingCommentsTestCase

        #region DeleteListingCommentsTest
        [TestMethod]
        /// <summary>
        /// test positive scenario for Delete ListingComments by Id
        /// </summary>
        public void Repo_DeleteListingCommentsTest()
        {
            // Arrange
            ListingComment dataObject = GetListingCommentObject();

            // Act
            var result = _listingCommmentsRepo.CreateListingComment(dataObject);
            Assert.IsNotNull(result._id);
            _listingCommmentsRepo.DeleteListingComment(result._id);
            var listCommentobj = _listingCommmentsRepo.GetListingCommentsById(result._id);

            // Assert
            Assert.IsNull(listCommentobj);

        }

        [TestMethod]
        /// <summary>
        /// test for invalid id returns exception
        /// </summary>
        [ExpectedException(typeof(FormatException))]
        public void Repo_DeleteListingCommentTest_InvalidId()
        {
            _listingCommmentsRepo.DeleteListingComment("qwer");
            Assert.IsTrue(true);
        }

        [TestMethod]
        /// <summary>
        /// test for null id returns exception
        /// </summary>
        [ExpectedException(typeof(NullReferenceException))]
        public void Repo_DeleteListCommentsTest_NUllId()
        {
            _listingCommmentsRepo.DeleteListingComment(null);
            Assert.IsTrue(true);
        }

        #endregion DeleteListingCommentsTest

        #endregion
    }
}
