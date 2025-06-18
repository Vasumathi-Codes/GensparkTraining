import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { User } from '../../models/user.model';
import { UserService } from '../../services/user.service';
import { CanvasJSAngularChartsModule } from '@canvasjs/angular-charts';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, FormsModule, CanvasJSAngularChartsModule],
  templateUrl: './user-list.html',
  styleUrls: ['./user-list.css']
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  filteredUsers: User[] = [];
  genderFilter: string = '';
  roleFilter: string = '';
  stateFilter: string = '';

  uniqueStates: string[] = [];
  uniqueRoles: string[] = [];

  totalUsers: number = 0;
  maleCount: number = 0;
  femaleCount: number = 0;
  avgAge: number = 0;
  errorMessage: string = '';

  genderChartOptions: any;
  roleChartOptions: any;
  stateChartOptions: any;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.userService.getAllUsers().subscribe({
      next: (res) => {
        if (res && res.users && res.users.length > 0) {
          this.users = res.users as User[];
          this.filteredUsers = [...this.users];
          this.uniqueStates = [...new Set(this.users.map(u => u.address.state))];
          this.uniqueRoles = [...new Set(this.users.map(u => u.role))];
          this.updateDashboard(this.filteredUsers);
        } else {
          this.errorMessage = "No users found";
        }
      },
      error: (err) => {
        console.log(err);
        this.errorMessage = "Failed to load users. Please try again";
      }
    });
  }

  applyFilter() {
    this.filteredUsers = this.users.filter(user => {
      const genderMatch = this.genderFilter ? user.gender.toLowerCase() === this.genderFilter.toLowerCase() : true;
      const stateMatch = this.stateFilter ? user.address.state.toLowerCase() === this.stateFilter.toLowerCase() : true;
      const roleMatch = this.roleFilter ? user.role.toLowerCase() === this.roleFilter.toLowerCase() : true;
      return genderMatch && stateMatch && roleMatch;
    });
    this.updateDashboard(this.filteredUsers);
  }

  private updateDashboard(usersArray: User[]) {
    this.totalUsers = usersArray.length;
    this.maleCount = usersArray.filter(user => user.gender.toLowerCase() === 'male').length;
    this.femaleCount = usersArray.filter(user => user.gender.toLowerCase() === 'female').length;

    const ages = usersArray.map(user => user.age);
    this.avgAge = ages.length ? this.totalUsers / ages.length : 0;

    this.genderChartOptions = {
      animationEnabled: true,
      title: { text: "Gender Distribution" },
      data: [{
        type: "pie",
        indexLabel: "{label}: {y}",
        dataPoints: [
          { label: "Male", y: this.maleCount },
          { label: "Female", y: this.femaleCount }
        ]
      }]
    };

    const roleCounts = this.uniqueRoles.map(role => ({
      label: role,
      y: usersArray.filter(user => user.role === role).length
    }));

    this.roleChartOptions = {
      animationEnabled: true,
      title: { text: "Roles Distribution" },
      data: [{
        type: "column",
        dataPoints: roleCounts
      }]
    };

    const stateCounts = this.uniqueStates.map(state => ({
      label: state,
      y: usersArray.filter(user => user.address.state === state).length
    }));

    this.stateChartOptions = {
      animationEnabled: true,
      title: { text: "States Distribution" },
      data: [{
        type: "column",
        dataPoints: stateCounts
      }]
    };
  }
}
