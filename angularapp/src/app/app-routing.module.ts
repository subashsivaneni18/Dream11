import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
 
// Public Components
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';
 
// User Components
import { UsernavComponent } from './components/usernav/usernav.component';
import { UserviewinvestmentComponent } from './components/userviewinvestment/userviewinvestment.component';
import { UserappliedinvestmentComponent } from './components/userappliedinvestment/userappliedinvestment.component';
import { UseraddfeedbackComponent } from './components/useraddfeedback/useraddfeedback.component';
import { UserviewfeedbackComponent } from './components/userviewfeedback/userviewfeedback.component';
import { ViewinvestmentComponent } from './components/viewinvestment/viewinvestment.component';
 
// Admin Components
import { AdminnavComponent } from './components/adminnav/adminnav.component';
import { CreateinvestmentComponent } from './components/createinvestment/createinvestment.component';
import { InvestmentformComponent } from './components/investmentform/investmentform.component';
import { AdminviewfeedbackComponent } from './components/adminviewfeedback/adminviewfeedback.component';
import { RequestedinvestmentComponent } from './components/requestedinvestment/requestedinvestment.component';
 
// Auth Guard
import { AuthGuard } from './components/authguard/auth.guard';
import { AdmineditinvestmentComponent } from './components/admineditinvestment/admineditinvestment.component';
import { TrainingComponent } from './components/training/training.component';
import { CibilComponent } from './components/cibil/cibil.component';
 
const routes: Routes = [
  // Public Routes
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'registration', component: RegistrationComponent },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  {path:'training',component:TrainingComponent},
    {path:'cibil',component:CibilComponent},
 
  // User Routes
  {
    path: 'usernav',
    component: UsernavComponent,
    canActivate: [AuthGuard],
    data: { role: ['User'] }
  },
  {
    path: 'userviewinvestment',
    component: UserviewinvestmentComponent,
    canActivate: [AuthGuard],
    data: { role: ['User'] }
  },
  {
    path: 'userappliedinvestment',
    component: UserappliedinvestmentComponent,
    canActivate: [AuthGuard],
    data: { role: ['User'] }
  },
  {
    path: 'useraddfeedback',
    component: UseraddfeedbackComponent,
    canActivate: [AuthGuard],
    data: { role: ['User'] }
  },
  {path:'admineditinvestment/:id' , component:AdmineditinvestmentComponent},
  {path:'investmentform/:id' , component:InvestmentformComponent},
  {
    path: 'userviewfeedback',
    component: UserviewfeedbackComponent,
    canActivate: [AuthGuard],
    data: { role: ['User'] }
  },
  {
    path: 'viewinvestment',
    component: ViewinvestmentComponent,
    canActivate: [AuthGuard],
    data: { role: ['User'] }
  },
 
  // Admin Routes
  {
    path: 'adminnav',
    component: AdminnavComponent,
    canActivate: [AuthGuard],
    data: { role: ['Admin'] }
  },
  {
    path: 'createinvestment',
    component: CreateinvestmentComponent,
    canActivate: [AuthGuard],
    data: { role: ['Admin'] }
  },
  {
    path: 'investmentform/:id',
    component: InvestmentformComponent,
    canActivate: [AuthGuard],
    data: { role: ['Admin'] }
  },
  {
    path: 'adminviewfeedback',
    component: AdminviewfeedbackComponent,
    canActivate: [AuthGuard],
    data: { role: ['Admin'] }
  },
  {
    path: 'requestedinvestment',
    component: RequestedinvestmentComponent,
    canActivate: [AuthGuard],
    data: { role: ['Admin'] }
  }
];
 
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}