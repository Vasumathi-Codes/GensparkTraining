import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LeaveTypeService } from '../../services/leave-type.service';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

export interface LeaveType {
  id: string;
  name: string;
  standardLeaveCount: number;
  description?: string;
}

@Component({
  selector: 'app-leave-types',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './leave-types.html',
  styleUrls: ['./leave-types.css']
})
export class LeaveTypes implements OnInit {
  leaveTypes: LeaveType[] = [];
  filteredLeaveTypes: LeaveType[] = [];
  isHR = false;
  showForm = false;
  isEditing = false;
  isTableView = false;
  searchTerm = '';
  private searchSubject = new Subject<string>();

  selectedLeaveType: LeaveType = {
    id: '',
    name: '',
    standardLeaveCount: 0,
    description: ''
  };

  constructor(
    private leaveTypeService: LeaveTypeService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.checkUserRole();
    this.loadLeaveTypes();

    this.searchSubject.pipe(debounceTime(300)).subscribe(term => {
      this.applyFilter(term);
    });
  }

  checkUserRole() {
    const role = localStorage.getItem('role');
    this.isHR = role === 'HR';
  }

  loadLeaveTypes() {
    this.leaveTypeService.getLeaveTypes().subscribe({
      next: data => {
        this.leaveTypes = data;
        this.filteredLeaveTypes = data;
      },
      error: err => {
        this.toastr.error('Error fetching leave types', 'Error');
        console.error('Error fetching leave types:', err);
      }
    });
  }

  addNew() {
    this.selectedLeaveType = {
      id: '',
      name: '',
      standardLeaveCount: 0,
      description: ''
    };
    this.isEditing = false;
    this.showForm = true;
  }

  editLeaveType(lt: LeaveType) {
    this.selectedLeaveType = { ...lt };
    this.isEditing = true;
    this.showForm = true;
  }

  deleteLeaveType(id: string) {
    if (confirm('Are you sure you want to delete this leave type?')) {
      this.leaveTypeService.deleteLeaveType(id).subscribe({
        next: () => {
          this.toastr.success('Leave type deleted successfully!', 'Deleted');
          this.loadLeaveTypes();
        },
        error: () => {
          this.toastr.error('Failed to delete leave type', 'Error');
        }
      });
    }
  }

  saveLeaveType() {
    const { name, standardLeaveCount } = this.selectedLeaveType;

    if (!name || standardLeaveCount <= 0) {
      this.toastr.warning('Name and Max Days/Year must be valid.', 'Validation Error');
      return;
    }

    if (this.isEditing) {
      this.leaveTypeService.updateLeaveType(this.selectedLeaveType.id, this.selectedLeaveType)
        .subscribe({
          next: () => {
            this.toastr.success('Leave type updated successfully!', 'Updated');
            this.loadLeaveTypes();
            this.showForm = false;
          },
          error: () => {
            this.toastr.error('Failed to update leave type', 'Error');
          }
        });
    } else {
      this.leaveTypeService.addLeaveType(this.selectedLeaveType)
        .subscribe({
          next: () => {
            this.loadLeaveTypes();
            this.toastr.success('Leave type added successfully!', 'Added');
            this.showForm = false;
          },
          error: () => {
            this.toastr.error('Failed to add leave type', 'Error');
          }
        });
    }
  }

  cancelForm() {
    this.showForm = false;
  }

  onSearchInputChange() {
    this.searchSubject.next(this.searchTerm);
  }

  applyFilter(term: string) {
    const lowerTerm = term.toLowerCase();
    this.filteredLeaveTypes = this.leaveTypes.filter(lt =>
      lt.name.toLowerCase().includes(lowerTerm) ||
      (lt.description && lt.description.toLowerCase().includes(lowerTerm))
    );
  }

  highlight(text: string): string {
    if (!this.searchTerm) return text;
    const re = new RegExp(`(${this.searchTerm})`, 'gi');
    return text.replace(re, `<mark class="bg-yellow-200">$1</mark>`);
  }
}
