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

- **Testes de integração**: usam um servidor web e banco de dados de teste ou in-memory para testar os endpoints HTTP.
- Para executar todos os testes:
  
    ```bash
    # navegue até o diretorio de testes
    cd tests
    # e rode o comando
    dotnet test
    ```
  
