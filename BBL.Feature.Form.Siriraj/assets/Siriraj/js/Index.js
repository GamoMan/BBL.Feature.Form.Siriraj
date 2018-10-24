﻿Vue.component(VueQrcode.name, VueQrcode);
Vue.component(VueBarcode.name, VueBarcode);

Vue.use(VueForm, {
    inputClasses: {
        valid: 'form-control-success',
        invalid: 'form-control-danger'
    }
});

Vue.component('modal',
    {
        template: '#modal-template',
        methods: {
            AcceptCanCelOrder: function (e) {
                app.onCancelOrder();

                app.showModal = false
            }
        }
    });

var app = new Vue({
    el: '#app',
    data: {
        qrcodeValue: '|012345678901200\n123456789012345678\n123456789012345678\n0',
        barcodeValue: '|012345678901200\n123456789012345678\n123456789012345678\n0',
        imgSize: 100,
        formstate: {},
        message: '',
        image: 'data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==',
        Price: 450,
        Province: '',
        Branches: [],
        Provinces: [],
        AvailableItems: [],
        haveCar: false,
        haveCamera: false,
        haveRadio: false,
        showModal: false,
        model: {
            Name: '',
            Surname: '',
            PersonalID: '',
            Mobile: '',
            Mobile2: '',
            Email: '',
            LineID: '',
            Address: '',
            Building: '',
            Soi: '',
            Road: '',
            Subdistrict: '',
            District: '',
            Province: '',
            Zip: '',
            PostAddress: '',
            PostBuilding: '',
            PostSoi: '',
            PostRoad: '',
            PostSubdistrict: '',
            PostDistrict: '',
            PostProvince: '',
            PostZip: '',
            Piggy: [],
            Type: [],
            TypeText: [],
            Receipt: '0',
            BranchProvince: '',
            BranchCode: '',
            BranchName: '',
            Delivery: '0',
            AddressOption: '0',
            Donate: 0,
            DeliveryAmount: 0,
            GrandTotal: 0,
            Total: 0,
            QRcode: '',
            Barcode: '',
            SummaryText: '',
            Summary: '',
            ServiceCode: '',
            Ref1: '',
            BillerName: '',
        },
        result: {
            InvoiceID: '',
            Piggy: [],
            Type: [],
            TypeText: [],
            Receipt: '0',
            BranchProvince: '',
            BranchCode: '',
            BranchName: '',
            Delivery: '0',
            AddressOption: '0',
            Donate: 0,
            DeliveryAmount: 0,
            GrandTotal: 0,
            Total: 0,
            QRcode: '',
            Barcode: ' ',
            SummaryText: '',
            Summary: '',
            car: 0,
            Radio: 0,
            Camera: 0
        },
        required: {
            Type: true,
            Post: false,
            Receipt: false
        },
    },
    mounted: function () {
        this.getAllBranches();

        this.getAvailableItems();

        this.model.TypeText[0] = $('#Type1').val();
        this.model.TypeText[1] = $('#Type2').val();
        this.model.TypeText[2] = $('#Type3').val();
        this.model.Summary = $('#Summary').val();
        this.model.TaxID = $('#TaxID').val();
        this.model.Suffix = $('#Suffix').val();
        this.model.ServiceCode = $('#ServiceCode').val();
        this.model.BillerName = $('#BillerName').val();

        $(document).ready(function () {
            $('#selProvince').selectmenu({
                change: function (event, ui) {
                    app.Branches = app.Provinces[ui.item.index - 1].Data;

                    app.model.BranchProvince = $('#selProvince').val();
                    Vue.nextTick(function () {
                        // DOM updated
                        $('#selBranch').selectmenu();
                        //$('#selBranch').val('dflkjdk');
                        $('#selBranch').selectmenu('refresh');
                    })
                }
            });

            $('#selBranch').selectmenu({
                change: function (event, ui) {
                    app.model.BranchCode = $('#selBranch').val();
                    Vue.nextTick(function () {
                        // DOM updated

                        app.model.BranchName = $('#selBranch option:selected').text();

                        //$('#selBranch').selectmenu('refresh');
                    })
                }
            });
        });
    },
    methods: {
        dummy: function () {
        },
        Calculate: function () {
            this.model.Piggy = [];

            var self = this;
            var cnt = 0;
            this.required.Type = true;

            for (var i = 0; i < this.model.Type.length; i++) {
                if (this.model.Type[i] === true) {
                    //Re Cal from Result
                    if (this.result.InvoiceID !== '') {
                        var OutOfStockMessage = 'หมด';

                        if ((i === 0) && (this.result.Car === 0)) {
                            Amount = 0;
                            Count = OutOfStockMessage;
                        }
                        else if ((i === 1) && (this.result.Radio === 0)) {
                            Amount = 0;
                            Count = OutOfStockMessage;
                        }
                        else if ((i === 2) && (this.result.Camera === 0)) {
                            Amount = 0;
                            Count = OutOfStockMessage;
                        } else {
                            Amount = self.twoDigit(self.Price);
                            Count = 1;
                            cnt++;
                        }

                        self.model.Piggy.push({ "Text": self.model.TypeText[i], "Count": Count, "Amount": Amount });
                    }
                    else {
                        //Cal From User Detail
                        self.model.Piggy.push({ "Text": self.model.TypeText[i], "Count": 1, "Amount": self.twoDigit(self.Price) });
                        cnt++;
                    }

                    this.required.Type = false;
                }
            }

            this.model.GrandTotal = (cnt > 0 ? this.Price * cnt : 0) + (this.model.Delivery === "1" ? 70 : 0);

            this.model.Total = cnt;
            if (this.model.Donate === "0")
                this.model.Donate = '';
        },
        ClickBtn: function (val) {
            //Set Non Check
            if (this.haveCar === false && val === 1) { return; }
            if (this.haveRadio === false && val === 2) { return; }
            if (this.haveCamera === false && val === 3) { return; }

            this.model.Type[val - 1] = !this.model.Type[val - 1];

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
            for (var i = 0; i < this.model.Type.length; i++) {
                if (this.haveCar === false && val === 0) { continue; }
                if (this.haveRadio === false && val === 1) { continue; }
                if (this.haveCamera === false && val === 2) { continue; }

                if (this.model.Type[i] === true) {
                    ret = true;
                }
            }
            return ret;
        },
        ReceiptClick: function () {
            if (this.model.Receipt === "1") {
                $('#Receipt').slideDown(300);
                this.required.Receipt = true;
            } else {
                $('#Receipt').slideUp(300);
                this.model.AddressOption = '1';
                this.required.Receipt = false;
                this.resetAddress();
                this.formstate._reset();
            }
            this.Calculate();
        },
        AddressOptionClick: function () {
            if (this.model.AddressOption === "1") {
                this.resetPostAddress();
                this.formstate._reset();
            } else {
                this.copyAddress();
            }
        },
        DeliveryClick: function () {
            if (this.model.Delivery === "0") {
                $('#AddressOption').slideUp(300);
                $('#PostNote').slideUp(300);
                $('#ShowBranch').slideDown(300);
                this.model.DeliveryAmount = 0;
                this.required.Post = false;
                if (this.model.Receipt === "0")
                    this.required.Receipt = false;
                else
                    this.required.Receipt = true;
            } else {
                $('#AddressOption').slideDown(300);
                $('#ShowBranch').slideUp(300);
                $('#PostNote').slideDown(300);
                this.model.DeliveryAmount = 70;
                this.required.Post = true;
                if (this.model.Receipt === "0") {
                    this.required.Receipt = false;
                } else {
                    this.required.Receipt = true;
                }
                this.formstate._reset();
            }
            this.AddressOptionClick();
            this.Calculate();
        },
        getAllBranches: function () {
            var data = {};
            data.__RequestVerificationToken = $(':input[name="__RequestVerificationToken"]').val();
            var self = this;
            $.ajax({
                async: false,
                method: 'post',
                url: '/FormSiriraj/Siriraj/getAllBranches',
                data: data,
                success: function (response) {
                    self.Provinces = JSON.parse(response);
                }
            });
        },

        getAvailableItems: function () {
            var data = {};
            data.__RequestVerificationToken = $(':input[name="__RequestVerificationToken"]').val();
            var self = this;
            $.ajax({
                async: false,
                method: 'post',
                url: '/FormSiriraj/Siriraj/AvailableItems',
                data: data,
                success: function (response) {
                    self.AvailableItems = response;

                    self.haveRadio = response.Radio;
                    self.haveCar = response.Car;
                    self.haveCamera = response.Camera;

                    //OutofStock
                    if (self.haveRadio === false && self.haveCar === false && self.haveCamera === false) {
                        $('#AlertForm').slideDown(100);
                        $("#errorOutOfStock").slideDown();
                        $('#MainForm').slideUp(100);

                        return;
                    }

                    if (self.haveRadio === false) {
                        $('#txtTipRadio').html('หมด');
                        $('#spnradio').addClass('Disableitem');
                    }
                    if (self.haveCar === false) {
                        $('#txtTipCar').html('หมด');
                        $('#spncar').addClass('Disableitem');
                    }
                    if (self.haveCamera === false) {
                        $('#txtTipCamera').html('หมด');
                        $('#spncamera').addClass('Disableitem');
                    }
                }
            });
        },

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
            return val.toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")
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
        copyAddress: function () {
            this.model.PostAddress = this.model.Address;
            this.model.PostBuilding = this.model.Building;
            this.model.PostSoi = this.model.Soi;
            this.model.PostRoad = this.model.Road;
            this.model.PostSubdistrict = this.model.Subdistrict;
            this.model.PostDistrict = this.model.District;
            this.model.PostProvince = this.model.Province;
            this.model.PostZip = this.model.Zip;
        },
        copyAddressBack: function () {
            if (this.model.AddressOption === "0") {
                this.model.Address = this.model.PostAddress;
                this.model.Building = this.model.PostBuilding;
                this.model.Soi = this.model.PostSoi;
                this.model.Road = this.model.PostRoad;
                this.model.Subdistrict = this.model.PostSubdistrict;
                this.model.District = this.model.PostDistrict;
                this.model.Province = this.model.PostProvince;
                this.model.Zip = this.model.PostZip;
            }
        },
        resetPostAddress: function () {
            this.model.PostAddress = '';
            this.model.PostBuilding = '';
            this.model.PostSoi = '';
            this.model.PostRoad = '';
            this.model.PostSubdistrict = '';
            this.model.PostDistrict = '';
            this.model.PostProvince = '';
            this.model.PostZip = '';
        },
        resetAddress: function () {
            this.model.Address = '';
            this.model.Building = '';
            this.model.Soi = '';
            this.model.Road = '';
            this.model.Subdistrict = '';
            this.model.District = '';
            this.model.Province = '';
            this.model.Zip = '';
        },
        resetPersonal: function () {
            this.model.Name = '';
            this.model.Surname = '';
            this.model.PersonalID = '';
            this.model.Mobile = '';
            this.model.Mobile2 = '';
            this.model.Email = '';
            this.model.LineID = '';
        },
        resetReserve: function () {
            this.model.Type = [];
            this.model.TypeText = [];
            this.model.BranchProvince = '';
            this.model.BranchCode = '';
            this.model.BranchName = '';
            this.model.Delivery = '0';
            this.model.Receipt = '0';
            this.model.Donate = '';
            this.model.GrandTotal = 0;
            this.model.Total = 0;
        },
        resetForm: function () {
            this.resetAddress();
            this.resetPersonal();
            this.resetPostAddress();
            this.resetReserve();
            //$("#MainForm").slideUp(300);
            //$("#ConfirmForm").slideDown(300);
        },
        onSelected: function (address) {
            this.model.Subdistrict = address.subdistrict;
            this.model.District = address.district;
            this.model.Province = address.province;
            this.model.Zip = address.postalCode;
        },
        onPostSelected: function (address) {
            this.model.PostSubdistrict = address.subdistrict;
            this.model.PostDistrict = address.district;
            this.model.PostProvince = address.province;
            this.model.PostZip = address.postalCode;
            this.copyAddressBack();
        },
        onConfirm: function (e) {
            if (!app.formstate.$invalid) {
                var data = {
                    "CitizenID": this.model.PersonalID,//.replace(SpacialCharacter, ''),
                    "Car": this.model.Type[0] === true ? 1 : 0,
                    "Camera": this.model.Type[2] === true ? 1 : 0,
                    "Radio": this.model.Type[1] === true ? 1 : 0,
                    "Receipt": this.model.Receipt,
                    "DeliveryType": this.model.Delivery,
                    "BranchCode": this.model.BranchCode,
                    "BranchName": this.model.BranchName,//.replace(SpacialCharacter, ''),
                    "BranchProvince": this.model.BranchProvince,//.replace(SpacialCharacter, ''),
                    "Barcode": this.model.Barcode,//.replace(SpacialCharacter, ''),
                    "QRcode": this.model.QRcode,//.replace(SpacialCharacter, ''),
                    "Amount": this.model.GrandTotal,
                    "Donate": this.model.Donate,
                    "TaxID": this.model.TaxID,
                    "Suffix": this.model.Suffix
                };
                data.__RequestVerificationToken = $(':input[name="__RequestVerificationToken"]').val();

                //dataCitizenID.__RequestVerificationToken = $(':input[name="__RequestVerificationToken"]').val();
                var self = this;
                //check Register ไปแล้ว
                $.ajax({
                    async: false,
                    method: 'post',
                    url: '/FormSiriraj/Siriraj/getRegister',
                    data: data,
                    success: function (response) {
                        if (response.length > 0) {
                            $('#AlertForm').slideDown(100);
                            $("#errorDupplicateID").slideDown();

                            //Regist แล้ว
                            $('#ConfirmForm').slideUp(100);
                            $('#MainForm').slideUp(100);
                            return;
                        }

                        //self.result.Ref1 = response[0].Barcode.substring(14, 25);
                        //if (response.Success === 'true') {
                        var strHTML = '';
                        //if (response.Success === 'true') {
                        strHTML += "<br><br>";
                        $('#ConfirmForm').slideDown(100);
                        $('#MainForm').slideUp(100);
                        $("html, body").animate({ scrollTop: 0 }, "slow");
                        //} else {
                        //  strHTML += "<br><br>";
                        //}
                        //}
                    }
                });
            }
        },
        onCancelConfirm: function (e) {
            $('#MainForm').slideDown(100);
            $('#ConfirmForm').slideUp(100);
            $("html, body").animate({ scrollTop: 0 }, "slow");
        },
        onPrint: function () {
            window.print();
        },

        onCancelOrder: function () {
            var data = {
                "CitizenID": this.model.PersonalID
            };

            data.__RequestVerificationToken = $(':input[name="__RequestVerificationToken"]').val();
            $.ajax({
                async: false,
                method: 'post',
                url: '/FormSiriraj/Siriraj/CancelOrder',
                data: data,
                success: function (response) {
                    if (response) {
                        $('#AlertForm').slideDown(100);
                        $("#AlertCancel").slideDown();
                    } else {
                        $('#AlertForm').slideDown(100);
                        $('#errorDefualt').slideDown(100);
                        //$("#AlertCancel").slideDown();
                    }

                    $('#ConfirmForm').slideUp(100);
                    $('#MainForm').slideUp(100);

                    $('#FinalForm').slideUp(100);

                    var strHTML = '';

                    strHTML += "<br><br>";

                    $("html, body").animate({ scrollTop: 0 }, "slow");
                }
            });
        },

        onSubmit: function (e) {
            if (!app.formstate.$invalid) {
                var data = {
                    Personal: {
                        "Name": this.model.Name,
                        "Surname": this.model.Surname,
                        "PersonalID": this.model.PersonalID,
                        "Mobile": this.model.Mobile,
                        "Mobile2": this.model.Mobile2,
                        "Email": this.model.Email,
                        "LineID": this.model.LineID,
                        "Address": this.model.Address,
                        "Building": this.model.Building,
                        "Soi": this.model.Soi,
                        "Road": this.model.Road,
                        "Subdistrict": this.model.Subdistrict,
                        "District": this.model.District,
                        "Province": this.model.Province,
                        "Zip": this.model.Zip,
                        "PostAddress": this.model.PostAddress,
                        "PostBuilding": this.model.PostBuilding,
                        "PostSoi": this.model.PostSoi,
                        "PostRoad": this.model.PostRoad,
                        "PostSubdistrict": this.model.PostSubdistrict,
                        "PostDistrict": this.model.PostDistrict,
                        "PostProvince": this.model.PostProvince,
                        "PostZip": this.model.PostZip,
                        "Piggy": this.model.Piggy,
                        "Type": this.model.Type,
                        "TypeText": this.model.TypeText,
                        "Receipt": this.model.Receipt,
                        "BranchProvince": this.model.BranchProvince,
                        "BranchCode": this.model.BranchCode,
                        "BranchName": this.model.BranchName,
                        "Delivery": this.model.Delivery,
                        "AddressOption": this.model.AddressOption,
                        "Donate": this.model.Donate,
                        "DeliveryAmount": this.model.DeliveryAmount,
                        "GrandTotal": this.model.GrandTotal,
                        "QRcode": this.model.QRcode,
                        "Barcode": this.model.Barcode,
                        "SummaryText": this.model.SummaryText,
                        "Summary": this.model.Summary,
                        "ServiceCode": this.model.ServiceCode,
                        "Ref1": this.model.Ref1,
                        "BillerName": this.model.BillerName,
                    },

                    "CitizenID": this.model.PersonalID,//.replace(SpacialCharacter, ''),
                    "Car": this.model.Type[0] === true ? 1 : 0,
                    "Camera": this.model.Type[2] === true ? 1 : 0,
                    "Radio": this.model.Type[1] === true ? 1 : 0,
                    "Receipt": this.model.Receipt,
                    "DeliveryType": this.model.Delivery,
                    "BranchCode": this.model.BranchCode,
                    "BranchName": this.model.BranchName,//.replace(SpacialCharacter, ''),
                    "BranchProvince": this.model.BranchProvince,//.replace(SpacialCharacter, ''),
                    "Barcode": this.model.Barcode,//.replace(SpacialCharacter, ''),
                    "QRcode": this.model.QRcode,//.replace(SpacialCharacter, ''),
                    "Amount": this.model.GrandTotal,
                    "Donate": this.model.Donate,
                    "TaxID": this.model.TaxID,
                    "Suffix": this.model.Suffix
                };
                data.__RequestVerificationToken = $(':input[name="__RequestVerificationToken"]').val();
                var self = this;

                $.ajax({
                    async: false,
                    method: 'post',
                    url: '/FormSiriraj/Siriraj/SaveRegister',
                    data: data,
                    success: function (response) {
                        //Error กลาง
                        if (response[0] === undefined) {
                            $("#ConfirmForm").slideUp(300);
                            $("#errorDefualt").slideDown();
                            $("#AlertForm").slideDown(300);
                            return;
                        }
                        //Sucess
                        if (response[0].Result === 1) {
                            self.result.Ref1 = response[0].Barcode.substring(14, 25);

                            var strHTML = '';
                            //strHTML += ReserveHeader + "<br><br>";
                            //strHTML += ActivityName + "<br>";
                            //strHTML += ActivityDetail + "<br><br>";
                            //strHTML += ReserveFooter;
                            //$("#MainResult").html(strHTML);
                            // this.resetForm();
                            $("#ConfirmForm").slideUp(300);
                            $("#FinalForm").slideDown(300);

                            self.result.InvoiceID = response[0].InvoiceID;

                            self.result.BranchProvince = self.model.BranchProvince;
                            self.result.Branch = self.model.Branch;

                            self.result.Name = self.model.Name;
                            self.result.Surname = self.model.Surname;

                            self.result.Mobile = self.model.Mobile;
                            self.result.Mobile2 = self.model.Mobile2;
                            self.result.Email = self.model.Email;

                            self.result.Address = self.model.Address;
                            self.result.Soi = self.model.Soi;
                            self.result.Road = self.model.Road;
                            self.result.Subdistrict = self.model.Subdistrict;
                            self.result.District = self.model.District;
                            self.result.Province = self.model.Province;
                            self.result.Zip = self.model.Zip;

                            self.result.QRcode = response[0].QRcode;
                            self.result.Barcode = response[0].Barcode;

                            self.model.ServiceCode = self.model.ServiceCode;
                            //self.result.Ref1 = self.model.Ref1;
                            self.model.BillerName = self.model.BillerName;

                            //Set ค่าที่จองได้

                            //self.model.Type[0] = (response[0].Car == 1);
                            //self.model.Type[1] = (response[0].Radio == 1);
                            //self.model.Type[2] = (response[0].Camera == 1);

                            self.result.Car = response[0].Car;
                            self.result.Radio = response[0].Radio;
                            self.result.Camera = response[0].Camera;

                            //self.result.GrandTotal = response[0].Amount;

                            self.Calculate();
                        }
                        else if (response[0].Result === 0) {
                            //Case Error slideUp AlertForm
                            //ของหมด
                            $("#ConfirmForm").slideUp(300);

                            $("#AlertForm").slideDown(300);
                            $("#errorOutOfStock").slideDown();
                        } else if (response[0].Result === -100) {
                            $("#ConfirmForm").slideUp(300);
                            $("#errorDupplicateID").slideDown();
                            $("#AlertForm").slideDown(300);
                        }
                    }
                });
            }
        },

        ShowModal: function () {
            debugger
            //app.model.head = term[termID].Head;

            //app.model.ModalMessage = app.htmlDecode(term[termID].Message);
            //app.model.Submit = term[termID].Submit;
            //app.model.AcceptTerms = term[termID].AcceptTerms

            app.showModal = true;
            return;
        }//ShowModal
    },
    computed: {
        // a computed getter
        AmountSummary: function () {
            // `this` points to the vm instance
            return String.format(this.model.Summary, this.twoDigit(this.model.GrandTotal));
        },
        ButtonText: function () {
            // `this` points to the vm instance
            var vm = this;
            return function (btn, val) {
                var prc = vm.Price;
                var str = '';
                if (btn === 1) {
                    str = vm.model.Type1Text;
                }
                if (btn === 2) {
                    str = vm.model.Type2Text;
                }
                if (btn === 3) {
                    str = vm.model.Type3Text;
                }
                return String.format(str, prc);
            };
        },
    },
});