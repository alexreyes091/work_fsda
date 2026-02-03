import { Component, computed, inject, OnInit, signal, effect } from '@angular/core';

// Services
import { HotelService, RoomOccupancyService, UiService } from '@app/hotels/services';
// Interfaces
import { ICalendarDay } from '@app/hotels/interfaces/ICalendarDay';
import { IRoomOccupancyData } from '@app/hotels/interfaces/IRoomOccupancyData';
import { HeatMap } from "@app/hotels/components/heat-map/heat-map";

@Component({
  selector: 'app-room-occupancy',
  imports: [HeatMap],
  templateUrl: './RoomOccupancy.html',
})
export class RoomOccupancy  implements OnInit {
  // Servicios inyectados
  private uiService = inject(UiService);
  private hotelService = inject(HotelService);
  private occupancyService = inject(RoomOccupancyService);
  // Signals
  viewDate = signal<Date>(new Date());
  selectedHotelId = signal<string>('');
  dataHotels = signal<Array<{ id: string; name: string }>>([]);
  // Data de ocupación de habitaciones - estructura para el mapa de calor
  // Formato: { 'YYYY-MM-DD': { occupied, available, total, hotelName } }
  occupancyData = signal<Record<string, { occupied: number; available: number; total: number; hotelName: string }>>({}); 
  selectedHotelName = signal<string>('');

  async getAllHotel(){
    const pageNumber = 1;
    // Obtener todos los hoteles (sin paginación) 
    // TODO!: Ajustar en el servicio si es necesario
    const pageSize = 1000;
    const hotels = await this.hotelService.getAll(pageNumber, pageSize);
    console.log('Hoteles obtenidos para RoomOccupancy:', hotels);
    // Mapear datos para el select
    this.dataHotels.set(hotels.data.map(hotel => ({ id: hotel.id, name: hotel.name })));
  }

  constructor() {
    effect(async () => {
      const hotelId = this.selectedHotelId();
      const date = this.viewDate();

      if (hotelId) {
        // Calculamos el rango del mes para la cuadrícula
        const start = new Date(date.getFullYear(), date.getMonth(), 1);
        const end = new Date(date.getFullYear(), date.getMonth() + 1, 0);

        const response = await this.occupancyService.getOccupancyGrid(hotelId, start, end);
        if (response.isSuccess && response.data) {
          // Transformar los datos del API a la estructura que necesita el mapa de calor
          const transformedData: Record<string, { occupied: number; available: number; total: number; hotelName: string }> = {};
          
          response.data.forEach((item: IRoomOccupancyData) => {
            const dateKey = item.date.split('T')[0]; // Asegurar formato YYYY-MM-DD
            const available = item.totalRooms - item.occupiedCount;
            
            transformedData[dateKey] = {
              occupied: item.occupiedCount,
              available: available,
              total: item.totalRooms,
              hotelName: item.roomName
            };
          });

          this.occupancyData.set(transformedData);
          
          // Guardar el nombre del hotel seleccionado
          if (response.data.length > 0) {
            this.selectedHotelName.set(response.data[0].roomName);
          }
        }
      }
    });
  }

  ngOnInit(): void {
    this.uiService.setTitle('Disponibilidad de Habitaciones');
    this.getAllHotel();
  }

  onSelectDay(day: ICalendarDay) {
    console.log('=== Información del día seleccionado ===');
    console.log('Fecha:', day.date.toLocaleDateString('es-ES', { year: 'numeric', month: 'long', day: 'numeric' }));
    console.log('Hotel:', day.hotelName || this.selectedHotelName());
    console.log('Habitaciones Disponibles:', day.available || 0);
    console.log('Habitaciones Ocupadas:', day.occupied || 0);
    console.log('Total de Habitaciones:', day.total || 0);
    console.log('Porcentaje de Ocupación:', day.percentage + '%');
    console.log('========================================');
  }

  // Controllers for HeatMap navigation
  onNextMonth(){
    const date = this.viewDate();
    this.viewDate.set(new Date(date.getFullYear(), date.getMonth() + 1, 1));
  }
  // Controllers for HeatMap navigation
  onPreviousMonth(){
    const date = this.viewDate();
    this.viewDate.set(new Date(date.getFullYear(), date.getMonth() - 1, 1));
  }


}
