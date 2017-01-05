
import { Component, ViewEncapsulation } from '@angular/core';
import { AppState } from './app.service';
import  {SettingsService} from  './_common/services/setting.service';


@Component({
  selector: 'app',
  encapsulation: ViewEncapsulation.None,
  styleUrls: [
    './app.component.css'
  ],
  template: `
    <nav>
      <span>
        <a [routerLink]=" ['./home'] ">
          Home
        </a>
      </span>
    </nav>

    <main>
      <router-outlet></router-outlet>
    </main>

    <!--<pre class="app-state">this.appState.state = {{ appState.state | json }}</pre>-->

    <!--<footer>-->
      <!--<span>WebPack Angular 2 Starter by <a [href]="url">@AngularClass</a></span>-->
      <!--<div>-->
        <!--<a [href]="url">-->
          <!--<img [src]="angularclassLogo" width="25%">-->
        <!--</a>-->
      <!--</div>-->
    <!--</footer>-->
  `
})
export class AppComponent {
  angularclassLogo = 'assets/img/angularclass-avatar.png';
  name = 'Angular 2 Webpack Starter';
  url = 'https://twitter.com/AngularClass';

  constructor(public appState: AppState) {

  }

  ngOnInit() {
    console.log('Initial App State', this.appState.state);
  }

}
