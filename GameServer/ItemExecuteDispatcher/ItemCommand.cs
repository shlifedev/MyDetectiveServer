﻿
using GameServer.Struct;

namespace GameServer.ItemExecuteDispatcher
{
    public abstract class ItemCommand : IItemCommand
    {
        
        public GameRoom Room
        {
            get
            {
                return executeManager.room;
            }
        }
        public ItemCommand(ItemExecuteDispatcher.ItemExecuteManager executer)
        {
            this.executeManager = executer;
        }
        public ItemExecuteDispatcher.ItemExecuteManager executeManager;
        public abstract void Execute(Player usePlayer, NEntity targetEntity, Item useItem);
        public abstract bool Executeable(Player usePlayer, NEntity targetEntity, Item useItem);
        public void NotifyBuffGive()
        {

        }
    }
}
