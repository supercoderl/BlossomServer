using BlossomServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Infrastructure.Configuration
{
    public sealed class WorkScheduleConfiguration : IEntityTypeConfiguration<WorkSchedule>
    {
        public void Configure(EntityTypeBuilder<WorkSchedule> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.TechnicianId).IsRequired();

            builder.Property(w => w.WorkDate).IsRequired();

            builder.Property(w => w.StartTime).IsRequired();    

            builder.Property(w => w.EndTime).IsRequired();

            builder.Property(w => w.IsDayOff).IsRequired();

            builder.HasOne(t => t.Technician)
                .WithMany(w => w.WorkSchedules)
                .HasForeignKey(t => t.TechnicianId)
                .HasConstraintName("FK_WorkSchedule_Technician_TechnicianId")
                .OnDelete(DeleteBehavior.Cascade);  
        }
    }
}
