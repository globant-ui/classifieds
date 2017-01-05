import { Component } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../_common/services/setting.service';

let styles = require('../styles/home.component.scss').toString();
let tpls = require('../tpls/home.component.html').toString();

@Component({
  selector: 'home',
  styles : [ styles ],
  providers:[SettingsService],
  template : tpls
})

export class HomeComponent {
  private settings : any ;
  private baseUrl : any ;
  localState = { value: '' };
  constructor(public appState: AppState,private _settingsService: SettingsService) {}

  ngOnInit() {
    this.baseUrl=this._settingsService.getBaseUrl();
    console.log('------this-----',this.baseUrl);
    console.log('hello `Home` component');
  }

  submitState(value: string) {
    console.log('submitState', value);
    this.appState.set('value', value);
    this.localState.value = '';
  }
}
