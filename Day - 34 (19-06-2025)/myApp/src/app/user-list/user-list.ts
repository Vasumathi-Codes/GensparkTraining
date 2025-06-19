import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { selectAllUsers, selectUserError, selectUserLoading } from '../ngrx/user.selector';
import { loadUsers, loadUsersSuccess, loadUsersFailure } from '../ngrx/users.actions';
import { Observable } from 'rxjs';
import { User } from '../models/User';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { AddUser } from "../add-user/add-user";
import { UserService } from '../services/UserService';

@Component({
  selector: 'app-user-list',
  imports: [NgIf, NgFor, AsyncPipe, AddUser],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css'
})
export class UserList implements OnInit {

  users$:Observable<User[]> ;
  loading$:Observable<boolean>;
  error$:Observable<string | null>;

  constructor(private store:Store, private userService:UserService){
    this.users$ = this.store.select(selectAllUsers);
    this.loading$ = this.store.select(selectUserLoading);
    this.error$ = this.store.select(selectUserError);

  }
  ngOnInit(): void {
    this.store.dispatch(loadUsers()); // loading true
    this.userService.getUsers().subscribe({
      next: (users: User[]) => {
        this.store.dispatch(loadUsersSuccess({ users }));
      },
      error: (err: string) => {
        this.store.dispatch(loadUsersFailure({ error: err }));
      }
    });
  }

}
