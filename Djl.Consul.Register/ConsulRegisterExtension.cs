using Microsoft.Extensions.DependencyInjection;
using System;
using Consul;
using Djl.Consul.Register.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Djl.Consul.Register
{
    /// <summary>
    /// Consul服务注册扩展类
    /// </summary>
    public static class ConsulRegisterExtension
    {
        /// <summary>
        /// 服务注册配置节点Key
        /// </summary>
        private const string ServiceDiscovery = "ServiceDiscovery";

        /// <summary>
        /// 添加服务发现服务到DI容器中,示例参考配置项如下:
        /// "ServiceDiscovery": {
        ///     "ServiceName": "userapi",
        ///     "Consul": {
        ///         "HttpEndpoint": "http://127.0.0.1:8500",
        ///         "DnsEndpoint": {
        ///             "Address": "127.0.0.1",
        ///             "Port": 8600
        ///         }
        ///     }
        /// }
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddConsulRegister(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(ServiceDiscovery);
            if (!section.Exists())
            {
                throw new ArgumentNullException(nameof(section), "未发现任何Consul配置项,请检查Configuration是否存在相关配置");
            }

            var serviceDisvoveryOptions = new ServiceDisvoveryOption();
            section.Bind(serviceDisvoveryOptions);
            if (!ValidateConsulOption(serviceDisvoveryOptions))
            {
                throw new ArgumentException("配置项校验失败,请检查配置是否符合要求,请参考方法示例配置说明", nameof(serviceDisvoveryOptions));
            }

            services.AddOptions();
            services.Configure<ServiceDisvoveryOption>(configuration.GetSection("ServiceDiscovery"));

            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDisvoveryOption>>().Value;
                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    // if not configured, the client will use the default value "127.0.0.1:8500"
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));
        }

        private static bool ValidateConsulOption(ServiceDisvoveryOption serviceDisvoveryOptions)
        {
            if (string.IsNullOrWhiteSpace(serviceDisvoveryOptions.ServiceName) ||
                serviceDisvoveryOptions.Consul == null)
            {
                return false;
            }

            var consulOptions = serviceDisvoveryOptions.Consul;
            if (string.IsNullOrWhiteSpace(consulOptions.HttpEndpoint) || consulOptions.DnsEndpoint == null)
            {
                return false;
            }

            var dnsEndpoint = consulOptions.DnsEndpoint;
            if (string.IsNullOrWhiteSpace(dnsEndpoint.Address) || dnsEndpoint.Port <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
