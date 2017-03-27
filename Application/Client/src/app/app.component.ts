
import { Component, ViewEncapsulation,ViewChildren,Input} from '@angular/core';
import { AppState } from './app.service';
import  {SettingsService} from  './_common/services/setting.service';


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

@Input() data1;

  @ViewChildren("cheader") CHeader;

  constructor(public appState: AppState) {

  }


  ngOnInit() {
console.log("main app",this.data1)
  }

}
