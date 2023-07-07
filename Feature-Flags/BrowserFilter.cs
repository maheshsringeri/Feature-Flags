using Microsoft.FeatureManagement;

namespace Feature_Flags
{
    [FilterAlias("Browser")]
    public class BrowserFilter : IFeatureFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BrowserFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context, CancellationToken cancellationToken = default)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var userAgent = _httpContextAccessor.HttpContext.Request.Headers.UserAgent;
                var settings = context.Parameters.Get<BrowserFilterSettings>();

                return Task.FromResult(settings.Allowed.Any(a => userAgent.Contains(a)));
            }

            throw new NotImplementedException();
        }

        public class BrowserFilterSettings
        {
            public string[] Allowed { get; set; }
        }
    }
}
