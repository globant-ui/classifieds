import { Component, OnInit,ViewChild} from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import {CService} from  '../../_common/services/http.service';
import {mapData} from  '../../mapData/mapData';
import { PopUpMessageComponent } from '../../_common/popup/';
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
    private sessionObj;
    private showPopupMessage: boolean = false;
    private showPopupDivMessage: string = '';
    
    constructor(private httpService:CService,private apiPath:apiPaths,private data:mapData,private _cookieService:CookieService){
        console.log("constructor createCard");
        this.isActive = '';
        this.isCompleted = [];
        this.endPoints.push("SELECT","UPLOAD","ADD INFO","DONE");
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
       let url = (this.myForm.get('subCategory').value!=undefined) ? this.apiPath.FILTERS + this.myForm.get('subCategory').value:this.apiPath.FILTERS + this.subcategories[0];
       let that = this;
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
        let year = this.filters.Filters.findIndex(x => x.FilterName=="Year");
        if(year!=-1){
            this.textBoxes.push(this.filters.Filters.splice( year, 1 )[0]);
        }
        let kmDriven = this.filters.Filters.findIndex(x => x.FilterName=="KMDriven");
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
        // console.log(this.myForm);
        let that = this;
        this.filters.Filters.forEach(function(element) {
            that.myForm.addControl(element.FilterName,new FormControl("", Validators.required));
        });
        this.textBoxes.forEach(function(element){
            that.myForm.addControl(element.FilterName,new FormControl("", Validators.required));
        });

        console.log(this.filters)
        console.log(this.textBoxes);
        console.log(this.myForm);

    }

    reloadSubcategories(category){
        this.selectedCategory = category.ListingCategory;
        this.subcategories = category.SubCategory;
        
    }

    fileNameChanged(event){
        // console.log(this.myForm.get('cardType').value);
        // console.log(this.myForm.get('subCategory').value);
        // console.log(this.selectedCategory);
        
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
       this.showPopupMessage = true;
       this.showPopupDivMessage = 'listing';
       console.log(this.isCompleted + '********************' + this.isActive);
       
       this.myForm.patchValue({category:this.selectedCategory});
       this.myForm.patchValue({submittedBy:this.sessionObj.useremail});
       
       let cardData = this.data.mapCardData(this.myForm);
       if(action == 'create'){
           cardData.IsPublished = true;
       }
       
    //    this.httpService.observablePostHttp(this.apiPath.CREATE_CARD,cardData,null,false)
    //    .subscribe((res)=> {
    //        console.log("comes here in result",res);
    //         if(res['_id'].length != 0){
    //             console.log("length in create card for id",res['_id'])
    //             this.showPopupMessage = true;
    //         }
    //      },
    //      error => {
    //        console.log("error in response");
    //        this.showPopupMessage = false;;
    //      },
    //      ()=>{
    //        console.log("Finally");
    //      })
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
    
}

