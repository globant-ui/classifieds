import { Component,Output,EventEmitter,Input,OnChanges,SimpleChanges } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../_common/services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../_common/services/http.service';
import 'rxjs/Rx';

let styles = require('../styles/filter.component.scss').toString();
let tpls = require('../tpls/filter.component.html').toString();

@Component({
  selector: 'filter',
  styles : [ styles ],
  providers:[SettingsService, CService],
  template : tpls
})

export class FilterComponent {

  private filterData : any;
  private filterCategoryUrl = 'http://in-it0289/ListingAPI/api/Listings/GetListingsByCategory?Category=';
  private categoryUrl:any;
  public filterCategoryData:any;
  public isActive:boolean = false;

  @Input() selectedFilter;
  @Output() getSelectedFilterOption: EventEmitter<any> = new EventEmitter <any>();

  constructor(public appState: AppState,private _settingsService: SettingsService,private _cservice:CService) {}

  ngOnInit()
  {
    this.filterData = this._settingsService.getFilterListingData();
   this.filterData = this.filterData;
    for (let item of this.filterData) {
      if(item.listName==='Top ten'){item.isActive = true;}
    }
  }

  ngOnChanges(changes: SimpleChanges) { 
    if( changes[ 'selectedFilter' ] ) {
      this.updateSelectedFilter();
    }
  }

  updateSelectedFilter() {
    if( this.filterData ) {
      for (let item of this.filterData ) {
        if( item.listName.toLowerCase() == this.selectedFilter.toLowerCase() ) {
          item.isActive = true;
        } else {
          item.isActive = false;
        }
      }
    }
  }

  selectedOption(category, index) {
     this.filterData = this.filterData;
    
    for (let item of  this.filterData) {
      item.isActive = false;
    }
     this.filterData[index].isActive = true;
    this.getSelectedFilterOption.emit( category );
  }

  getCardsByCategory(category)
  {
    this.categoryUrl = this.filterCategoryUrl+category;
    this._cservice.observableGetHttp(this.categoryUrl,null,false)
      .subscribe((res:Response)=> {
          //console.log('res = ',res);
          //this.filterCategory.emit(res);
        },   error => {
          console.log("error in response");
        },
        ()=>{
          console.log("Finally");
        })
  }}

