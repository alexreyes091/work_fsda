import { Component, input, output, computed, effect } from '@angular/core';
import { 
  getCoreRowModel, 
  createAngularTable, 
  FlexRenderDirective,
  ColumnDef,
} from '@tanstack/angular-table';

@Component({
  selector: 'app-smart-table',
  standalone: true,
  imports: [FlexRenderDirective],
  templateUrl: './smart-table.html',
})
export class SmartTable { 
  // Exponer Math para usar en el template
  Math = Math;
  
  // Signals para inputs
  data = input<any[]>([]);
  columns = input.required<ColumnDef<any, any>[]>();
  totalCount = input<number>(0);
  currentPage = input<number>(1);
  pageSize = input<number>(10);
  
  // Outputs
  onPageChange = output<{ page: number, size: number }>();
  
  // Computed para cálculos de paginación
  totalPages = computed(() => Math.ceil(this.totalCount() / this.pageSize()));
  canPreviousPage = computed(() => this.currentPage() > 1);
  canNextPage = computed(() => this.currentPage() < this.totalPages());
  
  // Tabla TanStack - SOLO para renderizado - (sin paginación del lado del cliente)
  table = createAngularTable(() => ({
    data: this.data(),
    columns: this.columns(),
    getCoreRowModel: getCoreRowModel(),
    manualPagination: true,
  }));

  constructor() {
  }

  // Navegación de página
  goToNextPage() {
    if (this.canNextPage()) {
      this.onPageChange.emit({ 
        page: this.currentPage() + 1, 
        size: this.pageSize() 
      });
    }
  }

  goToPreviousPage() {
    if (this.canPreviousPage()) {
      this.onPageChange.emit({ 
        page: this.currentPage() - 1, 
        size: this.pageSize() 
      });
    }
  }

  changePageSize(newSize: number) {
    this.onPageChange.emit({ 
      page: 1, 
      size: newSize 
    });
  }
}