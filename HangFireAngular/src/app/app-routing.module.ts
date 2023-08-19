import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LogInComponent } from './components/logIn/logIn.component';
import { SignUpComponent } from './components/signUp/signUp.component';
import { MovieComponent } from './components/movie/movie.component';

const routes: Routes = [
  {path:"",component:MovieComponent},
  {path:"movie",component:MovieComponent},
  {path:"login",component:LogInComponent},
  {path:"signup",component:SignUpComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
