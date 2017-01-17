using System;
using System.Collections.Generic;
using System.Linq;
using Classifieds.MastersData.BusinessEntities;
using Classifieds.MastersData.Repository;


namespace Classifieds.MastersData.BusinessServices
{
    public class MasterDataService : IMasterDataService
    {
        #region MasterDataService

        private readonly IMasterDataRepository<Category> _masterDataRepository;
        /// <summary>
        /// The class constructor. 
        /// </summary>
        public MasterDataService(IMasterDataRepository<Category> MasterDataRepository)
        {
            _masterDataRepository = MasterDataRepository;
        }

        #endregion
        /// <summary>
        /// Returns All Category
        /// </summary>
        /// <returns></returns>
        #region GetAllCategory

        public List<Category> GetAllCategory()
        {
            try
            {
                return _masterDataRepository.GetAllCategory().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        
        /// <summary>
        /// Returns All Categgries matching the imput text.
        /// </summary>
        /// <param name="catText">Category Text</param>
        /// <returns>Category List</returns>
        #region GetCategorySuggetion

        public List<string> GetCategorySuggetion(string categoryText)
        {
            try
            {
                return _masterDataRepository.GetCategorySuggetion(categoryText).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CreateCategory

        /// <summary>
        /// Insert new Category item into the database
        /// </summary>
        /// <param name="Category">Category Object</param>
        /// <returns></returns>
        public Category CreateCategory(Category listObject)
        {
            try
            {
                return _masterDataRepository.AddCategory(listObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region UpdateCategory
        /// <summary>
        /// Update Category item for given Id
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <returns></returns>
        public Category UpdateCategory(string id, Category listObject)
        {
            try
            {
                return _masterDataRepository.UpdateCategory(id, listObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DeleteCategory
        /// <summary>
        /// Delete Category item for given Id
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <returns></returns>
        public void DeleteCategory(string id)
        {
            try
            {
                _masterDataRepository.DeleteCategory(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
