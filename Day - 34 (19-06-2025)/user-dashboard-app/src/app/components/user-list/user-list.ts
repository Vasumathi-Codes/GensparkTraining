import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user.service';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, combineLatest, startWith, map } from 'rxjs';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-list.html',
  styleUrls: ['./user-list.css']
})
export class UserListComponent {

  searchControl = new FormControl('');
  roleControl = new FormControl('All');

  filteredUsers$;

  constructor(private userService: UserService) {
    this.filteredUsers$ = combineLatest([
      this.userService.users$,
      this.searchControl.valueChanges.pipe(startWith(''), debounceTime(3000)),
      this.roleControl.valueChanges.pipe(startWith('All'))
    ]).pipe(
      map(([users, search, role]) => {
        return users.filter(user => 
          (role === 'All' || user.role === role) &&
          (user.username.toLowerCase().includes(search?.toLowerCase() || '') ||
           user.role.toLowerCase().includes(search?.toLowerCase() || ''))
        );
      })
    );
  }
}
