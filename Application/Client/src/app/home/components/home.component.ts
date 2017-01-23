import { Component } from '@angular/core';
import { AppState } from '../../app.service';

let styles = require('../styles/home.component.scss').toString();
let tpls = require('../tpls/home.component.html').toString();

@Component({
  selector: 'home',
  styles : [ styles ],
  template : tpls
})

export class HomeComponent {
  localState = { value: '' };
  constructor(public appState: AppState) {

  }

  ngOnInit() {
    console.log('hello `Home` component');
  }

  submitState(value: string) {
    console.log('submitState', value);
    this.appState.set('value', value);
    this.localState.value = '';
  }
}
