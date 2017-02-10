import { Component, OnInit} from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import {CService} from  '../../_common/services/http.service';
import {mapData} from  '../../mapData/mapData';

import {apiPaths} from  '../../../serverConfig/apiPaths';

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
    
    constructor(private httpService:CService,private apiPath:apiPaths,private data:mapData){
        console.log("comes here in create listing component");
    }

    ngOnInit() {
        // the long way
        this.myForm = new FormGroup({
            cardType: new FormControl('', [<any>Validators.required]),
            //category: new FormControl('', [<any>Validators.required]),
            subCategory: new FormControl('', [<any>Validators.required]),
            //cardImages: new FormControl('', [<any>Validators.required]),
            title: new FormControl('', [<any>Validators.required]),
            shortDesc: new FormControl('', [<any>Validators.required]),
            negotiable: new FormControl('', [<any>Validators.required])
            // price: new FormControl('', [<any>Validators.required]),
            // location: new FormControl('', [<any>Validators.required])
            
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
        console.log(category)
        this.selectedCategory = category.name;
        this.subcategories = category.SubCategory;

    }

    fileNameChanged(event){
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
       this.myForm._value.selectedCategory = this.selectedCategory;
       let cardData = this.data.mapCardData(this.myForm);
       if(action == 'create'){
           cardData.IsPublished = true;
       }
       console.log(cardData)
       this.httpService.observablePostHttp(this.apiPath.CREATE_CARD,cardData,null,false)
       .subscribe((res)=> {
           console.log("comes here in result",res);
        //    this.categories = res;
        //    this.subcategories = this.categories[0].SubCategory;
         },
         error => {
           console.log("error in response");
         },
         ()=>{
           console.log("Finally");
         })
        this.submitted = true;
        
    }
    
}

