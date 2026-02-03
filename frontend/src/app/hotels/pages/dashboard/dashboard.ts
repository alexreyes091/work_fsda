import { Component, inject, OnInit } from '@angular/core';
// Services
import { UiService } from '@app/hotels/services';

@Component({
  selector: 'app-dashboard',
  imports: [],
  template: `<p>dashboard works!</p>`,
})
export class Dashboard implements OnInit {
  private uiService = inject(UiService);

  ngOnInit(){
    this.uiService.setTitle('Dashboard');
  }
}
