using System;
using System.Collections.Generic;

// See https://refactoring.guru/design-patterns/builder/csharp/example
namespace DesignPatterns.Creational.Builder
{
    public interface IBuilder
    {
        IBuilder SetHost(string host);

        IBuilder SetPort(ushort port);

        IBuilder SetCredential(string username, string password);

        HostClient GetClient();
    }

    public class ConcreteBuilder : IBuilder
    {
        private HostClient _client = new HostClient();

        public IBuilder SetHost(string host)
        {
            _client.Host = host;
            return this;
        }

        public IBuilder SetPort(ushort port)
        {
            _client.Port = port;
            return this;
        }

        public IBuilder SetCredential(string username, string password)
        {
            _client.Username = username;
            _client.Password = password;
            return this;
        }

        public HostClient GetClient()
        {
            HostClient result = _client;
            _client = new HostClient();
            return result;
        }
    }

    public class HostClient
    {
        public string Host { get; set; } = default!;
        public ushort Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public override string ToString() => $"{Host}:{Port}";
    }

    public class Director
    {
        public IBuilder? Builder { private get; set; }

        public void BuildMinimalViableProduct()
        {
            Builder?.SetHost("localhost");
        }

        public void BuildFullFeaturedProduct()
        {
            Builder?.SetHost("localhost").SetPort(0)
                .SetCredential(string.Empty, string.Empty);
        }
    }
}