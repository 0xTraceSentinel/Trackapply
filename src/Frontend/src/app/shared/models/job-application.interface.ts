export interface JobApplication {
    id?: string;
    title: string;
    company: string;
    companyUrl?: string;
    description?: string;
    status: 'Bookmarked' | 'Applied' | 'Interviewing' | 'Offered' | 'Rejected';
    createdAt?: Date;
    updatedAt?: Date;
}
