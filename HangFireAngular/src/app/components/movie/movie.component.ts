import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/services/Auth.service';
import { MovieService } from 'src/app/services/movie.service';
import { UserDashbnoardComponent } from '../userDashbnoard/userDashbnoard.component';

@Component({
  selector: 'app-movie',
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.css'],
})
export class MovieComponent implements OnInit {
  finalResult: any[] = [];
  constructor(private movieService: MovieService, private auth: AuthService) {}
  url: string = '';
  ngOnInit() {
    this.GetAllMovies();
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

  logout() {
    this.auth.signOut();
  }
}
