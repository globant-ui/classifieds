#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using Classifieds.Listings.BusinessEntities;
using Classifieds.Search.Repository;
#endregion  

namespace Classifieds.Search.BusinessServices
{
    /// <summary>
    /// This SearchService implements ISearchService
    /// class name: SearchService 
    /// Purpose : This class is used for Global Search in all categorys.
    /// Created By : Amol Pawar
    /// Created Date: 08/12/2016
    /// Modified by :
    /// Modified date:
    /// </summary>
    public class SearchService : ISearchService
    {
        #region Private Variables
        private readonly ISearchRepository _searchRepository;
        #endregion

        #region Constructor

        public SearchService(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Used for FullTextSearch()
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public List<Listing> FullTextSearch(string searchText)
        {
            try
            {
                return _searchRepository.FullTextSearch(searchText).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
