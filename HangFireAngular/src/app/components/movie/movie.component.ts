import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/services/Auth.service';
import { MovieService } from 'src/app/services/movie.service';
import { UserDashbnoardComponent } from '../userDashbnoard/userDashbnoard.component';
import { CategoryService } from 'src/app/services/category.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-movie',
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.css'],
})
export class MovieComponent implements OnInit {
  finalResult: any[] = [];
  reloadflag:boolean=true;
  constructor(private movieService: MovieService,private router : ActivatedRoute, private auth: AuthService,private categoryService:CategoryService) {
    this.router.queryParams.subscribe(params=>{
      this.id = params['id']
      });
  }
  url: string = '';
  id:number=0
  ngOnInit() {
    debugger
   console.log('id',this.id)
    if(!this.id){
      this.GetAllMovies();
    }
    else{
      this.finalResult = this.categoryService.getMovieData().data
      if(!this.finalResult||this.finalResult.length == 0)
      this.categoryService.getMoviesByCategory(this.id).subscribe(x=>{
    this.finalResult = x
      })

    }
    //    if(this.categoryService.getMovieData()){
    //      if(this.categoryService.getMovieData().flag){
    //       console.log('this.categoryService.getMovieData().data',this.categoryService.getMovieData().data)
    //      }

    // }

    
  }
  GetAllMovies() {
    this.movieService.GetAllMovies().subscribe((data: any[]) => {
      debugger;
      this.finalResult = data;
      this.finalResult.forEach((el) => {
        var url = 'data:image/jpeg;base64,' + el.poster;
        this.url = url;
      });
    });
  }

  // openModal(){
  // const modal = document.getElementById('myModal');
  // if(modal){
  //   modal.style.display = 'block'
  // }
  // }

  // closeModal(){
  //   const modal =   document.getElementById('myModal');
  //   if(modal){
  //     modal.style.display = 'none'
  //   }
  //   }

  logout() {
    this.auth.signOut();
  }
}
