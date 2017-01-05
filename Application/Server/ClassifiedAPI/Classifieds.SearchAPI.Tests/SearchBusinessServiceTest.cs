#region Imports
using System;
using System.Collections.Generic;
using Moq;
using Classifieds.Search.BusinessEntities;
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
            private Mock<ISearchRepository> _moqAppManager;
            private ISearchService _service;
        #endregion

        #region Initialization
        [TestInitialize]
            public void Initialize()
            {
                _moqAppManager = new Mock<ISearchRepository>();
                _service = new SearchService(_moqAppManager.Object);
            }
        #endregion

        #region Test Methods
        [TestMethod]
        public void BusinessService_FreeTextSearchTest()
        {
            //Arrange
            SetUpClassifields();
            //Act
            var classifieds = _service.FullTextSearch("searchText");
            //Assert
            Assert.AreEqual(classifieds.Count, 1);

        }

        [TestMethod]
        public void BusinessService_FreeTextSearch_EmptyResult_Test()
        {
            //Arrange
            _moqAppManager.Setup(x => x.FullTextSearch(It.IsAny<string>())).Returns(new List<Classified>() { new Classified() });
            //Act
            var result = _service.FullTextSearch("searchText");
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<Classified>));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BusinessService_FreeTextSearch_ThrowsException()
        {
            var result = _service.FullTextSearch(null);
        }

        #endregion 

        #region Private Methods
        private void SetUpClassifields()
        {
            var classified = new Classified()
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

            var classifiedList = new List<Classified>();
            classifiedList.Add(classified);
            _moqAppManager.Setup(x => x.FullTextSearch(It.IsAny<string>())).Returns(classifiedList);

        }
        #endregion 
    }
}
