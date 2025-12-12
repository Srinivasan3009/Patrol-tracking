import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
interface Role{
  roleId: String,
  roleName: String,
  description: String

}
@Component({
  selector: 'app-role',
  standalone: true,
  imports: [CommonModule,FormsModule,ReactiveFormsModule],
  templateUrl: './role.component.html',
  styleUrl: './role.component.css'
})
export class RoleComponent implements OnInit{
 showRoleForm=false;
 editIndex: number = -1;
  roles: Role[]= [];
  roleid: String='';
  updateForm: FormGroup=this.fb.group({
      roleid: ['', Validators.required],     // already updated to string in your code
      rolename: ['', Validators.required],
      description: ['', Validators.required]
    });;
  RoleForm: FormGroup=this.fb.group(
    {rolename:['',Validators.required],
    description:['',Validators.required]}
  );
  ngOnInit(): void {
      this.load();
  }
  constructor(private fb: FormBuilder, private http: HttpClient){}
  load(){
     this.http.get<any>("http://localhost:5206/api/Role/getrole").subscribe({
      next: (data) => this.roles = data,
      error: (err) => console.error('Error fetching roles:', err)
    });
  }
  onSubmit(){
    const formData={
      id:"",
      roleId:"",
      roleName: this.RoleForm.value.rolename,
      description: this.RoleForm.value.description
    }
    this.http.post<any>('http://localhost:5206/api/Role/addrole',formData).subscribe({
      next:(res: any)=>{
        alert('Added successfully');
        console.log('API Success Response:', res);
        this.load();
      },
      error:(err)=>{
        alert("Failed to add Role"+err.error.message);
      }
    });
    this.RoleForm.reset();
    this.showRoleForm = false;
  }
  toggleRoleForm(){
    this.showRoleForm=!this.showRoleForm;
  }
  editRole(index: number): void {
    this.editIndex = index;
    const role = this.roles[index];

    this.updateForm.setValue({
      roleid: role.roleId,
      rolename: role.roleName,
      description: role.description
    });
  }

  CancelEdit(): void {
    this.editIndex = -1;
  }

  UpdateRole(): void {
    const updatePayload = {
      id:"",
        roleId:this.updateForm.value.roleid,
        roleName: this.updateForm.value.rolename,
        description: this.updateForm.value.description
    };
    
    this.http.put("http://localhost:5206/api/Role/updaterole", updatePayload).subscribe({
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
    removeRole(index: number): void {
    const roleToDelete = this.roles[index];
    if (!roleToDelete) {
      console.error('User not found at index', index);
      return;
    }

    this.roleid= roleToDelete.roleId

    this.http.request('DELETE', `http://localhost:5206/api/Role/deleterole/${this.roleid}`).subscribe({
      next: () => {
        alert('Delete successful');
        this.roles.splice(index, 1);
      },
      error: (err) => {
        alert('Delete failed: ' + (err.error?.message || err.message));
      }
    });
  }

}
