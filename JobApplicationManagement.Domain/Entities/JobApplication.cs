using JobApplicationManagement.Domain.Enums;

namespace JobApplicationManagement.Domain.Entities;

public class JobApplication
{
    private JobApplication() { }

    public JobApplication(Guid jobId, Guid candidateId, string? coverLetter, string? resumeUrl)
    {
        Id = Guid.NewGuid();
        JobId = jobId;
        CandidateId = candidateId;
        CoverLetter = coverLetter;
        ResumeUrl = resumeUrl;
        AppliedAt = DateTime.UtcNow;
        Status = ApplicationStatus.Submitted;
    }

    public Guid Id { get; private set; }
    public Guid JobId { get; private set; }
    public Job Job { get; private set; } = null!;
    public Guid CandidateId { get; private set; }
    public DateTime AppliedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public ApplicationStatus Status { get; private set; }
    public string? CoverLetter { get; private set; }
    public string? ResumeUrl { get; private set; }

    public void UpdateStatus(ApplicationStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}
