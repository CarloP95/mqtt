﻿using System.Reactive;
using System.Reactive.Subjects;
using Hermes.Storage;
using ReactiveSockets;

namespace Hermes
{
	public class ClientFactory
	{
		public static IClient Create (string serverAddress, ProtocolConfiguration configuration)
		{	
			var reactiveSocket = new ReactiveClient (serverAddress, configuration.Port);

			reactiveSocket.ConnectAsync ().Wait ();

			var socket = new ReactiveSocketChannel (reactiveSocket);

			var topicEvaluator = new TopicEvaluator(configuration);
			var packetChannelFactory = new PacketChannelFactory (topicEvaluator); //TODO: We need to inject this, but it currently affects Client experience creating factories
			var channel = packetChannelFactory.CreateChannel (socket);

			var timeListener = new Subject<Unit> ();
			var packetIdentifierRepository = new InMemoryRepository<PacketIdentifier>(); //TODO: We need to inject this, but it currently affects Client experience creating factories

			return new Client (channel, timeListener, configuration, packetIdentifierRepository);
		}
	}
}