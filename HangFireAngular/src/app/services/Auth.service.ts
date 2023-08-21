import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";


@Injectable({
  providedIn: 'root'
})
export class AuthService {

constructor(private http:HttpClient) { }
url="https://localhost:7109/api/User"

signUp(userObj:any) {
  debugger;
  return this.http.post<any>(`${this.url}/register?userObj=`,userObj);
}

LogIn(userObj:any){
  debugger;
  return this.http.post<any>(`${this.url}/authenticate?userObj=`,userObj);

}

}
