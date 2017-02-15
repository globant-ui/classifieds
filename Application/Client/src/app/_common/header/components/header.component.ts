import { Component,OnInit } from '@angular/core';
import {AuthenticationWindowService} from '../../authentication/services/authentication.service';
import {SettingsService} from '../../services/setting.service';

let styles = require('../styles/header.component.scss').toString();
let tpls = require('../tpls/header.component.html').toString();

@Component({
  selector:'header',
  styles:[styles],
  providers:[AuthenticationWindowService, SettingsService],
  template: tpls
})
export class HeaderComponent implements OnInit{

  public isCollapsed:boolean = true;
  constructor(public _authenticationWindowService: AuthenticationWindowService){}

  ngOnInit(){}

  doLogout(){
    this._authenticationWindowService.doLogOut();
  }


}

