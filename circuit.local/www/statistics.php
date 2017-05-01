<?php

require_once("../include/common.inc.php");

$statistics = Storage::ReadStatistics();

?>
<!DOCTYPE html>
<html>
    <head>
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
        <script src="js/statistics_formatter.js"></script>
    </head>
    <body>
      <table border="1" class="statistics_container" id="statistics">
        <tr>
          <th>Correlation id</th>
          <th>Statistics</th>
        </tr>
      </table>
      <input type="hidden" id="statisticsData" value='<?= $statistics; ?>' />
    </body>
</html>
