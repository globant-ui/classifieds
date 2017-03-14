"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
require("rxjs/Rx");
var settingsJson = require("app/settings.json");
var bannerListingsJson = require("app/banner/json/banner.json");
var cardListingsJson = require("app/card-list/json/card-list.json");
var filterListingJson = require("app/filter/json/filter.json");
var SettingsService = (function () {
    function SettingsService(_cservice) {
        this._cservice = _cservice;
        this.cardUrl = 'http://in-it0289/ListingAPI/api/Listings/GetTopListings';
        this.productInfoUrl = 'http://in-it0289/ListingAPI/api/Listings/GetListingById?id=';
        this.getSettings();
    }
    SettingsService.prototype.getSettings = function () {
        this.settings = settingsJson;
        return settingsJson;
    };
    SettingsService.prototype.getBaseUrl = function () {
        return settingsJson.services.main;
    };
    SettingsService.prototype.getBannerListingsData = function () {
        return bannerListingsJson.details;
    };
    SettingsService.prototype.getCardListingsData = function () {
        return cardListingsJson;
    };
    SettingsService.prototype.getFilterListingData = function () {
        return filterListingJson.details;
    };
    return SettingsService;
}());
SettingsService = __decorate([
    core_1.Injectable()
], SettingsService);
exports.SettingsService = SettingsService;
