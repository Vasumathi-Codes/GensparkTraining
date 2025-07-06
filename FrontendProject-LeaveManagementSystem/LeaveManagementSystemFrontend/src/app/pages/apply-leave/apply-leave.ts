import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  Validators,
  FormGroup,
  ReactiveFormsModule
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { LeaveService } from '../../services/leave.service';
import { LeaveType } from '../../models/leave-type.model';

@Component({
  selector: 'app-apply-leave',
  standalone: true,
  templateUrl: './apply-leave.html',
  styleUrls: ['./apply-leave.css'],
  imports: [CommonModule, ReactiveFormsModule]
})
export class ApplyLeave implements OnInit {
  applyLeaveForm!: FormGroup;
  leaveTypes: LeaveType[] = [];
  filesToUpload: File[] = [];

  private toastr = inject(ToastrService);

  constructor(
    private fb: FormBuilder,
    private leaveService: LeaveService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.applyLeaveForm = this.fb.group({
      leaveTypeId: ['', Validators.required],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      reason: ['', Validators.required],
      files: [null]
    });

    // Dynamic validation: End Date cannot be before Start Date
    this.applyLeaveForm.get('startDate')?.valueChanges.subscribe(start => {
      const end = this.applyLeaveForm.get('endDate')?.value;
      if (end && end < start) {
        this.applyLeaveForm.get('endDate')?.setErrors({ dateMismatch: true });
      } else {
        this.applyLeaveForm.get('endDate')?.updateValueAndValidity();
      }
    });

    this.loadLeaveTypes();
  }

  loadLeaveTypes() {
    this.leaveService.getLeaveTypes().subscribe({
      next: (res) => {
        this.leaveTypes = res.data.$values;
      },
      error: () => {
        this.toastr.error('Error loading leave types');
      }
    });
  }

  handleFileInput(event: any) {
    this.filesToUpload = Array.from(event.target.files || []);
  }

  onSubmit() {
    this.applyLeaveForm.markAllAsTouched();
    if (this.applyLeaveForm.invalid) {
      this.toastr.warning('Please fill all required fields correctly');
      return;
    }

    const formData = this.applyLeaveForm.value;
    const leaveRequest = {
      leaveTypeId: formData.leaveTypeId,
      startDate: formData.startDate,
      endDate: formData.endDate,
      reason: formData.reason
    };

    this.leaveService.applyLeave(leaveRequest).subscribe({
      next: (res) => {
        const leaveRequestId = res.data.id;

        if (this.filesToUpload.length === 0) {
          this.toastr.success('Leave applied successfully');
          this.router.navigate(['/leave-history']);
          return;
        }

        let uploadedCount = 0;

        this.filesToUpload.forEach(file => {
          const attachmentForm = new FormData();
          attachmentForm.append('leaveRequestId', leaveRequestId);
          attachmentForm.append('file', file, file.name);

          this.leaveService.uploadAttachment(attachmentForm).subscribe({
            next: () => {
              uploadedCount++;
              if (uploadedCount === this.filesToUpload.length) {
                this.toastr.success('All attachments uploaded successfully');
                this.router.navigate(['/leave-history']);
              }
            },
            error: () => {
              this.toastr.error(`Attachment failed for file: ${file.name}`);
            }
          });
        });
      },
      error: (err) => {
        const msg = err?.error?.message || 'Failed to apply leave';
        this.toastr.error(msg);
      }
    });
  }
}
