import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Router } from '@angular/router';
import { TokenApi } from '../models/token-Api';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl: string = 'https://localhost:7109/api/User/';
  private userPayload:any;
  userPayLoad:any;
  constructor(private http: HttpClient, private router: Router) {
    this.userPayLoad = this.decodeToken();
   }

  signUp(userObj: any) {
    return this.http.post<any>(`${this.baseUrl}register?userObj=`, userObj)
  }

  signIn(loginObj : any){
    return this.http.post<any>(`${this.baseUrl}authenticate?loginObj=`,loginObj)
  }

  signOut(){
    localStorage.clear();
    this.router.navigate(['login'])
  }

  storeToken(tokenValue: string){
    localStorage.setItem('token', tokenValue)
  }
  storeRefreshToken(tokenValue: string){
    localStorage.setItem('refreshToken', tokenValue)
  }

  getToken(){
    return localStorage.getItem('token')
  }
  getRefreshToken(){
    return localStorage.getItem('refreshToken')
  }

  isLoggedIn(): boolean{
    return !!localStorage.getItem('token')
  }

  decodeToken(){
    const jwtHelper = new JwtHelperService();
    const token = this.getToken()!;
    return jwtHelper.decodeToken(token);
  }

  getUserNameFromToken(){
    debugger
    if(this.userPayLoad){
      console.log('this.userPayLoad',this.userPayLoad)
      return this.userPayLoad.unique_name;
    }
  }

  getRoleFromToken(){
    if(this.userPayLoad){
      return this.userPayLoad.role;
    }
  }

  reNewToken(token:TokenApi){
    return this.http.post<any>(`${this.baseUrl}refresh`,token);
  }
}