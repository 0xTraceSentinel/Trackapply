import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { ApiError } from '@shared';

export const errorInterceptor: HttpInterceptorFn = (req, next) =>
    next(req).pipe(catchError((error: unknown) => throwError(() => toApiError(error))));

function toApiError(error: unknown): ApiError {
    if (!(error instanceof HttpErrorResponse)) {
        return { status: 0, message: 'An unexpected error occurred.' };
    }

    if (error.status === 0) {
        return { status: 0, message: 'Unable to reach the server. Please check your connection.' };
    }

    const problem = error.error as {
        title?: string;
        detail?: string;
        errors?: Record<string, string[]>;
    } | null;

    return {
        status: error.status,
        message: problem?.detail ?? problem?.title ?? error.message,
        validationErrors: problem?.errors,
    };
}
