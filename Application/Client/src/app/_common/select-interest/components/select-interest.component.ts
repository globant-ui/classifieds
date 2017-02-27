import { Component,OnInit, AfterViewInit  } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../services/http.service';
import {CookieService} from 'angular2-cookie/core';
import 'rxjs/Rx';
import {Session} from '../../authentication/entity/session.entity';

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

export class SelectInterestComponent implements OnInit, AfterViewInit  {

private delayTimer  : any = null;
private subCategoryUrl:string = "";
private selectInterestUrl:string = "";
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
private session : Session;

  constructor(
                 private _settingsService: SettingsService,
                 private _cservice:CService,
                 private _cookieService:CookieService) {
                        this.subCategoryUrl = _settingsService.getPath('subCategoryUrl');
                        this.selectInterestUrl = _settingsService.getPath('searchUrl');
                 }

  ngOnInit(){
      this.emailId = this._cookieService.getObject('SESSION_PORTAL')["useremail"];
    }

    ngAfterViewInit(){
        this.session = new Session(this._cookieService.getObject('SESSION_PORTAL'));
        if (this.session['isFirstTimeLogin']) {
            $("#myModal").modal("show");
        }
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
                console.log("error in response",error);
            });
    }

//to select an item from dropdown
    selectInterest(val) {
        this.obj.SubCategory.push(val);
        //to duplicate element allowed in tag logic
        this.obj.SubCategory = this.obj.SubCategory.reduce(function (a, b) {
            if (a.indexOf(b) < 0) a.push(b);
            return a;
        }, []);
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
        } 

    //post call
    PostData(obj){
        if(obj.Location.length === 0){
            obj.Location.push("Pune","Banglore");
         }
          this._cservice.observablePostHttp(this.selectInterestUrl+this.emailId, obj, null, false)
            .subscribe((res: Response) => {
                console.log("postcall response",res)
            },
            error => {
                console.log("error in response",error);
            });
            $("#myModal").modal("hide");
   
    }

} 

