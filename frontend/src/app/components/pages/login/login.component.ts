import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from '../../services/login/login.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule,HttpClientModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit{
    username: string="";
    password: string="";
    message:string="";
    
    constructor(private http: HttpClient,private router: Router,private loginService: LoginService,private authService: AuthService) {}
    ngOnInit(): void {
    }
    OnLogin(){
      const body={
      username:this.username,
      password:this.password,
      };
      this.http.post<any>('http://localhost:5206/api/Auth/login', body).subscribe({
        next: (response)=>{
          this.message =response.message;
          alert(response.message);
          this.loginService.setUsername(this.username);
          this.loginService.setname(response.name);
          this.router.navigate(['/otp']);
        },
        error: (error)=>{
          this.message = error?.error?.message || 'Error! Login failed';
        }
      });
    }
    Signup(){
      this.router.navigate(['/signup']);
    }
}

