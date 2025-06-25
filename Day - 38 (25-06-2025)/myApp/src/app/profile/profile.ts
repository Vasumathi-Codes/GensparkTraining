import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
  init_forms
} from "./chunk-MVVWFVDH.js";
import {
  UserService,
  init_UserService
} from "./chunk-HKDSAIXA.js";
import {
  Router,
  init_router
} from "./chunk-H2KSZ5RO.js";
import "./chunk-VLBNHNUJ.js";
import "./chunk-JHNGGVN7.js";
import "./chunk-LVXYI34E.js";
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
  __commonJS,
  __esm
} from "./chunk-TTULUY32.js";

// angular:jit:template:src/app/login/login.html
var login_default;
var init_login = __esm({
  "angular:jit:template:src/app/login/login.html"() {
    login_default = '<!-- <div class="loginDiv">\n    <label class="form-control">Username</label>\n    <input class="form-control" type="text" required [(ngModel)]="user.username" #un="ngModel"/>\n    @if(un.control.touched && un.control.errors)\n    {\n        <span class="alert alert-danger">Username cannot be empty</span>\n    }\n     <label class="form-control">Password</label>\n    <input class="form-control" type="text" required [(ngModel)]="user.password" #pass="ngModel"/>\n     @if(pass.control.touched && pass.control.errors)\n    {\n        <span class="alert alert-danger">Psssword cannot be empty</span>\n    }\n    <button class="btn btn-success" (click)="handleLogin(un,pass)">Login</button>\n</div> -->\n<form [formGroup]="loginForm" class="loginDiv" (ngSubmit)="handleLogin()">\n    <label class="form-control">Username</label>\n    <input class="form-control" type="text" formControlName="un" />\n    @if(un.touched && un.errors)\n    {\n        <span class="alert alert-danger">Username cannot be empty</span>\n    }\n     <label class="form-control">Password</label>\n    <input class="form-control" type="text" formControlName="pass" />\n     @if(pass.touched && pass.errors)\n    {\n        @if (pass.errors?.required) {\n            <span class="alert alert-danger">Psssword cannot be empty</span>\n        }\n        @if (pass.errors?.lenError) {\n            <span class="alert alert-danger">Psssword cannot be less than 5 chars</span>\n        }\n\n    }\n    <button class="btn btn-success" [disabled]="loginForm.invalid" >Login</button>\n</form>';
  }
});

// angular:jit:style:src/app/login/login.css
var login_default2;
var init_login2 = __esm({
  "angular:jit:style:src/app/login/login.css"() {
    login_default2 = "/* src/app/login/login.css */\n.loginDiv {\n  width: 40%;\n}\n/*# sourceMappingURL=login.css.map */\n";
  }
});

// src/app/models/UserLoginModel.ts
var UserLoginModel;
var init_UserLoginModel = __esm({
  "src/app/models/UserLoginModel.ts"() {
    "use strict";
    UserLoginModel = class {
      username;
      password;
      constructor(username = "", password = "") {
        this.username = username;
        this.password = password;
      }
    };
  }
});

// src/app/misc/TextValidator.ts
function textValidator() {
  return (control) => {
    const value = control.value;
    if (value?.length < 6)
      return { lenError: "password is of worng length" };
    return null;
  };
}
var init_TextValidator = __esm({
  "src/app/misc/TextValidator.ts"() {
    "use strict";
  }
});

// src/app/login/login.ts
var Login;
var init_login3 = __esm({
  "src/app/login/login.ts"() {
    "use strict";
    init_tslib_es6();
    init_login();
    init_login2();
    init_core();
    init_UserLoginModel();
    init_UserService();
    init_forms();
    init_router();
    init_TextValidator();
    Login = class Login2 {
      userService;
      route;
      user = new UserLoginModel();
      loginForm;
      constructor(userService, route) {
        this.userService = userService;
        this.route = route;
        this.loginForm = new FormGroup({
          un: new FormControl(null, Validators.required),
          pass: new FormControl(null, [Validators.required, textValidator()])
        });
      }
      get un() {
        return this.loginForm.get("un");
      }
      get pass() {
        return this.loginForm.get("pass");
      }
      // handleLogin(un:any,pass:any){
      //   console.log(un.control.touched)
      //   if(un.control.errors || pass.control.errors)
      //     return;
      //   this.userService.validateUserLogin(this.user);
      //   this.route.navigateByUrl("/home/"+this.user.username);
      // }
      handleLogin() {
        console.log(this.pass);
        if (this.loginForm.invalid)
          return;
        this.userService.validateUserLogin(this.user);
        this.route.navigateByUrl("/home/" + this.user.username);
      }
      static ctorParameters = () => [
        { type: UserService },
        { type: Router }
      ];
    };
    Login = __decorate([
      Component({
        selector: "app-login",
        imports: [FormsModule, ReactiveFormsModule],
        template: login_default,
        styles: [login_default2]
      })
    ], Login);
  }
});

// src/app/login/login.spec.ts
var require_login_spec = __commonJS({
  "src/app/login/login.spec.ts"(exports) {
    init_testing();
    init_login3();
    describe("Login", () => {
      let component;
      let fixture;
      beforeEach(() => __async(null, null, function* () {
        yield TestBed.configureTestingModule({
          imports: [Login]
        }).compileComponents();
        fixture = TestBed.createComponent(Login);
        component = fixture.componentInstance;
        fixture.detectChanges();
      }));
      it("should create", () => {
        expect(component).toBeTruthy();
      });
    });
  }
});
export default require_login_spec();
//# sourceMappingURL=spec-app-login-login.spec.js.map
