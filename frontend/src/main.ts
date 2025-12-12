import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import {  provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes'; 
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { JwtInterceptor } from '@auth0/angular-jwt';
bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(),
    provideRouter(routes),
    provideAnimations(), provideAnimationsAsync('noop'), 
    provideHttpClient(withInterceptorsFromDi()), 
    { provide: JwtInterceptor, useClass: JwtInterceptor }
  ]
}).catch(err => console.error(err));
