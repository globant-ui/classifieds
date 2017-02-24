import { Component,Input,OnInit,AfterViewInit,Renderer,ElementRef } from '@angular/core';
import { DatePipe } from '@angular/common';
import { AppState } from '../../app.service';
import { ActivatedRoute } from '@angular/router';
import {SettingsService} from '../../_common/services/setting.service';
import {CService} from  '../../_common/services/http.service';
import { Http, Response,RequestOptions } from '@angular/http';
import {apiPaths} from  '../../../serverConfig/apiPaths';


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
  public isClicked: boolean = false;
  public type: any;
  private productInfoUrl = 'http://in-it0289/ListingAPI/api/Listings/GetListingById?id=';

  constructor(private _route: ActivatedRoute,
              public _datepipe: DatePipe,
              public appState: AppState,
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
   // console.log(this.productInfoData.Listing);
  //  this.type = this.productInfoData.Listing.SubCategory + '-' + this.productInfoData.Listing.ListingCategory;
  }

  showSimilarListing(){
    console.log('inside similar listing');
  }

  getProductInfo (){
    this.productDetails = this.productInfoUrl+this.productId;
    this._cservice.observableGetHttp(this.productDetails ,null,false)
      .subscribe((res:Response)=> {
          this.productInfoData = res;
          console.log(this.productInfoData.Listing);
          this.type = this.productInfoData.Listing.SubCategory + '-' + this.productInfoData.Listing.ListingCategory;
          console.log(this.productInfoData);
          this.transformDate(this.productInfoData.SubmittedDate);
        },
        error => {
          console.log("error in response");
        },
        ()=>{
          console.log("Finally");
        })
  }


}
