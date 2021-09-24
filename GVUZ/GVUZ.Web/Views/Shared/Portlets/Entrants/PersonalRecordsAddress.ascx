<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Entrants.PersonalRecordsAddressViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<div id="content">
    <% if (Model.ShowDenyMessage)
       { %> <div>Невозможно редактировать данное заявление</div> <script type="text/javascript">
                                                                     function doSubmit() { return false; }
                                                                 </script>  <% }
       else
       { %>	
        <table class="data tableApp2" style="max-width: 1500px"> <%--Поправить корректные стили под новый дизайн --%>
            <thead>
                <tr>
                    <th class="caption" colspan="2" align="center" style="font-size: 110%; width: 50%;">Прописка</th>
                    <th class="caption" colspan="2" align="center" style="font-size: 110%; width: 50%;">Фактическое место жительства</th>
                </tr>
            </thead>
            <tbody>
                <% if (Model.ApplicationStep == 0)
                   { %>
                    <tr>
                        <td class="caption"> </td>
                        <td class="commonData"> </td>
                        <td class="caption"><%= Html.TableLabelFor(m => m.IsOnlyRegistration) %></td>
                        <td class="commonData"><%= Html.CheckBoxFor(m => m.IsOnlyRegistration) %></td>
                    </tr>
                <% }
                   else
                   { %>

                    <tr>
                        <td class="caption"><%= Html.TableLabelFor(m => m.HostelRequired) %></td>
                        <td class="commonData"><%= Html.CheckBoxFor(m => m.HostelRequired) %></td>
                        <td class="caption"><%= Html.TableLabelFor(m => m.IsOnlyRegistration) %></td>
                        <td class="commonData"><%= Html.CheckBoxFor(m => m.IsOnlyRegistration) %></td>
                    </tr>
                <% } %>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationAddress.PostalCode, required: false) %></td>
                    <td class="registrationData"><%= Html.TextBoxExFor(m => m.RegistrationAddress.PostalCode, new {tabindex = 1}) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.FactAddress.PostalCode) %></td>
                    <td class="factData"><%= Html.TextBoxExFor(m => m.FactAddress.PostalCode, new {tabindex = 13}) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationAddress.CountryID, required: false) %></td>
                    <td class="registrationData"><%= Html.DropDownListFor(m => m.RegistrationAddress.CountryID, new SelectList(Model.CountryList, "ID", "Name"), new {tabindex = 2, style = "width:342px"}) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.FactAddress.CountryID) %></td>
                    <td class="factData"><%= Html.DropDownListFor(m => m.FactAddress.CountryID, new SelectList(Model.CountryList, "ID", "Name"), new {tabindex = 14, style = "width:342px"}) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationAddress.RegionID, required: false) %></td>
                    <td class="registrationRegion">
                        <span><%= Html.DropDownListFor(m => m.RegistrationAddress.RegionID, new SelectList(Model.RegistrationRegionsList, "ID", "Name"), new {tabindex = 3, style = "display:none;width:342px"}) %></span>
                        <span><%= Html.TextBoxExFor(m => m.RegistrationAddress.RegionName, new {tabindex = 3, style = "display:none;"}) %></span>
                    </td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.FactAddress.RegionID) %></td>
                    <td class="factRegion"><span><%= Html.DropDownListFor(m => m.FactAddress.RegionID,
                                                                          new SelectList(Model.FactRegionsList, "ID", "Name"), new {tabindex = 15, style = "display:none;width:342px"}) %></span>
                        <span><%= Html.TextBoxExFor(m => m.FactAddress.RegionName, new {tabindex = 15, style = "display:none;"}) %></span>
                    </td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationAddress.CityName) %></td>
                    <td class="registrationData"><%= Html.TextBoxExFor(m => m.RegistrationAddress.CityName, new {tabindex = 4}) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.FactAddress.CityName) %></td>
                    <td class="factData"><%= Html.TextBoxExFor(m => m.FactAddress.CityName, new {tabindex = 16}) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationAddress.Street, required: false) %></td>
                    <td class="registrationData"><%= Html.TextBoxExFor(m => m.RegistrationAddress.Street, new {tabindex = 5}) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.FactAddress.Street) %></td>
                    <td class="factData"><%= Html.TextBoxExFor(m => m.FactAddress.Street, new {tabindex = 17}) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationAddress.Building, new {style = "margin-right:0px;padding-right:0px"}, showColon: false, required: false) %><label style="margin-left: 0px; margin-right: 0px; padding-left: 0px; padding-right: 0px; width: auto;">&nbsp;/&nbsp;</label><%= Html.TableLabelFor(m => m.RegistrationAddress.BuildingPart, new {style = "width:auto;margin-left:0px;padding-left:0px"}) %></td>
                    <td class="registrationData"><%= Html.TextBoxExFor(m => m.RegistrationAddress.Building, new {tabindex = 6, style = "width:40px"}) %>&nbsp;/&nbsp;<%= Html.TextBoxExFor(m => m.RegistrationAddress.BuildingPart, new {tabindex = 7, style = "width:40px"}) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.FactAddress.Building, new {style = "margin-right:0px;padding-right:0px"}, showColon: false) %> <span id="reqFactAddressBuildingSpan"></span><label style="margin-left: 0px; margin-right: 0px; padding-left: 0px; padding-right: 0px; width: auto;">&nbsp;/&nbsp;</label><%= Html.TableLabelFor(m => m.FactAddress.BuildingPart, new {style = "width:auto;margin-left:0px;padding-left:0px"}) %></td>
                    <td class="factData"><%= Html.TextBoxExFor(m => m.FactAddress.Building, new {tabindex = 18, style = "width:40px"}) %>&nbsp;/&nbsp;<%= Html.TextBoxExFor(m => m.FactAddress.BuildingPart, new {tabindex = 19, style = "width:40px"}) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationAddress.Room, required: false) %></td>
                    <td class="registrationData"><%= Html.TextBoxExFor(m => m.RegistrationAddress.Room, new {tabindex = 8}) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.FactAddress.Room) %></td>
                    <td class="factData"><%= Html.TextBoxExFor(m => m.FactAddress.Room, new {tabindex = 20}) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationAddress.Phone) %></td>
                    <td class="registrationData"><%= Html.TextBoxExFor(m => m.RegistrationAddress.Phone, new {tabindex = 9}) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.FactAddress.Phone) %></td>
                    <td class="factData"><%= Html.TextBoxExFor(m => m.FactAddress.Phone, new {tabindex = 21}) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Mobile) %></td>
                    <td class="commonData"><%= Html.TextBoxExFor(m => m.Mobile, new {tabindex = 10}) %></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Email) %></td>
                    <td class="commonData"><%= Html.TextBoxExFor(m => m.Email, new {tabindex = 11}) %></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </tbody>
        </table>
        <% if (Model.ApplicationStep == 0)
           { %>
            <div>
                <a id="btnSubmit" href="<%= Url.Generate<EntrantController>(c => c.Address()) %>">Сохранить</a>
                <a id="btnCancel" href="<%= Url.Generate<EntrantController>(c => c.Address()) %>">Отмена</a>
            </div>
        <% } %>
        <script type="text/javascript">
            var RegistrationAddress_CountryHasRegions = <%= Model.RegistrationAddress.CountryHasRegions %>;
            var FactAddress_CountryHasRegions = <%= Model.FactAddress.CountryHasRegions %>;

            function doValidation() {
                var res = !revalidatePage(jQuery('.commonData'), true);
                res &= !revalidatePage(jQuery('.registrationData'));
                if (RegistrationAddress_CountryHasRegions == 1) {
                    clearValidationErrors(jQuery('#RegistrationAddress_RegionName').parent());
                    res &= !revalidatePage(jQuery('#RegistrationAddress_RegionID').parent());
                } else {
                    clearValidationErrors(jQuery('#RegistrationAddress_RegionID').parent());
                    res &= !revalidatePage(jQuery('#RegistrationAddress_RegionName').parent());
                }

                if (jQuery('#IsOnlyRegistration').attr('checked'))
                    clearValidationErrors(jQuery('.factData'));
                else {
                    res &= !revalidatePage(jQuery('.factData'));
                    if (FactAddress_CountryHasRegions == 1) {
                        clearValidationErrors(jQuery('#FactAddress_RegionName').parent());
                        res &= !revalidatePage(jQuery('#FactAddress_RegionID').parent());
                    } else {
                        clearValidationErrors(jQuery('#FactAddress_RegionID').parent());
                        res &= !revalidatePage(jQuery('#FactAddress_RegionName').parent());
                    }
                }

                showValidationMessagesForCompositeFields();
                return res;
            }

            function showValidationMessagesForCompositeFields() {
                // display validation in registration address  for "building" under "building part" field
                if (jQuery('#RegistrationAddress_Building').length > 0) {
                    if (jQuery('#RegistrationAddress_Building').hasClass('input-validation-error') ||
                        jQuery('#RegistrationAddress_Building').val() == '') {
                        var $valMessageSpan = jQuery('#RegistrationAddress_Building').next('span.field-validation-error');
                        if ($valMessageSpan.length > 0) {
                            $valMessageSpan = $valMessageSpan.detach();
                            jQuery('#RegistrationAddress_BuildingPart').after($valMessageSpan);
                        }
                    }
                }

                // display validation in fact address for "building" under "building part" field
                if (jQuery('#FactAddress_Building').length > 0) {
                    if (jQuery('#FactAddress_Building').hasClass('input-validation-error') ||
                        jQuery('#FactAddress_Building').val() == '') {
                        var $valMessageSpan2 = jQuery('#FactAddress_Building').next('span.field-validation-error');
                        if ($valMessageSpan2.length > 0) {
                            $valMessageSpan2 = $valMessageSpan2.detach();
                            jQuery('#FactAddress_BuildingPart').after($valMessageSpan2);
                        }
                    }
                }
            }

            function doSubmit(cmd) {
                if (cmd == 'back' || cmd == 'save')   <%-- если не заполнено ни одно из полей, даём уйти со страницы --%>
                {
                    var isFilled = false;
                    jQuery('.data input[type="text"]').each(function() { if (jQuery(this).val() != '') isFilled = true; });
                    if (!isFilled && typeof doAppNavigate != "undefined") {
                        doAppNavigate();
                        return false;
                    }
                }
                if (cmd != 'back' && cmd != 'save') {   <%-- если идём назад или сохраняем, то даём некорректные данные сохранить (пропускаем валидацию) --%>
                    if (!doValidation()) return false;
                }

                var model =
                {
                    RegistrationAddress:
                    {
                        PostalCode: jQuery('#RegistrationAddress_PostalCode').val(),
                        CountryID: jQuery('#RegistrationAddress_CountryID').val(),
                        CountryHasRegions: RegistrationAddress_CountryHasRegions,
                        RegionID: jQuery('#RegistrationAddress_RegionID').val(),
                        RegionName: jQuery('#RegistrationAddress_RegionName').val(),
                        CityName: jQuery('#RegistrationAddress_CityName').val(),
                        Street: jQuery('#RegistrationAddress_Street').val(),
                        Building: jQuery('#RegistrationAddress_Building').val(),
                        BuildingPart: jQuery('#RegistrationAddress_BuildingPart').val(),
                        Room: jQuery('#RegistrationAddress_Room').val(),
                        Phone: jQuery('#RegistrationAddress_Phone').val()
                    },
                    Mobile: jQuery('#Mobile').val(),
                    Email: jQuery('#Email').val(),
                    IsOnlyRegistration: jQuery('#IsOnlyRegistration').attr('checked'),
                    FactAddress:
                    {
                        PostalCode: jQuery('#FactAddress_PostalCode').val(),
                        CountryID: jQuery('#FactAddress_CountryID').val(),
                        CountryHasRegions: FactAddress_CountryHasRegions,
                        RegionID: jQuery('#FactAddress_RegionID').val(),
                        RegionName: jQuery('#FactAddress_RegionName').val(),
                        CityName: jQuery('#FactAddress_CityName').val(),
                        Street: jQuery('#FactAddress_Street').val(),
                        Building: jQuery('#FactAddress_Building').val(),
                        BuildingPart: jQuery('#FactAddress_BuildingPart').val(),
                        Room: jQuery('#FactAddress_Room').val(),
                        Phone: jQuery('#FactAddress_Phone').val()
                    },
                    <% if (Model.ApplicationStep != 0)
                       { %>
                    HostelRequired: jQuery('#HostelRequired').attr('checked'),
                    ApplicationID: <%= Model.ApplicationID %>,
                    <% } %>
                    EntrantID: <%= Model.EntrantID %>,
                    ActionCommand: cmd
                };
                doPostAjax('<%= Url.Generate<EntrantController>(x => x.SavePersonalAddress(null)) %>', 'model=' + encodeURIComponent(JSON.stringify(model)), function(data) {
                    if (!addValidationErrorsFromServerResponse(data, true)) {
                        if (typeof doAppNavigate != "undefined")
                            doAppNavigate();
                        else
                            jQuery('#btnCancel').click();
                    } else
                        showValidationMessagesForCompositeFields();
                }, "application/x-www-form-urlencoded");
                return false;
            }

            function copyAddressField(fieldName) {
                if (jQuery('#IsOnlyRegistration').attr('checked')) {
                    jQuery('#FactAddress_' + fieldName).val(jQuery('#RegistrationAddress_' + fieldName).val());
                }
            }

            function copyAddressFields(fieldNames) {
                jQuery.each(fieldNames, function(i, e) { copyAddressField(e); });
            }

            var addressFields = ['PostalCode', 'CountryID', 'RegionID', 'RegionName', 'CityName', 'Street', 'Building', 'BuildingPart', 'Room', 'Phone'];

            function disableAddress() {
                if (!jQuery('#IsOnlyRegistration').attr('checked'))
                    jQuery('.factData>input, .factData>select, .factRegion>span>input, .factRegion>span>select').removeAttr('disabled').removeClass('view');
                else {
                    var copyAddressFunc = function() {
                        jQuery('.factData>input, .factData>select, .factRegion>span>input, .factRegion>span>select').attr('disabled', 'disabled').addClass('view');
                        FactAddress_CountryHasRegions = RegistrationAddress_CountryHasRegions;
                        copyAddressFields(addressFields);
                    };
                    if (jQuery('#IsOnlyRegistration').attr('checked')) {
                        if (factAddressFieldsChanged) {
                            jQuery('#IsOnlyRegistration').removeAttr('checked');
                            confirmDialog('Вы уже начали вводить фактический адрес. Действительно ли он совпадает с пропиской?',
                                function() {
                                    factAddressFieldsChanged = false;
                                    jQuery('#IsOnlyRegistration').attr('checked', 'checked');
                                    copyAddressFunc();
                                }
                            );
                        } else copyAddressFunc();
                    }
                }
            }

            var factAddressFieldsChanged = false;

            function initAddressAutoFill() {
                jQuery.each(addressFields, function(i, e) {
                    var e2 = e;
                    jQuery('#RegistrationAddress_' + e2).blur(function() {
                        copyAddressField(e2);
                    });
                });
                jQuery.each(addressFields, function(i, e) {
                    var e2 = e;
                    jQuery('#FactAddress_' + e2).change(function() {
                        factAddressFieldsChanged = true;
                    });
                });
            }

            //показываем либо список регионов, либо текстовое поле для ввода

            function showRegion(regionsControlID, isExistsRegions) {
                var regionsNameControlID = regionsControlID.replace('ID', 'Name');
                var CountryHasRegions = 0;
                if (isExistsRegions) {
                    jQuery('#' + regionsControlID).css("display", "inline");
                    jQuery('#' + regionsNameControlID).css("display", "none");
                    CountryHasRegions = 1;
                } else {
                    jQuery('#' + regionsControlID).css("display", "none");
                    jQuery('#' + regionsNameControlID).css("display", "inline");
                    CountryHasRegions = 0;
                }
                return CountryHasRegions;
            }

            //запись регионов в drop-down

            function changeRegionsOptions(regionsList, control) {
                var regionsHtml = "";
                if (regionsList.length > 0)
                    for (var i = 0; i < regionsList.length; i++)
                        regionsHtml += '<option value="' + regionsList[i].ID + '">' + regionsList[i].Name + '</option>';
                else
                    regionsHtml = '<option value="0">--</option>';
                jQuery('#' + control).empty();
                jQuery('#' + control).prepend(regionsHtml);
            }

            //получение списка регионов по стране

            function changeRegionsList(controlID, countryID) {
                var regionsControlID = controlID.replace('Country', 'Region');
                var cityNameControlID = controlID.replace('CountryID', 'CityName');
                var regionsNameControlID = regionsControlID.replace('ID', 'Name');
                var CountryHasRegions = 0;
                doPostAjax('<%= Url.Generate<EntrantController>(x => x.RegionsList(null)) %>', 'countryID=' + countryID, function(data) {
                    changeRegionsOptions(data.Data, regionsControlID);
                    jQuery('#' + regionsNameControlID).val("");
                    CountryHasRegions = showRegion(regionsControlID, (data.Data.length > 0));
                    if (controlID.substring(0, 4) == "Fact")
                        FactAddress_CountryHasRegions = CountryHasRegions;
                    else {
                        RegistrationAddress_CountryHasRegions = CountryHasRegions;
                        if (jQuery('#IsOnlyRegistration').attr('checked')) {
                            changeRegionsList('FactAddress_CountryID', countryID);
                        }
                    }

                    onChangeRegion(regionsControlID, CountryHasRegions);

                }, "application/x-www-form-urlencoded", null, true);
                return false;
            }

            function onChangeRegion(regionsControlID, CountryHasRegions) {
                var cityNameControlID = regionsControlID.replace('RegionID', 'CityName');
                var regionID = jQuery('#' + regionsControlID).val();
                if (!CountryHasRegions)
                    regionID = 0;
                //для записи id текущего региона в сессию
                var regionString = "area=1&regionID=" + regionID;
                if (regionsControlID.substring(0, 4) == "Fact")
                    regionString = "area=2&fRegionID=" + regionID;
                doPostAjax('<%= Url.Generate<EntrantController>(x => x.ChangeRegion(null)) %>', regionString, function(data) {
                    cityAutocomplete(cityNameControlID, regionID);
                    <% if (Url.IsInsidePortlet())
                       { %>;
                    <% } %>;
                }, "application/x-www-form-urlencoded", null, true);                 			<%-- GVUZ-539 --%>
                /*if(regionID == 1)
			{
				jQuery("label[for='" + cityNameControlID + "']").next().detach();
			}
			else
			{
				if(regionID != 1 && !jQuery('#' + cityNameControlID).attr('disabled') &&
				 jQuery("label[for='" + cityNameControlID + "']").next('span').size() == 0)
					jQuery("label[for='" + cityNameControlID + "']").after(' <span class=\"required\">(*)</span>');
			}*/
                return false;
            }

                        		<%--//список городов по региону из сессии--%>

            function cityAutocomplete(cityNameControlID, regionID) {
                //jQuery("#RegistrationAddress_CityName, #FactAddress_CityName")

                <% if (Url.IsInsidePortlet())
                   { %>;
                if (cityNameControlID.substring(0, 4) == "Fact")
                    jQuery('#' + cityNameControlID).autocomplete('<%= Url.Generate<EntrantController>(x => x.CitiesList2(null, "")) %>',
                        {
                            delay: 10,
                            minChars: 1,
                            matchSubset: 1,
                            autoFill: false,
                            cacheLength: 0,
                            matchContains: 1,
                            usePost: true,
                            lineSeparator: ',',
                            maxItemsToShow: 20
                        });
                else {
                    jQuery('#' + cityNameControlID).autocomplete('<%= Url.Generate<EntrantController>(x => x.CitiesList(null, "")) %>',
                        {
                            delay: 10,
                            minChars: 1,
                            matchSubset: 1,
                            autoFill: false,
                            cacheLength: 0,
                            matchContains: 1,
                            usePost: true,
                            lineSeparator: ',',
                            maxItemsToShow: 20
                        });
                }
                <% }
                   else
                   { %>;
                if (cityNameControlID.substring(0, 4) == "Fact")

                    autocompleteDropdown(jQuery('#' + cityNameControlID),
                        {
                            source: '<%= Url.Generate<EntrantController>(x => x.CitiesList2Local(null, "")) %>?regionID=' + regionID,
                            minLength: 2
                        });
                else {
                    autocompleteDropdown(jQuery('#' + cityNameControlID),
                        {
                            source: '<%= Url.Generate<EntrantController>(x => x.CitiesListLocal(null, "")) %>?regionID=' + regionID,
                            minLength: 2
                        });
                }
                <% } %>;
            }

            function onlyRegistrationChangeHandler() {
                var isOnlyReg = jQuery('#IsOnlyRegistration:checked').val();
                var tdWithFact = jQuery('table.data tr > td.caption:odd');

                // fact address the same with registration
                if (isOnlyReg) {
                    // empty req building asterisk
                    jQuery('span#reqFactAddressBuildingSpan').html('');

                    tdWithFact.each(function(index) {
                        if (index > 1) {
                            var elem = jQuery(this);
                            elem.children('span.required').remove();
                            clearValidationErrors(elem.next());
                        }
                    });
                }
                    // fact address different from registration
                else {
                    tdWithFact.each(function(index) {
                        var td = jQuery(this);
                        /*var label = td.children('label');					
					if(td.children('span#reqFactAddressBuildingSpan').size() == 1)
						// set req building asterisk
						td.children('span#reqFactAddressBuildingSpan').append('<span class=\"required\">(*)</span>');
					else if(index > 1 && index < tdWithFact.length - 1)
						// set asterisk for fact address
						label.after(' <span class=\"required\">(*)</span>');*/
                    });
                    onChangeRegion('FactAddress_RegionID', FactAddress_CountryHasRegions);
                }
                disableAddress();
            }

            jQuery('#IsOnlyRegistration').change(onlyRegistrationChangeHandler);

            jQuery('#RegistrationAddress_CountryID, #FactAddress_CountryID').change(function() { changeRegionsList(jQuery(this).attr('id'), jQuery(this).val()); });
            jQuery('#RegistrationAddress_RegionID').change(function() { onChangeRegion(jQuery(this).attr('id'), RegistrationAddress_CountryHasRegions); });
            jQuery('#FactAddress_RegionID').change(function() { onChangeRegion(jQuery(this).attr('id'), FactAddress_CountryHasRegions); });
            jQuery(document).ready(function() {
                jQuery('#btnCancel, #btnSubmit').button();

                showRegion('RegistrationAddress_RegionID', RegistrationAddress_CountryHasRegions);
                showRegion('FactAddress_RegionID', FactAddress_CountryHasRegions);

                disableAddress();
                onlyRegistrationChangeHandler();

                onChangeRegion('RegistrationAddress_RegionID', RegistrationAddress_CountryHasRegions);
                onChangeRegion('FactAddress_RegionID', FactAddress_CountryHasRegions);

                jQuery('#btnSubmit').click(function() {
                    doSubmit();
                    return false;
                });
                jQuery('#btnCancel').click(function() { window.location.href = jQuery(this).attr('href'); });
                jQuery('#RegistrationAddress_Phone').mask('\89999999999');
                jQuery('#Mobile').mask('\89999999999');
                jQuery('#FactAddress_Phone').mask('\89999999999');

                initAddressAutoFill();
            });
        </script>
    <% } %>
</div>