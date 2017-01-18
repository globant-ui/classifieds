
declare var $: any;

export class UserInformation{

  public id: string;
  public globerId: number;
  public givenName: string
  public globerName: Array<string>;
  public familyName:string;
  public globerFirstName:string;
  public gender: string;
  public username: string;
  public seniority: string;
  public position: string;
  public email: string;
  public picture: string;
  public phone: string;
  public internal: string;
  public hd: string;
  public link: string;
  public entryDate: string;
  public locale: string;
  public location: string;
  public roles: Array<any>;

  constructor( _json: Object ) {
    Object.assign( this, _json );

  }

}
