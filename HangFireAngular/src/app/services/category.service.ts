import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  sharedData:any;
  constructor(private http:HttpClient) { }
  url="https://localhost:7109/api/Movies"
  GetAllCategories() {
    return this.http.get<any[]>('https://localhost:7109/api/Categories');
  }

  getMoviesByCategory(categoryId:number){

    return this.http.get<any[]>(`${this.url}/movieByCategory?categoryId=${categoryId}`);
  }

  setMovie(data:any){
this.sharedData=data;
  }

  getMovieData(){
    return {data:this.sharedData,flag:true};
  }
}
