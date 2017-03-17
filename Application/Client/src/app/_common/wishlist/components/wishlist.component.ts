import { Component,ViewChild,OnInit, AfterViewInit  } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../services/http.service';
import {SharedService} from  '../../services/shared.service';
import {CookieService} from 'angular2-cookie/core';
import 'rxjs/Rx';
import {Session} from '../../authentication/entity/session.entity';
import { ModalDirective } from 'ng2-bootstrap/modal';

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
private cardUrl : string = '';
private initialCardData : any;
private wishListArray : any
private wishListDataId : any;
private getSelectedWishListObj:any = [];
private cardsByCategoryUrl:string;
private errorMessage:any;
private restAllCards : any;

@ViewChild('childModal') public childModal:ModalDirective;

  constructor(
                 private _settingsService: SettingsService,
                 private _cservice:CService,
                 private _cookieService:CookieService,
                 private sharedService:SharedService) {
                        this.GetUserWishList = _settingsService.getPath('GetUserWishList');
                        this.cardUrl = _settingsService.getPath('cardUrl');
                        this.cardsByCategoryUrl = _settingsService.getPath('cardsByCategoryUrl');
                this.getTopTen(); 
                this.getWishList();
                this.getRestAllCards();
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
//get top ten data
   getTopTen() {
     this.sharedService.setData()
      .subscribe((res: Response) => {
       if (res['length'] != 0) {
           this.initialCardData = res;
           console.log("logged",this.initialCardData)        
       }
      },
       error => {
         console.log("error in response",error);
       });
   }

   //get wishlist selected data
  getWishList() {
    this.sharedService.setWishListData()
      .subscribe((res: Response) => {
        if (res['length'] != 0) {
          this.wishListArray = res;
          console.log("datttt0000000000", this.wishListArray)
        }
      },
      error => {
        console.log("error in response", error);
      });
  }

  //get rest all data
  getRestAllCards() {
    this.sharedService.setRestAllCards()
      .subscribe((res: Response) => {
        if (res['length'] != 0) {
          this.restAllCards = res;
          console.log("rest", this.restAllCards)
        }
      },
      error => {
        console.log("error in response", error);
      });
  }

} 

