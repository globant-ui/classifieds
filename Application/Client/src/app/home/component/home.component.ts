import { Component, ViewChildren, OnInit, HostListener,
  Inject, ElementRef, ViewChild, Input } from '@angular/core';
import { AppState } from '../../app.service';
import { SettingsService } from '../../_common/services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response, RequestOptions } from '@angular/http';
import { CService } from  '../../_common/services/http.service';
import { SearchComponent } from '../../_common/search/components/search.component';
import { CardListComponent } from '../../card-list/components/card-list.component';
import { SelectInterestComponent }
from '../../_common/select-interest/components/select-interest.component';
import 'rxjs/Rx';

@Component({
  selector: 'home',
  styles : [require('../styles/home.component.scss').toString()],
  providers: [SettingsService, CService],
  template : require('../tpls/home.component.html').toString()
})

export class HomeComponent implements OnInit {
  private settings: any ;
  private baseUrl: any ;
  private data: any;
  private cardUrl: string = '';
  private bannerUrl: string = 'http://in-it0289/MasterDataAPI/api/category/GetAllCategory';
  private cardsByCategoryUrl: string = '';
  private selectedFilter: string = '';
  private initialCardData: any;
  private bannerData: any;
  private filterCat: any;
  private navIsFixed: boolean = false;
  private isSearchActive: boolean = false;
  private topNavbar: any;
  private affixEl: any;
  private affixElOffsetTop: number;
  private recommededUrl: string = '';
  private searchValue: string = '';

  @ViewChild(SearchComponent) private searchComponent;
  @ViewChild( CardListComponent) private cardListComponent;

  constructor(
      public appState: AppState,
      private _settingsService: SettingsService,
      private _cservice: CService,
      private el: ElementRef) {
        this.cardUrl = _settingsService.getPath('cardUrl');
        this.cardsByCategoryUrl = _settingsService.getPath('cardsByCategoryUrl');
        this.recommededUrl = _settingsService.getPath('recommededUrl');
  }

  public ngOnInit() {
    this.baseUrl = this._settingsService.getBaseUrl();
    this.getInitialCards();
    this.getBannerListing();
    this.getAffixElOffsetTop();
  }

  private getAffixElOffsetTop() {
    this.affixEl = this.el.nativeElement.querySelector('#cheader1');
    this.affixElOffsetTop = this.affixEl.offsetTop;
  }

  private getInitialCards () {
    this._cservice.observableGetHttp(this.cardUrl, null, false)
      .subscribe((res: Response) => {
            this.searchComponent.setFilter( 'TOP TEN' );
            this.initialCardData = res;
            console.log('this.initialCardData', this.initialCardData);
        },
        (error) => {
          console.log('error in response');
        },
        () => {
          console.log('Finally');
        });
  }

  private getSelectedFilterOption( selectedFilter ) {
    this.getCards( selectedFilter );
  }

  private getCards( data ) {
    this.cardListComponent.loading( true );
    let url;
    if (data.result) {
      this.isSearchActive = true;
      this.cardListComponent.loading( false );
      this.initialCardData = data.result;
      this.searchValue = data.categoryName;
    }else if ( data.categoryName === 'Top ten') {
      url = this.cardUrl;
      this.searchComponent.setFilter( 'TOP TEN' );
    }else if (data.categoryName === 'Recommended') {
         url = this.recommededUrl;
         this.searchComponent.setFilter( 'Recommended' );
    }else {
      url = this.cardsByCategoryUrl + data.categoryName;
      this.searchComponent.setFilter( data.categoryName );
    }

    if (!data.result) {
      this._cservice.observableGetHttp(url, null, false)
      .subscribe((res: Response) => {
            this.cardListComponent.loading( false );
            this.isSearchActive = false;
            this.searchValue = '';
            if (res) {
                this.initialCardData = res;
              }else {
                this.initialCardData = [];
              }
        },
        (error) => {
          console.log('error in response', error);
        },
        () => {
          console.log('Finally');
        });
    }
  }

   private getBannerListing () {
     this._cservice.observableGetHttp(this.bannerUrl, null, false)
       .subscribe((res: Response) => {
           this.bannerData = res;
           console.log('bannerData', this.bannerData);
         },
         (error) => {
           console.log('error in response');
         },
         () => {
           console.log('Finally');
         });
   }

    private getSelectedFilter(selectedOpt) {
      this.selectedFilter = selectedOpt;
    }

    @HostListener('window:scroll', ['$event'])
    private onWindowScroll(e) {
        let navY = e.target.body.scrollTop + 125;
        if (navY > this.affixElOffsetTop) {
            this.navIsFixed = true;
        }else if (this.navIsFixed && navY < this.affixElOffsetTop) {
            this.navIsFixed = false;
        }
    }
}
