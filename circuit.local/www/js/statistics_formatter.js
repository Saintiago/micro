$(function()
{
    var statisticsData = $.parseJSON($('#statisticsData').val());
    var statisticsContainer = $('#statistics');
    $.each(statisticsData, function(corrId, stat){
        statisticsContainer.append(
            '<tr>' +
                '<td>' + corrId + '</td>' +
                '<td>' +
                    '<ul>' +
                        '<li>Total lines count: ' + stat.totalLinesCount + '</li>' +
                        '<li>Good lines count: ' + stat.goodLinesCount + '</li>' +
                    '</ul>' +
                '</td>' +
            '</tr>'
        );
    });

});