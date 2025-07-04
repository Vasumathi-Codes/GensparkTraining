import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { PaymentForm } from './payment-form';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

describe('PaymentForm', () => {
  let component: PaymentForm;
  let fixture: ComponentFixture<PaymentForm>;
  let razorpayOptions: any;

  beforeEach(() => {
    (window as any).Razorpay = function (options: any) {
      razorpayOptions = options;
      return {
        open: jasmine.createSpy('open')
      };
    };

    TestBed.configureTestingModule({
      imports: [PaymentForm]
    });

    fixture = TestBed.createComponent(PaymentForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should not submit if form is invalid', () => {
    component.paymentForm.setValue({
      amount: null,
      name: '',
      email: '',
      contact: ''
    });
    component.pay();
    expect(component.paymentForm.invalid).toBeTrue();
    expect(component.isProcessing).toBeFalse();
  });

  it('should show error messages for invalid fields', () => {
    component.paymentForm.markAllAsTouched();
    expect(component.showError('amount')).toBe('Required');
    expect(component.showError('name')).toBe('Required');
    expect(component.showError('email')).toBe('Required');
    expect(component.showError('contact')).toBe('Required');
  });

  it('should open Razorpay with correct options if form is valid', () => {
    component.paymentForm.setValue({
      amount: 100,
      name: 'vasu',
      email: 'vasu@example.com',
      contact: '9876543210'
    });
    component.pay();
    expect(component.isProcessing).toBeTrue();
    expect(razorpayOptions.amount).toBe(10000);
    expect(razorpayOptions.prefill.name).toBe('vasu');
    expect(razorpayOptions.open).toBeFalsy(); 
  });

  it('should handle successful payment via handler', fakeAsync(() => {
    component.paymentForm.setValue({
      amount: 100,
      name: 'vasu',
      email: 'vasu@example.com',
      contact: '9876543210'
    });
    component.pay();
    tick();

    razorpayOptions.handler({ razorpay_payment_id: 'pay_test_123' });
    tick();
    fixture.detectChanges();

    expect(component.message).toContain('Payment Successful!');
    expect(component.isProcessing).toBeFalse();
  }));

  it('should handle payment cancellation via modal.ondismiss', fakeAsync(() => {
    component.paymentForm.setValue({
      amount: 100,
      name: 'vasu',
      email: 'vasu@example.com',
      contact: '9876543210'
    });
    component.pay();
    tick();

    razorpayOptions.modal.ondismiss();
    tick();
    fixture.detectChanges();

    expect(component.message).toContain('Payment Cancelled');
    expect(component.isProcessing).toBeFalse();
  }));

  it('should validate email format', () => {
    component.paymentForm.get('email')?.setValue('invalid');
    component.paymentForm.get('email')?.markAsTouched();
    expect(component.showError('email')).toBe('Invalid email');
  });

  it('should validate contact number pattern', () => {
    component.paymentForm.get('contact')?.setValue('123');
    component.paymentForm.get('contact')?.markAsTouched();
    expect(component.showError('contact')).toBe('Must be 10 digits');
  });

  it('should validate min amount', () => {
    component.paymentForm.get('amount')?.setValue(0);
    component.paymentForm.get('amount')?.markAsTouched();
    expect(component.showError('amount')).toBe('Amount must be > 0');
  });
});
