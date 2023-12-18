using Microsoft.AspNetCore.SignalR;
using WebApiSignalR.Entities;
using WebApiSignalR.Helpers;
using WebApiSignalR.Repositories;

namespace WebApiSignalR.Hubs
{
    public class MessageHub : Hub
    {
        private readonly IRoomRepository _roomRepository;

        private readonly static Dictionary<string, List<string>> _groupUsers = new Dictionary<string, List<string>>();
        public MessageHub(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Others.SendAsync("ReceiveConnectInfo", "User Connected");
        }

        public int GetUsersCountInGroup(string groupName)
        {
            if (_groupUsers.ContainsKey(groupName))
            {
                return _groupUsers[groupName].Count;
            }
            return 0;
        }

        public IEnumerable<string> GetRoomNames()
        {
            var roomNames = _roomRepository.Getall().Select(r => r.Name);
            return roomNames;
        }

        public async Task SendMessage(string message)
        {
            await Clients.Others.SendAsync("ReceiveMessage", message + "'s Offer : ", FileHelper.Read());
        }

        public async Task SendWinnerMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveInfo", message, FileHelper.Read());
        }

        public async Task JoinRoom(string room, string user)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
            if (!_groupUsers.ContainsKey(room))
            {
                _groupUsers[room] = new List<string>();
            }
            _groupUsers[room].Add(Context.ConnectionId);
            await Clients.OthersInGroup(room).SendAsync("ReceiveJoinInfo", user);
            await UpdateUsersCount(room);
        }



        public async Task LeaveRoom(string room, string user)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);

            if (_groupUsers.ContainsKey(room))
            {
                _groupUsers[room].Remove(Context.ConnectionId);
            }
            await Clients.OthersInGroup(room).SendAsync("ReceiveLeaveInfo", user);
            await UpdateUsersCount(room);

        }

        public async Task SendMessageRoom(string room, string user)
        {
            await Clients.OthersInGroup(room).SendAsync("ReceiveInfoRoom", user, FileHelper.Read(room));
        }

        public async Task SendWinnerMessageRoom(string room, string message)
        {
            await Clients.Groups(room).SendAsync("ReceiveWinInfoRoom", message, FileHelper.Read(room));
        }


        public async Task UpdateUsersCount(string roomName)
        {
            var userCount = GetUsersCountInGroup(roomName);
            await Clients.All.SendAsync("ReceiveUpdatedUsersCount", roomName, userCount);

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
           
            var groupName = _groupUsers.FirstOrDefault(x => x.Value.Contains(Context.ConnectionId)).Key;

            if (!string.IsNullOrEmpty(groupName))
            {
                _groupUsers[groupName].Remove(Context.ConnectionId);

                await UpdateUsersCount(groupName);

                await Clients.OthersInGroup(groupName).SendAsync("ReceiveLeaveInfo", "User");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
