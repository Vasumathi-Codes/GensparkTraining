export interface Address {
  state: string;
}

export interface User {
  firstName: string;
  gender: string;
  age: number;
  role: string;
  address: Address;
}