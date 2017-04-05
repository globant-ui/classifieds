import { Component, OnInit,ViewChild} from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import {CService} from  '../../_common/services/http.service';
import {MapData} from  '../../mapData/mapData';
import { PopUpMessageComponent } from '../../_common/popup/';
import { ActivatedRoute } from '@angular/router';
import {SettingsService} from '../../_common/services/setting.service';
import {apiPaths} from  '../../../serverConfig/apiPaths';
import {Http, Headers} from '@angular/http';
import {CookieService} from 'angular2-cookie/core';

let tpls = require('../tpls/createCard.html').toString();
let styles = require('../styles/createCard.scss').toString();

const URL = 'http://in-it0289/ListingAPI/api/Listings/PostListing';

@Component({
    selector:'create-card',
    template: tpls,
    styles: [styles],
    providers: [apiPaths,SettingsService]
})
export class CreateCardComponent implements OnInit {

    @ViewChild('PopUpMessageComponent') popUpMessageComponent;

    public myForm: FormGroup;
    public submitted: boolean;
    public selectedCategory: string = 'Automotive';
    public categories;
    public subcategories;
    public uploadedImages = [];
    private uploadedImageData = [];
    public endPoints = [];
    public isActive: string = '';
    public isCompleted = [];
    public type: string = '';
    public objDynamicData = {}
    public currentSubCategory: string = '';
    public filters;
    public textBoxes = [];
    private showPopupMessage: boolean = false;
    private showPopupDivMessage: string = '';
    public productId: string;
    public action: string = 'Create';
    public productInfo = {};
    public sessionObj;
    private photos = [];
    private existingImageCount = 0;

    constructor(private httpService:CService,
                private apiPath:apiPaths,
                private data:MapData,
                private _route: ActivatedRoute,
                private _settingsService: SettingsService,
                private _cookieService:CookieService
                ){
        var self = this;

        this._route.params.subscribe(params => {
            self.productId = params['id'];
            if(self.productId !== undefined){
                self.action = 'Edit';
                self.getCategories(true);
            }
        });
        this.isActive = '';
        this.isCompleted = [];
        this.endPoints.push('SELECT','UPLOAD','ADD','INFO','DONE');
        this.type = 'Car-Automotive';
        this.currentSubCategory = 'Car';
        this.filters = [];
        this.sessionObj = this._cookieService.getObject('SESSION_PORTAL');
    }

    ngOnInit() {
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
        submittedBy: new FormControl('', []),
        file: new FormControl('', [])
      });
        this.objDynamicData = {};
        this.getCategories();
    }

    getCategories(p_editMode = false){
      var self = this;
       this.httpService.observableGetHttp(this.apiPath.GET_ALL_CATEGORIES,null,false)
       .subscribe((res)=> {

           self.categories = res;
           self.subcategories = self.categories[0].SubCategory;
           if(p_editMode){
               self.getProductData(self.productId);
           }
         },
         error => {
           console.log("error in response");
         },
         ()=>{
           console.log("Finally");
         })
    }

    getFilters(){
       if(this.subcategories && this.subcategories.length > 0){
        let url = (this.myForm.get('subCategory').value!=undefined && this.myForm.get('subCategory').value!='') ? this.apiPath.FILTERS + this.myForm.get('subCategory').value:this.apiPath.FILTERS + this.subcategories[0];
        let self = this;
        this.httpService.observableGetHttp(url,null,false)
        .subscribe((res)=> {
            self.filters = res;
            self.loadFilters();
            },
            error => {
            console.log("error in response");
            },
            ()=>{
            console.log("Finally");
            })
        }
    }

    loadFilters(){
        this.textBoxes = []
        let filters = this.filters;
        let removeSaleRent = this.filters.Filters.findIndex(x => x.FilterName==='Sale/Rent');
        this.filters.Filters.splice( removeSaleRent, 1 )[0]
        let year = this.filters.Filters.findIndex(x => x.FilterName==='YearOfPurchase');
        if(year!=-1){
            this.textBoxes.push(this.filters.Filters.splice( year, 1 )[0]);
        }
        let kmDriven = this.filters.Filters.findIndex(x => x.FilterName==='KmDriven');
        if(kmDriven!=-1){
            this.textBoxes.push(this.filters.Filters.splice( kmDriven, 1 )[0]);
        }
        //for dimensions
        let dimensionLength = this.filters.Filters.findIndex(x => x.FilterName==='DimensionLength');
        if(dimensionLength!=-1){
            this.textBoxes.push(this.filters.Filters.splice( dimensionLength, 1 )[0]);
        }
        let dimensionHeight = this.filters.Filters.findIndex(x => x.FilterName==='DimensionHeight');
        if(dimensionHeight!=-1){
            this.textBoxes.push(this.filters.Filters.splice( dimensionHeight, 1 )[0]);
        }
        let dimensionWidth = this.filters.Filters.findIndex(x => x.FilterName==='DimensionWidth');
        if(dimensionWidth!=-1){
            this.textBoxes.push(this.filters.Filters.splice( dimensionWidth, 1 )[0]);
        }
        let self = this;
        this.filters.Filters.forEach(function(element) {
            self.myForm.addControl(element.FilterName,new FormControl("", Validators.required));
        });
        this.textBoxes.forEach(function(element){
            self.myForm.addControl(element.FilterName,new FormControl("", Validators.required));
        });
        self.setFieldValue();
    }

    reloadSubcategories(category,subCategory = ''){
        this.selectedCategory = category.ListingCategory;
        this.subcategories = category.SubCategory;
        this.currentSubCategory = subCategory != '' ? subCategory : this.subcategories[0];
        this.type = this.currentSubCategory + '-' + this.selectedCategory;
        this.getFilters();
    }

    fileNameChanged(event){
        if(this.myForm.get('cardType').value!='' && this.myForm.get('subCategory').value!='' && this.selectedCategory!=''){
            this.isCompleted.push(this.endPoints[0]);
        }
        this.isActive = this.endPoints[1];
        if(this.uploadedImages.length<4){
            if (event.target.files && event.target.files[0]) {
                this.uploadedImageData.push(event.target.files[0]);
                var reader = new FileReader();
                reader.onload = (event:any) => {
                    this.uploadedImages.push(event.target.result);
                }
                reader.readAsDataURL(event.target.files[0]);
            }
        }
    }

    createCard(action){
       this.isCompleted.push(this.endPoints[2]);
       this.isActive = this.endPoints[3];
       console.log(this.isCompleted + '********************' + this.isActive);
       this.myForm.patchValue({category:this.selectedCategory});
       this.myForm.patchValue({submittedBy:this.sessionObj.useremail});
       let cardData = this.data.mapCardData(this.myForm);
       if(action === 'create'){
           cardData.IsPublished = true;
       }
      var data = new FormData();
      data.append("listing", JSON.stringify(cardData));
        for (let i = 0; i < this.uploadedImageData.length; i++) {
            data.append("file", this.uploadedImageData[i]);
        }
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "http://in-it0289/ListingAPI/api/Listings/PostListing");
        xhr.setRequestHeader("accesstoken", "c4fd7b85796f4d05b12504fbf1c42a3e");
        xhr.setRequestHeader("useremail", "avadhut.lakule@globant.com");
        xhr.send(data);
        this.showPopupDivMessage="listing";
        this.showPopupMessage = true;
        this.submitted = true;
    }

    isAddInfoCompleted() {
        if(this.myForm.get('title').value !== '' && this.myForm.get('price').value !== '' && this.myForm.get('shortDesc').value !== '' && this.myForm.get('location').value !== ''){
            this.isCompleted.push(this.endPoints[1]);
        }
        this.isActive = this.endPoints[2];
    }

    subCategoryUpdated(){
        this.type = this.myForm.get('subCategory').value + '-' + this.selectedCategory;
        this.currentSubCategory = this.myForm.get('subCategory').value;
        console.log('type:'+this.type);
        console.log('currentSubCategory:'+ this.currentSubCategory);
        this.getFilters();
    }

    getProductData(productId){
        let productInfoUrl = this._settingsService.getPath('productInfoUrl')+productId;
        console.log("Info URL" ,productInfoUrl);
        let self = this;
        this.httpService.observableGetHttp(productInfoUrl,null,false)
        .subscribe((res)=> {
            self.productInfo = res;
            console.log(res);
            console.log("productInfo:");
            console.log(self.productInfo);
            if(self.productInfo['Listing']){
              self.myForm.patchValue({cardType:self.productInfo['Listing'].ListingType});
              //self.myForm.patchValue({category:self.productInfo['Listing'].ListingCategory});
              //self.myForm.patchValue({subCategory:self.productInfo['Listing'].SubCategory});
              self.myForm.patchValue({title:self.productInfo['Listing'].Title});
              self.myForm.patchValue({location:self.productInfo['Listing'].City});
              self.myForm.patchValue({shortDesc:self.productInfo['Listing'].Details});
              self.myForm.patchValue({price:self.productInfo['Listing'].Price});
              self.myForm.patchValue({area:self.productInfo['Listing'].Address.split("-")[0]});
              self.myForm.patchValue({city:self.productInfo['Listing'].City});
              self.myForm.patchValue({country:self.productInfo['Listing'].Country});
              self.myForm.patchValue({negotiable:self.productInfo['Listing'].Negotiable});
              self.photos = self.productInfo['Listing']['Photos'];
              self.existingImageCount = self.photos.length;
              let categoryIndex;
              if(self.categories){
                 categoryIndex = self.categories.findIndex(function(o){
                    return o.ListingCategory === self.productInfo['Listing'].ListingCategory;
                });
                self.selectedCategory = self.categories[categoryIndex];
              }
              else{
                  console.log("No Category Found !!!")
              }
              self.currentSubCategory = self.productInfo['Listing'].SubCategory;
              //self.getFilters();
              self.reloadSubcategories(self.selectedCategory,self.currentSubCategory);

              console.log('productInfo');
              console.log(self.productInfo);
            }


            },
            error => {
            console.log("error in response");
            },
            ()=>{
            console.log("Finally");
            })
    }

    setFieldValue(){
        if(this.productInfo){
            if(this.productInfo['Fields']){
                for(let i = 0 ; i < this.productInfo['Fields'].length; i++){
                    this.objDynamicData[this.productInfo['Fields'][i].FieldName] = this.productInfo['Fields'][i].FieldValue;
                }
            }
        }
    }

    updateCard(){
        let cardData = this.data.mapCardData(this.myForm,this.productInfo['Listing'].IsPublished);
        cardData["_id"] = this.productId;
        cardData["ListingCategory"] = this.selectedCategory;
        cardData['Photos'] = this.photos;
        let url = this.apiPath.UPDATE_CARD;
        var data = new FormData();
        data.append("listing", JSON.stringify(cardData));  //json object
        for (let i = 0; i < this.uploadedImageData.length; i++) {
            data.append("file", this.uploadedImageData[i]);   //image file object
        }
        var xhr = new XMLHttpRequest();
        xhr.open("PUT", url);
        xhr.setRequestHeader("accesstoken", "c4fd7b85796f4d05b12504fbf1c42a3e");
        xhr.setRequestHeader("useremail", "avadhut.lakule@globant.com");
        xhr.send(data);
    }
}

