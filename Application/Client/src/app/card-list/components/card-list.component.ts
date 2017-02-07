import { Component,Input,OnInit } from '@angular/core';
import { AppState } from '../../app.service';
import { SettingsService } from '../../_common/services/setting.service';
import { LoaderComponent } from '../../_common/loader/components/loader.component';

let styles = require('../styles/card-list.component.scss').toString();
let tpls = require('../tpls/card-list.component.html').toString();

@Component({
    selector : 'card-list',
    styles : [styles],
    providers:[SettingsService],
    template : tpls
})
export class CardListComponent{
    constructor(public appState: AppState,private _settingsService : SettingsService) {}

    @Input() cards;

    private isLoading:boolean = false;
    
    ngOnInit() {
        console.log('all = ',this.cards);
    }
        
    loading( flag ) {
        this.isLoading = flag;
    }
}
