// src/app/services/notification.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Notification {
  id: string;
  message: string;
  createdAt: string;
  isRead: boolean;
  recipientId: string;
  readAt?: string;
}

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private baseUrl = 'http://localhost:5000/api/notifications';

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('accessToken');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  getAll(userId: string): Observable<{ $values: Notification[] }> {
    return this.http.get<{ $values: Notification[] }>(
        `${this.baseUrl}/all/${userId}`,
        { headers: this.getAuthHeaders() }
    );
  }

  getUnread(userId: string): Observable<{ $values: Notification[] }> {
    return this.http.get<{ $values: Notification[] }>(
        `${this.baseUrl}/unread/${userId}`,
        { headers: this.getAuthHeaders() }
    );
   }


  markAsRead(id: string): Observable<void> {
    return this.http.post<void>(
      `${this.baseUrl}/mark-as-read/${id}`,
      {}, // No body required
      { headers: this.getAuthHeaders() }
    );
  }

  markAllAsRead(userId: string): Observable<void> {
    return this.http.post<void>(
      `${this.baseUrl}/mark-all-as-read/${userId}`,
      {},
      { headers: this.getAuthHeaders() }
    );
  }

}
