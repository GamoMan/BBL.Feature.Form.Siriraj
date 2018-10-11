Vue.component(VueQrcode.name, VueQrcode);
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
        model: {
            Name: '',
            Surname: '',
            PersonalID: '',
            Mobile: '',
            Mobile2:'',
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
            Total:0,
            QRcode: '',
            Barcode: '',
            SummaryText: '',
            Summary: '',
            ServiceCode: '',
            Ref1: '',
            BillerName: '',
        },
        result: {
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
        },
        required: {
            Type: true,
            Post: false,
            Receipt: false
        }
    },
    mounted: function () {
        this.getAllBranches();
        this.model.TypeText[0] = $('#Type1').val();
        this.model.TypeText[1] = $('#Type2').val();
        this.model.TypeText[2] = $('#Type3').val();
        this.model.Summary = $('#Summary').val();
        this.model.TaxID = $('#TaxID').val();
        this.model.Suffix = $('#Suffix').val();
        this.model.ServiceCode = $('#ServiceCode').val();

        $(document).ready(function () {

            $('#selProvince').selectmenu({
                change: function (event, ui) {
                    app.Branches = app.Provinces[ui.item.index - 1].Data;
                    app.model.BranchProvince = $('#selProvince').val();
                    Vue.nextTick(function () {
                        // DOM updated
                        $('#selBranch').selectmenu();
                        $('#selBranch').val('');
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
                    })

                }
            });

        });

    },
    methods: {
        dummy: function () {
        },
        Calculate: function(){
            this.model.Piggy = [];
            var self = this;
            var cnt = 0;
            this.required.Type = true;
            for(var i=0;i<this.model.Type.length;i++) {
                if (this.model.Type[i] === true) {
                    self.model.Piggy.push({ "Text": self.model.TypeText[i], "Count": 1, "Amount": self.twoDigit(self.Price) });
                    cnt++;
                    this.required.Type = false;
                }
            }
            this.model.GrandTotal = (cnt > 0 ? this.Price * cnt : 0) + (this.model.Delivery === "1" ? 70 : 0);
            this.model.Total = cnt;
            if (this.model.Donate === "0")
                this.model.Donate = '';
        },
        ClickBtn: function (val) {
            this.model.Type[val-1] = !this.model.Type[val-1];
            this.Calculate();
            event.preventDefault();
        },
        validateThaiCitizenID: function (id) {
            if(
                id.length !== 13 ||
                id.charAt(0).match(/[09]/)
            ) return false;

            var sum = 0;
            for( i=0; i < 12; i++ ){
                sum += parseInt(id.charAt(i))*(13-i);
            }

            if( (11 - sum%11 )%10 !== parseInt(id.charAt(12)) ){
                return false;
            }

            return true;
        },
        validateType: function () {
            var ret = false;
            for (var i = 0; i < this.model.Type.length; i++) {
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
            }
            this.Calculate();
        },
        AddressOptionClick : function(){
            if (this.model.AddressOption === "1") {
                this.resetPostAddress();
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
            } else {
                $('#AddressOption').slideDown(300);
                $('#ShowBranch').slideUp(300);
                $('#PostNote').slideDown(300);
                this.model.DeliveryAmount = 70;
                this.required.Post = true;
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
        twoDigit:function(val){
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
        resetPostAddress: function(){
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
        resetPersonal: function() {
            this.model.Name = '';
            this.model.Surname = '';
            this.model.PersonalID = '';
            this.model.Mobile = '';
            this.model.Email = '';
            this.model.LineID = '';
        },
        resetReserve: function(){
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
        onSelected: function(address) {
            this.model.Subdistrict = address.subdistrict;
            this.model.District = address.district;
            this.model.Province = address.province;
            this.model.Zip = address.postalCode;
        },
        onPostSelected: function(address) {
            this.model.PostSubdistrict = address.subdistrict;
            this.model.PostDistrict = address.district;
            this.model.PostProvince = address.province;
            this.model.PostZip = address.postalCode;
        },
        onConfirm: function(e){
            if (!app.formstate.$invalid) {
                var data = {
                    "CitizenID": this.model.PersonalID,//.replace(SpacialCharacter, ''),
                    "Car":this.model.Type[0]===true?1:0,
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
                        self.result.Ref1 = response[0].Barcode.substring(14, 25);
                        if (response.Success === 'true') {
                            var strHTML = '';
                            if (response.Success === 'true') {
                                strHTML += "<br><br>";
                                $('#ConfirmForm').slideDown(100);
                                $('#MainForm').slideUp(100);
                                $("html, body").animate({ scrollTop: 0 }, "slow");
                            } else {
                                strHTML += "<br><br>";
                            }
                        }
                    }
                });
            }
        },
        onCancelConfirm: function (e) {
            $('#MainForm').slideDown(100);
            $('#ConfirmForm').slideUp(100);
            $("html, body").animate({ scrollTop: 0 }, "slow");
        },
        onPrint: function(){
            window.print();
        },
        onSubmit: function (e) {
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
                    "Amount": this.model.Amount,
                    "Donate": this.model.Donate,
                    "TaxID": this.model.TaxID,
                    "Suffix": this.model.Suffix
                };
                data.__RequestVerificationToken = $(':input[name="__RequestVerificationToken"]').val();
                var self = this;

                $.ajax({
                    async: false,
                    method: 'post',
                    url: '/FormSiriraj/Siriraj/SaveData',
                    data: data,
                    success: function (response) {
                        self.result.Ref1 = response[0].Barcode.substring(14, 25);
                        if (response.Success === 'true') {
                            var strHTML = '';
                            if (response.Success === 'true') {
                                strHTML += ReserveHeader + "<br><br>";
                                strHTML += ActivityName + "<br>";
                                strHTML += ActivityDetail + "<br><br>";
                                strHTML += ReserveFooter;
                                $("#MainResult").html(strHTML);
                                this.resetForm();
                                $("#MainForm").slideUp(300);
                                $("#ConfirmForm").slideDown(300);
                            } else {
                                strHTML += AttendeeFullHeader + "<br><br>";
                                strHTML += ActivityName + "<br>";
                                strHTML += ActivityDetail + "<br><br>";
                                strHTML += AttendeeFullFooter;
                                $("#MainResult").html(strHTML);
                                this.resetForm();
                                $("#MainForm").slideUp(300);
                                $("#ConfirmForm").slideDown(300);
                            }
                        }
                    }
                });
            }
        }
    },
    computed: {
        // a computed getter
        AmountSummary: function () {
            // `this` points to the vm instance
            return String.format(this.model.Summary,this.twoDigit(this.model.GrandTotal));
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
    //watch: {
    //    Branches: function (val, oldVal) {
    //        // change of userinput, do something
    //        //if(val !== oldVal)
    //        $('#selBranch').selectmenu();
    //        $('#selBranch').selectmenu('refresh');
    //    }
    //}
});


