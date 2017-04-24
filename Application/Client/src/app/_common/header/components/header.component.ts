import { Component, OnInit } from '@angular/core';
import { AuthenticationWindowService } from '../../authentication/services/authentication.service';
import { SettingsService } from '../../services/setting.service';
import { Base64Service } from '../../services/base64.service';

import { Session } from '../../authentication/entity/session.entity';
import { CookieService } from 'angular2-cookie/core';
import { Router } from '@angular/router';

@Component({
  selector: 'header',
  styles: [require('../styles/header.component.scss').toString()],
  providers: [AuthenticationWindowService, SettingsService, CookieService],
  template: require('../tpls/header.component.html').toString()
})
export class HeaderComponent implements OnInit {

  public isCollapsed: boolean = true;
  private session: Session;
  private activeSession: boolean = false;
  private wishlistFlag: boolean = false;
  private signOutHideShow: boolean = false;
  private userEmail: any;
  private userName: any;

  constructor(public _authenticationWindowService: AuthenticationWindowService,
              private _cookieService: CookieService,
              private _route: Router,
              public _base64Service: Base64Service ) {}

  public ngOnInit() {
    this.session = new Session( this._cookieService.getObject( 'SESSION_PORTAL' ) );
    console.log(this.session);
    this.userEmail = this.session['useremail'];
    let name =  this.userEmail.substring(0,  this.userEmail.indexOf('@'));
    this.userName = name.replace(/\./g, ' ');
    this.activeSession = (this.session && this.session.isValid());
    this._route.navigateByUrl('/dashboard/home');
    if (!this.activeSession) {
      this.doLogout();
    }
  }

    public collapsed(event: any): void {
       // Collapsed Method
    }

    public expanded(event: any): void {
      // Expanded Method
    }

    public showUserProfile() {
    let usermail = btoa(this.session['useremail']);
    this._route.navigateByUrl('/dashboard/profile/' + usermail);
  }

  public doLogout() {
    this._authenticationWindowService.doLogOut();
  }

  public wishlistPopup() {
    this.wishlistFlag = !this.wishlistFlag;
  }
 // profile button click
  public profileButton() {
    this.signOutHideShow = !this.signOutHideShow;
  }
}
