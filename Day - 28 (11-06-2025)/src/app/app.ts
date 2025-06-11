import { Component } from '@angular/core';
import { First } from "./first/first";
import { CustomerDetails } from '../app/customer-details/customer-details';
import { ProductList } from '../app/product-list/product-list';
import { RouterModule } from '@angular/router';
import { RecipeComponent } from './recipe/recipe';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [First, CustomerDetails, ProductList, RouterModule, RecipeComponent]
})
export class App {
  protected title = 'myApp';
}