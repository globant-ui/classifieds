import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home';
import { ProductInfoComponent } from './product-info';
import { LoginComponent } from './_common/login/component/login.component';
import { ProfileComponent } from './createProfile/component/createProfile.component';
import { CreateCardComponent } from './createCard/component/createCard.component';


export const ROUTES: Routes = [
  { path: '', component: LoginComponent},
  {
    path: 'home', component: HomeComponent
  },
  {path: 'productInfo', component: ProductInfoComponent},
  {path: 'productInfo/:id', component: ProductInfoComponent},
  {path: 'createCard', component: CreateCardComponent},
  {path: 'createCard/:id', component: CreateCardComponent},
  {path: 'profile/:usermail', component: ProfileComponent},


];

