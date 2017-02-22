import {Injectable} from '@angular/core';

@Injectable()
export class mapData{
  
  constructor() {
  }

  mapCardData(selectedCategory,form) {
    console.log(form);
      let today = new Date();
      let locale = "en-us";
      let cardCreatedDate = today.getDate() + '-' + today.toLocaleString(locale, { month: "short" })  + '-' + today.getFullYear();
      //let DimensionHeight = 
      let dimensions = (form.get('DimensionLength')!=null && form.get('DimensionHeight')!=null && form.get('DimensionWidth')!=null)? {
         "Length": form.get('DimensionLength').value,
         "Width": form.get('DimensionWidth').value,
         "Height": form.get('DimensionHeight').value
      }:{};
      let data = {   
        "ListingType": (form.get('cardType')!=null)?form.get('cardType').value:'',
        "ListingCategory": selectedCategory,
        "SubCategory": (form.get('subCategory')!=null)?form.get('subCategory').value:'',
        "Title": (form.get('title')!=null)?form.get('title').value:'',
        "Address": "",
        "ContactNo": "",
        "ContactName": "",
        "Configuration": "",
        "Details": "",
        "Brand": (form.get('Brand')!=null)?form.get('Brand').value:'',
        "Price": (form.get('price')!=null)?form.get('price').value:'',
        "YearOfPurchase": 0,
        "ExpiryDate": "",
        "Status": "",
        "SubmittedBy": "",
        "SubmittedDate": cardCreatedDate,
        "IdealFor": "",
        "Furnished": "",
        "FuelType": "",
        "KmDriven": (form.get('KMDriven')!=null)?form.get('KMDriven').value:'',
        "YearofMake": (form.get('Year')!=null )?form.get('Year').value:'',
        "Dimensions": dimensions,
        "TypeofUse": "",
        "Type": "",
        "Model": "NA",
        "Negotiable": form.get('negotiable').value,
        "IsPublished": false,
        "Photos": []
      };
      return data;

  }
  
}



