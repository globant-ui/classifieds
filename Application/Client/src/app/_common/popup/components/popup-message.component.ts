import { Component, AfterViewInit, Input, ViewChild, OnChanges, SimpleChange }
from '@angular/core';
import { AppState } from '../../app.service';
import { SettingsService } from '../../services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response, RequestOptions } from '@angular/http';
import { CService } from  '../../services/http.service';
import { CookieService } from 'angular2-cookie/core';
import 'rxjs/Rx';
import { Session } from '../../authentication/entity/session.entity';
import { ModalDirective } from 'ng2-bootstrap/modal';
import { Router } from '@angular/router';

// using jquery syntax $
declare var $;

@Component({
  selector: 'popup-message',
  styles : [ require('../styles/popup-message.component.scss').toString() ],
  providers: [SettingsService, CService],
  template : require('../tpls/popup-message.component.html').toString()
})

export class PopUpMessageComponent implements OnChanges, AfterViewInit {

@Input() public showPopupDivMessage;
@ViewChild('childModal') public childModal1: ModalDirective;

constructor(public _route: Router) {}

public ngAfterViewInit() {
    this.showChildModal();
  }

public showChildModal(): void {
   this.childModal1.show();
 }

public viewListingRedirect() {
   this._route.navigate(['dashboard/home']);
 }

public redirectToMyProfile() {
    this._route.navigateByUrl('/dashboard/home');
  }

public ngOnChanges(changes: {[propKey: string]: SimpleChange}) {
   console.log(changes);
 }

}
