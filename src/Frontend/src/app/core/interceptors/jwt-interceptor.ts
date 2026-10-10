import { HttpInterceptorFn } from '@angular/common/http';

// TODO: Attach the bearer token to API requests once the auth flow is implemented.
export const jwtInterceptor: HttpInterceptorFn = (req, next) => next(req);
