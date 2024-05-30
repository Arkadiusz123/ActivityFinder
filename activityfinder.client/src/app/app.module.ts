import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './layout/app.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { EventFormComponent } from './event-form/event-form.component';
import { RouterModule, Routes } from '@angular/router';
import { EventsListComponent } from './events-list/events-list.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpErrorInterceptor } from './http-error.interceptor';

const routes: Routes = [
  { path: '', component: EventsListComponent },
  { path: 'event-form', component: EventFormComponent },
  { path: 'events-list', component: EventsListComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    EventFormComponent,
    EventsListComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(routes),
    HttpClientModule,
    MatToolbarModule,
    NoopAnimationsModule,
    MatIconModule,
    MatTabsModule,
    ReactiveFormsModule, MatInputModule, MatButtonModule, MatCardModule, MatAutocompleteModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
