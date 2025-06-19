import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router'; // ✅ must import these for routerLink to work

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive], // ✅ must add these
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class App {
  protected title = 'user-management-app';
}
