import { Routes } from '@angular/router';
import { Product } from './product/product'; // Correct path (not './product/product')

export const routes: Routes = [
  { path: 'product', component: Product }, // Route to ProductComponent
  { path: '', redirectTo: 'product', pathMatch: 'full' } // Optional: default route redirects to /product
];
