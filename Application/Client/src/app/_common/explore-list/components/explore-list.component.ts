import { Component } from '@angular/core';

let styles = require('../styles/explore-list.component.scss').toString();
let tpls = require('../tpls/explore-list.component.html').toString();

@Component({
  selector: 'explore-list',
  styles : [ styles ],
  template : tpls
})

export class ExploreComponent  {

} 

