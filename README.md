# Запустить DataProcessorService и FileProcessorService
## Для запуска DataProcessorService:
- Создать бд с помощью миграций: Add-Migration название_миграции; Update-Database
- Поменять название можно appsettings.json "ConnectionStrings": {
    "DefaultConnection": "Data Source=yourName.db"
  },
## Для подключения к RabbitMQ:
- docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.13-management
- указать через переменные окружения  "RABBITMQ_HOST" и "RABBITMQ_QUEUE" , если переменные не указаны будут использоваться дефолтные значения
## Для работы FileProcessorService:
- для обработки Xml файлов нужно добавить файл в папку xml внутри проекта.
### Краткое описание работы:
- Запускается DataProcessorService, он ждет отправки файлов
- Далее запускается FileProcessorService он отправляет файлы находящиеся в папке, если надо отправить еще просто закинуть файлы в папку xml
