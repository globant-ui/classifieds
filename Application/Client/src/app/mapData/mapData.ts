import {Injectable} from '@angular/core';

@Injectable()
export class mapData{
  
  constructor() {
  }

  mapCardData(form) {
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
        "ListingCategory": (form.get('category'))?form.get('category').value:'',
        "SubCategory": (form.get('subCategory'))?form.get('subCategory').value:'',
        "Title": (form.get('title'))?form.get('title').value:'',
        "Address": "",
        "Details": (form.get('shortDesc'))?form.get('shortDesc').value:'',
        "Brand": (form.get('Brand'))?form.get('Brand').value:'',
        "Price": (form.get('price'))?form.get('price').value:0,
        "YearOfPurchase": (form.get('Year'))?form.get('Year').value:'',
        "Status": "",
        "SubmittedDate": cardCreatedDate,
        "IdealFor": (form.get('IdealFor'))?form.get('IdealFor').value:'',
        "Furnished": (form.get('Furnished'))?form.get('Furnished').value:'',
        "FuelType": (form.get('FuelType'))?form.get('FuelType').value:'',
        "KmDriven": (form.get('KMDriven'))?form.get('KMDriven').value:0,
        "Dimensions": dimensions,
        "TypeofUse": "",
        "Type": (form.get('Type'))?form.get('Type').value:'',
        "Negotiable": (form.get('negotiable'))?form.get('negotiable').value:false,
        "IsPublished": false,
        "Photos": [],
        "SubmittedBy": (form.get('submittedBy'))?form.get('submittedBy').value:false,
        "Rooms": (form.get('Rooms'))?form.get('Rooms').value:'',
        "State": (form.get('state'))?form.get('state').value:'',
        "Country": (form.get('country'))?form.get('country').value:'',
        "City": (form.get('city'))?form.get('city').value:''
      };
      return data;

  }
  
}




