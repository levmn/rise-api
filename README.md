# 📈 Rise Api

**RiseApi** é a API back-end do projeto Rise, construída para o desafio Global Solution da FIAP, com o tema **"O Futuro do Trabalho"**. Projetada para prover endpoints RESTful e gerenciar os recursos principais do sistema (como usuários, currículo com formações acadêmicas, experiências profissionais etc).

### 👥 Integrantes
- [RM558948] [Allan Brito Moreira](https://github.com/Allanbm100)
- [RM558868] [Caio Liang](https://github.com/caioliang)
- [RM98276] [Levi Magni](https://github.com/levmn)

## ⚙️ Requisitos e Configuração

### Pré-requisitos

- .NET 9
- Acesso a banco Oracle (para migrations/execução)

### Passos para rodar localmente

1. Clone o repositório:

    ```bash
    git clone https://github.com/levmn/rise-api.git
    cd rise-api
    ```

2. Configure variáveis de ambiente:

    ```bash
    # faça uma copia do arquivo '.env.sample', e renomeie para '.env'
    # atualize as variáveis com as suas credenciais
    DB_USER=seu_usuario
    DB_PASSWORD=sua_senha
    ```

3. Instale dependências e construa o projeto:

    ```bash
    dotnet restore
    dotnet build  
    ```

4. Execute a API:

    ```bash
    dotnet run --project src/RiseApi
    ```

5. Acesse a documentação Swagger:

    ```bash
    http://localhost:5000/swagger
    ```

## 🗃️ Entity Framework Core & Migrations

A **RiseApi** utiliza o **Entity Framework Core** como ORM para realizar o mapeamento objeto-relacional e gerenciar o schema do banco Oracle por meio de *migrations*.

### 📦 Estrutura do EF Core

- O contexto principal está localizado em:

  ```
  src/RiseApi/Data/AppDbContext.cs
  ```

- As *migrations* geradas são armazenadas em:

  ```
  src/RiseApi/Migrations
  ```

### 🔧 Criando uma nova Migration

Antes de gerar migrations, certifique-se de que as variáveis de ambiente do banco estão configuradas e que o `Oracle.EntityFrameworkCore` está instalado.

Para criar uma nova migration:

```bash
dotnet ef migrations add NomeDaMigration \
  --project src/RiseApi \
  --startup-project src/RiseApi \
  --output-dir Migrations
```

### ⬆️ Aplicando as Migrations ao Banco de Dados

Para atualizar o schema do banco de dados com as migrations pendentes:

```bash
dotnet ef database update \
  --project src/RiseApi \
  --startup-project src/RiseApi
```

## 🚦 Versionamento de Rotas (API Versioning)

Utilizamos **versionamento via rotas**, seguindo o padrão REST:

```
/api/v1/...
```

Esse padrão permite evoluir a API sem quebrar compatibilidade com clientes antigos.

### 🔧 Configuração

O versionamento está configurado no pipeline da aplicação em:

```
src/RiseApi/Program.cs
```

Cada controller define explicitamente a versão:

```csharp
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsuarioController : ControllerBase
{
}
```

### ➕ Criando uma nova versão da API

Para adicionar uma nova versão (ex: `v2`):

1. Criar controllers com a nova anotação:

   ```csharp
   [ApiVersion("2.0")]
   [Route("api/v{version:apiVersion}/[controller]")]
   public class UsuarioControllerV2 : ControllerBase { }
   ```

2. Registrar a versão no `ApiVersioning` dentro do `Program.cs`.

3. Manter todos os endpoints `v1` funcionando até a migração completa dos clientes.

### ✔️ Benefícios

- Permite mudanças sem breaking changes  
- Oferece múltiplas versões simultaneamente  
- Facilita o deprecamento controlado de endpoints antigos 

## 📜 Documentação da API

A API utiliza **Swagger** para documentar seus endpoints.  
Os modelos de request e response são definidos via DTOs, com exemplos e descrições.  
É possível testar diretamente pelo Swagger UI quando a aplicação está rodando em ambiente de desenvolvimento (`Development`).

## 🔐 Autenticação & Autorização

- A API usa **JWT** para autenticar os usuários.  
- Fluxo típico:
  1. `POST /auth/login` com credenciais -> retorna token JWT.  
  2. Usar `Authorization: Bearer <token>` nos headers das requisições protegidas.  

## 🩺 Health Check / Saúde da API

A API expõe um endpoint para monitoramento:

    GET /api/v1/health

Esse endpoint pode ser usado para liveness e readiness probes (monitoramento de disponibilidade).

## ✅ Testes
**Testes de integração**: usam um servidor web e banco de dados de teste ou in-memory para testar os endpoints HTTP.

Para executar todos os testes:

```bash
# navegue até o diretorio de testes
cd tests
# e rode o comando
dotnet test
```
