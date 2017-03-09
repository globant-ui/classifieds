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

    @Input() cards;

    constructor(
                public appState: AppState,
                private _settingsService : SettingsService,
                private _router:Router,
                private _cservice:CService,
                private _cookieService:CookieService
                 ) {
                    this.favoriteUrl = _settingsService.getPath('favoriteUrl');
                    this.filterCategoryUrl = _settingsService.getPath('filterCategoryUrl');
                    this.GetUserWishList = _settingsService.getPath('GetUserWishList');
                 }
    ngOnChanges(changes:SimpleChanges){
        if(changes['cards']){
            this.GetWishList();
        }
    }

    private updatedCards = [];
    private isLoading:boolean = false;
    private listingId:number;
    
    ngOnInit() {
        console.log('all = ',this.cards);
        this.emailId = this._cookieService.getObject('SESSION_PORTAL')["useremail"];
        console.log("card-list email",this.emailId)
    }
        
    loading( flag ) {
        this.isLoading = flag;
    }

   showProductInfo(id){
     this.showProductInfoPage = true;
     this._router.navigate(['productInfo',id]);
  }
//favorite card method
  favorite(id, i) {
      debugger;
     // this.favColor = !this.favColor;
      //post call for favorite 
      this.listingId = id; 
      this.favoriteUrl = this.favoriteUrl + this.emailId + '&listingId='+id;
        this._cservice.observablePostHttp(this.favoriteUrl , id, null, false)
          .subscribe((res: Response) => {
              this.updatedCards[i]['isInWishList'] = true;
              console.log("postcall response", res);
          },
          error => {
              console.log("error in response", error);
          });
  }
  GetWishList(){
        this._cservice.observableGetHttp(this.GetUserWishList + this.emailId, null, false)
          .subscribe((res: Response) => {
            if(res['length']!=0){
                this.updateCards(res);
            }
          },
          error => {
              console.log("error in response", error);
          });
  }
  updateCards(wishListData){
      if( this.cards && this.cards.length != 0 ) {
          for(let i=0;i<this.cards.length;i++) {
            if( wishListData.indexOf(this.cards[i]['_id']) == -1 ){
                this.cards[i]['isInWishList'] = false;
            } else {
                this.cards[i]['isInWishList'] = true;
            }

            this.updatedCards.push( this.cards[i] );
        }
        console.log( this.updatedCards );
      } 
  }
}
