#region Imports
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
#endregion

namespace Classifieds.ListingComments.BusinessEntities
{
    public class ListingComment
    {

        /// <summary>
        /// Class Name: ListingComments.cs
        /// Purpose: Used as a property / DTO class to hold and pass property values.
        /// Created By: Varun
        /// Created Date: 06-Jan-2017
        /// </summary>
        #region Propeties

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } //MongoDb uses this field as identity.
        public string ListingId { get; set; }
        public string SubmittedDate { get; set; }
        public string SubmittedBy { get; set; }
        public bool Verified { get; set; }
        public string Comments { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        #endregion
    }
}
