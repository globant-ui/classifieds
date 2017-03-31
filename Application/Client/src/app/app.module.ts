import { NgModule, ApplicationRef } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { DatePipe } from '@angular/common';
import {CookieService} from 'angular2-cookie/core';
import {CService} from "./_common/services/http.service";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule, PreloadAllModules } from '@angular/router';
import { removeNgStyles, createNewHosts, createInputTransfer } from '@angularclass/hmr';

import { ENV_PROVIDERS } from './environment';
import { APP_ROUTERS } from './app.routes';
import {DASHBOARD_ROUTERS } from './dashboard/components/dashboard.routes';
import { AppComponent } from './app.component';
import { APP_RESOLVER_PROVIDERS } from './app.resolver';
import { AppState, InternalStateType } from './app.service';
import { HomeComponent } from './home/component/home.component';
import { ProgressComponent } from './_common/progress/components/progress.component';

import { HeaderComponent } from './_common/header/components/header.component';
import { SimilarListingComponent } from './_common/similarListing/components/similarListing.component';

import { CardListComponent } from './card-list/components/card-list.component';
import { BannerComponent } from './banner/components/banner.component';
import { DashboardComponent } from './dashboard/components/dashboard.component';
import { ProductInfoComponent } from './product-info/components/product-info.component';
import {Base64Service} from "./_common/services/base64.service";
import {Broadcaster} from "./_common/services/broadcast.service";
import { LoginComponent } from './_common/login/component/login.component';
import { CreateCardComponent } from './createCard/component/createCard.component';
import { ProfileComponent } from './createProfile/component/createProfile.component';
import { MyListingsComponent } from './myListings/component/myListings.component';
import { MapData} from  './mapData/mapData';

import { FilterComponent } from './filter/components/filter.component';
import { SearchComponent } from './_common/search/components/search.component';
import { LoaderComponent } from './_common/loader/components/loader.component';
import { SelectInterestComponent } from './_common/select-interest/components/select-interest.component';
import { WishListComponent } from './_common/wishlist/components/wishlist.component';
import  {SettingsService} from  './_common/services/setting.service';
import { PopUpMessageComponent } from './_common/popup';
import  {WishListService} from  './_common/wishlist/service/wishlist.service';
import { PageNotFoundComponent } from './_common/page-not-found';
import { ExploreComponent } from './_common/explore-list';

/*ng2-bootstrap*/
import { ModalModule } from 'ng2-bootstrap';
import { CollapseModule } from 'ng2-bootstrap/collapse';
import { DropdownModule } from 'ng2-bootstrap/dropdown';
import { AccordionModule } from 'ng2-bootstrap/accordion';
import { CarouselModule } from 'ng2-bootstrap/carousel';

const APP_PROVIDERS = [
  ...APP_RESOLVER_PROVIDERS,
  AppState
];

type StoreType = {
  state: InternalStateType,
  restoreInputValues: () => void,
  disposeOldHosts: () => void
};

/**
 * `AppModule` is the main entry point into Angular2's bootstraping process
 */

@NgModule({
  bootstrap: [ AppComponent ],
  declarations: [
    AppComponent,
    HomeComponent,
    HeaderComponent,
    DashboardComponent,
    ProgressComponent,
    CardListComponent,
    SimilarListingComponent,
    BannerComponent,
    SearchComponent,
    FilterComponent,
    LoginComponent,
    LoaderComponent,
    SelectInterestComponent,
    ProductInfoComponent,
    CreateCardComponent,
    ProfileComponent,
    WishListComponent,
    MyListingsComponent,
    PopUpMessageComponent,
    PageNotFoundComponent,
    ExploreComponent
  ],
  imports: [ // import Angular's modules
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    RouterModule.forRoot(APP_ROUTERS, { useHash: true, preloadingStrategy: PreloadAllModules }),
    RouterModule.forRoot(DASHBOARD_ROUTERS, { useHash: true, preloadingStrategy: PreloadAllModules }),
    ModalModule.forRoot(),
    CollapseModule.forRoot(),
    DropdownModule.forRoot(),
    AccordionModule.forRoot(),
    CarouselModule.forRoot()
  ],
  providers: [ // expose our Services and Providers into Angular's dependency injection
    ENV_PROVIDERS,
    APP_PROVIDERS,
    CService,
    MapData,
    CookieService,
    SettingsService,
    Base64Service,
    DatePipe,
    WishListService,
    Broadcaster
  ]
})
export class AppModule {
  constructor(public appRef: ApplicationRef, public appState: AppState) {}

  hmrOnInit(store: StoreType) {
    if (!store || !store.state) return;
    //console.log('HMR store', JSON.stringify(store, null, 2));
    // set state
    this.appState._state = store.state;
    // set input values
    if ('restoreInputValues' in store) {
      let restoreInputValues = store.restoreInputValues;
      setTimeout(restoreInputValues);
    }

    this.appRef.tick();
    delete store.state;
    delete store.restoreInputValues;
  }

  hmrOnDestroy(store: StoreType) {
    const cmpLocation = this.appRef.components.map(cmp => cmp.location.nativeElement);
    // save state
    const state = this.appState._state;
    store.state = state;
    // recreate root elements
    store.disposeOldHosts = createNewHosts(cmpLocation);
    // save input values
    store.restoreInputValues  = createInputTransfer();
    // remove styles
    removeNgStyles();
  }

  hmrAfterDestroy(store: StoreType) {
    // display new elements
    store.disposeOldHosts();
    delete store.disposeOldHosts;
  }

}
