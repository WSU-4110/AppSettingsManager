import { TestBed } from '@angular/core/testing';

import { HttpHeadersInterceptor } from './http-headers.interceptor';

describe('HttpHeadersInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      HttpHeadersInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: HttpHeadersInterceptor = TestBed.inject(HttpHeadersInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
