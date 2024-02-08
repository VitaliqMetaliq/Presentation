# CurrencyExchange

### Проект для презентации. 
Разделен на 3 микросервиса <br />
Crawler - выгружает данные из API по истории курса валют <br />
Converter - вычисляет курсы валют относительно других валют, кроме рубля <br />
Storage - сервис для хранения данных <br />
ExchangeTypes - shared типы для MassTransit <br />
БД - PostgreSQL <br />
Взаимодействие с помощью RabbitMQ + MassTransit <br />
Redis для кешa данных для Excel отчетов
