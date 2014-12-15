namespace Teamroom.Common.Configuration
{
    public interface IConfiguration
    {
        string StorageConnectionString { get; set; }
        string FacebookAppId { get; }
        string FacebookAppSecret { get; }
    }
}
