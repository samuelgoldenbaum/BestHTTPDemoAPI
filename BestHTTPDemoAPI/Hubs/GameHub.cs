using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using MessagePack;

namespace BestHTTPDemoAPI {
    [Serializable]
    [MessagePackObject (true)]
    public class Move {
        public int RollNumber;
        public int Roll;
        public int From;
        public int To;
    }

    [Serializable]
    public enum TurnType {
        Manual,
        Automatic
    }

    [Serializable]
    public enum ActionType {
        Start,
        Roll,
        Move,
        Double,
        Resign,
        Forfit,
        End,
        AcceptDouble
    }

    [Serializable]
    public enum PlayerNumber {
        One,
        Two
    }

    [Serializable]
    [MessagePackObject (keyAsPropertyName: true)]
    public class Foo {
        public string Bar;
    }

    [Serializable]
    [MessagePackObject (keyAsPropertyName: true)]
    public class TurnAction {
        public int TurnNumber;
        public PlayerNumber PlayerNumber;
        public ActionType ActionType;
        public Dictionary<string, object> Data;

        public List<Move> Moves;
        // public DateTime Time;
    }

    public class GameHub : Hub {
        public override async Task OnConnectedAsync () {
            await Clients.All.SendAsync ("Send", $"{Context.ConnectionId} joined");
        }

        public async Task JoinGroup (string groupName) {
            await Groups.AddToGroupAsync (Context.ConnectionId, groupName);

            await Clients.Group (groupName).SendAsync ("Send", $"{Context.ConnectionId} joined {groupName}");
        }

        public async Task TurnAction (TurnAction turnAction) {
            await Clients.Group ("game").SendAsync ("TurnAction", turnAction);
        }
    }
}