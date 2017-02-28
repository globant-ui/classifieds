import { Component,Input,OnInit,AfterViewInit,Renderer,ElementRef } from '@angular/core';
import { DatePipe } from '@angular/common';
import { AppState } from '../../app.service';
import { ActivatedRoute } from '@angular/router';
import {SettingsService} from '../../_common/services/setting.service';
import {CService} from  '../../_common/services/http.service';
import { Http, Response,RequestOptions } from '@angular/http';
import {apiPaths} from  '../../../serverConfig/apiPaths';
import {subscribeOn} from "../../../../node_modules/rxjs/operator/subscribeOn";


let styles = require('../styles/product-info.component.scss').toString();
let tpls = require('../tpls/product-info.html').toString();

@Component({
  selector: 'product-info',
  styles : [ styles ],
  providers:[SettingsService, apiPaths, DatePipe],
  template : tpls
})

export class ProductInfoComponent {
  localState = { value: '' };

  @Input() showProductInfoPage;

  private productId : any;
  private productInfoData: any;
  private productDetails: any;
  public postedDate : any;
  public productSubcategory:any;
  private ProductCategoryData: any;
  public isClicked: boolean = false;
  public type: any;
  public subcategoryData = [];

  private productInfoUrl = 'http://in-it0289/ListingAPI/api/Listings/GetListingById?id=';
  private filterSubCategoryUrl = 'http://in-it0289/MasterDataAPI/api/Category/GetAllFiltersBySubCategory?subCategory=';
 //private productImageUrl = 'http://in-it0054:51868/api/DocumentUpload/Get';
  //private  productImages: any;

  constructor(private _route: ActivatedRoute,
              public _datepipe: DatePipe,
              public appState: AppState,
              public _http:Http,
              private _settingsService: SettingsService,
              private renderer: Renderer,
              private elRef:ElementRef,
              public  _cservice:CService) {

    this._route.params.subscribe(params => {
      this.productId = params['id'];
      console.log(this.productId);
    });
  }

  ngOnInit() {
    this.type = "";

  }

  transformDate(date) {
    this.productInfoData.SubmittedDate=new Date();
    this.postedDate =this._datepipe.transform(this.productInfoData.SubmittedDate, 'yyyy-MM-dd');
    console.log(this.postedDate);
  }


  ngAfterViewInit(){
    this.getProductInfo();
  }

  showSimilarListing(){
    console.log('inside similar listing');
  }

  getProductInfo (){
    this.productDetails = this.productInfoUrl+this.productId;
    this._cservice.observableGetHttp(this.productDetails ,null,false)
      .subscribe((res:Response)=> {
          this.productInfoData = res;
          console.log(this.productInfoData);
          this.type = this.productInfoData.Listing.SubCategory + '-' + this.productInfoData.Listing.ListingCategory;
          this.transformDate(this.productInfoData.SubmittedDate);
        //  this.getProductFilters(this.productInfoData.Listing.SubCategory);
        },
        error => {
          console.log("error in response");
        },
        ()=>{
          console.log("Finally");
        })
  }

  // getProductFilters(subCategory){
  //   let productSubcategoryUrl = this.filterSubCategoryUrl+subCategory;
  //   console.log("---------------------------------------",this.productSubcategory);
  //   this._cservice.observableGetHttp(productSubcategoryUrl, null, false)
  //     .subscribe((res:Response)=> {
  //       this.ProductCategoryData = res;
  //       console.log("********************",this.ProductCategoryData);
  //       let filtersArray = this.ProductCategoryData['Filters'];
  //         let selectedSubcategory ={
  //           filetrKey : '',
  //           fiiletValue : ''
  //         }
  //     console.log(filtersArray[0]['FilterName']);
  //       for(let i=0; i < filtersArray.length;i++){
  //         selectedSubcategory.filetrKey = filtersArray[i]['FilterName'];
  //         selectedSubcategory.fiiletValue = this.productInfoData.Listing[filtersArray[i]['FilterName']];
  //         this.subcategoryData.push(selectedSubcategory);
  //       }
  //       console.log(this.subcategoryData);
  //     },
  //     error =>{
  //     console.log("error in response");
  //     },
  //       () => {
  //     console.log("finally");
  //     })
  //   }

  }
