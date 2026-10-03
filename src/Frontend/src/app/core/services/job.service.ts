import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { JobApplication } from '../../shared/models/job-application.interface';

@Injectable({
    providedIn: 'root',
})
export class JobService {
    private readonly apiUrl = 'http://localhost:5000/api/jobs';

    constructor(private http: HttpClient) {}

    getApplications(): Observable<JobApplication[]> {
        return this.http.get<JobApplication[]>(this.apiUrl);
    }

    createApplication(application: JobApplication): Observable<JobApplication> {
        return this.http.post<JobApplication>(this.apiUrl, application);
    }

    updateApplicationStatus(id: string, status: string): Observable<JobApplication> {
        return this.http.put<JobApplication>(`${this.apiUrl}/${id}/status`, { status });
    }
}
