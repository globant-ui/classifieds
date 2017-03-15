import { Component,Input,OnInit } from '@angular/core';
import { AppState } from '../../app.service';
import { SettingsService } from '../../_common/services/setting.service';
import { LoaderComponent } from '../../_common/loader/components/loader.component';
import {Router} from '@angular/router';

let styles = require('../styles/card-list.component.scss').toString();
let tpls = require('../tpls/card-list.component.html').toString();

@Component({
    selector : 'card-list',
    styles : [styles],
    providers:[SettingsService],
    template : tpls
})
export class CardListComponent{

    public showProductInfoPage: boolean = false;
    constructor(public appState: AppState, private _settingsService : SettingsService, private _router:Router) {}

    @Input() cards;

    private isLoading:boolean = false;

    ngOnInit() {}

    loading( flag ) {
        this.isLoading = flag;
    }

   showProductInfo(id){
     this._router.navigate(['productInfo',id]);
  }
}
