import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home';
import { LoginComponent } from './_common/login/component/login.component';
import { BannerComponent } from './banner/components/banner.component';

import { CreateListingComponent } from './_common/createListing/component/createListing.component';


export const ROUTES: Routes = [
  { path: '', component: LoginComponent},
  { path: 'home', component: HomeComponent },
  { path: 'createListing', component: CreateListingComponent }
];
