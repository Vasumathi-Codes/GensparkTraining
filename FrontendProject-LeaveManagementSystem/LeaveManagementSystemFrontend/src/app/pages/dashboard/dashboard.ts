import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeaveService } from '../../services/leave.service';
import { UserService } from '../../services/user.service';
import { LeaveTypeService } from '../../services/leave-type.service';
import { RouterLink } from '@angular/router';
import { AuthStateService } from '../../services/auth-state.service';
import { WeeklyLeaveChart } from '../../components/weekly-leave-chart/weekly-leave-chart';
import { ToastrService } from 'ngx-toastr';
import { LeaveCalendarComponent } from '../../components/leave-calendar/leave-calendar';

@Component({
  selector: 'app-hr-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, WeeklyLeaveChart, LeaveCalendarComponent],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css'],
})
export class Dashboard implements OnInit {
  private toastr = inject(ToastrService);
  stats = {
    totalLeaves: 0,
    approved: 0,
    pending: 0,
    rejected: 0,
    autoRejected:0,
    totalUsers: 0,
    leaveTypes: 0,
  };

  recentLeaves: any[] = [];
  loading = false;
  role: string | null = null;

  constructor(
    private leaveService: LeaveService,
    private userService: UserService,
    private leaveTypeService: LeaveTypeService,
    private authState: AuthStateService
  ) {}

  ngOnInit(): void {
    console.log('Toast trigger running...');

    this.authState.role$.subscribe((r) => (this.role = r));
    console.log(this.role);
    
    this.fetchData();
  }

  fetchData(): void {
  this.loading = true;

  this.leaveService
    .getAllLeaveRequests(1, 1000, '', '', '', '')
    .subscribe((res) => {
      const allLeaves = res?.data?.data?.$values || [];
      const today = new Date();

      // Filter future leaves only
      const upcomingLeaves = allLeaves
        .filter((x: any) => new Date(x.startDate) >= today)
        .sort((a: any, b: any) => new Date(a.startDate).getTime() - new Date(b.startDate).getTime()); // sort by start date

      this.stats.totalLeaves = upcomingLeaves.length;
      this.stats.approved = upcomingLeaves.filter((x: any) => x.status === 'Approved').length;
      this.stats.pending = upcomingLeaves.filter((x: any) => x.status === 'Pending').length;
      this.stats.rejected = upcomingLeaves.filter((x: any) => x.status === 'Rejected').length;
      this.stats.autoRejected = upcomingLeaves.filter((x: any) => x.status === 'Auto-Rejected').length;

      this.recentLeaves = upcomingLeaves.slice(0, 5); // show only top 5
      this.loading = false;
    });

  this.userService.getUsers(1, 1000).subscribe((res) => {
    const users = res?.data?.$values || [];
    this.stats.totalUsers = users.length;
  });

  this.leaveTypeService.getLeaveTypes().subscribe((types) => {
    this.stats.leaveTypes = types.length;
  });
}

}
