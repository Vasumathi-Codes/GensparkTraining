import {
  provideLocationMocks
} from "./chunk-M3A56FKC.js";
import {
  NoPreloading,
  ROUTER_CONFIGURATION,
  ROUTER_PROVIDERS,
  ROUTES,
  Router,
  RouterModule,
  RouterOutlet,
  afterNextNavigation,
  init_router,
  init_router_CsukTOog,
  init_router_module_DTmwsUYo,
  withPreloading
} from "./chunk-H2KSZ5RO.js";
import "./chunk-VLBNHNUJ.js";
import "./chunk-JHNGGVN7.js";
import "./chunk-LVXYI34E.js";
import "./chunk-ZCYQ42IE.js";
import {
  Component,
  FactoryTarget,
  Injectable,
  NgModule,
  TestBed,
  ViewChild,
  __decorate,
  core_exports,
  init_core,
  init_testing,
  init_tslib_es6,
  signal,
  ɵɵngDeclareClassMetadata,
  ɵɵngDeclareComponent,
  ɵɵngDeclareFactory,
  ɵɵngDeclareInjectable,
  ɵɵngDeclareInjector,
  ɵɵngDeclareNgModule
} from "./chunk-JDYYP2R5.js";
import {
  __async
} from "./chunk-TTULUY32.js";

// src/app/auth-guard.spec.ts
init_testing();

// src/app/auth-guard.ts
init_tslib_es6();
init_core();
init_router();
var AuthGuard = class AuthGuard2 {
  router;
  constructor(router) {
    this.router = router;
  }
  canActivate(route, state) {
    const isAuthenticated = localStorage.getItem("token") ? true : false;
    if (!isAuthenticated) {
      this.router.navigate(["login"]);
      return false;
    }
    return true;
  }
  static ctorParameters = () => [
    { type: Router }
  ];
};
AuthGuard = __decorate([
  Injectable()
], AuthGuard);

// src/app/auth-guard.spec.ts
init_router();

// node_modules/@angular/router/fesm2022/testing.mjs
init_core();
init_core();
init_testing();
init_router_CsukTOog();
init_router_module_DTmwsUYo();
var RouterTestingModule = class _RouterTestingModule {
  static withRoutes(routes, config) {
    return {
      ngModule: _RouterTestingModule,
      providers: [
        { provide: ROUTES, multi: true, useValue: routes },
        { provide: ROUTER_CONFIGURATION, useValue: config ? config : {} }
      ]
    };
  }
  static \u0275fac = \u0275\u0275ngDeclareFactory({ minVersion: "12.0.0", version: "20.0.4", ngImport: core_exports, type: _RouterTestingModule, deps: [], target: FactoryTarget.NgModule });
  static \u0275mod = \u0275\u0275ngDeclareNgModule({ minVersion: "14.0.0", version: "20.0.4", ngImport: core_exports, type: _RouterTestingModule, exports: [RouterModule] });
  static \u0275inj = \u0275\u0275ngDeclareInjector({ minVersion: "12.0.0", version: "20.0.4", ngImport: core_exports, type: _RouterTestingModule, providers: [
    ROUTER_PROVIDERS,
    provideLocationMocks(),
    withPreloading(NoPreloading).\u0275providers,
    { provide: ROUTES, multi: true, useValue: [] }
  ], imports: [RouterModule] });
};
\u0275\u0275ngDeclareClassMetadata({ minVersion: "12.0.0", version: "20.0.4", ngImport: core_exports, type: RouterTestingModule, decorators: [{
  type: NgModule,
  args: [{
    exports: [RouterModule],
    providers: [
      ROUTER_PROVIDERS,
      provideLocationMocks(),
      withPreloading(NoPreloading).\u0275providers,
      { provide: ROUTES, multi: true, useValue: [] }
    ]
  }]
}] });
var RootFixtureService = class _RootFixtureService {
  fixture;
  harness;
  createHarness() {
    if (this.harness) {
      throw new Error("Only one harness should be created per test.");
    }
    this.harness = new RouterTestingHarness(this.getRootFixture());
    return this.harness;
  }
  getRootFixture() {
    if (this.fixture !== void 0) {
      return this.fixture;
    }
    this.fixture = TestBed.createComponent(RootCmp);
    this.fixture.detectChanges();
    return this.fixture;
  }
  static \u0275fac = \u0275\u0275ngDeclareFactory({ minVersion: "12.0.0", version: "20.0.4", ngImport: core_exports, type: _RootFixtureService, deps: [], target: FactoryTarget.Injectable });
  static \u0275prov = \u0275\u0275ngDeclareInjectable({ minVersion: "12.0.0", version: "20.0.4", ngImport: core_exports, type: _RootFixtureService, providedIn: "root" });
};
\u0275\u0275ngDeclareClassMetadata({ minVersion: "12.0.0", version: "20.0.4", ngImport: core_exports, type: RootFixtureService, decorators: [{
  type: Injectable,
  args: [{ providedIn: "root" }]
}] });
var RootCmp = class _RootCmp {
  outlet;
  routerOutletData = signal(void 0);
  static \u0275fac = \u0275\u0275ngDeclareFactory({ minVersion: "12.0.0", version: "20.0.4", ngImport: core_exports, type: _RootCmp, deps: [], target: FactoryTarget.Component });
  static \u0275cmp = \u0275\u0275ngDeclareComponent({ minVersion: "14.0.0", version: "20.0.4", type: _RootCmp, isStandalone: true, selector: "ng-component", viewQueries: [{ propertyName: "outlet", first: true, predicate: RouterOutlet, descendants: true }], ngImport: core_exports, template: '<router-outlet [routerOutletData]="routerOutletData()"></router-outlet>', isInline: true, dependencies: [{ kind: "directive", type: RouterOutlet, selector: "router-outlet", inputs: ["name", "routerOutletData"], outputs: ["activate", "deactivate", "attach", "detach"], exportAs: ["outlet"] }] });
};
\u0275\u0275ngDeclareClassMetadata({ minVersion: "12.0.0", version: "20.0.4", ngImport: core_exports, type: RootCmp, decorators: [{
  type: Component,
  args: [{
    template: '<router-outlet [routerOutletData]="routerOutletData()"></router-outlet>',
    imports: [RouterOutlet]
  }]
}], propDecorators: { outlet: [{
  type: ViewChild,
  args: [RouterOutlet]
}] } });
var RouterTestingHarness = class {
  /**
   * Creates a `RouterTestingHarness` instance.
   *
   * The `RouterTestingHarness` also creates its own root component with a `RouterOutlet` for the
   * purposes of rendering route components.
   *
   * Throws an error if an instance has already been created.
   * Use of this harness also requires `destroyAfterEach: true` in the `ModuleTeardownOptions`
   *
   * @param initialUrl The target of navigation to trigger before returning the harness.
   */
  static create(initialUrl) {
    return __async(this, null, function* () {
      const harness = TestBed.inject(RootFixtureService).createHarness();
      if (initialUrl !== void 0) {
        yield harness.navigateByUrl(initialUrl);
      }
      return harness;
    });
  }
  /**
   * Fixture of the root component of the RouterTestingHarness
   */
  fixture;
  /** @internal */
  constructor(fixture) {
    this.fixture = fixture;
  }
  /** Instructs the root fixture to run change detection. */
  detectChanges() {
    this.fixture.detectChanges();
  }
  /** The `DebugElement` of the `RouterOutlet` component. `null` if the outlet is not activated. */
  get routeDebugElement() {
    const outlet = this.fixture.componentInstance.outlet;
    if (!outlet || !outlet.isActivated) {
      return null;
    }
    return this.fixture.debugElement.query((v) => v.componentInstance === outlet.component);
  }
  /** The native element of the `RouterOutlet` component. `null` if the outlet is not activated. */
  get routeNativeElement() {
    return this.routeDebugElement?.nativeElement ?? null;
  }
  navigateByUrl(url, requiredRoutedComponentType) {
    return __async(this, null, function* () {
      const router = TestBed.inject(Router);
      let resolveFn;
      const redirectTrackingPromise = new Promise((resolve) => {
        resolveFn = resolve;
      });
      afterNextNavigation(TestBed.inject(Router), resolveFn);
      yield router.navigateByUrl(url);
      yield redirectTrackingPromise;
      this.fixture.detectChanges();
      const outlet = this.fixture.componentInstance.outlet;
      if (outlet && outlet.isActivated && outlet.activatedRoute.component) {
        const activatedComponent = outlet.component;
        if (requiredRoutedComponentType !== void 0 && !(activatedComponent instanceof requiredRoutedComponentType)) {
          throw new Error(`Unexpected routed component type. Expected ${requiredRoutedComponentType.name} but got ${activatedComponent.constructor.name}`);
        }
        return activatedComponent;
      } else {
        if (requiredRoutedComponentType !== void 0) {
          throw new Error(`Unexpected routed component type. Expected ${requiredRoutedComponentType.name} but the navigation did not activate any component.`);
        }
        return null;
      }
    });
  }
};

// src/app/auth-guard.spec.ts
describe("AuthGuard", () => {
  let guard;
  let router;
  let route;
  let state;
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule.withRoutes([])],
      providers: [AuthGuard]
    });
    guard = TestBed.inject(AuthGuard);
    router = TestBed.inject(Router);
    route = {};
    state = { url: "/protected" };
  });
  afterEach(() => {
    localStorage.clear();
  });
  it("should allow activation if token exists", () => {
    localStorage.setItem("token", "fake-token");
    const result = guard.canActivate(route, state);
    expect(result).toBeTrue();
  });
  it("should block activation and redirect if token is missing", () => {
    spyOn(router, "navigate");
    localStorage.removeItem("token");
    const result = guard.canActivate(route, state);
    expect(result).toBeFalse();
    expect(router.navigate).toHaveBeenCalledWith(["login"]);
  });
});
/*! Bundled license information:

@angular/router/fesm2022/testing.mjs:
  (**
   * @license Angular v20.0.4
   * (c) 2010-2025 Google LLC. https://angular.io/
   * License: MIT
   *)
*/
//# sourceMappingURL=spec-app-auth-guard.spec.js.map
