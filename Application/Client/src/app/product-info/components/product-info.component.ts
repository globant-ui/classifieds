import { Component,Input,OnInit,HostListener,AfterViewInit,Renderer,ElementRef } from '@angular/core';
import { AppState } from '../../app.service';
import { ActivatedRoute } from '@angular/router';
import {SettingsService} from '../../_common/services/setting.service';
import {CService} from  '../../_common/services/http.service';
import { Http, Response,RequestOptions } from '@angular/http';


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
  constructor(private _route: ActivatedRoute,
              public appState: AppState,
              private _settingsService: SettingsService,
              private renderer: Renderer,
              private elRef:ElementRef,
              public  _cservice:CService) {

    this._route.params.subscribe(params => {
      this.productId = +params['id'];
    });
  }

  @Input() showProductInfoPage;

  private productId : number;
  private productInfoData: any;
  private ProductInfoUrl = 'http://in-it0289/ListingAPI/api/Listings/GetListingById?id=';

  ngOnInit() {
    console.log("product info");

  }

  getInitialCards (){
    this._cservice.observableGetHttp(this.ProductInfoUrl +this.productId ,null,false)
      .subscribe((res:Response)=> {
          this.productInfoData = res;
            console.log('-----------------',this.productInfoData);
        },
        error => {
          console.log("error in response");
        },
        ()=>{
          console.log("Finally");
        })
  }


}
