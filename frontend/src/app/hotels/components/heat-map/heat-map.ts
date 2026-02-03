import { Component, computed, input, output, model } from '@angular/core';
import { DatePipe } from '@angular/common';
// Interfaces
import { ICalendarDay } from '@app/hotels/interfaces/ICalendarDay';

@Component({
  selector: 'app-heat-map',
  imports: [DatePipe],
  templateUrl: './heat-map.html',
})
export class HeatMap { 
  // Inputs Signals
  viewDate = input.required<Date>();
  occupancyData = input.required<Record<string, { occupied: number; available: number; total: number; hotelName: string }>>();
  dataHotels = input.required<Array<{ id: string; name: string }>>();
  // Outputs Events
  previousMonth = output<void>();
  nextMonth = output<void>();
  selectDay = output<ICalendarDay>();
  // Model
  selectedHotelId = model<string>('');

  getHeatmapClass(day: ICalendarDay): string {
    if (!day.isCurrentMonth) return 'bg-base-200 text-base-content';
    
    const p = day.percentage;
    // Invertido: Verde cuando está MÁS DISPONIBLE (menos ocupado)
    if (p <= 20) return 'bg-success text-success-content'; // Verde fuerte - muy disponible
    if (p <= 40) return 'bg-success/70 text-success-content'; // Verde medio
    if (p <= 60) return 'bg-warning text-warning-content'; // Amarillo - medio ocupado
    if (p <= 80) return 'bg-orange-500 text-white'; // Naranja - casi lleno
    return 'bg-error text-error-content'; // Rojo - completamente ocupado
  }

  getTooltipText(day: ICalendarDay): string {
    if (!day.isCurrentMonth || !day.total) return '';
    
    // Formatear fecha manualmente en español
    const diasSemana = ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'];
    const meses = ['enero', 'febrero', 'marzo', 'abril', 'mayo', 'junio', 'julio', 'agosto', 'septiembre', 'octubre', 'noviembre', 'diciembre'];
    
    const diaSemana = diasSemana[day.date.getDay()];
    const dia = day.date.getDate();
    const mes = meses[day.date.getMonth()];
    
    const formattedDate = `${diaSemana}, ${dia} de ${mes}`;
    
    return `${formattedDate}\nDisponibles: ${day.available || 0}\nOcupadas: ${day.occupied || 0}`;
  }

  calendarDays = computed(() => {
    // Data info for the current month
    const date = this.viewDate();
    const year = date.getFullYear();
    const month = date.getMonth();

    // Calculate first day of the month
    const firstDay = new Date(year, month, 1);
    const dayOfWeek = firstDay.getDay();

    const startOffset = dayOfWeek === 0 ? 6 : dayOfWeek - 1;
    let days: any[] = [];

    // -1. Previous month's days
    days = this.generateLastMonth(year, month, startOffset, days);
    // 0. Current month's days
    days = this.generateCurrentMonth(year, month, days);
    // +1. Next month's days 
    days = this.generateNextMonth(year, month, days);
    return days;
  });

  /**
   * Genera los días del mes anterior para completar la primera semana del calendario.
   * @param year Año actual
   * @param month Mes actual (0-11)
   * @param startOffset Número de días a agregar del mes anterior
   * @param days Arreglo donde se agregarán los días
   * @returns Arreglo actualizado con los días del mes anterior
   */
  generateLastMonth(year: number, month: number, startOffset: number, days: any[]) {
    const prevMonthLastDay = new Date(year, month, 0).getDate();
    for (let i = startOffset; i > 0; i--) {
      days.push({
        day: prevMonthLastDay - i + 1,
        date: new Date(year, month - 1, prevMonthLastDay - i + 1),
        isCurrentMonth: false,
        percentage: 0,
        occupied: 0,
        available: 0,
        total: 0,
        hotelName: ''
      });
    }

    return days;
  }

  /**
   * Genera los días del mes actual con los datos de ocupación.
   * @param year Año actual
   * @param month Mes actual (0-11)
   * @param daysInMonth Número de días en el mes actual
   * @param days Arreglo donde se agregarán los días
   * @returns Arreglo actualizado con los días del mes actual
   */
  generateCurrentMonth(year: number, month: number, days: any[]) {
    const lastDay = new Date(year, month + 1, 0).getDate();
    for (let i = 1; i <= lastDay; i++) {
      const fullDate = new Date(year, month, i);
      const dateKey = fullDate.toISOString().split('T')[0]; // Formato YYYY-MM-DD
      
      const dayData = this.occupancyData()[dateKey];
      const occupied = dayData?.occupied || 0;
      const available = dayData?.available || 0;
      const total = dayData?.total || 0;
      const hotelName = dayData?.hotelName || '';
      
      // Calcular porcentaje de ocupación
      const percentage = total > 0 ? Math.round((occupied / total) * 100) : 0;
      
      days.push({
        day: i,
        date: fullDate,
        isCurrentMonth: true,
        percentage: percentage,
        occupied: occupied,
        available: available,
        total: total,
        hotelName: hotelName
      });
    }

    return days;
  }

  /**
   * Genera los días del mes siguiente para completar el calendario.
   * @param year Año actual
   * @param month Mes actual (0-11)
   * @param daysNeeded Número de días necesarios para completar el calendario
   * @param days Arreglo donde se agregarán los días
   * @returns Arreglo actualizado con los días del mes siguiente
   */
  generateNextMonth(year: number, month: number, days: any[]) {
    const remainder = days.length % 7;
    const daysNeeded = remainder === 0 ? 0 : 7 - remainder;
    for (let i = 1; i <= daysNeeded; i++) {
      days.push({
        day: i,
        date: new Date(year, month + 1, i),
        isCurrentMonth: false,
        percentage: 0,
        occupied: 0,
        available: 0,
        total: 0,
        hotelName: ''
      });
    }
    return days;
  }
}
