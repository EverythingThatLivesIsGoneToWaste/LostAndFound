namespace LostAndFound.Configs
{
    class Config
    {
        private static IConfiguration? _configuration;

        private static DatabaseConfig? _databaseConfig;

        static Config()
        {
            Initialize();
        }

        public static void Initialize(IConfiguration? configuration = null)
        {
            if (configuration != null) _configuration = configuration;
            else
            {
                _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            }

            _databaseConfig = _configuration.GetSection("Database").Get<DatabaseConfig>();

            if (_databaseConfig == null)
            {
                throw new InvalidOperationException("Database config section is missing");
            }
        }

        public static string DatabaseConnectionString
        {
            get
            {
                if (_databaseConfig == null)
                {
                    Initialize();
                }
                return _databaseConfig!.BuildConnectionString();
            }
        }

        public static string SuperadminUsername => (_configuration
            ?? throw new InvalidOperationException("Configuration missing")).GetValue<string>("Superadmin:Username")
                                         ?? throw new InvalidOperationException("Username not configured");

        public static string SuperadminPassword => (_configuration
            ?? throw new InvalidOperationException("Configuration missing")).GetValue<string>("Superadmin:Password")
                                         ?? throw new InvalidOperationException("Password not configured");
    }
}
