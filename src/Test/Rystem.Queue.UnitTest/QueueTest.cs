using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rystem.Queue.Test.UnitTest
{
    public class Sample
    {
        public string? Id { get; set; }
    }
    public class QueueTest
    {
        private static readonly IServiceProvider _serviceProvider;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3963:\"static\" fields should be initialized inline", Justification = "Test")]
        static QueueTest()
        {
            IServiceCollection services = new ServiceCollection()
                .AddMemoryQueue<Sample>(x =>
                {
                    x.MaximumBuffer = 1000;
                    x.Actions.Add(t =>
                    {
                        _ = t;
                        return Task.CompletedTask;
                    });
                    x.MaximumRetentionCronFormat = "*/3 * * * * *";
                });
            _serviceProvider = services.BuildServiceProvider();
            _serviceProvider.WarmUpAsync().ToResult();
        }
        [Fact]
        public async Task ExpiryRun()
        {
            var queue = _serviceProvider.GetService<IQueue<Sample>>()!;
            for (int i = 0; i < 100; i++)
                await queue.AddAsync(new Sample() { Id = i.ToString() });
            Assert.Equal(100, await queue.CountAsync());
            await Task.Delay(7_000);
            Assert.Equal(0, await queue.CountAsync());
        }
        [Fact]
        public async Task MoreThan1000Run()
        {
            var queue = _serviceProvider.GetService<IQueue<Sample>>()!;
            for (int i = 0; i < 1001; i++)
                await queue.AddAsync(new Sample() { Id = i.ToString() });
            await Task.Delay(4_000);
            Assert.Equal(0, await queue.CountAsync());
        }
    }
}