using Tekly.WebSockets.Core;

namespace Tekly.WebSockets.Channeled
{
	public class Subscription
	{
		public readonly string SessionId;
		private readonly Channels m_channels;

		private readonly Client m_client;
		private readonly Channel m_channel;

		private readonly FrameEncoding m_frameEncoding = new FrameEncoding();

		public Subscription(Client client, Channel channel, string sessionId, Channels channels)
		{
			m_client = client;
			m_channel = channel;
			SessionId = sessionId;
			m_channels = channels;

			m_channel.Messaged += Message;
			m_channel.Subscribe(this);
		}

		public void Close()
		{
			m_channel.Messaged -= Message;
		}

		public void Message(ChannelFrameEvt evt)
		{
			var frameData = m_frameEncoding.Encode(evt.Command, SessionId, m_channel.Id, evt.ContentType, evt.Content);
			m_client.Send(frameData);
		}
		
		public void Message<T>(T value)
		{
			var json = m_channels.Serialize(value);
			var frameData = m_frameEncoding.Encode(FrameCommands.MESSAGE, SessionId, m_channel.Id, "json", json);
			m_client.Send(frameData);
		}
	}
}