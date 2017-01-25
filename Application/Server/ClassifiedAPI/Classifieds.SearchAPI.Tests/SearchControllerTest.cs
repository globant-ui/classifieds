#region Imports
using System;
using System.Collections.Generic;
using Moq;
using Classifieds.Listings.BusinessEntities;
using Classifieds.Search.BusinessServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Classifieds.Common;
using Classifieds.SearchAPI.Controllers;
#endregion

namespace Classifieds.SearchAPI.Tests
{
    /// <summary>
    /// SearchController test class 
    /// Moq Unit test for Public Methods of SearchController
    /// </summary>
    [TestClass]
    public class SearchControllerTest
    {
        #region Class Variables
        private Mock<ISearchService> mockService;
        private Mock<ILogger> logger;
        private SearchController controller;
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<ISearchService>();
            logger = new Mock<ILogger>();
            controller = new SearchController(mockService.Object, logger.Object);
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Controller_FreeTextSearchTest Unit Test positive scenario by any string
        /// </summary>
        [TestMethod]
        public void Controller_FreeTextSearchTest()
        {
            //Arrange           
            mockService.Setup(x => x.FullTextSearch(It.IsAny<string>()))
                .Returns(
                new List<Listing>
                { new Listing
                    {
                        Title ="title",
                        ListingType = "ListingType",
                        ListingCategory = "ListingCategory",
                        SubCategory = "SubCategory",
                        Address = "Address",
                        ContactNo = "ContactNo",
                        ContactName = "Contact Name",
                        Configuration = "Configuration"
                    }
                });

            logger.Setup(x => x.Log(It.IsAny<Exception>(),It.IsAny<string>()));
            
            //Act
            List<Listing> list = new List<Listing>();
            list = controller.GetFullTextSearch("searchText");

            //Assert
            Assert.AreEqual(list.Count, 1);
            Assert.AreEqual(list[0].Title, "title");
        }

        /// <summary>
        /// Controller_FreeTextSearch_ThrowsException Test Exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Controller_FreeTextSearch_ThrowsException()
        {
            NullReferenceException ex = new NullReferenceException("ArgumentNullException", new NullReferenceException());
            mockService.Setup(x => x.FullTextSearch(It.IsAny<string>())).Throws(ex);
            logger.Setup(x => x.Log(It.IsAny<Exception>(),It.IsAny<string>()));            
            var classified = controller.GetFullTextSearch(null);
        }
        #endregion
    }
}
