import { Component, OnInit } from '@angular/core';
import { AuthenticationWindowService }
from '../../_common/authentication/services/authentication.service';
import { SettingsService } from '../../_common/services/setting.service';
import { Base64Service } from '../../_common/services/base64.service';
import { Session } from '../../_common/authentication/entity/session.entity';
import { CookieService } from 'angular2-cookie/core';
import { Router } from '@angular/router';

@Component({
  selector: 'dashboard',
  styles: [require('../styles/dashboard.component.scss').toString()],
  providers: [AuthenticationWindowService, SettingsService, CookieService],
  template: require('../tpls/dashboard.component.html').toString()
})
export class DashboardComponent implements OnInit {
  private isCollapsed: boolean = true;
  private session: Session;
  private activeSession: boolean = false;
  private signOutHideShow: boolean = false;

  constructor(
    public _authenticationWindowService: AuthenticationWindowService,
    private _cookieService: CookieService,
    private _route: Router,
    public _base64Service: Base64Service
    ) {}

  public ngOnInit() {
    this.session = new Session( this._cookieService.getObject( 'SESSION_PORTAL' ) );
    this.activeSession = (this.session && this.session.isValid());
  }
}
