import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user-add',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-add.html',
  styleUrls: ['./user-add.css']
})
export class UserAddComponent {
  userForm: FormGroup;
  message: string = '';

  constructor(private fb: FormBuilder, private userService: UserService) {
    this.userForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required]],
      age: ['', [Validators.required, Validators.min(1), Validators.max(120)]],
      gender: ['', [Validators.required]]
    });
  }

  onSubmit() {
    if (this.userForm.valid) {
      this.userService.addUser(this.userForm.value).subscribe(res => {
        this.message = 'User added successfully!';
        this.userForm.reset();
      }, err => {
        this.message = 'Error adding user.';
      });
    } else {
      this.message = 'Please correct the errors in the form.';
    }
  }

  get f() { return this.userForm.controls; }
}
