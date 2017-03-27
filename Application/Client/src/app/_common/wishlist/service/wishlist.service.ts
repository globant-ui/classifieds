import { Injectable, Output, EventEmitter } from '@angular/core';
import { Response, Http, Headers, RequestOptions } from '@angular/http';
import { CService } from '../../services/http.service';
import { SettingsService } from '../../services/setting.service';
import { CookieService } from 'angular2-cookie/core';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class WishListService extends CService {

  constructor(
    private _settingsService: SettingsService,
    public _cookieService: CookieService,
    public _http: Http
  ) {
    super(_http, _cookieService);
  }

  //get my wishlist pop-up api call
  getUserWishList(url) {
    return this.promiseGetHttp(url, null, false);
  }

  //get wishlist ids.
  GetWishList(url) {
    return this.promiseGetHttp(url, null, false);
  }

  //delete api call
  deleteWishList(url) {
    // this.onWishlistUpdate.emit(true);
    return this.promiseDeleteHttp(url, null, false);
  }

}