import { Component, inject, OnInit, signal } from '@angular/core';
// Services
import { UiService, ReservationService } from '@app/hotels/services';
// Components
import { SmartTable } from '@app/hotels/components';
// Interfaces
import { IReservation } from '@app/hotels/interfaces/IReservation';
import { ColumnDef } from '@tanstack/angular-table';

@Component({
  selector: 'app-reservations',
  imports: [SmartTable],
  templateUrl: './reservations.html',
})
export class Reservations implements OnInit {
  // Servicios inyectados
  private uiService = inject(UiService);
  private reservationService = inject(ReservationService);

  // Signals
  reservationsList = signal<IReservation[]>([]);
  isLoading = signal(false);
  totalCount = signal(0);
  currentPage = signal(1);
  pageSize = signal(10);

  async ngOnInit() {
    this.uiService.setTitle('Gestión de Reservas');
    await this.loadReservations();
  }

  columns: ColumnDef<IReservation>[] = [
    {
      header: 'ID',
      accessorKey: 'id',
      cell: info => {
        const id = info.getValue<string>();
        return id ? id.substring(0, 8) : 'N/A';
      }
    },
    {
      header: 'Huésped',
      accessorKey: 'guestFullName',
      cell: info => {
        const reservation = info.row.original;
        return `${reservation.guestFullName} - ${reservation.guestEmail}`;
      }
    },
    {
      header: 'Hotel',
      accessorKey: 'roomHotelName',
    },
    {
      header: 'Habitación',
      accessorKey: 'roomName',
    },
    {
      header: 'Check-in',
      accessorKey: 'checkIn',
      cell: info => {
        const date = new Date(info.getValue<string>());
        return date.toLocaleDateString('es-ES', { day: '2-digit', month: 'short', year: 'numeric' });
      }
    },
    {
      header: 'Check-out',
      accessorKey: 'checkOut',
      cell: info => {
        const date = new Date(info.getValue<string>());
        return date.toLocaleDateString('es-ES', { day: '2-digit', month: 'short', year: 'numeric' });
      }
    },
    {
      header: 'Noches',
      accessorKey: 'nights',
      cell: info => `${info.getValue<number>()} noches`
    },
    {
      header: 'Precio',
      accessorKey: 'price',
      cell: info => {
        const price = info.getValue<number>();
        return new Intl.NumberFormat('es-ES', {
          style: 'currency',
          currency: 'EUR'
        }).format(price);
      }
    },
    {
      header: 'Estado',
      accessorKey: 'statusReservation',
      cell: info => {
        const status = info.getValue<string>();
        return this.getStatusBadge(status);
      }
    },
    {
      id: 'actions',
      header: 'Acciones',
      cell: info => {
        return `...`; // Placeholder para acciones
      }
    }
  ];

  // Cargar reservas desde el servicio, por cada página
  async loadReservations(page: number = 1, size: number = 10) {
    try {
      this.isLoading.set(true);
      const response = await this.reservationService.getAll(page, size);

      if (response.isSuccess) {
        this.reservationsList.set([...response.data]);

        if (response.pagination)
          this.totalCount.set(response.pagination.totalCount);
      }
    } catch (error) {
      console.error('❌ Error cargando reservas:', error);
    } finally {
      this.isLoading.set(false);
    }
  }

  // Manejar cambios de paginación desde SmartTable
  async onTablePageChange(event: { page: number, size: number }) {
    this.currentPage.set(event.page);
    this.pageSize.set(event.size);
    await this.loadReservations(event.page, event.size);
  }

  // Obtener badge HTML para el estado
  getStatusBadge(status: string): string {
    const statusConfig: Record<string, { label: string, class: string }> = {
      'Pending': { label: 'Pendiente', class: 'badge-warning' },
      'Confirmed': { label: 'Confirmada', class: 'badge-info' },
      'Cancelled': { label: 'Cancelada', class: 'badge-error' },
      'CheckedIn': { label: 'En curso', class: 'badge-success' },
      'CheckedOut': { label: 'Finalizada', class: 'badge-ghost' }
    };

    const config = statusConfig[status] || { label: status, class: 'badge-ghost' };
    return `${config.label}`;
  }

  handleEdit(reservation: IReservation) {
    console.log('Editando reserva:', reservation);
  }

  handleView(reservation: IReservation) {
    console.log('Ver detalles de reserva:', reservation);
  }

  handleCancel(reservation: IReservation) {
    console.log('Cancelar reserva:', reservation);
  }
}
