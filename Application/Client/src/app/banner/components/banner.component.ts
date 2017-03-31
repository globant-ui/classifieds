import { Component,Input,OnInit,HostListener,AfterViewInit,Renderer,ElementRef } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../_common/services/setting.service';
import {Router} from '@angular/router';

let styles = require('../styles/banner.component.scss').toString();
let tpls = require('../tpls/banner.component.html').toString();

@Component({
    selector: 'banner',
    styles : [ styles ],
    providers:[SettingsService],
    template : tpls
})
export class BannerComponent implements OnInit {
  private settings: any;
  private showSubCategory: boolean = true;
  private listingsData: any;
  localState = {value: ''};

  constructor(
    public appState: AppState, 
    private _settingsService: SettingsService,
    private renderer: Renderer,
    private elRef: ElementRef,
    private _router: Router,) {
  }

  @Input()
  categories;

  ngOnInit() {
    this.listingsData = this._settingsService.getBannerListingsData();
  }

  submitState(value: string) {
    console.log('submitState', value);
    this.appState.set('value', value);
    this.localState.value = '';
  }

  mouseHoverHandler() {
    alert("mouse hover");
    this.showSubCategory = true;
  }

  clickHandler() {
    alert("click");
    this.showSubCategory = true;
  }

  delegate(el, evt, handler) {
    el.addEventListener(evt, function () {
      handler.call(el);
    });
  }
  exploreSubCategory(sub){
     // sub = sub.replace(/\s+/g, '');
      sub.replace("", "-");
      this._router.navigateByUrl('/dashboard/exploreList/'+sub);
    console.log("sub",sub)
  }
}
