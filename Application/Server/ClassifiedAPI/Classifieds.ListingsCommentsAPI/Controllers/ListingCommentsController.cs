using Classifieds.ListingComments.BusinessEntities;
using Classifieds.ListingComments.BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Classifieds.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Classifieds.ListingCommentsAPI.Controllers
{
    /// <summary>
    /// This Service is used to perform CRUD operation on ListingComments
    /// class name: ListingCommentsController
    /// Purpose : This class is used for to implement get/post/put/delete methods on Listing Comments
    /// Created By : Varun Wadsamudrakar
    /// Created Date: 16/01/2017
    /// Modified by :
    /// Modified date: 
    /// </summary>
    [TestClass]
    public class ListingCommentsController : ApiController
    {
        #region Private Variable

        private readonly IListingCommentService _listingCommentsService;
        private readonly ILogger _logger;

        #endregion

        #region ListingCommentsController
        /// <summary>
        /// The class constructor. 
        /// </summary>
        public ListingCommentsController(IListingCommentService listingCommentService, ILogger logger)
        {
            _listingCommentsService = listingCommentService;
            _logger = logger;
        }

        #endregion

        #region GetAllListingComment
        /// <summary>
        /// Returns the All Listing Comments 
        /// </summary>
        /// <param name="listingId">All Listing Comments</param>
        /// <returns>All Listing Comments</returns>
        public List<ListingComment> GetAllListingComment(string listingId)
        {
            try
            {
                return _listingCommentsService.GetAllListingComment(listingId).ToList();

            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Globant/User");
                throw ex;
            }
        }

        #endregion

        #region PostListingComments

        /// <summary>
        /// Insert new Listing Comments item into the database
        /// </summary>
        /// <param name="listingCommentsObj">Add Listing Comments</param>
        /// <returns>Newly added Listing Comment object</returns>
        public HttpResponseMessage Post(ListingComment listingCommentsObj)
        {
            HttpResponseMessage result = null;
            try
            {
                var classified = _listingCommentsService.CreateListingComment(listingCommentsObj);
                result = Request.CreateResponse<ListingComment>(HttpStatusCode.Created, classified);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Globant/User");
                throw ex;
            }

            return result;
        }

        #endregion

        #region UpdateListingComments
        /// <summary>
        /// Update Listing Comments item for given Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="listingcommentobj">Listing Comments Object</param>
        /// <returns>Updated Listing Comment obj</returns>
        public HttpResponseMessage Put(string id, ListingComment listingcommentobj)
        {
            HttpResponseMessage result = null;
            try
            {
                var classified = _listingCommentsService.UpdateListingComment(id, listingcommentobj);
                result = Request.CreateResponse<ListingComment>(HttpStatusCode.Accepted, classified);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Globant/User");
                throw ex;
            }
            return result;
        }

        #endregion

        #region DeleteListingComments
        /// <summary>
        /// Delete Listing Comments item for given Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Deleted id</returns>
        public HttpResponseMessage Delete(string id)
        {
            HttpResponseMessage result = null;

            try
            {
                _listingCommentsService.DeleteListingComment(id);
                result = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Globant/User");
                throw ex;
            }

            return result;
        }

        #endregion
    }
}
