"use strict";
var DOMAIN = 'http://in-it0289';
var PATHS = {
    "GET_ALL_CATEGORIES": DOMAIN + "/MasterDataAPI/api/category/GetAllCategory",
    "CREATE_CARD": DOMAIN + "/ListingAPI/api/Listings/post",
    "SIMILAR_LISTING": DOMAIN + "/ListingAPI/api/Listings/GetListingsByCategoryAndSubCategory",
    "FILTERS": DOMAIN + "/MasterDataAPI/api/category/GetAllFiltersBySubCategory?subCategory="
};
var apiPaths = (function () {
    function apiPaths() {
        this.GET_ALL_CATEGORIES = PATHS.GET_ALL_CATEGORIES;
        this.CREATE_CARD = PATHS.CREATE_CARD;
        this.SIMILAR_LISTING = PATHS.SIMILAR_LISTING;
        this.FILTERS = PATHS.FILTERS;
    }
    return apiPaths;
}());
exports.apiPaths = apiPaths;
