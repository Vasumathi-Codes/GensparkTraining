import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private usersUrl = 'https://dummyjson.com/users';
  private addUserUrl = 'https://dummyjson.com/users/add';

  constructor(private http: HttpClient) { }

  addUser(user: any): Observable<any> {
    return this.http.post(this.addUserUrl, user);
  }

  getAllUsers(): Observable<any> {
    return this.http.get<any>(this.usersUrl);
  }
}


