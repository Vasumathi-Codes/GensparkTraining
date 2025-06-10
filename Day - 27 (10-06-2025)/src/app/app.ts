import { Component } from '@angular/core';
import { First } from "./first/first";
import { CustomerDetails } from '../app/customer-details/customer-details';
import { ProductList } from '../app/product-list/product-list';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [First, CustomerDetails, ProductList]
})
export class App {
  protected title = 'myApp';
}