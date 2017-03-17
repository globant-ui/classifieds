import { Injectable } from '@angular/core';
import {Response, Http, Headers, RequestOptions} from '@angular/http';
import {CService} from  '../services/http.service';
import {SettingsService} from '../services/setting.service';
import {CookieService} from 'angular2-cookie/core';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

@Injectable()
export class SharedService {

private cardUrl : string = '';
private GetUserWishList:string = '';
 private cardsByCategoryUrl:string = '';
private getTopTenData : any;
private getWishListData : any;
private emailId : string = '';

    constructor(
        private _http: Http, 
        private _cservice:CService,
        private _settingsService: SettingsService,
        private _cookieService:CookieService,
        ) { 
            this.cardUrl = _settingsService.getPath('cardUrl');
            this.GetUserWishList = _settingsService.getPath('GetUserWishList');
             this.cardsByCategoryUrl = _settingsService.getPath('cardsByCategoryUrl');
            this.emailId = this._cookieService.getObject('SESSION_PORTAL')["useremail"];
            this.GetUserWishList = this.GetUserWishList + this.emailId;
            console.log("cardUrl",this.cardUrl)
            console.log('datrrrrrrrrrrrrr', this.GetUserWishList)
            this.getData();
            this. getWishList();
            this.getRestAllCards('Electronics');
        }
    ngOnit() {
       
    }
//get top ten cards
    getData() {
        this.getTopTenData = this._cservice.observableGetHttp(this.cardUrl, null, false);
    }
    setData() {
        return this.getTopTenData;
    }

//get wishlistget
    getWishList() {
        this.getWishListData = this._cservice.observableGetHttp(this.GetUserWishList, null, false)
    }

    setWishListData() {
        return this.getWishListData;
    }

    //get all cards api call
    getRestAllCards(categoryName){
        this.getWishListData = this._cservice.observableGetHttp(this.cardsByCategoryUrl + categoryName, null, false)
    }

     setRestAllCards(){
        return this.getWishListData; 
    }

 }
//http://in-it0289/ListingAPI/api/Listings/GetListingsByCategory?Category=Electronics