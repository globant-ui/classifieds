import { Component,ViewChildren,ViewChild ,Input,OnInit } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../_common/services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../_common/services/http.service';
import { SearchComponent } from '../../_common/search/components/search.component';
import { CardListComponent } from '../../card-list/components/card-list.component';
import { SelectInterestComponent } from '../../_common/select-interest/components/select-interest.component';
import 'rxjs/Rx';


let styles = require('../styles/home.component.scss').toString();
let tpls = require('../tpls/home.component.html').toString();

@Component({
  selector: 'home',
  styles : [ styles ],
  providers:[SettingsService, CService],
  template : tpls
})

export class HomeComponent {
  private settings : any ;
  private baseUrl : any ;
  private  data : any;
  private cardUrl = 'http://in-it0289/ListingAPI/api/Listings/GetTopListings';
  private bannerUrl = 'http://in-it0289/MasterDataAPI/api/category/GetAllCategory';
  private cardsByCategoryUrl = 'http://in-it0289/ListingAPI/api/Listings/GetListingsByCategory?Category=';
  private selectedFilter: string = '';
  public initialCardData: any;
  public bannerData: any;
  public filterCat:any;
 


  @ViewChildren("cheader") CHeader;
  @ViewChild(SearchComponent) searchComponent;
  @ViewChild( CardListComponent) cardListComponent



  constructor(
      public appState: AppState,
      private _settingsService: SettingsService,
      private _cservice:CService) {
  }
  ngOnInit() {
    this.baseUrl=this._settingsService.getBaseUrl();
    this.getInitialCards();
    this.getBannerListing();
  }

  getInitialCards () {
    this._cservice.observableGetHttp(this.cardUrl,null,false)
      .subscribe((res:Response)=> {
            this.searchComponent.setFilter( 'TOP TEN' );
            this.initialCardData = res;
        },
        error => {
          console.log("error in response");
        },
        ()=>{
          console.log("Finally");
        })
  }

  getSelectedFilterOption( selectedFilter ) {
    this.getCards( selectedFilter );
  }

  getCards( categoryName ) {
    this.cardListComponent.loading( true );
    let url;
    if( categoryName == 'Top ten') {
      url = this.cardUrl;
      this.searchComponent.setFilter( 'TOP TEN' );
    } else {
      url = this.cardsByCategoryUrl + categoryName;
      this.searchComponent.setFilter( categoryName );
    }

    this._cservice.observableGetHttp(url, null, false)
    .subscribe((res:Response)=> {
          this.cardListComponent.loading( false );
          this.initialCardData = res;
      },
      error => {
        console.log("error in response");
      },
      ()=>{
        console.log("Finally");
      })
  }

   getBannerListing (){
     this._cservice.observableGetHttp(this.bannerUrl,null,false)
       .subscribe((res:Response)=> {
           this.bannerData = res;
         },
         error => {
           console.log("error in response");
         },
         ()=>{
           console.log("Finally");
         })
   }

    getSelectedFilter(selectedOpt){
      this.selectedFilter = selectedOpt;
    }
}
