import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@env';
import {
    CreateJobApplicationRequest,
    JobApplication,
    JobApplicationStatus,
    UpdateJobApplicationRequest,
} from '@shared';

@Injectable({
    providedIn: 'root',
})
export class JobApplicationService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiUrl}/job-applications`;

    getAll(): Observable<JobApplication[]> {
        return this.http.get<JobApplication[]>(this.apiUrl);
    }

    getById(id: string): Observable<JobApplication> {
        return this.http.get<JobApplication>(`${this.apiUrl}/${id}`);
    }

    create(request: CreateJobApplicationRequest): Observable<JobApplication> {
        return this.http.post<JobApplication>(this.apiUrl, request);
    }

    update(id: string, request: UpdateJobApplicationRequest): Observable<JobApplication> {
        return this.http.put<JobApplication>(`${this.apiUrl}/${id}`, request);
    }

    updateStatus(id: string, status: JobApplicationStatus): Observable<JobApplication> {
        return this.http.patch<JobApplication>(`${this.apiUrl}/${id}/status`, { status });
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
