import {Injectable} from '@angular/core';

@Injectable()
export class mapData{
  
  constructor() {
  }

  mapCardData(form) {
      let today = new Date();
      let locale = "en-us";
      let cardCreatedDate = today.getDate() + '-' + today.toLocaleString(locale, { month: "short" })  + '-' + today.getFullYear();

      let data = {   
        "ListingType": form._value.cardType,
        "ListingCategory": form._value.selectedCategory,
        "SubCategory": form._value.subCategory,
        "Title": form._value.title,
        "Address": "",
        "ContactNo": "",
        "ContactName": "",
        "Configuration": "",
        "Details": "",
        "Brand": "",
        "Price": 0,
        "YearOfPurchase": 0,
        "ExpiryDate": "",
        "Status": "",
        "SubmittedBy": "",
        "SubmittedDate": cardCreatedDate,
        "IdealFor": "",
        "Furnished": "",
        "FuelType": "",
        "KmDriven": 0,
        "YearofMake": 0,
        "Dimensions": "",
        "TypeofUse": "",
        "Type": "",
        "Model": "NA",
        "Negotiable": form._value.negotiable,
        "IsPublished": false,
        "Photos": []
      };
      return data;

  }
  
}



