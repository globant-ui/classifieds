/*jshint bitwise: false*/

export class CEntity {

  private hash: number;

  constructor( _json: any ) {
    if ( _json ) {
      this.hash = this.hashcode( JSON.stringify( _json ) );
      Object.assign( this, _json );
    }
  }

  equalTo( object: CEntity ): boolean {
    return this.hash === object.hash;
  }

  private hashcode( _json_string: string ): number {
    let hash = 0, i, chr, len;
    if ( _json_string.length === 0 ) {
      return hash;
    }
    for ( i = 0, len = _json_string.length; i < len; i++ ) {
      chr = _json_string.charCodeAt( i );
      hash = ((hash << 5) - hash) + chr;
      hash |= 0; // Convert to 32bit integer
    }
    return hash;
  }
}

export declare abstract class Get {
  abstract get( string ): any;
}

export declare abstract class Set {
  abstract set( string, any ): any;
}

export declare abstract class IsValid {
  abstract isValid(): boolean;
}
