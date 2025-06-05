import { Component, OnInit } from '@angular/core';
import { ResetPasswordModel } from '../../models/reset-password.model';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../service/auth.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  formGroup!: FormGroup;
  resetPasswordModel = new ResetPasswordModel();
  constructor(private route: ActivatedRoute, private authService: AuthService, private toastr: ToastrService,
    private fb: FormBuilder, private router:Router
  ) {
  }
  ngOnInit(): void {
    this.initForm()
    this.route.queryParams.subscribe((param) => {
      this.resetPasswordModel.email = param['email'];
      this.resetPasswordModel.token = param['code'];
    });
  }

  initForm() {
    this.formGroup = this.fb.group({
      password: [
        '',
        [
          Validators.required,
          Validators.pattern(/^(?=.*[0-9])(?=.*[#\$@!\.\-\_])[A-Za-z\d#\$@!\.\-\_]{8,}$/)
        ]
      ],
      confirmPassword: [
        '',
        [
          Validators.required,
          Validators.pattern(/^(?=.*[0-9])(?=.*[#\$@!\.\-\_])[A-Za-z\d#\$@!\.\-\_]{8,}$/)
        ]
      ]
    }, { validators: this.passwordsMatchValidator });
  }
  get _password() {
    return this.formGroup.get('password')
  }
  get _confirmPassword() {
    return this.formGroup.get('confirmPassword')
  }

  passwordsMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;

    if (password !== confirmPassword) {
      return { passwordsMismatch: true };
    }
    return null;
  }
  Submit() {
    if(this.formGroup.valid){
      this.resetPasswordModel.password = this._password?.value
      this.authService.ResetPassword(this.resetPasswordModel).subscribe({
        next: (value) => {
          this.toastr.success(value.message, 'success'.toUpperCase())
          this.router.navigateByUrl('/Account/login')
        },
        error: (err) => {
          this.toastr.error(err.message, 'error'.toUpperCase())
        }
      })
    }
  }

}
