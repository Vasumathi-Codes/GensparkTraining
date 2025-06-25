import {
  ProductService,
  init_product_service
} from "./chunk-KZWBCYOO.js";
import {
  CurrencyPipe,
  init_common
} from "./chunk-LVXYI34E.js";
import {
  Component,
  EventEmitter,
  Input,
  Output,
  __decorate,
  init_core,
  init_tslib_es6,
  inject
} from "./chunk-JDYYP2R5.js";
import {
  __esm
} from "./chunk-TTULUY32.js";

// src/app/models/product.ts
var ProductModel;
var init_product = __esm({
  "src/app/models/product.ts"() {
    "use strict";
    ProductModel = class {
      id;
      title;
      price;
      thumbnail;
      description;
      constructor(id = 0, title = "", price = 0, thumbnail = "", description = "") {
        this.id = id;
        this.title = title;
        this.price = price;
        this.thumbnail = thumbnail;
        this.description = description;
      }
    };
  }
});

// angular:jit:template:src/app/product/product.html
var product_default;
var init_product2 = __esm({
  "angular:jit:template:src/app/product/product.html"() {
    product_default = `<div class="card" style="width: 18rem;">
  <img class="card-img-top" [src]="product?.thumbnail" alt="Card image cap">
  <div class="card-body">
    <h5 class="card-title">{{product?.title}}</h5>
    <p class="card-text">{{product?.description}}</p>
    <button (click)="handleBuyClick(product?.id)" class="btn btn-primary">Buy for {{product?.price | currency:'INR'}}</button>
  </div>
</div>`;
  }
});

// angular:jit:style:src/app/product/product.css
var product_default2;
var init_product3 = __esm({
  "angular:jit:style:src/app/product/product.css"() {
    product_default2 = "/* src/app/product/product.css */\n/*# sourceMappingURL=product.css.map */\n";
  }
});

// src/app/product/product.ts
var Product;
var init_product4 = __esm({
  "src/app/product/product.ts"() {
    "use strict";
    init_tslib_es6();
    init_product2();
    init_product3();
    init_core();
    init_product_service();
    init_product();
    init_common();
    Product = class Product2 {
      product = new ProductModel();
      addToCart = new EventEmitter();
      productService = inject(ProductService);
      handleBuyClick(pid) {
        if (pid) {
          this.addToCart.emit(pid);
        }
      }
      constructor() {
      }
      static ctorParameters = () => [];
      static propDecorators = {
        product: [{ type: Input }],
        addToCart: [{ type: Output }]
      };
    };
    Product = __decorate([
      Component({
        selector: "app-product",
        imports: [CurrencyPipe],
        template: product_default,
        styles: [product_default2]
      })
    ], Product);
  }
});

export {
  ProductModel,
  init_product,
  Product,
  init_product4 as init_product2
};
//# sourceMappingURL=chunk-MZDBEONG.js.map
