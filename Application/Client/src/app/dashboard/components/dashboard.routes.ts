import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from '../../home/component/home.component';
import { ProductInfoComponent } from '../../product-info';
import { LoginComponent } from '../../_common/login/component/login.component';
import { ProfileComponent } from '../../createProfile/component/createProfile.component';
import { CreateCardComponent } from '../../createCard/component/createCard.component';
import { DashboardComponent } from './dashboard.component';
import { CREATE_CARD_ROUTERS } from '../../createCard/component/create.card.routes';


export const DASHBOARD_ROUTERS: Routes = [
  {
    path: 'dashboard',
    component: DashboardComponent,
    children:[
      {path: '', component: HomeComponent},
      {path: 'home', component: HomeComponent},
      {path: 'profile', component: ProfileComponent},
      {path: 'profile/:usermail', component: ProfileComponent},
      {path: 'productInfo', component: ProductInfoComponent},
      {path: 'productInfo/:id', component: ProductInfoComponent},
      ...CREATE_CARD_ROUTERS
  ]
}
];

