import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApplicationInputComponent } from './application-input/application-input.component';
import { MainNavbarComponent } from './main-navbar/main-navbar.component';

const routes: Routes = [
  
  { path: "apply", component: ApplicationInputComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
