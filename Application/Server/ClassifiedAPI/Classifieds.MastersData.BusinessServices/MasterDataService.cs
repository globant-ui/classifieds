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

        public List<CategoryViewModel> GetAllCategory()
        {
            try
            {
                //return _masterDataRepository.GetAllCategory().ToList();
                List<Category> categories = _masterDataRepository.GetAllCategory().ToList();
                List<CategoryViewModel> categoryVms = new List<CategoryViewModel>();
                List<string> subCategoryNames = new List<string>();
                foreach (Category ct in categories)
                {
                    foreach (SubCategory sc in ct.SubCategory ?? Enumerable.Empty<SubCategory>())
                    {
                        subCategoryNames.Add(sc.Name);
                    }
                    CategoryViewModel categoryVm = new CategoryViewModel
                    {
                        _id = ct._id,
                        ListingCategory = ct.ListingCategory,
                        Image = ct.Image,
                        SubCategory = subCategoryNames.ToArray()
                    };
                    categoryVms.Add(categoryVm);
                    subCategoryNames.Clear();
                }
                return categoryVms;
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

        /// <summary>
        /// Returns All sub Categories matching the imput text.
        /// </summary>
        /// <param name="subCategoryText">sub Category Text</param>
        /// <returns>All Sub Category List</returns>

        #region GetSubCategorySuggetion

        public List<string> GetSubCategorySuggestion(string subCategoryText)
        {
            try
            {
                return _masterDataRepository.GetSubCategorySuggestion(subCategoryText).ToList();
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
                SubCategory selectedSubCategory = null;
                var result = _masterDataRepository.GetAllFiltersBySubCategory(subCategory);
                if (result.Count > 0)
                {
                    foreach (SubCategory sc in result[0].SubCategory ?? Enumerable.Empty<SubCategory>())
                    {
                        if (subCategory.Contains(sc.Name))
                        {
                            selectedSubCategory = sc;
                        }
                    }
                }
                return selectedSubCategory;
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
                Filters selectedSubCategory = null;
                var result = _masterDataRepository.GetFiltersByFilterName(subCategory, filterName);
                if (result.Count > 0)
                {
                    foreach (SubCategory sc in result[0].SubCategory ?? Enumerable.Empty<SubCategory>())
                    {
                        if (subCategory.Contains(sc.Name))
                        {
                            foreach (Filters filter in sc.Filters ?? Enumerable.Empty<Filters>())
                            {
                                if (filterName.Contains(filter.FilterName))
                                {
                                    selectedSubCategory = filter;
                                }
                            }
                        }
                    }
                }
                return selectedSubCategory;
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
                List<string> selectedSubCategory = new List<string>();
                var result = _masterDataRepository.GetFilterNamesOnly(subCategory);
                if (result.Count > 0)
                {
                    foreach (SubCategory sc in result[0].SubCategory)
                    {
                        if (subCategory.Contains(sc.Name))
                        {
                            foreach (Filters flt in sc.Filters)
                            {
                                selectedSubCategory.Add(flt.FilterName);
                            }
                        }
                    }
                }
                return selectedSubCategory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
