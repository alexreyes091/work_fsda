import { inject, Injectable } from '@angular/core';
import { HttpParams } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
// Services
import { ClientHttpAltaris } from '@app/lib/ClientHttpAltaris';
// Interfaces
import { IApiResponse } from '../interfaces/IApiResponse';
import { IRoomOccupancyData } from '../interfaces/IRoomOccupancyData';

@Injectable({
  providedIn: 'root'
})
export class RoomOccupancyService {
  private api = inject(ClientHttpAltaris);
  private readonly baseUrl = 'room-occupancy';

  /**
   * Obtiene la cuadrícula de ocupación para una habitación específica en un rango de fechas.
   * @param roomId El ID de la habitación.
   * @param startDate La fecha de inicio del rango.
   * @param endDate La fecha de fin del rango.
   * @returns Una promesa que resuelve la respuesta de la API con los datos de ocupación.
   */
  async getOccupancyGrid(hotelId: string, startDate: Date, endDate: Date): Promise<IApiResponse<IRoomOccupancyData[]>> {
    const params = new HttpParams()
      .set('startDate', startDate.toISOString())
      .set('endDate', endDate.toISOString());

    return firstValueFrom(
      this.api.get<IApiResponse<IRoomOccupancyData[]>>(`${this.baseUrl}/grid/${hotelId}`, params)
    );
  }

  /**
   * Inicializa el inventario/stock de una habitación.
   * @param roomId El ID de la habitación.
   * @param date La fecha para la cual se inicializa el stock.
   * @param stock La cantidad de stock a inicializar.
   * @returns Una promesa que resuelve la respuesta de la API.
   */
  async initializeStock(roomId: string, date: Date, stock: number): Promise<IApiResponse<string>> {
    const params = new HttpParams()
      .set('roomId', roomId)
      .set('date', date.toISOString())
      .set('stock', stock.toString());

    return firstValueFrom(
      this.api.post<IApiResponse<string>>(`${this.baseUrl}/initialize`, params)
    );
  }
}