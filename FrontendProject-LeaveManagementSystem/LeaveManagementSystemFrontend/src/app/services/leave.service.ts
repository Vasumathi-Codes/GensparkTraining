import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LeaveAttachmentResponse } from '../models/leave-attachment.model';
import { ApiResponse } from '../models/api-response.model';
import { UserLeaveBalanceResponseDto } from '../models/user-leave-balance.model';

@Injectable({ providedIn: 'root' })
export class LeaveService {
  private baseUrl = 'http://localhost:5000/api/v1';

  constructor(private http: HttpClient) {}

  getLeaveTypes(): Observable<any> {
    return this.http.get(`${this.baseUrl}/leavetypes`);
  }

  applyLeave(leaveRequest: any): Observable<any> {
    const token = localStorage.getItem('accessToken'); 
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.http.post(`${this.baseUrl}/leave-requests`, leaveRequest, { headers });
  }

  uploadAttachment(formData: FormData): Observable<any> {
    const token = localStorage.getItem('accessToken'); 
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.post(`${this.baseUrl}/leave-attachments`, formData, { headers });
  }

 

  getLeaveHistory(search: string, sortBy: string, sortDirection: string, page: number, pageSize: number): Observable<any> {
    const token = localStorage.getItem('accessToken');
    const headers = new HttpHeaders({ 'Authorization': `Bearer ${token}` });

    const params = {
      search,
      sortBy,
      sortDirection,
      page: page.toString(),
      pageSize: pageSize.toString()
    };

    return this.http.get(`${this.baseUrl}/leave-requests`, { headers, params });
  }



  getLeaveRequestById(id: string): Observable<any> {
    const token = localStorage.getItem('accessToken');
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
    });
    return this.http.get(`${this.baseUrl}/leave-requests/${id}`, { headers });
  }

  getLeaveBalance(userId: string): Observable<any> {
  const token = localStorage.getItem('token');
  const headers = new HttpHeaders({
    'Authorization': `Bearer ${token}`
  });

  return this.http.get(`${this.baseUrl}/leave-balance/user/${userId}`, { headers });
}


getAllLeaveRequests(
  page: number,
  pageSize: number,
  searchTerm: string,
  status: string,
  sortBy: string,
  sortOrder: string
): Observable<any> {
  const token = localStorage.getItem('accessToken');
  const headers = new HttpHeaders({ 'Authorization': `Bearer ${token}` });

  const params = {
    page: page.toString(),
    pageSize: pageSize.toString(),
    searchTerm,
    status,
    sortBy,
    sortOrder
  };

  return this.http.get(`${this.baseUrl}/leave-requests`, { headers, params });
}



updateLeaveStatus(leaveRequestId: string, status: string): Observable<any> {
  const token = localStorage.getItem('accessToken'); // Use the same token key as others
  const headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  });

  const payload = { status }; // The API expects { status: 'Approved' } or { status: 'Rejected' }

  return this.http.put(`${this.baseUrl}/leave-requests/${leaveRequestId}/status`, payload, { headers });
}

getAttachmentsByLeaveId(leaveId: string): Observable<any> {
  const token = localStorage.getItem('accessToken');
  const headers = new HttpHeaders({ 'Authorization': `Bearer ${token}` });
  return this.http.get(`${this.baseUrl}/leave-attachments/${leaveId}`, { headers });
}

getLeaveAttachmentsByRequestId(leaveRequestId: string): Observable<ApiResponse<LeaveAttachmentResponse[]>> {
  const token = localStorage.getItem('accessToken');
  const headers = new HttpHeaders({ 'Authorization': `Bearer ${token}` });

  return this.http.get<ApiResponse<LeaveAttachmentResponse[]>>(
    `${this.baseUrl}/leave-attachments/by-leave-request/${leaveRequestId}`, { headers }
  );
}

getUserLeaveBalance(userId: string): Observable<ApiResponse<UserLeaveBalanceResponseDto>> {
  const token = localStorage.getItem('accessToken');
  const headers = new HttpHeaders({ 'Authorization': `Bearer ${token}` });

  return this.http.get<ApiResponse<UserLeaveBalanceResponseDto>>(
    `${this.baseUrl}/leave-balance/user/${userId}`, { headers }
  );
}


getLeaveBalanceForType(userId: string, leaveTypeId: string): Observable<any> {
    const token = localStorage.getItem('accessToken');
    const headers = new HttpHeaders({ 'Authorization': `Bearer ${token}` });
    return this.http.get(`${this.baseUrl}/leave-balance/user/${userId}/leaveType/${leaveTypeId}`, { headers });
  }

  deleteAttachment(attachmentId: string): Observable<any> {
  const token = localStorage.getItem('accessToken');
  const headers = new HttpHeaders({ 'Authorization': `Bearer ${token}` });

  return this.http.delete(`${this.baseUrl}/leave-attachments/${attachmentId}`, { headers });
}

cancelLeaveRequest(leaveRequestId: string): Observable<any> {
  const token = localStorage.getItem('accessToken');
  const headers = new HttpHeaders({
    'Authorization': `Bearer ${token}`
  });

  return this.http.put(`${this.baseUrl}/leave-requests/${leaveRequestId}/cancel`, {}, { headers });
}


}
