import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  RequestCount = 0;
  constructor(private _LoadingService:NgxSpinnerService) { }
  loading(){
    this.RequestCount++;
    this._LoadingService.show(undefined, {
      bdColor : "rgba(0, 0, 0, 0.8)",
      size : "large",
      color : "#fff",
      type : "square-jelly-box",
      fullScreen: true
    })
  }
  hideloader(){
    this.RequestCount--;
    if(this.RequestCount <= 0 ) {
      this.RequestCount = 0;
      this._LoadingService.hide()
    }
  }
}
