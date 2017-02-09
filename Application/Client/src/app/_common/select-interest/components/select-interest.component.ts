import { Component } from '@angular/core';
import { AppState } from '../../app.service';
import {SettingsService} from '../../services/setting.service';
import { Observable }     from 'rxjs/Observable';
import { Http, Response,RequestOptions } from '@angular/http';
import {CService} from  '../../services/http.service';
import 'rxjs/Rx';

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
private subCategoryUrl:string = "http://localhost:3000/app/_common/select-interest/json/select-interest.json";
private interestResult:any ;
private selectedInterests = [];
private enabledDropdown : boolean = false;

  constructor(
                 private _settingsService: SettingsService,
                 private _cservice:CService) {}

    fetchInterest( e:Event,val ){

        if (val.length >= 3) {
            this.subCategoryUrl = this.subCategoryUrl;
            //Delay of some time to slow down the results
            clearTimeout(this.delayTimer);

            this.delayTimer = setTimeout(() => {
                this.fetchInterestData(val);
            }, 1000);
        }
    }
      //get call to get the dropdowndown list
    fetchInterestData( text: string ) {
      this._cservice.observableGetHttp( this.subCategoryUrl, null, false )
          .subscribe((res: Response) => {
              if (res['subCategory'] && res['subCategory'][ 'length' ] > 0) {
                  this.interestResult = res['subCategory'];
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
    selectInterest(val){
        this.selectedInterests.push(val);
        this.enabledDropdown = false;
        this.interestResult = [];
    }
     
    //to delete an item from tag 
    deleteIntrest( index, val ) {
      this.selectedInterests.splice(index, 1);
    }
} 