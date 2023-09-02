import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/services/Auth.service';
import { MovieService } from 'src/app/services/movie.service';
import { UserDashbnoardComponent } from '../../userDashbnoard/userDashbnoard.component';
import { CategoryService } from 'src/app/services/category.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders,HttpResponse  } from '@angular/common/http';

@Component({
  selector: 'app-movie',
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.css'],
})
export class MovieComponent implements OnInit {
  public createMovieForm = new FormGroup({
    title: new FormControl(),
    poster: new FormControl(),
    categoryName: new FormControl(),
    rate: new FormControl(),
    storeLine: new FormControl(),
    year: new FormControl(),
    categoryId: new FormControl()
  });
  finalResult: any[] = [];
  reloadflag:boolean=true;
  constructor(private movieService: MovieService,private http:HttpClient,private router : ActivatedRoute, private auth: AuthService,private categoryService:CategoryService) {
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
      console.log('daaaaa',data)
      this.finalResult.forEach((el) => {
        var url = 'data:image/jpeg;base64,' + el.poster;
        this.url = url;
      });
    });
  }


  addMovie(){
    debugger
    if(this.createMovieForm.valid){
      console.log('this.createCategoryForm.value',this.createMovieForm.value)
      this.convertImageToByteArray(this.createMovieForm.value.poster);
        this.movieService.addNewMovie(this.createMovieForm.value).subscribe((data: any[]) => {
        debugger;
        console.log('categoriesssssssssss',data)
        this.closeModal();
        // this.router.navigate(['movie']);   
        location.reload();
      });
    }
    else{
      this.validateAllFormFields(this.createMovieForm);
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
  convertImageToByteArray(image :any) {
    debugger
    const fileUrl = image; // Replace with the actual path to your image file
  
    // Set the HTTP request options
    const options = {
      responseType: 'arraybuffer' as 'json', // Set the response type to arraybuffer
      headers: new HttpHeaders({
        'Content-Type': 'application/octet-stream' // Set the content type to application/octet-stream
      })
    };
  
    // Make the HTTP GET request
    this.http.get<ArrayBuffer>(fileUrl, options)
      .subscribe((response: ArrayBuffer) => {
        const byteArray = new Uint8Array(response);
  
        // You can use the byteArray as needed
        console.log(byteArray);
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
