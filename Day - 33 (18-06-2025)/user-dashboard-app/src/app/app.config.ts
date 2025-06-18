import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideCharts } from 'ng2-charts';
import { appRoutes } from './app.routes'; // 👈 imported here

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(),
    provideCharts(),
    provideRouter(appRoutes) // 👈 routing done via appRoutes
  ]
};
