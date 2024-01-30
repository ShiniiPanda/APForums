namespace APForums.Server.Data.DTO.PageDTOS
{
    public class EventsResponse
    {

        public List<EventWIthInterestDTO> PublicEvents { get; set; } = new();

        public List<EventWIthInterestDTO> PrivateEvents { get; set; } = new();

    }
}
