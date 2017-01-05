#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classifieds.Search.BusinessEntities;
using Classifieds.Search.Repository;
#endregion  

namespace Classifieds.Search.BusinessServices
{
    public class SearchService : ISearchService
    {
        #region Private Variables

            private ISearchRepository _searchRepository;

        #endregion

        #region Constructor

        public SearchService(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public List<Classified> FullTextSearch(string searchText)
        {
            try
            {
                return _searchRepository.FullTextSearch(searchText).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
