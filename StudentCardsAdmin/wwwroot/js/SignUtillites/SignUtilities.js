recordId = 0;
urlTransactions = "";
urlSendToBankOpenRepository = "";
urlLookDepositOpenRepository = "";
selectedSign = [];
let detailsExecutingOperation = "";
let urlRequestSendSmsForSign = "";
let urlRequestCheckSms = "";

function signSelectedDocuments(documents, gridId) {
    selectedSign = documents || [];
    serverLog = "";
    $('#ViewEnterPasswordKey').modal('hide');
    modalWindows = $('#ViewDetailsOperationSign');
    modalWindows.modal('show');
    var password = $('#passwordKey').val();
    //grid = getCurrentGrid(event, gridId).data("kendoGrid");
    urlRequest = requestSignUrl;
    urlResponse = responseSignUrl + "?id=" + selectedSign[recordIndex].Id + "&dateDocument=" + selectedSign[recordIndex].Date;
    detailsExecutingOperation = $('#detailsDocumentOperation');
    dataLog = $('#detailsOperation');
    dataLogTotal = $('#detailsOperationTotal');
    dataInput = "id=" + selectedSign[recordIndex].Id + "&password=" + encodeURIComponent(password) + "&dateDocument=" + selectedSign[recordIndex].Date;
    currentDocument = selectedSign[recordIndex].Number + ' (' + selectedSign[recordIndex].Date + ')';
    dataLogTotal.html('<div></div>');
    detailsExecutingOperation.html(currentDocument);
    recordCountTotal = selectedSign.length;
    socketWeb();
}

function operationNextSign() {
    var password = $('#passwordKey').val();
    urlRequest = requestSignUrl;
    urlResponse = responseSignUrl + "?id=" + selectedSign[recordIndex].Id + "&dateDocument=" + selectedSign[recordIndex].Date;
    dataLog = $('#detailsOperation');
    dataLogTotal = $('#detailsOperationTotal');
    dataInput = "id=" + selectedSign[recordIndex].Id + "&password=" + encodeURIComponent(password) + "&dateDocument=" + selectedSign[recordIndex].Date;
    currentDocument = selectedSign[recordIndex].Number + ' (' + selectedSign[recordIndex].Date + ')';
    dataLogTotal.html('<div></div>');
    detailsExecutingOperation.html(currentDocument);
    recordCountTotal = selectedSign.length;
    socketWeb();
}

let nextSignDocument = function () {
    serverLog = serverLog + '<div class="row m-1"><div class="col-12 text-wrap text-break">' + window.jsLocalizer.Localizer.DocumentNumber+' '
        + '<span class="text" style="color: green;">' + currentDocument + resultExecute + ' </span> ' + serverErrorDetails+'</div></div>';
    if (recordCountTotal > recordIndex + 1) {
        recordIndex++;
        operationNextSign();
    }
    else {
        detailsExecutingOperation.html('<div></div>');
        recordIndex = 0;
        recordCountTotal = 0;
        dataLog.html('<div class="row"><div class="col text-wrap text-break">' + window.jsLocalizer.Localizer.SignResult +':</div></div>'); 
        dataLogTotal.html(serverLog);
    }
}

function showEnterPasswordDialog(id, stepId, substId, substName, number, date, gridName, showSmsSendingForm, forbiddenForIso) {

    recordIndex = 0;
    showGlobalLoader();
    let messageError = "";
    selectedSign = [];
    let selected = [];

    const grid = getCurrentGrid(event, gridName).data("kendoGrid");
    console.log(grid);
    const rowUid = $(event.currentTarget).parent().attr('rowuid') || $(event.currentTarget).closest("tr").attr('data-Uid');
    const activeRow = rowUid ? $('tr[data-Uid=' + rowUid + ']') : $(event.currentTarget).closest("tr");

    if (activeRow.length === 0 && grid.select().length === 0) {
        hideGlobalLoader();
        $('#MessageBoxSignModal').modal('show');
        return;
    }

    $("#ViewEnterPasswordKey").attr('rowUid', rowUid);

    $('#MessageBoxSignModal').find("#textSignMessage").html(messageError);
    $('#ViewEnterPasswordKey').find("#textPasswordMessage").html(messageError);

    $('#MessageBoxSignModal').find("#listBoxSignDocuments").text(JSON.stringify(selectedSign));
    $('#ViewEnterPasswordKey').find("#listBoxSignDocuments").text(JSON.stringify(selectedSign));

    if (activeRow.length !== 0) {
        selected.push(grid.dataItem(activeRow));
    } else {
        grid.select().each(function () {
            selected.push(grid.dataItem(this));
        });
    }

    let totalNumber = selected.length;

    if (totalNumber > 0) {
        let messageRemovedDocumentes = '<h5 class="modal-title text-center" style="color: green;">' + window.jsLocalizer.Localizer.ExcludedDocuments +'</h5>'
        let arrayDate = "";
        for (var i = 0; i < totalNumber; i++) {
            let currentDataItem = selected[i];
            let step = currentDataItem[stepId];
            let subst = currentDataItem[substId];
            if (
                (step === 1 && subst === 1) ||
                (step === 2 && subst === 25) ||
                (step === 1 && subst === 125) ||
                (step === 1 && subst === 61) ||
                (currentDataItem[stepId] === null && currentDataItem[substId] === null) 
            ) {
                if (forbiddenForIso === "true") {
                    arrayDate = currentDataItem[date].split(".");
                    if (new Date(arrayDate[2], arrayDate[1] - 1, arrayDate[0]) >= (new Date() < new Date('2022-08-01') ? new Date('2022-07-14') : new Date('2022-07-31'))) {
                        selectedSign.push(currentDataItem);
                    }
                    else{
                        messageError = messageError + '<div class="row borderForm m-1"><div class="col text-wrap text-break">' + window.jsLocalizer.Localizer.DocumentNumber +' '
                            + '<span class="text" style="color: forestgreen;">' + currentDataItem[number] + ' (' + currentDataItem[date]
                            + ') </span>' + window.jsLocalizer.Localizer.BlockIso20022;
                        $('tr[data-uid="' + currentDataItem.uid + '"]')[0].childNodes[1].firstElementChild.checked = false;
                        $('tr[data-uid="' + currentDataItem.uid + '"]')[0].childNodes[1].firstElementChild.setAttribute("aria-checked", false);
                        $('tr[data-uid="' + currentDataItem.uid + '"]')[0].childNodes[1].firstElementChild.setAttribute("aria-label", "Select row");
                        $('tr[data-uid="' + currentDataItem.uid + '"]')[0].classList.remove("k-state-selected");

                    }
                }
                else {
                    selectedSign.push(currentDataItem);
                }
                
            }
            else {
                messageError = messageError + '<div class="row borderForm m-1"><div class="col text-wrap text-break">' + window.jsLocalizer.Localizer.DocumentNumber + ' '
                    + '<span class="text" style="color: forestgreen;">' + currentDataItem[number] + ' (' + currentDataItem[date]
                    + ') </span>' + window.jsLocalizer.Localizer.ExecludedDocumentsState
                    + '<span class="text" style="color: green;">' + currentDataItem[substName] + '</span></div></div>';
                $('tr[data-uid="' + currentDataItem.uid + '"]')[0].childNodes[1].firstElementChild.checked = false;
                $('tr[data-uid="' + currentDataItem.uid + '"]')[0].childNodes[1].firstElementChild.setAttribute("aria-checked", false);
                $('tr[data-uid="' + currentDataItem.uid + '"]')[0].childNodes[1].firstElementChild.setAttribute("aria-label", "Select row");
                $('tr[data-uid="' + currentDataItem.uid + '"]')[0].classList.remove("k-state-selected");
            }
        }

        if (messageError.length > 0) {
            messageError = messageRemovedDocumentes + messageError;
            $('#MessageBoxSignModal').find("#textSignMessage").html(messageError);
            $('#ViewEnterPasswordKey').find("#textPasswordMessage").html(messageError);
            $('#MessageBoxSignModal').find("#listBoxSignDocuments").text(JSON.stringify(selectedSign));
            hideGlobalLoader();
            if (!showSmsSendingForm)
                $('#MessageBoxSignModal').modal('show');
        }
        $("#btnStart").css("display", "");
        if (showSmsSendingForm) {
            $("#showSmsForm").css("display", "");
            $("#showKeyForm").css("display", "none");
            $("#btnStart").attr("onclick", "CheckSms(); return false;");
            $('#ViewEnterPasswordKey').find("#textErrorSendSms").text("");
            $('#ViewEnterPasswordKey').find("#textErrorSendSms").css("display", "none");
        }

        if (selectedSign.length > 0) {
            $('#ViewEnterPasswordKey').find("#listBoxSignDocuments").text(JSON.stringify(selectedSign));
            hideGlobalLoader();
            $("#ViewEnterPasswordKey").attr('docId', id);
            $("#ViewEnterPasswordKey").attr('docNumber', number);
            $("#ViewEnterPasswordKey").attr('docDate', date);
            $("#ViewEnterPasswordKey").attr('gridId', grid.element.attr('Id'));

            $('#gridDocuments').click();
            $('#IdViewEnterPasswordKey').show();
            $("#passwordKey").val("");
            $("#textSms").val("");
      
            $('#ViewEnterPasswordKey').modal('show');


            if ($('#ViewEnterPasswordKey').find("#showSmsForm").css("display") != "none") {

                 $.ajax(
                {
                    type: "POST",
                         url: urlRequestSendSmsForSign,
                    data: null
                }
            ).done(function (responce) {
                if (!responce.result){
                    $('#ViewEnterPasswordKey').find("#textErrorSendSms").text(responce.text);
                    $('#ViewEnterPasswordKey').find("#textErrorSendSms").css("display", "");
                }
            })   
            }
            return
        }
        hideGlobalLoader();
        $('#MessageBoxSignModal').modal('show');
        return;
    }

    hideGlobalLoader();
    $('#MessageBoxSignModal').modal('show');
}

function signDocuments(e) {
    var password = $('#passwordKey').val();
    if (password == '') {
        $('#textErrorSendPassword').show();
        return;
    } else {
        $('#textErrorSendPassword').hide();
    }
    let grid = getCurrentGrid(event).data("kendoGrid");
    let rowUid = $("#ViewEnterPasswordKey").attr('rowUid');
    let activeRow = rowUid ? $('tr[data-Uid=' + rowUid + ']') : null;
    const documents = [];

    if (activeRow && activeRow.length !== 0) {
        grid = getCurrentGrid(activeRow).data("kendoGrid");
        let dataActiveRow = grid.dataItem(activeRow);
        const document = {
            Id: dataActiveRow[$("#ViewEnterPasswordKey").attr('docId')],
            Date: dataActiveRow[$("#ViewEnterPasswordKey").attr('docDate')],
            Number: dataActiveRow[$("#ViewEnterPasswordKey").attr('docNumber')]
        };
        documents.push(document);
    }
    else {
        grid.select().each(function () {
            let item = grid.dataItem(this);
            const document = {
                Id: item[$("#ViewEnterPasswordKey").attr('docId')],
                Date: item[$("#ViewEnterPasswordKey").attr('docDate')],
                Number: item[$("#ViewEnterPasswordKey").attr('docNumber')]
            };

            documents.push(document);
        });
    }

    signSelectedDocuments(documents, $("#ViewEnterPasswordKey").attr('gridId'));
}

function error_handler(e) {
    if (e.errors) {
        var message = "Errors:\n";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });
        alert(message);
    }
}

function dataBoundHandler(arg) {
    $(".btn-success").removeClass("k-button");
    $(".btn-danger").removeClass("k-button");
    $(".btn-orange").removeClass("k-button");
    $(".btn-dark-green").removeClass("k-button");
    kendoConsole.log("Grid data bound");
}

function CheckSms() {

    let text = String($("#textSms").val());
    $.ajax(
        {
            type: "POST",
            url: urlRequestCheckSms,
            data: { textSms: text}
        }
    ).done(function (responce) {
        if (responce.result) {
            $("#showSmsForm").css("display", "none");
            $("#showKeyForm").css("display", "");
            $("#btnStart").attr("onclick", "signDocuments(); return false;");
        }
        else {
            $('#ViewEnterPasswordKey').find("#textErrorSendSms").text(responce.text);
            $('#ViewEnterPasswordKey').find("#textErrorSendSms").css("display", "");
            if (!responce.result && responce.text === window.jsLocalizer.Localizer.CodeExpired)
              $("#btnStart").css("display", "none");

        }
    })

    
}