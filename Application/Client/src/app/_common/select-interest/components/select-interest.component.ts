import { Component } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../services/http.service';
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

export class SelectInterestComponent {

private delayTimer  : any = null;
private subCategoryUrl:string = "http://IN-IT0289/MasterDataAPI/api/Category/GetSubCategorySuggestion?subCategoryText=";
private interestResult:any ;
private obj = {
  'selectedInterests': [],
  'preferedLoc': []
};
private enabledDropdown : boolean = true;
//private selectInterestPopUpFlag : boolean = true;

  constructor(
                 private _settingsService: SettingsService,
                 private _cservice:CService) {}

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
        this.obj.selectedInterests.push(val);
        this.obj.selectedInterests = this.obj.selectedInterests.reduce(function (a, b) {
            if (a.indexOf(b) < 0) a.push(b);
            return a;
        }, []);
        console.log("---------", this.obj.selectedInterests)
        console.log("this.obj.selectedInterests=====", this.obj.selectedInterests)
        this.enabledDropdown = false;
        this.interestResult = [];
    }
     
    //to delete an item from tagList
    deleteIntrest(index, val) {
        this.obj.selectedInterests.splice(index, 1);
    }

    //to close the pop-up window
    skipInterest(val){
        $("#myModal").modal("hide");
    }

    //checkbox code
    selectCheckbox(element: HTMLInputElement): void {

        let elemIndex = this.obj.preferedLoc.indexOf(element.value);
            if (element.checked && elemIndex === -1) {
                this.obj.preferedLoc.push(element.value);
            } else if (elemIndex > -1) {
                this.obj.preferedLoc.splice(elemIndex, 1);
            }
        console.log("----ff", this.obj.preferedLoc);
        console.log("----obj", this.obj);
    }

} 