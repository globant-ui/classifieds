import { Component,Input,OnInit,HostListener,AfterViewInit,Renderer,ElementRef } from '@angular/core';
import { AppState } from '../../app.service';
import { ActivatedRoute } from '@angular/router';
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
  localState = { value: '' };
  constructor(private _route: ActivatedRoute, public appState: AppState,private _settingsService: SettingsService,private renderer: Renderer,private elRef:ElementRef) {
    this._route.params.subscribe(params => {
      this.productId = +params['id'];
    });
  }

  @Input() showProductInfoPage;

  private productId;

  ngOnInit() {
    console.log("product info");

  }

}
