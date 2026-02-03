import { HotelService } from './../../services/HotelService';
import { Component, inject, OnInit, signal } from '@angular/core';
// Services
import { UiService } from '@app/hotels/services';
// Components
import { SmartTable } from '@app/hotels/components';
// Interfaces
import { IHotel } from '@app/hotels/interfaces/IHotel';
import { ColumnDef } from '@tanstack/angular-table';


@Component({
  selector: 'app-hotels',
  imports: [SmartTable],
  templateUrl: './hotels.html',
})
export class Hotels implements OnInit {
  // Servicios inyectados
  private uiService = inject(UiService);
  private hotelService = inject(HotelService);

  // Signals
  hotelsList = signal<IHotel[]>([]);
  isLoading = signal(false);
  totalCount = signal(0); 
  currentPage = signal(1);
  pageSize = signal(10);

  async ngOnInit(){
    this.uiService.setTitle('Gestión de Hoteles');
    await this.loadHotels();
  }

columns: ColumnDef<IHotel>[] = [
  {
    header: 'Nombre del Hotel',
    accessorKey: 'name',
  },
  {
    header: 'Ubicación',
    accessorKey: 'city',
    cell: info => `${info.row.original.city}, ${info.row.original.country}`
  },
  {
    header: 'Estrellas',
    accessorKey: 'stars',
    cell: info => '⭐'.repeat(info.getValue<number>())
  },
  {
    header: 'Contacto',
    accessorKey: 'email',
    cell: info => info.getValue()
  },
  {
    header: 'Habitaciones',
    accessorKey: 'totalRooms',
    cell: info => info.getValue()
  },
  {
    id: 'actions',
    header: 'Acciones',
    cell: info => {
      const hotel = info.row.original;
      return `...`; // Placeholder para acciones, TODO: Reemplazar con botones reales en la plantilla
    }
  }
];

  // Cargar hoteles desde el servicio, por cada página
  async loadHotels(page: number = 1, size: number = 10) {
    try {
      this.isLoading.set(true);
      const response = await this.hotelService.getAll(page, size);
      
      if (response.isSuccess) {
        this.hotelsList.set([...response.data]);
        
        if (response.pagination) 
          this.totalCount.set(response.pagination.totalCount);
      }
    } catch (error) {
      console.error('❌ Error en Altairis:', error);
    } finally {
      this.isLoading.set(false);
    }
  }

  // Manejar cambios de paginación desde SmartTable
  async onTablePageChange(event: { page: number, size: number }) {
    this.currentPage.set(event.page);
    this.pageSize.set(event.size);
    await this.loadHotels(event.page, event.size);
  }

  handleEdit(hotel: any) {
    console.log('Editando hotel:', hotel);
  }
}
