import { Component,ViewChild,OnInit, AfterViewInit  } from '@angular/core';
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

export class WishListComponent implements OnInit, AfterViewInit  {

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
                 private wishListService : WishListService ) {

    this.emailId = this._cookieService.getObject('SESSION_PORTAL')["useremail"];
    this.GetUserWishList = _settingsService.getPath('GetUserWishList') + this.emailId;
    console.log("this.GetUserWishList", this.GetUserWishList)
    this.wishListSelectedUrl = _settingsService.getPath('wishListSelectedUrl');
    this.getUserWishListData();
    //this.GetWishList();
    this.DeleteUserWishListUrl = _settingsService.getPath('DeleteUserWishListUrl') + this.emailId + '&listingId=';;
    console.log("this.DeleteUserWishListUrl", this.DeleteUserWishListUrl)
  }

  ngOnInit() {}

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
    },
        error => {
         console.log("error in response", error);
       }
    );
  }

  //delete api call
  deleteWishListData(obj) {
    this.wishListService.deleteWishList(this.DeleteUserWishListUrl+ obj._id)
    .then(res=>{
      console.log('deleted');
      this.getUserWishListData();
    },
        error => {
         console.log("error in response", error);
       }
    );
    // console.log('obj del id', this.DeleteUserWishListUrl)
    // this._cservice.observableDeleteHttp(this.DeleteUserWishListUrl + obj._id, null, false)
    //   .subscribe((res: Response) => {
    //     console.log("deleted");
    //     this.GetWishList();
    //     this.getUserWishList();
    //   },
    //   error => {
    //     console.log("error in response", error);
    //   });
  }

  // GetWishList() {
  //   console.log("this.GetUserWishList", this.GetUserWishList)
  //   this._cservice.observableGetHttp(this.GetUserWishList, null, false)
  //     .subscribe((res: Response) => {
  //       console.log("this.GetUserWishList Response", res);
  //       if (res['length'] != 0) {
  //         console.log('wishlist--')
  //       }
  //     },
  //     error => {
  //       console.log("error in response", error);
  //     });
  // }

} 

