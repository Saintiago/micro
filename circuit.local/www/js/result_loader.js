$(function()
{
    var preloader = $('<img src="images/preloader.gif" alt="Working..." />');
    $('#result').prepend(preloader);
    var uid = $('#uid').val();
    var tenant = $('#tenant').val();
    console.log('uid: ' + uid);
    $.get( "getFilteredPoemAjax.php?tenant=" + tenant + "&uid=" + uid, function(poemGoodLines) {
        if (poemGoodLines !== '')
        {
            $("#result").html('<pre>' + poemGoodLines + '</pre>');
        }
        else
        {
            $("#result").html('There is no good lines in your poem!');
        }
    });
});