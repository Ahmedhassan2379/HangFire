import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { AuthService } from 'src/app/services/Auth.service';

@Component({
  selector: 'app-signUp',
  templateUrl: './signUp.component.html',
  styleUrls: ['./signUp.component.css'],
})
export class SignUpComponent implements OnInit {
  type: string = 'password';
  isText: boolean = false;
  eyeIcon: string = 'fa-eye-slash';
  public signupForm = new FormGroup({
    firstname: new FormControl(),
    lastname: new FormControl(),
    username: new FormControl(),
    email: new FormControl(),
    password: new FormControl(),
  });
  constructor(private auth: AuthService,private router:Router,private toast:NgToastService) {}

  ngOnInit() {}

  showHiddenPassword() {
    this.isText = !this.isText;
    this.isText ? (this.eyeIcon = 'fa-eye') : (this.eyeIcon = 'fa-eye-slash');
    this.isText ? (this.type = 'text') : (this.type = 'password');
  }

  onSubmit() {
    debugger
    if (this.signupForm.valid) {
      //send object to database
      this.auth.signUp(this.signupForm.value).subscribe({
        next:(res)=>{
          this.toast.success({detail:"Success",summary:res.message,duration:5000})
          this.router.navigate(['login']);   
        },
        error:(err)=>{
          this.toast.error({detail:"ERROR",summary:err.message,sticky:true})
        }
      });
    } else {
      //throw exception with toastr
      this.validateAllFormFields(this.signupForm);
    }
  }

  validateAllFormFields(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach((el) => {
      const control = formGroup.get(el);
      if (control instanceof FormControl) {
        control.markAsDirty({ onlySelf: true });
      } else if (control instanceof FormGroup) {
        this.validateAllFormFields(control);
      }
    });
  }
}
