import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-customer-details',
  imports: [FormsModule],
  templateUrl: './customer-details.html',
  styleUrl: './customer-details.css'
})
export class CustomerDetails {
  likeCount = 0;
  dislikeCount = 0;
  customer = {
    name: 'Loki',
    email: 'loki@example.com'
  };

  like() {
    this.likeCount++;
  }

  dislike() {
    this.dislikeCount++;
  }
}

