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
  private session : Session;
  private activeSession:boolean = false;

  constructor(public _authenticationWindowService: AuthenticationWindowService){}

  ngOnInit(){
    this.activeSession = (this.session && this.session.isValid());
    if(!this.activeSession){
      this.doLogout();
    }
  }

  doLogout(){
    this._authenticationWindowService.doLogOut();
  }


}

