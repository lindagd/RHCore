# RHCore

Aplicação de gestão de pessoal para o departamento de RH. Utiliza ASP.NET e SQL Server.

![image](https://github.com/user-attachments/assets/09ced6d7-3578-479e-94eb-edf4188af472)

# Setup

## 1. Clone o repositório

    git clone https://github.com/lindagd/RHCore.git
   
## 2. Instale as dependências
   
    dotnet restore

## 3. Conecte-se ao BD e adicione a string de conexão
No arquivo appsettings.json

    "ConnectionStrings": {
        "DefaultConnection": "Server=<SEU_SERVIDOR>;Database=<SEU_BANCO>;Trusted_Connection=True;"
     }     

## 4. Aplique as migrações no banco

    dotnet ef database update

## 5. (Opcional) Popule a base de dados

Para a <strong>primeira execução</strong> do projeto, rode

    dotnet run seeddata

Para as demais

    dotnet run
