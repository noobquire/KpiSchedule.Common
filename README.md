# KpiSchedule.Common
![CI/CD](https://github.com/noobquire/KpiSchedule.Common/actions/workflows/nuget.yml/badge.svg)
Common code libraries used by KPI Schedule project services. This includes:
- Client to get academic schedules data from schedule.kpi.ua API with models.
- Parsers to parse data from roz.kpi.ua HTML schedule pages.
- Client to get and parse academic schedules data from roz.kpi.ua API and HTML pages with models.
- Repositories to read/write schedules from/to Amazon DynamoDB with DB entities and mappers.
- Dependency Injection configuration helpers.
- Sample roz.kpi.ua scraper that saves schedules to JSON files.