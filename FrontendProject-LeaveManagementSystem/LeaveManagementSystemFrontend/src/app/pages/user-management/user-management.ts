import { CommonModule } from '@angular/common';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, NavigationEnd } from '@angular/router';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';

import { UserService } from '../../services/user.service';
import { UserDto } from '../../models/user-dto.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-management.html',
  styleUrls: ['./user-management.css']
})
export class UserManagement implements OnInit, OnDestroy {
  users: UserDto[] = [];
  page = 1;
  pageSize = 10;
  totalPages = 1;
  searchTerm = '';
  roleFilter = '';
  sortBy = 'CreatedAt';
  sortOrder = 'desc';

  private routerSub!: Subscription;

  constructor(private userService: UserService, private router: Router, private toastr: ToastrService) {}

  ngOnInit() {
    this.loadUsers();

    this.routerSub = this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.loadUsers();
      });
  }

  ngOnDestroy() {
    if (this.routerSub) this.routerSub.unsubscribe();
  }

  loadUsers() {
    this.userService.getUsers(
      this.page,
      this.pageSize,
      this.searchTerm,
      this.roleFilter,
      this.sortBy,
      this.sortOrder
    ).subscribe({
      next: res => {
        const users = res.data?.$values || [];
        const totalPages = res.pagination?.totalPages || 1;

        if (this.page > totalPages && totalPages > 0) {
          this.page = totalPages;
          this.loadUsers();
          return;
        }

        this.users = users;
        this.totalPages = totalPages;
      },
      error: err => console.error('Error loading users:', err)
    });
  }

  applyFilters() {
    this.page = 1;
    this.loadUsers();
  }

  changePage(num: number) {
    const newPage = this.page + num;
    if (newPage >= 1 && newPage <= this.totalPages) {
      this.page = newPage;
      this.loadUsers();
    }
  }

    deleteUser(id: string) {
    if (confirm('Are you sure you want to delete this user?')) {
      this.userService.deleteUser(id).subscribe({
        next: () => {
          this.toastr.success('User deleted successfully', 'Deleted');
          this.loadUsers();
        },
        error: err => {
          console.error('Delete Failed:', err);
          const msg = err?.error?.message || 'Failed to delete user';
          this.toastr.error(msg, 'Error');
        }
      });
    }
  }

  navigateToAddUser() {
    this.router.navigate(['/users/add']);
  }

  navigateToEditUser(id: string) {
    this.router.navigate(['/users/edit', id]);
  }
}
