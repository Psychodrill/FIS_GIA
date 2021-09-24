$StartableSites = @('check.ege.test', 'check.ege.services.test', 'check.ege.blanks.test')

function Contains($collection, $element)
{
    return ($collection | Where-Object {$_ -eq $element } | measure).Count -gt 0;
}
