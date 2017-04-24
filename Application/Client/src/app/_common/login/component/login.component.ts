import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { AuthenticationWindowService } from '../../authentication/services/authentication.service';
import { SettingsService } from '../../services/setting.service';
import { Http, Headers } from '@angular/http';
import { CookieService } from 'angular2-cookie/core';
import { UserInformation } from '../../authentication/entity/userInformation.entity';
import { Session } from '../../authentication/entity/session.entity';
import { Router } from '@angular/router';
import { ModalDirective } from 'ng2-bootstrap';

@Component({
  selector: 'login',
  styles: [require('../styles/login.scss').toString()],
  providers: [AuthenticationWindowService, SettingsService, CookieService],
  template: require('../tpls/login.html').toString()
})
export class LoginComponent implements OnInit, AfterViewInit {

    private UserInformation: UserInformation;
    private windowHandle: any;
    private intervalLength = 100;
    private loopCount = 600;
    private  intervalId = null;
    private session: Session;
    private code: any;
    private validateUrl: string = 'http://in-it0289/UserApi/api/User/RegisterUser';
    private activeSession: boolean = false;

    @ViewChild('childModal') private childModal: ModalDirective;

    constructor(public _authenticationWindowService: AuthenticationWindowService,
                private _settingsService: SettingsService,
                private _http: Http,
                private _router: Router,
                private _cookieService: CookieService) {}
    public ngOnInit() {
        this._settingsService.getSettings();
        this.session = new Session(this._cookieService.getObject('SESSION_PORTAL'));
        this.activeSession = (this.session && this.session.isValid());
        if (this.activeSession) {
            this._router.navigateByUrl('/dashboard/home');
        }
    }

  public ngAfterViewInit() {
    this.showChildModal();
  }

  private showChildModal(): void {
    this.childModal.show();
  }

  private hideChildModal(): void {
    this.childModal.hide();
  }

  private doLogin() {
    console.log(this._settingsService.settings);
    let context = this;
    this.session = new Session({});
    let params = '?client_id='
      + encodeURIComponent( this._settingsService.settings.auth.client_id )
      + '&redirect_uri=' + encodeURIComponent( this._settingsService.settings.auth.redirect_uri)
      + '&scope=' + encodeURIComponent( this._settingsService.settings.auth.scope )
      + '&response_type=' + this._settingsService.settings.auth.response_type
      + '&prompt=' + this._settingsService.settings.auth.prompt
      + '&access_type=' + this._settingsService.settings.auth.access_type;
    let loopCount = this.loopCount;// tslint:disable
    this.windowHandle = this._authenticationWindowService.createWindow( (this._settingsService.settings.auth.google_login + params), 'Login', 0, 0, true );
    this.intervalId = setInterval(() => {// tslint:enable
      if ( loopCount-- < 0 ) {
        context.windowHandle.close();
        clearInterval( context.intervalId );
      } else {
        let href: string;
        try {
          href = context.windowHandle.location.href;
        }catch (e) {
          console.log('error..');
        }
        if (href != null) {
          let re = /code=(.*)/;
          let found = href.match(re);
          if (found && found.length > 1) {
            console.log('Google callback URL: ', href);
            context.windowHandle.close();
            clearInterval(context.intervalId);
            let unfilteredCode = found[1];// tslint:disable
            this.code = unfilteredCode.substring(0, unfilteredCode.length - +( unfilteredCode.lastIndexOf('#') == unfilteredCode.length - 1));
            this.getAuthToken(this.code).then((res) => {// tslint:enable
              this.getUserInfoGoogle(res['access_token']);
            });
          }
        }
      }
    }, context.intervalLength);
  }

  private getAuthToken(token: string) {
    let headers = new Headers();
    headers.append('Content-Type', 'application/x-www-form-urlencoded');
    return new Promise( ( resolve, reject ) => {
      if ( token ) {
        this._http.post(this._settingsService.settings.auth.getToken
          + '&code=' + encodeURIComponent(token)
          + '&client_id=' + encodeURIComponent(this._settingsService.settings.auth.client_id)
          + '&client_secret=' + encodeURIComponent('A2Sr3dnWbdE_uyufnVZJwGdj')
          + '&redirect_uri=' + 'http://localhost:3000'
          , {headers})
          .subscribe(
            (response) => {
              let userGoogle = new UserInformation( response.json() );
              console.log('user GOogle', userGoogle);
              resolve(userGoogle);
            },
            (error) => {
              console.error(error);
            });
      }else {
        console.error('token not found');
      }
    });
  }

  private getUserInfoGoogle( token: any ): Promise < {} > {
    return new Promise( ( resolve, reject ) => {
      if ( token ) {
        let headers = new Headers();
        headers.append('Authorization', 'Bearer ' + token);
        this._http.get(this._settingsService.settings.auth.google_userinfo, {headers})
          .subscribe(
            (response) => {
              let userGoogle = new UserInformation( response.json() );
              console.log(userGoogle, 'usergoogle');
              let ClassifiedsUser = {
                UserName : '',
                UserEmail : ''
              };
              ClassifiedsUser.UserEmail = userGoogle['email'];
              ClassifiedsUser.UserName = userGoogle['name'];
              console.log('check classifieds user = ', ClassifiedsUser);
              this.validateUser(ClassifiedsUser);
            },
            (error) => {
              console.error(error);
            });
      }else {
        console.error('token not found');
      }
    });
  }

  private validateUser (user) {
    let data = user;
    this._http.post(this.validateUrl, data)
      .subscribe((res) => {
          let validUser = res.json();
          console.log('valid user response = ', validUser);
          console.log('valid use AT = ', validUser.AccessToken);
          console.log('valid use email = ', validUser.UserEmail);
          this.session.set( 'authenticated', true );
          this.session.set( 'isFirstTimeLogin', validUser.IsFirstTimeLogin );
          this.session.set( 'token', validUser.AccessToken);
          this.session.set( 'useremail', validUser.UserEmail);
          console.log('valid user session started', this.session);
          this._cookieService.putObject('SESSION_PORTAL', this.session);
          this._router.navigateByUrl('/dashboard/home');
        },
        (error) => {
          console.log('error in response');
        },
        () => {
          console.log('Finally');
        });
  }

}
