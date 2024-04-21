let urlRequestKey = "";
let urlResponseKey = "";
let urlLoginKey = "";
let dataLogKey = "";
let dataResultKey = "";
let socketKey;
let dataInputKey = "";
let dataInputAnswerKey = "";


let socketWebKey = function () {
    socketKey = new WebSocket("ws://localhost:37770/xml");
    //socketKey = new WebSocket("wss://wss.int.bb.asb:37443/xml");
    socketKey.onopen = socketKeyOnopen;
    socketKey.onerror = socketKeyError;
    socketKey.onclose = socketKeyClose;
    socketKey.onmessage = socketKeyMessage;
}

let socketKeyOnopen = function (e) {
    dataLogKey.append('<div class="row px-1">Соединение c ssf_server установлено</div>');
    $.ajax({
        url: urlRequestKey,
        type: 'GET',
        data: dataInputKey,
        cache: false
    }).done(function (data) {
        dataLogKey.append('<div class="row px-1">Данные для передачи на ssf_server:</div>');
        dataLogKey.append('<div class="row px-1"><div class="text-break">' + data + '</div></div>');
       
        socketKey.send(data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        dataLogKey.append('<div class="row px-1">[ERROR]' + jqXHR.statusText + textStatus + errorThrown + '</div>');
    });
}

let socketKeyError = function (error) {
    dataLogKey.append(error.message);
};

let socketKeyClose = function (event) {
    if (event.wasClean) {
        dataLogKey.append('<div class="row px-1">[close] Соединение закрыто чисто, код=' + event.code + ' причина = ' + event.reason + '</div>');
    } else {
        // например, сервер убил процесс или сеть недоступна
        // обычно в этом случае event.code 1006
        dataLogKey.append('<div class="row px-1">[close] Соединение прервано код=' + event.code + ' причина=' + event.reason + '</div>');
        if (event.code == "1006")
            setTimeout(socketWebKey, 100);
    }
};

let socketKeyMessage = function (event) {
    dataLogKey.append('<div class="row px-1">Данные получены с ssf_server:</div>');
    dataLogKey.append('<div class="row px-1"><div class="text-break">' + event.data + '</div></div>');
    dataLogKey.append('<div class="row px-1">Результат обработки данных:</div>');
    
    $.ajax({
        url: urlResponseKey,
        type: 'GET',
        //dataType: 'json',
        data: 'data=' + encodeURIComponent(event.data) + dataInputAnswerKey,
        cache: false
    }).done(function (data) {
        //console.log("SSF: " + data)
        //console.log("URL: " + urlLoginKey)
       if (data == "OK") {
           window.location.href = urlLoginKey;
           console.log("urlLoginKey: " + urlLoginKey);
           console.log("urlResponseKey: " + urlResponseKey);
        }
        else
        { 
            dataResultKey.html(data);
            dataLogKey.append('<div class="row px-1"><div class="text-break">' + data + '</div></div>');
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        dataLogKey.append('<div class="row px-1">[ERROR] ' + jqXHR.status + ' ' + jqXHR.responseText + '</div>');
        console.log('<div class="row px-1">[ERROR] ' + jqXHR.status + ' ' + jqXHR.responseText + '</div>');
        dataResultKey.html(jqXHR.responseText);
    });
};
