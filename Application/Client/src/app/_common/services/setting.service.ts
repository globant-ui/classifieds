import { Injectable } from '@angular/core';
import { Observable  } from 'rxjs/Observable';
import { Http, Response, RequestOptions } from '@angular/http';
import { CService } from './http.service';
import 'rxjs/Rx';

@Injectable()
export class SettingsService {
  public  settings: any;
  public  data: any;
  private cardUrl = 'http://in-it0289/ListingAPI/api/Listings/GetTopListings';
  private productInfoUrl = 'http://in-it0289/ListingAPI/api/Listings/GetListingById?id=';
  private settingsJson = require('app/settings.json');
  private bannerListingsJson = require('app/banner/json/banner.json');
  private cardListingsJson = require('app/card-list/json/card-list.json');
  private filterListingJson = require('app/filter/json/filter.json');
  constructor(private _cservice: CService) {
    this.getSettings();
  }

  public getSettings() {
    this.settings = this.settingsJson;
    return this.settingsJson;
  }

  public getBaseUrl() {
    return this.settingsJson.services.main;
  }

  public getBannerListingsData() {
      return this.bannerListingsJson.details;
  }

  public getCardListingsData() {
      return this.cardListingsJson;
  }

  public getFilterListingData() {
    return this.filterListingJson.details;
  }

  public getPath(path) {
    return this.settingsJson.classifieds.services[path];
  }
}
