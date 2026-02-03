import { Component, signal } from '@angular/core';
import { Menu } from "./shared/menu/menu";

@Component({
  selector: 'app-root',
  imports: [Menu],
  templateUrl: './app.html',
})
export class App {
  protected readonly title = signal('frontend');
}
