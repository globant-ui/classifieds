﻿#region Imports
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
#endregion

namespace Classifieds.Listings.BusinessEntities
{
    public class Listing
    {
        /// <summary>
        /// Class Name: Listing.cs
        /// Purpose: Used as a property / DTO class to hold and pass property values.
        /// Created By: Globant
        /// Created Date: 12-Dec-2016
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } //MongoDb uses this field as identity.
        public string ListingType { get; set; }
        public string ListingCategory { get; set; }
        public string SubCategory { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string ContactName { get; set; }
        public string Configuration { get; set; }
        public string Details { get; set; }
        public string Brand { get; set; }
        public Int32 Price { get; set; }
        public Int32 YearOfPurchase { get; set; }
        public string Status { get; set; }
        public string SubmittedBy { get; set; }
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime SubmittedDate { get; set; }
        public string IdealFor { get; set; }
        public string Furnished { get; set; }
        public string FuelType { get; set; }
        public Int32 KmDriven { get; set; }
        public Int32 YearofMake { get; set; }
        public Dimension Dimensions { get; set; }
        public string TypeofUse { get; set; }
        public string Type { get; set; }
        public string Model { get; set; }
        public bool Negotiable { get; set; }
        public bool IsPublished { get; set; }
        public string[] Photos { get; set; }
    }
    public class Dimension
    {
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
    }
}
