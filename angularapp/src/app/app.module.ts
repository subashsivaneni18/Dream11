import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdmineditinvestmentComponent } from './components/admineditinvestment/admineditinvestment.component';
import { AdminnavComponent } from './components/adminnav/adminnav.component';
import { AdminviewfeedbackComponent } from './components/adminviewfeedback/adminviewfeedback.component';

import { CreateinvestmentComponent } from './components/createinvestment/createinvestment.component';
import { ErrorComponent } from './components/error/error.component';
import { HomeComponent } from './components/home/home.component';
import { InvestmentformComponent } from './components/investmentform/investmentform.component';
import { LoginComponent } from './components/login/login.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { RequestedinvestmentComponent } from './components/requestedinvestment/requestedinvestment.component';
import { UseraddfeedbackComponent } from './components/useraddfeedback/useraddfeedback.component';
import { UserappliedinvestmentComponent } from './components/userappliedinvestment/userappliedinvestment.component';
import { UsernavComponent } from './components/usernav/usernav.component';
import { UserviewfeedbackComponent } from './components/userviewfeedback/userviewfeedback.component';
import { UserviewinvestmentComponent } from './components/userviewinvestment/userviewinvestment.component';
import { ViewinvestmentComponent } from './components/viewinvestment/viewinvestment.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ConfirmDeleteDialogComponent } from './confirm-delete-dialog/confirm-delete-dialog.component';
import { InvestmentListComponent } from './components/investment-list/investment-list.component';
import { UserProfileDailogComponent } from './user-profile-dailog/user-profile-dailog.component';
import { DeleteFeedbackDailogComponent } from './delete-feedback-dailog/delete-feedback-dailog.component';
import { DeleteAppliedInvestmentDailogComponent } from './delete-applied-investment-dailog/delete-applied-investment-dailog.component';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { LogoutDailogComponent } from './logout-dailog/logout-dailog.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { InvestmentInfoComponent } from './investment-info/investment-info.component';
import { UserviewinvestmentDailogComponent } from './userviewinvestment-dailog/userviewinvestment-dailog.component';
import { CibilComponent } from './components/cibil/cibil.component';
import { TrainingComponent } from './components/training/training.component'


@NgModule({
  declarations: [
    AppComponent,
    AdmineditinvestmentComponent,
    AdminnavComponent,
    AdminviewfeedbackComponent,
    CreateinvestmentComponent,
    ErrorComponent,
    HomeComponent,
    InvestmentformComponent,
    LoginComponent,
    NavbarComponent,
    RegistrationComponent,
    RequestedinvestmentComponent,
    UseraddfeedbackComponent,
    UserappliedinvestmentComponent,
    UsernavComponent,
    UserviewfeedbackComponent,
    UserviewinvestmentComponent,
    ViewinvestmentComponent,
    ConfirmDeleteDialogComponent,
    InvestmentListComponent,
    UserProfileDailogComponent,
    DeleteFeedbackDailogComponent,
    DeleteAppliedInvestmentDailogComponent,
    LogoutDailogComponent,
    InvestmentInfoComponent,
    UserviewinvestmentDailogComponent,
    CibilComponent,
    TrainingComponent

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatDialogModule,
    MatSnackBarModule,
    MatProgressSpinnerModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
