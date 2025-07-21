// src/app/app.routes.ts
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'videos',
    loadComponent: () => import('./pages/video-list/video-list').then(m => m.VideoList)
  },
  {
    path: 'upload',
    loadComponent: () => import('./pages/video-upload/video-upload').then(m => m.VideoUpload)
  }
];
