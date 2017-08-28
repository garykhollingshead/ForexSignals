import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { UserModel } from '../../models/user.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  public user: UserModel;
  public loginForm: FormGroup;
  public username: FormControl;
  public password: FormControl;

  constructor(
      private fb: FormBuilder
      //private authService: AuthService,
      //private store: Store<AppState>
  ) {
      this.user = new UserModel;

  }

  ngOnInit() {
      this.createFormControls();
      this.createForm();

      //this.form = this.fb.group({
    //  loginName: ['', [Validators.required]],
    //  loginPassword: ['', [Validators.required], [Validators.minLength(6)], [Validators.maxLength(20)]]
    //});
  }

    private createFormControls() {
            this.username = new FormControl('', Validators.required),
            this.password = new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(20)])
    }

    private createForm() {
        this.loginForm = new FormGroup({
                username: this.username,
                password: this.password
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
