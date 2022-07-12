namespace Rystem.Queue
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed", Justification = "T is needed for injection.")]
    public sealed class QueueProperty<T>
    {
        public int MaximumBuffer { get; set; } = 5000;
        public string MaximumRetentionCronFormat { get; set; } = "*/1 * * * *";
        public string BackgroundJobCronFormat { get; set; } = "*/1 * * * *";
    }
}