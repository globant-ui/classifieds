import { Component,Input } from '@angular/core';
import {SettingsService} from '../../services/setting.service';
import {CService} from  '../../services/http.service';


let styles = require('../styles/loader.component.scss').toString();
let tpls = require('../tpls/loader.component.html').toString();

@Component({
  selector: 'loader',
  styles: [styles],
  providers: [SettingsService, CService],
  template: tpls
})
export class LoaderComponent {

    @Input() isLoading : boolean = false;

    constructor() {}
}
