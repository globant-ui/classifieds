#region Imports
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
#endregion

namespace Classifieds.UserService.BusinessEntities
{
    public class UserToken
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } //MongoDb uses this field as ideantity.

        public string UserEmail { get; set; }

        public string AccessToken { get; set; }

        public string LoginDateTime { get; set; }
    }
}
