import { TestBed } from '@angular/core/testing';

import { AdresCompleteService } from './adres-complete.service';

describe('AdresCompleteService', () => {
  let service: AdresCompleteService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdresCompleteService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
