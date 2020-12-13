import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ReservationsComponent } from './components/reservations/reservations.component';
import { EditReservationComponent } from './components/edit-reservation/edit-reservation.component';
import { HttpService } from './services/http.service';
import { NotificationsService, SimpleNotificationsModule } from 'angular2-notifications';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ContactListComponent } from './components/contact-list/contact-list.component';
import { DataTablesModule } from 'angular-datatables';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    ReservationsComponent,
    EditReservationComponent,
    ContactListComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    CKEditorModule,
    DataTablesModule,
    LoadingBarHttpClientModule,
    SimpleNotificationsModule.forRoot({
      timeOut: 3000,
      showProgressBar: true,
      pauseOnHover: true,
      clickToClose: false,
      clickIconToClose: true,
      position: ['top', 'center']
    }),
    RouterModule.forRoot([
      { path: '', component: ReservationsComponent, pathMatch: 'full' },
      { path: 'create-reservation', component: EditReservationComponent },
      { path: 'create-reservation/:reservationId', component: EditReservationComponent },
      { path: 'create-contact', component: EditReservationComponent },
      { path: 'create-contact/:contactId', component: EditReservationComponent },
      { path: 'contact-list', component: ContactListComponent },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
    ], { relativeLinkResolution: 'legacy' }),
    NgbModule,
  ],
  providers: [HttpService, NotificationsService],
  bootstrap: [AppComponent]
})
export class AppModule { }
