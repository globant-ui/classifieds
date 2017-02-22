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
        "Details": (form.get('shortDesc')!=null)?form.get('shortDesc').value:'',
        "Brand": (form.get('Brand')!=null)?form.get('Brand').value:'',
        "Price": (form.get('price')!=null)?form.get('price').value:'',
        "YearOfPurchase": (form.get('Year')!=null )?form.get('Year').value:'',
        "ExpiryDate": "",
        "Status": "",
        "SubmittedBy": "",
        "SubmittedDate": cardCreatedDate,
        "IdealFor": (form.get('IdealFor')!=null)?form.get('IdealFor').value:'',
        "Furnished": (form.get('Furnished')!=null)?form.get('Furnished').value:'',
        "FuelType": (form.get('FuelType')!=null)?form.get('FuelType').value:'',
        "KmDriven": (form.get('KMDriven')!=null)?form.get('KMDriven').value:'',
        "YearofMake": 0,
        "Dimensions": dimensions,
        "TypeofUse": "",
        "Type": (form.get('Type')!=null)?form.get('Type').value:'',
        "Model": "NA",
        "Negotiable": form.get('negotiable').value,
        "IsPublished": false,
        "Photos": []
      };
      return data;

  }
  
}



