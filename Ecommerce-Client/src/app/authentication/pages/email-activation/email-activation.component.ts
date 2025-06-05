import { AfterViewInit, Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ActiveAccount } from '../../models/active-account.model';
import { AuthService } from '../../service/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-email-activation',
  templateUrl: './email-activation.component.html',
  styleUrls: ['./email-activation.component.scss']
})
export class EmailActivationComponent implements AfterViewInit {
  activeAccount = new ActiveAccount();
  constructor(private route:ActivatedRoute, private authService: AuthService, private toastr:ToastrService,
    private router:Router
  ) {
    
  }
  ngAfterViewInit(): void {
    this.route.queryParams.subscribe(result => {
          this.activeAccount.email = result['email'],
          this.activeAccount.token = result['code']
          console.log(this.activeAccount)
          this.authService.ActivateAccount(this.activeAccount).subscribe({
            next:(value) => {
             this.toastr.success("Email Activated Successfully", 'success'.toUpperCase())
             this.router.navigateByUrl('/Account/login')
            },
            error:(err) => {
              this.toastr.error("Wrong Email Or Token Is Expired", 'error'.toUpperCase())
            }
          })
      })
  }
}
