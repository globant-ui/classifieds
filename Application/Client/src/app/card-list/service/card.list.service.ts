// import { Injectable } from '@angular/core';
// import {Response, Http, Headers, RequestOptions} from '@angular/http';
// import {CService} from  '../../_common/services/http.service';
// import {SettingsService} from '../../_common/services/setting.service';
// import {CookieService} from 'angular2-cookie/core';
// import {Observable} from 'rxjs/Observable';

// @Injectable()
// export class WishListCardService extends CService  {

// private wishListSelectedUrl : string = '';
// private GetUserWishList:string = '';
// private emailId:string = '';
// private DeleteUserWishListUrl: string = '';
// private WishListSelectedData : any;

// constructor(
//     private _settingsService: SettingsService,
//     public _cookieService: CookieService,
//     public _http:Http
// ) {
//     super( _http, _cookieService );

//     this.emailId = this._cookieService.getObject('SESSION_PORTAL')["useremail"];
//     this.GetUserWishList = _settingsService.getPath('GetUserWishList') + this.emailId;
//     this.DeleteUserWishListUrl = _settingsService.getPath('DeleteUserWishListUrl') + this.emailId + '&listingId=';;
//     console.log("this.DeleteUserWishListUrl", this.DeleteUserWishListUrl);
//     this.wishListSelectedUrl = _settingsService.getPath('wishListSelectedUrl');
// }
//  //get my wishlist on landing page api call
//  GetWishListData(url){
//    return this.promiseGetHttp( url, null, false );
//  }

//   //delete api call
//   deleteWishList( url ) {
//     return this.promiseDeleteHttp( url, null, false );
//   }

// }