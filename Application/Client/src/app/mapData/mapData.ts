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

      let address = (form.get('area') && form.get('city') && form.get('state') && form.get('country')) ? form.get('area').value + '-' + form.get('city').value + '-' + form.get('state').value + '-' + form.get('country').value : '';
      debugger;
      let data = {   
        "ListingType": (form.get('cardType'))?form.get('cardType').value:'',
        "ListingCategory": (form.get('category'))?form.get('category').value:'',
        "SubCategory": (form.get('subCategory'))?form.get('subCategory').value:'',
        "Title": (form.get('title'))?form.get('title').value:'',
        "Address": address,
        "ContactNo": "",
        "ContactName": "",
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



