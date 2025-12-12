import { Routes } from '@angular/router';
import { LoginComponent } from './components/pages/login/login.component';
import { OtpComponent } from './components/pages/otp/otp.component';
import { DashboardComponent } from './components/pages/dashboard/dashboard.component';
import { SignupComponent } from './components/pages/signup/signup.component';
import { NavigationComponent } from './components/pages/navigation/navigation.component';
import { ReportsComponent } from './components/pages/reports/reports.component';
import { EventsComponent } from './components/pages/events/events.component';
import { InsightsComponent } from './components/pages/insights/insights.component';
import { UserManagementComponent } from './components/pages/usermanagement/usermanagement.component';
import { RoleComponent } from './components/pages/role/role.component';
import { PatroltrackingComponent } from './components/pages/patroltracking/patroltracking.component';

export const routes: Routes = [
    {path:"",
        redirectTo:"/login",
        pathMatch: "full",
    },
    {path:"login",
        component:LoginComponent
    },
       {path:"otp",
        component:OtpComponent
    },
    
    {path:"signup",
        component:SignupComponent
    },
    {path:"navigation",
        component:NavigationComponent,
        children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'reports', component: ReportsComponent },
      { path: 'events', component: EventsComponent },
      { path: 'insights', component: InsightsComponent },
      { path: 'patroltracking', component: PatroltrackingComponent },
      { path: 'usermanagement', component: UserManagementComponent, data: { breadcrumb: 'User Management' }},
      { path: 'role', component: RoleComponent,data: { breadcrumb: 'Role',breadcrumbTrail: [{ label: 'User Management', url: '/navigation/usermanagement' },
    ]
  }}
    ]
    },
];
