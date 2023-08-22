import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http:HttpClient) { }
  url="https://localhost:7109/api/Movies"
  GetAllCategories() {
    return this.http.get<any[]>('https://localhost:7109/api/Categories');
  }

  getMoviesByCategory(categoryId:number){
    debugger
    return this.http.post<any[]>(`${this.url}/movieByCategory`,categoryId);
  }
}
