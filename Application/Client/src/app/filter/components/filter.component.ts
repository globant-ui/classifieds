import { Component, Output, EventEmitter, Input, OnChanges, SimpleChanges, OnInit }
from '@angular/core';
import { AppState } from '../../app.service';
import { SettingsService } from '../../_common/services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response, RequestOptions } from '@angular/http';
import { CService } from  '../../_common/services/http.service';
import 'rxjs/Rx';

@Component({
  selector: 'filter',
  styles : [ require('../styles/filter.component.scss').toString() ],
  providers: [SettingsService, CService],
  template : require('../tpls/filter.component.html').toString()
})

export class FilterComponent implements OnChanges, OnInit {
  public filterCategoryData: any;
  public isActive: boolean = false;
  private filterData: any;
  private filterCategoryUrl: string = '';
  private categoryUrl: any;
  @Input() private selectedFilter;
  @Input() private isSearchActive;
  @Input() private searchValue;
  @Output() private getSelectedFilterOption: EventEmitter<any> = new EventEmitter <any>();

  constructor(public appState: AppState,
              private _settingsService: SettingsService,
              private _cservice: CService) {
                this.filterCategoryUrl = _settingsService.getPath('filterCategoryUrl');
              }
  public ngOnInit() {
    this.filterData = this._settingsService.getFilterListingData();
    this.filterData = this.filterData;
    for (let item of this.filterData) {
      if (item.listName === 'Top ten') {
        item.isActive = true;
      }
    }
  }

  public ngOnChanges(changes: SimpleChanges) {
    if ( changes[ 'selectedFilter' ] ) {
      this.updateSelectedFilter();
    }
  }

  private updateSelectedFilter() {
    if ( this.filterData ) {
      for (let item of this.filterData ) {
        if ( item.listName.toLowerCase() === this.selectedFilter.toLowerCase() ) {
          item.isActive = true;
        }else {
          item.isActive = false;
        }
      }
    }
  }

  private selectedOption(category, index) {
    this.filterData = this.filterData;
    for (let item of this.filterData) {
      item.isActive = false;
    }
    this.filterData[index].isActive = true;
    this.getSelectedFilterOption.emit( {categoryName: category} );
  }

  private getCardsByCategory(category) {
    this.categoryUrl = this.filterCategoryUrl + category;
    this._cservice.observableGetHttp(this.categoryUrl, null, false)
      .subscribe((res: Response) => {
          // console.log('res = ',res);
          // this.filterCategory.emit(res);
        },
        (error) => {
          console.log('error in response');
        },
        () => {
          console.log('Finally');
        });
  }}
