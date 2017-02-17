import { Component,OnInit,Input,OnChanges,SimpleChange } from '@angular/core';

let styles = require('../styles/progress.component.scss').toString();
let tpls = require('../tpls/progress.component.html').toString();

@Component({
  selector:'progress-bar',
  styles:[styles],
  template: tpls
})
export class ProgressComponent implements OnChanges{

  @Input()
  endPoints;
  
  @Input()
  isCompleted;
  
  @Input()
  isActive:string;
  
  public showTick = '&#10003;';
  
  constructor(){
    console.log(this.endPoints);
    console.log(this.isCompleted);
    console.log(this.isActive);

  }

  ngOnChanges(changes: {[propKey: string]: SimpleChange}){
    console.log("changes");
    console.log(changes);
    
  }

    
}

