import { Component, OnInit} from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import {CService} from  '../../_common/services/http.service';
import {mapData} from  '../../mapData/mapData';

import {apiPaths} from  '../../../serverConfig/apiPaths';
import {Http, Headers} from '@angular/http';
import {CookieService} from 'angular2-cookie/core';

let tpls = require('../tpls/createCard.html').toString();
let styles = require('../styles/createCard.scss').toString();

@Component({
    selector:'create-card',
    template: tpls,
    styles: [styles],
    providers: [apiPaths]
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
    private sessionObj;

    constructor(private httpService:CService,private apiPath:apiPaths,private data:mapData,private _cookieService:CookieService){
        this.isActive = '';
        this.isCompleted = [];
        this.endPoints.push('SELECT','UPLOAD','ADD','INFO','DONE');
        this.type = 'Cars-Automotive';
        this.filters = [];
        this.sessionObj = this._cookieService.getObject('SESSION_PORTAL');
    }

    ngOnInit() {
        // the long way
        this.myForm = new FormGroup({
            cardType: new FormControl('', [<any>Validators.required]),
            category: new FormControl('', [<any>Validators.required]),
            subCategory: new FormControl('', [<any>Validators.required]),
            //cardImages: new FormControl('', [<any>Validators.required]),
            title: new FormControl('', [<any>Validators.required]),
            shortDesc: new FormControl('', [<any>Validators.required]),
            negotiable: new FormControl('', [<any>Validators.required]),
            price: new FormControl('', [<any>Validators.required]),
            location: new FormControl('', [<any>Validators.required]),
            submittedBy: new FormControl('', [])
        });
        this.getCategories();
    }

    getCategories(){
       this.httpService.observableGetHttp(this.apiPath.GET_ALL_CATEGORIES,null,false)
       .subscribe((res)=> {
           console.log(res);
           this.categories = res;
           this.subcategories = this.categories[0].SubCategory;
         },
         error => {
           console.log("error in response");
         },
         ()=>{
           console.log("Finally");
         })
    }

    getFilters(){
       let url = (this.myForm.get('subCategory').value!==undefined) ? this.apiPath.FILTERS + this.myForm.get('subCategory').value:this.apiPath.FILTERS + this.subcategories[0];
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
        this.textBoxes = [];

        let filters = this.filters;
        let removeSaleRent = this.filters.Filters.findIndex(x => x.FilterName==='Sale/Rent');
        this.filters.Filters.splice( removeSaleRent, 1 )[0]
        let year = this.filters.Filters.findIndex(x => x.FilterName=='Year');
        if(year!=-1){
            this.textBoxes.push(this.filters.Filters.splice( year, 1 )[0]);
        }
        let kmDriven = this.filters.Filters.findIndex(x => x.FilterName==='KMDriven');
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
            self.myForm.addControl(element.FilterName,new FormControl('', Validators.required));
        });
        this.textBoxes.forEach(function(element){
            self.myForm.addControl(element.FilterName,new FormControl('', Validators.required));
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


       this.myForm.patchValue({category:this.selectedCategory});
       this.myForm.patchValue({submittedBy:this.sessionObj.useremail});

       let cardData = this.data.mapCardData(this.myForm);
       if(action == 'create'){
           cardData.IsPublished = true;
       }

       this.httpService.observablePostHttp(this.apiPath.CREATE_CARD,cardData,null,false)
       .subscribe((res)=> {
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
    }

    subCategoryUpdated(){
        this.type = this.myForm.get('subCategory').value + '-' + this.selectedCategory;

    }

}

