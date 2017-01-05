using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classifieds.Listings.BusinessEntities;
using Classifieds.Listings.Repository;

namespace Classifieds.Listings.BusinessServices
{
    public class ListingService : IListingService
    {
        private IListingRepository _listingRepository;

        public ListingService(IListingRepository ListingRepository)
        {
            //_listingRepository = new ListingRepository();
            _listingRepository = ListingRepository;
        }

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
        /// service method returns collection of listing
        /// </summary>
        /// <param name="category">listing category</param>
        /// <returns>collection(listing)</returns>
        public List<Listing> GetListingsByCategory(string category)
        {
            try
            {
                return _listingRepository.GetListingsByCategory(category);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public Listing CreateListing(Listing listObject)
        {
            try
            {
                return _listingRepository.Add(listObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Listing UpdateListing(string id, Listing listObject)
        {
            try
            {
                return _listingRepository.Update(id, listObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
    }
}
