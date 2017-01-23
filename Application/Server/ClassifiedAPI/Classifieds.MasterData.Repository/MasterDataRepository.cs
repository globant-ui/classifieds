using Classifieds.MastersData.BusinessEntities;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Classifieds.MastersData.Repository
{
    public class MasterDataRepository<TEntity> : DBRepository, IMasterDataRepository<TEntity> where TEntity:Category
    {
        #region MasterDataRepository
        private readonly string _collectionClassifieds = ConfigurationManager.AppSettings["MasterDataCollection"];
        private readonly IDBRepository _dbRepository;
        public MasterDataRepository(IDBRepository DBRepository)
        {
            _dbRepository = DBRepository;
        }
        MongoCollection<TEntity> classifieds
        {
            get { return _dbRepository.GetCollection<TEntity>(_collectionClassifieds); }
        }

        #endregion

        #region GetAllCategory
        /// <summary>
        /// Returns a category 
        /// </summary>
        /// <returns>Category</returns>
        public List<TEntity> GetAllCategory()
        {
            try
            {
                var partialRresult = this.classifieds.FindAll()
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


        #region GetCategorySuggetion
        /// <summary>
        /// Returns All Categgries matching the imput text.
        /// </summary>
        /// <param name="catText">Category Text</param>
        /// <returns>Category List</returns>
        public List<string> GetCategorySuggetion(string categoryText)
        {
            List<string> myCategory = null;
            try
            {
                var partialRresult = this.classifieds.FindAll()
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
        /// <param name="object">Category object</param>
        /// <returns>return newly added Category object</returns>
        public TEntity AddCategory(TEntity category)
        {
            try
            {
                var result = this.classifieds.Save(category);
                if (result.DocumentsAffected == 0 && result.HasLastErrorMessage)
                {

                }
                return category;
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
        /// <param name="object">category object </param>
        /// <returns>return updated category object</returns>
        /// 

        public TEntity UpdateCategory(string id, TEntity dataObj)
        {
            try
            {
                var query = Query<TEntity>.EQ(p => p._id, id);
                var update = Update<TEntity>.Set(p => p.ListingCategory, dataObj.ListingCategory)
                                               .Set(p => p.SubCategory, dataObj.SubCategory)
                                               .Set(p => p.Image, dataObj.Image);

                var result = this.classifieds.Update(query, update);
                if (result.DocumentsAffected == 0 && result.HasLastErrorMessage)
                {

                }

                return dataObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DeleteCategory
        /// <summary>
        /// Delete category object based on id from the database
        /// </summary>
        /// <param name="id">category Id</param>
        /// <returns>return void</returns>
        public void DeleteCategory(string id)
        {
            try
            {
                var query = Query<TEntity>.EQ(p => p._id, id.ToString());
                var result = this.classifieds.Remove(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
