console.log("Hello world 3");
const WebSocketIso = require("isomorphic-ws");
const ws = new WebSocketIso("ws://tuleap.mrl.ai:8999/");
ws.binaryType = 'arraybuffer';
ws.onopen = function open() {
    console.log("connected. Woohoo");
    let loginCmd = { cmd: 2, value: { data: 0, timestamp: new Date().getTime() } };
    ws.send(JSON.stringify(loginCmd));
};
ws.onclose = function close() {
    console.log('disconnected');
};
ws.onmessage = function incoming(data) {
    console.log("Got message");
    var dataView = new DataView(data);
    var decoder = new TextDecoder('utf-8');
    var decodedString = decoder.decode(dataView);
    console.log(decodedString);
    let json = JSON.parse(decodedString);
    console.log(json);
};
//# sourceMappingURL=monitor.js.map