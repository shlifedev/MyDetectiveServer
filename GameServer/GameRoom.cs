﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Struct;
using UnityEngine;
using Nettention.Proud;
using HID = Nettention.Proud.HostID;
using RMI = Nettention.Proud.RmiContext;
using Vector3 = UnityEngine.Vector3;

namespace GameServer
{
    
    public class GameRoom
    {

        public Players players;
        public ItemExecuteDispatcher.ItemExecuteManager itemExecuteManager;
        public GameRoom(GameRoomServer server, Server gameServer)
        {
            this.srv = server;
            this.gSrv = gameServer;
            this.srv.room = this;
            itemExecuteManager = new ItemExecuteDispatcher.ItemExecuteManager(this);
            players = new Players(); 
            players.room = this; 
            entityManager = new NEntityManager(this);
            //임시로 스텁처리
            srv.c2sStub.ReqMove += OnReqEnityMove;
            srv.c2sStub.ReqUseItem += players.OnStubItemUseReq;
            srv.StartServer();
            gameManager = new GameManager(this); 
            Logger.Log(this, "GameRoom Setting Succesfully");
            this.CreateNPCEntity(new Vector2(-3, 0));
        }
        public Server gSrv = null;

        public int identifier = 0;
        public NEntityManager entityManager;
        public List<HID> connectedHosts = new List<HID>();
        public GameRoomServer srv = new GameRoomServer();
        public GameManager gameManager;
        public HID[] GetOthers(HID ignore)
        {
            int cur = 0;
            HID[] hids = new HID[connectedHosts.Count - 1];
            for (int i = 0; i < connectedHosts.Count; i++)
            {
                if (connectedHosts[i] != ignore)
                {
                    hids[cur] = connectedHosts[i];
                    cur++;
                }
            }
            return hids;
        }
        public bool OnReqEnityMove(HID requester, RMI rmi, int entityId, UnityEngine.Vector3 pos, Vector3 vel)
        { 
            var entity = entityManager.entityMap[entityId];

            if (entity != null)
            {
                entity.position = pos; 
                foreach (var data in GetOthers(requester))
                { 
                    srv.s2cProxy.NotifyEntityMove(data, RMI.ReliableSend, entityId, pos, vel);
                }
            }
            else
            {
                Logger.Error(this, $"entity {entityId} not found..");
            }
            return true;
        }
        public void JoinClient(HID hostID)
        {

            connectedHosts.Add(hostID);
            var playerEntity = CreatePlayerEntity(new Vector2(0, 1), hostID);
            players.AddPlayer(playerEntity, hostID,  this);
            foreach (var hid in connectedHosts)
            { 
                srv.s2cProxy.NotifyServerMessage(hid, RMI.ReliableSend, $"Player {hostID} Connected.");
                srv.s2cProxy.NotifyPlayerCreate(hid, RMI.ReliableSend, playerEntity.ownerHostID, playerEntity.entityIndex, playerEntity.position);
              
            }
            srv.s2cProxy.NotifyNPCList(hostID, RMI.ReliableSend, new NEntityList()
            {
                count = entityManager.npcList.Count,
                list = entityManager.npcList
            }); 
            srv.s2cProxy.NotifyJoinPlayer(hostID, RMI.ReliableSend, new NEntityList
            {
                list = entityManager.playerList,
                count = entityManager.playerList.Count
            });
        }
        public void LeaveClient(HID hostID)
        { 
            connectedHosts.Remove(hostID);
            var player = players.GetPlayerByEntityId((int)hostID); 
            players.RemovePlayer(hostID);
            var entity = entityManager.entitiList.Find(x => x.ownerHostID == (int)hostID);
            if (entity != null)
            { 
                entityManager.RemoveEntity(entity.entityIndex);
            }
            foreach (var hid in connectedHosts)
            {
                srv.s2cProxy.NotifyServerMessage(hid, RMI.ReliableSend, $"Player {hostID} Leave. ");
            }
        }
        public void ShowOnlinePlayer()
        {
            string v = null;
            foreach (var hid in connectedHosts)
            {
                v += hid + " ";
            }
            v += $"({connectedHosts.Count}onlines)";
            Console.WriteLine("onlines : " + v);
        }
        public int CreateIdentifier()
        {
            identifier++;
            return identifier;
        }

        public void StartGame()
        { 
            
            //플레이어 아이템설정
            gameManager.SettingPlayerStartItems();
            //플레이어 랜덤킬러 설정
            gameManager.SetRandomKiller();
        }
        public void EndGame()
        {

        }
        public NEntity CreatePlayerEntity(Vector2 position, HID ownerID)
        {
            NEntity playerEntity = entityManager.CreatePlayerEntity(position, ownerID);
            for (int i = 0; i < connectedHosts.Count; i++)
            {
                srv.s2cProxy.NotifyServerMessage(connectedHosts[i], RMI.ReliableSend, $"[Log]Player {ownerID} Created!");
            }
            return playerEntity;
        }
        public NEntity CreateNPCEntity(Vector2 position)
        {
            NEntity playerEntity = entityManager.CreateNPCEntity(-1, position);
            for (int i = 0; i < connectedHosts.Count; i++)
            {
                srv.s2cProxy.NotifyServerMessage(connectedHosts[i], RMI.ReliableSend, $"[Log]NPC ? Created! (pos:{position})");
            }
            return playerEntity;
        } 
        public NItemEntity CreateItemEntity(int itemIndex, Vector2 position)
        {
            NItemEntity createEntity = entityManager.CreateItemEntity(itemIndex, position);
            for (int i = 0; i < connectedHosts.Count; i++)
            {
                srv.s2cProxy.NotifyItemCreate(connectedHosts[i], RMI.ReliableSend, createEntity.entityIndex, createEntity.itemIndex, position);
                Console.WriteLine("send to " + connectedHosts[i]);
            }
            return createEntity;
        }
    }
}
