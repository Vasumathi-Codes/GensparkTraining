import {
  Product,
  init_product2 as init_product
} from "./chunk-MZDBEONG.js";
import {
  ProductService,
  init_product_service
} from "./chunk-KZWBCYOO.js";
import {
  FormsModule,
  init_forms
} from "./chunk-MVVWFVDH.js";
import "./chunk-JHNGGVN7.js";
import "./chunk-LVXYI34E.js";
import "./chunk-ZCYQ42IE.js";
import {
  CUSTOM_ELEMENTS_SCHEMA,
  Component,
  HostListener,
  Subject,
  TestBed,
  __decorate,
  debounceTime,
  distinctUntilChanged,
  fakeAsync,
  init_core,
  init_esm,
  init_testing,
  init_tslib_es6,
  of,
  switchMap,
  tap,
  tick
} from "./chunk-JDYYP2R5.js";
import {
  __async,
  __commonJS,
  __esm
} from "./chunk-TTULUY32.js";

// angular:jit:template:src/app/products/products.html
var products_default;
var init_products = __esm({
  "angular:jit:template:src/app/products/products.html"() {
    products_default = '<button type="button" class="btn btn-primary">\n  Notifications <span class="badge badge-light">{{cartCount}}</span>\n</button>\n<input type="text" [(ngModel)]="searchString" (input)="handleSearchProducts()"/>\n  <div>\n        @if (cartCount>0) {\n            @for (item of cartItems; track item.Id) {\n            <li>{{item.Id}} -- {{item.Count}}</li>\n            }\n        }\n  </div>\n\n\n@if (!loading) {\n    <div class="fade-in">\n        @for (item of products; track item.id) {\n            <app-product (addToCart)="handleAddToCart($event)" [product]="item"></app-product>\n        }\n    </div>\n}\n@else {\n    <div>\n        <div class="spinner-border text-success" role="status">\n            <span class="sr-only"></span>\n        </div>\n    </div>\n}\n';
  }
});

// angular:jit:style:src/app/products/products.css
var products_default2;
var init_products2 = __esm({
  "angular:jit:style:src/app/products/products.css"() {
    products_default2 = "/* src/app/products/products.css */\n.fade-in {\n  opacity: 0;\n  animation: fadeIn 01s ease-in forwards;\n}\n@keyframes fadeIn {\n  to {\n    opacity: 1;\n  }\n}\n/*# sourceMappingURL=products.css.map */\n";
  }
});

// src/app/models/cartItem.ts
var CartItem;
var init_cartItem = __esm({
  "src/app/models/cartItem.ts"() {
    "use strict";
    CartItem = class {
      Id;
      Count;
      constructor(Id = 0, Count = 0) {
        this.Id = Id;
        this.Count = Count;
      }
    };
  }
});

// src/app/products/products.ts
var Products;
var init_products3 = __esm({
  "src/app/products/products.ts"() {
    "use strict";
    init_tslib_es6();
    init_products();
    init_products2();
    init_core();
    init_product_service();
    init_product();
    init_cartItem();
    init_forms();
    init_esm();
    Products = class Products2 {
      productService;
      products = [];
      cartItems = [];
      cartCount = 0;
      searchString = "";
      searchSubject = new Subject();
      loading = false;
      limit = 10;
      skip = 0;
      total = 0;
      constructor(productService) {
        this.productService = productService;
      }
      handleSearchProducts() {
        this.searchSubject.next(this.searchString);
      }
      handleAddToCart(event) {
        console.log("Handling add to cart - " + event);
        let flag = false;
        for (let i = 0; i < this.cartItems.length; i++) {
          if (this.cartItems[i].Id == event) {
            this.cartItems[i].Count++;
            flag = true;
          }
        }
        if (!flag)
          this.cartItems.push(new CartItem(event, 1));
        this.cartCount++;
      }
      ngOnInit() {
        this.searchSubject.pipe(debounceTime(5e3), distinctUntilChanged(), tap(() => this.loading = true), switchMap((query) => this.productService.getProductSearchResult(query, this.limit, this.skip)), tap(() => this.loading = false)).subscribe({
          next: (data) => {
            this.products = data.products;
            this.total = data.total;
            console.log(this.total);
          }
        });
      }
      onScroll() {
        const scrollPosition = window.innerHeight + window.scrollY;
        const threshold = document.body.offsetHeight - 100;
        if (scrollPosition >= threshold && this.products?.length < this.total) {
          console.log(scrollPosition);
          console.log(threshold);
          this.loadMore();
        }
      }
      loadMore() {
        this.loading = true;
        this.skip += this.limit;
        this.productService.getProductSearchResult(this.searchString, this.limit, this.skip).subscribe({
          next: (data) => {
            [...this.products, ...data.products];
            this.loading = false;
          }
        });
      }
      static ctorParameters = () => [
        { type: ProductService }
      ];
      static propDecorators = {
        onScroll: [{ type: HostListener, args: ["window:scroll", []] }]
      };
    };
    Products = __decorate([
      Component({
        selector: "app-products",
        imports: [Product, FormsModule],
        template: products_default,
        styles: [products_default2]
      })
    ], Products);
  }
});

// src/app/products/products.spec.ts
var require_products_spec = __commonJS({
  "src/app/products/products.spec.ts"(exports) {
    init_tslib_es6();
    init_testing();
    init_products3();
    init_product_service();
    init_forms();
    init_esm();
    init_core();
    init_cartItem();
    init_core();
    var MockProductComponent = class MockProductComponent {
    };
    MockProductComponent = __decorate([
      Component({
        selector: "app-product",
        template: ""
      })
    ], MockProductComponent);
    describe("Products Component (Standalone)", () => {
      let component;
      let fixture;
      let productServiceSpy;
      const dummyProductData = {
        products: [
          { id: 1, title: "Phone" },
          { id: 2, title: "Laptop" }
        ],
        total: 20
      };
      beforeEach(() => __async(null, null, function* () {
        const spy = jasmine.createSpyObj("ProductService", ["getProductSearchResult"]);
        yield TestBed.configureTestingModule({
          imports: [Products, FormsModule],
          // Include Products component as standalone
          providers: [{ provide: ProductService, useValue: spy }],
          schemas: [CUSTOM_ELEMENTS_SCHEMA]
          // to ignore <app-product> if not mocked
        }).compileComponents();
        fixture = TestBed.createComponent(Products);
        component = fixture.componentInstance;
        productServiceSpy = TestBed.inject(ProductService);
        productServiceSpy.getProductSearchResult.and.returnValue(of(dummyProductData));
        fixture.detectChanges();
      }));
      it("should create the component", () => {
        expect(component).toBeTruthy();
      });
      it("should add new item to cart", () => {
        component.handleAddToCart(1);
        expect(component.cartItems.length).toBe(1);
        expect(component.cartItems[0].Id).toBe(1);
        expect(component.cartItems[0].Count).toBe(1);
        expect(component.cartCount).toBe(1);
      });
      it("should increment item count if item already in cart", () => {
        component.cartItems = [new CartItem(1, 1)];
        component.cartCount = 1;
        component.handleAddToCart(1);
        expect(component.cartItems[0].Count).toBe(2);
        expect(component.cartCount).toBe(2);
      });
      it("should debounce and search products", fakeAsync(() => {
        component.searchString = "mobile";
        component.handleSearchProducts();
        tick(5e3);
        fixture.detectChanges();
        expect(productServiceSpy.getProductSearchResult).toHaveBeenCalledWith("mobile", 10, 0);
        expect(component.products.length).toBe(2);
        expect(component.total).toBe(20);
      }));
      it("should trigger loadMore on scroll near bottom", () => {
        spyOn(component, "loadMore");
        spyOnProperty(window, "innerHeight").and.returnValue(1e3);
        spyOnProperty(window, "scrollY").and.returnValue(900);
        Object.defineProperty(document.body, "offsetHeight", { value: 1800 });
        component.products = Array(10).fill({});
        component.total = 20;
        window.dispatchEvent(new Event("scroll"));
        expect(component.loadMore).toHaveBeenCalled();
      });
      it("should load more products and update skip", () => {
        const newProducts = {
          products: [{ id: 3, title: "Camera" }],
          total: 20
        };
        productServiceSpy.getProductSearchResult.and.returnValue(of(newProducts));
        component.products = [];
        component.searchString = "";
        component.skip = 0;
        component.loadMore();
        expect(productServiceSpy.getProductSearchResult).toHaveBeenCalledWith("", 10, 10);
        expect(component.loading).toBeFalse();
      });
    });
  }
});
export default require_products_spec();
//# sourceMappingURL=spec-app-products-products.spec.js.map
