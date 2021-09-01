using GK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class SendSetUpInfo : MonoBehaviour
{


    public int port;
    private UdpBroadcast _udp;


    void Start()
    {
        _udp = new UdpBroadcast(port);
    }

    public void send(string message)
    {
        if (_udp != null)
        {
            _udp.send(message);
        }
    }
}
