import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { BreadcrumbComponent } from '../breadcrumb/breadcrumb.component';
import { LoginService } from '../../services/login/login.service';
import { HttpHeaders } from '@angular/common/http';
@Component({
  selector: 'app-navigation',
  standalone: true,
  imports: [RouterModule,CommonModule,BreadcrumbComponent],
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.css'
})
export class NavigationComponent implements OnInit {
sidebarExpanded = false;
username: string='';
name:string='';
constructor(private router: Router,@Inject(PLATFORM_ID) private platformId: Object,private loginService: LoginService) {
  
}

  toggleSidebar() {
    this.sidebarExpanded = !this.sidebarExpanded;
  }
  
   ngOnInit() {
    this.name=this.loginService.getname();
}
showProfile = false;

toggleProfile() {
  this.showProfile = !this.showProfile;
}

logout() {
  localStorage.removeItem('username');
  alert('Logged out!');
  this.router.navigate(['/login']);
}

}
