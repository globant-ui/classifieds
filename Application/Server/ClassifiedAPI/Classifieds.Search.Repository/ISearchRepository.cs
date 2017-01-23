using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classifieds.Listings.BusinessEntities;

namespace Classifieds.Search.Repository
{
    public interface ISearchRepository<TEntity> where TEntity : Listing
    {
        List<TEntity> FullTextSearch(string searchText);        
    }
}
