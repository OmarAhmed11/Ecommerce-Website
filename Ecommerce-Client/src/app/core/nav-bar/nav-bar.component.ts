import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CartService } from 'src/app/cart/service/cart.service';
import { ICart } from 'src/app/shared/models/cart';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {
  Count!:Observable<ICart | null>;
  constructor( private cartService:CartService ) {
    
  }
  ngOnInit(): void {
    this.getCartItems()
  }
  getCartItems() {
    const CartId = localStorage.getItem('CartId') || ''
    if(CartId){

      this.cartService.GetCart(CartId).subscribe({
        next: value => {
          console.log(value)
          this.Count = this.cartService.cart
        },
        error(err) {
            console.log(err)
        },
      })
    }
  }
}
