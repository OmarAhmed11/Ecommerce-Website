import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Pagination } from '../shared/models/pagination';
import { ProductService } from './service/product.service';
import { Category } from '../shared/models/category';
import { CategoryService } from './service/category.service';
import { ProductPriceSortingEnum } from '../shared/models/product';
import { ProductParams } from '../shared/models/product-params';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  Products!: Pagination
  Categories: Category[] = []
  ProductParams = new ProductParams()
  TotalCount:number = 0;
  SortingSelectOptions: any[] = [
    {name:"Choose an Option", value: ProductPriceSortingEnum.None},
    {name:"Name", value: ProductPriceSortingEnum.Name},
    {name:"Price:min-max", value: ProductPriceSortingEnum.PriceAsc},
    {name:"Price:max-min", value: ProductPriceSortingEnum.PriceDsc},
  ]
  @ViewChild('Search') searchInput!: ElementRef;
  @ViewChild('SortSelected') SortSelected!: ElementRef;
  constructor(private productService:ProductService, private categoryService: CategoryService,
    private toastr:ToastrService
  ) {

  }

  ngOnInit(): void {
    this.ProductParams.Sort = ""
    this.ProductParams.PageNumber = 1
    this.ProductParams.PageSize = 6
    this.getAllCategories()
    this.getAllProduct()
  }
  getAllProduct(): void {
    this.productService.getProducts(this.ProductParams).subscribe({
      next: (value: Pagination) => {
        this.Products = value;
        this.TotalCount = value.totalCount
        this.toastr.success("Products Loaded","Success")
      }
    });
  }
  getAllCategories(){
    this.categoryService.getCategories().subscribe({
      next: (value) => {
        this.Categories = value
      }
    })
  }
  SearchByCategory(categoryId:number | null ) {
    if (categoryId) {
      this.ProductParams.CategoryId = categoryId;
    } else {
      this.ProductParams.CategoryId = 0;
    }
    this.getAllProduct();
  }
  SelectedOption(event:any){
    if(event.target.value != ProductPriceSortingEnum.None) {
    this.ProductParams.Sort = event.target.value;
    this.getAllProduct();
    } else {
      this.ProductParams.Sort = "";
      this.getAllProduct();
    }
  }
  OnSearch(SearchValue:string) {
    if(SearchValue) {
      this.ProductParams.Search = SearchValue
      this.getAllProduct()
    } else {
      this.ProductParams.Search = ""
      this.getAllProduct()
    }
  }

  ResetSearch(){
    this.ProductParams.Search = ""
    this.searchInput.nativeElement.value = '';
    this.getAllProduct()
  }
  PageChange(event:any) {
    this.ProductParams.PageNumber = event
    this.getAllProduct()
  }
  ResetAllFilter(){
    this.ProductParams = new ProductParams()
    this.searchInput.nativeElement.value = '';
    this.SortSelected.nativeElement.value = ProductPriceSortingEnum.None;
    this.getAllProduct()
  }
}
