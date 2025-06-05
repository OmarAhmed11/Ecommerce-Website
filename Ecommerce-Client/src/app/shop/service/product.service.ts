import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from 'src/app/shared/models/pagination';
import { Product } from 'src/app/shared/models/product';
import { ProductParams } from 'src/app/shared/models/product-params';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  baseURL= 'https://localhost:44307/api/'

  constructor(private http: HttpClient) {
      
  }

  getProducts(productParams?: ProductParams | null){
    let params = new HttpParams();

    if (productParams) {
      // Object.keys(productParams).forEach(key => {
      //   const value = productParams[key as keyof ProductParams];
      //   if (value !== null && value !== undefined) {
      //     params = params.set(key, value.toString());
      //   }
      // });
      if(productParams.CategoryId){
        params = params.append("CategoryId", productParams.CategoryId)
      }
      if(productParams.Sort){
        params = params.append("Sort", productParams.Sort)
      }
      if(productParams.Search){
        params = params.append("Search", productParams.Search)
      }
      if(productParams.PageSize){
        params = params.append("PageSize", productParams.PageSize)
      }
      if(productParams.PageNumber){
        params = params.append("PageNumber", productParams.PageNumber)
      }
    }
    return this.http.get<Pagination>(this.baseURL + "Products/GetAll" , { params })
  }
    getProductById(productId:number){
      return this.http.get<Product>(this.baseURL + "Products/GetById/"+productId)
    }
  
}
