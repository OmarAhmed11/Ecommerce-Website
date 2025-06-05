import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Category } from 'src/app/shared/models/category';
import { environment } from 'src/assets/environment/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  baseURL= environment.ApiUrl

  constructor(private http: HttpClient) {
      
  }

  getCategories(){
    return this.http.get<Category[]>(this.baseURL + "Categories/GetAll")
  }
  
}
