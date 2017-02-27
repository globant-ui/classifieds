
export class UserInformation{

  public id: string;
  public position: string;
  public email: string;
  public location: string;

  constructor( _json: Object ) {
    Object.assign( this, _json );

  }

}
