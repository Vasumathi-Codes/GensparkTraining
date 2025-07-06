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
  templateUrl: './leave-history-by-id.html',
  styleUrls: ['./leave-history-by-id.css'],
  standalone: true,
  imports: [CommonModule]
})
export class LeaveHistoryById implements OnInit {
  leaveId!: string;
  leaveRequest!: LeaveRequest;
  attachments: any[] = [];
  leaveBalanceForType!: LeaveBalanceForType;
  errorMsg = '';
  isLoading = false;
  selectedFile: File | null = null;

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
            console.log('Attachment API response:', a.data);
            console.log('Attachments Array:', this.attachments);
          },
          error: e => console.error('Attachments fetch failed', e)
        });


        // Fetch Leave Balance for this leave type only
        const userId = this.leaveRequest.userId;
        const leaveTypeId = this.leaveRequest.leaveTypeId;
        this.leaveBalanceService.getLeaveBalanceForType(userId, leaveTypeId).subscribe({
          next: b => 
            {
              this.leaveBalanceForType = b.data.leaveBalance;
              console.log(this.leaveBalanceForType);

            },
          error: e => console.error('Leave balance fetch failed', e)
        });

        this.isLoading = false;
      },
      error: err => {
        this.errorMsg = 'Failed to load leave details';
        console.error(err);
        this.isLoading = false;
      }
    });
  }

  updateStatus(newStatus: 'Approved' | 'Rejected') {
  if (!this.leaveId) return;

  this.leaveService.updateLeaveStatus(this.leaveId, newStatus).subscribe({
    next: () => {
      alert(`Leave request has been ${newStatus.toLowerCase()} successfully!`);

      this.leaveService.getLeaveRequestById(this.leaveId).subscribe({
        next: res => {
          this.leaveRequest = res.data; 
        },
        error: err => {
          console.error('Failed to reload leave request:', err);
        }
      });
    },
    error: err => {
      console.error('Failed to update leave status:', err);
      alert(`Error updating leave status. ${err?.error?.message || 'Unknown error'}`);
    }
  });
}


deleteAttachment(attachmentId: string) {
  if (!confirm('Are you sure you want to delete this attachment?')) return;

  this.leaveService.deleteAttachment(attachmentId).subscribe({
    next: () => {
      this.attachments = this.attachments.filter(att => att.id !== attachmentId);
      alert('Attachment deleted successfully.');
    },
    error: err => {
      console.error('Failed to delete attachment:', err);
      alert('Error deleting attachment.');
    }
  });
}


onFileSelected(event: Event) {
  const input = event.target as HTMLInputElement;
  if (input.files && input.files.length > 0) {
    this.selectedFile = input.files[0];
  }
}

uploadAttachment() {
  if (!this.selectedFile || !this.leaveId) return;

  const formData = new FormData();
  formData.append('leaveRequestId', this.leaveId);
  formData.append('file', this.selectedFile);

  this.leaveService.uploadAttachment(formData).subscribe({
    next: () => {
      alert('File uploaded successfully!');
      this.selectedFile = null;

      this.leaveService.getLeaveAttachmentsByRequestId(this.leaveId).subscribe({
        next: a => {
          this.attachments = (a.data as any)?.$values ?? [];
        },
        error: e => console.error('Attachments refresh failed after upload', e)
      });
    },
    error: err => {
      console.error('File upload failed:', err);
      alert('Failed to upload file.');
    }
  });
}


cancelLeaveRequest() {
  if (!this.leaveId) return;

  const confirmCancel = confirm('Are you sure you want to cancel this leave request?');
  if (!confirmCancel) return;

  this.leaveService.cancelLeaveRequest(this.leaveId).subscribe({
    next: () => {
      this.toastr.success('Leave request cancelled successfully.', 'Cancelled');
      this.leaveService.getLeaveRequestById(this.leaveId).subscribe({
        next: res => this.leaveRequest = res.data,
        error: err => console.error('Failed to refresh leave data:', err)
      });
    },
    error: err => {
      console.error('Cancellation failed:', err);
      this.toastr.error(err?.error?.message || 'Failed to cancel leave request.', 'Error');
    }
  });
}


}
