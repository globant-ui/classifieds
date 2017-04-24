import { Component, Output, EventEmitter } from '@angular/core';
import { AppState } from '../../../app.service';
import { SettingsService } from '../../services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response, RequestOptions } from '@angular/http';
import { CService } from  '../../services/http.service';
import 'rxjs/Rx';

@Component({
  selector: 'search',
  styles: [ require('../styles/search.component.scss').toString() ],
  providers: [SettingsService, CService],
  template: require('../tpls/search.component.html').toString()
})
export class SearchComponent {

    @Output() private searchCategory: EventEmitter<any> = new EventEmitter<any>();
    @Output() private getSelectedFilter: EventEmitter<any> = new EventEmitter<any>();
    private searchUrl = '';
    private searchAutoSuggestionUrl = '';
    private searchCategoryByStringUrl: any;
    private searchCategorySuggestionUrl: string;
    private delayTimer: any = null;
    private searchResult;
    private searchCategoryByStr: string = '';
    private enabledDropdown: boolean = false;
    private isLoading: boolean = false;

    constructor(
        public appState: AppState,
        private _settingsService: SettingsService,
        private _cservice: CService
    ) {
        this.searchUrl = _settingsService.getPath('searchUrl');
        this.searchAutoSuggestionUrl = _settingsService.getPath('searchAutoSuggestionUrl');
    }

    private fetchSearchedData( text: string ) {
        this.isLoading = true;
        this._cservice.observableGetHttp( this.searchCategoryByStringUrl, null, false )
            .subscribe((res: Response) => {
                this.isLoading = false;
                if (res && res[ 'length' ] > 0) {
                    this.searchResult = res;
                    this.enabledDropdown = true;
                }else {
                    console.log('No result found!!!');
                }
            },
            (error) => {
                console.log('error in response', error);
                this.isLoading = false;
            });
    }

    private searchCategoryByString( str: string) {
        if (str.length >= 3) {
            let self = this;
            this.searchCategoryByStringUrl = this.searchUrl + str;
            this._cservice.observableGetHttp( this.searchCategoryByStringUrl, null, false )
                .subscribe((res: Response) => {
                    let obj = { categoryName: str, result: res };
                    this.searchCategory.emit(obj);
                    self.searchCategoryByStr = '';
                },
                (error) => {
                    console.log('error in response', error);
                });
        }
    }

    private selectOption(selectedOpt) {
        this.searchCategoryByStr = selectedOpt;
        this.enabledDropdown = false;
        this.searchCategoryByString(selectedOpt);
        this.getSelectedFilter.emit(selectedOpt);
    }

    private setFilter(filterData) {
        // this.searchCategoryByStr = filterData;
    }
}
