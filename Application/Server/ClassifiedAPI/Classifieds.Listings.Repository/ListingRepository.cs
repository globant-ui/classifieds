using Classifieds.Listings.BusinessEntities;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Classifieds.Listings.Repository
{
    public class ListingRepository<TEntity> : DBRepository, IListingRepository<TEntity> where TEntity : Listing
    {
        #region Private Variables
        private readonly string _collectionClassifieds = ConfigurationManager.AppSettings["ListingCollection"];
        private readonly IDBRepository _dbRepository;
        private enum Status
        {
            Active,
            Closed,
            Expired
        };
        private MongoCollection<TEntity> Classifieds
        {
            get { return _dbRepository.GetCollection<TEntity>(_collectionClassifieds); }
        }
        #endregion

        #region Constructor
        public ListingRepository(IDBRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        #endregion

        #region Public Methods

        #region GetListingById
        /// <summary>
        /// Returns a listing based on listing id
        /// </summary>
        /// <param name="id">listing id</param>
        /// <returns>listing</returns>
        public TEntity GetListingById(string id)
        {
            try
            {
                var query = Query<TEntity>.EQ(p => p._id, id);
                var partialRresult = Classifieds
                    .Find(query).SingleOrDefault(p => p.Status == Status.Active.ToString());

                return partialRresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetListingsBySubCategory
        /// <summary>
        /// Returns a collection of listings based on sub category
        /// </summary>
        /// <param name="subCategory">listing Sub Category</param>
        /// <param name="startIndex">start index for page</param>
        /// <param name="pageCount">No of listings to include in result</param>
        /// <param name="isLast">Whether last page</param>
        /// <returns>Collection of listings</returns>
        public List<TEntity> GetListingsBySubCategory(string subCategory, int startIndex, int pageCount, bool isLast)
        {
            try
            {
                int skip;
                if (isLast)
                {
                    int count = Classifieds.FindAll()
                   .Where(p => (p.SubCategory == subCategory) && (p.Status == Status.Active.ToString()))
                    .Count();
                    skip = GetLastPageSkipValue(pageCount, count);

                }
                else
                {
                    skip = startIndex - 1;
                }
                List<TEntity> listings = Classifieds.FindAll()
                                            .Where(p => (p.SubCategory == subCategory) && (p.Status == Status.Active.ToString()))
                                            .Select(p => p)
                                            .OrderByDescending(p=>p.SubmittedDate)
                                            .Skip(skip)
                                            .Take(pageCount)
                                            .ToList();
                listings = listings.Count > 0 ? listings.ToList() : null;
                return listings;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetListingsByCategory
        /// <summary>
        /// Returns a collection of listings based on category
        /// </summary>
        /// <param name="category">listing category</param>
        /// <param name="startIndex">start index for page</param>
        /// <param name="pageCount">No of listings to include in result</param>
        /// <param name="isLast">Whether last page</param>
        /// <returns>Collection of listings</returns>
        public List<TEntity> GetListingsByCategory(string category, int startIndex, int pageCount, bool isLast)
        {
            try
            {
                int skip;
                if (isLast)
                {
                    int count = Classifieds.FindAll()
                    .Where(p => (p.ListingCategory == category) && (p.Status == Status.Active.ToString()))
                    .Count();
                    skip = GetLastPageSkipValue(pageCount, count);
                }
                else
                {
                    skip = startIndex - 1;
                }
                List<TEntity> listings = Classifieds.FindAll()
                                            .Where(p => (p.ListingCategory == category) && (p.Status == Status.Active.ToString()))
                                            .Select(p => p)
                                            .OrderByDescending(p => p.SubmittedDate)
                                            .Skip(skip)
                                            .Take(pageCount)
                                            .ToList();
                listings = listings.Count > 0 ? listings.ToList() : null;
                return listings;
            }
            catch (MongoException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetTopListings
        /// <summary>
        /// Returns top listing object collection
        /// </summary>
        /// <param name="noOfRecords">integer value for retrieving number of records for listing collection</param>
        /// <returns>Listing collection</returns>
        public List<TEntity> GetTopListings(int noOfRecords)
        {
            try
            {
                SortByBuilder sortBuilder = new SortByBuilder();
                sortBuilder.Descending("_id");
                var result = Classifieds.FindAllAs<TEntity>().SetSortOrder(sortBuilder).SetLimit(noOfRecords).Where(p => p.Status == Status.Active.ToString());
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetListingsByEmail
        /// <summary>
        /// Returns a listing based on listing Email
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="pageCount">pageCount</param>
        /// <param name="isLast">isLast</param>
        /// <returns>Listing Email</returns>
        public List<TEntity> GetListingsByEmail(string email, int startIndex, int pageCount, bool isLast)
        {
            try
            {
                int skip;
                if (isLast)
                {
                    int count = Classifieds
                   .FindAll()
                    .Count(p => p.SubmittedBy == email);
                    skip = GetLastPageSkipValue(pageCount, count);

                }
                else
                {
                    skip = startIndex - 1;
                }

                List<TEntity> listings = Classifieds.FindAll()
                                            .Where(p => p.SubmittedBy == email)
                                            .Select(p => p)
                                            .OrderByDescending(p => p.SubmittedDate)
                                            .Skip(skip)
                                            .Take(pageCount)
                                            .ToList();
                listings = listings.Count > 0 ? listings.ToList() : null;
                return listings;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion GetListingsByEmail

        #region GetListingsByCategoryAndSubCategory
        /// <summary>
        /// Returns a listing based on listing Email
        /// </summary>
        /// <param name="category">Listing category</param>
        /// <param name="subCategory">Listing subCategory</param>
        /// <param name="email">User email</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="pageCount">pageCount</param>
        /// <param name="isLast">isLast</param>
        /// <returns>Listing Email</returns>
        public List<TEntity> GetListingsByCategoryAndSubCategory(string category, string subCategory, string email, int startIndex, int pageCount, bool isLast)
        {
            try
            {
                int skip;
                if (isLast)
                {
                    int count = Classifieds
                   .FindAll()
                    .Count(p => (p.ListingCategory == category) && (p.SubCategory == subCategory) && (p.SubmittedBy != email) && (p.Status == Status.Active.ToString()));
                    skip = GetLastPageSkipValue(pageCount, count);

                }
                else
                {
                    skip = startIndex - 1;
                }
                List<TEntity> result = Classifieds.FindAll()
                                            .Where(p => (p.ListingCategory == category) && (p.SubCategory == subCategory) && (p.SubmittedBy != email) && (p.Status == Status.Active.ToString()))
                                            .Select(p => p)
                                            .OrderByDescending(p => p.SubmittedDate)
                                            .Skip(skip)
                                            .Take(pageCount)
                                            .ToList();
                result = result.Count > 0 ? result.ToList() : null;
                return result;
            }
            catch (MongoException ex)
            {
                throw ex;
            }
        }

        #endregion GetListingsByCategoryAndSubCategory

        #region GetMyWishList
        /// <summary>
        /// Returns listing object collection for given listing ids
        /// </summary>
        /// <param name="listingIds">array of listing ids</param>
        /// <returns>Listing collection</returns>
        public List<TEntity> GetMyWishList(string[] listingIds)
        {
            try
            {
                if (listingIds != null)
                {
                    ObjectId[] newObjectId = listingIds.Select(item => ObjectId.Parse(item)).ToArray();
                    var query = Query.In("_id", BsonArray.Create(newObjectId));
                    SortByBuilder sortBuilder = new SortByBuilder();
                    sortBuilder.Descending("_id");
                    var listings = Classifieds.Find(query).SetSortOrder(sortBuilder).Where(p => p.Status == Status.Active.ToString());
                    return listings.ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetRecommendedList
        /// <summary>
        /// Returns recommended listing collection for given tag
        /// </summary>
        /// <param name="tag">array of listing ids</param>
        /// <returns>Listing collection</returns>
        public List<TEntity> GetRecommendedList(Tags tag)
        {
            try
            {
                var finalQuery = Query.And(Query.In("City", BsonArray.Create(tag.Location)), Query.In("SubCategory", BsonArray.Create(tag.SubCategory)));
                var listings = Classifieds.Find(finalQuery).Where(p => p.Status == Status.Active.ToString())
                                                            .OrderByDescending(p => p.SubmittedDate);
                return listings.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Add Listing
        /// <summary>
        /// Insert a new listing object into the database
        /// </summary>
        /// <param name="listing">listing object</param>
        /// <returns>return newly added listing object</returns>
        public TEntity Add(TEntity listing)
        {
            try
            {
                Classifieds.Save(listing);
                return listing;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Update Listing
        /// <summary>
        /// Update existing listing object based on id from the database
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <param name="listObj">listing object </param>
        /// <returns>return updated listing object</returns>
        public TEntity Update(string id, TEntity listObj)
        {
            try
            {
                var query = Query<TEntity>.EQ(p => p._id, id);
                var update = Update<TEntity>.Set(p => p.Title, listObj.Title)
                    .Set(p => p.ListingType, listObj.ListingType)
                    .Set(p => p.ListingCategory, listObj.ListingCategory)
                    .Set(p => p.Brand, listObj.Brand)
                    .Set(p => p.Price, listObj.Price)
                    .Set(p => p.YearOfPurchase, listObj.YearOfPurchase)
                    .Set(p => p.Status, listObj.Status)
                    //.Set(p => p.SubmittedBy, listObj.SubmittedBy)
                    //.Set(p => p.SubmittedDate, listObj.SubmittedDate)
                    .Set(p => p.IdealFor, listObj.IdealFor)
                    .Set(p => p.Furnished, listObj.Furnished)
                    .Set(p => p.FuelType, listObj.FuelType)
                    .Set(p => p.KmDriven, listObj.KmDriven)
                    .Set(p => p.DimensionLength, listObj.DimensionLength)
                    .Set(p => p.DimensionWidth, listObj.DimensionWidth)
                    .Set(p => p.DimensionHeight, listObj.DimensionHeight)
                    .Set(p => p.TypeofUse, listObj.TypeofUse)
                    .Set(p => p.Address, listObj.Address)
                    .Set(p => p.Details, listObj.Details)
                    .Set(p => p.SubCategory, listObj.SubCategory)
                    .Set(p => p.Type, listObj.Type)
                    .Set(p => p.Negotiable, listObj.Negotiable)
                    .Set(p => p.IsPublished, listObj.IsPublished);

                Classifieds.Update(query, update);
                return listObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete Listing
        /// <summary>
        /// Delete listing object based on id from the database
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <returns>return void</returns>
        public void Delete(string id)
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

        #region CloseListing

        /// <summary>
        /// Update Close listing  based on id from the database
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <param name="listObj">listing object </param>
        /// <returns>return updated listing object</returns>
        public bool CloseListing(string id)
        {
            try
            {                
                var update = Update<TEntity>.Set(p => p.Status, Convert.ToString(Status.Closed));
                var result = Classifieds.Update(Query<TEntity>.EQ(p => p._id, id), update);
                return result.UpdatedExisting;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion CloseListing

        #region UpdateImagePath

        public void UpdateImagePath(string listingId, ListingImages[] photos)
        {
            try
            {
                foreach (var t in photos)
                {
                    var result = Query<TEntity>.ElemMatch(p => p.Photos, builder => Query<ListingImages>.EQ(sc => sc.ImageName, t.ImageName));
                    var isExist= Classifieds.Find(result).ToList();
                    if (isExist.Count == 0)
                    {
                        var queryBuilder = Query<TEntity>.EQ(p => p._id, listingId);
                        var update = Update<TEntity>.Push(p => p.Photos, t);
                        Classifieds.Update(queryBuilder, update);
                    }
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PublishListing

        /// <summary>
        /// Update listing's isPublished field based on id from the database
        /// </summary>
        /// <param name="id">Listing Id</param>        
        /// <returns>return true/false</returns>
        public bool PublishListing(string id)
        {
            try
            {
                var update = Update<TEntity>.Set(p => p.IsPublished, true)
                    .Set(p => p.Status, Convert.ToString(Status.Active));
                var result = Classifieds.Update(Query<TEntity>.EQ(p => p._id, id), update);
                return result.UpdatedExisting;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion PublishListing
        #endregion

        #region private methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageCount"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        private int GetLastPageSkipValue(int pageCount, int rowCount)
        {
            int temp;
            if (rowCount % pageCount == 0)
            {
                temp = rowCount / pageCount - 1;
            }
            else
            {
                temp = rowCount / pageCount;
            }
            return temp * pageCount;
        }
        #endregion
    }
}
