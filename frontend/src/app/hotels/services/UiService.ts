import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UiService {

  title = signal<string>('Dashboard');

  setTitle(newTitle: string) {
    this.title.set(newTitle);
  }

}
