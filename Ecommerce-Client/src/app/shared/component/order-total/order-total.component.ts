import { Component, OnInit } from '@angular/core';
import { CartService } from 'src/app/cart/service/cart.service';
import { CartTotal } from '../../models/cart';

@Component({
  selector: 'app-order-total',
  templateUrl: './order-total.component.html',
  styleUrls: ['./order-total.component.scss']
})
export class OrderTotalComponent implements OnInit {
  CartTotal!:CartTotal | null
  constructor(private cartService: CartService){

  }
  ngOnInit(): void {
    this.getCartTotal()
  }
  getCartTotal(){
    this.cartService.cartTotal$.subscribe({
      next:(value) => {
        this.CartTotal = value
      },
      error(err) {
          console.log(err)
      },
    })
  }
}
