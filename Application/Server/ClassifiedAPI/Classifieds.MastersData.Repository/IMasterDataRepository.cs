using System.Collections.Generic;
using Classifieds.MastersData.BusinessEntities;

namespace Classifieds.MastersData.Repository
{
    public interface IMasterDataRepository<TEntity> where TEntity : Category
    {
        List<TEntity> GetAllCategory();
        TEntity AddCategory(TEntity listObject);
        List<TEntity> GetCategoryById(string id);
        TEntity UpdateCategory(string id, TEntity listObject);
        void DeleteCategory(string id);
        List<string> GetCategorySuggetion(string categoryText);
    }
}
