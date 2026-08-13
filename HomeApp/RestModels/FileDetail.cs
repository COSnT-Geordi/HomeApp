namespace HomeApp.RestModels
{
    public class FileDetail
    {
        public int Id { get; set; }
        public string FileName { get; set; } = "";
        public string FilePath { get; set; } = "";
        public long FileSize { get; set; } = 0;
        public long BundleVersion { get; set; } = 0;
        public bool IsUpdaterBundle { get; set; } = false;
        public DateTime CreationDate { get; set; } = DateTime.MinValue;

        public TimeSpan InstallAfter { get; set; }
        public TimeSpan InstallBefore { get; set; }
    }
}
