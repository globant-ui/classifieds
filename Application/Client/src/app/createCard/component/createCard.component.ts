import { Component} from '@angular/core';
let tpls = require('../tpls/createCard.html').toString();

@Component({
    selector:'create-card',
    template: tpls
})
export class CreateCardComponent {

    constructor(){
        console.log("comes here in create listing component");
    }
    
}

