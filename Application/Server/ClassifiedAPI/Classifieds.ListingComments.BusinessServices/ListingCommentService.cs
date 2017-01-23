using System;
using System.Collections.Generic;
using System.Linq;
using Classifieds.ListingComments.BusinessEntities;
using Classifieds.ListingComments.Repository;

namespace Classifieds.ListingComments.BusinessServices
{
    public class ListingCommentService : IListingCommentService
    {
        #region ListingCommentService

        private readonly IListingCommentsRepository _listingCommentRepository;
        /// <summary>
        /// The class constructor. 
        /// </summary>
        public ListingCommentService(IListingCommentsRepository listingCommentRepository)
        {
            _listingCommentRepository = listingCommentRepository;
        }

        #endregion

        #region GetAllListingComment
        /// <summary>
        /// Returns All Listing Comment
        /// </summary>
        /// <returns></returns>
        public List<ListingComment> GetAllListingComment(string listingId)
        {
            try
            {
                return _listingCommentRepository.GetAllListingComment(listingId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CreateListingComment

        /// <summary>
        /// Insert new Listing Comment item into the database
        /// </summary>
        /// <param name="Listing Comment">Listing Comment Object</param>
        /// <returns></returns>
        public ListingComment CreateListingComment(ListingComment listObject)
        {
            try
            {
                return _listingCommentRepository.CreateListingComment(listObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region UpdateListingComment
        /// <summary>
        /// Update Listing Comment item for given Id
        /// </summary>
        /// <param name="id">Listing Comment Id</param>
        /// <returns></returns>
        public ListingComment UpdateListingComment(string id, ListingComment listObject)
        {
            try
            {
                return _listingCommentRepository.UpdateListingComment(id, listObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DeleteListingComment
        /// <summary>
        /// Delete Listing Comment item for given Id
        /// </summary>
        /// <param name="id">ListingComment Id</param>
        /// <returns></returns>
        public void DeleteListingComment(string id)
        {
            try
            {
                _listingCommentRepository.DeleteListingComment(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
