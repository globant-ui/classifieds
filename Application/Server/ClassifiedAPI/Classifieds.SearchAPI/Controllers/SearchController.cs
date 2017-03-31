#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using Classifieds.Listings.BusinessEntities;
using Classifieds.Search.BusinessServices;
using Classifieds.Common;
using Classifieds.Common.Repositories;
using System.Configuration;

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
    public class SearchController : ApiController
    {
        #region Private Variable
        private readonly ISearchService _searchService;
        private readonly ILogger _logger;
        private readonly ICommonRepository _commonRepository;
        private string _userEmail = string.Empty;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchService"></param>
        /// <param name="logger"></param>
        /// <param name="commonRepository"></param>
        public SearchController(ISearchService searchService,ILogger logger, ICommonRepository commonRepository)
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
        /// <param name="searchText">Search text</param>
        /// <param name="startIndex">Start Page no</param>
        /// <param name="pageCount">No of results included</param>
        /// <param name="isLast">Whether last page</param>
        /// <returns>Collection of listings</returns>
        public List<Listing> GetFullTextSearch(string searchText, int startIndex = 1, int pageCount = 10, bool isLast = false)
        {
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                if (startIndex < 0 || pageCount <= 0)
                {
                    string param = startIndex < 0 ? "Start Index" : "Page Count";
                    throw new Exception(param + "passed cannot be negative!");
                }
                //return _searchService.FullTextSearch(searchText, startIndex, pageCount, isLast).ToList();               
                var objListing = _searchService.FullTextSearch(searchText, startIndex, pageCount, isLast).ToList();
                return MapImageName(objListing);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }

        }
        #endregion

        #region private methods
        private string GetUserEmail()
        {
            IEnumerable<string> headerValues;
            HttpRequestMessage message = Request ?? new HttpRequestMessage();
            message.Headers.TryGetValues("UserEmail", out headerValues);
            string headerVal = headerValues == null ? string.Empty : headerValues.FirstOrDefault();
            return headerVal;
        }

        private List<Listing> MapImageName(List<Listing> listArray)
        {
            if (listArray != null)
            {
                foreach (Listing lst in listArray)
                {
                    if (lst.Photos != null)
                    {
                        foreach (ListingImages img in lst.Photos)
                        {
                            if (img != null)
                            {
                                img.Image = ConfigurationManager.AppSettings["ImageServer"].ToString() + img.Image;
                            }
                        }
                    }
                }
            }
            return listArray;
        }
        #endregion
    }
}