#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Classifieds.Listings.BusinessEntities;
using Classifieds.Search.BusinessServices;
using Classifieds.Common;
using System.Web.Http.Cors;
#endregion

namespace Classifieds.SearchAPI.Controllers
{
    /// <summary>
    /// This Service is used for Global Search in all categorys
    /// class name: SearchController
    /// Purpose : This class is used for Global Search in all categorys.
    /// Created By : Amol Pawar
    /// Created Date: 08/12/2016
    /// Modified by :
    /// Modified date:
    /// </summary>
    [EnableCors("http://localhost:3000", "*", "*")]
    public class SearchController : ApiController
    {
        #region Private Variable
        private readonly ISearchService _searchService;
        private readonly ILogger _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchService"></param>
        /// <param name="logger"></param>
        public SearchController(ISearchService searchService,ILogger logger)
        {
            _searchService = searchService;
            _logger = logger;
        }
        #endregion

        #region Public_Methods
        /// <summary>
        /// GetFulltext search on title, description and category
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns>SearchResult</returns>
        public List<Listing> GetFullTextSearch(string searchText)
        {
            try
            {
                return _searchService.FullTextSearch(searchText).ToList();
            }
            catch (Exception ex)
            {
                //ToDo UseName is hardcoded
                throw _logger.Log(ex,"Globant/User");
            }

        }
        #endregion
    }
}