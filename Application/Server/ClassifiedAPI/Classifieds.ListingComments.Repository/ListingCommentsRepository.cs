using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Classifieds.ListingComments.BusinessEntities;

namespace Classifieds.ListingComments.Repository
{
    public class ListingCommentsRepository : DBRepository, IListingCommentsRepository
    {
        #region ListingCommentsRepository
        private readonly string _collectionClassifieds = ConfigurationManager.AppSettings["ListingCommentsCollection"];
        private readonly IDBRepository _dbRepository;
        public ListingCommentsRepository(IDBRepository DBRepository)
        {
            _dbRepository = DBRepository;
        }
        MongoCollection<ListingComment> classifieds
        {
            get { return _dbRepository.GetCollection<ListingComment>(_collectionClassifieds); }
        }

        #endregion

        #region GetAllListingComments
        /// <summary>
        /// Returns a Listing Comments 
        /// </summary>s
        /// <returns>Listing Comments</returns>
        public List<ListingComment> GetAllListingComment(string listingId)
        {
            try
            {
                var partialRresult = this.classifieds.FindAll()
                                     .Where(p => p.ListingId == listingId)
                                    .ToList();
                List<ListingComment> result = partialRresult.Count > 0 ? partialRresult.ToList() : null;


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetListingCommentsById
        /// <summary>
        /// Returns a listing based on listingComments id
        /// </summary>
        /// <param name="id">listingComments id</param>
        /// <returns>listingComments</returns>
        public List<ListingComment> GetListingCommentsById(string id)
        {
            try
            {
                var partialRresult = this.classifieds.FindAll()
                                        .Where(p => p._id == id)
                                        .ToList();

                List<ListingComment> result = partialRresult.Count > 0 ? partialRresult.ToList() : null;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion GetListingCommentsById

        #region CreateListingComment

        /// <summary>
        /// Insert a new Listing Comment object into the database
        /// </summary>
        /// <param name="object">Listing Comment object</param>
        /// <returns>return newly added Listing Comment object</returns>
        public ListingComment CreateListingComment(ListingComment listingComments)
        {
            try
            {
                var result = this.classifieds.Save(listingComments);
                if (result.DocumentsAffected == 0 && result.HasLastErrorMessage)
                {

                }
                return listingComments;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region UpdateListingComment

        /// <summary>
        /// Update existing Listing Comment object based on id from the database
        /// </summary>
        /// <param name="object">Listing Comment object </param>
        /// <returns>return updated Listing Comment  object</returns>
        /// 
        public ListingComment UpdateListingComment(string id, ListingComment dataObj)
        {
            try
            {
                var query = Query<ListingComment>.EQ(p => p._id, id);
                var update = Update<ListingComment>.Set(p => p.Comments, dataObj.Comments)
                                               .Set(p => p.UpdatedBy, dataObj.UpdatedBy)
                                               .Set(p => p.Verified, dataObj.Verified)
                                               .Set(p => p.UpdatedDate, dataObj.UpdatedDate);
                var result = this.classifieds.Update(query, update);
                if (result.DocumentsAffected == 0 && result.HasLastErrorMessage)
                {

                }

                return dataObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DeleteListingComment
        /// <summary>
        /// Delete Listing Comments object based on id from the database
        /// </summary>
        /// <param name="id">Listing Comments Id</param>
        /// <returns>return void</returns>
        public void DeleteListingComment(string id)
        {
            try
            {
                var query = Query<ListingComment>.EQ(p => p._id, id.ToString());
                var result = this.classifieds.Remove(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
