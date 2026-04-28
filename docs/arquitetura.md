# Arquitetura da solucao

## Visao geral

A API foi estruturada em quatro camadas seguindo Clean Architecture:

- `Domain`: entidade `ApplicationUser` e primitivas de resultado/erro.
- `Application`: handlers, requests, responses e validadores.
- `Infrastructure`: Identity, JWT, EF Core e persistencia.
- `Api`: endpoints Minimal API, filtros, middleware e configuracao HTTP.

## Decisoes tecnicas principais

- Senhas sao protegidas com ASP.NET Core Identity. O hash e gerado internamente pelo `UserManager`, sem armazenamento em texto puro.
- Os endpoints de autenticacao ficam em `/api/identity` e os endpoints de gestao de usuarios em `/api/users`.
- O grupo `/api/users` exige autenticacao JWT para demonstrar controle basico de acesso.
- A validacao de entrada usa `ValidationFilter<T>` com FluentValidation.
- O tratamento global de excecao e centralizado em `GlobalExceptionHandler`.
- O middleware `ResponseTimingMiddleware` mede o tempo da requisicao para compor o contrato de resposta.

## Contrato HTTP adotado

Todas as respostas da API sao envelopadas no formato abaixo:

- `dados_resposta`: payload funcional da operacao.
- `timestamp_resposta`: data e hora da resposta no formato `dd/MM/yyyy HH:mm:ss`.
- `tempo_da_resposta`: tempo total medido para a requisicao em milissegundos.

## JSON e serializacao

- `snake_case` em todas as propriedades serializadas.
- Campos nulos omitidos da resposta.
- Datas serializadas em `dd/MM/yyyy HH:mm:ss`.

## CORS

A politica explicita de CORS libera `https://brunotrbr.github.io` e origens de desenvolvimento em `localhost`. A configuracao fica em `src/Api/appsettings.json`.

## Persistencia

- Banco: PostgreSQL via EF Core.
- Usuario persistido por `ApplicationUser` do ASP.NET Identity.
- Campos de auditoria adicionados: `CreatedAt` e `UpdatedAt`.

## Testes

- `tests/Application.UnitTests`: cobre handlers e validadores da feature de usuarios.
- `tests/Architecture.Tests`: garante as regras de dependencia entre camadas.
- `src/Api/GestaoDeUsuarios.Api.http`: colecao simples de chamadas para validar endpoints manualmente.
- `tests/Api.IntegrationTests`: valida envelope HTTP, serializacao e CORS usando `WebApplicationFactory`.