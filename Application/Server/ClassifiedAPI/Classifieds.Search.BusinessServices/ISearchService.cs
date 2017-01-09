#region Imports
    using System.Collections.Generic;
    using Classifieds.Listings.BusinessEntities;
#endregion  

namespace Classifieds.Search.BusinessServices
{
    public interface ISearchService
    {
        List<Listing> FullTextSearch(string searchText);
    }
}
