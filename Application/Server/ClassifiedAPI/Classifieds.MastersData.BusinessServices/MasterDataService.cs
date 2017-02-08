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
        public MasterDataService(IMasterDataRepository<Category> masterDataRepository)
        {
            _masterDataRepository = masterDataRepository;
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
        /// <param name="categoryText">Category Text</param>
        /// <returns>All Category List</returns>
       
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
        /// <param name="categoryObj">Category Object</param>
        /// <returns>Newly added Category object</returns>
        public Category CreateCategory(Category categoryObj)
        {
            try
            {
                return _masterDataRepository.AddCategory(categoryObj);
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
        /// <param name="categoryObj">Category Object</param>
        /// <returns>Update Category obj</returns>
        public Category UpdateCategory(string id, Category categoryObj)
        {
            try
            {
                return _masterDataRepository.UpdateCategory(id, categoryObj);
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
        /// <param name="id">Id</param>
        /// <returns>deleted Id</returns>
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

        #region Get all filters by subcategory

        public SubCategory GetAllFiltersBySubCategory(string subCategory)
        {
            try
            {
                return _masterDataRepository.GetAllFiltersBySubCategory(subCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get filters by filter name and subcategory name

        public Filters GetFiltersByFilterName(string subCategory, string filterName)
        {
            try
            {
                return _masterDataRepository.GetFiltersByFilterName(subCategory, filterName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get filters by filter name and subcategory name

        public List<string> GetFilterNamesOnly(string subCategory)
        {
            try
            {
                return _masterDataRepository.GetFilterNamesOnly(subCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
