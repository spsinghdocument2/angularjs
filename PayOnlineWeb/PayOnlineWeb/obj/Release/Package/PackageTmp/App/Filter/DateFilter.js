
app.filter("date", function () {
    var ddd = '';
    return function (x) {
        ddd = x.toString().split(" ").slice(0, 4).join(" ");

        var value = new Date(ddd);
        if (isNaN(value.getDate() < 10 ? '0' + value.getDate() : value.getDate()))
        {
            ddd = x.toString().split(" ").slice(0, 3).join(" ");
            value = new Date(ddd);
        }

        var month = value.getMonth() + 1;
        ///////////////////////////////////////////////
        var date = (month < 10 ? '0' + month : month)
            +


                       "/" +
          (value.getDate() < 10 ? '0' + value.getDate() : value.getDate())
           +
                       "/" +
           value.getFullYear();

        ///////////////////////
        // var date = value.getFullYear();

        ///////////////////////////////////

        //var date = (value.getDate() < 10 ? '0' + value.getDate() : value.getDate()) +


        //             "/" +
        //(month < 10 ? '0' + month : month) +
        //             "/" +
        // value.getFullYear();



        return date;
    };
});
