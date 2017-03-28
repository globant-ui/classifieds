using System.Collections.Generic;
using Classifieds.Listings.BusinessEntities;
using Classifieds.Listings.BusinessServices.ServiceAgent;

namespace Classifieds.Listings.BusinessServices
{
    public interface IListingService
    {
        Listing GetListingById(string id);
        List<Listing> GetListingsBySubCategory(string subCategory, int startIndex, int pageCount, bool isLast);
        List<Listing> GetListingsByCategory(string category, int startIndex, int pageCount, bool isLast);
        Listing CreateListing(Listing listObject);
        Listing UpdateListing(string id, Listing listObject);
        bool DeleteListing(string id);
        List<Listing> GetTopListings(int noOfRecords);
        List<Listing> GetListingsByEmail(string email, int startIndex, int pageCount, bool isLast);
        List<Listing> GetListingsByCategoryAndSubCategory(string category, string subCategory, string email, int startIndex, int pageCount, bool isLast);
        Listing CloseListing(string id, Listing entity);
        List<Listing> GetMyWishList(string[] listingIds);
        List<Listing> GetRecommendedList(Tags tags);
        void UpdateImagePath(string listingId, ListingImages[] photos);
    }
}
