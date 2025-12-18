using Npgsql;

namespace LostAndFound.Configs
{
    class DatabaseConfig
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string SslMode { get; set; } = "Prefer";
        public bool Pooling { get; set; } = true;
        public int MaxPoolSize { get; set; } = 10;

        public string BuildConnectionString()
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = Host,
                Port = Port,
                Password = Password,
                Username = Username,
                Database = DatabaseName,
                SslMode = Enum.Parse<SslMode>(SslMode),
                Pooling = Pooling,
                MaxPoolSize = MaxPoolSize,
                Timeout = 30,
                KeepAlive = 30
            };

            return builder.ToString();
        }
    }
}
