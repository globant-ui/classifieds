import { Component,OnInit } from '@angular/core';
import { AppState } from '../../../app.service';
import {AuthenticationWindowService} from '../../authentication/services/authentication.service';
import {SettingsService} from '../../services/setting.service';
import {Http, Headers, RequestOptions, Jsonp} from '@angular/http';
import {UserInformation} from '../../authentication/entity/userInformation.entity';
import {Session} from '../../authentication/entity/session.entity';
import {Router} from '@angular/router';
import {CookieService} from 'angular2-cookie/core';





let styles = require('../styles/header.component.scss').toString();
let tpls = require('../tpls/header.component.html').toString();

@Component({
  selector:'header',
  styles:[styles],
  providers:[AuthenticationWindowService, SettingsService, CookieService],
  template: tpls
})
export class HeaderComponent implements OnInit{

  public UserInformation: UserInformation;
  private windowHandle: any;
  private intervalLength = 100;
  private loopCount = 600;
  private  intervalId = null;
  private  code : any;
  private session : Session;
  private activeSession:boolean = false;
  private  validateUrl = 'http://in-it0289/UserApi/api/User/Register';
  public isCollapsed:boolean = true;


  constructor(public _authenticationWindowService: AuthenticationWindowService,
              private _settingsService: SettingsService,
              private _http: Http,
              private _router:Router,
              private _cookieService:CookieService){}

  ngOnInit(){
    console.log('here');
    console.log(this._settingsService.getSettings());
    console.log(this._settingsService.settings);
    this.session = new Session( this._cookieService.getObject( 'SESSION_PORTAL' ) );
    this.activeSession = (this.session && this.session.isValid());
    console.log(this.activeSession);
    if(!this.activeSession){
      this.doLogout();
    }
  }

    public collapsed(event:any):void {
        console.log(event);
    }

    public expanded(event:any):void {
        console.log(event);
    }

  doLogout(){
    this._authenticationWindowService.doLogOut();
  }

  doLogin(){
    console.log(this._settingsService.settings);
    let context = this;
    let params = '?client_id=' + encodeURIComponent( this._settingsService.settings.auth.client_id )
      + '&redirect_uri=' + encodeURIComponent( this._settingsService.settings.auth.redirect_uri )
      + '&scope=' + encodeURIComponent( this._settingsService.settings.auth.scope )
      + '&response_type=' + this._settingsService.settings.auth.response_type
      + '&prompt=' + this._settingsService.settings.auth.prompt
      + '&access_type=' + this._settingsService.settings.auth.access_type;

    let loopCount = this.loopCount;

     this.windowHandle = this._authenticationWindowService.createWindow( (this._settingsService.settings.auth.google_login + params), 'Login', 0, 0, true );
    this.intervalId = setInterval(() => {
      if ( loopCount-- < 0 ) {
       context.windowHandle.close();
        clearInterval( context.intervalId );
      } else {
        let href: string;
        try {
          href = context.windowHandle.location.href;
        } catch (e) {
        }
        if (href != null) {
          let re = /code=(.*)/;
          let found = href.match(re);
          if (found && found.length > 1) {
            console.log('Google callback URL: ', href);
            context.windowHandle.close();
            clearInterval(context.intervalId);
            let unfilteredCode = found[1];
            this.code = unfilteredCode.substring(0, unfilteredCode.length - +( unfilteredCode.lastIndexOf('#') == unfilteredCode.length - 1));
            this.getAuthToken(this.code).then((res)=>{
               this.getUserInfoGoogle(res['access_token']);
               console.log('token',res['access_token']);

            });
          }
        }
      }
    },context.intervalLength);
    }

    private getAuthToken(token: string){
      var headers = new Headers();
      headers.append('Content-Type', 'application/x-www-form-urlencoded');
      return new Promise( ( resolve, reject ) => {
        if ( token ) {
          this._http.post(this._settingsService.settings.auth.getToken
                              +'&code=' +encodeURIComponent(token)
                              +'&client_id='+encodeURIComponent(this._settingsService.settings.auth.client_id)
                              +'&client_secret='+encodeURIComponent('ySOaT67r44BcWkNqovW7MmM-')
                              +'&redirect_uri=' +'http://localhost:3000'
                              , {headers: headers})
            .subscribe(
              response => {
                let userGoogle = new UserInformation( response.json() );
                resolve(userGoogle);
              },
              error => {
                console.error(error);
              });
        }else {
          console.error("token not found");
        }
      } )
    }

  private getUserInfoGoogle( token: any ): Promise < {} > {
    return new Promise( ( resolve, reject ) => {
      if ( token ) {
        var headers = new Headers();
        headers.append('Authorization', 'Bearer ' + token);
        this._http.get(this._settingsService.settings.auth.google_userinfo, {headers: headers})
          .subscribe(
            response => {
              let userGoogle = new UserInformation( response.json() );
              console.log(userGoogle,'usergoogle');
              let ClassifiedsUser ={
                  userName: "",
                  userEmail : ""
              }
              ClassifiedsUser.userEmail =userGoogle['email'];
              ClassifiedsUser.userName = userGoogle['name'];
              this.validateUser(ClassifiedsUser);
            },
        error => {
              console.error(error);
        });
      }else {
        console.error("token not found");
      }
    } )
  }

  private validateUser (user){
    let data= user;
    console.log(user,"user");
    this._http.post(this.validateUrl,data)
      .subscribe((res)=> {
      console.log(res);
        },
        error => {
          console.log("error in response");
        },
        ()=>{
          console.log("Finally");
        })
  }

}

