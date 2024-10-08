import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class MovieService {

constructor(private http:HttpClient) { }
url="https://localhost:7109/api/Movies"
GetAllMovies() {
  return this.http.get<any[]>(this.url);
}

getMoviesByCategory(){
  return this.http.get<any[]>(this.url);
}

addNewMovie(movieObj: any){
  debugger
  return this.http.post<any>(`${this.url}/CreateMovie?movieObj=`, movieObj)
}

}