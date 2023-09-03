import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { ToastrService } from 'ngx-toastr';
import { Password } from 'primeng/password';
import { ConfirmPasswordValidator } from 'src/app/helpers/confirm-password.validator';
import { ResetPassword } from 'src/app/models/reset-password';
import { ResetPasswordService } from 'src/app/services/reset-password.service';

@Component({
  selector: 'app-reset',
  templateUrl: './reset.component.html',
  styleUrls: ['./reset.component.css']
})
export class ResetComponent implements OnInit {

   resetPasswordForm !: FormGroup
  isText: boolean = false;
  Icon: string = 'fa-eye-slash';
  type: string = 'password';
  emailToReset: any;
  emailToken: any;
  resetPasswordObj = new ResetPassword;

  constructor(private fp:FormBuilder,private router:Router,private ac:ActivatedRoute,private res:ResetPasswordService,private toast:NgToastService) { }

  ngOnInit() {
    this.resetPasswordForm = this.fp.group({
      password:['', Validators.required],
      confirmPassword:['', Validators.required]
    },
    {
      validator:ConfirmPasswordValidator('password','confirmPassword')
    });
    this.ac.queryParams.subscribe(val=>{
      this.emailToReset = val['email'];
      let uriToken = val['code'];
      this.emailToken = uriToken.replace(/ /g,'+');
    });
  }

  showHiddenPassword() {
    this.isText = !this.isText;
    this.isText ? (this.Icon = 'fa-eye') : (this.Icon = 'fa-eye-slash');
    this.isText ? (this.type = 'text') : (this.type = 'password');
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


  reset(){
    if(this.resetPasswordForm.valid){
      this.resetPasswordObj.email = this.emailToReset;
      this.resetPasswordObj.newPassword = this.resetPasswordForm.value.password;
      this.resetPasswordObj.confirmPassword = this.resetPasswordForm.value.confirmPassword;
      this.resetPasswordObj.emailToken = this.emailToken;
      console.log('this.resetPasswordObj',this.resetPasswordObj);
      this.res.resetPassword(this.resetPasswordObj).subscribe({
        next:(x)=>{
          this.toast.success({detail:'SUCCESS',summary:'Password Reset Successfully',duration:5000});
        },
        error:err=>{
          this.toast.error({
            detail:'ERROR',
            summary:'SomeThing Go Wrong',
            duration:5000 
          });
          this.router.navigate(['/']);
        }
      });
    }
    else{
      this.validateAllFormFields(this.resetPasswordForm);
    }
  }

}
