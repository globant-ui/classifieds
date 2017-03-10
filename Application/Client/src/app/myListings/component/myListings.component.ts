import { Component, OnInit} from '@angular/core';
import {CService} from  '../../_common/services/http.service';

import {apiPaths} from  '../../../serverConfig/apiPaths';
import {Http, Headers} from '@angular/http';

let tpls = require('../tpls/myListings.component.html').toString();
let styles = require('../styles/myListings.component.scss').toString();

@Component({
    selector:'my-listings',
    template: tpls,
    styles: [styles],
    providers: []
})
export class MyListingsComponent implements OnInit {


    constructor(){
    }

    ngOnInit() {}


}

