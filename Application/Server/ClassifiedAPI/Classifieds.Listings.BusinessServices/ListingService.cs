using System;
using System.Collections.Generic;
using System.Linq;
using Classifieds.Listings.BusinessEntities;
using Classifieds.Listings.Repository;

namespace Classifieds.Listings.BusinessServices
{
    public class ListingService : IListingService
    {
        #region Private Variables
        private readonly IListingRepository<Listing> _listingRepository;
        public enum Status
        {
            Active,
            Closed,
            Expired
        };
        #endregion

        #region Constructor
        public ListingService(IListingRepository<Listing> listingRepository)
        {
            _listingRepository = listingRepository;
        }
        #endregion

        #region Public Methods

        #region GetListingById
        /// <summary>
        /// Returns the collection of listing for given id
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <returns></returns>
        public Listing GetListingById(string id)
        {
            try
            {
                return _listingRepository.GetListingById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetListingsBySubCategory

        /// <summary>
        /// Returns the listings for given sub category
        /// </summary>
        /// <param name="subCategory">Sub category</param>
        /// <param name="startIndex">start index for page</param>
        /// <param name="pageCount">No of listings to include in result</param>
        /// <param name="isLast">Whether last page</param>
        /// <returns>Collection of listings</returns>
        public List<Listing> GetListingsBySubCategory(string subCategory, int startIndex, int pageCount, bool isLast)
        {
            try
            {
                return _listingRepository.GetListingsBySubCategory(subCategory, startIndex, pageCount, isLast);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetListingsByCategory

        /// <summary>
        /// service method returns collection of listings based on category
        /// </summary>
        /// <param name="category">Cateogry</param>
        /// <param name="startIndex">start index for page</param>
        /// <param name="pageCount">No of listings to include in result</param>
        /// <param name="isLast">Whether last page</param>
        /// <returns>Collection of listings</returns>
        public List<Listing> GetListingsByCategory(string category, int startIndex, int pageCount, bool isLast)
        {
            try
            {
                return _listingRepository.GetListingsByCategory(category, startIndex, pageCount, isLast);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        
        #region GetTopListings

        /// <summary>
        /// Returns top listings from database  
        /// </summary>
        /// <param name="noOfRecords">Number of listing collection to be return </param>
        /// <returns></returns>
        public List<Listing> GetTopListings(int noOfRecords)
        {
            try
            {
                return _listingRepository.GetTopListings(noOfRecords);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetListingsByEmail
        /// <summary>
        /// Returns the collection of listing for given email
        /// </summary>
        /// <param name="email">listing email</param>
        /// <param name="startIndex">listing startIndex</param>
        /// <param name="pageCount">listing pageCount</param>
        /// <param name="isLast">listing isLast</param>
        /// <returns></returns>
        public List<Listing> GetListingsByEmail(string email, int startIndex, int pageCount, bool isLast)
        {
            try
            {
                return _listingRepository.GetListingsByEmail(email, startIndex, pageCount, isLast);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion GetListingsByEmail

        #region GetListingsByCategoryAndSubCategory

        /// <summary>
        /// service method returns collection of listing by Ccatgeory and Subcategory
        /// </summary>
        /// <param name="category">listing category</param>
        /// <param name="subCategory">listing subCategory</param>
        /// <param name="email">listing email</param>
        /// <param name="startIndex">listing startIndex</param>
        /// <param name="pageCount">listing pageCount</param>
        /// <param name="isLast">listing isLast</param>
        /// <returns>collection(listing)</returns>
        public List<Listing> GetListingsByCategoryAndSubCategory(string category, string subCategory, string email, int startIndex, int pageCount, bool isLast)
        {
            try
            {
                return _listingRepository.GetListingsByCategoryAndSubCategory(category, subCategory, email, startIndex, pageCount, isLast);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion GetListingsByCategoryAndSubCategory

        #region GetMyWishlist

        /// <summary>
        /// Returns the collection of listing for given listing Ids
        /// </summary>
        /// <param name="listingIds">array of Listing Id</param>
        /// <returns></returns>
        public List<Listing> GetMyWishList(string[] listingIds)
        {
            try
            {
                return _listingRepository.GetMyWishList(listingIds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetRecommendedList

        public List<Listing> GetRecommendedList(Tags tags)
        {
            try
            {
                return _listingRepository.GetRecommendedList(tags);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetPhotosByListingId
        public ListingImages[] GetPhotosByListingId(string id)
        {
            try
            {
                return _listingRepository.GetPhotosByListingId(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region CreateListing
        /// <summary>
        /// Create new listing item into the database
        /// </summary>
        /// <param name="listing">Listing Object</param>
        /// <returns></returns>
        public Listing CreateListing(Listing listing)
        {
            try
            {
                return _listingRepository.Add(listing);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region UpdateListing
        /// <summary>
        /// Update listing item for given Id
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <param name="listing">Listing Object</param>
        /// <returns></returns>
        public Listing UpdateListing(Listing listing)
        {
            try
            {
                return _listingRepository.Update(listing);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DeleteListing
        /// <summary>
        /// Delete listing item for given Id
        /// </summary>
        /// <param name="id">Listing Id</param>
        public void DeleteListing(string id)
        {
            try
            {
                _listingRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region UpdateImagePath

        public void UpdateImagePath(string listingId, ListingImages[] photos)
        {
            try
            {
                _listingRepository.UpdateImagePath(listingId, photos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CloseListing

        /// <summary>
        /// Update listing status for given Id
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <param name="listing">Listing Object</param>
        /// <returns></returns>
        public bool CloseListing(string id)
        {
            try
            {
                return _listingRepository.CloseListing(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion CloseListing

        #region PublishListing

        /// <summary>
        /// Update listing's isPublished field for given Id
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <param name="listing">Listing Object</param>
        /// <returns></returns>
        public bool PublishListing(string id)
        {
            try
            {
                return _listingRepository.PublishListing(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion PublishListing

        #region DeleteListingImage

        /// <summary>
        /// Update listing's isPublished field for given Id
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <param name="listing">Listing Object</param>
        /// <returns></returns>
        public bool DeleteListingImage(string id, ListingImages lstImage)
        {
            try
            {
                return _listingRepository.DeleteListingImage(id, lstImage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion DeleteListingImage
        #endregion
    }
}
