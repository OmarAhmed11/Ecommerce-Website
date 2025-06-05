import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PaginationComponent } from './component/pagination/pagination.component';
import { RouterModule } from '@angular/router';
import { NgxImageZoomModule } from 'ngx-image-zoom';
import { OrderTotalComponent } from './component/order-total/order-total.component';

@NgModule({
  declarations: [
    PaginationComponent,
    OrderTotalComponent
  ],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    RouterModule,
    NgxImageZoomModule
  ],
  exports:[
    PaginationModule,
    PaginationComponent,
    RouterModule,
    NgxImageZoomModule,
    OrderTotalComponent
  ]
})
export class SharedModule { }
