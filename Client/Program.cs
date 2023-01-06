using Consul;
using Service.Framework;
using Service.Framework.ConsulExtends;
using Service.Framework.ConsulExtends.ClienExtend;
using System.Web;
using static Service.Framework.ConsulExtends.ConsulExtend;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ConsulRegisterOptions>(builder.Configuration.GetSection("ConsulRegisterOptions"));
builder.Services.AddConsulDispatcher(ConsulDispatcherType.Polling);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/ServiceADiscoveryTest", () =>
{
    //基于Consul去获取地址信息
    string url = "http://ServiceA/test";
    ConsulClient client = new ConsulClient(c =>
    {
        c.Address = new Uri("http://localhost:8500/");
    });//找到consul--像DNS

    //获取Consul全部服务清单
    var response = client.Agent.Services().Result.Response;

    Uri uri = new Uri(url);
    string groupName = uri.Host;

    AgentService agentService = null;
    var dictionary = response.Where(s => s.Value.Service.Equals(groupName, StringComparison.OrdinalIgnoreCase)).ToArray();
    {
        //负载均衡策略-随机
        agentService = dictionary[Random.Shared.Next(1, 10) % dictionary.Length].Value;
    }
    url = $"{uri.Scheme}://localhost:{agentService.Port}{uri.PathAndQuery}";
    string content = new HttpAPIInvoker().InvokeApi(url);
    return HttpUtility.UrlDecode(content);
});

app.MapGet("/ServiceBDiscoveryTest", (AbstractConsulDispatcher abstractConsulDispatcher) =>
{
    //基于Consul去获取地址信息
    string url = "http://ServiceB/test";
    string realUrl = abstractConsulDispatcher.GetAddress(url);
    string content = new HttpAPIInvoker().InvokeApi(realUrl); 
    return HttpUtility.UrlDecode(content);
});

app.Run();

public class HttpAPIInvoker
{
    public int Index = 0;
    public string InvokeApi(string url)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            HttpRequestMessage message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri(url);
            var result = httpClient.SendAsync(message).Result;
            string content = result.Content.ReadAsStringAsync().Result;
            return content;
        }
    }
}

