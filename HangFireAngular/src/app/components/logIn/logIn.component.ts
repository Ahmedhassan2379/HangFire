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
import { ResetPasswordService } from 'src/app/services/reset-password.service';
import { UserStoreService } from 'src/app/services/userStore.service';
// import { faEye,faEyeSlash } from '@fortawesome/free-solid-svg-icons'
// import { FaIconLibrary } from '@fortawesome/angular-fontawesome';

@Component({
  selector: 'app-logIn',
  templateUrl: './logIn.component.html',
  styleUrls: ['./logIn.component.css'],
})
export class LogInComponent implements OnInit {
  public resetPasswordEmail!: string;
  public isValidEmail!: boolean;
  type: string = 'password';
  isText: boolean = false;
  Icon: string = 'fa-eye-slash';
  public loginForm = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required),
  });
  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private userstore: UserStoreService,
    private router: Router,
    private toast: NgToastService,
    private resetPassword : ResetPasswordService,
  ) {}

  ngOnInit() {
    // this.loginForm = this.fb.group({
    //   username:['',Validators.required],
    //   password:['',Validators.required]
    // });
  }
  showHiddenPassword() {
    this.isText = !this.isText;
    this.isText ? (this.Icon = 'fa-eye') : (this.Icon = 'fa-eye-slash');
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
          this.auth.storeRefreshToken(res.refreshToken);
          const tokenPatLoad = this.auth.decodeToken();
          this.userstore.setUserNameInStore(tokenPatLoad.userName);
          this.userstore.setRoleInStore(tokenPatLoad.role);
          this.toast.success({
            detail: 'SUCCESS',
            summary: res.message,
            duration: 5000,
          });
          this.router.navigate(['movie']);
        },
        error: (err) => {
          this.toast.error({
            detail: 'ERROR',
            summary: err.message,
            duration: 5000,
          });
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

  openForgetPasswordModal() {
    const modal = document.getElementById('exampleModal');
    if (modal) {
      modal.style.display = 'block';
    }
  }

  closeForgetPasswordModal() {
    const modal = document.getElementById('exampleModal');
    if (modal) {
      modal.style.display = 'none';
    }
  }

  checkValidEmail(event:string){
    const value  = event;
    const pattern = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/;
    this.isValidEmail = pattern.test(value);
    return this.isValidEmail;
  }

  confirmEmailToSend(){
    debugger;
    if(this.checkValidEmail(this.resetPasswordEmail)){
      console.log(this.resetPasswordEmail);
      this.resetPassword.sendResetPasswordLink(this.resetPasswordEmail).subscribe({
        next:(res)=>{
          this.toast.success({
            detail:'Success',
            summary:'Reset Success',
            duration:3000
          });
          this.resetPasswordEmail='';
          const button = document.getElementById('closeBtn');
          button?.click();
        },
        error: (err)=>{
          this.toast.error({
            detail:'ERROR',
            summary:'SomeThing Went Wrong',
            duration:3000
          });
        }
      });
    }
  }
}
