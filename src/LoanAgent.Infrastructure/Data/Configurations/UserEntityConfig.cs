using LoanAgent.Domain.Entities;
using LoanAgent.Infrastructure.Data.Configurations.Common;
using LoanAgent.Infrastructure.Data.Configurations.Seeds;
using LoanAgent.Infrastructure.Data.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanAgent.Infrastructure.Data.Configurations;

internal class UserEntityConfig : EntityBaseConfig<UserEntity>
{
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        base.Configure(builder);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Username)
            .HasMaxLength(EntityConfigConstants.UsernameMaxLength)
            .IsRequired();

        builder.Property(x => x.Password)
            .HasMaxLength(EntityConfigConstants.PasswordMaxLength)
            .IsRequired();

        builder.Property(x => x.Salt)
            .HasMaxLength(EntityConfigConstants.SaltMaxLength)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(EntityConfigConstants.EmailMaxLength)
            .IsRequired();

        builder.Property(x => x.FirstName)
            .HasMaxLength(EntityConfigConstants.FirstNameMaxLength)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(EntityConfigConstants.LastNameMaxLength)
            .IsRequired();

        builder.Property(x => x.IdNumber)
            .HasMaxLength(EntityConfigConstants.IdNumberMaxLength)
            .IsRequired();

        builder.Property(x => x.DateOfBirth)
            .HasColumnType(EntityConfigConstants.DateOfBirthColumnType)
            .IsRequired();

        builder.Property(x => x.UserRole)
               .HasConversion<int>()
               .IsRequired();

        builder.HasMany(x => x.Loans)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder.ToTable("Users");

        builder.HasData(AdminUserSeeds.Data);
    }
}

