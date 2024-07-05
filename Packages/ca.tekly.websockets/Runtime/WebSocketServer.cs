using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Tekly.WebSockets
{
	public class WebSocketServer
	{
		public Clients Clients => m_clients;
		
		private TcpListener m_tcpListener;
		private Thread m_listenerThread;
		private TcpClient m_connectedClient;
		private NetworkStream m_clientStream;

		private readonly int m_port;
		private bool m_active;

		private readonly Clients m_clients = new Clients();

		public WebSocketServer(int port)
		{
			m_port = port;
		}

		public void Start()
		{
			m_active = true;

			m_listenerThread = new Thread(ListenForClients);
			m_listenerThread.IsBackground = true;
			m_listenerThread.Start();
		}

		private void ListenForClients()
		{
			m_tcpListener = new TcpListener(IPAddress.Any, m_port);
			m_tcpListener.Start();

			Debug.Log($"WebSocket server started on ws://localhost:{m_port}");

			try {
				while (m_active) {
					var client = m_tcpListener.AcceptTcpClient();
					m_clients.TryAdd(client);
				}
			} catch (ThreadAbortException) {
				// Do Nothing
			} catch (Exception exception) {
				Debug.LogException(exception);
			}
		}

		public void Stop()
		{
			m_clients.Stop();
			m_tcpListener.Stop();
			
			m_listenerThread.Abort();
			
			m_tcpListener = null;
		}
	}
}