import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home';
import { ProductInfoComponent } from './product-info';
import { LoginComponent } from './_common/login/component/login.component';



export const ROUTES: Routes = [
  { path: '', component: LoginComponent},
  { path: 'home', component: HomeComponent},
  { path: 'product-info', component: ProductInfoComponent}
];
