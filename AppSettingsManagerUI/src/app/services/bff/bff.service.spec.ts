import { TestBed } from '@angular/core/testing';

import { BffService } from './bff.service';

describe('BffService', () => {
  let service: BffService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BffService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
