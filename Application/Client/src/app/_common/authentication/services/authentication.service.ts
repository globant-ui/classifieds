import {Injectable} from '@angular/core';
import {CookieService} from 'angular2-cookie/core';
import {Session} from '../../authentication/entity/session.entity';
import { Router } from '@angular/router';

@Injectable()
export class AuthenticationWindowService {

  public session:Session;

  constructor(private _cookieService:CookieService,
              private _router:Router) {}

  public doLogOut(){
    this.session = null;
    this._cookieService.remove('SESSION_PORTAL');
    this._router.navigateByUrl('/');

  }

  createWindow( url, name = 'Window', width = 430, height = 600, fullScreen: boolean ) {
    if ( fullScreen ) {
      let options = 'scrollbars=no,width=' + screen.width + ', height=' + screen.height + ', top=1, left=1';
      let wind = window.open( url, name, options );
      wind.moveTo( 1, 1);
      return wind;
    } else {
      let l = Math.round( (screen.width / 2) - (width / 2) );
      let t = Math.round( (screen.height / 2) - (height / 2) );
      let options = 'scrollbars=no,width=' + width + ', height=' + height + ', top=' + t + ', left=' + l;
      let wind = window.open( url, name, options );
      wind.moveTo( l, t );
      return wind;
    }
  }
}
