using APForums.Server.Models;
using APForums.Server.Models.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APForums.Server.Data
{
    public class EventEntityTypeConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(e => e.PostedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Visibility)
                .HasConversion<int>()
                .HasDefaultValue(EventVisibility.Public);

           /* builder.HasData(
                new Event
                {
                    Id = 1,
                    Title = "APU Zombie Run 2023",
                    SubTitle = "Sport Festival 5KM Run",
                    Description = "Hey there, you ever wonder what would it be like if zombies were chasing you?"
                    + " Well, today is your lucky day! ~ Come join us at APU's 2023 Sports Festival Zombie Run"
                    + " for a thrilling 5 KM run around Teknologi Park Malaysia. Be fast or be sqaure ;)!",
                    ImagePath = "Data/Resources/Events/1-APU Zombie Run 2023.jpg",
                    PostedDate = new DateTime(2023, 7, 18),
                    StartDate = new DateTime(2023, 8, 19),
                    EndDate = new DateTime(2023, 8, 20)
                },
                new Event
                {
                    Id = 2,
                    Title = "APU Merdeka Fiesta 2023",
                    SubTitle = "Merdeka Festival 2023",
                    Description = "Celebrate Merdeka with APU through a series of fun activities. Join with your friends" +
                    " and make new ones in a plethora of events including the Merdeka Run and Paintball Arena!",
                    ImagePath = "2-APU Merdeka Fiesta 2023.jpg",
                    PostedDate = new DateTime(2023, 7, 9),
                    StartDate = new DateTime(2023, 9, 9),
                    EndDate = new DateTime(2023, 9, 10)
                }
                );*/
        }
    }
}
