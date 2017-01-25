#region Imports
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
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } //MongoDb uses this field as identity.
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}
