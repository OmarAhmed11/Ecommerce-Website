import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/assets/environment/environment';
import { ActiveAccount } from '../models/active-account.model';
import { ResetPasswordModel } from '../models/reset-password.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl:string = environment.ApiUrl
  constructor(private http:HttpClient) { }

  Register(registerUser:any){
    return this.http.post<any>(this.baseUrl + "Account/Register", registerUser)
  }
  Login(loginForm:any){
    return this.http.post<any>(this.baseUrl + "Account/Login", loginForm)
  }
  ActivateAccount(activeAccount:ActiveAccount){
    return this.http.post<any>(this.baseUrl + "Account/Active-account", activeAccount)
  }
  SendEmailForForgetPassword(email:string){
    return this.http.get<any>(this.baseUrl + "Account/send-email-forget-password?email="+email)
  }
  ResetPassword(resetPasswordModel:ResetPasswordModel){
    return this.http.post<any>(this.baseUrl + "Account/reset-password", resetPasswordModel)
  }
}
