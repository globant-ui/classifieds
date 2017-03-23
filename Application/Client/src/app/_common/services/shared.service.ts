import { Injectable,EventEmitter } from '@angular/core';
import {Response, Http, Headers, RequestOptions} from '@angular/http';
import {CService} from  '../services/http.service';
import {SettingsService} from '../services/setting.service';
import {CookieService} from 'angular2-cookie/core';
import {Observable} from 'rxjs/Observable';

@Injectable()
export class WishlistService {

private wishListSelectedUrl : string = '';
private GetUserWishList:string = '';
private emailId:string = '';
private WishListSelectedData : any;
private DeleteUserWishListUrl: string = '';
private wishListGetData:any;

constructor(
    private _settingsService: SettingsService,
    private _cservice: CService,
    private _cookieService: CookieService

) {
    this.wishListGetData = new EventEmitter();
    this.emailId = this._cookieService.getObject('SESSION_PORTAL')["useremail"];
    this.GetUserWishList = _settingsService.getPath('GetUserWishList') + this.emailId;
     this.DeleteUserWishListUrl = _settingsService.getPath('DeleteUserWishListUrl') + this.emailId + '&listingId=';;
    console.log("this.DeleteUserWishListUrl", this.DeleteUserWishListUrl)

}

   GetWishList() {
       this._cservice.observableGetHttp(this.GetUserWishList, null, false)
           .subscribe((res: Response) => {
               console.log("this.GetUserWishList Response", res);
               if (res['length'] != 0) {
                    this.wishListGetData.emit('hello');
                   console.log('wishlist--')
               }
           },
           error => {
               console.log("error in response", error);
           });
   }
     //get my wishlist pop-up api call
  getUserWishList() {
    this._cservice.observableGetHttp(this.wishListSelectedUrl, null, false)
      .subscribe((res: Response) => {
        if (res['length'] > 0) {
          this.WishListSelectedData = res;
          console.log("this.WishListSelectedData", this.WishListSelectedData)
        }
      },
      error => {
        console.log("error in response", error);
      });
  }

  //delete api call
  deleteWishList(obj) {
    console.log('obj del id', this.DeleteUserWishListUrl)
    this._cservice.observableDeleteHttp(this.DeleteUserWishListUrl + obj._id, null, false)
      .subscribe((res: Response) => {
        console.log("deleted");
        this.GetWishList();
        this.getUserWishList();
      },
      error => {
        console.log("error in response", error);
      });
  }
}