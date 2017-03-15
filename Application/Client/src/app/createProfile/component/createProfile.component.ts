import { Component, OnInit} from '@angular/core';
import {CService} from  '../../_common/services/http.service';
import {SettingsService} from '../../_common/services/setting.service';
import { ActivatedRoute } from '@angular/router';
import {apiPaths} from  '../../../serverConfig/apiPaths';
import {Response} from '@angular/http';

let tpls = require('../tpls/createProfile.html').toString();
let styles = require('../styles/createProfile.scss').toString();

@Component({
  selector:'create-profile',
  template: tpls,
  styles: [styles],
  providers: [apiPaths, SettingsService]
})
export class ProfileComponent implements OnInit {

  private GetUserProfileUrl = 'http://in-it0289/Userapi/api/user/GetUserProfile?userEmail=';
  private userEmail:any;
  private userDetails: any;
  private userProfileData : any = {};
  private tagData: any = [];
  private subscribeCat:any = [];
  private UserImage: any;
  private UserProfileImage = this._settingService.settings;

  constructor(private _cservice:CService,
              private _route:ActivatedRoute,
              private _settingService : SettingsService){
  }

  ngOnInit() {
    var self = this;
    this._route.params.subscribe(params => {
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
          this.tagData = this.userProfileData.Tags.SubCategory;
          this.UserImage = this.userProfileData.Image;
        },
        error => {
          console.log("error in response");
        },
        ()=>{
          console.log("Finally");
        })
  }
}

