﻿Vue.component(VueQrcode.name, VueQrcode);
Vue.component(VueBarcode.name, VueBarcode);

Vue.use(VueForm, {
    inputClasses: {
        valid: 'form-control-success',
        invalid: 'form-control-danger'
    }
});

var app = new Vue({
    el: '#app',
    data: {
        imgSize: 100,
        formstate: {},
        message: '',
        image: 'data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==',
        Price: 450,
        DeleteSuccess: '',
        DeleteFailed: '',
        ReprintFailed: '',
        hash: '',
        model: {
            PersonalID: '',
        },
        result: {
            InvoiceID: '',
            PersonalID: '',
            CreateDate: '',
            Piggy: [],
            Type: [],
            TypeText: [],
            Receipt: '0',
            BranchProvince: '',
            Branch: '',
            BranchCode: '',
            Delivery: '0',
            AddressOption: '0',
            Donate: '',
            DeliveryAmount: 0,
            GrandTotal: 0,
            Total: 0,
            QRCode: '|',
            Barcode: '|',
            SummaryText: '',
            Summary: '',
            ServiceCode: '',
            Ref1: '',
            BillerName: '',
        },
    },
    mounted: function () {
        this.result.Summary = $('#Summary').val();
        this.ReprintFailed = $('#ReprintFailed').val();
        this.result.ServiceCode = $('#ServiceCode').val();
        this.result.BillerName = $('#BillerName').val();
        this.result.TypeText[0] = $('#Type1').val();
        this.result.TypeText[1] = $('#Type2').val();
        this.result.TypeText[2] = $('#Type3').val();
    },
    methods: {
        dummy: function () {
        },
        Calculate: function () {
            this.result.Piggy = [];
            var self = this;
            var cnt = 0;
            //this.required.Type = true;
            for (var i = 0; i < this.result.Type.length; i++) {
                if (this.result.Type[i] === true) {
                    self.result.Piggy.push({ "Text": self.result.TypeText[i], "Count": 1, "Amount": self.twoDigit(self.Price) });
                    cnt++;
                    //this.required.Type = false;
                }
            }
            this.result.GrandTotal = (cnt > 0 ? this.Price * cnt : 0) + (this.result.Delivery === "1" ? 70 : 0);
            this.result.Total = cnt;
            if (this.result.Donate === "0")
                this.result.Donate = '';
            if (this.result.Delivery === "1")
                this.result.DeliveryAmount = 70;
            else this.result.DeliveryAmount = 0;

        },
        ClickBtn: function (val) {
            this.result.Type[val - 1] = !this.result.Type[val - 1];
            this.Calculate();
            event.preventDefault();
        },
        validateThaiCitizenID: function (id) {
            if (
                id.length !== 13 ||
                id.charAt(0).match(/[09]/)
            ) return false;

            var sum = 0;
            for (i = 0; i < 12; i++) {
                sum += parseInt(id.charAt(i)) * (13 - i);
            }

            if ((11 - sum % 11) % 10 !== parseInt(id.charAt(12))) {
                return false;
            }

            return true;
        },
        validateType: function () {
            var ret = false;
            for (var i = 0; i < this.result.Type.length; i++) {
                if (this.result.Type[i] === true) {
                    ret = true;
                }
            }
            return ret;
        },
        //getBarcode: function () {
        //    var data = {
        //        "TaxID": $('#TaxID').val(),
        //        "Suffix": $('#Suffix').val(),
        //        "Ref1": '0870538432',
        //        "Ref2": '000000121',
        //        "Amount": 1340.00
        //    };
        //    data.__RequestVerificationToken = $(':input[name="__RequestVerificationToken"]').val();
        //    var self = this;
        //    $.ajax({
        //        async: false,
        //        method: 'post',
        //        url: '/FormSiriraj/Siriraj/getBarcode',
        //        data: data,
        //        success: function (response) {
        //            self.result.Barcode = response;
        //        }
        //    });
        //},
        //genQRCode: function (pixel) {
        //    var data = {
        //        "Data": this.model.PersonalID,
        //        "Pixel": pixel
        //    };
        //    data.__RequestVerificationToken = $(':input[name="__RequestVerificationToken"]').val();
        //    $.ajax({
        //        async: false,
        //        method: 'post',
        //        url: '/FormSiriraj/Siriraj/GenQRCode',
        //        data: data,
        //        success: function (response) {
        //            app.image = 'data:image/jpg;base64,'.concat(response);
        //        }
        //    });
        //},
        maskID: function (id) {
            if (id !== '')
                return id.replace(id.substring(7, 10), "xxxx")
            else return '';
        },
        maskPhone: function (id) {
            if (id !== '')
                return id.replace(id.substring(4, 7), "xxxx")
            else return '';
        },
        twoDigit: function (val) {
            return val.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",")
        },
        formatDate: function (date) {
            var monthNames = [
              "January", "February", "March",
              "April", "May", "June", "July",
              "August", "September", "October",
              "November", "December"
            ];
            var d = new Date(date);
            var day = d.getDate();
            var monthIndex = d.getMonth();
            var year = d.getFullYear();

            return day + ' ' + monthNames[monthIndex] + ' ' + year;
        },
        formatThaiDate: function (date) {
            var monthNames = [
              "มกราคม", "กุมภาพันธ์", "มีนาคม",
              "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฏาคม",
              "สิงหาคม", "กันยายน", "ตุลาคม",
              "พฤศจิกายน", "ธันวาคม"
            ];
            var d = new Date(date);
            var day = d.getDate();
            var monthIndex = d.getMonth();
            var year = d.getFullYear() + 543;

            return day + ' ' + monthNames[monthIndex] + ' ' + year;
        },
        fieldClassName: function (field) {
            if (!field) {
                return '';
            }
            if ((field.$touched || field.$submitted) && field.$valid) {
                return 'has-success';
            }
            if ((field.$touched || field.$submitted) && field.$invalid) {
                return 'has-danger';
            }
        },
        resetForm: function () {
            this.result.PersonalID = '';
            this.result.CreateDate = '',
            this.result.SecretCode = '';
            this.result.Type = [];
            this.result.PersonalID = '';
            this.result.Receipt = '';
            this.result.Delivery = '';
            this.result.BranchCode = '';
            this.result.Branch = '';
            this.result.BranchProvince = '';
            this.result.Barcode = '|';
            this.result.QRCode = '|';
        },
        onConfirm: function (e) {
            if (!app.formstate.$invalid) {
                var self = this;
                var data = {
                    "CitizenID": app.model.PersonalID,
                };
                data.__RequestVerificationToken = $(':input[name="__RequestVerificationToken"]').val();

                this.resetForm();

                $.ajax({
                    async: false,
                    method: 'post',
                    url: '/FormSiriraj/Siriraj/getRegister',
                    data: data,
                    success: function (response) {
                        if (response.length > 0 ) {
                            //self.hash = response;
                            self.result.InvoiceID = response[0].InvoiceID;
                            self.result.Type[0] = response[0].Car === 1 ? true : false;
                            self.result.Type[1] = response[0].Radio === 1 ? true : false;
                            self.result.Type[2] = response[0].Camera === 1 ? true : false;
                            self.result.CreateDate = response[0].CreateDate;
                            self.result.Receipt = response[0].Receipt.toString();
                            self.result.Delivery = response[0].DeliveryType.toString();
                            self.result.BranchCode = response[0].BranchCode;
                            self.result.Branch = response[0].BranchName;
                            self.result.BranchProvince = response[0].BranchProvince;
                            self.result.Barcode = response[0].Barcode;
                            self.result.QRCode = response[0].QRCode;
                            self.result.Ref1 = response[0].Barcode.substring(14, 25);
                            self.Calculate();
                            $('#ConfirmForm').slideDown(100);
                            $('#MainForm').slideUp(100);
                            $("html, body").animate({ scrollTop: 0 }, "slow");
                        } else {
                            $("#ResultMessage").html(self.ReprintFailed);
                            $('#MainResult').slideDown(100);
                            $('#MainForm').slideUp(100);
                            $("html, body").animate({ scrollTop: 0 }, "slow");
                        }
                    }
                });
            }
        },
        onCancelConfirm: function (e) {
            this.model.PersonalID = '';
            this.model.Secretcode = '';
            $('#ConfirmForm').slideUp(100);
            $('#MainForm').slideDown(100);
            $("html, body").animate({ scrollTop: 0 }, "slow");
        },
        onGoBack: function (e) {
            this.model.PersonalID = '';
            this.model.Secretcode = '';
            $('#MainResult').slideUp(100);
            $('#MainForm').slideDown(100);
            $("html, body").animate({ scrollTop: 0 }, "slow");
            this.formstate._reset();
        },
        onPrint: function () {
            window.print();
        },
        //onDelete: function (e) {
        //    if (!app.formstate.$invalid) {
        //        var data = {
        //            "CitizenID": app.model.PersonalID.replace(SpacialCharacter, ''),
        //            "SecretCode": app.model.SecretCode.replace(SpacialCharacter, ''),
        //        };
        //        data.__RequestVerificationToken = $(':input[name="__RequestVerificationToken"]').val();
        //        $.ajax({
        //            async: false,
        //            method: 'post',
        //            url: '/FormSiriraj/Siriraj/DeleteData',
        //            data: data,
        //            success: function (response) {
        //                if (response.Success === 'true') {
        //                    var strHTML = '';
        //                    if (response.Success === 'true') {
        //                        strHTML += this.DeleteSuccess + "<br><br>";
        //                        $("#MainResult").html(strHTML);
        //                        app.resetForm();
        //                    } else {
        //                        strHTML += this.DeleteFailed + "<br><br>";
        //                        $("#MainResult").html(strHTML);
        //                        app.resetForm();
        //                    }
        //                }
        //            }
        //        });
        //    }
        //}
    },
    computed: {
        // a computed getter
        AmountSummary: function () {
            // `this` points to the vm instance
            return String.format(this.result.Summary, this.twoDigit(this.result.GrandTotal));
        },
        ButtonText: function () {
            // `this` points to the vm instance
            var vm = this;
            return function (btn, val) {
                var prc = vm.Price;
                var str = '';
                if (btn === 1) {
                    str = vm.result.Type1Text;
                }
                if (btn === 2) {
                    str = vm.result.Type2Text;
                }
                if (btn === 3) {
                    str = vm.result.Type3Text;
                }
                return String.format(str, prc);
            };

        },
    },
    //watch: {
    //    Branches: function (val, oldVal) {
    //        // change of userinput, do something
    //        //if(val !== oldVal)
    //        $('#selBranch').selectmenu();
    //        $('#selBranch').selectmenu('refresh');
    //    }
    //}
});


