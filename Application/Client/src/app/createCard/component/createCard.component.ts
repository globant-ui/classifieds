import { Component, OnInit} from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import {CService} from  '../../_common/services/http.service';
import {mapData} from  '../../mapData/mapData';

import {apiPaths} from  '../../../serverConfig/apiPaths';
import {Http, Headers} from '@angular/http';

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
    
    constructor(private httpService:CService,private apiPath:apiPaths,private data:mapData){
        console.log("constructor createCard");
        this.isActive = '';
        this.isCompleted = [];
        this.endPoints.push("SELECT","UPLOAD","ADD INFO","DONE");
        this.type = 'Cars-Automotive';
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
            location: new FormControl('', [<any>Validators.required])
            
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
       this.isCompleted.push(this.endPoints[2]);
       this.isActive = this.endPoints[3];
       
       console.log(this.isCompleted + '********************' + this.isActive);

       let cardData = this.data.mapCardData(this.selectedCategory,this.myForm);
       if(action == 'create'){
           cardData.IsPublished = true;
       }
       
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
        console.log(this.myForm)
        if(this.myForm.get('title').value!='' && this.myForm.get('price').value!='' && this.myForm.get('shortDesc').value!='' && this.myForm.get('negotiable').value!='' && this.myForm.get('location').value!=''){
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

