using Classifieds.Listings.BusinessEntities;
using Classifieds.Listings.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Classifieds.ListingsAPI.Tests
{
    [TestClass]
    public class ListingRepositoryTest
    {
        #region Class Variables
        private IListingRepository<Listing> _listingRepo;
        private IDBRepository _dbRepository;
        private readonly List<Listing> classifiedList = new List<Listing>();
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _dbRepository = new DBRepository();
            _listingRepo = new ListingRepository<Listing>(_dbRepository);

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
                Photos = new string[] { "/Photos/Merc2016.jpg", "/Photos/Merc2016.jpg" }
            };
            return listObject;
        }
        #endregion

        #region Unit Test Cases
        /// <summary>
        /// test positive scenario for Get Listing By Id 
        /// </summary>
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

        /// <summary>
        /// test for incorrect id return null;
        /// </summary>
        [TestMethod]
        public void Repo_GetListingByIdTest_InvalidId()
        {
            //Act
            var result = _listingRepo.GetListingById(null);

            //Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// test positive scenario for get listing by category 
        /// </summary>
        [TestMethod]
        public void Repo_GetListingByCategoryTest()
        {
            // Arrange
            SetUpClassifiedsListing();

            //Act
            var result = _listingRepo.GetListingsByCategory("Housing");

            //Assert            
            Assert.IsNotNull(result[0]);
        }

        /// <summary>
        /// test for incorrect or null category returns empty result
        /// </summary>
        [TestMethod]
        public void Repo_GetListingByCategory_Invalid_OR_Null_Category()
        {
            var result = _listingRepo.GetListingsByCategory("qazxsw");
            Assert.AreEqual(0, result.Count);

            var nullResult = _listingRepo.GetListingsByCategory(null);
            Assert.AreEqual(0, nullResult.Count);
        }

        /// <summary>
        /// test positive scenario for add listing object into the database
        /// </summary>
        [TestMethod]
        public void Repo_AddListTest()
        {
            //Arrange
            Listing lstObject = GetListObject();
            
            //Act
            var result = _listingRepo.Add(lstObject);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(Listing));
        }

        /// <summary>
        /// test for adding empty listing object returns null result
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Repo_AddListTest_EmptyList_ThrowException()
        {
            var result = _listingRepo.Add(null);
            Assert.IsNull(result, null);
        }

        /// <summary>
        /// test positive scenario for Delete list by Id
        /// </summary>
        [TestMethod]
        public void Repo_DeleteListTest()
        {
            //Arrange
            Listing lstObject = GetListObject();

            //Act
            var result = _listingRepo.Add(lstObject);
            Assert.IsNotNull(result._id);
            _listingRepo.Delete(result._id);

            //Assert
            Assert.IsTrue(true);

        }

        /// <summary>
        /// test for incorrect id returns exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Repo_DeleteListTest_InvalidId()
        {
            _listingRepo.Delete("qwer");
            Assert.IsTrue(true);
        }

        /// <summary>
        /// test for null id returns exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Repo_DeleteListTest_NullId_ThrowException()
        {
            _listingRepo.Delete(null);
            Assert.IsTrue(true);
        }

        /// <summary>
        /// test positive scenario for updating listing object
        /// </summary>
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

        /// <summary>
        /// test for incorrect listing id returns null result
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Repo_UpdateListTest_NullId_ThrowException()
        {
            Listing updatedList = null;
            var result = _listingRepo.Update(null, updatedList);
            Assert.IsNull(result);
        }

        /// <summary>
        /// test positive scenario for get listing by sub category
        /// </summary>
        [TestMethod]
        public void Repo_GetListingsBySubCategoryTest()
        {
            Listing lstObject = GetListObject();

            //Act
            var result = _listingRepo.Add(lstObject);

            Assert.IsNotNull(result, null);

            //Act
             var newResult = _listingRepo.GetListingsBySubCategory(result.SubCategory);

            //Assert
            Assert.IsNotNull(newResult[0]);
        }

        /// <summary>
        /// test for null subcategory returns null result
        /// </summary>
        [TestMethod]
        public void Repo_GetListingsBySubCategoryTest_NullSubCategory()
        {
            var result = _listingRepo.GetListingsBySubCategory(null);
            Assert.IsTrue(true);
            Assert.IsNull(result);
        }

        /// <summary>
        /// test for invalid subcategory returns null result
        /// </summary>
        [TestMethod]
        public void Repo_GetListingsBySubCategoryTest_InvalidSubCategory()
        {
            var result = _listingRepo.GetListingsBySubCategory("qwer");
            Assert.IsNull(result);
        }

        /// <summary>
        /// test positive scenario for get top 2 listing oject  
        /// </summary>
        [TestMethod]
        public void Repo_GetTopListingTest()
        {
            //Act
            var result = _listingRepo.GetTopListings(2);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.AreEqual(result.Count, 2);
        }
        #endregion

    }
}
