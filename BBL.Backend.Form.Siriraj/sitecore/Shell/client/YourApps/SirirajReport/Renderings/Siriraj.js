define(["sitecore"], function (Sitecore) {
    var model = Sitecore.Definitions.Models.ControlModel.extend({
        initialize: function (options) {
            this._super();
            app = this;
            app.set("jsondata", '');
        },
    });

    var view = Sitecore.Definitions.Views.ControlView.extend({
        initialize: function (options) {
            this._super();
            var now = new Date();

            var day = ("0" + now.getDate()).slice(-2);
            var month = ("0" + (now.getMonth() + 1)).slice(-2);
            var lmonth = ("0" + (now.getMonth())).slice(-2);
            var fullYear = now.getFullYear();
            if (lmonth == "00") {
                lmonth = "12";
                fullYear = fullYear - 1;
            }
            var today = now.getFullYear() + "-" + (month) + "-" + (day);
            var lastmonth = fullYear + "-" + (lmonth).toString() + "-" + (day);

            $("#fdate").datepicker({
                dateFormat: 'yy-mm-dd',
                changeMonth: true,
                changeYear: true,
                yearRange: "c-100:c+0"
            });
            $("#tdate").datepicker({
                dateFormat: 'yy-mm-dd',
                changeMonth: true,
                changeYear: true,
                yearRange: "c-100:c+0"
            });

            // $("#tdate").datepicker("setDate", '2010-07-25');
            $('#fdate').datepicker('setDate', lastmonth);
            $('#tdate').datepicker('setDate', today);

            this.GetUsersData();
        },
        GetUsersData: function () {
            var data = {
                "FromDate": $("#fdate").val(),
                "ToDate": $("#tdate").val()
            };
            data.__RequestVerificationToken = $(':input[name="__RequestVerificationToken"]').val();

            $.ajax({
                url: "/api/sitecore/SirirajBackend/GetUserData",
                type: "POST",
                data: data,
                context: this,
                success: function (data) {
                    var rows = [];
                    for (var i = 0; i < data.length; i++) {
                        rows.push(JSON.parse(data[i].Data));
                    }

                    app.set("jsondata", rows);

                    if (rows.length > 0) {
                        $('#divReport').css("display", "block");
                        $('#divNoData').css("display", "none");
                    } else {
                        $('#divReport').css("display", "none");
                        $('#divNoData').css("display", "block");
                    }
                }
            });
        },
        ExportCSV: function () {
            var fileName = "Siriraj Register User Report";
            var JSONData = app.get("jsondata");
            var ShowLabel = true;
            var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
            var CSV = '';
            if (ShowLabel) {
                var row = "";
                for (var index in arrData[0]) {
                    row += index + ',';
                }
                row = row.slice(0, -1);
                CSV += row + '\r\n';
            }
            for (var i = 0; i < arrData.length; i++) {
                var row = "";
                for (var index in arrData[i]) {
                    enc = arrData[i][index];
                    if (enc === null)
                        enc = "";
                    row += '"' + enc + '",';

                    //var arrValue = arrData[i][index] == null ? "" : '"' + arrData[i][index] + '"';
                    //row += arrValue + ',';
                }
                row.slice(0, row.length - 1);
                CSV += row + '\r\n';
            }
            if (CSV == '') {
                growl.error("Invalid data");
                return;
            }

            var csvContent = "data:text/csv;charset=utf-8";
            var BOM = "\uFEFF";
            CSV = BOM + CSV;
            var csvData = new Blob([CSV], { type: csvContent });

            if (navigator.msSaveBlob) {
                navigator.msSaveBlob(csvData, fileName + ".csv");
            } else {
                //In FF link must be added to DOM to be clicked
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(csvData);
                link.setAttribute('download', fileName + ".csv");
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            }
        },
        OnChange: function () {
            $('#divReport').css("display", "none");
        }
    });

    Sitecore.Factories.createComponent("Siriraj", model, view, ".sc-Siriraj");
});