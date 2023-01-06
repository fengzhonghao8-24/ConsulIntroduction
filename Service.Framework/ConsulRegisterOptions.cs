namespace Service.Framework
{
    public class ConsulRegisterOptions
    {
        /// <summary>
        /// Consul 客户端地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 健康检查地址
        /// </summary>
        public string HealthCheck { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 服务绑定Ip
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 服务绑定端口
        /// </summary>
        public string Port { get; set; }
    }
}
