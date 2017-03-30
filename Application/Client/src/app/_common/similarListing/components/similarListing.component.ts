import { Component,OnInit,Input,Output,EventEmitter,OnChanges,SimpleChange,NgZone} from '@angular/core';
import {CService} from  '../../services/http.service';
import {apiPaths} from  '../../../../serverConfig/apiPaths';
import {Router} from '@angular/router';


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

  constructor(private httpService:CService,private apiPath:apiPaths,private _router:Router,private zone:NgZone){}

  ngOnChanges(changes: {[propKey: string]: SimpleChange}){
    this.getSimilarListing();

  }

  showProductInfo(id){
    this._router.navigateByUrl('/dashboard/productInfo/'+id);
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

  openListing(id){
      this._router.navigateByUrl('/dashboard/productInfo/'+id);
    // this._router.navigateByUrl('/dashboard/productInfo/'+id,{ skipLocationChange: true });
  }


}

