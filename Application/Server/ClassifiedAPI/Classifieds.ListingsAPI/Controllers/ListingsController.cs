using Classifieds.Common;
using Classifieds.Listings.BusinessEntities;
using Classifieds.Listings.BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Classifieds.ListingsAPI.Controllers
{
    /// <summary>
    /// This Service is used to perform CRUD operation on Listings
    /// class name: ListingsController
    /// Purpose : This class is used for to implement get/post/put/delete methods on listings
    /// Created By : Suyash, Santosh
    /// Created Date: 08/12/2016
    /// Modified by :
    /// Modified date: 
    /// </summary>
    [EnableCors("http://localhost:3000", "*", "*")]
    public class ListingsController : ApiController
    {
        #region Private Variable
        private readonly IListingService _listingService;
        private readonly ILogger _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// The class constructor. 
        /// </summary>
        public ListingsController(IListingService listingService, ILogger logger)
        {
            _listingService = listingService;
            _logger = logger;
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
                return _listingService.GetListingById(id).ToList();
            }
            catch (Exception ex)
            {               
                _logger.Log(ex, "Globant/User");
                throw ex;
            }
        }

        /// <summary>
        /// Returns the listings for given sub category
        /// </summary>
        /// <param name="subCategory">listing Sub Category</param>
        /// <returns></returns>
        public List<Listing> GetListingsBySubCategory(string subCategory)
        {
            try
            {
                return _listingService.GetListingsBySubCategory(subCategory).ToList();
            }
            catch (Exception ex)
            {               
                _logger.Log(ex, "Globant/User");
                throw ex;
            }
        }

        /// <summary>
        /// Returns the listings for given category
        /// </summary>
        /// <param name="category">listing category</param>
        /// <returns></returns>
        public List<Listing> GetListingsByCategory(string category)
        {
            try
            {
                return _listingService.GetListingsByCategory(category).ToList();
            }
            catch (Exception ex)
            {               
                _logger.Log(ex, "Globant/User");
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
                var classified = _listingService.CreateListing(listing);
                result = Request.CreateResponse(HttpStatusCode.Created, classified);
                var newItemUrl = Url.Link("Listings", new { id = classified._id });
                result.Headers.Location = new Uri(newItemUrl);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Globant/User");
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
                var classified = _listingService.UpdateListing(id, listing);
                result = Request.CreateResponse(HttpStatusCode.Accepted, classified);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Globant/User");
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
                _listingService.DeleteListing(id);
                result = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Globant/User");
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Returns top listings from database  
        /// </summary>
        /// <param name="noOfRecords">Number of records to be return </param>
        /// <returns></returns>
        public List<Listing> GetTopListings(int noOfRecords=10)
        {
            try
            {
                return _listingService.GetTopListings(noOfRecords);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Globant/User");
                throw ex;
            }
        }
        #endregion
    }
}
