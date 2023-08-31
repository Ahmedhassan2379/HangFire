import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {ResetPassword} from '../models/reset-password'

@Injectable({
  providedIn: 'root'
})
export class ResetPasswordService {
private baseUrl :string = 'https://localhost:7109/api/User/'
constructor(private Http : HttpClient) { }

sendResetPasswordLink(email:string){
  debugger;
return this.Http.post<any>(`${this.baseUrl}send-reset-email/${email}`,{});
}

resetPassword(passwordModel:ResetPassword){
  return this.Http.post<any>(`${this.baseUrl}reset-password`,passwordModel);
}

}
