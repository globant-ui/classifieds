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
  template : tpls,
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
  public subcategoryData = [];
  private productInfoUrl = 'http://in-it0289/ListingAPI/api/Listings/GetListingById?id=';

  private wishListPostUrl:string = '';
  private wishListCondtionalUrl:string = '';
  private emailId:string = '';
  private filterCategoryUrl:string = '';
  private GetUserWishList: string = '';
  private DeleteUserWishListUrl: string = '';
  private WishListSelectedData : any;
  private isInWishList:any;

  constructor(private _route: ActivatedRoute,
              public _datepipe: DatePipe,
              public appState: AppState,
              public _http:Http,
              private _router:Router,
              private _settingsService: SettingsService,
              private renderer: Renderer,
              private elRef:ElementRef,
              public  _cservice:CService,
              private _cookieService: CookieService,) {

    this._route.params.subscribe(params => {
      this.productId = params['id'];

      this.wishListPostUrl = _settingsService.getPath('wishListPostUrl');
      this.filterCategoryUrl = _settingsService.getPath('filterCategoryUrl');
      this.GetUserWishList = _settingsService.getPath('GetUserWishList');
      this.DeleteUserWishListUrl = _settingsService.getPath('DeleteUserWishListUrl');
    });
  }

  ngOnInit() {
    this.emailId = this._cookieService.getObject('SESSION_PORTAL')["useremail"];
    this.GetUserWishList = this.GetUserWishList + this.emailId;
    this.type = "";
    this.showSimilarListing();
    this.GetWishList(); 
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
 favorite(card,id) {
   
   let index = this.WishListSelectedData.findIndex(function (o) {
     return o === id;
   })
   this.isInWishList = index > -1 ? true : false;

       if (this.WishListSelectedData.length <= 4) {
         this.wishListCondtionalUrl = (this.isInWishList ? this.DeleteUserWishListUrl : this.wishListPostUrl) + (this.emailId + '&listingId=' + id);
         var operation = this.isInWishList ? 'observableDeleteHttp' : 'observablePostHttp';
         this._cservice[operation](this.wishListCondtionalUrl, this.isInWishList ? null : card, this.isInWishList ? false : null, false)
           .subscribe((res: Response) => {
             console.log("product data wishlist", res)
             //  this.card[i]['isInWishList'] = !this.card[i]['isInWishList'];
             this.GetWishList();
           },
           error => {
             console.log("error in response", error);
           });
       }
       else{
           alert("Only 20 Wishlist is allowed !!!");
       }

   }

//get wishlist api
   GetWishList() {
       this._cservice.observableGetHttp(this.GetUserWishList, null, false)
           .subscribe((res: Response) => {
               console.log("this.GetUserWishList Response", res);
               /**/
                this.updateCards(res);
                this.WishListSelectedData = res;
           },
           error => {
               console.log("error in response", error);
           });
   }

//update the cards
   updateCards(wishListData) {
     console.log("wishListData",wishListData)
      //  if ( this.cards && this.cards.length != 0 ) {
      //      for ( let i = 0; i < this.cards.length; i++ ) {
      //          this.cards[i]['isInWishList'] = wishListData.indexOf(this.cards[i]['_id']) > -1;
      //      }
      //  }
   }
 }
  
