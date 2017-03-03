import { Component,OnInit } from '@angular/core';
import {AuthenticationWindowService} from '../../authentication/services/authentication.service';
import {SettingsService} from '../../services/setting.service';
import {Base64Service} from '../../services/base64.service';

import {Session} from '../../authentication/entity/session.entity';
import {CookieService} from 'angular2-cookie/core';
import {Router} from '@angular/router';

let styles = require('../styles/header.component.scss').toString();
let tpls = require('../tpls/header.component.html').toString();

@Component({
  selector:'header',
  styles:[styles],
  providers:[AuthenticationWindowService, SettingsService, CookieService],
  template: tpls
})
export class HeaderComponent implements OnInit{

  public isCollapsed:boolean = true;
  private session : Session;
  private activeSession:boolean = false;

  constructor(public _authenticationWindowService: AuthenticationWindowService,
              private _cookieService:CookieService,
              private _route:Router,
              public _base64Service:Base64Service ){}

  ngOnInit(){
    this.session = new Session( this._cookieService.getObject( 'SESSION_PORTAL' ) );
    console.log("session email",this.session);
    this.activeSession = (this.session && this.session.isValid());
    if(!this.activeSession){
      this.doLogout();
    }
  }

    public collapsed(event:any):void {
        console.log(event);
    }

    public expanded(event:any):void {
        console.log(event);
    }

    showUserProfile(){
    let usermail =btoa(this.session.useremail);
    //this._base64Service.encode(usermail)
    this._route.navigate(['profile' ,usermail]);
    }

  doLogout(){
    this._authenticationWindowService.doLogOut();
  }


}

