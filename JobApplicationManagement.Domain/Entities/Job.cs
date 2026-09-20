namespace JobApplicationManagement.Domain.Entities;

public class Job
{
    private Job() { }

    public Job(string title, string description, string location, decimal? salaryMin, decimal? salaryMax, Guid recruiterId)
    {
        EnsureSalaryRange(salaryMin, salaryMax);
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Location = location;
        SalaryMin = salaryMin;
        SalaryMax = salaryMax;
        RecruiterId = recruiterId;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Location { get; private set; } = null!;
    public decimal? SalaryMin { get; private set; }
    public decimal? SalaryMax { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Guid RecruiterId { get; private set; }
    public ICollection<JobApplication> Applications { get; private set; } = new List<JobApplication>();

    public void Update(string title, string description, string location, decimal? salaryMin, decimal? salaryMax)
    {
        if (!IsActive) throw new InvalidOperationException("A closed job cannot be updated.");
        EnsureSalaryRange(salaryMin, salaryMax);
        Title = title;
        Description = description;
        Location = location;
        SalaryMin = salaryMin;
        SalaryMax = salaryMax;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Close()
    {
        if (!IsActive) throw new InvalidOperationException("Job is already closed.");
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void EnsureSalaryRange(decimal? salaryMin, decimal? salaryMax)
    {
        if (salaryMin.HasValue && salaryMax.HasValue && salaryMin > salaryMax)
            throw new ArgumentException("Minimum salary cannot be greater than maximum salary.");
    }
}
