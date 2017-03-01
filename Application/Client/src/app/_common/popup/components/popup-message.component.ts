import { Component,OnInit, AfterViewInit  } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../services/http.service';
import {CookieService} from 'angular2-cookie/core';
import 'rxjs/Rx';
import {Session} from '../../authentication/entity/session.entity';

//using jquery syntax $
declare var $;

let styles = require('../styles/popup-message.component.scss').toString();
let tpls = require('../tpls/popup-message.component.html').toString();

@Component({
  selector: 'popup-message',
  styles : [ styles ],
  providers:[SettingsService, CService],
  template : tpls
})

export class PopUpMessageComponent implements OnInit   {

ngOnInit(){}

} 
