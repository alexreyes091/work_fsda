import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
// Services
import { UiService, DashboardService, ReservationService } from '@app/hotels/services';
// Interfaces
import { IDashboardStats, IHotelOccupancy } from '@app/hotels/interfaces/IDashboardStats';
import { IReservation } from '@app/hotels/interfaces/IReservation';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.html',
})
export class Dashboard implements OnInit {
  private uiService = inject(UiService);
  private dashboardService = inject(DashboardService);
  private reservationService = inject(ReservationService);

  // Signals
  stats = signal<IDashboardStats>({
    totalHotels: 0,
    totalRooms: 0,
    activeReservations: 0,
    occupancyRate: 0,
    checkInsToday: 0,
    checkOutsToday: 0,
    pendingReservations: 0,
    monthlyRevenue: 0
  });
  
  hotelOccupancy = signal<IHotelOccupancy[]>([]);
  recentReservations = signal<IReservation[]>([]);
  isLoading = signal<boolean>(true);

  // Computed signal para limitar hoteles a mostrar (top 8)
  topHotelOccupancy = computed(() => {
    return this.hotelOccupancy().slice(0, 8);
  });

  // Computed signal para saber si hay más hoteles
  hasMoreHotels = computed(() => {
    return this.hotelOccupancy().length > 8;
  });

  async ngOnInit() {
    this.uiService.setTitle('Dashboard');
    await this.loadDashboardData();
  }

  async loadDashboardData() {
    try {
      this.isLoading.set(true);

      // Cargar datos en paralelo
      const [statsData, occupancyData, reservationsResponse] = await Promise.all([
        this.dashboardService.getDashboardStats(),
        this.dashboardService.getHotelOccupancy(),
        this.reservationService.getAll(1, 8)
      ]);

      this.stats.set(statsData);
      this.hotelOccupancy.set(occupancyData);
      this.recentReservations.set(reservationsResponse.data || []);

    } catch (error) {
      console.error('Error loading dashboard data:', error);
    } finally {
      this.isLoading.set(false);
    }
  }

  getStatusBadgeClass(status: string): string {
    const statusClasses: Record<string, string> = {
      'Pending': 'badge-warning',
      'Confirmed': 'badge-info',
      'Cancelled': 'badge-error',
      'CheckedIn': 'badge-success',
      'CheckedOut': 'badge-ghost'
    };
    return statusClasses[status] || 'badge-ghost';
  }

  getStatusLabel(status: string): string {
    const statusLabels: Record<string, string> = {
      'Pending': 'Pendiente',
      'Confirmed': 'Confirmada',
      'Cancelled': 'Cancelada',
      'CheckedIn': 'En curso',
      'CheckedOut': 'Finalizada'
    };
    return statusLabels[status] || status;
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('es-ES', {
      style: 'currency',
      currency: 'EUR'
    }).format(value);
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('es-ES', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  getInitials(fullName: string): string {
    return fullName
      .split(' ')
      .map(n => n[0])
      .join('')
      .substring(0, 2)
      .toUpperCase();
  }
}

