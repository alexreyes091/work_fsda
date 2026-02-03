import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: "",
        loadChildren: () => import("@app/hotels/routes").then(m => m.routes),
    },
    {
        path: '',
        redirectTo: 'hotels',
        pathMatch: 'full'
    }
];
