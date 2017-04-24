import { Component, Input, OnInit, HostListener, AfterViewInit,
   Renderer, ElementRef } from '@angular/core';
import { AppState } from '../../app.service';
import { SettingsService } from '../../_common/services/setting.service';
import { Router } from '@angular/router';

@Component({
    selector: 'banner',
    styles: [ require('../styles/banner.component.scss').toString() ],
    providers: [SettingsService],
    template: require('../tpls/banner.component.html').toString()
})
export class BannerComponent implements OnInit {
  private settings: any;
  private showSubCategory: boolean = true;
  private listingsData: any;
  private localState = {value: ''};
  @Input() private categories;
  constructor(
    public appState: AppState,
    private _settingsService: SettingsService,
    private renderer: Renderer,
    private elRef: ElementRef,
    private _router: Router) {
  }

  public ngOnInit() {
    this.listingsData = this._settingsService.getBannerListingsData();
  }

  private submitState(value: string) {
    console.log('submitState', value);
    this.appState.set('value', value);
    this.localState.value = '';
  }

  private mouseHoverHandler() {
    this.showSubCategory = true;
  }

  private clickHandler() {
    this.showSubCategory = true;
  }

  private delegate(el, evt, handler) {
    el.addEventListener(evt, () => {
      handler.call(el);
    });
  }

  // banner click subCategory route
  private exploreSubCategory( sub ) {
    sub = sub.replace(/\s+/g, '');
    sub = sub.replace('/', '-');
    this._router.navigateByUrl('/dashboard/exploreList/' + sub);
  }

  // explore click route
  private exploreCategory( subCategoryMain ) {
    this._router.navigateByUrl('/dashboard/exploreList/' + subCategoryMain);
  }
}
