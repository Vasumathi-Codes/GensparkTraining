import { Routes } from '@angular/router';
import { Login } from './login/login';
import { Products } from './products/products';
import { ProductDetail } from './product-detail/product-detail';
import { authGuard } from './auth-guard';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'products', component: Products, canActivate: [authGuard] },
  { path: 'products/:id', component: ProductDetail, canActivate: [authGuard] },
  { path : '**', component: Login}
];
