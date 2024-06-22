import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DeployPageComponent } from './deploy-page/deploy-page.component';
import { ResourcesComponent } from './resources/resources.component';
import { NotfoundComponent } from './notfound/notfound.component';
import { UserDashboardComponent } from './user-dashboard/user-dashboard.component';
import { AboutComponent } from './about/about.component';
import { SignupComponent } from './signup/signup.component';
import { LoginComponent } from './login/login.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { AdminUsersComponent } from './admin-users/admin-users.component';
import { AdminResourcesComponent } from './admin-resources/admin-resources.component';

const routes: Routes = [
  {
    path: "",
    redirectTo: "/Home",
    pathMatch: "full"
  },
  {
    path: "signup",
    component: SignupComponent
  },
  {
    path: "login",
    component: LoginComponent
  },
  {
    path: "Home",
    component: HomeComponent
  },
  {
    path: "Deploy",
    component: DeployPageComponent
  },
  {
    path: "Resources",
    component: ResourcesComponent
  },
  {
    path: "About",
    component: AboutComponent
  },
  {
    path: "Dashboard",
    component: UserDashboardComponent
  },
  {
    path: "Admin",
    component: AdminDashboardComponent,
  },
  {
    path: "Admin/Users",
    component: AdminUsersComponent
  },
  {
    path: "Admin/Resources",
    component: AdminResourcesComponent
  },
  {
    path: "**",
    component: NotfoundComponent // Make sure you have a NotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
