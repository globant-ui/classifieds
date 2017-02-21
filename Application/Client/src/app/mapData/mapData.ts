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

      let data = {   
        "ListingType": (form.get('cardType').value!=undefined || form.get('cardType')!=null)?form.get('cardType').value:'',
        "ListingCategory": selectedCategory,
        "SubCategory": (form.get('subCategory').value!=undefined || form.get('subCategory')!=null)?form.get('subCategory').value:'',
        "Title": (form.get('title').value!=undefined || form.get('title')!=null)?form.get('title').value:'',
        "Address": "",
        "ContactNo": "",
        "ContactName": "",
        "Configuration": "",
        "Details": "",
        "Brand": (form.get('Brand').value!=undefined || form.get('Brand')!=null)?form.get('Brand').value:'',
        "Price": (form.get('price').value!=undefined || form.get('price')!=null)?form.get('price').value:'',
        "YearOfPurchase": 0,
        "ExpiryDate": "",
        "Status": "",
        "SubmittedBy": "",
        "SubmittedDate": cardCreatedDate,
        "IdealFor": "",
        "Furnished": "",
        "FuelType": "",
        "KmDriven": (form.get('KMDriven').value!=undefined || form.get('KMDriven')!=null)?form.get('KMDriven').value:'',
        "YearofMake": (form.get('Year').value!=undefined || form.get('Year')!=null )?form.get('Year').value:'',
        "Dimensions": '',
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



