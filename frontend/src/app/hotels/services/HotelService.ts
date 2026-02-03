import { firstValueFrom } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
// Services
import { ClientHttpAltaris } from '@app/lib/ClientHttpAltaris';
// Interfaces
import { IHotel } from '../interfaces/IHotel';
import { IApiResponse } from '../interfaces/IApiResponse';

@Injectable({
  providedIn: 'root'
})
export class HotelService {
  private api = inject(ClientHttpAltaris);
  private readonly baseUrl = 'hotel';

  /**
   * Get all hotels with pagination.
   * @param page The page number to retrieve.
   * @param size The number of items per page.
   * @returns An observable of the API response containing a list of hotels.
   */
  getAll(page: number = 1, size: number = 10): Promise<IApiResponse<IHotel[]>> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('size', size.toString());

    return firstValueFrom(
      this.api.get<IApiResponse<IHotel[]>>(`${this.baseUrl}/all`, params)
    );
  }

  /**
   * Get a hotel by its ID.
   * @param id The ID of the hotel to retrieve.
   * @returns An observable of the API response containing the hotel details.
   */
  async create(data: IHotel): Promise<IApiResponse<IHotel>> {
    return firstValueFrom(
      this.api.post<IApiResponse<IHotel>>(`${this.baseUrl}/create`, data)
    );
  }
}
