import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { Cart, CartItem, CartTotal, ICart } from 'src/app/shared/models/cart';
import { Product } from 'src/app/shared/models/product';
import { environment } from 'src/assets/environment/environment';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor(private http: HttpClient) { }
  baseURL= environment.ApiUrl
  private cartSource = new BehaviorSubject<ICart | null>(null);
  cart = this.cartSource.asObservable();
  private cartSourceTotal = new BehaviorSubject<CartTotal| null>(null);
  cartTotal$ = this.cartSourceTotal.asObservable()

  GetCart(id : string) {
    return this.http.get<ICart>(this.baseURL+ "Cart/get-cart/"+ id).pipe(
      map((value:ICart) => {
        this.cartSource.next(value)
        this.CalcualteTotal()
        return value
      })
    )
  }
  SetCart(cart: ICart){
    return this.http.post<ICart>(this.baseURL+ "Cart/update-cart", cart).subscribe({
      next: (value: ICart) => {
        this.cartSource.next(value);
        this.CalcualteTotal()
      },
      error(err) {
          throw err;
      },
    })
  }
  GetCurrentValue() {
    return this.cartSource.value;
  } 
  AddItemToCart(product: Product, quantity:number = 1) {
    const itemToAdd: CartItem = this.MapProductToCartItem(product, quantity)
    let cart = this.GetCurrentValue() 
    if(cart?.id == null){

      cart = this.CreateCart()
    }
    cart.cartItems = this.AddOrUpdate(cart.cartItems, itemToAdd,quantity)
    this.SetCart(cart)
  }
  AddOrUpdate(cartItems: CartItem[], itemToAdd: CartItem, quantity: number): CartItem[] {
    const index = cartItems.findIndex(c => c.id === itemToAdd.id)
    if(index == -1) {
      itemToAdd.quantity = quantity;
      cartItems.push(itemToAdd)
    } else {
      cartItems[index].quantity += quantity
    }
    return cartItems
  }
  private CreateCart(): ICart{
    const cart = new Cart()
    localStorage.setItem('CartId', cart.id)
    return cart
  }
  private MapProductToCartItem(product: Product, quantity:number) : CartItem
  {
    return {
      id : product.id,
      category : product.categoryName,
      description : product.description,
      image : product.images[0].imageName,
      name: product.name,
      price: product.newPrice,
      quantity: quantity
    }
  }
  IncreaseCartItemQuantity(cartItem: CartItem){
    const cart = this.GetCurrentValue();
    const itemIndex = cart?.cartItems.findIndex(item => item.id === cartItem.id)
    if (itemIndex !== undefined && itemIndex !== -1) {
      cart!.cartItems[itemIndex].quantity++;
    }
    this.SetCart(cart!)
  }
  DecreaseCartItemQuantity(cartItem: CartItem){
    const cart = this.GetCurrentValue();
    const itemIndex = cart?.cartItems.findIndex(item => item.id === cartItem.id)

    if (itemIndex !== undefined && itemIndex !== -1) {
      if(cart!.cartItems[itemIndex].quantity > 1) {
        cart!.cartItems[itemIndex].quantity--;
        this.SetCart(cart!)
      } else {
        this.RemoveItemdFromCart(cartItem)
      }
    }
  }
  RemoveItemdFromCart(item: CartItem) {
    const cart = this.GetCurrentValue();
    if (cart!.cartItems.some(i => i.id === item.id)) {
      cart!.cartItems = cart!.cartItems.filter(i => i.id !== item.id);
      if (cart!.cartItems.length > 0) {
        this.SetCart(cart!);
      } else {
        this.DeleteCartItem(cart!);
      }
    }
  }
  
  DeleteCartItem(cart: ICart) {
    return this.http.delete(this.baseURL + "/Cart/delete-cart/" + cart.id)
      .subscribe({
        next: (value) => {
          this.cartSource.next(null);
          localStorage.removeItem('CartId');
        },
        error(err) {
          console.error(err);
        }
      });
  }
  

  CalcualteTotal(){
    const cart = this.GetCurrentValue()
    const shipping = 0;
    const subtotal = cart?.cartItems.reduce((a,c) => {
      return (c.price * c.quantity) + a
    }, 0)!
    const total = shipping + subtotal!
    this.cartSourceTotal.next({shipping, subtotal, total})
  }

}
