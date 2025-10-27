using Com.Coppel.Web.Api.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Com.Coppel.Web.Api.Infrastructure.Persistence.Context;

/// <summary>
/// DbContext para el módulo de certificación de especificaciones
/// </summary>
public class SpecDbContext : DbContext
{
    public SpecDbContext(DbContextOptions<SpecDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<Revision> Revisions { get; set; } = null!;
    public DbSet<Evaluation> Evaluations { get; set; } = null!;
    public DbSet<Criterion> Criteria { get; set; } = null!;
    public DbSet<EvaluationCriterion> EvaluationCriteria { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Revisión configurations
        modelBuilder.Entity<Revision>(entity =>
        {
            entity.ToTable("revisions");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.Number).HasColumnName("number").IsRequired();
            entity.Property(e => e.Type).HasColumnName("type").HasMaxLength(16).IsRequired();
            entity.Property(e => e.Key).HasColumnName("key").HasMaxLength(128).IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(256).IsRequired();
            entity.Property(e => e.UploadedAt).HasColumnName("uploaded_at").IsRequired();
            entity.Property(e => e.SpecVersion).HasColumnName("spec_version").HasMaxLength(32);
            entity.Property(e => e.SpecDate).HasColumnName("spec_date");
            entity.Property(e => e.SpecAuthor).HasColumnName("spec_author").HasMaxLength(128);
            entity.Property(e => e.Checksum).HasColumnName("checksum");
            
            // Indexes
            entity.HasIndex(e => new { e.Key, e.Number }).IsUnique();
            entity.HasIndex(e => e.UploadedAt);
            entity.HasIndex(e => e.Type);
        });

        // Evaluation configurations
        modelBuilder.Entity<Evaluation>(entity =>
        {
            entity.ToTable("evaluations");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.RevisionId).HasColumnName("revision_id").IsRequired();
            entity.Property(e => e.Idempotency).HasColumnName("idempotency").HasMaxLength(16);
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(16).IsRequired();
            entity.Property(e => e.OverallStatus).HasColumnName("overall_status").HasMaxLength(16);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.Model).HasColumnName("model").HasMaxLength(64);
            entity.Property(e => e.PromptVersion).HasColumnName("prompt_version").HasMaxLength(32);
            entity.Property(e => e.Retries).HasColumnName("retries").HasDefaultValue(0);
            
            entity.Property(e => e.ScorePass).HasColumnName("score_pass");
            entity.Property(e => e.ScoreFail).HasColumnName("score_fail");
            entity.Property(e => e.ScoreNa).HasColumnName("score_na");
            entity.Property(e => e.ScoreTotal).HasColumnName("score_total");
            
            entity.Property(e => e.CritPass).HasColumnName("crit_pass");
            entity.Property(e => e.CritFail).HasColumnName("crit_fail");
            entity.Property(e => e.CritNa).HasColumnName("crit_na");
            
            entity.Property(e => e.GatePassed).HasColumnName("gate_passed");
            entity.Property(e => e.GateReason).HasColumnName("gate_reason").HasMaxLength(512);
            
            entity.Property(e => e.RawResult).HasColumnName("raw_result").HasColumnType("jsonb");
            
            // Indexes
            entity.HasIndex(e => e.RevisionId);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.OverallStatus);
            
            // Relationship
            entity.HasOne(e => e.Revision)
                .WithMany(r => r.Evaluations)
                .HasForeignKey(e => e.RevisionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Criterion configurations
        modelBuilder.Entity<Criterion>(entity =>
        {
            entity.ToTable("criteria");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.Version).HasColumnName("version").HasMaxLength(16).IsRequired();
            entity.Property(e => e.Category).HasColumnName("category").HasMaxLength(128).IsRequired();
            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(256).IsRequired();
            entity.Property(e => e.Severity).HasColumnName("severity").HasMaxLength(16).IsRequired();
        });

        // EvaluationCriterion configurations
        modelBuilder.Entity<EvaluationCriterion>(entity =>
        {
            entity.ToTable("evaluation_criteria");
            
            entity.HasKey(e => new { e.EvaluationId, e.CriterionId });
            
            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.EvaluationId).HasColumnName("evaluation_id").IsRequired();
            entity.Property(e => e.CriterionId).HasColumnName("criterion_id").IsRequired();
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(8).IsRequired();
            entity.Property(e => e.Severity).HasColumnName("severity").HasMaxLength(16).IsRequired();
            entity.Property(e => e.Evidence).HasColumnName("evidence").HasColumnType("text");
            entity.Property(e => e.Comments).HasColumnName("comments").HasColumnType("text");
            
            // Indexes
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Severity);
            
            // Relationships
            entity.HasOne(e => e.Evaluation)
                .WithMany(ev => ev.Criteria)
                .HasForeignKey(e => e.EvaluationId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Criterion)
                .WithMany()
                .HasForeignKey(e => e.CriterionId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

