# **Documentação do Projeto**

## **Requisitos**

### **Backend**

- .NET 9.0 SDK instalado.
- Ferramenta de gerenciamento de banco de dados compatível com o SGBD de escolha (Ex.: MySQL Workbench, SQL Server Management Studio).
- Banco de dados relacional (MySQL, PostgreSQL, ou SQL Server).
- Ferramenta de linha de comando do Entity Framework Core (`dotnet ef`).

### **Frontend**

- Node.js 18.x ou superior.
- Gerenciador de pacotes npm ou yarn.

---

## **Configuração do Backend**

1. **Clone o repositório:**
2. **Instale as dependências do projeto:**
    
    ```bash
    dotnet restore
    ```
    
3. **Configure o banco de dados:**
    - Localize o arquivo `appsettings.json` na pasta `backend/` e atualize a seção `ConnectionStrings` com a string de conexão do banco de dados escolhido. Exemplo para SQL Server:
        
        ```json
        
        {
          "ConnectionStrings": {
            "DefaultConnection": "Server=localhost;Database=NomeDoBanco;User Id=seu_usuario;Password=sua_senha;"
          }
        }
        
        ```
        
4. **Aplicar as Migrations:**
Certifique-se de que o banco de dados especificado na string de conexão existe.
    - Gere ou aplique as migrations:
        
        ```bash
        
        dotnet ef database update
        
        ```
        
5. **Executar o servidor:**
    - Rode a aplicação:
        
        ```bash
        
        dotnet run
        
        ```
        
    - A API estará disponível em `http://localhost:5000`.

---

## **Configuração do Frontend**

1. **Clone o repositório:**
    
    ```bash
    git clone <url-do-repositorio>
    cd frontend
    ```
    
2. **Instale as dependências:**
    
    ```bash
    npm install
    # ou
    yarn install
    ```
    
3. **Configuração da URL do Backend:**
    - Localize o arquivo de configuração (ex.: `.env` ou `src/config.ts`) e configure a URL base da API:
        
        ```jsx
        export const API_BASE_URL = 'http://localhost:5000';
        ```
        
4. **Rodar o servidor do frontend:**
    - Execute o servidor:
        
        ```bash
        npm run serve
        # ou
        yarn serve
        ```
        
    - A aplicação estará disponível em `http://localhost:8080`.

---

## **Estrutura do Projeto**

### **Backend**

- `Controllers/`: Define os endpoints RESTful.
- `Entities/`: Define os modelos de dados.
- `Data/`: Configuração do banco de dados e contextos.
- `Migrations/`: Arquivos gerados pelo Entity Framework para controle de versão do banco de dados.

### **Frontend**

- `src/`: Contém o código-fonte da aplicação Vue.
    - `components/`: Componentes Vue.
    - `views/`: Páginas do aplicativo.
    - `store/`: Gerenciamento de estado Vuex.
    - `router/`: Configuração de rotas.
    - `types/`: Interfaces genéricas.
    - `assets/`: Assets do projeto.

---

## **Comandos Úteis**

### **Backend**

- Criar uma nova migration:
    
    ```bash
    dotnet ef migrations add NomeDaMigration
    ```
    
- Reverter a última migration:
    
    ```bash
    dotnet ef database update NomeDaMigrationAnterior
    
    ```
    

### **Frontend**

- Construir o projeto para produção:
    
    ```bash
    npm run build
    # ou
    yarn build
    ```
    

---

## **Notas**

- O projeto funciona apenas em ambiente local (localhost) e não foi configurado para produção.
- Certifique-se de que a porta configurada para o backend (`5000`) e frontend (`8080`) não esteja em uso por outros serviços.
- Caso ocorra algum problema, verifique os logs do backend e os erros no console do navegador.
