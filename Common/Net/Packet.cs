﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Voicer.Common.Data;

namespace Voicer.Common.Net
{
    public class Packet : IDisposable
    {
        public enum Messages : short
        {
            // Client
            CONNECT = 1,
            KEEPALIVE,
            DISCONNECT,
            RECIEVED,

            // Server
            CONNECTED,
            GETUSERS,
            SHUTDOWN,
            SWAPCHANNEL, // Server to notify other users of user swapping channel

            // Shared
            CHAT, //Send/Recieve chat message
            VOICE, //Send/Recieve voice packet
            JOINCHANNEL, // User requests to join channel, server to notify user swapped channel
            CONNECTCHANNEL,
            MOVE, //Move a user from channel to channel (forced by a second user)
            BAN, //Ban a user from the server
            KICK, // kick user from the server
            SETKEY,
            NEWKEY,
            GETKEY,
            SERVERMESSAGE,
            SETADMIN,
        };

        private IPEndPoint sender;
        public IPEndPoint Sender
        {
            get
            { return sender; }
            private set
            { sender = value; }
        }

        private byte[] data;
        public byte[] Data
        {
            get
            { return data; }
            private set
            { data = value; }
        }

        private Messages type;
        public Messages Type
        {
            get
            { return type; }
            private set
            { type = value; }
        }

        public Packet(Messages type, byte[] data = null)
        {
            this.type = type;
            this.data = data;
        }

        public Packet(byte[] data, IPEndPoint endP)
        {
            sender = endP;
            type = (Messages)BitConverter.ToInt16(data, 0);
            if (data.Length > 2)
                this.data = data.Skip(2).ToArray();
            else this.data = null;
        }

        public virtual byte[] Encode()
        {
            byte[] buffer = BitConverter.GetBytes((short)type);
            if (data != null)
                buffer = buffer.Concat(data).ToArray();

            return buffer;
        }

        public void Dispose()
        {
            data = null;
            type = 0;
            sender = null;
        }
    }
}
