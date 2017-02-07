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
                var partialRresult = Classifieds.FindAll()
                                    .ToList();
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

        #region GetCategorySuggetion
        /// <summary>
        /// Returns All Categgries matching the imput text.
        /// </summary>
        /// <param name="categoryText">Category Text</param>
        /// <returns>Category List Suggetion</returns>
        public List<string> GetCategorySuggetion(string categoryText)
        {
            List<string> myCategory = null;
            try
            {
                var partialRresult = Classifieds.FindAll()
                                    .Where(p => p.ListingCategory.StartsWith(categoryText))
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
                var result = Classifieds.Save(categoryobj);
                if (result.DocumentsAffected == 0 && result.HasLastErrorMessage)
                {

                }
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

                var result = Classifieds.Update(query, update);
                if (result.DocumentsAffected == 0 && result.HasLastErrorMessage)
                {

                }

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
    }
}
