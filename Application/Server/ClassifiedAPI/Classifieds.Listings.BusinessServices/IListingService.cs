using System.Collections.Generic;
using Classifieds.Listings.BusinessEntities;

namespace Classifieds.Listings.BusinessServices
{
    public interface IListingService
    {
        List<Listing> GetListingById(string id);
        List<Listing> GetListingsBySubCategory(string subCategory, int startIndex, int pageCount, bool isLast);
        List<Listing> GetListingsByCategory(string category, int startIndex, int pageCount, bool isLast);
        Listing CreateListing(Listing listObject);
        Listing UpdateListing(string id, Listing listObject);
        void DeleteListing(string id);
        List<Listing> GetTopListings(int noOfRecords);
        List<Listing> GetListingsByEmail(string email, int startIndex, int pageCount, bool isLast);
        List<Listing> GetListingsByCategoryAndSubCategory(string category, string subCategory, string email, int startIndex, int pageCount, bool isLast);
    }
}
