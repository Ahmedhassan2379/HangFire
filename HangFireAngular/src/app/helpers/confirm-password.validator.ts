import { FormGroup } from "@angular/forms";

export function ConfirmPasswordValidator(controlName:string,matchControlName:string){
return (formGroup: FormGroup)=>{
    const passwordControl = formGroup.controls[controlName];
    const matchPasswordControl = formGroup.controls[matchControlName];
    if(matchPasswordControl.errors && matchPasswordControl.errors['confirmPasswordValidator'] ){
      return;
    }
    else if(passwordControl.value != matchPasswordControl.value){
      matchPasswordControl.setErrors({confirmPasswordValidator : true})
    }
    else{
      matchPasswordControl.setErrors(null);
    }

  }
}