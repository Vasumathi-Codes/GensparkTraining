
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserDto, CreateUserDto, UpdateUserDto } from '../models/user-dto.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private apiUrl = 'http://localhost:5000/api/v1/Users'; 

  constructor(private http: HttpClient) {}

  getUsers(
    page = 1, pageSize = 5, searchTerm = '', role = '', 
    sortBy = 'CreatedAt', sortOrder = 'asc'
  ): Observable<ApiResponse<{ $values: UserDto[] }>> {
    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize)
      .set('searchTerm', searchTerm)
      .set('role', role)
      .set('sortBy', sortBy)
      .set('sortOrder', sortOrder);

    return this.http.get<ApiResponse<{ $values: UserDto[] }>>(this.apiUrl, { params });
  }


  getUserById(id: string): Observable<ApiResponse<UserDto>> {
    return this.http.get<ApiResponse<UserDto>>(`${this.apiUrl}/${id}`);
  }

  createUser(user: CreateUserDto): Observable<ApiResponse<UserDto>> {
    return this.http.post<ApiResponse<UserDto>>(this.apiUrl, user);
  }

  updateUser(id: string, user: UpdateUserDto): Observable<ApiResponse<UserDto>> {
    return this.http.put<ApiResponse<UserDto>>(`${this.apiUrl}/${id}`, user);
  }

  deleteUser(id: string): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.apiUrl}/${id}`);
  }
}
