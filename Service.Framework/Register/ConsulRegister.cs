using Consul;
using Microsoft.Extensions.Options;

namespace Service.Framework.Register
{
    public class ConsulRegister : IConsulRegister
    {
        private readonly ConsulRegisterOptions _consulRegisterOptions;

        public ConsulRegister(IOptionsMonitor<ConsulRegisterOptions> consulRegisterOptions)
        {
            _consulRegisterOptions = consulRegisterOptions.CurrentValue;
        }

        public async Task ConsulRegistAsync()
        {
            var client = new ConsulClient(options =>
            {
                options.Address = new Uri(_consulRegisterOptions.Address); // Consul客户端地址
            });

            var registration = new AgentServiceRegistration
            {
                ID = Guid.NewGuid().ToString(), // 唯一Id
                Name = _consulRegisterOptions.Name, // 服务名(分组--多个实例组成的集群)
                Address = _consulRegisterOptions.Ip, // 服务绑定IP
                Port = Convert.ToInt32(_consulRegisterOptions.Port), // 服务绑定端口
                //Tag 标签
                Check = new AgentServiceCheck
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5), // 服务启动多久后注册
                    Interval = TimeSpan.FromSeconds(10), // 健康检查时间间隔
                    HTTP = $"http://{_consulRegisterOptions.Ip}:{_consulRegisterOptions.Port}{_consulRegisterOptions.HealthCheck}", // 健康检查地址
                    Timeout = TimeSpan.FromSeconds(5) // 超时时间
                }
            };

            await client.Agent.ServiceRegister(registration);
        }
    }
}
