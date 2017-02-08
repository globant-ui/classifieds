using System.Collections.Generic;
using Classifieds.Listings.BusinessEntities;

namespace Classifieds.Listings.Repository
{
    public interface IListingRepository<TEntity> where TEntity : Listing
    {
        List<TEntity> GetListingById(string id);
        List<TEntity> GetListingsBySubCategory(string subCategory, int startIndex, int pageCount, bool isLast);
        List<TEntity> GetListingsByCategory(string category, int startIndex, int pageCount, bool isLast);
        TEntity Add(TEntity entity);
        TEntity Update(string id, TEntity entity);
        void Delete(string id);
        List<TEntity> GetTopListings(int noOfRecords);
        List<TEntity> GetListingsByEmail(string email);
        List<TEntity> GetListingsByCategoryAndSubCategory(string category, string subCategory);
    }
}
