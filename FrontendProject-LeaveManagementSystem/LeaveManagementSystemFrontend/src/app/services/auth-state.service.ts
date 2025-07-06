import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthStateService {
  private roleSubject = new BehaviorSubject<string | null>(this.getRole());
  role$ = this.roleSubject.asObservable();

  private isLoggedInSubject = new BehaviorSubject<boolean>(this.hasToken());
  isLoggedIn$ = this.isLoggedInSubject.asObservable();

  private usernameSubject = new BehaviorSubject<string | null>(this.getUsername());
  username$ = this.usernameSubject.asObservable();

  private hasToken() {
    return !!localStorage.getItem('accessToken');
  }

  public getRole() {
    return localStorage.getItem('role');
  }

  private getUsername() {
    return localStorage.getItem('username');
  }

  setAuthState(isLoggedIn: boolean, role: string | null, username: string | null) {
    this.isLoggedInSubject.next(isLoggedIn);
    this.roleSubject.next(role);
    this.usernameSubject.next(username);  
  }

  logout() {
    localStorage.clear();
    this.setAuthState(false, null, null);
  }
}
