import {Injectable} from '@angular/core';
import { Observable  } from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from './http.service';
import 'rxjs/Rx';

var settingsJson = require("app/settings.json");

var bannerListingsJson = require("app/banner/json/banner.json");
var cardListingsJson = require("app/card-list/json/card-list.json");
var filterListingJson = require("app/filter/json/filter.json");


@Injectable()
export class SettingsService{
  public  settings : any ;
  public  data: any;
  private cardUrl = 'http://in-it0289/ListingAPI/api/Listings/GetTopListings';
  private productInfoUrl = 'http://in-it0289/ListingAPI/api/Listings/GetListingById?id=';

  constructor(private _cservice:CService) {
    this.getSettings();
  }

  getSettings(){
    this.settings = settingsJson;
    return settingsJson;
  }

  getBaseUrl(){
    return settingsJson.services.main;
  }

  getBannerListingsData(){
      return bannerListingsJson.details;
  }

  getCardListingsData() {
      return cardListingsJson;
  }

  getFilterListingData(){
    return filterListingJson.details;
  }

}



