import {Get, Set} from './entity';
import {CEntity} from './entity';
import {NavigationLink} from "./navigation.link.entity.ts";

export class Session extends CEntity implements Get, Set {

  public authenticated: boolean;
  public token: string;
  public expires: number;

  constructor( _json: Object ) {
    super( _json );
  }

  isValid(): boolean {
    return this.authenticated;
  }

  isAuthenticated(): boolean {
    return this.authenticated;
  }

  getSecondsLeft(): number {
    return this.expires - new Date().getTime();
  }

  get( key: string ): any {
    if ( key !== undefined ) {
      return this[ key ];
    } else {
      return undefined;
    }
  }

  set( key: string, value: any ): void {
    return this[ key ] = value;
  }
}
