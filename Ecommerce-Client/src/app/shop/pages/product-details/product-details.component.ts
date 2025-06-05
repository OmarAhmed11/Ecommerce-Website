import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../service/product.service';
import { Product } from 'src/app/shared/models/product';
import { ActivatedRoute } from '@angular/router';
import { environment } from 'src/assets/environment/environment';
import { ToastrService } from 'ngx-toastr';
import { CartService } from 'src/app/cart/service/cart.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {

  Product!:Product
  productId:number = 0
  public environment = environment
  MainImage:string = ''
  Quantity:number = 1;
  constructor(private productService:ProductService, private route:ActivatedRoute, private toastr:ToastrService,
    private cartService: CartService
  ){
    this.productId = parseInt(this.route.snapshot.paramMap.get('id')!)
  }
  ngOnInit(): void {
    this.getProductDetails()
  }
  getProductDetails(){
    this.productService.getProductById(this.productId).subscribe({
      next: (value:Product) => {
        this.Product = value
        this.MainImage =  this.Product.images[0].imageName
      }
    })
  }
  ReplaceImage(src:string){
    this.MainImage = src
  }
  IncreaseProducytQuantity(){
    this.Quantity++
    this.toastr.success("item added successfully", "Success")
  }
  DecreaseProducytQuantity(){
    if(this.Quantity > 1 ){
      this.Quantity--
    }
    else {
      this.toastr.warning("you can not decrease less than 1", "Warning")
    }
  }
  AddToCart(){
    this.cartService.AddItemToCart(this.Product, this.Quantity)
  }
  CalculateDiscount(oldPrice: number, newPrice: number): number {
    return parseFloat(
        Math.round(((oldPrice - newPrice) / oldPrice) * 100).toFixed(1)
    );
}

}
