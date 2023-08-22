import { Component, OnInit } from '@angular/core';
import { ApiUserService } from 'src/app/services/ApiUser.service';
import { AuthService } from 'src/app/services/Auth.service';

@Component({
  selector: 'app-userDashbnoard',
  templateUrl: './userDashbnoard.component.html',
  styleUrls: ['./userDashbnoard.component.css']
})
export class UserDashbnoardComponent implements OnInit {
  allUsers :any=[]
  constructor(private users:ApiUserService,private auth:AuthService) { }

  ngOnInit() {
    this.users.getAllUsers().subscribe((data:any)=>{
      console.log(data);
      this.allUsers = data
    });
  }

  logout() {
    this.auth.signOut();
  }

}
