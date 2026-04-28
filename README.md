# GestaoDeUsuarios API

**v1.0.0**

API de gestao de usuarios organizada em Clean Architecture, com autenticacao JWT, CORS explicito, validacao por filtro, middleware customizado, contrato HTTP padronizado e testes automatizados.

## Arquitetura

```
┌──────────────────────────────────────────────────┐
│                    Camada Api                    │
│         Endpoints, Program.cs, Scalar            │
└──────────────────┬───────────────────────────────┘
                                     │ depende de
┌──────────────────▼───────────────────────────────┐
│              Camada Infrastructure               │
│         EF Core, Identity, JWT e cache           │
└──────────────────┬───────────────────────────────┘
                                     │ depende de
┌──────────────────▼───────────────────────────────┐
│               Camada Application                 │
│        Handlers, validadores e DTOs              │
└──────────────────┬───────────────────────────────┘
                                     │ depende de
┌──────────────────▼───────────────────────────────┐
│                 Camada Domain                    │
│        Entidades e primitivas do dominio         │
└──────────────────────────────────────────────────┘
```

Regra de dependencia: cada camada depende apenas da camada imediatamente inferior. A camada Domain nao depende de nenhuma outra camada da solucao. Os testes de arquitetura garantem isso automaticamente.

## Principais requisitos atendidos

- Cadastro, listagem, consulta, atualizacao e desativacao de usuarios.
- Senhas protegidas com ASP.NET Core Identity.
- Middleware customizado para medicao de tempo de resposta.
- Filtro de validacao com FluentValidation.
- CORS explicito para `https://brunotrbr.github.io` e `localhost`.
- JSON em `snake_case`, datas formatadas e nulos omitidos.
- Resposta HTTP padronizada com `dados_resposta`, `timestamp_resposta` e `tempo_da_resposta`.
- JWT para controle basico de acesso aos endpoints protegidos.

## Estrutura do projeto

```
├── src/
│   ├── Api/                                # Endpoints, filtros, middleware e contrato JSON
│   ├── AppHost/                            # Orquestracao Aspire
│   ├── Application/                        # Requests, handlers, DTOs e validadores
│   ├── Domain/                             # Entidades e primitivas do dominio
│   ├── Infrastructure/                     # EF Core, Identity, JWT e persistencia
│   ├── ServiceDefaults/                    # Telemetria, health checks e defaults
│   └── GestaoDeUsuarios.slnx               # Solucao da aplicacao
├── tests/
│   ├── Api.IntegrationTests/               # Envelope HTTP, serializacao, CORS e endpoints
│   ├── Application.UnitTests/              # Testes unitarios de handlers e validadores
│   └── Architecture.Tests/                 # Regras de dependencia entre camadas
├── Directory.Build.props                         # Configuracoes gerais do .NET e C#
├── Directory.Packages.props                      # Gerenciamento central de pacotes
├── .editorconfig                                 # Regras de estilo
├── docker-compose.yml                            # PostgreSQL para execucao sem Aspire
└── README.md
```

## Como executar

### Executar com Aspire

```bash
dotnet run --project src/AppHost
```

Esse comando sobe:

- PostgreSQL
- API com migrations automaticas
- Aspire Dashboard para telemetria

Abra a URL do Aspire Dashboard mostrada no console para visualizar traces, logs e metricas.

### Executar sem Aspire

```bash
docker compose up -d
dotnet run --project src/Api
```

Nesse modo, a API usa a connection string de desenvolvimento configurada em `src/Api/appsettings.Development.json`.
Ao iniciar em `Development`, as migrations sao aplicadas automaticamente.

### Explorar a API

Sem Aspire, abra `https://localhost:7200/scalar/v1` para acessar a documentacao interativa via Scalar.

Com Aspire, abra o dashboard exibido no console e use o link externo publicado para a API. Como as portas podem variar, a URL do Scalar passa a depender do endpoint exposto pelo AppHost.

Voce tambem pode usar a colecao de requests em `src/Api/GestaoDeUsuarios.Api.http`.

Credenciais iniciais do administrador:

- Email: `admin@gestaodousuario.dev`
- Senha: `Admin@123`

## Contrato HTTP da resposta

Toda resposta da API segue o envelope abaixo:

```json
{
    "dados_resposta": {
        "id": "...",
        "nome": "Maria Silva",
        "email": "maria.silva@empresa.com",
        "ativo": true,
        "criado_em": "26/04/2026 20:10:00"
    },
    "timestamp_resposta": "26/04/2026 20:10:00",
    "tempo_da_resposta": "12 ms"
}
```

As respostas de erro seguem o mesmo envelope, colocando o payload de erro dentro de `dados_resposta`.

## CORS

A API libera explicitamente as origens abaixo:

- `https://brunotrbr.github.io`
- `http://localhost`
- `https://localhost`
- `http://localhost:3000`
- `https://localhost:3000`
- `http://localhost:5173`
- `https://localhost:5173`

A politica fica configurada em `src/Api/appsettings.json`.

## Serializacao JSON

- As propriedades sao serializadas em `snake_case`.
- Campos nulos sao omitidos da resposta.
- Datas usam o formato `dd/MM/yyyy HH:mm:ss`.

## Autenticacao e autorizacao

- `POST /api/identity/register`, `POST /api/identity/login` e `POST /api/identity/refresh` sao publicos.
- O grupo `/api/users` exige autenticacao via JWT e perfil `Admin`.

## Executar os testes

```bash
cd src
dotnet test GestaoDeUsuarios.slnx
```

## Endpoints principais

- `POST /api/identity/register`
- `POST /api/identity/login`
- `POST /api/identity/refresh`
- `POST /api/users/`
- `GET /api/users/{id}`
- `GET /api/users/?page=1&pageSize=10`
- `PUT /api/users/{id}`
- `PATCH /api/users/{id}/deactivate`

## Documentacao complementar

- `docs/arquitetura.md` — resumo tecnico da arquitetura e das decisoes adotadas.
- `src/Api/GestaoDeUsuarios.Api.http` — colecao de chamadas para validacao manual.
- `tests/Api.IntegrationTests` — testes de integracao para envelope HTTP, serializacao e CORS.