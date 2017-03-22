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
  selector: 'search',
  styles: [styles],
  providers: [SettingsService, CService],
  template: tpls
})
export class SearchComponent {

    @Output() searchCategory: EventEmitter<any> = new EventEmitter<any>();
    @Output() getSelectedFilter: EventEmitter<any> = new EventEmitter<any>();

    private searchUrl = '';
    private searchAutoSuggestionUrl = "";
    private searchCategoryByStringUrl:any;
    private searchCategorySuggestionUrl:string;
    private delayTimer  : any = null;
    private searchResult;
    private searchCategoryByStr: string = '';
    private enabledDropdown:boolean = false; 
    private isLoading : boolean = false;

    constructor( public appState: AppState,
                 private _settingsService: SettingsService,
                 private _cservice:CService) {
                     this.searchUrl = _settingsService.getPath('searchUrl');
                     this.searchAutoSuggestionUrl =_settingsService.getPath('searchAutoSuggestionUrl');
                 }

    onKeyUp( event: any, text: string ) {
        if (text.length >= 3) {
            this.searchCategoryByStringUrl = this.searchAutoSuggestionUrl + text;
            //Delay of some time to slow down the results
            clearTimeout(this.delayTimer);

            this.delayTimer = setTimeout(() => {
                this.fetchSearchedData(text);
            }, 1000);
        }
    }

    fetchSearchedData( text: string ) {
        this.isLoading = true;  ;
        this._cservice.observableGetHttp( this.searchCategoryByStringUrl, null, false )
            .subscribe((res: Response) => {
                this.isLoading = false;
                if (res && res[ 'length' ] > 0) {
                    this.searchResult = res;
                    this.enabledDropdown = true;
                } else {
                    console.log("No result found!!!");
                }
            },
            error => {
                console.log("error in response",error);
                 this.isLoading = false;
            });
    }
  
    searchCategoryByString( str: string) {
        if (str.length >= 3) {
            this.searchCategoryByStringUrl = this.searchUrl + str;
            this._cservice.observableGetHttp( this.searchCategoryByStringUrl, null, false )
                .subscribe((res: Response) => {
                    let obj = { 'categoryName': str, 'result': res };
                    this.searchCategory.emit(obj.categoryName);
                },
                error => {
                    console.log("error in response",error);
                });
        }
    }

    selectOption(selectedOpt) {
        this.searchCategoryByStr = selectedOpt;
        this.enabledDropdown = false;
        this.searchCategoryByString(selectedOpt);
        this.getSelectedFilter.emit(selectedOpt);
    }

    setFilter(filterData) {
        //this.searchCategoryByStr = filterData;
    }
}
