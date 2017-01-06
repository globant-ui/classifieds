import { Component,Input,OnInit } from '@angular/core';
import { AppState } from '../../app.service';
import { SettingsService } from '../../_common/services/setting.service';

let styles = require('../styles/card-list.component.scss').toString();
let tpls = require('../tpls/card-list.component.html').toString();

@Component({
    selector : 'card-list',
    styles : [styles],
    providers:[SettingsService],
    template : tpls
})
export class CardListComponent{
  //  private cardListings : any;
    private initialCards:any;
    constructor(public appState: AppState,private _settingsService : SettingsService) {}

    @Input() cardListings;

      ngOnInit()
      {
        console.log('in the card list component',this.cardListings);
      }

}

