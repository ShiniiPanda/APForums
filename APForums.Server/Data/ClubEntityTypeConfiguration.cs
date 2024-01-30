using APForums.Server.Models;
using APForums.Server.Models.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APForums.Server.Data
{
    public class ClubEntityTypeConfiguration : IEntityTypeConfiguration<Club>
    {
        public void Configure(EntityTypeBuilder<Club> builder)
        {
            builder.Property(c => c.Status)
                .HasColumnType("nvarchar(20)");

            builder.Property(c => c.Type)
                .HasConversion<int>()
                .HasDefaultValue(ClubType.SIG);


            /* builder.HasData(
                new Club {
                    Id = 1,
                    Name = "Manga, Anime and Games",
                    Abbreviation = "MAG",
                    Description = "The MAG club was founded in July 2012 and aims to promote a friendly environment"
                    + " for APU Students to talk about Anime, Manga and Games and help students"
                    + " of similar interests to meet with each other!",
                    Status = ClubStatus.Active
                },
                new Club
                {
                    Id = 2,
                    Name = "Games Development Club",
                    Abbreviation = "GDC",
                    Description = "The APU GDC promotes the development of video games among"
                    + " APU students by hosting various workshops, game jams, and many cool events!",
                    Status = ClubStatus.Active
                },
                new Club
                {
                    Id = 3,
                    Name = "Japanese Culture Club",
                    Abbreviation = "JCC",
                    Description = "APU's GCC hopes to create a community of Japanese Culture Enthusiasts."
                    + " The club holds monthly member activities usually related to japanese"
                    + " culture as well as some less serious and fun events!",
                    Status = ClubStatus.Active
                },
                new Club
                {
                    Id = 4,
                    Name = "Chinese Language & Culture Society",
                    Abbreviation = "CLCS",
                    Description = "APU's CLCS promotes the wonderful culture of china and"
                    + " offers fun and educational events about chinese culture and"
                    + " many dialects. Everyone is welcome to join!",
                    Status = ClubStatus.Active
                }
                 );*/
        }
    }
}
