import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/api-response.model';
import { UserLeaveBalanceResponseDto } from '../models/user-leave-balance.model';

@Injectable({ providedIn: 'root' })
export class LeaveBalanceService {
  private baseUrl = 'http://localhost:5000/api/v1';

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('accessToken') || '';
    return new HttpHeaders({ 'Authorization': `Bearer ${token}` });
  }

  // Get user's full leave balances
  getLeaveBalance(userId: string): Observable<ApiResponse<UserLeaveBalanceResponseDto>> {
    return this.http.get<ApiResponse<UserLeaveBalanceResponseDto>>(
      `${this.baseUrl}/leave-balance/user/${userId}`,
      { headers: this.getAuthHeaders() }
    );
  }

  // Get specific leave type balance
  getLeaveBalanceForType(userId: string, leaveTypeId: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/leave-balance/user/${userId}/leaveType/${leaveTypeId}`, {
      headers: this.getAuthHeaders()
    });
  }

  // Initialize leave balance for a user
  initializeLeaveBalance(userId: string): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(
      `${this.baseUrl}/leave-balance/initialize/user/${userId}`,
      {},
      { headers: this.getAuthHeaders() }
    );
  }

  // Initialize balance for new leave type (optional)
  initializeLeaveType(userId: string, leaveTypeId: string, standardLeaveCount: number): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(
      `${this.baseUrl}/leave-balance/initialize/user/${userId}/leaveType/${leaveTypeId}?standardLeaveCount=${standardLeaveCount}`,
      {},
      { headers: this.getAuthHeaders() }
    );
  }

  // Deduct leave
  deductLeave(userId: string, leaveTypeId: string, days: number): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(
      `${this.baseUrl}/leave-balance/deduct?userId=${userId}&leaveTypeId=${leaveTypeId}&days=${days}`,
      {},
      { headers: this.getAuthHeaders() }
    );
  }

  // Reset leave balances
  resetLeaveBalance(userId: string): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(
      `${this.baseUrl}/leave-balance/reset/user/${userId}`,
      {},
      { headers: this.getAuthHeaders() }
    );
  }
}
