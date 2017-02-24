import { Component,Input,OnInit,HostListener,AfterViewInit,Renderer,ElementRef } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../_common/services/setting.service';

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
  private showSubCategory: any;
  private listingsData: any;
  localState = {value: ''};

  constructor(public appState: AppState, private _settingsService: SettingsService, private renderer: Renderer, private elRef: ElementRef) {
  }

  @Input()
  categories;

  ngOnInit() {
    this.listingsData = this._settingsService.getBannerListingsData();
    console.log('dsf = ', this.listingsData);
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

  @HostListener('window:resize', ['$event'])
  onResize(event) {
    let that = this;
    let size = event.target.innerWidth;

    if (size > 450) {
      this.renderer.listen(this.elRef.nativeElement, 'mouseover', (event) => {
        this.showSubCategory = true;
      });
    } else {
      this.renderer.listen(this.elRef.nativeElement, 'click', (event) => {
        this.showSubCategory = true;
      });
    }

  }
}
