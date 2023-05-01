// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var vm = new Vue({
    el: '#puller',

    data: function () {
        return {
            sprintnumber: 1,
            sprintvelocity: 0,
            buttondisabled: false
        };
    },
    methods: {
        getdividentcalander: function () {

            vm.buttondisabled = true;

            var xhr = new XMLHttpRequest();
            var url = "/api/puller/pulldividendcalander/";
            xhr.open("GET", url, false);
            xhr.onreadystatechange = function () {
                if (this.readyState === XMLHttpRequest.DONE) {
                    if (this.status === 200) {

                        var responsedata = JSON.parse(this.responseText);

                        console.log(responsedata);
                        alert('done');
                        vm.buttondisabled = false;
                    }
                }
            }.bind(xhr, this);
            xhr.send();
        },
        getsharedata: function () {

            vm.buttondisabled = true;

            var xhr = new XMLHttpRequest();
            var url = "/api/puller/pullsharedata/";
            xhr.open("GET", url, false);
            xhr.onreadystatechange = function () {
                if (this.readyState === XMLHttpRequest.DONE) {
                    if (this.status === 200) {

                        var responsedata = JSON.parse(this.responseText);

                        console.log(responsedata);
                        alert('done');
                        vm.buttondisabled = false;
                    }
                }
            }.bind(xhr, this);
            xhr.send();
        },
        getlsedividenddata: function () {

            vm.buttondisabled = true;
   
            var xhr = new XMLHttpRequest();
            var url = "/api/puller/pulllsedividenddata/";
            xhr.open("GET", url, false);
            xhr.onreadystatechange = function () {
                if (this.readyState === XMLHttpRequest.DONE) {
                    if (this.status === 200) {

                        var responsedata = JSON.parse(this.responseText);

                        console.log(responsedata);
                        alert('done');
                        vm.buttondisabled = false;
                    }
                }
            }.bind(xhr, this);
            xhr.send();
        }
    }

});


