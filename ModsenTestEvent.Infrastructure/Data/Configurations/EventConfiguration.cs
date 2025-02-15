namespace ModsenTestEvent.Infrastructure.Data.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Participants)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        builder.HasMany(x => x.Images)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
    }
}