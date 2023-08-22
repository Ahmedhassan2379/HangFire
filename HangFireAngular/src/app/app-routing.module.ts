import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LogInComponent } from './components/logIn/logIn.component';
import { SignUpComponent } from './components/signUp/signUp.component';
import { MovieComponent } from './components/movie/movie.component';
import { AuthGuard } from './Guards/auth.guard';
import { UserDashbnoardComponent } from './components/userDashbnoard/userDashbnoard.component';
import { CategoryComponent } from './components/category/category.component';

const routes: Routes = [
  {path:"",redirectTo:'login', pathMatch:'full'},
  {path:"login",component:LogInComponent},
  {path:"signup",component:SignUpComponent},
  {path:"users",component:UserDashbnoardComponent,canActivate:[AuthGuard]},
  {path:"movie",component:MovieComponent,canActivate:[AuthGuard]},
  {path:"category",component:CategoryComponent,canActivate:[AuthGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
