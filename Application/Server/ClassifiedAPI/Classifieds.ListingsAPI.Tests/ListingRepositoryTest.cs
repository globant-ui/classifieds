using Classifieds.Listings.BusinessEntities;
using Classifieds.Listings.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Classifieds.ListingsAPI.Tests
{
    [Ignore]
    [TestClass]
    public class ListingRepositoryTest
    {
        #region Class Variables
        private IListingRepository<Listing> _listingRepo;
        private IDBRepository _dbRepository;
        private readonly List<Listing> _classifiedList = new List<Listing>();
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
            _classifiedList.Add(lstListing);
        }

        private Listing GetListObject()
        {
            var listObject = new Listing
            {
                ListingType = "sale",
                ListingCategory = "Housing",
                SubCategory = "Apartments",
                Title = "flat on rent",
                Address = "pune",
                Details = "for rupees 49,00,000",
                Brand = "Kumar",
                Price = 45000,
                YearOfPurchase = 2000,
                Status = "Active",
                SubmittedBy = "v.wadsamudrakar@globant.com",
                SubmittedDate = new DateTime(2018, 02, 03),
                IdealFor = "Family",
                Furnished = "yes",
                FuelType = "test",
                KmDriven = 5000,
                Dimensions = new Dimension { Length = "9'9", Width = "16'", Height = "8'" },
                TypeofUse = "test",
                Type = "2 BHK",
                IsPublished = true,
                Negotiable = true,
                Photos = new ListingImages[] { }
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
            var lstObject = GetListObject();

            //Act
            var result = _listingRepo.Add(lstObject);

            Assert.IsNotNull(result, null);

            var recentlyAddedRecord = _listingRepo.GetListingById(result._id);

            //Assert
            Assert.IsNotNull(recentlyAddedRecord.Title);
        }

        /// <summary>
        /// test for incorrect id return null;
        /// </summary>
        [TestMethod]
        public void Repo_GetListingByIdTest_NullId()
        {
            //Act
            var result = _listingRepo.GetListingById(null);

            //Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// test for incorrect id throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Repo_GetListingByIdTest_InvalidId_ThrowException()
        {
            _listingRepo.GetListingById("qwer");
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
            var result = _listingRepo.GetListingsByCategory(_classifiedList[0].ListingCategory, 1, 5, false);

            //Assert            
            Assert.IsNotNull(result[0]);
        }

        /// <summary>
        /// test positive scenario for get listing by category result last page
        /// </summary>
        [TestMethod]
        public void Repo_GetListingByCategoryTest_LastPage()
        {
            // Arrange
            SetUpClassifiedsListing();

            //Act
            var result = _listingRepo.GetListingsByCategory(_classifiedList[0].ListingCategory, 1, 5, true);

            //Assert            
            Assert.IsNotNull(result[0]);
        }

        /// <summary>
        /// test for invalid category returns empty result
        /// </summary>
        [TestMethod]
        public void Repo_GetListingByCategoryTest_InvalidCategory()
        {
            var result = _listingRepo.GetListingsByCategory("qazxsw", 1, 5, false);
            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        /// test for null category returns empty result
        /// </summary>
        [TestMethod]
        public void Repo_GetListingByCategoryTest_NullCategory()
        {
            var nullResult = _listingRepo.GetListingsByCategory(null, 1, 5, false);
            Assert.AreEqual(0, nullResult.Count);
        }

        /// <summary>
        /// test positive scenario for add listing object into the database
        /// </summary>
        [TestMethod]
        public void Repo_AddListTest()
        {
            //Arrange
            var lstObject = GetListObject();

            //Act
            var result = _listingRepo.Add(lstObject);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(Listing));
        }

        /// <summary>
        /// test for adding empty listing object throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Repo_AddListTest_EmptyList_ThrowException()
        {
            _listingRepo.Add(null);
        }

        /// <summary>
        /// test positive scenario for Delete list by Id
        /// </summary>
        [TestMethod]
        public void Repo_DeleteListTest()
        {
            //Arrange
            var lstObject = GetListObject();

            //Act
            var result = _listingRepo.Add(lstObject);
            Assert.IsNotNull(result._id);
            _listingRepo.Delete(result._id);

            var newresult = _listingRepo.GetListingById(result._id);

            //Assert
            Assert.IsNull(newresult);

        }

        /// <summary>
        /// test for incorrect id returns exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Repo_DeleteListTest_InvalidId_ThrowException()
        {
            _listingRepo.Delete("qwer");
        }

        /// <summary>
        /// test positive scenario for updating listing object
        /// </summary>
        [TestMethod]
        public void Repo_UpdateListTest()
        {
            //Arrange
            var lstObject = GetListObject();


            //Act
            var result = _listingRepo.Add(lstObject);
            Assert.IsNotNull(result._id);
            result.Title = "UpdatedTest";
            result.ListingCategory = "UpdatedHousing";

            var updatedresult = _listingRepo.Update(result._id, result);
            Assert.IsNotNull(updatedresult);

            Assert.AreEqual(result.Title, updatedresult.Title);
            Assert.IsInstanceOfType(result, typeof(Listing));
        }

        /// <summary>
        /// test for updating listing object with null listing id throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Repo_UpdateListTest_NullId_ThrowException()
        {
            var result = _listingRepo.Update(null, null);
            Assert.IsNull(result);
        }

        /// <summary>
        /// test positive scenario for get listing by sub category
        /// </summary>
        [TestMethod]
        public void Repo_GetListingsBySubCategoryTest()
        {
            var lstObject = GetListObject();

            //Act
            var result = _listingRepo.Add(lstObject);
            Assert.IsNotNull(result, null);

            //Act
            var newResult = _listingRepo.GetListingsBySubCategory(result.SubCategory, 1, 5, false);

            //Assert
            Assert.IsNotNull(newResult[0]);
        }

        /// <summary>
        /// test positive scenario for get listing by sub category
        /// </summary>
        [TestMethod]
        public void Repo_GetListingsBySubCategoryTest_LastPage()
        {
            var lstObject = GetListObject();

            //Act
            var result = _listingRepo.Add(lstObject);

            Assert.IsNotNull(result, null);

            //Act
            var newResult = _listingRepo.GetListingsBySubCategory(result.SubCategory, 1, 5, true);

            //Assert
            Assert.IsNotNull(newResult[0]);
        }

        /// <summary>
        /// test for null subcategory returns null result
        /// </summary>
        [TestMethod]
        public void Repo_GetListingsBySubCategoryTest_NullSubCategory()
        {
            var result = _listingRepo.GetListingsBySubCategory(null, 1, 5, false);
            Assert.IsNull(result);
        }

        /// <summary>
        /// test for invalid subcategory returns null result
        /// </summary>
        [TestMethod]
        public void Repo_GetListingsBySubCategoryTest_InvalidSubCategory()
        {
            var result = _listingRepo.GetListingsBySubCategory("qwer", 1, -5, false);
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
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 2);
        }

        /// <summary>
        /// test GetTopListing throws exception whenever.
        /// to pass this test case database server must be down.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void Repo_GetTopListingTest_ThrowException()
        {
            //Act
            _listingRepo.GetTopListings(2);
        }

        #region GetListingsByEmailTest

        /// <summary>
        /// test positive scenario for Get Listing By Email 
        /// </summary>
        [TestMethod]
        public void Repo_GetListingsByEmailTest()
        {
            // Arrange
            SetUpClassifiedsListing();

            //Act
            var result = _listingRepo.GetListingsByEmail(_classifiedList[0].SubmittedBy, 1, 5, false);

            //Assert            
            Assert.IsNotNull(result[0]);
        }

        /// <summary>
        /// test for incorrect email return null;
        /// </summary>
        [TestMethod]
        public void Repo_GetListingsByEmailTest_Null()
        {
            //Act
            var result = _listingRepo.GetListingsByEmail(null, 1, -5, false);

            //Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// test for incorrect email throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Repo_GetListingByEmailTest_InvalidEmail_ThrowException()
        {
            var result = _listingRepo.GetListingsByEmail("qazxsw", 1, 5, false);
            Assert.AreEqual(0, result.Count);
        }

        #endregion

        #region GetListingsByCategoryAndSubCategoryTest

        /// <summary>
        /// test positive scenario for Get Listing By Category and SubCategory 
        /// </summary>
        [TestMethod]
        public void GetListingsByCategoryAndSubCategoryTest()
        {
            // Arrange
            SetUpClassifiedsListing();

            //Act
            var result = _listingRepo.GetListingsByCategoryAndSubCategory(_classifiedList[0].ListingCategory, _classifiedList[0].SubCategory, _classifiedList[0].SubmittedBy, 1, 5, false);

            //Assert            
            Assert.IsNotNull(result[0]);
        }

        /// <summary>
        /// test for incorrect Category And SubCategory return null;
        /// </summary>
        [TestMethod]
        public void Repo_GetListingsByCategoryAndSubCategoryTest_Null()
        {
            //Act
            var result = _listingRepo.GetListingsByCategoryAndSubCategory(null, null, null, 1, 5, false);

            //Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// test for incorrect Category And SubCategory throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Repo_GetListingByCategoryAndSubCategoryTest_Invalid_Exception()
        {
            var result = _listingRepo.GetListingsByCategoryAndSubCategory("qazxsw", "qazxsw", "qazxsw", 1, 5, false);
            Assert.AreEqual(0, result.Count);
        }

        #endregion

        #region Repo_CloseListingTest

        /// <summary>
        /// test positive scenario for updating listing object
        /// </summary>
        [TestMethod]
        public void Repo_CloseListingTest()
        {
            //Arrange
            var lstObject = GetListObject();


            //Act
            var result = _listingRepo.Add(lstObject);
            Assert.IsNotNull(result._id);
            result.Status = "Closed";

            var updatedresult = _listingRepo.CLoseListing(result._id, result);
            Assert.IsNotNull(updatedresult);

            Assert.AreEqual(result.Title, updatedresult.Title);
            Assert.IsInstanceOfType(result, typeof(Listing));
        }

        /// <summary>
        /// test for Update Close listing object with null listing id throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Repo_CloseListingTest_NullId_ThrowException()
        {
            var result = _listingRepo.CLoseListing(null, null);
            Assert.IsNull(result);
        }

        #endregion

        #endregion
    }
}
