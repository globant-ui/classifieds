"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
require("rxjs/add/operator/toPromise");
var CService = (function () {
    function CService(_http, _cookieService) {
        this._http = _http;
        this._cookieService = _cookieService;
    }
    CService.prototype.getHeaders = function () {
        var headers = new http_1.Headers();
        this.sessionObj = this._cookieService.getObject('SESSION_PORTAL');
        headers.append('Content-Type', 'application/json; charset=UTF-8');
        if (this.sessionObj != undefined) {
            headers.append('AccessToken', this.sessionObj.token);
            headers.append('UserEmail', this.sessionObj.useremail);
        }
        return headers;
    };
    CService.prototype.getRequestOptions = function () {
        var requestOptions = new http_1.RequestOptions();
        requestOptions.headers = this.getHeaders();
        return requestOptions;
    };
    CService.prototype.observableGetHttp = function (url, options, native) {
        return this._http.get(url, (native ? options : this.getRequestOptions()))
            .map(this.extractData)
            .catch(this.handleErrorObservable);
    };
    CService.prototype.observablePostHttp = function (url, data, options, native) {
        return this._http.post(url, data, (native ? options : this.getRequestOptions()))
            .map(this.extractData)
            .catch(this.handleErrorObservable);
    };
    CService.prototype.observablePutHttp = function (url, data, options, native) {
        return this._http.put(url, data, (native ? options : this.getRequestOptions()))
            .map(this.extractData)
            .catch(this.handleErrorObservable);
    };
    CService.prototype.observableDeleteHttp = function (url, options, native) {
        return this._http.delete(url, (native ? options : this.getRequestOptions()))
            .map(this.extractData)
            .catch(this.handleErrorObservable);
    };
    CService.prototype.promiseGetHttp = function (url, options, native) {
        return this._http.get(url, (native ? options : this.getRequestOptions()))
            .toPromise()
            .then(this.extractData)
            .catch(this.handlePromise);
    };
    CService.prototype.promisePostHttp = function (url, data, options, native) {
        return this._http.post(url, data, (native ? options : this.getRequestOptions()))
            .toPromise()
            .then(this.extractData)
            .catch(this.handlePromise);
    };
    CService.prototype.promisePutHttp = function (url, data, options, native) {
        return this._http.put(url, data, (native ? options : this.getRequestOptions()))
            .toPromise()
            .then(this.extractData)
            .catch(this.handlePromise);
    };
    CService.prototype.promiseDeleteHttp = function (url, options, native) {
        return this._http.delete(url, (native ? options : this.getRequestOptions()))
            .toPromise()
            .then(this.extractData)
            .catch(this.handlePromise);
    };
    CService.prototype.extractData = function (response) {
        var body = (response.status == 200);
        try {
            var json = response.json();
            body = json;
        }
        catch (e) {
        }
        return body || null;
    };
    CService.prototype.handleErrorObservable = function (error) {
        return Rx_1.Observable.throw({
            'status': error.status,
            'message': (error.message) ? error.message : error.status ? error.status + " - " + error.statusText : 'Server error'
        });
    };
    CService.prototype.handlePromise = function (error) {
        return Promise.reject({
            'status': error.status,
            'message': (error.message) ? error.message : error.status ? error.status + " - " + error.statusText : 'Server error'
        });
    };
    CService.prototype.objectToArray = function (_json, _type) {
        return Object.keys(_json).map(function (key) {
            if (_type) {
                return new _type(_json[key]);
            }
            else {
                return _json[key];
            }
        });
    };
    CService.prototype.arrayToArrayType = function (_array, _type) {
        var array = [];
        _array.forEach(function (val, key) {
            array.push(new _type(_array[key]));
        });
        return this.cleanArray(array);
    };
    CService.prototype.cleanArray = function (_array) {
        var array = [];
        _array.forEach(function (val, key) {
            if (_array[key] && val) {
                array.push(_array[key]);
            }
        });
        return array;
    };
    ;
    return CService;
}());
CService = __decorate([
    core_1.Injectable()
], CService);
exports.CService = CService;
