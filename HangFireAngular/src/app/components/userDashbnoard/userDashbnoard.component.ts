import { Component, OnInit } from '@angular/core';
import { ApiUserService } from 'src/app/services/ApiUser.service';
import { AuthService } from 'src/app/services/Auth.service';
import { UserStoreService } from 'src/app/services/userStore.service';

@Component({
  selector: 'app-userDashbnoard',
  templateUrl: './userDashbnoard.component.html',
  styleUrls: ['./userDashbnoard.component.css']
})
export class UserDashbnoardComponent implements OnInit {
  allUsers :any=[]
  public myUserName :string=""
  constructor(private users:ApiUserService,private auth:AuthService,private userstore:UserStoreService) { }

  ngOnInit() {
    this.users.getAllUsers().subscribe((data:any)=>{
      console.log(data);
      this.allUsers = data
    });
this.userstore.getUserNameFromeStore()?.subscribe(x=>{
  let userNameFromToken = this.auth.getUserNameFromToken();
  this.myUserName = x ||userNameFromToken
})
  }

  logout() {
    this.auth.signOut();
  }

}
