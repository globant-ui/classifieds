using System.Collections.Generic;
using Classifieds.Listings.BusinessEntities;

namespace Classifieds.Search.Repository
{
    public interface ISearchRepository<TEntity> where TEntity : Listing
    {
        List<TEntity> FullTextSearch(string searchText, int startIndex, int pageCount, bool isLast);        
    }
}
