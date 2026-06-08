using Microsoft.EntityFrameworkCore;
using WebLab.Api.Models;

namespace WebLab.Api.Data;

public sealed class WebLabDbContext : DbContext
{
    public WebLabDbContext(DbContextOptions<WebLabDbContext> options)
        : base(options)
    {
    }

    public DbSet<Submission> Submissions => Set<Submission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Submission>(entity =>
        {
            entity.HasKey(submission => submission.Id);
            entity.Property(submission => submission.Type).IsRequired();
            entity.Property(submission => submission.Name).IsRequired();
            entity.Property(submission => submission.Email).IsRequired();
            entity.Property(submission => submission.Message).IsRequired();
            entity.Property(submission => submission.SubmittedAt).IsRequired();
            entity.HasIndex(submission => submission.Type);
            entity.HasIndex(submission => submission.Email);
        });
    }
}
