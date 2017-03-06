import { Component, OnInit} from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import {CService} from  '../../_common/services/http.service';
import {mapData} from  '../../mapData/mapData';
import { ActivatedRoute } from '@angular/router';
import {Base64Service} from '../../_common/services/base64.service';
import {apiPaths} from  '../../../serverConfig/apiPaths';
import {Response} from '@angular/http';

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
    private userEmail:any;
    private userDetails: any;
    private userProfileData : any = {};
    private tagData: any = [];
    private subscribeCat:any = [];
    private SubscribeSubCat:any = [];

    constructor(private _cservice:CService,
                private apiPath:apiPaths,
                private data:mapData,
                private _route:ActivatedRoute,
                public _base64service:Base64Service){
    }

    ngOnInit() {
      var self = this;
      this._route.params.subscribe(params => {
        //this._base64service.decode(params['usermail']);
        this.userEmail = atob(params['usermail']);
        this.getProfileData(this.userEmail);
      });

    }

    getProfileData(userMail){
      this.userDetails = this.GetUserProfileUrl+this.userEmail;
      this._cservice.observableGetHttp(this.userDetails ,null,false)
        .subscribe((res:Response)=> {
            this.userProfileData = res;
            this.subscribeCat = this.userProfileData.Alert;
            this.SubscribeSubCat =  this.userProfileData.Alert;
            this.tagData = this.userProfileData.Tags.SubCategory;
            console.log(this.tagData);
            console.log(this.userProfileData);
          },
          error => {
            console.log("error in response");
          },
          ()=>{
            console.log("Finally");
          })
    }

}

