import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { Inject, PLATFORM_ID } from '@angular/core';
import { LoginService } from '../../services/login/login.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-otp',
  standalone: true,
  imports: [FormsModule,HttpClientModule],
  templateUrl: './otp.component.html',
  styleUrl: './otp.component.css'
})
export class OtpComponent {
  otp:String=''
  message: String=''
  username: string=''
    constructor(@Inject(PLATFORM_ID) private platformId: Object,private http: HttpClient,private router: Router,private loginService: LoginService, private authService: AuthService) {
    }
    ngOnInit() {
  this.username=this.loginService.getUsername();
  console.log('Username:', this.username);
  if (isPlatformBrowser(this.platformId)) {
  localStorage.setItem('username', this.username);
  
}

}
  VerifyOtp(){
    
    const body={
      name:this.username,
      otp:this.otp,
      };
      this.http.post<any>('http://localhost:5206/api/Auth/verify-otp', body).subscribe({
        next: (response)=>{
          console.log("Otp verified");
          localStorage.setItem('token', response.token);
          this.message =response.message || 'Otp verified';
          alert(response.message);
          this.router.navigate(['/navigation']);


        },
        error: (error)=>{
          this.message==error.error.message || 'Error! Otp verification falied';
        }
      });
  }
}