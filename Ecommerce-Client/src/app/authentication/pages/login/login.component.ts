import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../service/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  formGroup!: FormGroup;
  emailModel: string = ''
  constructor(private fb: FormBuilder, private authService: AuthService,
    private toastr: ToastrService, private router:Router) { }
  ngOnInit(): void {
    this.initForm()
  }

  initForm() {
    this.formGroup = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: [
        '',
        [
          Validators.required,
          Validators.pattern(
            /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$/
          ),
        ],
      ],
    });
  }

  get _email() {
    return this.formGroup.get('email');
  }

  get _password() {
    return this.formGroup.get('password');
  }
  Submit() {
    if (this.formGroup.valid) {

      this.authService.Login(this.formGroup.value).subscribe({
        next: (value => {
          console.log(value)
          this.toastr.success(value.message, 'success'.toUpperCase())
          this.router.navigateByUrl('/')
        }),
        error: (err) => {
          console.log(err.message)
          this.toastr.error(err.error.message, 'error'.toUpperCase())
        }
      })
    }
  }
  SendEmailForForgetPassword() {

    this.authService.SendEmailForForgetPassword(this.emailModel).subscribe({
      next: (value => {
        console.log(value)
        this.toastr.success(value.message, 'success'.toUpperCase())
      }),
      error: (err) => {
        console.log(err.message)
        this.toastr.error(err.error.message, 'error'.toUpperCase())
      }
    })
  }

}
