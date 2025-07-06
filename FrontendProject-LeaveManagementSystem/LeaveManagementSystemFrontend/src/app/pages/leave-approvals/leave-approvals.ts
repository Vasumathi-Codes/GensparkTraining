import { Component, OnInit } from '@angular/core';
import { LeaveService } from '../../services/leave.service';
import { LeaveRequest } from '../../models/leave-request.model';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-leave-approvals',
  templateUrl: './leave-approvals.html',
  styleUrls: ['./leave-approvals.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule]
})
export class LeaveApprovals implements OnInit {
  leaveRequests: LeaveRequest[] = [];
  filteredRequests: LeaveRequest[] = [];
  errorMsg = '';
  searchTerm = '';
  page = 1;
  pageSize = 10;
  totalPages = 1;
  sortBy = 'createdAt';
  sortOrder = 'desc';
  isTableView = false;
  statusFilter: string = 'All';
  statusOptions = ['All', 'Pending', 'Approved', 'Rejected', 'Auto-Rejected', 'Cancelled'];

  sortOptions = [
    { value: 'createdAt', label: 'Created At' },
    { value: 'updatedAt', label: 'Updated At' },
    { value: 'startDate', label: 'Start Date' },
    { value: 'endDate', label: 'End Date' },
    { value: 'status', label: 'Status' }
  ];

  constructor(private leaveService: LeaveService) {}

  ngOnInit(): void {
    this.fetchData();
  }

  fetchData() {
    this.leaveService.getAllLeaveRequests(
      1,       // page
      10000,   // pageSize large enough to include all
      '', '', this.sortBy, this.sortOrder
    ).subscribe({
      next: (res) => {
        this.leaveRequests = res?.data?.data?.$values || [];
        this.filterRequests();
      },
      error: (err) => {
        this.errorMsg = 'Error loading leave approvals';
        console.error(err);
      }
    });
  }

  filterRequests() {
    const term = this.searchTerm.toLowerCase();

    const filtered = this.leaveRequests
      .filter(leave =>
        (leave.userName?.toLowerCase().includes(term) ||
         leave.reason?.toLowerCase().includes(term) ||
         leave.leaveTypeName?.toLowerCase().includes(term)) &&
        (this.statusFilter === 'All' || leave.status === this.statusFilter)
      )
      .sort((a: any, b: any) => {
        const aValue = a[this.sortBy];
        const bValue = b[this.sortBy];
        return this.sortOrder === 'asc'
          ? (aValue > bValue ? 1 : -1)
          : (aValue < bValue ? 1 : -1);
      });

    //  Manual pagination
    this.totalPages = Math.ceil(filtered.length / this.pageSize);
    const start = (this.page - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.filteredRequests = filtered.slice(start, end);
  }

  onSearchChange() {
    this.page = 1;
    this.filterRequests();
  }

  onSortChange() {
    this.page = 1;
    this.filterRequests();
  }

  onStatusChange() {
    this.page = 1;
    this.filterRequests();
  }

  previousPage() {
    if (this.page > 1) {
      this.page--;
      this.filterRequests();
    }
  }

  nextPage() {
    if (this.page < this.totalPages) {
      this.page++;
      this.filterRequests();
    }
  }
}
