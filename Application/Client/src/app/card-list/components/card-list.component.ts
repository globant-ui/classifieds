import { Component,Input,OnInit,SimpleChanges,AfterViewInit} from '@angular/core';
import { AppState } from '../../app.service';
import { SettingsService } from '../../_common/services/setting.service';
import { LoaderComponent } from '../../_common/loader/components/loader.component';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../_common/services/http.service';
import {Broadcaster} from  '../../_common/services/broadcast.service';
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
    private wishListPostUrl:string = '';
    private wishListCondtionalUrl:string = '';
    private emailId:string = '';
    private filterCategoryUrl:string = '';
    private GetUserWishList: string = '';
    private DeleteUserWishListUrl: string = '';
    private isLoading:boolean = false;
    private wishListData : any;

    @Input() cards;

    constructor(
        public appState: AppState,
        private _settingsService: SettingsService,
        private _router: Router,
        private broadcaster: Broadcaster,
        private _cservice: CService,
        private _cookieService: CookieService,
    ) {
        this.wishListPostUrl = _settingsService.getPath('wishListPostUrl');
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
        this.emailId = this._cookieService.getObject('SESSION_PORTAL')["useremail"];
        this.GetUserWishList = this.GetUserWishList + this.emailId;
        this.GetWishList();
        let self = this;
        this.broadcaster.on<string>('WISH_LIST_UPDATED')
            .subscribe(data => {
                self.GetWishList();
        });
    }
    
    loading( flag ) {
        this.isLoading = flag;
    }

   showProductInfo(id){
     this._router.navigateByUrl('/dashboard/productInfo/'+id);
  }

    //favorite card method
   favorite(event,id, i, card) {
       event.stopPropagation();
       if(this.wishListData.length <= 20 || card.isInWishList === true){
        this.wishListCondtionalUrl = (card.isInWishList ? this.DeleteUserWishListUrl : this.wishListPostUrl) + (this.emailId + '&listingId=' + id);
        var operation = card.isInWishList ? 'observableDeleteHttp' : 'observablePostHttp';
        this._cservice[operation](this.wishListCondtionalUrl, null, false)
            .subscribe((res: Response) => {
                this.cards[i]['isInWishList'] = !this.cards[i]['isInWishList'];
                this.GetWishList();
            },
            error => {
                console.log("error in response", error);
            });
       }
       else{
           alert("Only 20 Wishlist is allowed !!!");
       }
   }

//get wishlist api
   GetWishList() {
       let self = this;
       if(this.emailId != "")
       this._cservice.observableGetHttp(this.GetUserWishList, null, false)
           .subscribe((res: Response) => {
                console.log("this.GetUserWishList Response", res);
                self.wishListData = res;
                this.updateCards(res);
           },
           error => {
               console.log("error in response", error);
           });
   }

//update the cards
   updateCards(wishListData) {
       if ( this.cards && this.cards.length != 0 ) {
           for ( let i = 0; i < this.cards.length; i++ ) {
               this.cards[i]['isInWishList'] = wishListData.indexOf(this.cards[i]['_id']) > -1;
           }
       }
   }
}
