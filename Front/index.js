
var CURRENTROOM = "";
var totalSecond = 10;
var currentUser = "";
var room = document.querySelector("#room");
var rooms = document.querySelector("#rooms");
var element = document.querySelector("#offerValue");
var timeSection = document.querySelector("#time-section");
var time = document.querySelector("#time");
var button = document.querySelector("#offerBtn");



const connection = new signalR.HubConnectionBuilder()
    .withUrl("https:localhost:7264/offers")
    .configureLogging(signalR.LogLevel.Information)
    .build();


const url = "https://localhost:7264/";

startConn(connection);

async function startConn(connection){
    connection.start().then(function () {

        connection.invoke("GetRoomNames").then(function (roomNames) {
            const roomsContainer = document.getElementById("rooms");
    
            roomNames.forEach(function (roomName) {
    
                const priceSpan=document.createElement("span");
                priceSpan.style.marginLeft="10px"
                $.get(url + "Room?room=" + roomName, function (data, status) {
                    priceSpan.id=`${roomName}Price`;
                    priceSpan.innerHTML=data+"$";
                })
                connection.invoke("GetUsersCountInGroup", roomName).then(function (userCount) {
                    const button = document.createElement("button");
                    button.textContent = `Join ${roomName}`;
                    button.onclick = function () {
                        JoinRoom(roomName);
                    };
    
                    const userCountSpan = document.createElement("span");
                    userCountSpan.style.marginLeft="10px"
                    userCountSpan.id = `${roomName}Count`;
                    userCountSpan.innerText = `${userCount}/3`;
    
                    const section = document.createElement("section");
                    section.appendChild(button);
                    section.appendChild(userCountSpan);
                    section.appendChild(priceSpan);
    
                    roomsContainer.appendChild(section);
                }).catch(function (error) {
                    console.error(error);
                });
            });
        }).catch(function (error) {
            console.error(error);
        });
    }).catch(function (error) {
        console.error(error);
    });
}

async function start() {
    try {
        if (connection.state !== "Connected") {
            startConn(connection);
        }

        $.get(url + "Room?room=" + CURRENTROOM, function (data, status) {
            console.log(data);
            element.innerHTML = 'Begin Price $' + data;
        })

        console.log("SignalR Connected");
    }
    catch (err) {
        console.log(err);
        setTimeout(() => {
            start();
        }, 5000);
    }
}

connection.on("ReceiveJoinInfo", (user) => {
    let infoUser = document.querySelector("#info");
    infoUser.innerHTML = user + " connected to our room";
})


connection.on("ReceiveLeaveInfo", (user) => {
    let infoUser = document.querySelector("#info");
    infoUser.innerHTML = user + " leaved our room";
})


connection.on("ReceiveInfoRoom", (user, data) => {
    var element2 = document.querySelector("#offerValue2");
    element2.innerHTML = user + " offer this price " + (Number(data) + 100) + "$";
    button.disabled = false;
    timeSection.style.display = "none";
    clearTimeout(myInterval);
    totalSecond = 10;
})


connection.on("ReceiveWinInfoRoom", (user, data) => {
    var element2 = document.querySelector("#offerValue2");
    element2.innerHTML = user + " offer this price " + (Number(data)) + "$";
    button.disabled = true;
    timeSection.style.display = "none";
})


connection.on("ReceiveUpdatedUsersCount", (roomName, count) => {
    let current=document.querySelector(`#${roomName}Count`);
    current.innerHTML=`${count}/3`
})




async function LeaveRoom() {
    updateUsersCountInRoom(CURRENTROOM);
    room.style.display = "none";
    rooms.style.display = "block";
    await connection.invoke("LeaveRoom", CURRENTROOM, currentUser);
    let infoUser = document.querySelector("#info");
    infoUser.innerHTML = ""
    var element2 = document.querySelector("#offerValue2");
    element2.innerHTML ="";
    button.disabled = false;
    $.get(url + "Room?room=" + CURRENTROOM, function (data, status) {
        let pricespan=document.querySelector(`#${CURRENTROOM}Price`)
        pricespan.innerHTML=data+"$";
    })

}


async function updateUsersCountInRoom(roomName) {
    try {
        await connection.invoke("UpdateUsersCount", roomName);
    } catch (error) {
        console.error(error);
    }
}



async function JoinRoom(roomName) {
    CURRENTROOM = roomName;
    let current=document.querySelector(`#${roomName}Count`);
    let count=current.innerHTML.split('/')[0];
    if(count==3){
        alert("Room is full");
        return;
    }
    updateUsersCountInRoom(roomName);
    room.style.display = "block";
    rooms.style.display = "none";
    await start();
    currentUser = document.querySelector("#user").value;
    await connection.invoke("JoinRoom", CURRENTROOM, currentUser);
}

var myInterval;
async function IncreaseOffer() {
    clearTimeout(myInterval);
    timeSection.style.display = "block";
    totalSecond = 10;
    let result = document.querySelector("#user");
    var lastOffer = 0;
    $.get(url + `IncreaseRoom?room=${CURRENTROOM}&number=100`, function (data, status) {
        $.get(url + "Room?room=" + CURRENTROOM, function (data, status) {
            var element2 = document.querySelector("#offerValue2")
            element2.innerHTML = data;
            lastOffer = data;
        })
    })

    await connection.invoke("SendMessageRoom", CURRENTROOM, result.value);
    button.disabled = true;




    myInterval = setInterval(async () => {
        --totalSecond;
        time.innerHTML = totalSecond;
        console.log(totalSecond);
        if (totalSecond == 0) {
            totalSecond = 10;
            button.disabled = false;
            clearTimeout(myInterval);
            button.disabled = true;
            await connection.invoke("SendWinnerMessageRoom", CURRENTROOM, "Game Over\n" + result.value + " is Winner!!!");
        }
    }, 1000);
} 