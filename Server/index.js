const WebSocket = require('ws');

const wss = new WebSocket.Server({ port: 8080 });

wss.on('connection', function connection(ws) {
    ws.on('message', function incoming(message) {
        console.log('received: %s', message);
    });

    console.log("CONNECTION");
});

wss.broadcast = function broadcast(data) {
    wss.clients.forEach(function each(client) {
        if (client.readyState === WebSocket.OPEN) {
            client.send(data);
        }
    });
};

//

const socket = require('socket.io-client')('ws://heat-ebs.j38.net');
//const socket = require('socket.io-client')('ws://localhost:3000/');
socket.on('connect', function () {
    console.log("CONNECTED");
    wss.broadcast("CONNECTED");
    socket.emit("channel", "97032862");
});

socket.on('click', function (data) {
    console.log(data);
    wss.broadcast(data);
});
socket.on('disconnect', function () { });
