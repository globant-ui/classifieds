﻿#region Imports
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
#endregion

namespace Classifieds.UserService.BusinessEntities
{
    public class ClassifiedsUser
    {
        
        /// <summary>
        /// Class Name: ClassifiedsUser.cs
        /// Purpose: Used as a property / DTO class to hold and pass property values.
        /// Created By: Globant
        /// Created Date: 10-Jan-2017
        /// Modified By : Amol Pawar
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } //MongoDb uses this field as identity.
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Designation { get; set; }
        public string Mobile { get; set; }
        public string Location { get; set; }
        public string[] WishList { get; set; }
        public string Image { get; set; }
        public Tags[] Tags;
        public Subscription[] Subscription;
    }

    public class Tags
    {
        public string SubCategory { get; set; }
        public string[] Location { get; set; }
    }

    public class Subscription
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public bool IsSms { get; set; }
        public bool IsEmail { get; set; }
    }
}
