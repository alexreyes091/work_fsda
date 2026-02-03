import { inject, Injectable } from '@angular/core';
// Services
import { HotelService } from './HotelService';
import { ReservationService } from './ReservationService';
import { RoomOccupancyService } from './RoomOccupancyService';
// Interfaces
import { IDashboardStats, IHotelOccupancy } from '../interfaces/IDashboardStats';
import { IReservation } from '../interfaces/IReservation';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private hotelService = inject(HotelService);
  private reservationService = inject(ReservationService);
  private occupancyService = inject(RoomOccupancyService);

  /**
   * Calcula las estadísticas globales del dashboard.
   * Obtiene los datos directamente desde el endpoint del backend.
   */
  async getDashboardStats(): Promise<IDashboardStats> {
    try {
      const response = await this.reservationService.getDashboardStats();
      return response.data || {
        totalHotels: 0,
        totalRooms: 0,
        activeReservations: 0,
        occupancyRate: 0,
        checkInsToday: 0,
        checkOutsToday: 0,
        pendingReservations: 0,
        monthlyRevenue: 0
      };
    } catch (error) {
      console.error('Error fetching dashboard stats:', error);
      return {
        totalHotels: 0,
        totalRooms: 0,
        activeReservations: 0,
        occupancyRate: 0,
        checkInsToday: 0,
        checkOutsToday: 0,
        pendingReservations: 0,
        monthlyRevenue: 0
      };
    }
  }

  /**
   * Obtiene la ocupación por hotel.
   */
  async getHotelOccupancy(): Promise<IHotelOccupancy[]> {
    const hotelsResponse = await this.hotelService.getAll(1, 1000);
    const hotels = hotelsResponse.data || [];

    const occupancyPromises = hotels.map(async (hotel) => {
      const today = new Date();
      // Obtener ocupación para el mes actual completo
      const startOfMonth = new Date(today.getFullYear(), today.getMonth(), 1);
      const endOfMonth = new Date(today.getFullYear(), today.getMonth() + 1, 0);

      try {
        const occupancyResponse = await this.occupancyService.getOccupancyGrid(
          hotel.id,
          startOfMonth,
          endOfMonth
        );

        // Calcular ocupación basada en datos del mes
        const monthData = occupancyResponse.data || [];
        const totalRoomsHotel = hotel.totalRooms || 0;
        
        if (monthData.length === 0 || totalRoomsHotel === 0) {
          return {
            hotelName: hotel.name,
            location: hotel.city || 'N/A',
            occupancyRate: 0,
            occupiedRooms: 0,
            totalRooms: totalRoomsHotel
          };
        }

        // Promedio de habitaciones ocupadas por día
        const avgOccupied = monthData.reduce((sum, day) => sum + (day.occupiedCount || 0), 0) / monthData.length;
        
        // Calcular porcentaje (promedio ocupadas / total habitaciones del hotel)
        const occupancyRate = Math.min(100, Math.round((avgOccupied / totalRoomsHotel) * 100));

        return {
          hotelName: hotel.name,
          location: hotel.city || 'N/A',
          occupancyRate,
          occupiedRooms: Math.round(avgOccupied),
          totalRooms: totalRoomsHotel
        };
      } catch (error) {
        return {
          hotelName: hotel.name,
          location: hotel.city || 'N/A',
          occupancyRate: 0,
          occupiedRooms: 0,
          totalRooms: hotel.totalRooms || 0
        };
      }
    });

    const occupancyData = await Promise.all(occupancyPromises);
    
    // Ordenar por tasa de ocupación descendente
    return occupancyData.sort((a, b) => b.occupancyRate - a.occupancyRate);
  }
}
