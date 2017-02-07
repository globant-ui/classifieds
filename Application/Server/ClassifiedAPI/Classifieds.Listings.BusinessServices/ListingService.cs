﻿using System;
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
        #endregion

        #region Constructor
        public ListingService(IListingRepository<Listing> listingRepository)
        {            
            _listingRepository = listingRepository;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the collection of listing for given id
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <returns></returns>
        public List<Listing> GetListingById(string id)
        {
            try
            {
                return _listingRepository.GetListingById(id).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns the listings for given sub category
        /// </summary>
        /// <param name="subCategory">Sub category</param>
        /// <param name="startIndex">start index for page</param>
        /// <param name="pageCount">No of listings to include in result</param>
        //// <param name="isLast">Whether last page</param>
        /// <returns>Collection of listings</returns>
        public List<Listing> GetListingsBySubCategory(string subCategory, int startIndex, int pageCount, bool isLast)
        {
            try
            {
                return _listingRepository.GetListingsBySubCategory(subCategory, startIndex, pageCount, isLast).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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

        /// <summary>
        /// Update listing item for given Id
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <param name="listing">Listing Object</param>
        /// <returns></returns>
        public Listing UpdateListing(string id, Listing listing)
        {
            try
            {
                return _listingRepository.Update(id, listing);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
    }
}
