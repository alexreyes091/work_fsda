import { inject, Injectable } from '@angular/core';
import { HttpParams } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
// Services
import { ClientHttpAltaris } from '@app/lib/ClientHttpAltaris';
// Interfaces
import { IApiResponse, IApiResponsePg } from '../interfaces/IApiResponse';
import { IReservation } from '../interfaces/IReservation';
import { IDashboardStats } from '../interfaces/IDashboardStats';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  private api = inject(ClientHttpAltaris);
  private readonly baseUrl = 'reservation';

  /**
   * Obtiene todas las reservas con paginación.
   */
  async getAll(pageNumber: number = 1, pageSize: number = 10): Promise<IApiResponsePg<IReservation[]>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return firstValueFrom(
      this.api.get<IApiResponsePg<IReservation[]>>(`${this.baseUrl}/all`, params)
    );
  }

  /**
   * Obtiene reservas por rango de fechas.
   */
  async getByDateRange(startDate: string, endDate: string, pageNumber: number = 1, pageSize: number = 10): Promise<IApiResponsePg<IReservation[]>> {
    const params = new HttpParams()
      .set('startDate', startDate)
      .set('endDate', endDate)
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return firstValueFrom(
      this.api.get<IApiResponsePg<IReservation[]>>(`${this.baseUrl}/byRange`, params)
    );
  }

  /**
   * Obtiene reservas por hotel ID.
   */
  async getByHotelId(hotelId: string, pageNumber: number = 1, pageSize: number = 10): Promise<IApiResponsePg<IReservation[]>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return firstValueFrom(
      this.api.get<IApiResponsePg<IReservation[]>>(`${this.baseUrl}/byHotelId/${hotelId}`, params)
    );
  }

  /**
   * Obtiene las estadísticas del dashboard.
   */
  async getDashboardStats(): Promise<IApiResponse<IDashboardStats>> {
    return firstValueFrom(
      this.api.get<IApiResponse<IDashboardStats>>(`${this.baseUrl}/dashboard-stats`)
    );
  }
}
