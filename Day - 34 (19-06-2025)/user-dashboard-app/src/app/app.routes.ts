import { Routes } from '@angular/router';
import { UserFormComponent } from './components/user-form/user-form';
import { UserListComponent } from './components/user-list/user-list';

export const routes: Routes = [
  { path: '', redirectTo: 'user-list', pathMatch: 'full' }, 
  { path: 'user-form', component: UserFormComponent },
  { path: 'user-list', component: UserListComponent }
];
