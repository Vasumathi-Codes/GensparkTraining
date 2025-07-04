import { Injectable } from '@angular/core';
import { loadStripe, Stripe } from '@stripe/stripe-js';

@Injectable({ providedIn: 'root' })
export class StripeService {
  private stripePromise = loadStripe('pk_test_51Rh1lYRSkAqJCX2UtPREoogGGWdbKZEmyCNU0cVKo1nrIufFQpXODoUShBBqy9zt8K2e7IwYEG6BqspABzILETDs00vju6MGtX'); // ✅ Your publishable key

  getStripe(): Promise<Stripe | null> {
    return this.stripePromise;
  }
}
