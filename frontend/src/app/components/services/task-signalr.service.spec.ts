import { TestBed } from '@angular/core/testing';

import { TaskSignalrService } from './task-signalr.service';

describe('TaskSignalrService', () => {
  let service: TaskSignalrService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TaskSignalrService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
