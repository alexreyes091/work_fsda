import { DatePipe } from '@angular/common';
import { Component, signal, input, computed } from '@angular/core';
import { IRoomTypeOccupancy } from '@app/hotels/interfaces/IRoomTypeOccupancy';


@Component({
  selector: 'app-room-type-detail',
  imports: [DatePipe],
  templateUrl: './room-type-detail.html',
})
export class RoomTypeDetail { 
// Inputs que vienen del padre (Hotels.ts)
  startDate = input<Date>(new Date()); 
  hotelId = input.required<number>();
  nextDaysCount: number = (7);

  // Señal cruda con los datos que traerás de tu API de C#
  // En tu rama 'hotel-management', esto se llenará tras un fetch
  rawApiData = signal<any[]>([]); 

  // 1. 💡 Genera dinámicamente el encabezado de los próximos 14 días
  nextFortnight = computed(() => {
    const days = [];
    const start = this.startDate();
    for (let i = 0; i < this.nextDaysCount; i++) {
      const next = new Date(start);
      next.setDate(start.getDate() + i);
      days.push(next);
    }
    return days;
  });

  // 2. 💡 Transforma los datos para el loop de la tabla
  roomOccupancy = computed<IRoomTypeOccupancy[]>(() => {
    const data = this.rawApiData();
    if (data.length === 0) return [];

    // Aquí mapeas la respuesta de tu Backend a la interfaz IRoomTypeOccupancy
    return data.map(item => ({
      typeName: item.name,
      total: item.totalInventory,
      dailyOccupancy: item.availabilityList.map((a: any) => ({
        available: a.count,
        percentage: (1 - (a.count / item.totalInventory)) * 100
      }))
    }));
  });
}