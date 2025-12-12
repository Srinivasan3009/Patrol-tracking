import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule} from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [FormsModule,HttpClientModule],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  username: string=''
  password: string=''
  email: string='';
  designation: string='';
  plain:string='';
  department: string='';
  mobilenumber: number| null=null;
  rolename: string='';
  constructor(private http: HttpClient,private route: Router) {}

  SignUp(){
    const formData={
      IdNo: "",
      Id: "",
      Name: this.username,
      Password: this.password,
      plain:this.password,
      email: this.email,
      designation: this.designation,
      department: this.department,
      mobilenumber: this.mobilenumber,
      rolename: this.rolename
    };
    this.http.post('http://localhost:5206/api/Auth/signup',formData).subscribe({
      next:(res)=>{
        alert('Signup successful');
        this.route.navigate(['/login']);
      },
      error: (err)=>{
        alert('Signup failed:'+err.error.message);
      }
    }
    );
  }
}
