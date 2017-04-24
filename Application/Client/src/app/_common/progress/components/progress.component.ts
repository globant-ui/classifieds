import { Component, OnInit, Input, SimpleChange } from '@angular/core';

@Component({
  selector: 'progress-bar',
  styles: [require('../styles/progress.component.scss').toString()],
  template: require('../tpls/progress.component.html').toString()
})
export class ProgressComponent {

  @Input()
  private endPoints;

  @Input()
  private isCompleted;

  @Input()
  private isActive: string;

  private showTick = '&#10003;';

  constructor() {
    console.log(this.endPoints);
    console.log(this.isCompleted);
    console.log(this.isActive);

  }

}
