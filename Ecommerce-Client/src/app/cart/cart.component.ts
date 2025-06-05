import { Component, OnInit } from '@angular/core';
import { CartService } from './service/cart.service';
import { CartItem, ICart } from '../shared/models/cart';
import { environment } from 'src/assets/environment/environment';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {
  cart!:ICart | null
  public environment = environment
  constructor( private cartService:CartService) {

  }
  ngOnInit(): void {
    this.getCart()
  }
  getCart(){
    this.cartService.cart.subscribe({
      next: (value: ICart | null) => {
        this.cart = value
      },
      error(err) {
          throw err;
      },
    })
  }
  IncreaseQuantity(item: CartItem){
    this.cartService.IncreaseCartItemQuantity(item)
  }
  DecreaseQuantity(item: CartItem){
    this.cartService.DecreaseCartItemQuantity(item)
  }
  RemoveCartItem(item: CartItem){
    this.cartService.RemoveItemdFromCart(item)
  }
}
