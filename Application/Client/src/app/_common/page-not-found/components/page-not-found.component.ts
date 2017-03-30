import { Component } from '@angular/core';

let styles = require('../styles/page-not-found.component.scss').toString();
let tpls = require('../tpls/page-not-found.component.html').toString();

@Component({
  selector: 'page-not-found',
  styles : [ styles ],
  template : tpls
})

export class PageNotFoundComponent   {

  constructor(){}

}

