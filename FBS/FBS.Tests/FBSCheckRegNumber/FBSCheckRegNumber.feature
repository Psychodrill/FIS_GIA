Функция: Запрос по регистрационному номеру и баллам по предметам
	 
Предыстория: 
	Допустим Я захожу под пользователем "super"
	И Я открываю страницу Запрос по регистрационному номеру и баллам по предметам

Сценарий: 01. Проверка открытия страницы с Запрос по регистрационному номеру и баллам по предметам
	То нахожусь на странице Запрос по регистрационному номеру и баллам по предметам
	
Сценарий: 02. Проверка присутствия хлебных крошек на странице Запрос по регистрационному номеру и баллам по предметам
	То на странице есть следующие хлебные крошки:
         | Text                                                    |
         | Свидетельства                                           |
         | Свидетельства ЕГЭ                                       |
         | Запрос по регистрационному номеру и баллам по предметам |

Сценарий: 03. Проверка полей по умолчанию
	То вижу в полях следующие данные:
		| Name                      | Value               |
		| cNumber                   | Номер свидетельства |
		| rpSubjects_ctl01_txtValue |                     |
		| rpSubjects_ctl02_txtValue |                     |
		| rpSubjects_ctl03_txtValue |                     |
		| rpSubjects_ctl04_txtValue |                     |
		| rpSubjects_ctl05_txtValue |                     |
		| rpSubjects_ctl06_txtValue |                     |
		| rpSubjects_ctl07_txtValue |                     |
		| rpSubjects_ctl08_txtValue |                     |
		| rpSubjects_ctl09_txtValue |                     |
		| rpSubjects_ctl10_txtValue |                     |
		| rpSubjects_ctl11_txtValue |                     |
		| rpSubjects_ctl12_txtValue |                     |
		| rpSubjects_ctl13_txtValue |                     |
		| rpSubjects_ctl14_txtValue |                     |

Сценарий: 04. Проверка обязательный полей
	Когда вношу в поля следующие данные:
         | Name                      | Value |
         | txtNumber                 |       |
         | rpSubjects_ctl01_txtValue |       |
         | rpSubjects_ctl02_txtValue |       |
         | rpSubjects_ctl03_txtValue |       |
         | rpSubjects_ctl04_txtValue |       |
         | rpSubjects_ctl05_txtValue |       |
         | rpSubjects_ctl06_txtValue |       |
         | rpSubjects_ctl07_txtValue |       |
         | rpSubjects_ctl08_txtValue |       |
         | rpSubjects_ctl09_txtValue |       |
         | rpSubjects_ctl10_txtValue |       |
         | rpSubjects_ctl11_txtValue |       |
         | rpSubjects_ctl12_txtValue |       |
         | rpSubjects_ctl13_txtValue |       |
         | rpSubjects_ctl14_txtValue |       |
	И нажимаю кнопку "Проверить"
	То на экране есть:
         | Value                                                 |
         | Произошли следующие ошибки:                           |
         | Поле "Номер свидетельства" обязательно для заполнения |

Сценарий: 05. Проверка обязательности заполнения полей с баллами
	Когда вношу в поля следующие данные:
         | Name                      | Value           |
         | txtNumber                 | 16-000027009-10 |
         | rpSubjects_ctl01_txtValue |                 |
         | rpSubjects_ctl02_txtValue |                 |
         | rpSubjects_ctl03_txtValue |                 |
         | rpSubjects_ctl04_txtValue |                 |
         | rpSubjects_ctl05_txtValue |                 |
         | rpSubjects_ctl06_txtValue |                 |
         | rpSubjects_ctl07_txtValue |                 |
         | rpSubjects_ctl08_txtValue |                 |
         | rpSubjects_ctl09_txtValue |                 |
         | rpSubjects_ctl10_txtValue |                 |
         | rpSubjects_ctl11_txtValue |                 |
         | rpSubjects_ctl12_txtValue |                 |
         | rpSubjects_ctl13_txtValue |                 |
         | rpSubjects_ctl14_txtValue |                 |
	И нажимаю кнопку "Проверить"
	То на экране есть:
         | Value                                   |
         | Произошли следующие ошибки:             |
         | Укажите баллы хотя бы по двум предметам |

Сценарий: 06. Проверка очистки полей с помощью кнопки "Очистить"
	Когда вношу в поля следующие данные:
         | Name                      | Value           |
         | cNumber                   | 16-000027009-10 |
         | rpSubjects_ctl01_txtValue | 44              |
         | rpSubjects_ctl02_txtValue | 44              |
         | rpSubjects_ctl03_txtValue | 44              |
         | rpSubjects_ctl04_txtValue | 55              |
         | rpSubjects_ctl05_txtValue | 66              |
         | rpSubjects_ctl06_txtValue | 77              |
         | rpSubjects_ctl07_txtValue | 88              |
         | rpSubjects_ctl08_txtValue | 99              |
         | rpSubjects_ctl09_txtValue | 99              |
         | rpSubjects_ctl10_txtValue | 10              |
         | rpSubjects_ctl11_txtValue | 11              |
         | rpSubjects_ctl12_txtValue | 10              |
         | rpSubjects_ctl13_txtValue | 11              |
         | rpSubjects_ctl14_txtValue | 10              |
	И нажимаю кнопку "Очистить"
	То вижу в полях следующие данные:
         | Name                      | Value               |
         | cNumber                   | Номер свидетельства |
         | rpSubjects_ctl01_txtValue |                     |
         | rpSubjects_ctl02_txtValue |                     |
         | rpSubjects_ctl03_txtValue |                     |
         | rpSubjects_ctl04_txtValue |                     |
         | rpSubjects_ctl05_txtValue |                     |
         | rpSubjects_ctl06_txtValue |                     |
         | rpSubjects_ctl07_txtValue |                     |
         | rpSubjects_ctl08_txtValue |                     |
         | rpSubjects_ctl09_txtValue |                     |
         | rpSubjects_ctl10_txtValue |                     |
         | rpSubjects_ctl11_txtValue |                     |
         | rpSubjects_ctl12_txtValue |                     |
         | rpSubjects_ctl13_txtValue |                     |
         | rpSubjects_ctl14_txtValue |                     |

Сценарий: 07. Проверка результата, если внесены верные данные
	Когда вношу в поля следующие данные:
         | Name                      | Value           |
         | txtNumber                 | 16-000027009-10 |
         | rpSubjects_ctl03_txtValue | 44              |
         | rpSubjects_ctl05_txtValue | 70              |
	И нажимаю кнопку "Проверить"
	То на экране есть:
         | Value                             |
         | Номер свидетельства               |
         | 16-000027009-10                   |
         | Типографский номер                |
         | 6856345                           |
         | Документ, удостоверяющий личность |
         | 9205 527439                       |
         | Регион                            |
         | Республика Татарстан (Татарстан)  |
         | Год выдачи свидетельства          |
         | 2010                              |
         | Проверки                          |
         | 1                                 |
         | Статус свидетельства              |
         | Истек срок                        |
	И на экране есть:
		  | Value     |
		  | Предмет   |
		  | Заявлено  |
		  | В базе    |
		  | Апелляция |
		  | ФИЗИКА    |
		  | !44,0     |
		  | !44,0     |
		  | Да        |
		  | БИОЛОГИЯ  |
		  | !70,0     |
		  | !70,0     |
		  | Нет       |
	И на экране есть:
         | Value                                                                                                                               |
         | Количество уникальных проверок свидетельства ВУЗами и их филиалами. Для пользователей ССУЗов данное поле носит справочный характер. |
	И на экране есть:
         | Value                                                                                                                             |
         | В 2011-2012 годах в свидетельства о результатах ЕГЭ баллы ниже установленного Рособрнадзором минимального, не выставляются.       |
         | С Приказами Рособрнадзора о применении шкалы минимальных баллов за соответствующие годы можно ознакомиться в разделе «Документы». |
         | Срок действия свидетельств ЕГЭ определяется в соответствии с действующими нормативными документами.                               |
         | В 2012 году действительны свидетельства 2011 и 2012 годов.                                                                        |
         | Свидетельства 2010 года действительны только для лиц, проходивших военную службу по призыву и уволенных с военной службы.         |
         | С нормативными документами можно ознакомиться в разделе «Документы».                                                              |

Сценарий: 08. Проверка результата "Не найдено"
	Когда вношу в поля следующие данные:
         | Name                      | Value           |
         | txtNumber                 | 16-000027009-10 |
         | rpSubjects_ctl01_txtValue | 1               |
         | rpSubjects_ctl02_txtValue | 2               |
         | rpSubjects_ctl03_txtValue | 3               |
	И нажимаю кнопку "Проверить"
	То на экране есть:
		 | Value                               |
		 | По Вашему запросу ничего не найдено |
	И на экране есть:
         | Value                                                                                                                               |
         | Количество уникальных проверок свидетельства ВУЗами и их филиалами. Для пользователей ССУЗов данное поле носит справочный характер. |
	И на экране есть:
         | Value                                                                                                                             |
         | В 2011-2012 годах в свидетельства о результатах ЕГЭ баллы ниже установленного Рособрнадзором минимального, не выставляются.       |
         | С Приказами Рособрнадзора о применении шкалы минимальных баллов за соответствующие годы можно ознакомиться в разделе «Документы». |
         | Срок действия свидетельств ЕГЭ определяется в соответствии с действующими нормативными документами.                               |
         | В 2012 году действительны свидетельства 2011 и 2012 годов.                                                                        |
         | Свидетельства 2010 года действительны только для лиц, проходивших военную службу по призыву и уволенных с военной службы.         |
         | С нормативными документами можно ознакомиться в разделе «Документы».                                                              |

Сценарий: 09. Проверка отображения раздела "Другие свидетельства"
	Когда вношу в поля следующие данные:
         | Name                      | Value           |
         | txtNumber                 | 77-000057923-11 |
         | rpSubjects_ctl06_txtValue | 38              |
         | rpSubjects_ctl11_txtValue | 56              |
	И нажимаю кнопку "Проверить"
	То на экране есть:
         | Value                |
         | Другие свидетельства |
	И на экране есть таблица "gvCertificateList" со значениями:
         | Value           |
         | Свидетельство   |
         | Год             |
         | Статус          |
         | 77-000045432-11 |
         | 2011            |
         | Аннулировано    |
         | 77-000057880-11 |
         | 2011            |
         | Аннулировано    |

Сценарий: 10.  Проверка правильности ввода данных в поле Номер свидетельства
	Когда вношу в поля следующие данные:
         | Name                      | Value               |
         | txtNumber                 | Номер свидетельства |
         | rpSubjects_ctl01_txtValue |                     |
         | rpSubjects_ctl02_txtValue |                     |
         | rpSubjects_ctl03_txtValue |                     |
         | rpSubjects_ctl04_txtValue |                     |
         | rpSubjects_ctl05_txtValue |                     |
         | rpSubjects_ctl06_txtValue |                     |
         | rpSubjects_ctl07_txtValue |                     |
         | rpSubjects_ctl08_txtValue |                     |
         | rpSubjects_ctl09_txtValue |                     |
         | rpSubjects_ctl10_txtValue |                     |
         | rpSubjects_ctl11_txtValue |                     |
         | rpSubjects_ctl12_txtValue |                     |
         | rpSubjects_ctl13_txtValue |                     |
         | rpSubjects_ctl14_txtValue |                     |
	И нажимаю кнопку "Проверить"
	То на экране есть:
         | Value                                                     |
         | Произошли следующие ошибки:                               |
         | Номер свидетельства должен быть в формате XX-XXXXXXXXX-XX |

Сценарий: 11. Проверка правильности заполнения поля Баллы
	Когда вношу в поля следующие данные:
         | Name                      | Value           |
         | txtNumber                 | 16-000027009-10 |
         | rpSubjects_ctl01_txtValue | test            |
         | rpSubjects_ctl02_txtValue |                 |
         | rpSubjects_ctl03_txtValue |                 |
         | rpSubjects_ctl04_txtValue |                 |
         | rpSubjects_ctl05_txtValue |                 |
         | rpSubjects_ctl06_txtValue |                 |
         | rpSubjects_ctl07_txtValue |                 |
         | rpSubjects_ctl08_txtValue |                 |
         | rpSubjects_ctl09_txtValue |                 |
         | rpSubjects_ctl10_txtValue |                 |
         | rpSubjects_ctl11_txtValue |                 |
         | rpSubjects_ctl12_txtValue |                 |
         | rpSubjects_ctl13_txtValue |                 |
         | rpSubjects_ctl14_txtValue |                 |
	И нажимаю кнопку "Проверить"
	То на экране есть:
         | Value                                    |
         | Произошли следующие ошибки:              |
         | Проверьте правильность заполнения баллов |