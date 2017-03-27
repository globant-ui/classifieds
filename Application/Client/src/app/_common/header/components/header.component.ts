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
  private wishlistFlag: boolean = false;
  private signOutHideShow: boolean = false;
  private userEmail: any;
  private userName:any;


  constructor(public _authenticationWindowService: AuthenticationWindowService,
              private _cookieService:CookieService,
              private _route:Router,
              public _base64Service:Base64Service ){}

  ngOnInit(){
    this.session = new Session( this._cookieService.getObject( 'SESSION_PORTAL' ) );
    console.log(this.session);
    this.userEmail = this.session.useremail;
    var name =  this.userEmail.substring(0,  this.userEmail.indexOf("@"));
    this.userName = name.replace(/\./g, ' ');
    this.activeSession = (this.session && this.session.isValid());
    this._route.navigateByUrl('/dashboard/home');
    if(!this.activeSession){
      this.doLogout();
    }
  }

    public collapsed(event:any):void {}

    public expanded(event:any):void {}

    showUserProfile(){
    let usermail =btoa(this.session.useremail);
    this._route.navigateByUrl('/dashboard/profile/'+usermail);
    }


  doLogout(){
    this._authenticationWindowService.doLogOut();
  }

  wishlistPopup() {
    this.wishlistFlag = !this.wishlistFlag;
  }
 //profile button click
  profileButton() {
    this.signOutHideShow = !this.signOutHideShow;
  }

}