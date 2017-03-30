import { Component,Input,OnInit,AfterViewInit,Renderer,ElementRef } from '@angular/core';
import { DatePipe } from '@angular/common';
import { AppState } from '../../app.service';
import { ActivatedRoute,Router } from '@angular/router';
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

export class ProductInfoComponent{
  localState = { value: '' };

  @Input() showProductInfoPage;

  private productId : any;
  private productInfoData: any;
  private productDetails: any;
  public postedDate : any;
  public isClicked: boolean = false;
  public type: any;
  public subcategoryData = [];
  private productInfoUrl = 'http://in-it0289/ListingAPI/api/Listings/GetListingById?id=';

  constructor(private _route: ActivatedRoute,
              public _datepipe: DatePipe,
              public appState: AppState,
              public _http:Http,
              private _router:Router,
              private _settingsService: SettingsService,
              private renderer: Renderer,
              private elRef:ElementRef,
              public  _cservice:CService) {
  }

  ngOnInit() {
    this.type = "";
    this.showSimilarListing();
    this._route.params.subscribe(params => {
      this.productId = params['id'];
      this.getProductInfo();
    });

  }

  transformDate(date) {
    this.productInfoData.SubmittedDate=new Date();
    this.postedDate =this._datepipe.transform(this.productInfoData.SubmittedDate, 'yyyy-MM-dd');
  }

  ngAfterViewInit(){
    this.getProductInfo();
  }

  showSimilarListing(){

    }

  showProductInfo(id){
    this._router.navigate(['productInfo',id]);
  }

  getProductInfo (){
    this.productDetails = this.productInfoUrl+this.productId;
    this._cservice.observableGetHttp(this.productDetails ,null,false)
      .subscribe((res:Response)=> {
          this.productInfoData = res;
          console.log(this.productInfoData);
          this.type = this.productInfoData.Listing.SubCategory + '-' + this.productInfoData.Listing.ListingCategory;
          this.subcategoryData = this.productInfoData.Fields;
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
