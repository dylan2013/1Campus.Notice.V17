


var setData = function (data) {

    $R('#content', 'source.setCode' , data);

    return "success";
}

var getData = function () {
    return $R('#content', 'source.getCode');
};
