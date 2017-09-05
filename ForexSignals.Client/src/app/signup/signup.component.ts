import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { UserModel } from '../../models/user.model';
import { Http } from '@angular/http';

@Component({
    selector: 'app-signup',
    templateUrl: './signup.component.html',
    styleUrls: ['./signup.component.scss']
})
export class SignupComponent implements OnInit {

    public user: UserModel;
    public signupForm: FormGroup;
    public firstName: FormControl;
    public lastName: FormControl;
    public email: FormControl;
    public username: FormControl;
    public password: FormControl;
    public termsAgreement: FormControl;
    public errorCode: number;

    constructor(
        private fb: FormBuilder,
        private http: Http
        //private authService: AuthService,
        //private store: Store<AppState>
    ) {
        this.user = new UserModel();

    }

    ngOnInit() {
        this.createFormControls();
        this.createForm();
        this.username.valueChanges
            .filter(val => val.length >= 3)
            .debounceTime(500)
            .map(val => this.http.get('http://myapi.com/${val}'))
            .subscribe();
    }

    private validateEmail(c: FormControl) {
        let emailRegexp = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/

        return emailRegexp.test(c.value) ? null : {
            validateEmail: {
                valid: false
            }
        };
    }

    private createFormControls() {
        this.firstName = new FormControl('', Validators.required),
            this.lastName = new FormControl('', Validators.required),
            this.email = new FormControl('', [Validators.required, this.validateEmail]),
            this.username = new FormControl('', Validators.required),
            this.password = new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]),
            this.termsAgreement = new FormControl('', Validators.required)
    }

    private createForm() {
        this.signupForm = new FormGroup({
            firstName: this.firstName,
            lastName: this.lastName,
            email: this.email,
            username: this.username,
            password: this.password,
            termsAgreement: this.termsAgreement
        });
    }

    private passwordMatch(pass, passConf) {
        if (pass === passConf) {
            this.errorCode = 0;
            return true;
        } else {
            this.errorCode = 2;
            return false; 
        }
    }


    public signup(value) {
        if (!this.passwordMatch(value.password, value.passwordConfirm)) {
            return false;
        } 
        
    }
}
