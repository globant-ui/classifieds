import { Component,ViewChild,OnInit, AfterViewInit  } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../services/http.service';
import {CookieService} from 'angular2-cookie/core';
import 'rxjs/Rx';
import {Session} from '../../authentication/entity/session.entity';
import { ModalDirective } from 'ng2-bootstrap/modal';

let styles = require('../styles/wishlist.component.scss').toString();
let tpls = require('../tpls/wishlist.component.html').toString();

@Component({
  selector: 'wishlist',
  styles : [ styles ],
  template : tpls
})

export class WishListComponent implements OnInit, AfterViewInit  {

@ViewChild('childModal') public childModal:ModalDirective;

  constructor(
                 private _settingsService: SettingsService,
                 private _cservice:CService,
                 private _cookieService:CookieService) {
                        //this.subCategoryUrl = _settingsService.getPath('subCategoryUrl');
                        //this.selectInterestUrl = _settingsService.getPath('searchUrl');
                 }

  ngOnInit() {
     
  }

  ngAfterViewInit() {
    this.showChildModal();
  }

  public showChildModal(): void {
    this.childModal.show();
  }

  public hideChildModal(): void {
    this.childModal.hide();
  }

} 

