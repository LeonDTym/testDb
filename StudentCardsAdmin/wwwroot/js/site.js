// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(".getPhoto").click(function () {
    var id = $(this).attr("id");
    $.ajax({
        html: true,
        url: "/Home/PhotoStudent",
        type: "GET",
        data: { "id": id },
        contentType: "application/json",
        success: function (result) {
            var html = '';
            var html2 = '';
            html += '<img id="treeViewItems" style="width: 224px; height: 298px;" src="data:image/jpeg;base64,' + result[0].photo + '" />'
            if (result[0].unique_id != 0) {
                html2 += 'Фото ' + result[0].unique_id + '';
            } else {
                html2 += 'Фото';
            }
            $('#photka').html(html);
            $('#ModalPhotoOpenlLongTitle').html(html2);
           
        }
    })
})

$(".getTransferdStud").click(function () {
    $.ajax({
        method: "GET",
        url: "/Home/TransferdStud",
        dataType: "json",
        contentType: "application/json",
        data: {
            'unique_id': $('#uniqueID').val(),
            'data': $('#date').val()
        },
        success: function (result) {
            $('#kidSurname').val(result[0].kid_surname);
            $('#kidName').val(result[0].kid_name);
            $('#kidPatronymic').val(result[0].kid_patronymic);
            $('#dateBirthday').val(result[0].year_birthday);
            $('#kidClass').val(result[0].kid_class);
            $('#endTraining').val(result[0].end_training);
            $('#schoolNameShort').html(result[0].school_name_short);
        /*    $('#uniqueIDStr').html(result[0].unique_id);*/
            $("#submitTransfer").attr("hidden", false)
        },
        error: function (er) {
            /*console.log(er);*/
        }
    });
})

$(".getSchoolEdit").click(function () {
    var id = $(this).attr("id");
    $.ajax({
        method: "GET",
        url: "/Home/EditableSchool",
        dataType: "json",
        contentType: "application/json",
        data: { 'id': id },
        success: function (result) {
            $('#id_edit').val(result[0].id);
            $('#unn_edit').val(result[0].unn);
            $('#cbu_edit').val(result[0].cbu);
            $('#school_name_edit').val(result[0].school_name);
            $('#school_name_short_edit').val(result[0].school_name_short);
            $('#school_name_card_edit').val(result[0].school_name_card);
            $('#school_address_edit').val(result[0].school_address);
            $('#school_phone_edit').val(result[0].school_phone);
            $('#region_edit').val(result[0].region);
            $('#district_edit').val(result[0].district);
            $('#email_edit').val(result[0].email);
            $('#locality_edit').val(result[0].locality);

            /*    $('#uniqueIDStr').html(result[0].unique_id);*/


        },
        error: function (er) {
            console.log(er);
        }
    });
})

$(".SchoolDel").click(function () {
    var id = $(this).attr("id");
    $.ajax({
        method: "GET",
        url: "/Home/DelSchool",
        dataType: "json",
        contentType: "application/json",
        data: { 'id': id },
        success: function () {
            var html = '';
            document.getElementById('loadSCSchool').click();
        /*   *//* location.reload();*/
                $("#partialSCSchool").load('@Url.Content("/Home/SCSchool")', function () {
                });
            '<div id="showMessageFail" style="position:absolute; z-index:999999;left:30%;top:1%" class="alert alert-danger alert-dismissible fade show" role="alert">' +
                '        <button type="button" style="margin-top:5px" class="close" data-dismiss="alert" aria-label="Close">' +
                '<span aria-hidden="false">&times;</span>' +
                '</button>' +
                '<h4>uiuiuui</h4></div>';
            $('#showMessageSuccess').html(html);
        },
        error: function (er) {
            console.log(er);
            document.getElementById('loadSCSchool').click();
        }
    });
})

function selectSchool(id) {
    console.log('Yes');
    sessionStorage.setItem("SelectedSchool", id);
    $.ajax({
        html: true,
        url: "/Home/ChangeSchool",
        type: "GET",
        data: { "id": id },
        contentType: "application/json",
        success: function (result) {
            location.reload();
        }
    })
}

$(".actSet").click(function () {
    var inps = document.getElementsByName('chbox[]');
    /* var tovar = document.getElementsByName('tovar[]');*/
    var set = [];
    for (var i = 0; i < inps.length; i++) {
        var inp = inps[i];
        /*   var tov = tovar[i];*/
        if (inp.checked == true) {
            set.push(inp.id);
        }
    }
    /*  document.getElementById("div1").innerHTML = set;*/
    $.ajax({
        html: true,
        url: "/Home/SetAct",
        type: "GET",
        data: { "set": JSON.stringify(set) },
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            location.reload();

        }
    })
})

$(".getStudEdit").click(function () {
    var id = $(this).attr("id");
    $.ajax({
        method: "GET",
        url: "/Home/_EditKid",
        dataType: "json",
        contentType: "application/json",
        data: { 'id': id },
        success: function (result) {
            $('#IdStud').val(result[0].id);
            $('#UniqueIdEdit').val(result[0].unique_id);
            $('#SNameEdit').val(result[0].kid_surname);
            $('#NameEdit').val(result[0].kid_name);
            $('#TNameEdit').val(result[0].kid_patronymic);
            $('#ClassEdit').val(result[0].kid_class);
            $('#EmailEdit').val(result[0].kid_email);
            $('#TelephoneEdit').val(result[0].n_telephone);
            $('#YearBirthdayEdit').val(result[0].year_birthday);
            $('#EndYearEdit').val(result[0].end_training);
            $('#SelectZayavka').selectpicker('val', result[0].status_zayavka_id);
            $('#SelectStudStatus').selectpicker('val', result[0].stud_status);
            $('#SchoolEdit').selectpicker('val', result[0].school_id);
            if (result[0].pers_data == 1) {
                $("#Stud_persData").prop('checked', true);
            } else {
                $("#Stud_persData").prop('checked', false);
            }
            if (result[0].card_template == 1) {
                $("#Stud_template").prop('checked', true);
            } else {
                $("#Stud_template").prop('checked', false);
            }
            $('#photo').val(null);
            $('#blah').prop('src','#');


        },
        error: function (er) {
            console.log(er);
        }
    });
})

$("#logout_btn").click(function () {
    sessionStorage.clear();
})

$(".etcDataStudent").click(function () {
    var id = $(this).attr("id");
    $.ajax({
        html: true,
        url: "/Home/EtcDataStudent",
        type: "GET",
        data: { "id": id },
        contentType: "application/json",
        success: function (result) {
            var html = '';
            var html2 = '';
            var html3 = '';
            var html4 = '';
            var html5 = '';
            var html6 = '';
            html += 'Номер UID: ' + result[0].nonfin_app_num + '';
            html2 += 'Номер телефона: ' + result[0].n_telephone + '';
            html3 += 'Почта: ' + result[0].kid_email + '';
            html4 += 'Наличие анкеты: ' + result[0].anketa + '';
            html5 += 'Дата изменения: ' + result[0].date_update + '';
            //if (result[0].status_zayavka_id == 0) {
            //    html6 += 'Статус: ' + 'Заявка отсутствует' + '';
            //}
            switch (result[0].status_zayavka_id) {
                case 0:
                    html6 += 'Статус: ' + 'Заявка отсутствует' + '';
                    break;
                case 1:
                    html6 += 'Статус: ' + 'Заявка оформлена' + '';
                    break;
                case 2:
                    html6 += 'Статус: ' + 'Заявка оформлена в Интернет-банкинге' + '';
                    break;
                case 3:
                    html6 += 'Статус: ' + 'Заявка оформлена в Корп Сайте' + '';
                    break;
                case 4:
                    html6 += 'Статус: ' + 'Заявка оформлена в Мобильном банкинге' + '';
                    break;
                case 5:
                    html6 += 'Статус: ' + 'Заявка оформлена в Отделении Банка' + '';
                    break;
                default:
                    html6 += 'Статус: ' + 'Заявка отсутствует' + '';
                    break;
            }
            $('#nonfin_app_num').html(html);
            $('#n_telephone').html(html2);
            $('#kid_email').html(html3);
            $('#anketa').html(html4);
            $('#date_update').html(html5);
            $('#status_zayavka_id').html(html6);
           
            var htmlB = '';
            var htmlA = '';
            //if (result[0].need_card_id == 0) {

            //    htmlA += '<button id="addForm" class="btn btn-outline-success">Подтвердить наличие анкеты</button>';
            //    $('#buttonSetAct').html(htmlA);
            //} else {
            //    $('#buttonSetAct').html(htmlA);
            //}
            htmlB += '<a id="submitAct" href="/Home/ActKid?unique_id=' + id + '" target="_blank" style="color:white"  class="btn btn-primary">Сформировать анкету</a>';           
            $('#buttonAct').html(htmlB);
        }
    })
})

$(".noAct").click(function () {
    var id = $(this).attr("id");
    $.ajax({
        html: true,
        url: "/Home/EtcDataStudent",
        type: "GET",
        data: { "id": id },
        contentType: "application/json",
        success: function (result) {
            var html = '';
            var html2 = '';
            var html3 = '';
            var html4 = '';
            var html5 = '';
            var html6 = '';
            html += 'Номер UID: ' + result[0].nonfin_app_num + '';
            html2 += 'Номер телефона: ' + result[0].n_telephone + '';
            html3 += 'Почта: ' + result[0].kid_email + '';
            html4 += 'Наличие анкеты: ' + result[0].anketa + '';
            html5 += 'Дата изменения: ' + result[0].date_update + '';
            /*  html6 += 'Статус: ' + result[0].status_zayavka_id + '';*/
            switch (result[0].status_zayavka_id) {
                case 1:
                    html6 += 'Статус: ' + 'Заявка оформлена' + '';
                    break;
                case 2:
                    html6 += 'Статус: ' + 'Заявка оформлена в Интернет-банкинге' + '';
                    break;
                case 3:
                    html6 += 'Статус: ' + 'Заявка оформлена в Корп Сайте' + '';
                    break;
                case 4:
                    html6 += 'Статус: ' + 'Заявка оформлена в Мобильном банкинге' + '';
                    break;
                case 5:
                    html6 += 'Статус: ' + 'Заявка оформлена в Отделении Банка' + '';
                    break;
                default:
                    html6 += 'Статус: ' + 'Заявка отсутствует' + '';
                    break;
            }
            $('#nonfin_app_num').html(html);
            $('#n_telephone').html(html2);
            $('#kid_email').html(html3);
            $('#anketa').html(html4);
            $('#date_update').html(html5);
            $('#status_zayavka_id').html(html6);
            var htmlB = '';
            $('#buttonAct').html(htmlB)
            //var htmlA = '';
            //if (result[0].need_card_id == 0) {

            //    htmlA += '<button id="addForm" disabled="" class="btn btn-outline-success" data-tooltip="Необходимо загрузить фото">Подтвердить наличие анкеты</button>';
            //    $('#buttonSetAct').html(htmlA);
            //} else {
            //    $('#buttonSetAct').html(htmlA);
            //}
        }
    })
})

$('.select-kid-photo').change(function () {
    if ($(this).val() != '') {
        if (this.files[0].size > 600000) {
            alert("Файл " + this.files[0].name + " слишком большой (" + Math.round(this.files[0].size / 1024) + "кб) рекомендуемый размер не более 600 кб!");
            $('#photo').val(null);
            $('#blah').prop('src', '#');
            return false;
        }
        else {
            var reader = new FileReader();//Check resolution
            //Read the contents of Image File.
            reader.readAsDataURL(this.files[0]);
            reader.onload = function (e) {
                //Initiate the JavaScript Image object.
                var image = new Image();
                //Set the Base64 string return from FileReader as source.
                image.src = e.target.result;
                //Validate the File Height and Width.
                image.onload = function () {
                    var resolution = this.width / this.height;
                    if (resolution.toString().slice(0, 4) != "0.75") {
                        alert("Формат фото не соответствует 30х40 мм");
                        $('#photo').val(null);
                        $('#blah').prop('src', '#');
                    }
                    /*document.getElementById(dataPhoto).value = image.src;*/
                    /*$("#submitAddForm").disabled = false;*/
                };
            };
            var src = URL.createObjectURL(event.target.files[0]);
            var preview = document.getElementById("blah");
            preview.src = src;


            /* blah.src = URL.createObjectURL(file);*/
            /*$(this).prev().text('Выбрано фото: ' + $(this)[0].files.length);*/
        }

    } else $(this).prev().text('Загрузить фото');
});

$('.select-new-sch').selectpicker('refresh');

function checkSession() {

    $.ajax({
        method: "GET",
        url: "/Home/CheckSession",
        //dataType: "json",
        //contentType: "application/json",
        data: {},
        success: function (result) {
            console.log(result)
            var out = result;
            if (out == 'ОК') {
                console.log(out)
            }
            if (out == 'Bad') {
               
                window.location.reload();
            }
            //$('#id_edit').val(result[0].id);
            //$('#unn_edit').val(result[0].unn);
            //$('#cbu_edit').val(result[0].cbu);
            //$('#school_name_edit').val(result[0].school_name);
            //$('#school_name_short_edit').val(result[0].school_name_short);
            //$('#school_name_card_edit').val(result[0].school_name_card);
            //$('#school_address_edit').val(result[0].school_address);
            //$('#school_phone_edit').val(result[0].school_phone);
            //$('#region_edit').val(result[0].region);
            //$('#district_edit').val(result[0].district);
            //$('#email_edit').val(result[0].email);
            //$('#locality_edit').val(result[0].locality);

            /*    $('#uniqueIDStr').html(result[0].unique_id);*/


        },
        error: function (er) {
            console.log(er);
        }
    });
}