import { Component} from '@angular/core';

let styles = require('../styles/login.scss').toString();
let tpls = require('../tpls/createListing.html').toString();

@Component({
    selector:'create-listing',
    styles:[styles],
    template: tpls
})
export class CreateListingComponent {

    constructor(){
        console.log("comes here in create listing component");
    }
    
}

