import { CanActivateFn } from '@angular/router';

// TODO: Check authentication state and redirect to /login once the auth flow is implemented.
export const authGuard: CanActivateFn = () => true;
