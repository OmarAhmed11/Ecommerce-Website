import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../service/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  formGroup!: FormGroup;

  constructor(private fb: FormBuilder, private authService:AuthService,
    private toastr:ToastrService, private router:Router) { }
  ngOnInit(): void {
    this.initForm()
  }
  initForm() {
    this.formGroup = this.fb.group({
      userName: ['', [Validators.required, Validators.minLength(6)]],
      email: ['', [Validators.required, Validators.email]],
      displayName: ['', [Validators.required]],
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

  get _username() {
    return this.formGroup.get('userName');
  }

  get _email() {
    return this.formGroup.get('email');
  }

  get _displayName() {
    return this.formGroup.get('displayName');
  }

  get _password() {
    return this.formGroup.get('password');
  }

  Submit(){
    if(this.formGroup.valid){
      this.authService.Register(this.formGroup.value).subscribe({
        next:(value => {
          console.log(value)
          this.toastr.success(value.message, 'success'.toUpperCase())
          this.router.navigateByUrl('/Account/login')
        }),
        error: (err) => {
          console.log(err.message)
          this.toastr.error(err.error.message, 'error'.toUpperCase())
        }
      })
    }
  }

}
