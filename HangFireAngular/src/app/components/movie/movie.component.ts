import { Component, OnInit } from '@angular/core';
import { MovieService } from 'src/app/services/movie.service';

@Component({
  selector: 'app-movie',
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.css']
})
export class MovieComponent implements OnInit {
finalResult:any[]=[]
  constructor(private movieservice:MovieService) { }
url:string=""
  ngOnInit() {
    this.GetAllMovies()
  }
GetAllMovies(){
  this.movieservice.GetAllMovies().subscribe((data:any[])=>{
    debugger
  this.finalResult=data
  this.finalResult.forEach(el => {

var url = 'data:image/jpeg;base64,' + el.poster;
this.url = url 
  });
  
  })
}

}
