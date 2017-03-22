import { Component, OnInit} from '@angular/core';
import {CService} from  '../../_common/services/http.service';
import {SettingsService} from '../../_common/services/setting.service';
import { ActivatedRoute } from '@angular/router';
import {Base64Service} from '../../_common/services/base64.service';
import {apiPaths} from  '../../../serverConfig/apiPaths';
import {Http,Response,Headers} from '@angular/http';

let tpls = require('../tpls/createProfile.html').toString();
let styles = require('../styles/createProfile.scss').toString();

@Component({
    selector:'create-profile',
    template: tpls,
    styles: [styles],
    providers: [apiPaths, SettingsService]
})
export class ProfileComponent implements OnInit {

    private getUserProfileUrl = 'http://in-it0289/Userapi/api/user/GetUserProfile?userEmail=';
    private updateUserProfile = 'http://in-it0289/Userapi/api/User/UpdateUserProfile';
    private deleteSubScription = 'http://in-it0289/Userapi/api/user/DeleteSubscription';
    private userEmail:any;
    private userDetails: any;
    private userProfileData : any = {};
    private tagData: any = [];
    private subscribeCat:any = [];
    private UserImage: any;
    private openEditProfile: boolean = false;
    private closeViewProfile:boolean = true;
    private UserProfileImage = this._settingService.settings;

    constructor(private httpService:CService,
                private _route:ActivatedRoute,
                private _settingService : SettingsService,
                public _base64service:Base64Service){
    }

    ngOnInit() {
      var self = this;
      this._route.params.subscribe(params => {
        this.userEmail = atob(params['usermail']);
        this.getProfileData(this.userEmail);
      });

    }

    getProfileData(userMail){
      this.userDetails = this.getUserProfileUrl+this.userEmail;
      this.httpService.observableGetHttp(this.userDetails ,null,false)
        .subscribe((res:Response)=> {
            this.userProfileData = res;
            console.log("Profile Data",this.userProfileData);
            this.subscribeCat = this.userProfileData.Alert;
            console.log("subscribeData",this.subscribeCat);
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

    ShowEditProfile(){
      this.openEditProfile = true;
      this.closeViewProfile = false;
    }

    UpdateProfileData(){
      let updatedData = this.userProfileData;
      let url = this.updateUserProfile;
      this.httpService.observablePutHttp(url,updatedData,null,false)
        .subscribe((res)=> {
            console.log("comes here in result",res);
          },
          error => {
            console.log("error in response");
          },
          ()=>{
            console.log("Finally");
          });
    }

    deleteSub(event){
      console.log("delete",event);
      this.subscribeCat = this.subscribeCat.splice();
      let UpdatedAlert = this.subscribeCat;
      let url = this.deleteSubScription+UpdatedAlert;
      this.httpService.observableDeleteHttp(url,null,false)
        .subscribe((res)=>{
        console.log("deleted the subsscription",res);
        },
          error =>{
          console.log("error in responese");
          },
          ()=>{
            console.log("finally");
          });
    }

}

