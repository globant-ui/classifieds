
const DOMAIN = 'http://in-it0289';
const PATHS = {
    "GET_ALL_CATEGORIES": DOMAIN+"/MasterDataAPI/api/category/GetAllCategory",
    "CREATE_CARD": DOMAIN+"/ListingAPI/api/Listings/post"
};

export class apiPaths{
    public GET_ALL_CATEGORIES = PATHS.GET_ALL_CATEGORIES;
    public CREATE_CARD = PATHS.CREATE_CARD;    
}
