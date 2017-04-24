// tslint:disable
export class CEntity {

  private hash: number;

  constructor( _json: any ) {
    if ( _json ) {
      this.hash = this.hashcode( JSON.stringify( _json ) );
      Object.assign( this, _json );
    }
  }

  private equalTo( object: CEntity ): boolean {
    return this.hash === object.hash;
  }

  private hashcode( jsonString: string ): number {
    let hash = 0;
    let i;
    let chr;
    let len;
    if ( jsonString.length === 0 ) {
      return hash;
    }
    for ( i = 0, len = jsonString.length; i < len; i++ ) {
      chr = jsonString.charCodeAt( i );
      hash = ((hash << 5) - hash) + chr;
      hash |= 0; // Convert to 32bit integer
    }
    return hash;
  }
}

export declare abstract class Get {
  abstract get( key: string ): any;
}

export declare abstract class Set {
  abstract set( key: string, value: any ): any;
}

export declare abstract class IsValid {
  abstract isValid(): boolean;
}
