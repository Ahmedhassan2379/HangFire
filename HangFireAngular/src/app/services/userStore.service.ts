import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserStoreService {

  private userName? = new BehaviorSubject<string>('')
  private role? = new BehaviorSubject<string>('')
constructor() {
  
 }

getRoleFromeStore(){
  return this.role?.asObservable();
}

setRoleInStore(role:string){
  return this.role?.next(role);
}

getUserNameFromeStore(){
  return this.userName?.asObservable();
}

setUserNameInStore(userName:string){
  return this.userName?.next(userName);
}

}
