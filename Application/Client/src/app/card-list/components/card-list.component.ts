import { Component,Input,OnInit,SimpleChanges } from '@angular/core';
import { AppState } from '../../app.service';
import { SettingsService } from '../../_common/services/setting.service';
import { LoaderComponent } from '../../_common/loader/components/loader.component';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../_common/services/http.service';
import {CookieService} from 'angular2-cookie/core';
import {Router} from '@angular/router';

let styles = require('../styles/card-list.component.scss').toString();
let tpls = require('../tpls/card-list.component.html').toString();

@Component({
    selector : 'card-list',
    styles : [styles],
    providers:[SettingsService],
    template : tpls
})
export class CardListComponent{

    public showProductInfoPage: boolean = false;
    private favoriteUrl:string = '';
    private emailId:string = '';
    private filterCategoryUrl:string = '';
    private GetUserWishList: string = '';
    private DeleteUserWishListUrl: string = '';
    private updatedCards = [];
    private isLoading:boolean = false;

    @Input() cards;

    constructor(
        public appState: AppState,
        private _settingsService: SettingsService,
        private _router: Router,
        private _cservice: CService,
        private _cookieService: CookieService
    ) {
        this.favoriteUrl = _settingsService.getPath('favoriteUrl');
        this.filterCategoryUrl = _settingsService.getPath('filterCategoryUrl');
        this.GetUserWishList = _settingsService.getPath('GetUserWishList');
        this.DeleteUserWishListUrl = _settingsService.getPath('DeleteUserWishListUrl');
    }

    ngOnChanges(changes:SimpleChanges){
        if(changes['cards']){
            this.GetWishList();
        }
    }

    ngOnInit() {
        console.log('all = ', this.cards);
        this.emailId = this._cookieService.getObject('SESSION_PORTAL')["useremail"];
        console.log("card-list email", this.emailId);
        this.GetUserWishList = this.GetUserWishList + this.emailId;
    }

    loading(flag) {
        this.isLoading = flag;
    }

   showProductInfo(id){
     this.showProductInfoPage = true;
     this._router.navigate(['productInfo',id]);
  }

//favorite card method
  favorite(id, i,card) {
     // if (card['isInWishList'] === undefined || card['isInWishList'] === false) {
          console.log("card : ",card)
          console.log("this.favoriteUrl",this.favoriteUrl)
          this.favoriteUrl = (card.isInWishList ? this.DeleteUserWishListUrl : this.favoriteUrl) + ( this.emailId + '&listingId=' + id);
          var operation = card.isInWishList ? 'observableDeleteHttp' :'observablePostHttp';
          this._cservice[operation]( this.favoriteUrl,card.isInWishList ? null : card, card.isInWishList ? false: null, false)
              .subscribe((res: Response) => {
                //   this.updatedCards = this.cards;
                  this.cards[i]['isInWishList'] = !this.cards[i]['isInWishList'];
                //   console.log("postcall response", res);
              },
              error => {
                  console.log("error in response", error);
              });
      //}
     
    
    //delete api call
    //   if(card.isInWishList === true){
    //       debugger;
    //    this.DeleteUserWishListUrl = this.DeleteUserWishListUrl + this.emailId + '&listingId='+id;
    //    this._cservice.observableDeleteHttp(this.DeleteUserWishListUrl, null, false)
    //       .subscribe((res: Response) => {
    //       },
    //       error => {
    //           console.log("error in response", error);
    //       });
    //   }
       
  }
//get wishlist api
  GetWishList() {
      // this.GetUserWishList = this.GetUserWishList + this.emailId;
      console.log("this.GetUserWishList", this.GetUserWishList)
      this._cservice.observableGetHttp(this.GetUserWishList, null, false)
          .subscribe((res: Response) => {
              console.log("this.GetUserWishList Response", res);
              if (res['length'] != 0) {
                  this.updateCards(res);
              }
          },
          error => {
              console.log("error in response", error);
          });
  }

  updateCards(wishListData) {
      if (this.cards && this.cards.length != 0) {
        //  this.updatedCards = [];
          for ( let i = 0; i < this.cards.length; i++ ) {

             this.cards[i]['isInWishList'] = wishListData.indexOf(this.cards[i]['_id'] ) > -1
            //   if ( wishListData.indexOf(this.cards[i]['_id'] ) == -1) {
            //       this.cards[i]['isInWishList'] = false;
            //   } else {
            //       this.cards[i]['isInWishList'] = true;
            //   }

             // this.updatedCards.push(this.cards[i]);
          }
          console.log(this.cards);
      }
  }
}
