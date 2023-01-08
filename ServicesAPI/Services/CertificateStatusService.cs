
namespace ServicesAPI.Services
{
    public class CertificateStatusService : BackgroundService
    {
        private readonly ILogger<CertificateStatusService> _logger;
        private readonly IHostApplicationLifetime _applicationLifetime;

        private readonly FileSystemWatcher _fileWatcher;
        
        public CertificateStatusService(ILogger<CertificateStatusService> logger, IHostApplicationLifetime applicationLifetime)
        {
            _fileWatcher = new FileSystemWatcher(@"/certs/");
            _logger = logger;
            _applicationLifetime = applicationLifetime;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("[ServicesAPI::CertificateStatusService] Initalizating Certificate Status Service...");


            _fileWatcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.Size
                                 | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security;

            _fileWatcher.Changed += OnChanged;
            _fileWatcher.Created += OnCreated;
            _fileWatcher.Deleted += OnDeleted;
            _fileWatcher.Renamed += OnRenamed;

            _fileWatcher.Filter = "*.crt";

            _fileWatcher.EnableRaisingEvents = true;

            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[ServicesAPI::CertificateStatusService] Stopping Certificate Status Service...");

            await base.StopAsync(cancellationToken);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            _logger.LogInformation("[ServicesAPI::CertificateStatusService::OnChanged]  Certificate file has been changed...");

            RestartApplication();
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Created)
            {
                return;
            }

            _logger.LogInformation("[ServicesAPI::CertificateStatusService::OnCreated]  Certificate file has been created...");

            RestartApplication();
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Deleted)
            {
                return;
            }

            _logger.LogInformation("[ServicesAPI::CertificateStatusService::OnDeleted]  Certificate file has been deleted...");

            RestartApplication();
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Renamed)
            {
                return;
            }

            _logger.LogInformation("[ServicesAPI::CertificateStatusService::OnRenamed] Certificate file has been renamed...");

            RestartApplication();
        }

        private void RestartApplication()
        {
            _logger.LogInformation("[ServicesAPI::CertificateStatusService::RestartApplication] Restarting Kesterl Web Server...");

            _applicationLifetime.StopApplication();
        }
    }
}
