import { Component,Input,OnInit,HostListener,AfterViewInit,Renderer,ElementRef } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../_common/services/setting.service';

let styles = require('../styles/profile.component.scss').toString();
let tpls = require('../tpls/profile.component.html').toString();

@Component({
  selector: 'profile',
  styles : [ styles ],
  providers:[SettingsService],
  template : tpls
})

export class ProfileComponent {
  private settings : any ;
  private showSubCategory:any;
  private listingsData : any ;
  localState = { value: '' };
  constructor(public appState: AppState,private _settingsService: SettingsService,private renderer: Renderer,private elRef:ElementRef) {}

  @Input()
  categories;

  ngOnInit() {
    console.log("in the profile component");
  }
  submitState(value: string) {
    console.log('submitState', value);
    this.appState.set('value', value);
    this.localState.value = '';
  }
