import { Component,Output,EventEmitter } from '@angular/core';
import { AppState } from '../../../app.service';
import {SettingsService} from '../../services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../services/http.service';
import 'rxjs/Rx';

let styles = require('../styles/search.component.scss').toString();
let tpls = require('../tpls/search.component.html').toString();

@Component({
  selector:'search',
  styles:[styles],
  providers:[SettingsService, CService],
  template: tpls
})
export class SearchComponent{

    private searchUrl = 'http://in-it0289/Search_WAH/api/Search/GetFullTextSearch?searchText=';
    private searchCategoryByStringUrl:any;

    @Output() searchCategory: EventEmitter<any> = new EventEmitter<any>();

    constructor(public appState: AppState,private _settingsService: SettingsService,private _cservice:CService) {}

    searchCategoryByString(str){
        if(str.length>3){
            this.searchCategoryByStringUrl = this.searchUrl+str;
            console.log('search cat by str=',this.searchCategoryByStringUrl);
            this._cservice.observableGetHttp(this.searchCategoryByStringUrl,null,false)
                .subscribe((res:Response)=> {
                    console.log('res = ',res);
                    this.searchCategory.emit(res);
                },
                error => {
                    console.log("error in response");
                },
                ()=>{
                    console.log("Finally");
                })
        }
    }
}
