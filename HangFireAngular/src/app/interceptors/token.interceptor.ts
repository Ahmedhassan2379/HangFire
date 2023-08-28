import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/Auth.service';
import { NgToastService } from 'ng-angular-popup';
import { Router } from '@angular/router';
import { TokenApi } from '../models/token-Api';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private auth :AuthService,private toast :NgToastService,private router : Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.auth.getToken();
    if(token){
      var request = request.clone({
        setHeaders:{Authorization:`Bearer ${token}`}
      })
    }
    return next.handle(request).pipe(
      catchError((err:any)=>{
        if(err instanceof HttpErrorResponse){
          if(err.status === 401){
            // this.toast.warning({detail:"WARNING",summary:"Token Is Expired Please Login Again"});
            // this.router.navigate(['login']);
           return this.handUnAuthorizedError(request,next);
          }
        }
        return throwError(()=>new Error("some other error occured!"))
      })
    );
  }

  handUnAuthorizedError(req:HttpRequest<any>,next : HttpHandler){
    var tokenApi = new TokenApi();
    tokenApi.accessToken=this.auth.getToken()!;
    tokenApi.refreshToken = this.auth.getRefreshToken()!;
    return this.auth.reNewToken(tokenApi).pipe(
      switchMap((data:TokenApi)=>{
        this.auth.storeRefreshToken(data.refreshToken);
        this.auth.storeToken(data.accessToken);
        req = req.clone({
          setHeaders:{Authorization:`Bearer ${data.accessToken}`}
        })
        return next.handle(req);
      }),
      catchError((err)=>{
        return throwError(()=>{
          this.toast.warning({detail:"WARNING",summary:"Token Is Expired Please Login Again"});
          this.router.navigate(['login']);
        })     
      })
    )
  }
}
