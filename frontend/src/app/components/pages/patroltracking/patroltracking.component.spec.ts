import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PatroltrackingComponent } from './patroltracking.component';

describe('PatroltrackingComponent', () => {
  let component: PatroltrackingComponent;
  let fixture: ComponentFixture<PatroltrackingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PatroltrackingComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PatroltrackingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
