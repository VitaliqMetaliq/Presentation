# CurrencyExchange

Проект для презентации. 
Разделен на 3 микросервиса
Crawler - выгружает данные из API по истории курса валют
Converter - вычисляет курсы валют относительно других валют, кроме рубля
Storage - сервис для хранения данных
ExchangeTypes - shared типы для MassTransit
БД - PostgreSQL
Взаимодействие с помощью RabbitMQ + MassTransit
Redis для кешa данных для Excel отчетов
