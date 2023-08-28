import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { AuthService } from 'src/app/services/Auth.service';
import { CategoryService } from 'src/app/services/category.service';
@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {

  public createCategoryForm = new FormGroup({
    name: new FormControl()
  });
  finalResult: any[] = [];
  constructor(private categoryService: CategoryService,private router:Router, private auth: AuthService,private toast:NgToastService) {}
  url: string = '';
  ngOnInit() {
    this.GetCategories();
  }
  GetCategories() {
    this.categoryService.GetAllCategories().subscribe((data: any[]) => {
      debugger;
      console.log('categories',data)
      this.finalResult = data;
    });
  }

  addCategory(){
    debugger
    if(this.createCategoryForm.valid){
      console.log('this.createCategoryForm.value',this.createCategoryForm.value)
      this.categoryService.addNewCategory(this.createCategoryForm.value).subscribe((data: any[]) => {
        debugger;
        console.log('categoriesssssssssss',data)
        this.closeModal();
        this.router.navigate(['movie']);   
      });
    }
    else{
      this.validateAllFormFields(this.createCategoryForm);
    }
 
  }

final:any
  getMoviesByCategory(event:any){
    debugger
    console.log('event',event)
    var target = event.target || event.srcElement || event.currentTarget;
        var idAttr = target.attributes.id;
        var value = idAttr.nodeValue;
        console.log('value',value)
    this.categoryService.getMoviesByCategory(value).subscribe({
      next: (res) => {
        this.final = res
        console.log('resss',res);
        this.router.navigate(['movie'],{queryParams:{id:value}})
    //  let url=   this.router.createUrlTree(['movie'],{queryParams:{id:value}}).toString();
    //  window.open(url,'_self')
      },
      error:(err)=>{
        this.toast.error({detail:"ERROR",summary:err.message,duration:5000});
      },
    })
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

  openModal(){
    const modal = document.getElementById('exampleModal');
    if(modal){
      modal.style.display = 'block'
    }
    }
  
    closeModal(){
      const modal =   document.getElementById('exampleModal');
      if(modal){
        modal.style.display = 'none'
      }
      }

  logout() {
    this.auth.signOut();
  }
}
