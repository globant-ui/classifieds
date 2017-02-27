using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classifieds.Listings.BusinessEntities;

namespace Classifieds.Search.Repository
{
    public interface ISearchRepository
    {
        List<Listing> FullTextSearch(string searchText);
    }
}
