import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ApiError } from '@shared';
import { errorInterceptor } from './error-interceptor';

describe('errorInterceptor', () => {
    let httpClient: HttpClient;
    let http: HttpTestingController;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                provideHttpClient(withInterceptors([errorInterceptor])),
                provideHttpClientTesting(),
            ],
        });
        httpClient = TestBed.inject(HttpClient);
        http = TestBed.inject(HttpTestingController);
    });

    afterEach(() => http.verify());

    function captureError(): () => ApiError | undefined {
        let captured: ApiError | undefined;
        httpClient.get('/test').subscribe({ error: (e: ApiError) => (captured = e) });
        return () => captured;
    }

    it('should map a ValidationProblemDetails response to an ApiError', () => {
        const getError = captureError();

        http.expectOne('/test').flush(
            {
                title: 'One or more validation errors occurred.',
                errors: { Title: ['The Title field is required.'] },
            },
            { status: 400, statusText: 'Bad Request' },
        );

        expect(getError()).toEqual({
            status: 400,
            message: 'One or more validation errors occurred.',
            validationErrors: { Title: ['The Title field is required.'] },
        });
    });

    it('should prefer the problem detail over the title', () => {
        const getError = captureError();

        http.expectOne('/test').flush(
            { title: 'Not Found', detail: 'Job application was not found.' },
            { status: 404, statusText: 'Not Found' },
        );

        expect(getError()?.message).toBe('Job application was not found.');
    });

    it('should report network failures with status 0', () => {
        const getError = captureError();

        http.expectOne('/test').error(new ProgressEvent('error'));

        expect(getError()?.status).toBe(0);
        expect(getError()?.message).toContain('Unable to reach the server');
    });
});
