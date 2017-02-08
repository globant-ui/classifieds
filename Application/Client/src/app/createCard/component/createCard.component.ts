import { Component} from '@angular/core';
let tpls = require('../tpls/createCard.html').toString();
let styles = require('../styles/createCard.scss').toString();

@Component({
    selector:'create-card',
    template: tpls,
    styles: [styles]
})
export class CreateCardComponent {

    constructor(){
        console.log("comes here in create listing component");
    }
    
}

