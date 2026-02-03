import { Component, inject } from "@angular/core";
import { RouterLinkActive, RouterLink, RouterOutlet } from "@angular/router";
// Services
import { UiService } from "@app/hotels/services";

@Component({
  selector: "app-menu",
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: "./menu.html",
})
export class Menu {
  private uiService = inject(UiService);
  titleMenu = this.uiService.title;
}
