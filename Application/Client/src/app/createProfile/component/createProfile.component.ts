import { Component, OnInit, NgZone } from '@angular/core';
import { CookieService } from 'angular2-cookie/core';
import { CService } from  '../../_common/services/http.service';
import { SettingsService } from '../../_common/services/setting.service';
import { ActivatedRoute } from '@angular/router';
import { Base64Service } from '../../_common/services/base64.service';
import { ApiPaths } from  '../../../serverConfig/apiPaths';
import { Http, Response, Headers } from '@angular/http';
import { Session } from '../../_common/authentication/entity/session.entity';
import { FileUploader } from 'ng2-file-upload';

const URL = 'http://in-it0289/UserAPI/api/User/PostUserImage';

@Component({
    selector: 'create-profile',
    template: require('../tpls/createProfile.html').toString(),
    styles: [require('../styles/createProfile.scss').toString()],
    providers: [ApiPaths, SettingsService]
})
export class ProfileComponent implements OnInit {

    private getUserProfileUrl = 'http://in-it0289/Userapi/api/user/GetUserProfile?userEmail=';
    private updateUserProfile = 'http://in-it0289/Userapi/api/User/UpdateUserProfile';
    private deleteSubScription = 'http://in-it0289/UserAPI/api/User/DeleteAlerts?userEmail=';
    private UpdateImageUrl =  'http://in-it0289/UserAPI/api/User/PostUserImage';
    private userEmail: any;
    private userDetails: any;
    private userProfileData: any = {};
    private tagData: any = [];
    private subscribeCat: any = [];
    private UserImage: any;
    private updatedTag: any = [];
    private showPopupDivMessage: string= '';
    private delayTimer: any;
    private showPopupMessage: boolean = false;
    private openEditProfile: boolean = false;
    private closeViewProfile: boolean = true;
    private UserProfileImage = this._settingService.settings;
    private subCategoryUrl: string = '';
    private selectInterestUrl: string = '';
    private enabledDropdown: boolean = true;
    private interestResult: any ;
    private session: Session;
    private availableLocation: String[] = ['Pune', 'Banglore'];
    private selectedLocation: any = [];
    private isAllLocationChecked: Boolean = false;
    private preferedLocation: any = '';

    constructor(
      private _http: Http,
      private httpService: CService,
      private _cookieService: CookieService,
      private _route: ActivatedRoute,
      private _settingService: SettingsService,
      public _base64service: Base64Service,
      private zone: NgZone
    ) {
      this.subCategoryUrl = this. _settingService.getPath('subCategoryUrl');
      this.selectInterestUrl = this._settingService.getPath('searchUrl');
    }

    public ngOnInit() {
      let self = this;
      this._route.params.subscribe((params) => {
        this.userEmail = atob(params['usermail']);
        this.getProfileData(this.userEmail);
      });

    }

    private getProfileData(userMail) {
      this.userDetails = this.getUserProfileUrl + this.userEmail;
      this.userProfileData = {};
      this.httpService.observableGetHttp(this.userDetails, null, false)
        .subscribe((res: Response) => {
            this.userProfileData = res;
            this.updateLocalLocation();
            this.subscribeCat = this.userProfileData.Alert;
            console.log('subscribeData', this.subscribeCat);
            this.tagData = this.userProfileData.Tags.SubCategory;
            this.UserImage = this.userProfileData.Image;
          },
          (error) => {
            console.log('error in response');
          },
          () => {
            console.log('Finally');
          });
    }

    private ShowEditProfile() {
      this.openEditProfile = true;
      this.closeViewProfile = false;
    }

    private updateLocalLocation() {
      if (this.userProfileData.Tags.Location) {
        this.selectedLocation =  Array.from(this.userProfileData.Tags.Location);
        if (this.selectedLocation.indexOf('All loactions') !== -1) {
          this.isAllLocationChecked = true;
          this.preferedLocation = this.availableLocation;
        }else {
          this.preferedLocation = this.selectedLocation;
        }
      }
    }

    private isChecked(checkData: string) {
        if (this.selectedLocation.indexOf(checkData) !== -1) {
          return true;
        }
        return false;
    }

    private onLocationChange($event) {
        let self = this;
        if ($event.target.name === 'All loactions') {
          self.isAllLocationChecked = !self.isAllLocationChecked;
        }
        if ($event.target.checked) {
          if (self.selectedLocation.indexOf($event.target.value) === -1) {
            self.selectedLocation.push($event.target.value);
          }
        }else {
          if (self.selectedLocation.indexOf($event.target.value) !== -1) {
              self.selectedLocation.splice(self.selectedLocation.indexOf($event.target.value), 1);
          }
        }
    }

    private UpdateProfileData() {
      this.userProfileData.Tags.Location = this.selectedLocation;
      let updatedData = this.userProfileData;
      let url = this.updateUserProfile;
      this.httpService.observablePutHttp(url, updatedData, null, false)
        .subscribe((res) => {
            console.log('comes here in result', res);
            this.showPopupMessage = true;
            this.showPopupDivMessage = 'profile';
          },
          (error) => {
            console.log('error in response');
          },
          () => {
            console.log('Finally');
          });
    }

    private deleteSub(res) {
     let UpdatedAlert = JSON.stringify(res);
     let url = this.deleteSubScription + this.userEmail;
     this.subscribeCat.push(res);
     this.httpService.observablePutHttp(url, UpdatedAlert, null, false)
        .subscribe((response) => {
            console.log('deleted the subsscription', response);
            this.getProfileData(this.userEmail);
          },
          (error) => {
            console.log('error in responese');
          },
          () => {
            console.log('finally');
          });
    }

  private deleteTag(res) {
      this.tagData.splice(res, 1);
  }

  private UpdateImage(event) {
    let self = this;
    let data = new FormData();
    let xhr = new XMLHttpRequest();
    xhr.onreadystatechange = () => {
      if (xhr.readyState === XMLHttpRequest.DONE) {
        if (xhr.status === 201) {
          self.UserImage = JSON.parse(xhr.response).Message;
        } else {
          console.log('Error');
        }
      }
    };
    this.session = new Session(this._cookieService.getObject('SESSION_PORTAL'));
    self.UserImage = null;
    xhr.open('POST', 'http:// in-it0289/UserAPI/api/User/PostUserImage');
    xhr.setRequestHeader('accesstoken', this.session['token'] );
    xhr.setRequestHeader('useremail', this.userEmail);
    xhr.send(data);
  }

  private fetchInterest(e: Event, val) {
    if (val.length >= 3) {
      // Delay of some time to slow down the results
      clearTimeout(this.delayTimer);
      this.delayTimer = setTimeout(() => {
        this.fetchInterestData(val);
      }, 1000);
    }
  }
  private fetchInterestData(text: string) {
    this.httpService.observableGetHttp(this.subCategoryUrl + text, null, false)
      .subscribe((res: Response) => {
          if (res['length'] > 0) {
            this.interestResult = res;
            this.enabledDropdown = true;
          } else {
            console.log('No result found!!!');
          }
        },
        (error) => {
          console.log('error in response', error);
        });
  }

  private selectInterest(val) {
    this.tagData.push(val);
    console.log(this.tagData);
    // to duplicate element allowed in tag logic
    this.tagData = this.tagData.reduce((a, b) => {
      if (a.indexOf(b) < 0) {
        a.push(b);
      }
      return a;
    }, []);
    this.enabledDropdown = false;
    this.interestResult = [];
  }
}
