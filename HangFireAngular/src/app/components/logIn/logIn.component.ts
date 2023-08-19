import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-logIn',
  templateUrl: './logIn.component.html',
  styleUrls: ['./logIn.component.css']
})
export class LogInComponent implements OnInit {
type:string="password";
isText:boolean=false;
eyeIcon:string="fa-eye-slash"
loginForm!:FormGroup
  constructor(private fb:FormBuilder) { }

  ngOnInit() {
    this.loginForm = this.fb.group({
      username:['',Validators.required],
      password:['',Validators.required]
    });
  }
  showHiddenPassword(){
this.isText = !this.isText
this.isText ? this.eyeIcon="fa-eye" : this.eyeIcon="fa-eye-slash";
this.isText ? this.type = "text" : this.type = "password"
  }
}
