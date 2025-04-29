namespace GameAPIClient.Model
{
    public class SessionRecordRequest
    {
        public string PlayerId { get; set; } = default!;
        public string SessionId { get; set; } = default!;
        public bool Won { get; set; }
        public int CoinsChange { get; set; }
    }

}
