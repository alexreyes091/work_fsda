export interface IDashboardStats {
  totalHotels: number;
  totalRooms: number;
  activeReservations: number;
  occupancyRate: number;
  checkInsToday: number;
  checkOutsToday: number;
  pendingReservations: number;
  monthlyRevenue: number;
}

export interface IHotelOccupancy {
  hotelName: string;
  location: string;
  occupancyRate: number;
  occupiedRooms: number;
  totalRooms: number;
}
