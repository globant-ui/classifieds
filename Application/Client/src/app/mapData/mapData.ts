import {Injectable} from '@angular/core';

@Injectable()
export class MapData{

  constructor() {
  }

  mapCardData(form) {
      let today = new Date();
      let locale = "en-us";
      let cardCreatedDate = today.getDate() + '-' + today.toLocaleString(locale, { month: "short" })  + '-' + today.getFullYear();
//       let dimensions = (form.get('DimensionLength') && form.get('DimensionHeight') && form.get('DimensionWidth'))? {
//          "Length": form.get('DimensionLength').value,
//          "Width": form.get('DimensionWidth').value,
//          "Height": form.get('DimensionHeight').value
//       }:{};

      let address = (form.get('area') && form.get('city') && form.get('state') && form.get('country')) ? form.get('area').value + '-' + form.get('city').value + '-' + form.get('state').value + '-' + form.get('country').value : '';
      let data = {

        "ListingType": (form.get('cardType'))?form.get('cardType').value:'',
        "ListingCategory": (form.get('category'))?form.get('category').value:'',
        "SubCategory": (form.get('subCategory'))?form.get('subCategory').value:'',
        "Title": (form.get('title'))?form.get('title').value:'',
        "Address": address,
        "Details": (form.get('shortDesc'))?form.get('shortDesc').value:'',
        "Brand": (form.get('Brand'))?form.get('Brand').value:'',
        "Price": (form.get('price'))?form.get('price').value:0,
        "YearOfPurchase": (form.get('YearOfPurchase'))?parseInt(form.get('YearOfPurchase').value):0,
        "Status": "",
        "SubmittedDate": cardCreatedDate,
        "IdealFor": (form.get('IdealFor'))?form.get('IdealFor').value:'',
        "Furnished": (form.get('Furnished'))?form.get('Furnished').value:'',
        "FuelType": (form.get('FuelType'))?form.get('FuelType').value:'',
        "KmDriven": (form.get('KmDriven'))?parseInt(form.get('KmDriven').value):0,
        "DimensionWidth": (form.get('DimensionWidth'))?parseInt(form.get('DimensionWidth').value):0,
        "DimensionLength": (form.get('DimensionLength'))?parseInt(form.get('DimensionLength').value):0,
        "DimensionHeight": (form.get('DimensionHeight'))?parseInt(form.get('DimensionHeight').value):0,
        "TypeofUse": "",
        "Type": (form.get('Type'))?form.get('Type').value:'',
        "Negotiable": (form.get('negotiable') && form.get('negotiable')!== '' )?form.get('negotiable').value:false,
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




