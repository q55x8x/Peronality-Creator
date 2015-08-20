function Ajax(page){
    $.ajax({
        url: page,
        context: document.body,
        cache: false,
        success: function(data) {
            eval(data);
        }
    });
}