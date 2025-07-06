import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LeaveService } from '../../services/leave.service';
import { LeaveBalanceService } from '../../services/leave-balance.service';
import { LeaveRequest } from '../../models/leave-request.model';
import { LeaveBalanceForType } from '../../models/leave-balance.model';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-leave-detail',
  templateUrl: './leave-details.html',
  styleUrls: ['./leave-details.css'],
  standalone: true,
  imports: [CommonModule]
})
export class LeaveDetails implements OnInit {
  leaveId!: string;
  leaveRequest!: LeaveRequest;
  attachments: any[] = [];
  leaveBalanceForType!: LeaveBalanceForType;
  errorMsg = '';
  isLoading = false;

  constructor(
    private route: ActivatedRoute,
    private leaveService: LeaveService,
    private leaveBalanceService: LeaveBalanceService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.leaveId = this.route.snapshot.paramMap.get('id')!;
    this.loadDetails();
  }

  loadDetails() {
    this.isLoading = true;
    this.leaveService.getLeaveRequestById(this.leaveId).subscribe({
      next: res => {
        this.leaveRequest = res.data;

        this.leaveService.getLeaveAttachmentsByRequestId(this.leaveId).subscribe({
          next: a => {
            this.attachments = (a.data as any)?.$values ?? [];
          },
          error: e => {
            this.toastr.error('Failed to fetch attachments', 'Error');
            console.error('Attachments fetch failed', e);
          }
        });

        const userId = this.leaveRequest.userId;
        const leaveTypeId = this.leaveRequest.leaveTypeId;
        this.leaveBalanceService.getLeaveBalanceForType(userId, leaveTypeId).subscribe({
          next: b => {
            this.leaveBalanceForType = b.data.leaveBalance;
          },
          error: e => {
            this.toastr.error('Failed to fetch leave balance', 'Error');
            console.error('Leave balance fetch failed', e);
          }
        });

        this.isLoading = false;
      },
      error: err => {
        this.errorMsg = 'Failed to load leave details';
        this.toastr.error(this.errorMsg, 'Error');
        console.error(err);
        this.isLoading = false;
      }
    });
  }

  updateStatus(newStatus: 'Approved' | 'Rejected') {
    if (!this.leaveId) return;

    this.leaveService.updateLeaveStatus(this.leaveId, newStatus).subscribe({
      next: () => {
        this.toastr.success(`Leave request ${newStatus.toLowerCase()} successfully!`, 'Success');

        // Reload updated data
        this.leaveService.getLeaveRequestById(this.leaveId).subscribe({
          next: res => {
            this.leaveRequest = res.data;
          },
          error: err => {
            this.toastr.warning('Status updated, but failed to refresh data.', 'Partial Success');
            console.error('Failed to reload leave request:', err);
          }
        });
      },
      error: err => {
        const msg = err?.error?.message || 'Unknown error';
        this.toastr.error(`Failed to update status. ${msg}`, 'Error');
        console.error('Failed to update leave status:', err);
      }
    });
  }
}
