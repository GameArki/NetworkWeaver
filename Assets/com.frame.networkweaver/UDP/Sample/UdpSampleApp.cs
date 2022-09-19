using System;
using UnityEngine;
using JackFrame.Network;
using JackBuffer;

namespace MySampleApp {

    public class UdpSampleApp : MonoBehaviour {

        struct UDPReqMessage : IJackMessage<UDPReqMessage> {
            void IJackMessage<UDPReqMessage>.FromBytes(byte[] src, ref int offset) {

            }

            int IJackMessage<UDPReqMessage>.GetEvaluatedSize(out bool isCertain) {
                isCertain = true;
                return 0;
            }

            byte[] IJackMessage<UDPReqMessage>.ToBytes() {
                return new byte[2];
            }

            void IJackMessage<UDPReqMessage>.WriteTo(byte[] dst, ref int offset) {

            }
        }

        struct UDPResMessage : IJackMessage<UDPResMessage> {
            void IJackMessage<UDPResMessage>.FromBytes(byte[] src, ref int offset) {
            }

            int IJackMessage<UDPResMessage>.GetEvaluatedSize(out bool isCertain) {
                isCertain = true;
                return 0;
            }

            byte[] IJackMessage<UDPResMessage>.ToBytes() {
                return new byte[2];
            }

            void IJackMessage<UDPResMessage>.WriteTo(byte[] dst, ref int offset) {
            }
        }

        UdpClient client;
        UdpServer server;

        void Awake() {

            server = new UdpServer();
            server.OnConnectedHandle += (connID) => {
                Debug.Log("SERVER CONN: " + connID);
            };
            server.OnDisconnectedHandle += (connID) => {
                Debug.Log("SERVER DISCONN: " + connID);
            };
            server.On(0, 0, () => new UDPReqMessage(), (connID, msg) => {
                Debug.Log("SERVER RECV: " + connID);
                server.Send(0, 1, connID, new UDPResMessage());
            });
            server.StartListen(5000);

            client = new UdpClient();
            client.OnConnectedHandle += () => {
                Debug.Log("CLIENT CONN");
                client.Send(0, 0, new UDPReqMessage());
            };
            client.OnDisconnectedHandle += () => {
                Debug.Log("CLIENT DISCONN");
            };
            client.On(0, 1, () => new UDPResMessage(), (msg) => {
                Debug.Log("CLIENT RECV");
            });
            client.Connect("localhost", 5000);

        }

        void Update() {
            server.Tick();
            client.Tick();
        }

    }

}