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
        private ISearchRepository<Listing> _searchRepo;
        #endregion

        #region Initialization
        [TestInitialize]
        public void Initialize()
        { 
            _searchRepo = new SearchRepository<Listing>();
        }
        #endregion

        #region Private Methods
     
        #endregion

        #region Test Methods
        [TestMethod]
        public void Repo_FreeTextSearch_Test()
        {
            //Act
            var classifieds = _searchRepo.FullTextSearch("Automobile", 1, 5);
            //Assert
            Assert.IsNotNull(classifieds);
        }

        [TestMethod]
        public void Repo_FreeTextSearch_EmptyText_Test()
        {
            //Arrange
            string searchText = string.Empty;
            //Act
            var result = _searchRepo.FullTextSearch(searchText, 1, 5);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 0);
            Assert.IsInstanceOfType(result, typeof(IList<Listing>));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Repo_FreeTextSearch_ThrowsException()
        {
            _searchRepo.FullTextSearch(null, 1, 5);
        }
        #endregion
    }
}
