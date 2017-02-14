using Classifieds.Common;
using Classifieds.Listings.BusinessEntities;
using Classifieds.Listings.BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Classifieds.Common.Repositories;

namespace Classifieds.ListingsAPI.Controllers
{
    /// <summary>
    /// This Service is used to perform CRUD operation on Listings
    /// class name: ListingsController
    /// Purpose : This class is used for to implement get/post/put/delete methods on listings
    /// Created By : Suyash, Santosh
    /// Created Date: 08/12/2016
    /// Modified by : Ashish
    /// Modified date: 12/01/2017
    /// </summary>
    public class ListingsController : ApiController
    {
        #region Private Variable
        private readonly IListingService _listingService;
        private readonly ILogger _logger;
        private readonly ICommonRepository _commonRepository;
        private string _userEmail = string.Empty;
        #endregion

        #region Constructor
        /// <summary>
        /// The class constructor. 
        /// </summary>
        public ListingsController(IListingService listingService, ILogger logger, ICommonRepository commonRepository)
        {
            _listingService = listingService;
            _logger = logger;
            _commonRepository = commonRepository;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the listing for given id
        /// </summary>
        /// <param name="id">listing id</param>
        /// <returns></returns>
        public List<Listing> GetListingById(string id)
        {
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }

                return _listingService.GetListingById(id).ToList();
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }

        /// <summary>
        /// Returns the listings for given sub category
        /// </summary>
        /// <param name="subCategory">listing sub category</param>
        /// <param name="startIndex">start index for page</param>
        /// <param name="pageCount">No of listings to include in result</param>
        /// <param name="isLast">Whether last page</param>
        /// <returns>Collection of filtered listings</returns>
        public List<Listing> GetListingsBySubCategory(string subCategory, int startIndex = 1, int pageCount = 10, bool isLast = false)
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

                return _listingService.GetListingsBySubCategory(subCategory, startIndex, pageCount, isLast).ToList();
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }

        /// <summary>
        /// Returns the listings for given category
        /// </summary>
        /// <param name="category">listing category</param>
        /// <param name="startIndex">start index for page</param>
        /// <param name="pageCount">No of listings to include in result</param>
        /// <param name="isLast">Whether last page</param>
        /// <returns>Collection of listings</returns>
        public List<Listing> GetListingsByCategory(string category, int startIndex = 1, int pageCount = 10, bool isLast = false)
        {
            try
            {
                _userEmail = GetUserEmail();
                string authResult = _commonRepository.IsAuthenticated(Request);
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                if (startIndex < 0 || pageCount <= 0)
                {
                    string param = startIndex < 0 ? "Start Index" : "Page Count";
                    throw new Exception(param + "passed cannot be negative!");
                }

                return _listingService.GetListingsByCategory(category, startIndex, pageCount, isLast).ToList();
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }

        /// <summary>
        /// Insert new listing item into the database
        /// </summary>
        /// <param name="listing">Listing Object</param>
        /// <returns></returns>
        public HttpResponseMessage Post(Listing listing)
        {
            HttpResponseMessage result;
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }

                var classified = _listingService.CreateListing(listing);
                result = Request.CreateResponse(HttpStatusCode.Created, classified);
                var newItemUrl = Url.Link("Listings", new { id = classified._id });
                result.Headers.Location = new Uri(newItemUrl);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Update listing item for given Id
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <param name="listing">Listing Object</param>
        /// <returns></returns>
        public HttpResponseMessage Put(string id, Listing listing)
        {
            HttpResponseMessage result;
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }

                var classified = _listingService.UpdateListing(id, listing);
                result = Request.CreateResponse(HttpStatusCode.Accepted, classified);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Delete listing item for given Id
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <returns></returns>
        public HttpResponseMessage Delete(string id)
        {
            HttpResponseMessage result;
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }

                _listingService.DeleteListing(id);
                result = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Returns top listings from database  
        /// </summary>
        /// <param name="noOfRecords">Number of records to be return </param>
        /// <returns></returns>
        public List<Listing> GetTopListings(int noOfRecords = 10)
        {
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }

                return _listingService.GetTopListings(noOfRecords);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }

        #region GetListingsByEmail

        /// <summary>
        /// Returns the listings for given email
        /// </summary>
        /// <param name="email">listing email</param>
        /// <param name="startIndex">listing startIndex</param>
        /// <param name="pageCount">listing pageCount</param>
        /// <param name="isLast">listing isLast</param>
        /// <returns></returns>
        public List<Listing> GetListingsByEmail(string email, int startIndex = 1, int pageCount = 10, bool isLast = false)
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

                return _listingService.GetListingsByEmail(email, startIndex, pageCount, isLast).ToList();
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }

        #endregion GetListingsByEmail

        #region GetListingsByCategoryAndSubCategory

        /// <summary>
        /// Returns the listings for given Category and Subcategory
        /// </summary>
        /// <param name="category">listing category</param>
        /// <param name="subCategory">listing subCategory</param>
        /// <param name="email">listing email</param>
        /// <param name="startIndex">listing startIndex</param>
        /// <param name="pageCount">listing pageCount</param>
        /// <param name="isLast">listing isLast</param>
        /// <returns></returns>
        public List<Listing> GetListingsByCategoryAndSubCategory(string category, string subCategory, string email, int startIndex = 1, int pageCount = 10, bool isLast = false)
        {
            try
            {
                _userEmail = GetUserEmail();
                string authResult = _commonRepository.IsAuthenticated(Request);
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                if (startIndex < 0 || pageCount <= 0)
                {
                    string param = startIndex < 0 ? "Start Index" : "Page Count";
                    throw new Exception(param + "passed cannot be negative!");
                }

                return _listingService.GetListingsByCategoryAndSubCategory(category, subCategory, email, startIndex, pageCount, isLast).ToList();
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }

        #endregion GetListingsByCategoryAndSubCategory

        #endregion

        #region private methods
        /// <summary>
        /// Returns user email string
        /// </summary>
        /// <returns>string</returns>
        private string GetUserEmail()
        {
            IEnumerable<string> headerValues;
            HttpRequestMessage message = Request ?? new HttpRequestMessage();
            message.Headers.TryGetValues("UserEmail", out headerValues);
            string hearderVal = headerValues == null ? string.Empty : headerValues.FirstOrDefault();
            return hearderVal;
        }
        #endregion
    }
}
