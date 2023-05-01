Vue.use(VueTables.ClientTable);


$(document).ready(function () {


    var xhr = new XMLHttpRequest();
    var url = "/api/dividend/getdividenddata/";
    xhr.open("GET", url, false);
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status === 200) {

                var responsedata = JSON.parse(this.responseText);
                console.log(responsedata);
                //return responsedata;


                new Vue({
                    el: "#tapp",
                    data: {
                        columns: ['symbol', 'companyName', 'currentSharePrice', 'dividendAmount', 'dividendPercent', 'dividendExDate', 'market'],
                        data: responsedata,
                        options: {
                            headings: {
                                symbol: 'Ticker',
                                companyName: 'Company Name',
                                currentSharePrice: 'Share Price',
                                dividendPercent: "Div %",
                                dividendAmount: "Dividend",
                                dividendExDate: "Ex-Date",
                                market: "Market"
                            },
                            sortable: ['symbol', 'companyName', 'currentSharePrice', 'dividendPercent', 'dividendAmount', 'dividendExDate', 'market']
                        }
                    }
                });



            }
        }
    }.bind(xhr, this);
    xhr.send();


});

function getData() {
    /*
    console.log([{
        uid: "ZW",
        name: "Zimbabwe"
    }, {
        uid: "ZM",
        name: "Zambia"
    }]);
    
    return [{
        uid: "ZW",
        name: "Zimbabwe"
    }, {
        uid: "ZM",
        name: "Zambia"
    }];*/



    var xhr = new XMLHttpRequest();
    var url = "/api/dividend/getdividenddata/";
    xhr.open("GET", url, false);
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status === 200) {

                var responsedata = JSON.parse(this.responseText);
                console.log(responsedata);
                return responsedata;
                

            }
        }
    }.bind(xhr, this);
    xhr.send();



}
