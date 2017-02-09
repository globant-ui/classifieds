using System.Collections.Generic;
using Classifieds.MastersData.BusinessEntities;

namespace Classifieds.MastersData.BusinessServices
{
    public interface IMasterDataService
    {
        List<Category> GetAllCategory();
        Category CreateCategory(Category listObject);
        Category UpdateCategory(string id, Category listObject);
        void DeleteCategory(string id);
        List<string> GetCategorySuggetion(string categoryText);
        List<string> GetSubCategorySuggestion(string subCategoryText);
    }
}
