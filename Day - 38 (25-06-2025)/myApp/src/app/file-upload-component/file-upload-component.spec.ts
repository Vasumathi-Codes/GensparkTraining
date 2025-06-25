import {
  HttpClient,
  HttpHeaders,
  init_http
} from "./chunk-JHNGGVN7.js";
import {
  BehaviorSubject,
  Injectable,
  __decorate,
  init_core,
  init_esm,
  init_tslib_es6,
  inject
} from "./chunk-JDYYP2R5.js";
import {
  __esm
} from "./chunk-TTULUY32.js";

// src/app/services/UserService.ts
var UserService;
var init_UserService = __esm({
  "src/app/services/UserService.ts"() {
    "use strict";
    init_tslib_es6();
    init_esm();
    init_http();
    init_core();
    UserService = class UserService2 {
      http = inject(HttpClient);
      usernameSubject = new BehaviorSubject(null);
      username$ = this.usernameSubject.asObservable();
      validateUserLogin(user) {
        if (user.username.length < 3) {
          this.usernameSubject.next(null);
        } else {
          this.callLoginAPI(user).subscribe({
            next: (data) => {
              this.usernameSubject.next(user.username);
              localStorage.setItem("token", data.accessToken);
            }
          });
        }
      }
      callGetProfile() {
        var token = localStorage.getItem("token");
        const httpHeader = new HttpHeaders({
          "Authorization": `Bearer ${token}`
        });
        return this.http.get("https://dummyjson.com/auth/me", { headers: httpHeader });
      }
      callLoginAPI(user) {
        return this.http.post("https://dummyjson.com/auth/login", user);
      }
      logout() {
        this.usernameSubject.next(null);
      }
    };
    UserService = __decorate([
      Injectable()
    ], UserService);
  }
});

export {
  UserService,
  init_UserService
};
//# sourceMappingURL=chunk-HKDSAIXA.js.map
