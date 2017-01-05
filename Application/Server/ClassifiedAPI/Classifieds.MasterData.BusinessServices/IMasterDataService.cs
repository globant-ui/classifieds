using System.Collections.Generic;
using Classifieds.MastersData.BusinessEntities;

namespace Classifieds.MastersData.BusinessServices
{
    public interface IMasterDataService
    {
        #region Properties

        List<Category> GetAllCategory();
        Category CreateCategory(Category listObject);
        Category UpdateCategory(string id, Category listObject);
        void DeleteCategory(string id);

        #endregion
    }
}
