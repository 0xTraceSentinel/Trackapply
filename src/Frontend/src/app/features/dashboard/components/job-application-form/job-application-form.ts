import { Component, inject, input, output } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import {
    CreateJobApplicationRequest,
    JOB_APPLICATION_STATUSES,
    JobApplicationStatus,
} from '@shared';

const TITLE_MAX_LENGTH = 250;
const COMPANY_MAX_LENGTH = 150;
const COMPANY_URL_MAX_LENGTH = 2048;
const DESCRIPTION_MAX_LENGTH = 4000;
const URL_PATTERN = /^https?:\/\/\S+$/i;

@Component({
    imports: [ReactiveFormsModule],
    selector: 'app-job-application-form',
    styleUrl: './job-application-form.scss',
    templateUrl: './job-application-form.html',
})
export class JobApplicationForm {
    readonly saving = input(false);
    readonly submitted = output<CreateJobApplicationRequest>();

    protected readonly statuses = JOB_APPLICATION_STATUSES;

    protected readonly form = inject(NonNullableFormBuilder).group({
        title: ['', [Validators.required, Validators.maxLength(TITLE_MAX_LENGTH)]],
        company: ['', [Validators.required, Validators.maxLength(COMPANY_MAX_LENGTH)]],
        companyUrl: [
            '',
            [Validators.maxLength(COMPANY_URL_MAX_LENGTH), Validators.pattern(URL_PATTERN)],
        ],
        description: ['', [Validators.maxLength(DESCRIPTION_MAX_LENGTH)]],
        status: ['Bookmarked' as JobApplicationStatus],
    });

    reset(): void {
        this.form.reset();
    }

    protected isInvalid(control: keyof typeof this.form.controls): boolean {
        const { invalid, touched } = this.form.controls[control];
        return invalid && touched;
    }

    protected submit(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        const { title, company, companyUrl, description, status } = this.form.getRawValue();
        this.submitted.emit({
            title: title.trim(),
            company: company.trim(),
            companyUrl: companyUrl.trim() || null,
            description: description.trim() || null,
            status,
        });
    }
}
