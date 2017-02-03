import { Component,ViewChildren,ViewChild ,OnInit } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../_common/services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../_common/services/http.service';
import { SearchComponent } from '../../_common/search/components/search.component';
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
  private selectedFilter: string = '';
  public initialCardData: any;
  public bannerData: any;
  public filterCat:any;


  @ViewChildren("cheader") CHeader;
  @ViewChild(SearchComponent) searchComponent;


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

    showOutput(obj){
        this.initialCardData = obj.result;
        this.searchComponent.setFilter( obj.categoryName );
    }
    getSelectedFilter(selectedOpt){
      this.selectedFilter = selectedOpt;
    }
}
