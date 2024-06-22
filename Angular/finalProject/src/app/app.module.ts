import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserResourcesComponent } from './user-resources/user-resources.component';
import { UserNavbarComponent } from './user-navbar/user-navbar.component';
import { HomeComponent } from './home/home.component';
import { DeployPageComponent } from './deploy-page/deploy-page.component';
import { ResourcesComponent } from './resources/resources.component';
import { NotfoundComponent } from './notfound/notfound.component';
import { UserDashboardComponent } from './user-dashboard/user-dashboard.component';
import { AboutComponent } from './about/about.component';
import { FooterComponent } from './footer/footer.component';
import { LoginComponent } from './login/login.component';
import { SignupComponent } from './signup/signup.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { AdminUsersComponent } from './admin-users/admin-users.component';
import { AdminResourcesComponent } from './admin-resources/admin-resources.component';
import { AdminNavbarComponent } from './admin-navbar/admin-navbar.component';
import { ChartComponent } from './chart/chart.component';
import { InitialPipe } from './initial.pipe';


@NgModule({
  declarations: [
    AppComponent,
    UserResourcesComponent,
    UserNavbarComponent,
    HomeComponent,
    DeployPageComponent,
    ResourcesComponent,
    NotfoundComponent,
    UserDashboardComponent,
    AboutComponent,
    FooterComponent,
    LoginComponent,
    SignupComponent,
    AdminDashboardComponent,
    AdminUsersComponent,
    AdminResourcesComponent,
    AdminNavbarComponent,
    ChartComponent,
    InitialPipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    provideClientHydration()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
