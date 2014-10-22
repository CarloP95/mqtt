﻿using System;
using System.IO;
using System.Threading.Tasks;
using Hermes;
using Hermes.Formatters;
using Hermes.Messages;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Tests.Formatters
{
	public class PingResponseFormatterSpec
	{
		private readonly Mock<IChannel<IMessage>> messageChannel;
		private readonly Mock<IChannel<byte[]>> byteChannel;

		public PingResponseFormatterSpec ()
		{
			this.messageChannel = new Mock<IChannel<IMessage>> ();
			this.byteChannel = new Mock<IChannel<byte[]>> ();
		}
		
		[Theory]
		[InlineData("Files/PingResponse.packet")]
		public async Task when_reading_ping_response_packet_then_succeeds(string packetPath)
		{
			packetPath = Path.Combine (Environment.CurrentDirectory, packetPath);

			var sentPingResponse = default(PingResponse);

			this.messageChannel
				.Setup (c => c.SendAsync (It.IsAny<IMessage>()))
				.Returns(Task.Delay(0))
				.Callback<IMessage>(m =>  {
					sentPingResponse = m as PingResponse;
				});

			var formatter = new PingResponseFormatter (this.messageChannel.Object, this.byteChannel.Object);
			var packet = Packet.ReadAllBytes (packetPath);

			await formatter.ReadAsync (packet);

			Assert.NotNull (sentPingResponse);
		}

		[Theory]
		[InlineData("Files/PingResponse.packet")]
		public async Task when_writing_ping_response_packet_then_succeeds(string packetPath)
		{
			packetPath = Path.Combine (Environment.CurrentDirectory, packetPath);

			var expectedPacket = Packet.ReadAllBytes (packetPath);
			var sentPacket = default(byte[]);

			this.byteChannel
				.Setup (c => c.SendAsync (It.IsAny<byte[]>()))
				.Returns(Task.Delay(0))
				.Callback<byte[]>(b =>  {
					sentPacket = b;
				});

			var formatter = new PingResponseFormatter (this.messageChannel.Object, this.byteChannel.Object);
			var pingResponse = new PingResponse ();

			await formatter.WriteAsync (pingResponse);

			Assert.Equal (expectedPacket, sentPacket);
		}
	}
}