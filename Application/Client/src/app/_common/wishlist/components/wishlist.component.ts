import { Component,EventEmitter, Output,ViewChild, AfterViewInit  } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../services/http.service';
import {CookieService} from 'angular2-cookie/core';
import 'rxjs/Rx';
import {Session} from '../../authentication/entity/session.entity';
import { ModalDirective } from 'ng2-bootstrap/modal';
import { WishListService } from '../service/wishlist.service'

let styles = require('../styles/wishlist.component.scss').toString();
let tpls = require('../tpls/wishlist.component.html').toString();

@Component({
  selector: 'wishlist',
  styles : [ styles ],
  template : tpls
})

export class WishListComponent implements AfterViewInit  {

private GetUserWishList:string = '';
private emailId:string = '';
private wishListSelectedUrl : string = '';
private DeleteUserWishListUrl: string = '';
private WishListSelectedData : any;



@ViewChild('childModal') public childModal:ModalDirective;

  constructor(
                 private _settingsService: SettingsService,
                 private _cservice:CService,
                 private _cookieService: CookieService,
                 private wishListService : WishListService) {

    this.emailId = this._cookieService.getObject('SESSION_PORTAL')["useremail"];
    this.GetUserWishList = _settingsService.getPath('GetUserWishList') + this.emailId;
    this.wishListSelectedUrl = _settingsService.getPath('wishListSelectedUrl');
    this.getUserWishListData();
    this.DeleteUserWishListUrl = _settingsService.getPath('DeleteUserWishListUrl') + this.emailId + '&listingId=';;
   
  }

  ngAfterViewInit() {
    this.showChildModal();
  }

  public showChildModal(): void {
    this.childModal.show();
  }

  public hideChildModal(): void {
    this.childModal.hide();
  }

  //get my wishlist pop-up api call
  getUserWishListData() {
    this.wishListService.getUserWishList(this.wishListSelectedUrl)
    .then(res=>{
      this.WishListSelectedData = res;
      console.log("this.WishListSelectedData",res)
    },
        error => {
         console.log("error in response", error);
       }
    );
  }

  //delete api call
  deleteWishListData(obj) {
    let self = this;
    this.wishListService.deleteWishList(this.DeleteUserWishListUrl+ obj._id)
    .then(res=>{
      let delIndex = self.WishListSelectedData.findIndex(function(o){
          return o._id === obj._id;
      })
      self.WishListSelectedData.splice(delIndex, 1);
    },
        error => {
         console.log("error in response", error);
       }
    );
  }
} 

