#region Imports
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
#endregion

namespace Classifieds.MastersData.BusinessEntities
{
    public class CategorySuggestion
    {
        /// <summary>
        /// Class Name: CategorySuggestion.cs
        /// Purpose: Used as a property / DTO class to hold and pass property values.
        /// Created By: Santosh
        /// Created Date: 6-Jan-2017
        /// </summary>
        
        #region Propeties
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } 
        public string Category { get; set; }
        #endregion
    }
}
