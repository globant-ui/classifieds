#region Imports
using System;
using System.Collections.Generic;
using Moq;
using Classifieds.Listings.BusinessEntities;
using Classifieds.Search.BusinessServices;
using Classifieds.Search.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endregion  

namespace Classifieds.SearchAPI.Tests
{
    [TestClass]
    public class SearchBusinessServiceTest
    {
        #region Private Variables
        private Mock<ISearchRepository<Listing>> _moqAppManager;
        private ISearchService _service;
        #endregion

        #region Initialization
        [TestInitialize]
        public void Initialize()
        {
            _moqAppManager = new Mock<ISearchRepository<Listing>>();
            _service = new SearchService(_moqAppManager.Object);
        }
        #endregion

        #region Private Methods
        private void SetUpClassifields()
        {
            var classified = new Listing()
            {
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
                Status = "test",
                SubmittedBy = "test",
                SubmittedDate = new DateTime(2018, 02, 03),
                IdealFor = "test",
                Furnished = "test",
                FuelType = "test",
                KmDriven = 123,
                YearofMake = 123,
                Dimensions = new Dimension { Length = "9'9", Width = "16'", Height = "8'" },
                TypeofUse = "test",
                Photos = new[] { "/Photos/Merc2016.jpg", "/Photos/Merc2016.jpg" }
            };

            var classifiedList = new List<Listing>();
            classifiedList.Add(classified);
            _moqAppManager.Setup(x => x.FullTextSearch(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(classifiedList);

        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Test positive scenario for FreeTextSearchTest by any string
        /// </summary>
        [TestMethod]
        public void BusinessService_FreeTextSearchTest()
        {
            //Arrange
            SetUpClassifields();
            //Act
            var classifieds = _service.FullTextSearch("searchText", 1, 5, false);
            //Assert
            Assert.AreEqual(classifieds.Count, 1);

        }

        /// <summary>
        /// Test FreeTextSearchTest by any string return empty result
        /// </summary>
        [TestMethod]
        public void BusinessService_FreeTextSearch_EmptyResult_Test()
        {
            //Arrange
            _moqAppManager.Setup(x => x.FullTextSearch(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(new List<Listing>());
            //Act
            var result = _service.FullTextSearch("searchText", 1, 5, false);
            //Assert
            Assert.AreEqual(result.Count, 0);
            Assert.IsInstanceOfType(result, typeof(IList<Listing>));
        }

        /// <summary>
        /// Test FreeTextSearch by null input throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BusinessService_FreeTextSearch_ThrowsException()
        {
            //Arrange
            ArgumentNullException ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _moqAppManager.Setup(x => x.FullTextSearch(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Throws(ex);
            _service.FullTextSearch(null, 1, 5, false);
        }

        #endregion
    }
}
