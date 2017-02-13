import { Component,Input,OnInit,HostListener,AfterViewInit,Renderer,ElementRef } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../_common/services/setting.service';

let styles = require('../styles/product-info.component.scss').toString();
let tpls = require('../tpls/product-info.html').toString();

@Component({
  selector: 'product-info',
  styles : [ styles ],
  providers:[SettingsService],
  template : tpls
})

export class ProductInfoComponent {
  private settings : any ;
  private showSubCategory:any;
  private listingsData : any ;
  localState = { value: '' };
  constructor(public appState: AppState,private _settingsService: SettingsService,private renderer: Renderer,private elRef:ElementRef) {}

  @Input() showProductInfoPage;

  ngOnInit() {
    console.log("product info");
  }

}
