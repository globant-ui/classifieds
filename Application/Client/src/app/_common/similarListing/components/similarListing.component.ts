import { Component,OnInit,Input,Output,EventEmitter,OnChanges,SimpleChange } from '@angular/core';
import {CService} from  '../../services/http.service';
import {apiPaths} from  '../../../../serverConfig/apiPaths';


let styles = require('../styles/similarListing.component.scss').toString();
let tpls = require('../tpls/similarListing.component.html').toString();

@Component({
  selector:'similar-listing',
  styles:[styles],
  template: tpls
})
export class SimilarListingComponent implements OnChanges{

  @Input() type;

  @Output() similarListingLoaded:EventEmitter<string> = new EventEmitter();

  public similarListing;

  constructor(private httpService:CService,private apiPath:apiPaths){}

  ngOnChanges(changes: {[propKey: string]: SimpleChange}){
    this.getSimilarListing();
  }

  getSimilarListing(){
    let pathUrl= '';
    let filter = this.type.split("-");

    pathUrl = this.apiPath.SIMILAR_LISTING + '?subCategory='+filter[0]+'&category='+filter[1];
    this.httpService.observableGetHttp(pathUrl,null,false)
    .subscribe((res)=> {
        this.similarListing = res;
      },
      error => {
        console.log("error in response");
      },
      ()=>{
        console.log("Finally");
        this.similarListingLoaded.emit('loaded');
      })
  }


}

