function getFAQ(sender){
    new Ajax.Request(sender.href + '&textonly', {
        method: 'get',
        onSuccess: function(transport){
            var response = transport.responseText.evalJSON();
             
            if (response != null) {
                $("QuestionContainer").innerHTML = response.question;
                $("AnswerContainer").innerHTML = response.answer;
            } else {
                showFaqError(true);
            }
            showFAQ(true, sender);
        },
        onFailure: function(){
            showFaqError(true);
            showFAQ(true, sender);
        }
    });
    return false;
}

function showFaqError(canShow) {
    if (canShow){
        $("FaqError").show();
        $("FaqContainer").hide();
    } else {
        $("FaqError").hide();
        $("FaqContainer").show();
    }
}

function showFAQ(canShow, sender)
{
    showLayer(!canShow);
    if (canShow){
        var pos = Element.cumulativeOffset(sender);
        $('faq').style.top = pos.top + Element.getHeight(sender) + 2 + 'px';
        $('faq').style.left = pos.left + 'px';
        $('faq-layer').style.display = '';
    }
    else
        $('faq-layer').style.display = 'none';
}