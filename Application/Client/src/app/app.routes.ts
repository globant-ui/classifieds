import { Routes, RouterModule, provideRoutes } from '@angular/router';
import { DASHBOARD_ROUTERS } from './dashboard/components/dashboard.routes';
import {DashboardComponent} from './dashboard/components/dashboard.component';
import {LoginComponent} from './_common/login/component/login.component';

import { HomeComponent } from './home/components/home.component';
import { HeaderComponent } from './_common/header/components/header.component';
import { ProductInfoComponent } from './product-info';
import { ProfileComponent } from './createProfile/component/createProfile.component';
import { CreateCardComponent } from './createCard/component/createCard.component';
import { ExploreComponent } from './_common/explore-list/components/explore-list.component';



export const APP_ROUTERS : Routes = [
  { path: '', component: LoginComponent}
];

export const routes : Routes = [
  ...DASHBOARD_ROUTERS,
  ...APP_ROUTERS
];

export const APP_ROUTER_PROVIDER= [
  provideRoutes( routes)
];

