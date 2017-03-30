import { Component,OnInit, AfterViewInit,Input,ViewChild,OnChanges,SimpleChange } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../services/http.service';
import {CookieService} from 'angular2-cookie/core';
import 'rxjs/Rx';
import {Session} from '../../authentication/entity/session.entity';
import { ModalDirective } from 'ng2-bootstrap/modal';
import {Router} from '@angular/router';

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

export class PopUpMessageComponent implements OnChanges {

@ViewChild('childModal') public childModal1:ModalDirective;

@Input() showPopupDivMessage;

constructor(private _route:Router){}

ngOnInit() {
}

ngOnChanges(changes: {[propKey: string]: SimpleChange}){
   console.log(changes);
 }

 ngAfterViewInit(){
    this.showChildModal();
  }

 public showChildModal(): void {
   this.childModal1.show();
 }
 
 viewListingRedirect() {
   this._route.navigate(['dashboard/home']);
 }
  redirectToMyProfile(){
    this._route.navigateByUrl('/dashboard/home');
  }
}
