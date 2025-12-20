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
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
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

        public static string SuperadminLogin => (_configuration
            ?? throw new InvalidOperationException("Configuration missing")).GetValue<string>("SuperAdmin:Login")
                                         ?? throw new InvalidOperationException("Login not configured");

        public static string SuperadminPassword => (_configuration
            ?? throw new InvalidOperationException("Configuration missing")).GetValue<string>("SuperAdmin:Password")
                                         ?? throw new InvalidOperationException("Password not configured");

        public static string SuperadminFullName => (_configuration
            ?? throw new InvalidOperationException("Configuration missing")).GetValue<string>("SuperAdmin:FullName")
                                         ?? throw new InvalidOperationException("FullName not configured");
    }
}
