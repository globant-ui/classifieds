#region Imports
using System;
using System.Collections.Generic;
using System.Net.Http;
using Moq;
using Classifieds.Listings.BusinessEntities;
using Classifieds.Search.BusinessServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Classifieds.Common;
using Classifieds.Common.Repositories;

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
        private Mock<ISearchService> _mockService;
        private Mock<ILogger> _logger;
        private SearchController _controller;
        private Mock<ICommonRepository> _mockAuthRepo;
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _mockService = new Mock<ISearchService>();
            _logger = new Mock<ILogger>();
            _mockAuthRepo = new Mock<ICommonRepository>();
            _controller = new SearchController(_mockService.Object, _logger.Object, _mockAuthRepo.Object);
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
            _mockService.Setup(x => x.FullTextSearch(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
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

            _logger.Setup(x => x.Log(It.IsAny<Exception>(),It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            

            //Act
            List<Listing> list = _controller.GetFullTextSearch("searchText", 1, 5);

            //Assert
            Assert.AreEqual(list.Count, 1);
            Assert.AreEqual(list[0].Title, "title");
        }

        /// <summary>
        /// Controller_FreeTextSearch_ThrowsException Test Exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_FreeTextSearch_ThrowsException()
        {
            ArgumentNullException ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _mockService.Setup(x => x.FullTextSearch(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Throws(ex);
            _logger.Setup(x => x.Log(It.IsAny<Exception>(),It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.GetFullTextSearch(null, 1, 5);
        }

        /// <summary>
        /// Controller_FreeTextSearch_ThrowsException with negative parameters throws Exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Controller_FreeTextSearch_ThrowsExceptionWithNegativeParams()
        {
            Exception ex = new Exception("Exception", new Exception());
            _mockService.Setup(x => x.FullTextSearch(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Throws(ex);
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.GetFullTextSearch("search text", -1, 5);
        }
        #endregion
    }
}
