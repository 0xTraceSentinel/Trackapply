import { DatePipe } from '@angular/common';
import { Component, DestroyRef, OnInit, computed, inject, signal, viewChild } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { finalize } from 'rxjs';
import { JobApplicationService } from '@core';
import {
    ApiError,
    CreateJobApplicationRequest,
    JOB_APPLICATION_STATUSES,
    JobApplication,
    JobApplicationStatus,
} from '@shared';
import { JobApplicationForm } from './components/job-application-form/job-application-form';

type StatusFilter = JobApplicationStatus | 'All';

@Component({
    imports: [DatePipe, JobApplicationForm],
    selector: 'app-dashboard',
    styleUrl: './dashboard.scss',
    templateUrl: './dashboard.html',
})
export class Dashboard implements OnInit {
    private readonly jobApplicationService = inject(JobApplicationService);
    private readonly destroyRef = inject(DestroyRef);
    private readonly form = viewChild.required(JobApplicationForm);

    protected readonly statuses = JOB_APPLICATION_STATUSES;
    protected readonly applications = signal<JobApplication[]>([]);
    protected readonly loading = signal(true);
    protected readonly saving = signal(false);
    protected readonly error = signal<string | null>(null);
    protected readonly statusFilter = signal<StatusFilter>('All');

    protected readonly filteredApplications = computed(() => {
        const filter = this.statusFilter();
        const applications = this.applications();
        return filter === 'All' ? applications : applications.filter((a) => a.status === filter);
    });

    protected readonly countsByStatus = computed(() => {
        const counts = Object.fromEntries(this.statuses.map((s) => [s, 0])) as Record<
            JobApplicationStatus,
            number
        >;
        for (const application of this.applications()) {
            counts[application.status]++;
        }
        return counts;
    });

    ngOnInit(): void {
        this.loadApplications();
    }

    protected loadApplications(): void {
        this.loading.set(true);
        this.error.set(null);

        this.jobApplicationService
            .getAll()
            .pipe(
                finalize(() => this.loading.set(false)),
                takeUntilDestroyed(this.destroyRef),
            )
            .subscribe({
                next: (applications) => this.applications.set(applications),
                error: (error: ApiError) => this.error.set(error.message),
            });
    }

    protected createApplication(request: CreateJobApplicationRequest): void {
        this.saving.set(true);
        this.error.set(null);

        this.jobApplicationService
            .create(request)
            .pipe(
                finalize(() => this.saving.set(false)),
                takeUntilDestroyed(this.destroyRef),
            )
            .subscribe({
                next: (created) => {
                    this.applications.update((applications) => [created, ...applications]);
                    this.form().reset();
                },
                error: (error: ApiError) => this.error.set(error.message),
            });
    }

    protected changeStatus(application: JobApplication, select: HTMLSelectElement): void {
        const status = select.value as JobApplicationStatus;
        if (application.status === status) {
            return;
        }

        this.error.set(null);

        this.jobApplicationService
            .updateStatus(application.id, status)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: (updated) => this.replaceApplication(updated),
                error: (error: ApiError) => {
                    this.error.set(error.message);
                    // No binding changed, so Angular won't touch the DOM; revert the control ourselves.
                    select.value = application.status;
                },
            });
    }

    protected deleteApplication(application: JobApplication): void {
        if (!confirm(`Delete "${application.title}" at ${application.company}?`)) {
            return;
        }

        this.error.set(null);

        this.jobApplicationService
            .delete(application.id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () =>
                    this.applications.update((applications) =>
                        applications.filter((a) => a.id !== application.id),
                    ),
                error: (error: ApiError) => this.error.set(error.message),
            });
    }

    private replaceApplication(updated: JobApplication): void {
        this.applications.update((applications) =>
            applications.map((a) => (a.id === updated.id ? updated : a)),
        );
    }
}
