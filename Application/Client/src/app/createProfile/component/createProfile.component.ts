import { Component, OnInit, NgZone} from '@angular/core';
import {CookieService} from 'angular2-cookie/core';
import {CService} from  '../../_common/services/http.service';
import {SettingsService} from '../../_common/services/setting.service';
import { ActivatedRoute } from '@angular/router';
import {Base64Service} from '../../_common/services/base64.service';
import {apiPaths} from  '../../../serverConfig/apiPaths';
import {Http,Response,Headers} from '@angular/http';
import {Session} from '../../_common/authentication/entity/session.entity';
import { FileUploader } from 'ng2-file-upload';
// import * as _ from "lodash";

let tpls = require('../tpls/createProfile.html').toString();
let styles = require('../styles/createProfile.scss').toString();

const URL = 'http://in-it0289/UserAPI/api/User/PostUserImage';
//const URL = 'https://evening-anchorage-3159.herokuapp.com/api/';

@Component({
    selector:'create-profile',
    template: tpls,
    styles: [styles],
    providers: [apiPaths, SettingsService]
})
export class ProfileComponent implements OnInit {

    private getUserProfileUrl = 'http://in-it0289/Userapi/api/user/GetUserProfile?userEmail=';
    private updateUserProfile = 'http://in-it0289/Userapi/api/User/UpdateUserProfile';
    private deleteSubScription = 'http://in-it0289/UserAPI/api/User/DeleteAlerts?userEmail=';
    private UpdateImageUrl =  'http://in-it0289/UserAPI/api/User/PostUserImage';
    private userEmail:any;
    private userDetails: any;
    private userProfileData : any = {};
    private tagData: any = [];
    private subscribeCat:any = [];
    private UserImage: any;
    private updatedTag:any = [];
    private showPopupDivMessage:string= '';
    private delayTimer: any;
    private showPopupMessage: boolean = false;
    private openEditProfile: boolean = false;
    private closeViewProfile:boolean = true;
    private UserProfileImage = this._settingService.settings;
    private subCategoryUrl: string = '';
    private selectInterestUrl: string = '';
    private enabledDropdown : boolean = true;
    private interestResult:any ;
    private session : Session;

    constructor(  private _http: Http,
                private httpService:CService,
                private _cookieService:CookieService,
                private _route:ActivatedRoute,
                private _settingService : SettingsService,
                public _base64service:Base64Service,
                  private zone:NgZone){

      this.subCategoryUrl =this. _settingService.getPath('subCategoryUrl');
      this.selectInterestUrl =this._settingService.getPath('searchUrl');

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
      this.userProfileData = {};
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
            this.showPopupMessage = true;
            this.showPopupDivMessage = 'profile';
          },
          error => {
            console.log("error in response");
          },
          ()=>{
            console.log("Finally");
          });
    }

    deleteSub(res){
     let UpdatedAlert = JSON.stringify(res);
      let url = this.deleteSubScription + this.userEmail;
      this.subscribeCat.push(res);
      this.httpService.observablePutHttp(url,UpdatedAlert,null,false)
        .subscribe((response)=>{
            console.log("deleted the subsscription",response);
            this.getProfileData(this.userEmail);
          },
          error =>{
            console.log("error in responese");
          },
          ()=>{
            console.log("finally");
          });
    }
  deleteTag(res){
      this.tagData.splice(res,1);
  }

  UpdateImage(event){
    let self = this;
    var data = new FormData();
    data.append("file", document.getElementById("myFileField").files[0]);

     var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function() {
      if (xhr.readyState == XMLHttpRequest.DONE) {
        if(xhr.status === 201) {
          self.UserImage = JSON.parse(xhr.response).Message;
        } else {
          console.log("Error");
        }
      }
    }
    this.session = new Session(this._cookieService.getObject('SESSION_PORTAL'));
    self.UserImage = null;
    xhr.open("POST", "http://in-it0289/UserAPI/api/User/PostUserImage");
    xhr.setRequestHeader("accesstoken",this.session['token'] );
    xhr.setRequestHeader("useremail", this.userEmail);
    xhr.send(data);
  }

  fetchInterest(e: Event, val) {
    if (val.length >= 3) {
      //Delay of some time to slow down the results
      clearTimeout(this.delayTimer);
      this.delayTimer = setTimeout(() => {
        this.fetchInterestData(val);
      }, 1000);
    }
  }
  fetchInterestData(text: string) {
    this.httpService.observableGetHttp(this.subCategoryUrl + text, null, false)
      .subscribe((res: Response) => {
          if (res['length'] > 0) {
            this.interestResult = res;
            this.enabledDropdown = true;
          } else {
            console.log("No result found!!!");
          }
        },
        error => {
          console.log("error in response", error);
        });
  }

  selectInterest(val) {
    this.tagData.push(val);
    console.log(this.tagData);
    //to duplicate element allowed in tag logic
    this.tagData = this.tagData.reduce(function (a, b) {
      if (a.indexOf(b) < 0) a.push(b);
      return a;
    }, []);
    this.enabledDropdown = false;
    this.interestResult = [];
  }
}

