import {Injectable} from '@angular/core';

@Injectable()
export class MapData{

  constructor() {
  }

  mapCardData(form,publishStatus = false) {
      let today = new Date();
      let locale = "en-us";
      let cardCreatedDate = today.getDate() + '-' + today.toLocaleString(locale, { month: "short" })  + '-' + today.getFullYear();
//       let dimensions = (form.get('DimensionLength') && form.get('DimensionHeight') && form.get('DimensionWidth'))? {
//          "Length": form.get('DimensionLength').value,
//          "Width": form.get('DimensionWidth').value,
//          "Height": form.get('DimensionHeight').value
//       }:{};

      let address = (form.value['area'] && form.value['city'] && form.value['state'] && form.value['country']) ? form.value['area'] + '-' + form.value['city'] + '-' + form.value['state'] + '-' + form.value['country'] : '';
      debugger;
      let data = {

        "ListingType": (form.value['cardType'])?form.value['cardType']:'',
        "ListingCategory": (form.value['category'])?form.value['category']:'',
        "SubCategory": (form.value['subCategory'])?form.value['subCategory']:'',
        "Title": (form.value['title'])?form.value['title']:'',
        "Address": address,
        "Details": (form.value['shortDesc'])?form.value['shortDesc']:'',
        "Brand": (form.value['Brand'])?form.value['Brand']:'',
        "Price": (form.value['price'])?form.value['price']:0,
        "YearOfPurchase": (form.value['YearOfPurchase'])?parseInt(form.value['YearOfPurchase']):0,
        "Status": "",
        "SubmittedDate": cardCreatedDate,
        "IdealFor": (form.value['IdealFor'])?form.value['IdealFor']:'',
        "Furnished": (form.value['Furnished'])?form.value['Furnished']:'',
        "FuelType": (form.value['FuelType'])?form.value['FuelType']:'',
        "KmDriven": (form.value['KmDriven'])?parseInt(form.value['KmDriven']):0,
        "DimensionWidth": (form.value['DimensionWidth'])?parseInt(form.value['DimensionWidth']):0,
        "DimensionLength": (form.value['DimensionLength'])?parseInt(form.value['DimensionLength']):0,
        "DimensionHeight": (form.value['DimensionHeight'])?parseInt(form.value['DimensionHeight']):0,
        "TypeofUse": "",
        "Type": (form.value['Type'])?form.value['Type']:'',
        "Negotiable": (form.value['negotiable'] && form.value['negotiable'] !== '' )?form.value['negotiable']:false,
        "IsPublished": publishStatus ,
        "Photos": [],
        "SubmittedBy": (form.value['submittedBy'])?form.value['submittedBy']:false,
        "Rooms": (form.value['Rooms'])?form.value['Rooms']:'',
        "State": (form.value['state'])?form.value['state']:'',
        "Country": (form.value['country'])?form.value['country']:'',
        "City": (form.value['city'])?form.value['city']:''
      };
      return data;

  }

}




