import { Component, OnInit,Input} from '@angular/core';
import {CService} from  '../../_common/services/http.service';
import {apiPaths} from  '../../../serverConfig/apiPaths';
import {Http, Headers,Response} from '@angular/http';
import {Router} from '@angular/router';

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
  private deleteMyListing:any;

  private getMyListingsUrl = "http://in-it0289/ListingAPI/api/Listings/GetListingsByEmail?email=";
  private deleteMyLisitngUrl = "http://in-it0289/ListingAPI/api/Listings/Delete?id=";

    constructor(private _cservice:CService, private _router: Router){
    }

    ngOnInit() {
      console.log("user email in my listing" ,this.userEmail);
      this.getMyListings(this.userEmail);
    }

  showProductInfo(id){
    this._router.navigateByUrl('/dashboard/productInfo/'+id);
  }


  getMyListings(userMail){
    this.userLisitngDetails = this.getMyListingsUrl+this.userEmail;
    this._cservice.observableGetHttp(this.userLisitngDetails ,null,false)
      .subscribe((res:Response)=> {
          this.userListingData = res;
          console.log("--------",this.userListingData);
        },
        error => {
          console.log("error in response");
        },
        ()=>{
          console.log("Finally");
        })
  }

  deleteLisitng(value){
    event.stopPropagation();
    this.deleteMyListing = this.deleteMyLisitngUrl+value;
    console.log(this.deleteMyListing);
    this._cservice.observableDeleteHttp(this.deleteMyListing ,null,false)
      .subscribe((res:Response)=> {
          console.log("ok",res);
          this.getMyListings(this.userEmail);
        },
        error => {
          console.log("error in response");
        },
        ()=>{
          console.log("Finally");
        })
  }

}

