import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  public form: FormGroup;
  public user: any = {};
  public loginName: string;
  public loginPassword: string;

  constructor(
    private fb: FormBuilder
    //private authService: AuthService,
    //private store: Store<AppState>
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      loginName: [null, [Validators.required]],
      loginPassword: [null, [Validators.required], [Validators.minLength(6)], [Validators.maxLength(20)]]
    });
  }



    public login() {
        //this.authService.login(this.user.username, this.user.password)
        //    .subscribe(
        //    data => {
        //        //this.router.navigate([this.returnUrl]);
        //    },
        //    error => {
        //        //this.alertService.error(error);

        //    });

    }
}
