import { Component, Input } from '@angular/core';
import { SettingsService } from '../../services/setting.service';
import { CService } from  '../../services/http.service';

@Component({
  selector: 'loader',
  styles: [require('../styles/loader.component.scss').toString()],
  providers: [SettingsService, CService],
  template: require('../tpls/loader.component.html').toString()
})
export class LoaderComponent {
    @Input() private isLoading: boolean = false;
}
