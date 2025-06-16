import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter, Routes } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { App } from './app/app';
import { Home } from './app/pages/home/home';
import { About } from './app/pages/about/about';

const routes: Routes = [
  { path: 'home', component: Home },
  { path: 'about', component: About },
  { path: '', redirectTo: 'home', pathMatch: 'full' }
];

bootstrapApplication(App, {
  providers: [
    provideRouter(routes),
    provideHttpClient()
  ]
});
