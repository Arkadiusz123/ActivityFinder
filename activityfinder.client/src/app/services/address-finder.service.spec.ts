import { TestBed } from '@angular/core/testing';

import { AddressFinderService } from './address-finder.service';

describe('AddressFinderService', () => {
  let service: AddressFinderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AddressFinderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
