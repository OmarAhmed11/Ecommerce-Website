import { Product } from "./product"

export interface Pagination {
    pageNumber: number
    pageSize: number
    totalCount: number
    data: Product[]
}