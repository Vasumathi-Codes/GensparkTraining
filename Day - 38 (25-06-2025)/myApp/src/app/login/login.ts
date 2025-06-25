import {
  Product,
  ProductModel,
  init_product,
  init_product2
} from "./chunk-MZDBEONG.js";
import {
  init_testing as init_testing2,
  provideHttpClientTesting
} from "./chunk-NRWDHC5U.js";
import {
  ProductService,
  init_product_service
} from "./chunk-KZWBCYOO.js";
import {
  ActivatedRoute,
  init_router
} from "./chunk-H2KSZ5RO.js";
import "./chunk-VLBNHNUJ.js";
import {
  init_http,
  provideHttpClient
} from "./chunk-JHNGGVN7.js";
import {
  CurrencyPipe,
  init_common
} from "./chunk-LVXYI34E.js";
import "./chunk-ZCYQ42IE.js";
import {
  Component,
  TestBed,
  __decorate,
  init_core,
  init_testing,
  init_tslib_es6
} from "./chunk-JDYYP2R5.js";
import {
  __async,
  __commonJS
} from "./chunk-TTULUY32.js";

// src/app/product/product.spec.ts
var require_product_spec = __commonJS({
  "src/app/product/product.spec.ts"(exports) {
    init_tslib_es6();
    init_testing();
    init_product2();
    init_product();
    init_core();
    init_product_service();
    init_common();
    init_router();
    init_http();
    init_testing2();
    var MockProductService = class {
      getProduct(id) {
        return { subscribe: () => {
        } };
      }
    };
    var mockActivatedRoute = {
      snapshot: {
        paramMap: {
          get: (key) => {
            if (key == "id")
              return "1";
            return null;
          }
        }
      }
    };
    var HostComponent = class HostComponent {
      product = new ProductModel();
      addedProductId = null;
      onAdd(pid) {
        this.addedProductId = pid;
      }
    };
    HostComponent = __decorate([
      Component({
        standalone: true,
        imports: [Product],
        template: `<app-product [product]=product (addToCart)="onAdd($event)"></app-product>`
      })
    ], HostComponent);
    describe("Product", () => {
      let component;
      let fixture;
      let hostComponent;
      beforeEach(() => __async(null, null, function* () {
        yield TestBed.configureTestingModule({
          imports: [HostComponent],
          providers: [
            { provide: ProductService, useClass: MockProductService },
            { provide: ActivatedRoute, useValue: mockActivatedRoute },
            provideHttpClient(),
            provideHttpClientTesting(),
            CurrencyPipe
          ]
        }).compileComponents();
        fixture = TestBed.createComponent(HostComponent);
        hostComponent = fixture.componentInstance;
        fixture.detectChanges();
      }));
      it("check render product object input", () => {
        hostComponent.product = {
          id: 1,
          title: "Abc",
          price: 90,
          description: "blah blah"
        };
        fixture.detectChanges();
        const compiled = fixture.nativeElement;
        expect(compiled.textContent).toContain("Abc");
      });
    });
  }
});
export default require_product_spec();
//# sourceMappingURL=spec-app-product-product.spec.js.map
