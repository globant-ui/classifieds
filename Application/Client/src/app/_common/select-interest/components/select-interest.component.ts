import { Component,OnInit } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../services/http.service';
import {CookieService} from 'angular2-cookie/core';
import 'rxjs/Rx';

//using jquery syntax $
declare var $;

let styles = require('../styles/select-interest.component.scss').toString();
let tpls = require('../tpls/select-interest.component.html').toString();

@Component({
  selector: 'select-interest',
  styles : [ styles ],
  providers:[SettingsService, CService],
  template : tpls
})

export class SelectInterestComponent implements OnInit {

private delayTimer  : any = null;
private subCategoryUrl:string = "http://IN-IT0289/MasterDataAPI/api/Category/GetSubCategorySuggestion?subCategoryText=";
private selectInterestUrl:string = "http://in-it0289/UserAPI/api/User/AddTag?userEmail=";
private interestResult:any ;
private obj = {
  'SubCategory': [],
  'Location': []
};
private emailId:string;
private enabledDropdown : boolean = true;
private selectInterestPopUpFlag : boolean = true;
private isValid : boolean = true;
private disabledCheckbox : boolean = false;

  constructor(
                 private _settingsService: SettingsService,
                 private _cservice:CService,
                 private _cookieService:CookieService) {}

  ngOnInit(){
      this.emailId = this._cookieService.getObject('SESSION_PORTAL')["useremail"];
      console.log('session obj interest = ',this.emailId);
    }

  fetchInterest(e: Event, val) {
      if (val.length >= 3) {
          //Delay of some time to slow down the results
          clearTimeout(this.delayTimer);

          this.delayTimer = setTimeout(() => {
              this.fetchInterestData(val);
          }, 1000);
      }
  }
      //get call to get the dropdowndown list
    fetchInterestData(text: string) {
        this._cservice.observableGetHttp(this.subCategoryUrl + text, null, false)
            .subscribe((res: Response) => {
                if (res['length'] > 0) {
                    this.interestResult = res;
                    this.enabledDropdown = true;
                } else {
                    console.log("No result found!!!");
                }
            },
            error => {
                console.log("error in response");
            });
    }

//to select an item from dropdown
    selectInterest(val) {
        this.obj.SubCategory.push(val);
        this.obj.SubCategory = this.obj.SubCategory.reduce(function (a, b) {
            if (a.indexOf(b) < 0) a.push(b);
            return a;
        }, []);
        console.log("---------", this.obj.SubCategory)
        console.log("this.obj.SubCategory=====", this.obj.SubCategory)
        this.enabledDropdown = false;
        this.isValid = false;
        this.interestResult = [];
    }
     
    //to delete an item from tagList
    deleteIntrest(index, val) {
        let delFlag = this.obj.SubCategory.splice(index, 1);
    }

    //to close the pop-up window
    skipInterest(val){
        $("#myModal").modal("hide");
    }

    //checkbox code
    selectCheckbox(element: HTMLInputElement): void {
           
        let elemIndex = this.obj.Location.indexOf(element.value);
        
        if( element.value != 'All' && element.checked ) {
                this.obj.Location.push( element.value );
                
        }
            else {
                this.obj.Location.splice( elemIndex, 1 );   
                this.disabledCheckbox = false;
            }
         if( element.value == 'All' ) {
                this.obj.Location.length = 0;

                if( element.checked ) {
                    this.obj.Location.push( "Pune", "Banglore" ); 
                    this.disabledCheckbox = true;
                }
                
            }
             console.log("----ff", this.obj);
        } 
        
    PostData(obj){
        if(obj.Location.length === 0){
            obj.Location.push("Pune","Banglore");
         }
          this._cservice.observablePostHttp(this.selectInterestUrl+this.emailId, obj, null, false)
            .subscribe((res: Response) => {
                console.log("postcall response",res)
            },
            error => {
                console.log("error in response");
            });
         console.log("im done",obj);
   
    }

} 

