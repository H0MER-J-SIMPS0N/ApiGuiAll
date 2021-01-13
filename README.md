# ApiGuiAll
Программу можно использовать для облегчения разработки интеграции по документации https://doc.medlinx.online/docs/integration.html и https://doc-r4.medlinx.online/docs/integration.html.

При запуске программы считываются настройки из файлов settings.json, GetRequests.json, PostRequests.json  и Headers.json, создаются папки Downloads (в нее сохраняются скаченные файлы) и Responses (в нее сохраняются файлы с большими ответами на запросы) в папке с приложением (если их там нет), считываются настройки из файлов GetRequests.json, PostRequests.json и Headers.json.
Получается токен по умолчанию (первый из списка). Если не удается получить токен, то информация об этом отображается.
На вкладках GET и POST можно делать соответствующие запросы. На вкладке Other можно делать оба типа запросов.
На вкладках Nomenclature STU3 и Nomenclature R4 можно запросить номенклатуру по определенному контракту и искать нужную номенклатуру с отображенем в читаемом виде.
На вкладке Stu3 Find Tasks By LabelId можно получить все таски уровня OrderProcessingTask, в которых есть указанный номер ШК.

settings.json - настройки для получения токенов
TokenAddress (тип string) - полный адрес для запроса токена
Zone (тип string) - название зоны для токена - может быть любым, но не должно повтряться. Используется в GetRequests.json и PostRequests.json
Data (тип Dictionary<string, string>) - post данные для запроса токена
BaseUrl (тип string) - основной url, после которого идут пути до конкретных ресурсов (например, fhir/Task/<guid>)

Headers.json - настройки заголовков post-запросов. Это список списков.
Сам список содержит имена заголовков, а в них уже идет список возможных значений заголовков.

GetRequests.json - настройки Get-запросов
Сам ресурс - это список объектов.
Каждый объект состоит из :
What (тип string) - название в пункте "Что найти" на вкладке GET
Zone (тип string) - список зон, при выборе которых отображается этот пункт (названия должны точно соответствовать названиям из Zone в settings.json).
ByWhat - список объектов. Объекты состоят из:
Name (тип string) - название пункта "Найти с помощью" (с помощью чего попытаться найти What)
WhatToEnter (тип string) - отображаемая подсказка что вводить для поиска
Endpoint (тип string) - часть пути запроса без основного url
Argument (тип string) - слово, которое будет полностью заменено на введенное в поле для ввода значение
Headers (тип Dictionary) - список заголовков, необходимых для запроса (поле - значение)

PostRequests.json - настройки Post-запросов
Сам ресурс - это список объектов.
Каждый объект состоит из :
What (тип string) - название в пункте "Что изменить" на вкладке POST
Zone (тип string) - список зон, при выборе которых отображается этот пункт (названия должны точно соответствовать названиям из Zone в settings.json).
WithWhat - список объектов. Обекты состоят из:
WhatToEnterOne (тип string) - отображаемая подсказка какое первое значение вводить для изменения ресурса
WhatToEnterTwo (тип string) - отображаемая подсказка какое второе значение вводить для изменения ресурса
Endpoint (тип string) - часть пути запроса без основного url
PostData (тип string) - post данные (одинарные кавычки в файле заменяются на двойные в программе)
ArgumentOne (тип string) - слово, которое будет полностью заменено в post данных на введенное в первое поле для ввода значение
ArgumentTwo (тип string) - слово, которое будет полностью заменено в post данных на введенное во второе поле для ввода значение
Headers (тип Dictionary) - список заголовков, необходимых для запроса (поле - значение)
