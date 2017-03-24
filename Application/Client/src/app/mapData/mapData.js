"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
var mapData = (function () {
    function mapData() {
    }
    mapData.prototype.mapCardData = function (selectedCategory, form) {
        console.log(form);
        var today = new Date();
        var locale = "en-us";
        var cardCreatedDate = today.getDate() + '-' + today.toLocaleString(locale, { month: "short" }) + '-' + today.getFullYear();
        //let DimensionHeight =
        var dimensions = (form.get('DimensionLength') && form.get('DimensionHeight') && form.get('DimensionWidth')) ? {
            "Length": form.get('DimensionLength').value,
            "Width": form.get('DimensionWidth').value,
            "Height": form.get('DimensionHeight').value
        } : {};
        var data = {
            "ListingType": (form.get('cardType')) ? form.get('cardType').value : '',
            "ListingCategory": selectedCategory,
            "SubCategory": (form.get('subCategory')) ? form.get('subCategory').value : '',
            "Title": (form.get('title')) ? form.get('title').value : '',
            "Address": "",
            "ContactNo": "",
            "ContactName": "",
            "Configuration": "",
            "Details": (form.get('shortDesc')) ? form.get('shortDesc').value : '',
            "Brand": (form.get('Brand')) ? form.get('Brand').value : '',
            "Price": (form.get('price')) ? form.get('price').value : '',
            "YearOfPurchase": (form.get('Year')) ? form.get('Year').value : '',
            "ExpiryDate": "",
            "Status": "",
            "SubmittedBy": "",
            "SubmittedDate": cardCreatedDate,
            "IdealFor": (form.get('IdealFor')) ? form.get('IdealFor').value : '',
            "Furnished": (form.get('Furnished')) ? form.get('Furnished').value : '',
            "FuelType": (form.get('FuelType')) ? form.get('FuelType').value : '',
            "KmDriven": (form.get('KMDriven')) ? form.get('KMDriven').value : '',
            "YearofMake": 0,
            "Dimensions": dimensions,
            "TypeofUse": "",
            "Type": (form.get('Type')) ? form.get('Type').value : '',
            "Model": "NA",
            "Negotiable": (form.get('negotiable')) ? form.get('negotiable').value : false,
            "IsPublished": false,
            "Photos": []
        };
        return data;
    };
    return mapData;
}());
mapData = __decorate([
    core_1.Injectable()
], mapData);
exports.mapData = mapData;
