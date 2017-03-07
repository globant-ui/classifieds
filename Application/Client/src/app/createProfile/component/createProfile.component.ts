import { Component, OnInit} from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import {CService} from  '../../_common/services/http.service';
import {mapData} from  '../../mapData/mapData';

import {apiPaths} from  '../../../serverConfig/apiPaths';
import {Http, Headers} from '@angular/http';

let tpls = require('../tpls/createProfile.html').toString();
let styles = require('../styles/createProfile.scss').toString();

@Component({
    selector:'create-profile',
    template: tpls,
    styles: [styles],
    providers: [apiPaths]
})
export class ProfileComponent implements OnInit {

    private GetUserProfileUrl = 'http://in-it0289/Userapi/api/user/GetUserProfile?userEmail=';

    constructor(private httpService:CService,private apiPath:apiPaths,private data:mapData){
    }

    ngOnInit() {
        this.getProfileData();
    }

    getProfileData(){

    }


}

