using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Web.UI;
using Esrp.Core.CatalogElements;
using System.Web.UI.WebControls;
using Esrp.Core.Organizations;

namespace Esrp.Web.Controls
{
    public class OrgFoundersWidget
    {
        private bool _allowEdit;
        public OrgFoundersWidget(bool allowEdit, IEnumerable<CatalogElement> selectedFounders)
        {
            _allowEdit = allowEdit;
            InitialSelectedFounders = selectedFounders;
        }

        private string SelectionInputId { get { return "selectedFounderIds"; } }

        private string DropDownId { get { return "selectableFounders"; } }

        private string SelectionDisplayId { get { return "selectedFounders"; } }

        private IEnumerable<CatalogElement> InitialSelectedFounders { get; set; }

        private IEnumerable<CatalogElement> PostedSelectedFounders
        {
            get
            {               
                string postedRaw = HttpContext.Current.Request[SelectionInputId];
                if (postedRaw == null)
                    return null;

                List<CatalogElement> result = new List<CatalogElement>();
                if (!String.IsNullOrEmpty(postedRaw))
                {
                    foreach (string founderId in postedRaw.Split('|'))
                    {
                        int founderIdInt;
                        if (Int32.TryParse(founderId, out founderIdInt))
                        {
                            result.Add(new CatalogElement(founderIdInt));
                        }
                    }
                }
                return result;
            }
        }

        public IEnumerable<CatalogElement> ActualSelectedFounders
        {
            get
            {
                List<CatalogElement> result = new List<CatalogElement>();
                if (PostedSelectedFounders != null)
                {
                    foreach (CatalogElement element in PostedSelectedFounders)
                    {
                        if (!result.Any(x => x.Id == element.Id))
                        {
                            result.Add(element);
                        }
                    }
                }
                else if (InitialSelectedFounders != null)
                {
                    foreach (CatalogElement element in InitialSelectedFounders)
                    {
                        if (!result.Any(x => x.Id == element.Id))
                        {
                            result.Add(element);
                        }
                    }
                }

                foreach (CatalogElement element in result)
                {
                    if (String.IsNullOrEmpty(element.Name))
                    {
                        CatalogElement elementWithName = SelectableFounders.FirstOrDefault(x => x.Id == element.Id);
                        if (elementWithName != null)
                        {
                            element.Name = elementWithName.Name;
                        }
                    }
                }
                return result;
            }
        }

        private IEnumerable<CatalogElement> _selectableFounders;
        private IEnumerable<CatalogElement> SelectableFounders
        {
            get
            {
                if (_selectableFounders == null)
                {
                    _selectableFounders = FoundersDataAcessor.GetAll();
                }
                return _selectableFounders;
            }
        }

        public string Html
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                if (_allowEdit)
                {
                    hw.AddAttribute("type", "hidden");
                    hw.AddAttribute("id", SelectionInputId);
                    hw.AddAttribute("name", SelectionInputId);
                    hw.AddAttribute("value", String.Join("|", ActualSelectedFounders.Select(x => x.Id.ToString()).ToArray()));
                    hw.RenderBeginTag("input");
                    hw.RenderEndTag();//input
                }

                hw.AddAttribute("id", SelectionDisplayId);
                hw.AddAttribute("class", "founders-list");
                hw.RenderBeginTag("ul");

                foreach (CatalogElement founder in ActualSelectedFounders)
                {
                    hw.AddAttribute("data-id", founder.Id.ToString());
                    hw.RenderBeginTag("li");
                    hw.RenderBeginTag("span");
                    hw.Write(founder.Name);
                    hw.RenderEndTag();//span 

                    if (_allowEdit)
                    {
                        hw.AddAttribute("type", "button");
                        hw.AddAttribute("class", "founders-remove");
                        hw.AddAttribute("title", "Удалить");
                        hw.AddAttribute("onClick", "unselectFounder('" + founder.Id.ToString() + "');");
                        hw.RenderBeginTag("input");
                        hw.RenderEndTag();//input
                    }
                    hw.RenderEndTag();//li
                }

                hw.RenderEndTag();//ul

                if (_allowEdit)
                {
                    hw.AddAttribute("id", DropDownId);
                    hw.RenderBeginTag("select");

                    IEnumerable<int?> selectedIds=ActualSelectedFounders.Select(x => x.Id).ToArray();
                    foreach (CatalogElement founder in SelectableFounders)
                    {
                        if (selectedIds.Contains(founder.Id))
                        {
                            hw.AddAttribute("selected", "selected");
                        }
                        hw.AddAttribute("value", founder.Id.ToString());
                        hw.RenderBeginTag("option");
                        hw.Write(founder.Name);
                        hw.RenderEndTag();//option
                    }
                    hw.RenderEndTag();//select

                    hw.AddAttribute("type", "button");
                    hw.AddAttribute("class", "founders-add");
                    hw.AddAttribute("title", "Выбрать");
                    hw.AddAttribute("value", "Выбрать");
                    hw.AddAttribute("onClick", "selectFounderFromDropDown();");
                    hw.RenderBeginTag("input");
                    hw.RenderEndTag();//input 

                    hw.AddAttribute("type", "text/javascript");
                    hw.RenderBeginTag("script");
                    hw.Write(JS);
                    hw.RenderEndTag();//script
                }

                return sb.ToString();
            }
        }

        private string JS
        {
            get
            {
                string result = @"
function selectFounderFromDropDown()
{
    var $ddFounders = $('#{0}');
    var $ddFoundersText = $ddFounders.siblings('.cuselText');
    selectFounder($ddFounders.val(), $ddFoundersText.text());
}
function selectFounder(id, name)
{
    var $hfFounders = $('#{1}');
    var $ulFounders = $('#{2}');

    var currentFounderIds = $hfFounders.val().split('|');
    var newFounderIds = [];
    for (var i = 0; i < currentFounderIds.length; i++)
    {
        if (currentFounderIds[i] == id)
            return;//дубль

        newFounderIds.push(currentFounderIds[i]);
    }
    newFounderIds.push(id);

    var newFoundersStr = '';
    for (var i = 0; i < newFounderIds.length; i++)
    {
        newFoundersStr += newFounderIds[i] + '|';
    }

    $hfFounders.val(newFoundersStr);

    var onClick = 'unselectFounder({4}'+id+'{4});';
    $ulFounders.append('<li data-id=\'' + id + '\'><span>' + name + '</span><input type=\'button\' title=\'Удалить\' class=\'founders-remove\' onClick=\'' + onClick + '\' /></li>');
}
function unselectFounder(id)
{ 
    var $hfFounders = $('#{1}');
    var $ulFounders = $('#{2}'); 

    var currentFounderIds = $hfFounders.val().split('|');
    var newFounderIds = [];
    for (var i = 0; i < currentFounderIds.length; i++)
    {
        if (currentFounderIds[i] != id)
        {
            newFounderIds.push(currentFounderIds[i]);
        }
    }
    var newFoundersStr = '';
    for (var i = 0; i < newFounderIds.length; i++)
    {
        newFoundersStr += newFounderIds[i] + '|';
    }

    $hfFounders.val(newFoundersStr);
    $ulFounders.find('li[data-id=\'' + id + '\']').remove();                        
}    
";
                return result
                    .Replace("{0}", DropDownId)
                    .Replace("{1}", SelectionInputId)
                    .Replace("{2}", SelectionDisplayId)
                    .Replace("{4}", "\"");
            }
        }
    }
}