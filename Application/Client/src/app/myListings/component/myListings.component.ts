import { Component, OnInit,Input} from '@angular/core';
import {CService} from  '../../_common/services/http.service';
import {apiPaths} from  '../../../serverConfig/apiPaths';
import {Http, Headers,Response} from '@angular/http';

let tpls = require('../tpls/myListings.component.html').toString();
let styles = require('../styles/myListings.component.scss').toString();

@Component({
    selector:'my-listings',
    template: tpls,
    styles: [styles],
    providers: []
})
export class MyListingsComponent implements OnInit {

  @Input () userEmail;

  private userLisitngDetails:any;
  private userListingData:any;

  private GetMyListings = "http://in-it0289/ListingAPI/api/Listings/GetListingsByEmail?email=";


    constructor(private _cservice:CService){
    }

    ngOnInit() {
      console.log("user email in my listing" ,this.userEmail);
      this.getMyListings(this.userEmail);
    }

  getMyListings(userMail){
    this.userLisitngDetails = this.GetMyListings+this.userEmail;
    this._cservice.observableGetHttp(this.userLisitngDetails ,null,false)
      .subscribe((res:Response)=> {
          this.userListingData = res;
          console.log(this.userListingData);
        },
        error => {
          console.log("error in response");
        },
        ()=>{
          console.log("Finally");
        })
  }


}

