import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastModule, NgToastService } from 'ng-angular-popup';
import { AuthService } from 'src/app/services/Auth.service';
import { UserStoreService } from 'src/app/services/userStore.service';

@Component({
  selector: 'app-logIn',
  templateUrl: './logIn.component.html',
  styleUrls: ['./logIn.component.css'],
})
export class LogInComponent implements OnInit {
  type: string = 'password';
  isText: boolean = false;
  eyeIcon: string = 'fa-eye-slash';
  public loginForm = new FormGroup({
    username: new FormControl(),
    password: new FormControl(),
  });
  constructor(private fb: FormBuilder, private auth: AuthService,private userstore:UserStoreService,private router:Router,private toast:NgToastService) {}

  ngOnInit() {
    // this.loginForm = this.fb.group({
    //   username:['',Validators.required],
    //   password:['',Validators.required]
    // });
  }
  showHiddenPassword() {
    this.isText = !this.isText;
    this.isText ? (this.eyeIcon = 'fa-eye') : (this.eyeIcon = 'fa-eye-slash');
    this.isText ? (this.type = 'text') : (this.type = 'password');
  }

  onSubmit() {
    debugger;
    if (this.loginForm.valid) {
      //send object to database
      this.auth.signIn(this.loginForm.value).subscribe({
        next: (res) => {
          this.loginForm.reset();
          console.log(res);
          this.auth.storeToken(res.accessToken);
          this.auth.storeRefreshToken(res.refreshToken)
          const tokenPatLoad = this.auth.decodeToken();
          this.userstore.setUserNameInStore(tokenPatLoad.userName);
          this.userstore.setRoleInStore(tokenPatLoad.role);
          this.toast.success({detail:"SUCCESS",summary:res.message,duration:5000});
          this.router.navigate(['movie'])
        },
        error:(err)=>{
          this.toast.error({detail:"ERROR",summary:err.message,duration:5000});
        },
      });
    } else {
      //throw exception with toastr
      this.validateAllFormFields(this.loginForm);
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
