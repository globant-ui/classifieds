using System.Collections.Generic;
using Classifieds.Listings.BusinessEntities;

namespace Classifieds.Listings.Repository
{
    public interface IListingRepository<TEntity> where TEntity : Listing
    {
        List<TEntity> GetListingById(string id);
        List<TEntity> GetListingsBySubCategory(string subCategory, int startIndex, int pageCount);
        List<TEntity> GetListingsByCategory(string category, int startIndex, int pageCount);
        TEntity Add(TEntity entity);
        TEntity Update(string id, TEntity entity);
        void Delete(string id);
        List<TEntity> GetTopListings(int noOfRecords);        
    }
}
