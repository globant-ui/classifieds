import { Component, OnInit} from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import {CService} from  '../../_common/services/http.service';
import {mapData} from  '../../mapData/mapData';
import { ActivatedRoute } from '@angular/router';
import {SettingsService} from '../../_common/services/setting.service';

import {apiPaths} from  '../../../serverConfig/apiPaths';
import {Http, Headers} from '@angular/http';
import {CookieService} from 'angular2-cookie/core';

let tpls = require('../tpls/createCard.html').toString();
let styles = require('../styles/createCard.scss').toString();

@Component({
    selector:'create-card',
    template: tpls,
    styles: [styles],
    providers: [apiPaths,SettingsService]
})
export class CreateCardComponent implements OnInit {
    public myForm: FormGroup;
    public submitted: boolean;
    public selectedCategory: string = 'Automotive';
    public categories;
    public subcategories;
    public uploadedImages = [];
    public endPoints = [];
    public isActive: string = '';
    public isCompleted = [];
    public type: string = '';
    public filters;
    public textBoxes = [];

    public productId: string;
    public action: string = 'Create';
    public productInfo = {};
    public sessionObj;

    constructor(private httpService:CService,
                private apiPath:apiPaths,
                private data:mapData,
                private _route: ActivatedRoute,
                private _settingsService: SettingsService,
                private _cookieService:CookieService
                ){
        this.myForm = new FormGroup({
            cardType: new FormControl('', [<any>Validators.required]),
            category: new FormControl('', [<any>Validators.required]),
            subCategory: new FormControl('', [<any>Validators.required]),
            title: new FormControl('', [<any>Validators.required]),
            area: new FormControl('', [<any>Validators.required]),
            city: new FormControl('', [<any>Validators.required]),
            state: new FormControl('', [<any>Validators.required]),
            country: new FormControl('', [<any>Validators.required]),
            shortDesc: new FormControl('', [<any>Validators.required]),
            negotiable: new FormControl('', [<any>Validators.required]),
            price: new FormControl('', [<any>Validators.required]),
            location: new FormControl('', [<any>Validators.required]),
            submittedBy: new FormControl('', [])
        });
        var that = this;

        this._route.params.subscribe(params => {
            that.productId = params['id'];
            if(that.productId !== undefined){
                console.log("comes here anyway");
                that.action = 'Edit';
                that.getProductData(that.productId);
            }
        });
        this.isActive = '';
        this.isCompleted = [];
        this.endPoints.push("SELECT","UPLOAD","ADD INFO","DONE");
        this.type = 'Car-Automotive';
        this.filters = [];
        this.sessionObj = this._cookieService.getObject('SESSION_PORTAL');

    }

    ngOnInit() {

        this.getCategories();
    }

    getCategories(){
      var that = this;
       this.httpService.observableGetHttp(this.apiPath.GET_ALL_CATEGORIES,null,false)
       .subscribe((res)=> {
           console.log(res);

          // this.categories = res;

           that.subcategories = res[0].SubCategory;
           debugger;
         },
         error => {
           console.log("error in response");
         },
         ()=>{
           console.log("Finally");
         })
    }

    getFilters(){

       let url = (this.myForm.get('subCategory').value!=undefined) ? this.apiPath.FILTERS + this.myForm.get('subCategory').value:this.apiPath.FILTERS + this.subcategories[0];
       let that = this;
      debugger;
       this.httpService.observableGetHttp(url,null,false)
       .subscribe((res)=> {
           that.filters = res;

           that.loadFilters();
         },
         error => {
           console.log("error in response");
         },
         ()=>{
           console.log("Finally");
         })
    }

    loadFilters(){
        this.textBoxes = [];
        console.log(this.filters)

        let filters = this.filters;
        console.log("filterss",filters);
        let removeSaleRent = this.filters.Filters.findIndex(x => x.FilterName=="Sale/Rent");
        this.filters.Filters.splice( removeSaleRent, 1 )[0]
        let year = this.filters.Filters.findIndex(x => x.FilterName=="YearOfPurchase");
        if(year!=-1){
            this.textBoxes.push(this.filters.Filters.splice( year, 1 )[0]);
        }
        let kmDriven = this.filters.Filters.findIndex(x => x.FilterName=="KmDriven");
        if(kmDriven!=-1){
            this.textBoxes.push(this.filters.Filters.splice( kmDriven, 1 )[0]);
        }
        //for dimensions
        let dimensionLength = this.filters.Filters.findIndex(x => x.FilterName=="DimensionLength");
        if(dimensionLength!=-1){
            this.textBoxes.push(this.filters.Filters.splice( dimensionLength, 1 )[0]);
        }
        let dimensionHeight = this.filters.Filters.findIndex(x => x.FilterName=="DimensionHeight");
        if(dimensionHeight!=-1){
            this.textBoxes.push(this.filters.Filters.splice( dimensionHeight, 1 )[0]);
        }
        let dimensionWidth = this.filters.Filters.findIndex(x => x.FilterName=="DimensionWidth");
        if(dimensionWidth!=-1){
            this.textBoxes.push(this.filters.Filters.splice( dimensionWidth, 1 )[0]);
        }
        let that = this;
        this.filters.Filters.forEach(function(element) {
            that.myForm.addControl(element.FilterName,new FormControl("", Validators.required));
        });
        this.textBoxes.forEach(function(element){
            that.myForm.addControl(element.FilterName,new FormControl("", Validators.required));
        });

        if(this.action === 'Edit'){

        }

    }

    reloadSubcategories(category){
        this.selectedCategory = category.ListingCategory;
        this.subcategories = category.SubCategory;
    }

    fileNameChanged(event){

        if(this.myForm.get('cardType').value!='' && this.myForm.get('subCategory').value!='' && this.selectedCategory!=''){
            this.isCompleted.push(this.endPoints[0]);
        }
        this.isActive = this.endPoints[1];

        console.log(this.isCompleted + 'this is completed nd active' + this.isActive);

        if(this.uploadedImages.length<4){
            if (event.target.files && event.target.files[0]) {
            var reader = new FileReader();

            reader.onload = (event:any) => {
            this.uploadedImages.push(event.target.result);
            }

            reader.readAsDataURL(event.target.files[0]);
        }
    }

    }

    createCard(action){
       console.log(this.myForm)
       this.isCompleted.push(this.endPoints[2]);
       this.isActive = this.endPoints[3];

       console.log(this.isCompleted + '********************' + this.isActive);


       this.myForm.patchValue({category:this.selectedCategory});
       this.myForm.patchValue({submittedBy:this.sessionObj.useremail});

       let cardData = this.data.mapCardData(this.myForm);
       if(action == 'create'){
           cardData.IsPublished = true;
       }
       this.uploadedImages.forEach(function(value,key){
           let imageDetails = {};
           imageDetails["ImageName"] = "Photo"+key;
           imageDetails["Image"] =  value;
           cardData.Photos.push(imageDetails);
       });

       this.httpService.observablePostHttp(this.apiPath.CREATE_CARD,cardData,null,false)
       .subscribe((res)=> {
           console.log("comes here in result",res);
         },
         error => {
           console.log("error in response");

         },
         ()=>{
           console.log("Finally");

         })

        this.submitted = true;

    }

    isAddInfoCompleted() {
        if(this.myForm.get('title').value !== '' && this.myForm.get('price').value !== '' && this.myForm.get('shortDesc').value !== '' && this.myForm.get('location').value !== ''){
            this.isCompleted.push(this.endPoints[1]);
        }
        this.isActive = this.endPoints[2];
        console.log(this.isCompleted + '--------' + this.isActive);
    }

    subCategoryUpdated(){
        console.log("comes here when subcategory updated")
        this.type = this.myForm.get('subCategory').value + '-' + this.selectedCategory;

    }

    getProductData(productId){
        let productInfoUrl = this._settingsService.getPath('productInfoUrl')+productId;
        let that = this;
        this.httpService.observableGetHttp(productInfoUrl,null,false)
        .subscribe((res)=> {
            that.productInfo = res;
            console.log(that.productInfo);
            that.myForm.patchValue({cardType:that.productInfo["Listing"].ListingType});
            that.myForm.patchValue({category:that.productInfo["Listing"].ListingCategory});
            that.myForm.patchValue({subCategory:that.productInfo["Listing"].SubCategory});
            that.myForm.patchValue({title:that.productInfo["Listing"].Title});
            that.myForm.patchValue({location:that.productInfo["Listing"].City});
            that.myForm.patchValue({shortDesc:that.productInfo["Listing"].Details});
            that.myForm.patchValue({price:that.productInfo["Listing"].Price});
            that.myForm.patchValue({area:that.productInfo["Listing"].Address.split("-")[0]});
            that.myForm.patchValue({city:that.productInfo["Listing"].City});
            that.myForm.patchValue({state:that.productInfo["Listing"].State});
            that.myForm.patchValue({country:that.productInfo["Listing"].Country});
            that.myForm.patchValue({negotiable:that.productInfo["Listing"].Negotiable});

            that.getFilters();

            },
            error => {
            console.log("error in response");
            },
            ()=>{
            console.log("Finally");
            })
    }

    updateCard(){
        let cardData = this.data.mapCardData(this.myForm);
        cardData["_id"] = this.productId;
        let url = this.apiPath.UPDATE_CARD + this.productId;
        this.httpService.observablePutHttp(url,cardData,null,false)
       .subscribe((res)=> {
           console.log("comes here in result",res);
         },
         error => {
           console.log("error in response");
         },
         ()=>{
           console.log("Finally");
         });
    }

}

