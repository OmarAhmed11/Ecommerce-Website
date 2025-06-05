import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})
export class PaginationComponent {
  @Input() PageSize: number = 0
  @Input() TotalCount : number = 0

  @Output() OnPageChange = new EventEmitter<number>()

  OnChangePage(event:any) {
    this.OnPageChange.emit(event)
  }
}
