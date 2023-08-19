import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-signUp',
  templateUrl: './signUp.component.html',
  styleUrls: ['./signUp.component.css']
})
export class SignUpComponent implements OnInit {
  type:string="password";
  isText:boolean=false;
  eyeIcon:string="fa-eye-slash"
    constructor() { }
  
    ngOnInit() {
    }
    showHiddenPassword(){
  this.isText = !this.isText
  this.isText ? this.eyeIcon="fa-eye" : this.eyeIcon="fa-eye-slash";
  this.isText ? this.type = "text" : this.type = "password"
    }
}
