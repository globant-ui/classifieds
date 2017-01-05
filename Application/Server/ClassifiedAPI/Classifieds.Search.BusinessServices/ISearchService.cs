#region Imports
    using System.Collections.Generic;
    using Classifieds.Search.BusinessEntities;
#endregion  

namespace Classifieds.Search.BusinessServices
{
    public interface ISearchService
    {
        List<Classified> FullTextSearch(string searchText);
    }
}
