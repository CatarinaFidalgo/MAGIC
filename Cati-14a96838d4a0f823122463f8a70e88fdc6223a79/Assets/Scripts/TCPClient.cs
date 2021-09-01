using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPClient : MonoBehaviour
{
	#region private members 	
	private TcpClient socketConnection;
	private Thread clientReceiveThread;
    #endregion
    public const int BUFF = 65000;

    public int serverPort;
	public string serverAddress;

    public bool connected;

	public ReceiveTrail receiveTrail;
    public StartEndLogs startEnd;
    public Evaluation evaluation;

	// Use this for initialization 	
	public void StartClient()
	{
		ConnectToTcpServer();
	}
	// Update is called once per frame
	void Update()
	{
        if (socketConnection == null) connected = false;
        else connected = socketConnection.Connected;

    }
	/// <summary> 	
	/// Setup socket connection. 	
	/// </summary> 	
	private void ConnectToTcpServer()
	{
		try
		{
			clientReceiveThread = new Thread(new ThreadStart(ListenForData));
			clientReceiveThread.IsBackground = true;
			clientReceiveThread.Start();
		}
		catch (Exception e)
		{
			Debug.Log("On client connect exception " + e);
		}
	}
	/// <summary> 	
	/// Runs in background clientReceiveThread; Listens for incomming data. 	
	/// </summary>     
	private void ListenForData()
	{
		try
		{
			socketConnection = new TcpClient(serverAddress, serverPort);
			Byte[] bytes = new Byte[BUFF];
			while (true)
			{
				// Get a stream object for reading 				
				using (NetworkStream stream = socketConnection.GetStream())
				{
					int length;
					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
					{
						var incommingData = new byte[length];
						Array.Copy(bytes, 0, incommingData, 0, length);
						// Convert byte array to string message. 						
						string serverMessage = Encoding.ASCII.GetString(incommingData);
						Debug.Log("server message received as: " + serverMessage);                        
						receiveTrail.newTrailMessage(serverMessage);

                        if (serverMessage.Split('#')[0] == "update")
                        {
                            startEnd.showWorkspace = Convert.ToBoolean(serverMessage.Split('#')[1]);
                            Debug.Log("Show Workspace");
                        }

                        if (serverMessage.Split('#')[0] == "demonstrator")
                        {
                            evaluation.localIsDemonstrator = Convert.ToBoolean(serverMessage.Split('#')[1]);
                            startEnd.changeCoordinator = true;
                            
                            Debug.Log("Become the DEMONSTRATOOOOR");
                        }

                    }
				}
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}
	/// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	
	public void SendAVeryImportantMessage(string message)
	{
		if (socketConnection == null)
		{
			return;
		}
		try
		{
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream();
			if (stream.CanWrite)
			{
				string clientMessage = message;
				// Convert string message to byte array.                 
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
				// Write byte array to socketConnection stream.                 
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
				Debug.Log("Client sent his message - should be received by server - Nr. characters: " + message.Length + " Message: " + message);
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}
}