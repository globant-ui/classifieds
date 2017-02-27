import {Injectable} from '@angular/core';
import {Headers} from '@angular/http';
import {CookieService} from 'angular2-cookie/core';

@Injectable()
export class HeaderService {
  private sessionObj: Object;
  public headers: Headers;
  constructor( private _cookieService:CookieService) {
  }

  getHeaders(): Headers {
    this.headers = new Headers();
      this.sessionObj = this._cookieService.getObject('SESSION_PORTAL');
      this.headers.append( 'Content-Type', 'application/json; charset=UTF-8' );
      if(this.sessionObj!=undefined){
        this.headers.append('AccessToken',this.sessionObj.token);
        this.headers.append('UserEmail',this.sessionObj.useremail);
      }
    return this.headers;
  }
  

}
