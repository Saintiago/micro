<? require_once('../include/common.inc.php'); ?>
<!DOCTYPE html>
<html>
  <head></head>
  <body>
    <form method="post" enctype="application/x-www-form-urlencoded" action="<?= Config::URL_FORM_ACTION; ?>">
      <textarea rows="30" cols="60" name="<?= Config::POEM_PARAM_NAME; ?>" placeholder="Place your poetry here">
From fairest creatures we desire increase,
That thereby beauty's rose might never die,
But as the riper should by time decease,
His tender heir might bear his memory:
But thou, contracted to thine own bright eyes,
Feed'st thy light's flame with self-substantial fuel,
Making a famine where abundance lies,
Thyself thy foe, to thy sweet self too cruel.
Thou that art now the world's fresh ornament
And only herald to the gaudy spring,
Within thine own bud buriest thy content
And, tender churl, makest waste in niggarding.
Pity the world, or else this glutton be,
To eat the world's due, by the grave and thee.
      </textarea><br />
      <input type="submit" />
    </form>
  </body>
</html>