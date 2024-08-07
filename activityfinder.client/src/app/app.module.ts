import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { BrowserModule } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, Routes } from '@angular/router';
import { DebounceClickDirective } from './directives/debounce-click.directive';
import { EventFormComponent } from './event-form/event-form.component';
import { EventsListComponent } from './events-list/events-list.component';
import { HttpErrorInterceptor } from './interceptors/http-error.interceptor';
import { AppComponent } from './layout/app.component';
import { LoginRegisterComponent } from './login-register/login-register.component';
import { AuthenticateService } from './services/authenticate.service';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorIntl, MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { PolishPaginatorIntl } from './mat-extensions/polish-paginator-intl';
import { LoadingInterceptor } from './interceptors/loading.interceptor';
import { MatMenuModule} from '@angular/material/menu';
import { AppTableComponent } from './app-table/app-table.component';
import { MatRadioModule } from '@angular/material/radio';
import { EventCommentsComponent } from './event-comments/event-comments.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { UsersEventsComponent } from './users-events/users-events.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ContactComponent } from './contact/contact.component';
import { AuthGuard } from './auth.guard';

const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'event-form', component: EventFormComponent, canActivate: [AuthGuard] },
  { path: 'event-form/:id', component: EventFormComponent, canActivate: [AuthGuard] },
  { path: 'events-list', component: EventsListComponent },
  { path: 'authenticate', component: LoginRegisterComponent },
  { path: 'event-comments/:id', component: EventCommentsComponent, canActivate: [AuthGuard] },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  { path: 'users-events', component: UsersEventsComponent, canActivate: [AuthGuard] },
  { path: 'contact', component: ContactComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    EventFormComponent,
    EventsListComponent,
    DebounceClickDirective,
    LoginRegisterComponent,
    AppTableComponent,
    EventCommentsComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    UsersEventsComponent,
    LandingPageComponent,
    ContactComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(routes),
    HttpClientModule,    
    NoopAnimationsModule,
    ReactiveFormsModule,
    FormsModule,
    MatIconModule, MatTabsModule, MatInputModule, MatButtonModule, MatCardModule, MatAutocompleteModule, MatCheckboxModule, MatToolbarModule,
    MatSidenavModule, MatListModule, MatTableModule, MatPaginatorModule, MatSortModule, MatSelectModule, MatProgressSpinnerModule, MatMenuModule,
    MatRadioModule, MatTooltipModule,
    FontAwesomeModule
  ],
  providers: [
    AuthenticateService,
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true },
    { provide: MatPaginatorIntl, useClass: PolishPaginatorIntl },    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
