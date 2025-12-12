import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterModule } from '@angular/router';
import { filter } from 'rxjs/operators';

interface Breadcrumb {
  label: string;
  url: string;
}

@Component({
  selector: 'app-breadcrumb',
  standalone: true,
  imports: [RouterModule,CommonModule],
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.css']
})
export class BreadcrumbComponent implements OnInit {
  breadcrumbs: Breadcrumb[] = [];

  constructor(private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      const currentRoute = this.getDeepestChild(this.route);
      const data = currentRoute.snapshot.data;

      const trail = data['breadcrumbTrail'] || [];
      const currentLabel = data['breadcrumb'];

      // Avoid duplicate if current label is already in trail
      if (currentLabel && !trail.some((b: { label: any; }) => b.label === currentLabel)) {
        trail.push({ label: currentLabel, url: this.router.url });
      }

      this.breadcrumbs = trail;
    });
  }

  getDeepestChild(route: ActivatedRoute): ActivatedRoute {
    while (route.firstChild) {
      route = route.firstChild;
    }
    return route;
  }
}
