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

  @Input() showProductInfoPage;

  private productId : any;
  private productInfoData: any;
  private productDetails: any;
  //private productData: any;
  private productInfoUrl = 'http://in-it0289/ListingAPI/api/Listings/GetListingById?id=';

  constructor(private _route: ActivatedRoute,
              public appState: AppState,
              private _settingsService: SettingsService,
              private renderer: Renderer,
              private elRef:ElementRef,
              public  _cservice:CService) {

    this._route.params.subscribe(params => {
      this.productId = params['id'];
      console.log(this.productId);
    });
  }

  ngOnInit() {
  }

  ngAfterViewInit(){
    this.getProductInfo();
  }

  getProductInfo (){
    this.productDetails = this.productInfoUrl+this.productId;
    this._cservice.observableGetHttp(this.productDetails ,null,false)
      .subscribe((res:Response)=> {
          this.productInfoData = res;
          //this.productInfoData =this.productData;
          console.log(this.productInfoData);
        },
        error => {
          console.log("error in response");
        },
        ()=>{
          console.log("Finally");
        })
  }

  


}
