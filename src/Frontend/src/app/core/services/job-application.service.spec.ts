import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { environment } from '@env';
import { JobApplication } from '@shared';
import { JobApplicationService } from './job-application.service';

describe('JobApplicationService', () => {
    const baseUrl = `${environment.apiUrl}/job-applications`;
    const application: JobApplication = {
        id: '1',
        title: 'Software Engineer',
        company: 'Acme',
        companyUrl: null,
        description: null,
        status: 'Applied',
        createdAt: '2026-01-01T00:00:00Z',
        updatedAt: null,
    };

    let service: JobApplicationService;
    let http: HttpTestingController;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [provideHttpClient(), provideHttpClientTesting()],
        });
        service = TestBed.inject(JobApplicationService);
        http = TestBed.inject(HttpTestingController);
    });

    afterEach(() => http.verify());

    it('getAll() should GET the collection', () => {
        let result: JobApplication[] | undefined;
        service.getAll().subscribe((applications) => (result = applications));

        const req = http.expectOne(baseUrl);
        expect(req.request.method).toBe('GET');
        req.flush([application]);

        expect(result).toEqual([application]);
    });

    it('create() should POST the request body', () => {
        const body = { title: 'Software Engineer', company: 'Acme' };
        service.create(body).subscribe();

        const req = http.expectOne(baseUrl);
        expect(req.request.method).toBe('POST');
        expect(req.request.body).toEqual(body);
        req.flush(application);
    });

    it('updateStatus() should PATCH the status sub-resource', () => {
        service.updateStatus('1', 'Interviewing').subscribe();

        const req = http.expectOne(`${baseUrl}/1/status`);
        expect(req.request.method).toBe('PATCH');
        expect(req.request.body).toEqual({ status: 'Interviewing' });
        req.flush({ ...application, status: 'Interviewing' });
    });

    it('delete() should DELETE the item', () => {
        service.delete('1').subscribe();

        const req = http.expectOne(`${baseUrl}/1`);
        expect(req.request.method).toBe('DELETE');
        req.flush(null, { status: 204, statusText: 'No Content' });
    });
});
