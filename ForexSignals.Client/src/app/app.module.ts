import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

/*todd motto module for errors*/
import { NgxErrorsModule } from '@ultimate/ngxerrors';

/*bootstrap*/
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AlertModule } from 'ngx-bootstrap';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent, SignupComponent, HeaderComponent } from './components';


@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        SignupComponent,
        HeaderComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        NgxErrorsModule,
        NgbModule.forRoot(),
        AlertModule.forRoot(),
        FormsModule,
        ReactiveFormsModule,
        HttpModule
    ],
    providers: [HttpModule],
    bootstrap: [AppComponent],

})
export class AppModule { }
