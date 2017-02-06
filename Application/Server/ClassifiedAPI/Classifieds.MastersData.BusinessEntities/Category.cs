﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Classifieds.MastersData.BusinessEntities
{
    public class Category
    {
        #region Propeties
        /// <summary>
        /// Class Name: Category.cs
        /// Purpose: Used as a property / DTO class to hold and pass property values.
        /// Created By: Varun
        /// Created Date: 27-Dec-2016
        /// </summary>

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } //MongoDb uses this field as identity.
        public string ListingCategory { get; set; }
        public string[] SubCategory { get; set; }
        public string Image { get; set; }

        #endregion

    }
}