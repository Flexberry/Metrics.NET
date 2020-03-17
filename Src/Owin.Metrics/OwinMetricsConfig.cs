using System;
using System.Text.RegularExpressions;
using Metrics;
using Metrics.Reports;
using Owin.Metrics.Middleware;
namespace Owin.Metrics
{
    public class OwinMetricsConfig
    {
        public static readonly OwinMetricsConfig Disabled = new OwinMetricsConfig();

        private readonly Action<object> middlewareRegistration;
        private readonly MetricsContext context;
        private readonly Func<HealthStatus> healthStatus;

        private readonly bool isDisabled;

        public OwinMetricsConfig(Action<object> middlewareRegistration, MetricsContext context, Func<HealthStatus> healthStatus)
        {
            this.middlewareRegistration = middlewareRegistration;
            this.context = context;
            this.healthStatus = healthStatus;
        }

        private OwinMetricsConfig()
        {
            this.isDisabled = true;
        }

        /// <summary>
        /// Register all predefined metrics.
        /// </summary>
        /// <param name="ignoreRequestPathPatterns">Patterns for paths to ignore.</param>
        /// <param name="owinContext">Name of the metrics context where to register the metrics.</param>
        /// <returns>Chainable configuration object.</returns>
        public OwinMetricsConfig WithRequestMetricsConfig(Regex[] ignoreRequestPathPatterns = null, string owinContext = "Owin")
        {
            if (this.isDisabled)
            {
                return this;
            }

            return WithRequestMetricsConfig(config => config.WithAllOwinMetrics(), ignoreRequestPathPatterns, owinContext);
        }

        /// <summary>
        /// Configure which Owin metrics to be registered.
        /// </summary>
        /// <param name="config">Action used to configure Owin metrics.</param>
        /// <param name="ignoreRequestPathPatterns">Patterns for paths to ignore.</param>
        /// <param name="owinContext">Name of the metrics context where to register the metrics.</param>
        /// <returns>Chainable configuration object.</returns>
        public OwinMetricsConfig WithRequestMetricsConfig(Action<OwinRequestMetricsConfig> config,
            Regex[] ignoreRequestPathPatterns = null, string owinContext = "Owin")
        {
            if (this.isDisabled)
            {
                return this;
            }

            OwinRequestMetricsConfig requestConfig = new OwinRequestMetricsConfig(this.middlewareRegistration, this.context.Context(owinContext), ignoreRequestPathPatterns);
            config(requestConfig);
            return this;
        }
    }
}
