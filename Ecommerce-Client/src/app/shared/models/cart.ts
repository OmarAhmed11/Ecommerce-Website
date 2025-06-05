import { v4 as uuidv4 } from 'uuid';
export interface ICart {
    id: string
    cartItems: CartItem[]
}
  
export interface CartItem {
    id: number
    name: string
    image: string
    description: string
    quantity: number
    price: number
    category: string
}
export class Cart implements ICart {
    id = uuidv4();
    cartItems: CartItem[] = []
}
export interface CartTotal {
    shipping: number;
    subtotal: number;
    total: number;
}