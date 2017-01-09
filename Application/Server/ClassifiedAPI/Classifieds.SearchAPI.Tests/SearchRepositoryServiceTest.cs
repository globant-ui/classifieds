using Classifieds.Listings.BusinessEntities;
using Classifieds.Search.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Classifieds.SearchAPI.Tests
{
    [TestClass]
   public class SearchRepositoryServiceTest
    {
        #region Private Variables        
        private ISearchRepository _searchRepo;
        #endregion

        #region Initialization
        [TestInitialize]
        public void Initialize()
        { 
            _searchRepo = new SearchRepository();
        }
        #endregion

        #region Private Methods
        private void SetUpClassifields()
        {
            var classified = new Listing()
            {
                ListingType = "ListingType",
                ListingCategory = "ListingCategory",
                SubCategory = "SubCategory",
                Title = "Title",
                Address = "Address",
                ContactNo = "1111",
                ContactName = "AAA AAA",
                Configuration = "NA",
                Details = "for rupees 20,000,000,000",
                Brand = "Brand",
                Price = 123,
                YearOfPurchase = 123,
                ExpiryDate = "ExpiryDate",
                Status = "Status",
                Submittedby = "Submittedby",
                SubmittedDate = "20-DEC-2016",
                IdealFor = "IdealFor",
                Furnished = "Furnished",
                FuelType = "FuelType",
                KmDriven = 123,
                YearofMake = 123,
                Dimensions = "Dimensions",
                TypeofUse = "TypeofUse",
                Photos = new string[] { "/Photos/Merc2016.jpg", "/Photos/Merc2016.jpg" }
            };

            var classifiedList = new List<Listing>();
            classifiedList.Add(classified);
            //_moqAppManager.Setup(x => x.FullTextSearch(It.IsAny<string>())).Returns(classifiedList);

        }
        #endregion

        #region Test Methods
        [TestMethod]
        public void Repo_FreeTextSearch_Test()
        {
            //Arrange
            SetUpClassifields();
            //Act
            var classifieds = _searchRepo.FullTextSearch("Automobile");
            //Assert
            Assert.IsNotNull(classifieds);

        }

        [TestMethod]
        public void Repo_FreeTextSearch_EmptyText_Test()
        {
            //Arrange
            string searchText = string.Empty;
            //Act
            var result = _searchRepo.FullTextSearch(searchText);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 0);
            Assert.IsInstanceOfType(result, typeof(IList<Listing>));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Repo_FreeTextSearch_ThrowsException()
        {
            var result = _searchRepo.FullTextSearch(null);
        }
        #endregion
    }
}
