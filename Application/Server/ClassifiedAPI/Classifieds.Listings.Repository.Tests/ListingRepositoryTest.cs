using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Classifieds.Listings.Repository;
using Classifieds.Listings.BusinessEntities;
using System.Collections.Generic;

namespace Classifieds.Listings.Repository.Tests
{
    [TestClass]
    public class ListingRepositoryTest
    {
        #region Class Variables
        private IListingRepository _listingRepo;
        private IDBRepository _dbRepository;
        private List<Listing> classifiedList = new List<Listing>();
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _dbRepository = new DBRepository();
            _listingRepo = new ListingRepository(_dbRepository);

        }
        #endregion

        #region Setup
        private void SetUpClassifiedsListing()
        {
            var lstListing = GetListObject();
            classifiedList.Add(lstListing);
        }

        private Listing GetListObject()
        {
            Listing listObject = new Listing
            {
                //_id = "9",
                ListingType = "sale",
                ListingCategory = "Housing",
                SubCategory = "2 bhk",
                Title = "flat on rent",
                Address = "pune",
                ContactNo = "12345",
                ContactName = "AAA AAA",
                Configuration = "NA",
                Details = "for rupees 49,00,000",
                Brand = "Kumar",
                Price = 90,
                YearOfPurchase = 2000,
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
        #endregion

        #region Unit Test Cases
        [TestMethod]
        public void Repo_GetListingByIdTest()
        {
            /*In this test case we add one post and pass recently added post's Id as a parameter to GetListingById() method instead of passing hard coded value*/
            //Arrange
            Listing lstObject = GetListObject();

            //Act
            var result = _listingRepo.Add(lstObject);

            Assert.IsNotNull(result, null);

            var recentlyAddedRecord = _listingRepo.GetListingById(result._id);

            //Assert
            Assert.AreEqual(recentlyAddedRecord.Count, 1);
        }

        [TestMethod]
        public void Repo_GetListingByIdTest_InvalidId()
        {
            //Act
            var result = _listingRepo.GetListingById(null);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Repo_GetListingByCategoryTest()
        {
            // Arrange
            SetUpClassifiedsListing();

            //Act
            var result = _listingRepo.GetListingsByCategory("Automobile");

            //Assert
            //Assert.AreEqual(1, result.Count);
            Assert.IsNotNull(result[0]);
        }

        [TestMethod]
        public void Repo_GetListingByCategory_Invalid_OR_Null_Category()
        {
            var result = _listingRepo.GetListingsByCategory("qazxsw");
            Assert.AreEqual(0, result.Count);

            var nullresult = _listingRepo.GetListingsByCategory(null);
            Assert.AreEqual(0, nullresult.Count);
        }

        [TestMethod]
        public void Repo_AddListTest()
        {
            //Arrange
            Listing lstObject = GetListObject();
            //_moqAppManager.Setup(x => x.Add(It.IsAny<Listing>())).Returns(lstObject);

            //Act
            var result = _listingRepo.Add(lstObject);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(Listing));
        }

        [TestMethod]
        public void Repo_AddListTest_EmptyList()
        {
            var result = _listingRepo.Add(null);
            Assert.IsNull(result, null);
        }

        [TestMethod]
        public void Repo_DeleteListTest()
        {
            //Arrange
            Listing lstObject = new Listing
            {
                // _id = Guid.NewGuid().ToString(),
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

            //Act
            var result = _listingRepo.Add(lstObject);
            Assert.IsNotNull(result._id);
            _listingRepo.Delete(result._id);

            //Assert
            Assert.IsTrue(true);

        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Repo_DeleteListTest_InvalidId()
        {
            _listingRepo.Delete("qwer");
            Assert.IsTrue(true);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Repo_DeleteListTest_NUllId()
        {
            _listingRepo.Delete(null);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Repo_UpdateListTest()
        {
            //Arrange
            Listing lstObject = GetListObject();


            //Act
            var result = _listingRepo.Add(lstObject);
            Assert.IsNotNull(result._id);
            result.Title = "UpdatedTest";
            result.ListingCategory = "UpdatedHousing";

            var Updatedresult = _listingRepo.Update(result._id, result);
            Assert.IsNotNull(Updatedresult);

            Assert.AreEqual(result.Title, Updatedresult.Title);
            Assert.IsInstanceOfType(result, typeof(Listing));
        }

        [TestMethod]
        public void Repo_UpdateListTest_EmptyList()
        {
            Listing updatedList = null;
            var result = _listingRepo.Update(null, updatedList);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Repo_GetListingsBySubCategoryTest()
        {
            // Arrange
            SetUpClassifiedsListing();

            //Act
            var result = _listingRepo.GetListingsBySubCategory("test");

            //Assert
            Assert.IsNotNull(result[0]);
        }

        [TestMethod]
        public void Repo_GetListingsBySubCategoryTest_NullSubCategory()
        {
            var result = _listingRepo.GetListingsBySubCategory(null);
            Assert.IsTrue(true);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Repo_GetListingsBySubCategoryTest_InvalidSubCategory()
        {
            var result = _listingRepo.GetListingsBySubCategory("qwer");
            Assert.IsNull(result);
        }

        #endregion

    }
}

