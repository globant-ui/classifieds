using System.Collections.Generic;
using Classifieds.MastersData.BusinessEntities;

namespace Classifieds.MastersData.BusinessServices
{
    public interface IMasterDataService
    {
        List<CategoryViewModel> GetAllCategory();
        Category CreateCategory(Category listObject);
        Category UpdateCategory(string id, Category listObject);
        void DeleteCategory(string id);
        List<string> GetCategorySuggestion(string categoryText);
        List<string> GetSubCategorySuggestion(string subCategoryText);
        SubCategory GetAllFiltersBySubCategory(string subCategory);
        Filters GetFiltersByFilterName(string subCategory, string filterName);
        List<string> GetFilterNamesOnly(string subCategory);
    }
}
