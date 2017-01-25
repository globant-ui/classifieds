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

    private searchUrl = 'http://in-it0289/SearchAPI/api/Search/GetFullTextSearch?searchText=';
    private searchAutoSuggestionUrl = "http://in-it0289/MasterDataAPI/api/category/GetCategorySuggetion?categoryText=";
    private searchCategoryByStringUrl:any;
    private searchCategorySuggestionUrl:string;
    private delayTimer  : any = null;
    private searchResult;
    private searchCategoryByStr: string = '';

    @Output() searchCategory: EventEmitter<any> = new EventEmitter<any>();

    constructor(public appState: AppState,private _settingsService: SettingsService,private _cservice:CService) {}

     values = '';
  onKey(event:any,text: string) {
      
    this.values += text ;
   text = text.charAt(0).toUpperCase() + text.slice(1);
    console.log('hello keypress',this.values)
    console.log("event.target.value",event.target.value);
    if (text.length >= 3) {
        this.searchCategoryByStringUrl = this.searchAutoSuggestionUrl+text;
        //Delay of some time to slow down the results
        clearTimeout(this.delayTimer);
        
        this.delayTimer = setTimeout(() => {
            this.fetchSearchedData(text);
        }, 1000);   
    }
  }

  fetchSearchedData(text: string) {
    console.log("in fetchSearchedData")
    this._cservice.observableGetHttp(this.searchCategoryByStringUrl,null,false)
    .subscribe((res:Response)=> {
        if( res && res.length > 0 ){
            console.log('resAutoSuggestion = ',res[0]);
            this.searchResult = res;
        } else {
            console.log("No result found!!!");
        }   
       // this.searchCategory.emit(res);
    },
    error => {
        console.log("error in response");
    },
    ()=>{
        console.log("Finally");
    })
}
  
    searchCategoryByString(str){
        if(str.length>=3){
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

    selectOption( selectedOpt ) {
        this.searchCategoryByStr = selectedOpt;
        //hide dropdown
        this.searchCategoryByString(selectedOpt);
    }
}
