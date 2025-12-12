// role.service.ts
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  constructor(private http: HttpClient) {}

  getRoles(): Observable<any> {
    return this.http.get('http://localhost:5206/api/Role/getrole');
  }
}
