

import { HotelsLayout } from "./layouts";
import { Routes } from "@angular/router";


export const routes: Routes = [
    {
        path: '',
        component: HotelsLayout,
        children: [
            {
                path: 'dashboard',
                loadComponent: () => import('./pages').then(m => m.Dashboard),
            },
            {
                path:'hotels',
                loadComponent: () => import('./pages').then(m => m.Hotels),
            },
            {
                path: '', redirectTo: 'hotels', pathMatch: 'full'
            }
        ]
    }
]