import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Product } from '../../models/product.model';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-card.html',
  styleUrls: ['./product-card.css']
})
export class ProductCard {
  @Input() product!: Product;
  @Input() searchTerm: string = '';

  get highlightedTitle() {
    if (!this.searchTerm) return this.product.title;
    const re = new RegExp(this.searchTerm, 'gi');
    return this.product.title.replace(re, (match: string) => `<mark>${match}</mark>`);
  }
}
