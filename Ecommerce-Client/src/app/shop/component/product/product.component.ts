import { Component, Input } from '@angular/core';
import { CartService } from 'src/app/cart/service/cart.service';
import { Image, Product } from 'src/app/shared/models/product';
import { environment } from 'src/assets/environment/environment';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class ProductComponent {

  @Input() Product!: Product
  public environment = environment

  constructor(private cartService:CartService) {

  }
  setCartVlue(){
    this.cartService.AddItemToCart(this.Product)
  }
}
