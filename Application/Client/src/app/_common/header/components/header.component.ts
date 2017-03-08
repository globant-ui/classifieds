import { Component,OnInit } from '@angular/core';
import {AuthenticationWindowService} from '../../authentication/services/authentication.service';
import {SettingsService} from '../../services/setting.service';
import {Session} from '../../authentication/entity/session.entity';
import {CookieService} from 'angular2-cookie/core';

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
  private UserFirstName: string = "";
  private UserProfileButton: boolean = false;

  constructor(public _authenticationWindowService: AuthenticationWindowService,
              private _cookieService:CookieService){}

  ngOnInit(){
    this.session = new Session( this._cookieService.getObject( 'SESSION_PORTAL' ) );
    //make 1st word capital n use only name
    this.UserFirstName = this.session['useremail'];
    this.UserFirstName = this.UserFirstName.charAt(0).toUpperCase() + this.UserFirstName.slice(1).split(".")[0];
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

  doLogout(){
    this._authenticationWindowService.doLogOut();
  }

  UserProfileNameButton(){
  this.UserProfileButton = !this.UserProfileButton;
  }

}

