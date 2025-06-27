using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Settings
{
    public sealed class RabbitMqConfiguration
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string VirtualHost { get; set; } = string.Empty;

        public string ConnectionString => $"amqps://{Username}:{Password}@{Host}:{Port}/{VirtualHost}";
    }
}
