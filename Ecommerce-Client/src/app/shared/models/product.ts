export interface Product {
    id: number
    name: string
    description: string
    newPrice: number
    oldPrice: number
    images: Image[]
    categoryName: string
}
export interface Image {
    imageName: string
    productId: number
}
export enum ProductPriceSortingEnum {
    None = "None",
    Name = "Name",
    PriceAsc = "PriceAsc",
    PriceDsc = "PriceDsc"
}