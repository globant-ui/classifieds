import { Component, EventEmitter, Output, ViewChild, AfterViewInit  } from '@angular/core';
import { AppState } from '../../app.service';
import { SettingsService } from '../../services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response, RequestOptions } from '@angular/http';
import { CService } from  '../../services/http.service';
import { CookieService } from 'angular2-cookie/core';
import 'rxjs/Rx';
import { Session } from '../../authentication/entity/session.entity';
import { ModalDirective } from 'ng2-bootstrap/modal';
import { WishListService } from '../service/wishlist.service';
import { Router } from '@angular/router';
import { Broadcaster } from  '../../services/broadcast.service';

@Component({
  selector: 'wishlist',
  styles: [ require('../styles/wishlist.component.scss').toString() ],
  providers: [],
  template: require('../tpls/wishlist.component.html').toString()
})

export class WishListComponent implements AfterViewInit {
  public GetUserWishList: string = '';
  public emailId: string = '';
  public wishListSelectedUrl: string = '';
  public DeleteUserWishListUrl: string = '';
  public WishListSelectedData: any;

@ViewChild('childModal') public childModal: ModalDirective;

  constructor(
                 public _settingsService: SettingsService,
                 public _cservice: CService,
                 public _router: Router,
                 public broadcaster: Broadcaster,
                 public _cookieService: CookieService,
                 public wishListService: WishListService) {

    this.emailId = this._cookieService.getObject('SESSION_PORTAL')['useremail'];
    this.GetUserWishList = _settingsService.getPath('GetUserWishList') + this.emailId;
    this.wishListSelectedUrl = _settingsService.getPath('wishListSelectedUrl');
    this.getUserWishListData();
    this.DeleteUserWishListUrl = _settingsService.getPath('DeleteUserWishListUrl')
    + this.emailId + '&listingId=';
  }

  public ngAfterViewInit() {
    this.showChildModal();
  }

  public showChildModal(): void {
    this.childModal.show();
  }

  public hideChildModal(): void {
    this.childModal.hide();
  }

  // get my wishlist pop-up api call
  public getUserWishListData() {
    this.wishListService.getUserWishList(this.wishListSelectedUrl)
    .then((res) => {
      this.WishListSelectedData = res;
      console.log('this.WishListSelectedData', res);
    },
        (error) => {
         console.log('error in response', error);
       }
    );
  }

  public showProductInfo(id) {
    this.hideChildModal();
    this._router.navigateByUrl('/dashboard/productInfo/' + id);
  }

  // delete api call
  public deleteWishListData(event, obj) {
    event.stopPropagation();
    let self = this;
    this.wishListService.deleteWishList(this.DeleteUserWishListUrl + obj._id)
    .then((res) => {
        let delIndex = self.WishListSelectedData.findIndex((o) => {
            return o._id === obj._id;
        });
        self.WishListSelectedData.splice(delIndex, 1);
        self.broadcaster.broadcast('WISH_LIST_UPDATED', 'success');
      },
      (error) => {
         console.log('error in response', error);
      }
    );
  }
}
