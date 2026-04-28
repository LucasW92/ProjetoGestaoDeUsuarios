# Projeto final: API de gestão de usuários

Você desenvolverá uma API web em .NET para gerenciamento de usuários, integrando os conceitos aprendidos ao longo do módulo.

A solução deve demonstrar uso de roteamento customizado, middleware, filtros, CORS, serialização/deserialização e proteção de senha. A entrega deverá incluir código-fonte, instruções de execução, coleção de testes de endpoint e breve documentação técnica.

&nbsp;

&nbsp;

## O desafio

Imagine que uma empresa precisa de um sistema interno para gerenciar os usuários da sua plataforma. Você é o desenvolvedor responsável por construir a API que vai suportar esse sistema.

A solução deve permitir cadastrar, consultar, atualizar e desativar usuários, com proteção de credenciais e controle básico de acesso. Ela deve ser organizada, segura e preparada para rodar em ambiente real.

&nbsp;

&nbsp;

## Funcionalidades obrigatórias

#### Cadastro e consulta de usuários
- Cadastrar usuário.
- Listar usuários.
- Buscar usuário por identificador.
- Atualizar dados básicos do usuário.
- Desativar usuário (**Não** apagar o registro do banco de dados)

#### Proteção da senha
- A senha não pode ser armazenada em texto puro.
- Deve ser utilizada uma estratégia segura de hash ou recurso equivalente do ecossistema .NET.

#### Uso de middleware
- Implementar pelo menos um middleware customizado, como log de requisições, medição de tempo de resposta ou tratamento global de exceções.

#### Uso de filtros
- Implementar pelo menos um filtro, como validação, auditoria ou tratamento padronizado de respostas.

#### Customização de roteamento
- Demonstrar rotas customizadas, como versionamento, rotas nomeadas ou estrutura aninhada de recursos.

#### Serialização e deserialização
- Demonstrar domínio da conversão de dados de entrada e saída, com contrato JSON coerente.

#### Configuração de CORS
- Habilitar CORS de forma explícita e justificar a política adotada.

#### Programação assíncrona
- Utilizar async/await e Task de forma coerente em pelo menos parte da API.

#### Uso de configuração externa
- Ler parâmetros da aplicação a partir de arquivo de configuração, como configuração de domínios aceitos e endpoints para política de CORS, entre outros.

&nbsp;

&nbsp;

## Especificações técnicas

### Usuários

A classe `Usuario` deve conter os seguintes atributos:

&nbsp;

#### Atributos obrigatórios

`Id` — Guid, gerado automaticamente no cadastro

`Nome` — string

`Email` — string, serve como identificador de login e deve ser único

`SenhaHash` — string, resultado do hash aplicado sobre a senha informada

`Ativo` — bool, controla a desativação do usuário.

`CriadoEm` — DateTime, preenchido automaticamente no cadastro

&nbsp;

#### Atributos recomendados (aumentam a qualidade sem complicar demais)

`AtualizadoEm` — DateTime?, atualizado a cada alteração no usuário; ajuda na auditoria

`Cargo` — string?, relevante no contexto corporativo e fácil de implementar

&nbsp;

### Resposta da API

A resposta da API está esperando um objeto no seguinte formato

```json
{
    "dados_resposta": "json original da requisição",
    "timestamp_resposta": "datetime no formato dd/mm/yyyy hh:mm:ss",
    "tempo_da_resposta": "xxx ms"
}
```

&nbsp;

### CORS

O domínio liberado (frontend que vai consumir sua API) para fazer requisições na API deve ser

> brunotrbr.github.io 

&nbsp;

### Serialização e desserialização

- Os campos nulos devem ser ignorados no processo de serialização da resposta
- As datas devem ser enviadas no formato `dd/mm/yyyy hh:mm:ss`
- Enviar os atributos do usuário no formato `snake_case`, em letras minúsculas

&nbsp;

### Estrutura de dados / banco de dados

Para o armazenamento dos dados dos usuários no programa, pode utilizar um dicionário. A chave do dicionário deve ser um campo que permita identificar e localizar facilmente o usuário.

Se virmos o conteúdo de BD/ORM, utilize o modelo visto em aula (`banco de dados + dapper` ou `banco de dados + EF`).

&nbsp;

&nbsp;

## Critérios de avaliação
- Funcionamento da API: A aplicação deve executar corretamente sem necessidade de fazer nenhuma alteração, e atender aos fluxos principais propostos.
- Organização do código: A solução deve apresentar separação clara entre controller, regra de negócio e camadas de apoio.
- Domínio dos temas do módulo: O projeto deve demonstrar roteamento customizado, middleware, filtros, CORS, serialização/deserialização e proteção de senha.
- Qualidade técnica mínima: A aplicação deve apresentar validação de entrada, tratamento de erros, respostas coerentes e ausência de credenciais expostas.

&nbsp;

### Referência para autoavaliação

| O que se espera ver | O que compromete a entrega |
|---|---|
| Entrega uma API funcional | Armazena senha em texto puro |
| Implementa CRUD básico de usuários | Apresenta fluxo quebrado |
| Protege a senha corretamente | Não utiliza middleware nem filtros |
| Usa pelo menos um middleware e um filtro | Concentra regra de negócio diretamente no controller |
| Configura CORS e utiliza async/await | Não demonstra uso correto de async/await |
| Organiza o código em camadas mínimas | Não consegue explicar as decisões técnicas adotadas |

&nbsp;

&nbsp;

## Entregáveis esperados

| Obrigatórios | Desejáveis |
|---|---|
| Código-fonte do projeto | Documentação Swagger |
| Instruções de execução | Tratamento padronizado de erros |
| Coleção de testes de endpoint (Postman ou similar) | Logs básicos de execução |
| Breve documento explicando a arquitetura da solução | Diagrama simples de camadas |
| Exemplos de requisição e resposta | Tratamento de erros |
| Evidência de proteção da senha | |

&nbsp;

&nbsp;

## Informações gerais
- A entrega deve ser feita no LMS, enviando o arquivo .zip contendo a API ou o link do github.