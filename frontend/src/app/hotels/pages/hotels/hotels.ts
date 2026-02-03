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
    cell: info => info.row.original 
  }
];

  async loadHotels() {
    try {
      this.isLoading.set(true);

      const response = await this.hotelService.getAll();
      
      if (response.isSuccess) {
        this.hotelsList.set(response.data);
      }
    } catch (error) {
      console.error('Error en Altairis:', error);
    } finally {
      this.isLoading.set(false);
    }
  }

  handleEdit(hotel: any) {
    console.log('Editando hotel:', hotel);
  }
}
