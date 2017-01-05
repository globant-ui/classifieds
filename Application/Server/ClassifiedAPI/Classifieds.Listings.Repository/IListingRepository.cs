using System.Collections.Generic;
using Classifieds.Listings.BusinessEntities;

namespace Classifieds.Listings.Repository
{
    public interface IListingRepository
    {
        List<Listing> GetListingById(string id);
        List<Listing> GetListingsByCategory(string category);
        Listing Add(Listing listObject);
        Listing Update(string id, Listing listObject);
        void Delete(string id);
    }
}
