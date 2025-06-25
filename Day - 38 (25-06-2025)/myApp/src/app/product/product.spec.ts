import {
  UserService,
  init_UserService
} from "./chunk-HKDSAIXA.js";
import "./chunk-JHNGGVN7.js";
import "./chunk-ZCYQ42IE.js";
import {
  Component,
  TestBed,
  __decorate,
  init_core,
  init_testing,
  init_tslib_es6,
  inject
} from "./chunk-JDYYP2R5.js";
import {
  __async,
  __commonJS,
  __esm
} from "./chunk-TTULUY32.js";

// angular:jit:template:src/app/profile/profile.html
var profile_default;
var init_profile = __esm({
  "angular:jit:template:src/app/profile/profile.html"() {
    profile_default = `<p>profile works!</p>
<div class="card" style="width: 18rem;">
  <img class="card-img-top" [src]="profileData?.image" alt="Card image cap">
  <div class="card-body">
    <h5 class="card-title">{{profileData?.firstName}}  {{profileData?.lastName}} </h5>
    <p class="card-text">Some quick example text to build on the card title and make up the bulk of the card's content.</p>
    <a href="#" class="btn btn-primary"></a>
  </div>
</div>`;
  }
});

// angular:jit:style:src/app/profile/profile.css
var profile_default2;
var init_profile2 = __esm({
  "angular:jit:style:src/app/profile/profile.css"() {
    profile_default2 = "/* src/app/profile/profile.css */\n/*# sourceMappingURL=profile.css.map */\n";
  }
});

// src/app/models/UserModel.ts
var UserModel;
var init_UserModel = __esm({
  "src/app/models/UserModel.ts"() {
    "use strict";
    UserModel = class _UserModel {
      id;
      username;
      email;
      firstName;
      lastName;
      gender;
      image;
      //     {
      //   "id": 1,
      //   "username": "emilys",
      //   "email": "emily.johnson@x.dummyjson.com",
      //   "firstName": "Emily",
      //   "lastName": "Johnson",
      //   "gender": "female",
      //   "image": "https://dummyjson.com/icon/emilys/128"
      // }
      constructor(id = 0, username = "", email = "", firstName = "", lastName = "", gender = "", image = "") {
        this.id = id;
        this.username = username;
        this.email = email;
        this.firstName = firstName;
        this.lastName = lastName;
        this.gender = gender;
        this.image = image;
      }
      static fromForm(data) {
        return new _UserModel(data.id, data.username, data.email, data.firstName, data.lastName, data.gender, data.image);
      }
    };
  }
});

// src/app/profile/profile.ts
var Profile;
var init_profile3 = __esm({
  "src/app/profile/profile.ts"() {
    "use strict";
    init_tslib_es6();
    init_profile();
    init_profile2();
    init_core();
    init_UserService();
    init_UserModel();
    Profile = class Profile2 {
      userService = inject(UserService);
      profileData = new UserModel();
      constructor() {
        this.userService.callGetProfile().subscribe({
          next: (data) => {
            this.profileData = UserModel.fromForm(data);
          }
        });
      }
      static ctorParameters = () => [];
    };
    Profile = __decorate([
      Component({
        selector: "app-profile",
        imports: [],
        template: profile_default,
        styles: [profile_default2]
      })
    ], Profile);
  }
});

// src/app/profile/profile.spec.ts
var require_profile_spec = __commonJS({
  "src/app/profile/profile.spec.ts"(exports) {
    init_testing();
    init_profile3();
    describe("Profile", () => {
      let component;
      let fixture;
      beforeEach(() => __async(null, null, function* () {
        yield TestBed.configureTestingModule({
          imports: [Profile]
        }).compileComponents();
        fixture = TestBed.createComponent(Profile);
        component = fixture.componentInstance;
        fixture.detectChanges();
      }));
      it("should create", () => {
        expect(component).toBeTruthy();
      });
    });
  }
});
export default require_profile_spec();
//# sourceMappingURL=spec-app-profile-profile.spec.js.map
