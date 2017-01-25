import { Component,ViewChildren,OnInit,AfterViewInit } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../_common/services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../_common/services/http.service';
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
  public initialCardData: any;
  public bannerData: any;
  public filterCat:any;

  @ViewChildren("cheader") CHeader;

  constructor(
      public appState: AppState,
      private _settingsService: SettingsService,
      private _cservice:CService) {
  }
  ngOnInit() {
    this.baseUrl=this._settingsService.getBaseUrl();
    this.getInitialCards();
    this.getBannerListing();
   // console.log("=======>",this.CHeader.first.nativeElement)

  }
  ngAfterViewInit(){
    let cheaderRef = this.CHeader.toArray();
     console.log("=======>",this.CHeader.first.nativeElement);
     console.log("toArrays=======>",cheaderRef[0].nativeElement.id);
  }
  getInitialCards (){
    this._cservice.observableGetHttp(this.cardUrl,null,false)
      .subscribe((res:Response)=> {
            console.log('cards response = ',res);
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

    showOutput(category){
        this.initialCardData = category;
    }
}
