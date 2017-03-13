<? require_once('../include/common.inc.php'); ?>
<!DOCTYPE html>
<html>
  <head></head>
  <body>
    <form method="post" action="<?= Config::URL_FORM_ACTION; ?>">
      <textarea name="<?= Config::POEM_PARAM_NAME; ?>" placeholder="Place your poetry here"></textarea><br />
      <input type="submit" />
    </form>
  </body>
</html>