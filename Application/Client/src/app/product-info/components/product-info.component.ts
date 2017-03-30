import { Component,Input,OnInit,AfterViewInit,Renderer,ElementRef } from '@angular/core';
import { DatePipe } from '@angular/common';
import { AppState } from '../../app.service';
import { ActivatedRoute,Router } from '@angular/router';
import {SettingsService} from '../../_common/services/setting.service';
import {CService} from  '../../_common/services/http.service';
import { Http, Response,RequestOptions } from '@angular/http';
import {apiPaths} from  '../../../serverConfig/apiPaths';
import {subscribeOn} from "../../../../node_modules/rxjs/operator/subscribeOn";
import {CookieService} from 'angular2-cookie/core';

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
  private GetUserWishList: string = '';
  private emailId:string = "";
  private isInWhishList:boolean = false;
  private wishListData:any[] = [];

  constructor(private _route: ActivatedRoute,
              public _datepipe: DatePipe,
              public appState: AppState,
              public _http:Http,
              private _router:Router,
              private _settingsService: SettingsService,
              private renderer: Renderer,
              private _cookieService: CookieService,
              private elRef:ElementRef,
              public  _cservice:CService) {
              this.GetUserWishList = _settingsService.getPath('GetUserWishList');
  }

  ngOnInit() {
    this.type = "";
    this.emailId = this._cookieService.getObject('SESSION_PORTAL')["useremail"];
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
          this.isClicked = false;
          this.productInfoData = res;
          this.GetWishList();
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

  //get wishlist api
   GetWishList() {
       let self = this;
       if(this.emailId && this.emailId != ""){
          this._cservice.observableGetHttp(this.GetUserWishList+this.emailId, null, false)
           .subscribe((res: Response) => {
                console.log("this.GetUserWishList Response", res);
                self.wishListData = res;
                self.isInWhishList  = self.wishListData.indexOf(self.productInfoData.Listing._id) === -1 ? false : true;
           },
           error => {
               console.log("error in response", error);
           });
       }
   }
  
  showContact(){
    this.isClicked = true;
  }
}
