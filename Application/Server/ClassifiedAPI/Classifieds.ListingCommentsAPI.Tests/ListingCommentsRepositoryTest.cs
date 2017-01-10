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
        private IListingCommentsRepository _listingCommmentsRepo;
        private IDBRepository _dbRepository;
        private readonly List<ListingComment> classifiedListingComments = new List<ListingComment>();
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _dbRepository = new DBRepository();
            _listingCommmentsRepo = new ListingCommentsRepository(_dbRepository);

        }
        #endregion

        #region Setup
        private void SetUpClassifiedsCategory()
        {
            var lstMasterData = GetListingCommentObject();
            classifiedListingComments.Add(lstMasterData);
        }

        private ListingComment GetListingCommentObject()
        {
            ListingComment dataObject = new ListingComment
            {
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
        public void Repo_CreateListingCommentTest()
        {
            //Arrange
            ListingComment dataObject = GetListingCommentObject();

            //Act
            var result = _listingCommmentsRepo.CreateListingComment(dataObject);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(ListingComment));
        }

        [TestMethod]
        public void Repo_DeleteMasterDataTest()
        {
            //Arrange
            ListingComment dataObject = GetListingCommentObject();

            //Act
            var result = _listingCommmentsRepo.CreateListingComment(dataObject);
            Assert.IsNotNull(result._id);
            _listingCommmentsRepo.DeleteListingComment(result._id);

            //Asserts
            Assert.IsTrue(true);

        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Repo_DeleteListingCommentTest_InvalidId()
        {
            _listingCommmentsRepo.DeleteListingComment("qwer");
            Assert.IsTrue(true);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Repo_DeleteListCommentsTest_NUllId()
        {
            _listingCommmentsRepo.DeleteListingComment(null);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Repo_UpdateMasterDataTest()
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
        #endregion
    }
}
