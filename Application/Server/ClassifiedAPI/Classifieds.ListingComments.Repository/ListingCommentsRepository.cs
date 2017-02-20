using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Classifieds.ListingComments.BusinessEntities;

namespace Classifieds.ListingComments.Repository
{
    public class ListingCommentsRepository<TEntity> : DBRepository, IListingCommentsRepository<TEntity> where TEntity : ListingComment
    {
        #region ListingCommentsRepository
        private readonly string _collectionClassifieds = ConfigurationManager.AppSettings["ListingCommentsCollection"];
        private readonly IDBRepository _dbRepository;
        public ListingCommentsRepository(IDBRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        MongoCollection<TEntity> Classifieds
        {
            get { return _dbRepository.GetCollection<TEntity>(_collectionClassifieds); }
        }

        #endregion

        #region GetAllListingComments
        /// <summary>
        /// Returns a Listing Comments 
        /// </summary>
        /// <returns>Listing Comments</returns>
        public List<TEntity> GetAllListingComment(string listingId)
        {
            try
            {
                var partialRresult = Classifieds.FindAll()
                                     .Where(p => p.ListingId == listingId)
                                    .ToList();
                List<TEntity> result = partialRresult.Count > 0 ? partialRresult.ToList() : null;


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
        /// <param name="id">id</param>
        /// <returns>listingComments</returns>
        public List<TEntity> GetListingCommentsById(string id)
        {
            try
            {
                var partialRresult = Classifieds.FindAll()
                                        .Where(p => p._id == id)
                                        .ToList();

                List<TEntity> result = partialRresult.Count > 0 ? partialRresult.ToList() : null;

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
        /// <param name="listingComments">Listing Comment object</param>
        /// <returns>return newly added Listing Comment object</returns>
        public TEntity CreateListingComment(TEntity listingComments)
        {
            try
            {
                Classifieds.Save(listingComments);
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
        /// <param name="id">id</param>
        /// <param name="dataObj">Listing Comment object </param>
        /// <returns>updated Listing Comment  object</returns>
        /// 
        public TEntity UpdateListingComment(string id, TEntity dataObj)
        {
            try
            {
                var query = Query<TEntity>.EQ(p => p._id, id);
                var update = Update<TEntity>.Set(p => p.Comments, dataObj.Comments)
                                               .Set(p => p.UpdatedBy, dataObj.UpdatedBy)
                                               .Set(p => p.Verified, dataObj.Verified)
                                               .Set(p => p.UpdatedDate, dataObj.UpdatedDate);
                Classifieds.Update(query, update);
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
        /// <param name="id">Id</param>
        /// <returns>return void</returns>
        public void DeleteListingComment(string id)
        {
            try
            {
                var query = Query<TEntity>.EQ(p => p._id, id);
                Classifieds.Remove(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
