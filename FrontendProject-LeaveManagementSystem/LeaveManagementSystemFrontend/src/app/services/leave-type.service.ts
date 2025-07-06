import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { LeaveType } from '../models/leave-type.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({ providedIn: 'root' })
export class LeaveTypeService {
  private apiUrl = 'http://localhost:5000/api/v1/leavetypes';

  constructor(private http: HttpClient) {}

  getLeaveTypes(): Observable<LeaveType[]> {
    return this.http
        .get<ApiResponse<{ $values: LeaveType[] }>>(this.apiUrl)
        .pipe(map(res => res.data.$values));
  }


  getLeaveTypeById(id: string): Observable<ApiResponse<LeaveType>> {
    return this.http.get<ApiResponse<LeaveType>>(`${this.apiUrl}/${id}`);
  }

  addLeaveType(dto: LeaveType): Observable<ApiResponse<LeaveType>> {
    return this.http.post<ApiResponse<LeaveType>>(this.apiUrl, dto);
  }

  updateLeaveType(id: string, dto: LeaveType): Observable<ApiResponse<LeaveType>> {
    return this.http.put<ApiResponse<LeaveType>>(`${this.apiUrl}/${id}`, dto);
  }

  deleteLeaveType(id: string): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.apiUrl}/${id}`);
  }
}
