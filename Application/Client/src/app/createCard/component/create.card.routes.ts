import { Routes, RouterModule } from '@angular/router';
import { CreateCardComponent } from './createCard.component';


export const CREATE_CARD_ROUTERS: Routes = [
  {path: 'createCard', component: CreateCardComponent},
  {path: 'createCard/:id', component: CreateCardComponent},
];

