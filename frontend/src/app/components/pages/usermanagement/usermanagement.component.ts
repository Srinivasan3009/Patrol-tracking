import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { RoleService } from '../../services/role.service';
import { LoginService } from '../../services/login/login.service';

interface User {
  idNo: string;           
  name: string;
  password: string;
  plain:string;
}

@Component({
  selector: 'app-usermanagement',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, HttpClientModule, FormsModule, RouterModule],
  templateUrl: './usermanagement.component.html',
  styleUrls: ['./usermanagement.component.css']
})
export class UserManagementComponent implements OnInit {
  showLoginForm = false;
  editIndex: number = -1;
  users: User[] = [];
  roles: any[]=[];
  loginForm: FormGroup;
  updateForm: FormGroup;
  idNo:string="";
  constructor(private fb: FormBuilder, private http: HttpClient,private roleService: RoleService,private loginServices:LoginService) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      email:[''],
      password: ['', Validators.required],
      designation:[''],
      department:[''],
      mobilenumber:[null],
      rolename:['']

    });

    this.updateForm = this.fb.group({
      userid: ['', Validators.required],     
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.load();
    this.loadRoles();
  }


  loadRoles() {
    this.roleService.getRoles().subscribe({
      next: (res: any) => {
        this.roles = res;
      },
      error: (err) => {
        console.error('Error loading roles:', err);
      }
    });
  }
  load(): void {
    this.http.get<any>("http://localhost:5206/api/User/getuser").subscribe({
      next: (data) => this.users = data,
      
      error: (err) => console.error('Error fetching users:', err)
    });
    this.loginServices.setuser(this.users);
  }

  toggleLoginForm(): void {
    this.showLoginForm = !this.showLoginForm;
  }

  onSubmit(): void {
    const formData = {
      IdNo: "",
      Id: "",
      Name: this.loginForm.value.username,
      Password: this.loginForm.value.password,
      email: this.loginForm.value.email,
      designation: this.loginForm.value.designation,
      department: this.loginForm.value.department,
      mobilenumber: this.loginForm.value.mobilenumber,
      rolename: this.loginForm.value.rolename,
      plain:this.loginForm.value.password
    };

    this.http.post('http://localhost:5206/api/Auth/signup', formData).subscribe({
      next: () => {
        alert('Signup successful');
        this.load();
      },
      error: (err) => {
        alert('Signup failed: ' + (err.error?.message || err.message));
      }
    });

    this.loginForm.reset();
    this.showLoginForm = false;
  }

  removeUser(index: number): void {
    const userToDelete = this.users[index];
    if (!userToDelete) {
      console.error('User not found at index', index);
      return;
    }
    
    this.idNo=userToDelete.idNo;
    this.http.request('DELETE', `http://localhost:5206/api/User/deleteuser/${this.idNo}`).subscribe({
      next: () => {
        alert('Delete successful');
        this.users.splice(index, 1);
      },
      error: (err) => {
        alert('Delete failed: ' + (err.error?.message || err.message));
      }
    });
  }

  editUser(index: number): void {
    this.editIndex = index;
    const user = this.users[index];

    this.updateForm.setValue({
      userid: user.idNo,
      username: user.name,
      password: user.password
    });
  }

  CancelEdit(): void {
    this.editIndex = -1;
  }

  UpdateUser(): void {
    const updatePayload = {
        idNo: this.updateForm.value.userid,
        Name: this.updateForm.value.username,
        Password: this.updateForm.value.password,
        plain:this.updateForm.value.password,
    };
    
    this.http.put("http://localhost:5206/api/User/updateuser", updatePayload).subscribe({
      next: () => {
        alert('User update successful');
        this.load();
        this.editIndex = -1;
      },
      error: (err) => {
        console.error(err);
        alert('Failed to update user: ' + (err.error?.message || err.message));
      }
    });
  }
}
