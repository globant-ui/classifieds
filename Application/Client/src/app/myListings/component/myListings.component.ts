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
  private publishListData:any;
  private listingId : any;
  private currentUserList = [];
  private startIndex:number = 0;
  private endIndex:number = 4;
  private isNextEnable:boolean = true;
  private isBackEnable:boolean = false;
  private getMyListingsUrl = "http://in-it0289/ListingAPI/api/Listings/GetListingsByEmail?email=";
  private deleteMyLisitngUrl = "http://in-it0289/ListingAPI/api/listings/PutCloseListing/";
  private publishListUrl= "http://in-it0289/ListingAPI/api/listings/PutPublishListing/";

    constructor(private _cservice:CService, private _router: Router){
    }

    ngOnInit() {
      console.log("user email in my listing" ,this.userEmail);
      this.getMyListings(this.userEmail);
    }

  showProductInfo(id){
      event.stopPropagation();
    this._router.navigateByUrl('/dashboard/productInfo/'+id);
  }


  getMyListings(userMail){
    this.userLisitngDetails = this.getMyListingsUrl+this.userEmail;
    this._cservice.observableGetHttp(this.userLisitngDetails ,null,false)
      .subscribe((res:Response)=> {
          this.userListingData = res;
          this.getUpdatedList();
          console.log("--------",this.userListingData);
        },
        error => {
          console.log("error in response");
        },
        ()=>{
          console.log("Finally");
        })
  }

  getUpdatedList(){
    let arr = [];
    if(this.userListingData.length < this.endIndex){
      this.endIndex = this.userListingData.length;
    }
    for(let i = this.startIndex ; i < this.endIndex ; i++){
      arr.push(this.userListingData[i]);
    }
    this.currentUserList = arr;
  }

  checkFormControlStatus(p_value,card){
      if(card){
          for(let i = 0 ; i < p_value.length; i++){
              if(card[p_value[i]]){
                  let status = card[p_value[i]] != "" ? true : false;
                  if(status === false){
                      return false;
                  }
              }
              else{
                  return false;
              }
          }
          return true;
      }
      else{
          return false;
      }
  }

  checkPublishStatus(card){
    let status = true;
    if(card.IsPublished === false){
      if(this.checkFormControlStatus(['ListingType','Title','Details','City','Price','ListingCategory','SubCategory'],card) && card['Photos'].length > 0){
        switch(card['ListingCategory']){
          case 'Automotive':
                if(this.checkFormControlStatus(['YearOfPurchase','Brand'],card)){
                    if(card['SubCategory'] != 'Bicycle'){
                        if(this.checkFormControlStatus(['Type','KmDriven'],card)){
                            if(card['SubCategory']  === 'Car'){
                                if(!this.checkFormControlStatus(['FuelType'],card)){
                                    status = false;
                                }
                            }
                        }
                        else{
                            status = false;
                        }
                    }
                }
                else{
                    status = false;
                }
          break;
          case 'Housing':
                if(this.checkFormControlStatus(['IdealFor','Furnished','YearOfPurchase'],card)){
                    status = card['SubCategory'] != 'Single Room' ? this.checkFormControlStatus(['Rooms'],card) : true;
                }
                else{
                    status = false;
                }
          break;
          case  'Furniture':
                status = this.checkFormControlStatus(['DimensionHeight','DimensionLength','DimensionWidth','YearOfPurchase'],card);
          break;
          case  'Electronics':
                status = card['SubCategory'] != 'Other' ? this.checkFormControlStatus(['Brand','YearOfPurchase'],card) : true;
          break;
          case  'Other':
                status = card['SubCategory'] != 'Sport Equipment' ? this.checkFormControlStatus(['Type'],card) : this.checkFormControlStatus(['Brand'],card);
          break;
        }
      }
      else{
        status = false;
      }
    }
    
    return !status;
  }

  deleteLisitng(value){
    event.stopPropagation();
   // this.deleteMyListing = this.deleteMyLisitngUrl+value;
    this.listingId = value;
    console.log(this.deleteMyListing);
    this._cservice.observablePutHttp(this.deleteMyLisitngUrl+ this.listingId,null,null,false)
      .subscribe((res:Response)=> {
          console.log(res);
          this.getMyListings(this.userEmail);
        },
        error => {
          console.log("error in response");
        },
        ()=>{
          console.log("Finally");
        })
  }

  publishListing(id){
    event.stopPropagation();
    this._cservice.observablePutHttp(this.publishListUrl+ id,null,null,false)
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

  nextClickHandler(){
    if(this.userListingData){
      if(this.userListingData.length > 4){
        if(this.startIndex+4 < this.userListingData.length){
          this.startIndex++;
          this.endIndex = this.startIndex + 4;
          this.getUpdatedList();
        }
        this.isNextEnable = this.endIndex >= this.userListingData.length ? false : true;
        this.isBackEnable = this.startIndex <= 0 ? false : true;
      }
    }
  }

  previousClickHandler(){
    if(this.userListingData){
      if(this.userListingData.length > 4){
        if(this.startIndex-1 >= 0){
          this.startIndex--;
          this.endIndex = this.startIndex + 4;
          this.getUpdatedList();
        }
        this.isNextEnable = this.endIndex >= this.userListingData.length ? false : true;
        this.isBackEnable = this.startIndex <= 0 ? false : true;
      }
    }
  }

  EditMyListing(id){
      this._router.navigateByUrl('dashboard/createCard/' +id);
  }

}

