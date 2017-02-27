#region Imports
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
#endregion

namespace Classifieds.Common.Entities
{
    public class UserToken
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } //MongoDb uses this field as ideantity.

        public string UserEmail { get; set; }

        public string AccessToken { get; set; }

        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime LoginDateTime { get; set; }

        public bool IsFirstTimeLogin { get; set; }

    }
}
