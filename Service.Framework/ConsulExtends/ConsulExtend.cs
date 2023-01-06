using Microsoft.Extensions.DependencyInjection;
using Service.Framework.ConsulExtends.ClienExtend;
using Service.Framework.Register;

namespace Service.Framework.ConsulExtends
{
    public static class ConsulExtend
    {
        public static void AddConsulRegister(this IServiceCollection services)
        {
            services.AddTransient<IConsulRegister, ConsulRegister>();
        }

        /// <summary>
        /// 注册Consul调度策略
        /// </summary>
        /// <param name="services"></param>
        /// <param name="consulDispatcherType"></param>
        public static void AddConsulDispatcher(this IServiceCollection services, ConsulDispatcherType consulDispatcherType)
        {
            switch (consulDispatcherType)
            {
                case ConsulDispatcherType.Average:
                    services.AddTransient<AbstractConsulDispatcher, AverageDispatcher>();
                    break;
                case ConsulDispatcherType.Polling:
                    services.AddTransient<AbstractConsulDispatcher, PollingDispatcher>();
                    break;
                case ConsulDispatcherType.Weight:
                    services.AddTransient<AbstractConsulDispatcher, WeightDispatcher>();
                    break;
                default:
                    break;
            }
        }

        public enum ConsulDispatcherType
        {
            Average = 0,
            Polling = 1,
            Weight = 2
        }
    }
}
