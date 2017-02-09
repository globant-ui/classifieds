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
        List<string> GetCategorySuggetion(string categoryText);
        SubCategory GetAllFiltersBySubCategory(string subCategory);
        Filters GetFiltersByFilterName(string subCategory, string filterName);
        List<string> GetFilterNamesOnly(string subCategory);
    }
}
