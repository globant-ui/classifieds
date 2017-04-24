import { Component, Input, OnInit, SimpleChanges, AfterViewInit, OnChanges } from '@angular/core';
import { AppState } from '../../app.service';
import { SettingsService } from '../../_common/services/setting.service';
import { LoaderComponent } from '../../_common/loader/components/loader.component';
import { Observable }     from 'rxjs/Observable';
import { Http, Response, RequestOptions } from '@angular/http';
import { CService } from  '../../_common/services/http.service';
import { Broadcaster } from  '../../_common/services/broadcast.service';
import { CookieService } from 'angular2-cookie/core';
import { Router } from '@angular/router';

@Component({
    selector : 'card-list',
    styles : [require('../styles/card-list.component.scss').toString()],
    providers: [SettingsService],
    template : require('../tpls/card-list.component.html').toString()
})
export class CardListComponent implements OnInit, OnChanges {
    public showProductInfoPage: boolean = false;
    private wishListPostUrl: string = '';
    private wishListCondtionalUrl: string = '';
    private emailId: string = '';
    private filterCategoryUrl: string = '';
    private GetUserWishList: string = '';
    private DeleteUserWishListUrl: string = '';
    private isLoading: boolean = false;
    private wishListData: any;

    @Input() private cards;

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

    public ngOnInit() {
        this.emailId = this._cookieService.getObject('SESSION_PORTAL')['useremail'];
        this.GetUserWishList = this.GetUserWishList + this.emailId;
        this.GetWishList();
        let self = this;
        this.broadcaster.on<string>('WISH_LIST_UPDATED')
            .subscribe((data) => {
                self.GetWishList();
        });
    }

    public ngOnChanges(changes: SimpleChanges) {
        if (changes['cards']) {
            this.GetWishList();
        }
    }

    private loading( flag ) {
        this.isLoading = flag;
    }

   private showProductInfo(id) {
     this._router.navigateByUrl('/dashboard/productInfo/' + id);
  }

// favorite card method
   private favorite(event, id, i, card) {
       event.stopPropagation();
       if (this.wishListData.length <= 20 || card.isInWishList === true) {
        this.wishListCondtionalUrl = (card.isInWishList
        ? this.DeleteUserWishListUrl : this.wishListPostUrl) + (this.emailId + '&listingId=' + id);
        let operation = card.isInWishList ? 'observableDeleteHttp' : 'observablePostHttp';
        this._cservice[operation](this.wishListCondtionalUrl, null, false)
            .subscribe((res: Response) => {
                this.cards[i]['isInWishList'] = !this.cards[i]['isInWishList'];
                this.GetWishList();
            },
            (error) => {
                console.log('error in response', error);
            });
       }else {
           alert('Only 20 Wishlist is allowed !!!');
       }
   }

// get wishlist api
   private GetWishList() {
       let self = this;
       if (this.emailId !== '') {
            this._cservice.observableGetHttp(this.GetUserWishList, null, false)
           .subscribe((res: Response) => {
                console.log('this.GetUserWishList Response', res);
                self.wishListData = res;
                this.updateCards(res);
           },
           (error) => {
               console.log('error in response', error);
           });
       }
   }

// update the cards
   private updateCards(wishListData) {
       if ( this.cards && this.cards.length !== 0 ) {
           for (let card of this.cards) {
               card['isInWishList'] = wishListData.indexOf(card['_id']) > -1;
           }
       }
   }
}
