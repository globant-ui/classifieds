#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
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
        private readonly ICommonDBRepository _commonRepository;
        private string userEmail = string.Empty;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchService"></param>
        /// <param name="logger"></param>
        public SearchController(ISearchService searchService,ILogger logger, ICommonDBRepository commonRepository)
        {
            _searchService = searchService;
            _logger = logger;
            _commonRepository = commonRepository;
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
                string authResult = _commonRepository.IsAuthenticated(Request);
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                userEmail = getUserEmail();
                return _searchService.FullTextSearch(searchText).ToList();
            }
            catch (Exception ex)
            {
                throw _logger.Log(ex, userEmail);
            }

        }
        #endregion

        #region private methods
        private string getUserEmail()
        {
            IEnumerable<string> headerValues;
            HttpRequestMessage message = Request ?? new HttpRequestMessage();
            bool found = message.Headers.TryGetValues("UserEmail", out headerValues);
            return headerValues.FirstOrDefault();
        }
        #endregion
    }
}