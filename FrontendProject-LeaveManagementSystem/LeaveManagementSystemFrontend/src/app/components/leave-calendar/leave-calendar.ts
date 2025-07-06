import { Component, OnInit, inject } from '@angular/core';
import { CalendarEvent, CalendarView } from 'angular-calendar';
import { Subject } from 'rxjs';
import { LeaveService } from '../../services/leave.service';
import { CommonModule } from '@angular/common';
import { CalendarStandaloneModule } from '../../calendar-standalone.module';
import { AuthStateService } from '../../services/auth-state.service';

@Component({
  selector: 'app-leave-calendar',
  standalone: true,
  imports: [CommonModule, CalendarStandaloneModule],
  templateUrl: './leave-calendar.html',
  styleUrls: ['./leave-calendar.css']
})
export class LeaveCalendarComponent implements OnInit {
  leaveService = inject(LeaveService);
  authStateService = inject(AuthStateService);

  viewDate = new Date();
  view: CalendarView = CalendarView.Month;
  events: CalendarEvent[] = [];
  refresh: Subject<void> = new Subject<void>();

  leaveTypeMap: Record<string, string> = {};
  isHR = false;

  ngOnInit(): void {
    this.isHR = this.authStateService.getRole() === 'HR';
    this.fetchLeaveTypesAndEvents();
  }

  fetchLeaveTypesAndEvents(): void {
    this.leaveService.getLeaveTypes().subscribe({
      next: (res: any) => {
        const leaveTypes = res?.data?.$values || [];
        for (let type of leaveTypes) {
          this.leaveTypeMap[type.id] = type.name;
        }
        this.loadLeaveEvents();
      },
      error: () => {
        console.error('Failed to fetch leave types');
      }
    });
  }

  loadLeaveEvents(): void {
    this.leaveService.getAllLeaveRequests(1, 100, '', '', 'startDate', 'asc')
      .subscribe({
        next: (res: any) => {
          const requests = res?.data?.$values || res?.data?.data?.$values || [];

          this.events = requests.map((r: any) => ({
            title: this.isHR
              ? `${r.userName} (${this.leaveTypeMap[r.leaveTypeId] || 'Leave'})`
              : `${this.leaveTypeMap[r.leaveTypeId] || 'Leave'}`,
            start: new Date(r.startDate),
            end: new Date(r.endDate),
            color: {
              primary: this.getColor(r.status),
              secondary: '#e3f2fd'
            },
            allDay: true
          }));

          this.refresh.next(); // Notify calendar to re-render
        },
        error: () => {
          console.error('Failed to fetch leave requests');
        }
      });
  }

  getColor(status: string): string {
    switch (status) {
      case 'Approved':
        return '#22c55e'; // green
      case 'Pending':
        return '#eab308'; // yellow
      case 'Rejected':
        return '#ef4444'; // red
      case 'Auto-Rejected':
        return '#6b7280'; // gray
      case 'Cancelled':
        return '#9ca3af'; // light gray
      default:
        return '#a1a1aa'; // fallback gray
    }
  }

}
