let urlRequest = "";
let urlResponse = "";
let dataLog = "";
let dataLogTotal = "";
let dataMessage = "";
let serverLog = "";
let socket;
let dataInput = "";
let currentDocument = "";
let modalWindows = "";
let grid = "";
let recordCountTotal = 0;
let recordIndex = 0;
let serverErrorDetails = "";
let resultExecute = "";


let socketWeb = function () {
    if (dataLog != undefined)
        dataLog.html('<div class="row"><div class="col text-wrap text-break">Соединение с модулем криптографии</div></div>');
    if (socket != null && socket.readyState == 1) {
        socketSend()
    }
    else {
        socket = new WebSocket("ws://localhost:37770/xml");
        //socket = new WebSocket("wss://wss.int.bb.asb:37443/xml");
        socket.onopen = function (evt) {
            socketSend()
        };
        socket.onerror = socketError;
        socket.onclose = function (evt) {
            socketClose(evt)
        };
        socket.onmessage = socketMessage;
    }
}


function socketSend() {
    if (dataLog != undefined)
        dataLog.html('<div class="row"><div class="col text-wrap text-break">Получение данных для подписи</div></div>');
    $.ajax({
        url: urlRequest,
        type: 'GET',
        data: dataInput
    }).done(function (data) {
        if (dataLog != undefined)
            dataLog.html('<div class="row"><div class="col text-wrap text-break">Подпись данных</div></div>');
        console.log(socket);
        socket.send(data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        if (dataLog != undefined) {
            dataLog.html('<div class="row "><div class="col text-wrap  text-break">' + jqXHR.responseText + '</div></div>');
            socket.close();
        }
    });
}

let socketError = function (error) {
    //dataLog.append(error.message);
    console.log('Socket Error: ' + error.message);
};

let socketClose = function (event) {
    if (event.wasClean) {
        //dataLog.append('<div class="row px-1">[close] Соединение закрыто чисто, код=' + event.code + ' причина = ' + event.reason + '</div>');
        console.log('Socket Closed: ' + 'Соединение закрыто чисто, код = ' + event.code + ' причина = ' + event.reason);
    } else {
        console.log('Socket Closed: ' + 'Обрыв соединения, код = ' + event.code + ' причина = ' + event.reason);
        if (
            event.code == "1006" ||
            event.code == "1015") {
            //var sslProtocolHack = (System.Security.Authentication.SslProtocols)(SslProtocolsHack.3072 | SslProtocolsHack.768 | SslProtocolsHack.192);
            //socket.SslConfiguration.EnabledSslProtocols = sslProtocolHack;
            setTimeout(socketWeb, 100);
        }
        else {
            socket.close();
        }
    }
};

let socketMessage = function (event) {
    if (dataLog != undefined)
        dataLog.html('<div class="row"><div class="col text-wrap text-break">Передача подписанных данных</div></div>');
    console.log(escape(event))
    $.ajax({
        url: urlResponse,
        type: 'POST',
        data: { data: encodeURIComponent(event.data) }
    }).done(function (data) {
        resultExecute = ' -> подписан';
        serverErrorDetails = "";
        if (recordCountTotal == recordIndex || nextSignDocument == undefined) {
            if (modalWindows != undefined)
                modalWindows.modal('hide');
            if (grid != undefined) {
                grid.dataSource.read();
            }
        }
        else {
            if (nextSignDocument != undefined) {
                nextSignDocument();
            }
            else {
                socket.close();
            }
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        resultExecute = ' -> Ошибка выработки ЭЦП:';
        serverErrorDetails = jqXHR.responseText;
        if (dataLog != undefined) {
            dataLog.html('<div class="row "><div class="col text-wrap  text-break">' + jqXHR.responseText + '</div></div>');
            if (nextSignDocument != undefined) {
                dataMessage = '<div class="row "><div class="col text-wrap  text-break">' + jqXHR.responseText + '</div></div>';
                nextSignDocument();
            }
            else {
                socket.close();
            }
        }
    });
};