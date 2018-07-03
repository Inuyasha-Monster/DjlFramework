namespace Djl.Consul.Register.Core
{
    public class ServiceDisvoveryOption
    {
        public string ServiceName { get; set; }

        public ConsulOption Consul { get; set; }
    }
}