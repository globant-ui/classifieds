import { Component } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../_common/services/setting.service';

let styles = require('../styles/filter.component.scss').toString();
let tpls = require('../tpls/filter.component.html').toString();

@Component({
  selector: 'filter',
  styles : [ styles ],
  providers:[SettingsService],
  template : tpls
})

export class FilterComponent {

  private filterData : any;

  constructor(public appState: AppState,private _settingsService: SettingsService) {}

  ngOnInit()
  {
    this.filterData=this._settingsService.getFilterListingData();
    console.log('filter',this.filterData);

  }

  showCards(){

  }
}
