import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, AbstractControl, ValidationErrors, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, MatSnackBarModule],
  templateUrl: './user-form.html',
  styleUrls: ['./user-form.css']
})
export class UserFormComponent {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private snackBar = inject(MatSnackBar);

  bannedWords = ['admin', 'root'];

  userForm = this.fb.group({
    username: ['', [Validators.required, this.bannedWordsValidator.bind(this)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6), this.passwordStrengthValidator]],
    confirmPassword: ['', Validators.required],
    role: ['User', Validators.required]
  }, { validators: this.passwordsMatchValidator });

  bannedWordsValidator(control: AbstractControl): ValidationErrors | null {
    return this.bannedWords.some(word => control.value?.toLowerCase().includes(word))
      ? { bannedWord: true } : null;
  }

  passwordStrengthValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value || '';
    return /[0-9]/.test(value) && /[!@#$%^&*]/.test(value)
      ? null : { weakPassword: true };
  }

  passwordsMatchValidator(group: FormGroup): ValidationErrors | null {
    const pass = group.get('password')?.value;
    const confirm = group.get('confirmPassword')?.value;
    return pass === confirm ? null : { passwordMismatch: true };
  }

  onSubmit() {
    if (this.userForm.valid) {
      const { confirmPassword, ...user } = this.userForm.value;
      this.userService.addUser(user as User);

      this.snackBar.open('User Added Successfully!', 'Close', {
        duration: 3000,
        verticalPosition: 'top'
      });

      this.userForm.reset({ role: 'User' });
    }
  }

  get username() { return this.userForm.get('username'); }
  get email() { return this.userForm.get('email'); }
  get password() { return this.userForm.get('password'); }
  get confirmPassword() { return this.userForm.get('confirmPassword'); }
  get role() { return this.userForm.get('role'); }
}
