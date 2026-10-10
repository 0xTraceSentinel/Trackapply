export const JOB_APPLICATION_STATUSES = [
    'Bookmarked',
    'Applied',
    'Interviewing',
    'Offered',
    'Rejected',
] as const;

export type JobApplicationStatus = (typeof JOB_APPLICATION_STATUSES)[number];

export interface JobApplication {
    id: string;
    title: string;
    company: string;
    companyUrl: string | null;
    description: string | null;
    status: JobApplicationStatus;
    createdAt: string;
    updatedAt: string | null;
}

export interface CreateJobApplicationRequest {
    title: string;
    company: string;
    companyUrl?: string | null;
    description?: string | null;
    status?: JobApplicationStatus;
}

export type UpdateJobApplicationRequest = Omit<CreateJobApplicationRequest, 'status'>;
