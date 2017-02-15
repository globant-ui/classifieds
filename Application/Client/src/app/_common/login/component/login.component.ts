import { Component,OnInit,ViewChild } from '@angular/core';
import {AuthenticationWindowService} from '../../authentication/services/authentication.service';
import {SettingsService} from '../../services/setting.service';
import {Http, Headers} from '@angular/http';
import {UserInformation} from '../../authentication/entity/userInformation.entity';
import {Session} from '../../authentication/entity/session.entity';
import {Router} from '@angular/router';
import { ModalDirective } from 'ng2-bootstrap';

let styles = require('../styles/login.scss').toString();
let tpls = require('../tpls/login.html').toString();

@Component({
    selector:'login',
    styles:[styles],
    providers:[AuthenticationWindowService, SettingsService],
    template: tpls
})
export class LoginComponent implements OnInit{

    public UserInformation: UserInformation;
    private windowHandle: any;
    private intervalLength: number = 100;
    private loopCount: number = 600;
    private  intervalId = null;
    private session : Session;
    private  code : any;
    private  validateUrl = 'http://in-it0289/UserApi/api/User/Register';
    private activeSession:boolean = false;

  @ViewChild('childModal') public childModal:ModalDirective;
    constructor(public _authenticationWindowService: AuthenticationWindowService,
                private _settingsService: SettingsService,
                private _http: Http,
                private _router: Router){}

    ngOnInit(){
        this._settingsService.getSettings();
        this.activeSession = (this.session && this.session.isValid());
        if(this.activeSession){
            this._router.navigateByUrl('home');
        }
    }
    doLogin(){
        let context = this;
        this.session =new Session({});
        console.log(this._settingsService.settings);
        debugger;
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
                        context.windowHandle.close();
                        clearInterval(context.intervalId);
                        let unfilteredCode = found[1];
                        this.code = unfilteredCode.substring(0, unfilteredCode.length - +( unfilteredCode.lastIndexOf('#') == unfilteredCode.length - 1));
                        this.getAuthToken(this.code).then((res)=>{
                            this.getUserInfoGoogle(res['access_token']);
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
                        let ClassifiedsUser ={
                            "UserName" : "",
                            "UserEmail" : ""
                        }
                        ClassifiedsUser.UserEmail =userGoogle['email'];
                        ClassifiedsUser.UserName = userGoogle['name'];
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
         this._http.post(this.validateUrl,data)
         .subscribe((res)=> {
              let validUser = res.json();
              this.session.set( 'authenticated', true );
              this.session.set( 'token', validUser.AccessToken);
              this.session.set( 'useremail', validUser.UserEmail);
              this._router.navigateByUrl('home');
         },
         error => {
            console.log("error in response");
         },
         ()=>{
            console.log("Finally");
         })
     }

}
