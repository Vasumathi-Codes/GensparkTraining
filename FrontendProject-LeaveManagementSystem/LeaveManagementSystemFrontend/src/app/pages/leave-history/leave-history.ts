  import { Component, OnInit } from '@angular/core';
  import { LeaveService } from '../../services/leave.service';
  import { LeaveRequest } from '../../models/leave-request.model';
  import { RouterLink } from '@angular/router';
  import { CommonModule } from '@angular/common';
  import { FormsModule } from '@angular/forms';

  @Component({
    selector: 'app-leave-history',
    standalone: true,
    templateUrl: './leave-history.html',
    imports: [RouterLink, CommonModule, FormsModule]
  })
  export class LeaveHistory implements OnInit {
    allLeaves: LeaveRequest[] = [];
    leaveRequests: LeaveRequest[] = [];
    errorMsg: string = '';
    searchText: string = '';
    sortBy: string = 'startDate';
    sortDirection: string = 'asc';
    page: number = 1;
    pageSize: number = 10;
    totalItems: number = 0;

    sortOptions = [
      { value: 'createdAt', label: 'Created At' },
      { value: 'updatedAt', label: 'Updated At' },
      { value: 'startDate', label: 'Start Date' },
      { value: 'endDate', label: 'End Date' },
      { value: 'status', label: 'Status' }
    ];

    directionOptions = [
      { value: 'asc', label: 'Ascending' },
      { value: 'desc', label: 'Descending' }
    ];

    constructor(private leaveService: LeaveService) {}

    ngOnInit(): void {
      this.loadLeaveHistory();
    }

    get totalPages(): number {
      return Math.ceil(this.totalItems / this.pageSize);
    }

    loadLeaveHistory() {
      this.leaveService.getLeaveHistory(
        '', // no search for server
        this.sortBy,
        this.sortDirection,
        1,
        9999 // large number to fetch all
      ).subscribe({
        next: (res: any) => {
          this.allLeaves = res.data?.data?.$values || res.data || [];
          this.applyFiltersAndPagination();
          this.errorMsg = '';
        },
        error: (err: any) => {
          this.errorMsg = 'Failed to load leave history.';
          console.error(err);
        }
      });
    }

    applyFiltersAndPagination() {
      let filtered = this.allLeaves;
      console.log(filtered);
      
      let id = localStorage.getItem('userId');

      let filteredbyid = filtered.filter(leave => leave.userId === id);
      filtered = filteredbyid;
      console.log(filtered);
      if (this.searchText.trim()) {
        const search = this.searchText.toLowerCase();
        filtered = filtered.filter(l =>
          l.reason?.toLowerCase().includes(search) ||
          l.leaveTypeName?.toLowerCase().includes(search)
        );
      }


      filtered.sort((a: any, b: any) => {
        const aValue = a[this.sortBy];
        const bValue = b[this.sortBy];

        if (aValue < bValue) return this.sortDirection === 'asc' ? -1 : 1;
        if (aValue > bValue) return this.sortDirection === 'asc' ? 1 : -1;
        return 0;
      });

      this.totalItems = filtered.length;
      const start = (this.page - 1) * this.pageSize;
      const end = start + this.pageSize;
      this.leaveRequests = filtered.slice(start, end);
    }

    onSortChange() {
      this.page = 1;
      this.applyFiltersAndPagination();
    }

    previousPage() {
      if (this.page > 1) {
        this.page--;
        this.applyFiltersAndPagination();
      }
    }

    nextPage() {
      if (this.page < this.totalPages) {
        this.page++;
        this.applyFiltersAndPagination();
      }
    }

  }
