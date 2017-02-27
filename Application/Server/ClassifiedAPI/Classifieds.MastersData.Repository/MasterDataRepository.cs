using Classifieds.MastersData.BusinessEntities;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Classifieds.MastersData.Repository
{
    public class MasterDataRepository<TEntity> : DBRepository, IMasterDataRepository<TEntity> where TEntity : Category
    {
        #region MasterDataRepository
        private readonly string _collectionClassifieds = ConfigurationManager.AppSettings["MasterDataCollection"];
        private readonly IDBRepository _dbRepository;
        public MasterDataRepository(IDBRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        MongoCollection<TEntity> Classifieds
        {
            get { return _dbRepository.GetCollection<TEntity>(_collectionClassifieds); }
        }

        #endregion

        #region GetAllCategory
        /// <summary>
        /// Returns a category 
        /// </summary>
        /// <returns>All Category object</returns>
        public List<TEntity> GetAllCategory()
        {
            try
            {
                var partialRresult = Classifieds.FindAll().ToList();
                List<TEntity> result = partialRresult.Count > 0 ? partialRresult.ToList() : null;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetCategoryById
        /// <summary>
        /// Returns a Category based on id
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>Category object by id</returns>
        public List<TEntity> GetCategoryById(string id)
        {
            try
            {
                var partialRresult = Classifieds.FindAll()
                                        .Where(p => p._id == id)
                                        .ToList();

                List<TEntity> result = partialRresult.Count > 0 ? partialRresult.ToList() : null;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion GetCategoryById

        #region GetCategorySuggestion
        /// <summary>
        /// Returns All Categgries matching the imput text.
        /// </summary>
        /// <param name="categoryText">Category Text</param>
        /// <returns>Category List Suggetion</returns>
        public List<string> GetCategorySuggestion(string categoryText)
        {
            List<string> myCategory = null;
            try
            {
                var partialRresult = Classifieds.FindAll()
                                    .Where(p => p.ListingCategory.ToUpper().StartsWith(categoryText.ToUpper()))
                                    .ToList();
                List<TEntity> result = partialRresult.Count > 0 ? partialRresult.ToList() : null;

                if (result != null)
                    myCategory = result.Select(l => l.ListingCategory).ToList();

                return myCategory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetSubCategorySuggetion
        /// <summary>
        /// Returns All Categgries matching the imput text.
        /// </summary>
        /// <param name="subCategoryText">Sub Category Text</param>
        /// <returns>Sub Category List Suggestion</returns>
        public List<string> GetSubCategorySuggestion(string subCategoryText)
        {
            List<string> mySubCategory = null;
            try
            {
                var partialRresult = Classifieds.FindAll()
                                    .ToList();
                if (partialRresult.Count > 0)
                {
                    mySubCategory = new List<string>();
                    foreach (var elem in partialRresult)
                    {
                        if (elem.SubCategory != null)
                        {
                            foreach (SubCategory s in elem.SubCategory)
                            {
                                if (s.Name.ToUpper().Contains(subCategoryText.ToUpper()))
                                    mySubCategory.Add(s.Name);
                            }
                        }
                    }
                }

                return mySubCategory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AddCategory

        /// <summary>
        /// Insert a new Category object into the database
        /// </summary>
        /// <param name="categoryobj">Category object</param>
        /// <returns>Newly added Category object</returns>
        public TEntity AddCategory(TEntity categoryobj)
        {
            try
            {
                Classifieds.Save(categoryobj);
                return categoryobj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Updatecategory

        /// <summary>
        /// Update existing category object based on id from the database
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="categoryObj">Category object </param>
        /// <returns>Updated Category object</returns>
        /// 

        public TEntity UpdateCategory(string id, TEntity categoryObj)
        {
            try
            {
                var query = Query<TEntity>.EQ(p => p._id, id);
                var update = Update<TEntity>.Set(p => p.ListingCategory, categoryObj.ListingCategory)
                                               .Set(p => p.SubCategory, categoryObj.SubCategory)
                                               .Set(p => p.Image, categoryObj.Image);

                Classifieds.Update(query, update);
                return categoryObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DeleteCategory
        /// <summary>
        /// Delete Category object based on id from the database
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>return void</returns>
        public void DeleteCategory(string id)
        {
            try
            {
                var query = Query<TEntity>.EQ(p => p._id, id);
                Classifieds.Remove(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetAllFiltersBySubCategory
        /// <summary>
        /// Returns all filters for specific subcategory with filter name and filter values
        /// </summary>
        /// <param name="subCategory">subCategory Name</param>
        /// <returns></returns>
        public List<TEntity> GetAllFiltersBySubCategory(string subCategory)
        {
            try
            {
                var subCategoryQuery = Query<Category>.ElemMatch(p => p.SubCategory, builder => Query<SubCategory>.EQ(sc => sc.Name, subCategory));
                return Classifieds.Find(subCategoryQuery).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetFiltersByFilterName
        /// <summary>
        /// Returns all filter values for given FilterName and subcategory
        /// </summary>
        /// <param name="subCategory">Subcategory Name</param>
        /// <param name="filterName">Filter Name</param>
        /// <returns></returns>
        public List<TEntity> GetFiltersByFilterName(string subCategory, string filterName)
        {
            try
            {
                var filterQuery = Query<Category>.ElemMatch(p => p.SubCategory, builder => Query<SubCategory>.EQ(sc => sc.Name, subCategory));
                return Classifieds.Find(filterQuery).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetFiltersNames
        /// <summary>
        /// Get filter names only for specific subcategory
        /// </summary>
        /// <param name="subCategory">Subcategory Name</param>
        /// <returns></returns>
        public List<TEntity> GetFilterNamesOnly(string subCategory)
        {
            try
            {
                var subCategoryQuery = Query<SubCategory>.EQ(sc => sc.Name, subCategory);
                var finalQuery = Query<Category>.ElemMatch(p => p.SubCategory, builder => subCategoryQuery);
                return Classifieds.Find(finalQuery).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
