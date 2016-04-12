using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using log4net;

namespace SERemoteConnection
{
    //public abstract class ISEConnection
    //{
    //    public abstract void OnTelegram(byte[] telegram);
    //}
    public delegate void OnTelegramDelegate(byte[] telegram);


    public class SEConnection : IDisposable
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SEConnection));

        private Socket m_socket;
        private System.Threading.Thread m_thread;
        private bool m_isActive;
        private System.IO.MemoryStream m_stream;

        public OnTelegramDelegate OnTelegram { set; get; }


        public SEConnection()
        {
        }

        ~SEConnection()
        {
            if (m_socket.Connected)
            {
                Close();
            }
        }

        public void Dispose()
        {
            // What the f**, call dispose on the context and any of its members here
            this.m_socket.Dispose();
            this.m_stream.Dispose();
        }

        public bool Open(string host, int port)
        {
            IPAddress[] IPs = Dns.GetHostAddresses(host);

            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                m_socket.Connect(IPs[0], port);

                m_isActive = m_socket.Connected;

                if (m_isActive)
                {
                    m_stream = new System.IO.MemoryStream();

                    m_thread = new System.Threading.Thread(Run);
                    m_thread.Start();
                }
            }
            catch (Exception)
            {
                m_isActive = false;
            }
            return m_isActive;
        }


        public void Close()
        {
            m_isActive = false;
            m_socket.Close();
            m_thread.Join();
        }

        private void Run()
        {
            // Buffer to read data
            var buffer = new byte[m_socket.ReceiveBufferSize];

            while (m_socket.Connected && m_socket.Poll(-1, SelectMode.SelectRead))
            {
                if (m_socket.Connected)
                {
                    // There is data waiting to be read"
                    int readCount = m_socket.Receive(buffer);

                    if (readCount > 0)
                    {
                        Process(buffer, readCount);
                    }
                }
                else
                {
                    // Something bad has happened, shut down
                }
            }
        }


        public void Write(Byte[] buffer)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            string dump = utf8.GetString(buffer);
            logger.DebugFormat("Write: {0}", dump);

            byte[] sendTerminater = Encoding.ASCII.GetBytes("\r\n");

            m_socket.Send(buffer);
            m_socket.Send(sendTerminater);
        }

        private bool Process(byte[] buffer, int length)
        {
            for (var i = 0; i < length; ++i)
            {
                if (buffer[i] == '\r')
                {
                    ++i;
                    if (i < length)
                    {
                        if (buffer[i] == '\n')
                        {
                            if (OnTelegram != null && m_stream.Capacity>0)
                            {
                                OnTelegram(m_stream.ToArray());
                            }
                            m_stream = new System.IO.MemoryStream();
                        }
                        else
                        {
                            m_stream.WriteByte(buffer[i]);
                        }
                    }
                }
                else
                {
                    m_stream.WriteByte(buffer[i]);
                }
            }
            return true;
        }
    }
}
