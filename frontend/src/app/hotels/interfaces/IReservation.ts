export interface IReservation {
  id?: string;
  roomName: string;
  roomHotelName: string;
  guestFullName: string;
  guestEmail: string;
  phoneNumber: string;
  checkIn: string; // ISO date string
  checkOut: string; // ISO date string
  nights: number;
  price: number;
  notes: string;
  statusReservation: 'Pending' | 'Confirmed' | 'Cancelled' | 'CheckedIn' | 'CheckedOut';
}
