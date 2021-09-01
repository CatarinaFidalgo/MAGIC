using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPServer : MonoBehaviour
{
	#region private members 	
	/// <summary> 	
	/// TCPListener to listen for incomming TCP connection 	
	/// requests. 	
	/// </summary> 	
	private TcpListener tcpListener;
	/// <summary> 
	/// Background thread for TcpServer workload. 	
	/// </summary> 	
	private Thread tcpListenerThread;
	/// <summary> 	
	/// Create handle to connected tcp client. 	
	/// </summary> 	
	private TcpClient connectedTcpClient;
    #endregion

    public const int BUFF = 65000;

	public int serverPort;
	public string serverAddress;

	public ReceiveTrail receiveTrail;
    public StartEndLogs startEnd;
    public Evaluation evaluation;

    // Use this for initialization
    public void StartServer()
	{
		// Start TcpServer background thread 		
		tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
		tcpListenerThread.IsBackground = true;
		tcpListenerThread.Start();
	}

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary> 	
	/// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
	/// </summary> 	
	private void ListenForIncommingRequests()
	{
		try
		{
			// Create listener on localhost port 8052. 			
			tcpListener = new TcpListener(IPAddress.Parse(serverAddress), serverPort);
			tcpListener.Start();
			Debug.Log("[NETWORK] Server is listening in " + serverAddress + ":" + serverPort);
			Byte[] bytes = new Byte[BUFF];
			while (true)
			{
				using (connectedTcpClient = tcpListener.AcceptTcpClient())
				{
					Debug.Log("[NETWORK] NEW CLINET");
					// Get a stream object for reading 					
					using (NetworkStream stream = connectedTcpClient.GetStream())
					{
						int length;
						// Read incomming stream into byte arrary. 						
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
						{
							var incommingData = new byte[length];
							Array.Copy(bytes, 0, incommingData, 0, length);
							// Convert byte array to string message. 							
							string clientMessage = Encoding.ASCII.GetString(incommingData);
							Debug.Log("client message received with: " + clientMessage.Length + " as: " + clientMessage);
							receiveTrail.newTrailMessage(clientMessage);

                            if (clientMessage.Split('#')[0] == "update")
                            {
                                startEnd.showWorkspace = Convert.ToBoolean(clientMessage.Split('#')[1]);
                            }

                            if (clientMessage.Split('#')[0] == "demonstrator")
                            {
                                evaluation.localIsDemonstrator = Convert.ToBoolean(clientMessage.Split('#')[1]);
                                Debug.Log("Become the DEMONSTRATOOOOR");
                                startEnd.changeCoordinator = true;
                            }

                        }
					}
				}
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("SocketException " + socketException.ToString());
		}
	}
	/// <summary> 	
	/// Send message to client using socket connection. 	
	/// </summary> 	
	public void SendAVeryImportantMessage(string message)
	{
		if (connectedTcpClient == null)
		{
			return;
		}

		try
		{
			// Get a stream object for writing. 			
			NetworkStream stream = connectedTcpClient.GetStream();
			if (stream.CanWrite)
			{
				string serverMessage = message;
				// Convert string message to byte array.                 
				byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
				// Write byte array to socketConnection stream.               
				stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                Debug.Log("Server sent his message - should be received by server - Nr. characters: " + message.Length + " Message: " + message);
            }
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}
}