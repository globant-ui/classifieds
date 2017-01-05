import {Injectable} from '@angular/core';

var settingsJson = require("app/settings.json");

@Injectable()
export class SettingsService{
  public  settings : any ;

  constructor() {
  }

  getSettings(){
    return settingsJson;
  }

  getBaseUrl(){
    return settingsJson.services.main;
  }
}



