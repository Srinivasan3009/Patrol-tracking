import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor() { }
  private username: string = '';
  private users:any=[];
  setUsername(name: string) {
    this.username = name;
  }

  getUsername(): string {
    return this.username;
  }
  private name:string='';
  setname(name: string) {
    this.name=name;
    localStorage.setItem('name',this.name);
  }
  getname(): string{
    return localStorage.getItem('name')||'';
  }
  setuser(users:any){
    this.users=users;
  }
  getuser(): any{
    return this.users
  }
}
