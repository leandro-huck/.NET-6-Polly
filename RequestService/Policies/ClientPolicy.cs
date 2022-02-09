using Polly;
using Polly.Retry;

namespace RequestService.Policies
{
    public class ClientPolicy
    {
        public AsyncRetryPolicy<HttpResponseMessage> ImmediateHttpRetry { get; set; }
        public AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetry { get; set; }
        public AsyncRetryPolicy<HttpResponseMessage> ExponentialHttpRetry { get; set; }

        public ClientPolicy()
        {
            // Retry immediately: max. 5 times
            ImmediateHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode).
                RetryAsync(5);

            // Wait 3 seconds and retry: máx 5 times
            LinearHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode).
                WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(3));

            // Wait 2 seconds raised to the number of attempts and retry: máx 5 times
            ExponentialHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode).
                WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}