
import { Component, ViewEncapsulation,ViewChildren,Input,OnInit} from '@angular/core';
import { AppState } from './app.service';
import  {SettingsService} from  './_common/services/setting.service';
import { TranslateService } from './translate';

@Component({
  selector: 'app',
  encapsulation: ViewEncapsulation.None,
  styleUrls: [
    './app.component.css'
  ],

  template: `<main>
           <router-outlet></router-outlet></main>`
})
export class AppComponent {
  angularclassLogo = 'assets/img/angularclass-avatar.png';
  name = 'Angular 2 Webpack Starter';
  url = 'https://twitter.com/AngularClass';

  ngOnInit() {
    // standing data
    this._translate.use('en');  
  }


  @ViewChildren("cheader") CHeader;

  constructor(public appState: AppState,private _translate: TranslateService) {

  }

}
