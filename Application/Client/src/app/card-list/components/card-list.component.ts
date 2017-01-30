import { Component } from '@angular/core';
import { AppState } from '../../app.service';

let styles = require('../styles/card-list.component.scss').toString();
let tpls = require('../tpls/card-list.component.html').toString();

@Component({
    selector : 'card-list',
    styles : [styles],
    template : tpls
})
export class CardListComponent{

}