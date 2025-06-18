import { Routes } from '@angular/router';
import { UserAddComponent } from './components/user-add/user-add';
import { UserListComponent } from './components/user-list/user-list';
// import { UserDashboardComponent } from './components/user-dashboard/user-dashboard.component';

export const appRoutes: Routes = [
  { path: '', redirectTo: 'add', pathMatch: 'full' }, // default route
  { path: 'add', component: UserAddComponent },
  { path: 'list', component: UserListComponent },
//   { path: 'dashboard', component: UserDashboardComponent }
];
