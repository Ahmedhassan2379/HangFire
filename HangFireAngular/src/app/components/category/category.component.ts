import { Component, OnInit } from '@angular/core';
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

  getMoviesByCategory(event:any){
    debugger
    console.log('event',event)
    var target = event.target || event.srcElement || event.currentTarget;
        var idAttr = target.attributes.id;
        var value = idAttr.nodeValue;
        console.log('value',value)
    this.categoryService.getMoviesByCategory(value).subscribe({
      next: (res) => {
        console.log('resss',res);
       // this.router.navigate(['movie'])
      },
      error:(err)=>{
        this.toast.error({detail:"ERROR",summary:err.message,duration:5000});
      },
    })
  }

  logout() {
    this.auth.signOut();
  }
}
