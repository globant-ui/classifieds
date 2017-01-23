using System.Collections.Generic;
using Classifieds.MastersData.BusinessEntities;

namespace Classifieds.MastersData.Repository
{
    public interface IMasterDataRepository
    {
        List<Category> GetAllCategory();
        Category AddCategory(Category listObject);
        Category UpdateCategory(string id, Category listObject);
        void DeleteCategory(string id);
        List<Category> GetCategorySuggetion(string categoryText);
    }
}
