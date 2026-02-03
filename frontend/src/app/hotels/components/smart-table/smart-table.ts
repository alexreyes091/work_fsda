import { Component, input, signal, computed } from '@angular/core';
import { 
  getCoreRowModel, 
  getFilteredRowModel, 
  createAngularTable, 
  FlexRenderDirective,
  ColumnDef,
  getPaginationRowModel
} from '@tanstack/angular-table';

@Component({
  selector: 'app-smart-table',
  standalone: true,
  imports: [FlexRenderDirective],
  templateUrl: './smart-table.html',
})
export class SmartTable { 
  // Signals
  filterText = signal(''); 
  data = input.required<any[]>();
  columns = input.required<ColumnDef<any, any>[]>();

  // Configuración de TanStack
  table = createAngularTable(() => ({
    data: this.data(),
    columns: this.columns(),
    state: {
      globalFilter: this.filterText(),
    },
    onGlobalFilterChange: (value) => this.filterText.set(value),
    getCoreRowModel: getCoreRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    initialState: {
      pagination: {
        pageSize: 10, 
      },
    },
  }));

  /**
   * Actualiza el texto de búsqueda basado en el evento de entrada
   * @param event Evento de entrada del usuario
   */
  updateSearch(event: Event) {
    const inputElement = event.target as HTMLInputElement;
    this.filterText.set(inputElement.value);
  }
}