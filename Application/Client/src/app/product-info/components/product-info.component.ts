import { Component, Input, OnInit, AfterViewInit, Renderer, ElementRef } from '@angular/core';
import { DatePipe } from '@angular/common';
import { AppState } from '../../app.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SettingsService } from '../../_common/services/setting.service';
import { CService } from  '../../_common/services/http.service';
import { Broadcaster } from  '../../_common/services/broadcast.service';
import { Http, Response, RequestOptions } from '@angular/http';
import { ApiPaths } from  '../../../serverConfig/apiPaths';
import { subscribeOn } from '../../../../node_modules/rxjs/operator/subscribeOn';
import { CookieService } from 'angular2-cookie/core';

@Component({
  selector: 'product-info',
  styles : [ require('../styles/product-info.component.scss').toString() ],
  providers: [SettingsService, ApiPaths, DatePipe],
  template : require('../tpls/product-info.html').toString()
})

export class ProductInfoComponent implements AfterViewInit, OnInit {
  private localState = { value: '' };
  @Input() private showProductInfoPage;
  private productId: any;
  private productInfoData: any;
  private productDetails: any;
  private postedDate: any;
  private isClicked: boolean = false;
  private type: any;
  private subcategoryData = [];
  private productInfoUrl: string = 'http://in-it0289/ListingAPI/api/Listings/GetListingById?id=';
  private GetUserWishList: string = '';
  private wishListPostUrl: string = '';
  private DeleteUserWishListUrl: string = '';
  private emailId: string = '';
  private isInWhishList: boolean = false;
  private wishListData: any = [];

  constructor(
    private _route: ActivatedRoute,
    private _datepipe: DatePipe,
    private appState: AppState,
    private _http: Http,
    private _router: Router,
    private _settingsService: SettingsService,
    private renderer: Renderer,
    private _cookieService: CookieService,
    private broadcaster: Broadcaster,
    private elRef: ElementRef,
    public  _cservice: CService) {
      this.GetUserWishList = _settingsService.getPath('GetUserWishList');
      this.wishListPostUrl = _settingsService.getPath('wishListPostUrl');
      this.DeleteUserWishListUrl = _settingsService.getPath('DeleteUserWishListUrl');
  }

  public ngAfterViewInit() {
    this.getProductInfo();
  }

  public ngOnInit() {
    this.type = '';
    this.emailId = this._cookieService.getObject('SESSION_PORTAL')['useremail'];
    this.showSimilarListing();
    this._route.params.subscribe((params) => {
      this.productId = params['id'];
      this.getProductInfo();
    });
    this.broadcaster.on<string>('WISH_LIST_UPDATED')
        .subscribe((data) => {
            this.GetWishList();
    });
  }

  private transformDate(date) {
    this.productInfoData.SubmittedDate = new Date();
    this.postedDate = this._datepipe.transform(this.productInfoData.SubmittedDate, 'yyyy-MM-dd');
  }

  private showSimilarListing() {
    console.log('display similar listing');
  }

  private showProductInfo(id) {
    this._router.navigate(['productInfo', id]);
  }

  private getProductInfo () {
    this.productDetails = this.productInfoUrl + this.productId;
    this._cservice.observableGetHttp(this.productDetails, null, false)
      .subscribe((res: Response) => {
          this.isClicked = false;
          this.productInfoData = res;
          this.GetWishList();
          this.type = this.productInfoData.Listing.SubCategory + '-'
          + this.productInfoData.Listing.ListingCategory;
          this.subcategoryData = this.productInfoData.Fields;
          this.transformDate(this.productInfoData.SubmittedDate);
        },
        (error) => {
          console.log('error in response');
        },
        () => {
          console.log('Finally');
        });
  }

  // get wishlist api
   private GetWishList() {
       let self = this;
       if (this.emailId && this.emailId !== '') {
          this._cservice.observableGetHttp(this.GetUserWishList + this.emailId, null, false)
           .subscribe((res: Response) => {
                console.log('this.GetUserWishList Response', res);
                self.wishListData = res;
                self.isInWhishList  = self.wishListData.indexOf(self.productInfoData.Listing._id)
                === -1 ? false : true;
           },
           (error) => {
               console.log('error in response', error);
           });
       }
   }

   private updateWishList() {
    if (this.wishListData.length <= 20 || this.isInWhishList === true) {
      let wishListCondtionalUrl = (this.isInWhishList ? this.DeleteUserWishListUrl
      : this.wishListPostUrl) + (this.emailId + '&listingId=' + this.productInfoData.Listing._id);
      let operation = this.isInWhishList ? 'observableDeleteHttp' : 'observablePostHttp';
      this._cservice[operation](wishListCondtionalUrl, null, false)
          .subscribe((res: Response) => {
              this.isInWhishList = !this.isInWhishList;
          },
          (error) => {
              console.log('error in response', error);
          });
      }else {
          alert('Only 20 Wishlist is allowed !!!');
      }
   }

  private showContact() {
    this.isClicked = true;
  }
}
