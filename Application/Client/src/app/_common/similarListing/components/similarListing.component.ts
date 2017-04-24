import { Component, OnInit, Input, Output, EventEmitter,
  OnChanges, SimpleChange, NgZone } from '@angular/core';
import { CService } from  '../../services/http.service';
import { ApiPaths } from  '../../../../serverConfig/apiPaths';
import { Router } from '@angular/router';

@Component({
  selector: 'similar-listing',
  styles: [require('../styles/similarListing.component.scss').toString()],
  template: require('../tpls/similarListing.component.html').toString()
})
export class SimilarListingComponent implements OnChanges {

  @Input() private type;

  @Output() private similarListingLoaded: EventEmitter<string> = new EventEmitter();

  private similarListing;

  constructor(
    private httpService: CService,
    private apiPath: ApiPaths,
    private _router: Router,
    private zone: NgZone
    ) {}

  public ngOnChanges(changes: {[propKey: string]: SimpleChange}) {
    this.getSimilarListing();
  }

  private showProductInfo(id) {
    this._router.navigateByUrl('/dashboard/productInfo/' + id);
  }

  private getSimilarListing() {
    let pathUrl = '';
    let filter = this.type.split('-');
    pathUrl = this.apiPath.SIMILAR_LISTING + '?subCategory=' + filter[0]
    + '&category=' + filter[1];
    this.httpService.observableGetHttp(pathUrl, null, false)
    .subscribe((res) => {
        this.similarListing = res;
      },
      (error) => {
        console.log('error in response');
      },
      () => {
        console.log('Finally');
        this.similarListingLoaded.emit('loaded');
      });
  }

  private openListing(id) {
      this._router.navigateByUrl('/dashboard/productInfo/' + id);
  }
}
