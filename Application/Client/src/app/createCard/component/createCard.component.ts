import { Component, OnInit,ViewChild} from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import {CService} from  '../../_common/services/http.service';
import {mapData} from  '../../mapData/mapData';
import { PopUpMessageComponent } from '../../_common/popup/';
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
    
    @ViewChild('PopUpMessageComponent') popUpMessageComponent;

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
    private showPopupMessage: boolean = false;
    private showPopupDivMessage: string = '';
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
        var self = this;

        this._route.params.subscribe(params => {
            self.productId = params['id'];
            if(self.productId !== undefined){
                self.action = 'Edit';
                self.getProductData(self.productId);
            }
        });
        this.isActive = '';
        this.isCompleted = [];
        this.endPoints.push('SELECT','UPLOAD','ADD','INFO','DONE');
        this.type = 'Car-Automotive';
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
        submittedBy: new FormControl('', [])
      });
        this.getCategories();
    }

    getCategories(){
      var self = this;
       this.httpService.observableGetHttp(this.apiPath.GET_ALL_CATEGORIES,null,false)
       .subscribe((res)=> {

           self.categories = res;
           self.subcategories = self.categories[0].SubCategory;

         },
         error => {
           console.log("error in response");
         },
         ()=>{
           console.log("Finally");
         })
    }

    getFilters(){
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
       this.isCompleted.push(this.endPoints[2]);
       this.isActive = this.endPoints[3];
       console.log(this.isCompleted + '********************' + this.isActive);
       this.myForm.patchValue({category:this.selectedCategory});
       this.myForm.patchValue({submittedBy:this.sessionObj.useremail});

       let cardData = this.data.mapCardData(this.myForm);
       if(action === 'create'){
           cardData.IsPublished = true;
       }

       this.uploadedImages.forEach(function(value,key){
           let imageDetails = {};
           imageDetails['ImageName'] = 'Photo'+key;
           imageDetails['Image'] =  value;
           cardData.Photos.push(imageDetails);
       });
       this.httpService.observablePostHttp(this.apiPath.CREATE_CARD,cardData,null,false)
       .subscribe((res)=> {
            if(res['_id'].length != 0){
                 console.log("length in create card for id",res['_id'])
                 this.showPopupMessage = true;
                 this.showPopupDivMessage = 'listing';
            }
         },
         error => {
             this.showPopupMessage = false;
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
    }

    subCategoryUpdated(){
        this.type = this.myForm.get('subCategory').value + '-' + this.selectedCategory;
        this.getFilters();
    }

    getProductData(productId){
        let productInfoUrl = this._settingsService.getPath('productInfoUrl')+productId;
        let self = this;
        this.httpService.observableGetHttp(productInfoUrl,null,false)
        .subscribe((res)=> {
            self.productInfo = res;
            console.log(self.productInfo);
            self.myForm.patchValue({cardType:self.productInfo['Listing'].ListingType});
            self.myForm.patchValue({category:self.productInfo['Listing'].ListingCategory});
            self.myForm.patchValue({subCategory:self.productInfo['Listing'].SubCategory});
            self.myForm.patchValue({title:self.productInfo['Listing'].Title});
            self.myForm.patchValue({location:self.productInfo['Listing'].City});
            self.myForm.patchValue({shortDesc:self.productInfo['Listing'].Details});
            self.myForm.patchValue({price:self.productInfo['Listing'].Price});
            self.myForm.patchValue({area:self.productInfo['Listing'].Address.split("-")[0]});
            self.myForm.patchValue({city:self.productInfo['Listing'].City});
            self.myForm.patchValue({state:self.productInfo['Listing'].State});
            self.myForm.patchValue({country:self.productInfo['Listing'].Country});
            self.myForm.patchValue({negotiable:self.productInfo['Listing'].Negotiable});

            self.getFilters();

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

