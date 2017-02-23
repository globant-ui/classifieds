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
      let dimensions = (form.get('DimensionLength') && form.get('DimensionHeight') && form.get('DimensionWidth'))? {
         "Length": form.get('DimensionLength').value,
         "Width": form.get('DimensionWidth').value,
         "Height": form.get('DimensionHeight').value
      }:{};
      let data = {   
        "ListingType": (form.get('cardType'))?form.get('cardType').value:'',
        "ListingCategory": selectedCategory,
        "SubCategory": (form.get('subCategory'))?form.get('subCategory').value:'',
        "Title": (form.get('title'))?form.get('title').value:'',
        "Address": "",
        "ContactNo": (form.get('usercontact'))?form.get('usercontact').value:'',
        "ContactName": (form.get('username'))?form.get('username').value:'',
        "Configuration": "",
        "Details": (form.get('shortDesc'))?form.get('shortDesc').value:'',
        "Brand": (form.get('Brand'))?form.get('Brand').value:'',
        "Price": (form.get('price'))?form.get('price').value:'',
        "YearOfPurchase": (form.get('Year'))?form.get('Year').value:'',
        "ExpiryDate": "",
        "Status": "",
        "SubmittedBy": "",
        "SubmittedDate": cardCreatedDate,
        "IdealFor": (form.get('IdealFor'))?form.get('IdealFor').value:'',
        "Furnished": (form.get('Furnished'))?form.get('Furnished').value:'',
        "FuelType": (form.get('FuelType'))?form.get('FuelType').value:'',
        "KmDriven": (form.get('KMDriven'))?form.get('KMDriven').value:'',
        "YearofMake": 0,
        "Dimensions": dimensions,
        "TypeofUse": "",
        "Type": (form.get('Type'))?form.get('Type').value:'',
        "Model": "NA",
        "Negotiable": form.get('negotiable').value,
        "IsPublished": false,
        "Photos": []
      };
      return data;

  }
  
}



