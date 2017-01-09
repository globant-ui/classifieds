import { Component } from '@angular/core';
import { AppState } from '../../../app.service';

let styles = require('../styles/search.component.scss').toString();
let tpls = require('../tpls/search.component.html').toString();

@Component({
  selector:'search',
  styles:[styles],
  template: tpls
})
export class SearchComponent{
    searchCategory(str){
        console.log('search cat by str=',str);
    }
}
