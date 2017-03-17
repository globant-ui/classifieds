import { Component,ViewChildren,OnInit, HostListener, Inject, ElementRef,ViewChild ,Input} from '@angular/core';
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

export class HomeComponent implements OnInit {
  private settings : any ;
  private baseUrl : any ;
  private  data : any;
  private cardUrl : string = '';
  private bannerUrl = 'http://in-it0289/MasterDataAPI/api/category/GetAllCategory';
  private cardsByCategoryUrl:string = '';
  private selectedFilter: string = '';
  public initialCardData: any;
  public bannerData: any;
  public filterCat:any;
  public navIsFixed: boolean = false;
  private topNavbar:any;
  private affixEl:any;
  private affixElOffsetTop: number;

  //@ViewChildren("cheader") CHeader;
  @ViewChild(SearchComponent) searchComponent;
  @ViewChild( CardListComponent) cardListComponent;


  constructor(
      public appState: AppState,
      private _settingsService: SettingsService,
      private _cservice:CService,
      private el:ElementRef) {
         this.cardUrl = _settingsService.getPath('cardUrl');
         this.cardsByCategoryUrl = _settingsService.getPath('cardsByCategoryUrl');
  }
  ngOnInit() {
    this.baseUrl=this._settingsService.getBaseUrl();
    this.getInitialCards();
    this.getBannerListing();
    this.getAffixElOffsetTop();
  }
  
   @HostListener( 'window:click', [ '$event' ] )
   onclick(event){
	   console.log("event====>",event)
   }
  getAffixElOffsetTop(){
    this.affixEl = this.el.nativeElement.querySelector('#cheader1');
    this.affixElOffsetTop = this.affixEl.offsetTop;
    //console.log('off = ',this.affixElOffsetTop);
  }

  getInitialCards () {
    this._cservice.observableGetHttp(this.cardUrl,null,false)
      .subscribe((res:Response)=> {
            this.searchComponent.setFilter( 'TOP TEN' );
            this.initialCardData = res;
            console.log("this.initialCardData",this.initialCardData)
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
           console.log("this.initialCardData",this.initialCardData)
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

    @HostListener("window:scroll", ['$event'])
    onWindowScroll(e) {
        let number = e.target.body.scrollTop+125;
        if (number > this.affixElOffsetTop) {
            this.navIsFixed = true;
        } else if (this.navIsFixed && number < this.affixElOffsetTop) {
            this.navIsFixed = false;
        }
    }
}
