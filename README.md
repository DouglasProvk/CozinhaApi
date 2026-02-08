# ??? CozinhaAPI - Backend

API RESTful desenvolvida em .NET 8 para gerenciar pratos e reservas de uma cozinha. A aplicação oferece endpoints para criar, ler, atualizar e deletar pratos e reservas, com suporte a CORS para integração com front-end em Angular.

## ?? Características

- ? **CRUD Completo** para Pratos e Reservas
- ? **Entity Framework Core** com SQL Server
- ? **Swagger/OpenAPI** para documentação interativa
- ? **CORS** configurado para Angular (localhost:4200)
- ? **Async/Await** para operações de banco de dados
- ? **Fluent API** para configuração do Entity Framework
- ? **.NET 8** com C# 12

## ??? Arquitetura

```
CozinhaApi/
??? Models/
?   ??? Prato.cs              # Modelo de Pratos
?   ??? Reserva.cs            # Modelo de Reservas
??? Controllers/
?   ??? PratosController.cs    # Endpoints de Pratos
?   ??? ReservasController.cs  # Endpoints de Reservas
??? Data/
?   ??? Context.cs            # DbContext principal
?   ??? DataConfiguration/
?       ??? PratosConfiguration.cs
?       ??? ReservasConfiguration.cs
??? Program.cs                 # Configuração da aplicação
??? appsettings.json          # Configurações
??? CozinhaApi.csproj        # Arquivo do projeto
```

## ?? Dependências

- **Microsoft.EntityFrameworkCore** (9.0.12)
- **Microsoft.EntityFrameworkCore.SqlServer** (9.0.12)
- **Swashbuckle.AspNetCore** (6.6.2)
- **System.Text.Json** (10.0.2)

## ?? Como Começar

### Pré-requisitos

- .NET 8 SDK
- SQL Server ou SQL Server LocalDB
- Visual Studio 2022 (recomendado) ou VS Code

### Instalação

1. **Clone o repositório:**
```bash
git clone https://github.com/DouglasProvk/CozinhaApi.git
cd CozinhaApi
```

2. **Restaure as dependências:**
```bash
dotnet restore
```

3. **Configure a string de conexão** em `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CozinhaDB;Trusted_Connection=true"
  }
}
```

4. **Execute as migrations do banco de dados:**
```bash
dotnet ef database update
```

5. **Inicie a aplicação:**
```bash
dotnet run
```

A API estará disponível em: `https://localhost:7000` (ou a porta configurada)

## ?? Endpoints da API

### ?? Pratos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/api/pratos` | Lista todos os pratos |
| `GET` | `/api/pratos/{id}` | Obtém um prato pelo ID |
| `POST` | `/api/pratos` | Cria um novo prato |
| `PUT` | `/api/pratos/{id}` | Atualiza um prato |
| `DELETE` | `/api/pratos/{id}` | Deleta um prato |

#### Exemplo de Requisição POST - Prato:
```json
{
  "nome": "Frango à Parmesana",
  "descricao": "Frango empanado com molho de tomate e queijo",
  "categoria": "frango",
  "disponivel": true
}
```

### ?? Reservas

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/api/reservas` | Lista todas as reservas (ordenadas por data) |
| `GET` | `/api/reservas/{id}` | Obtém uma reserva pelo ID |
| `POST` | `/api/reservas` | Cria uma nova reserva |
| `PUT` | `/api/reservas/{id}` | Atualiza uma reserva |
| `DELETE` | `/api/reservas/{id}` | Deleta uma reserva |

#### Exemplo de Requisição POST - Reserva:
```json
{
  "nomePessoa": "João Silva",
  "pratoId": 1,
  "pratoNome": "Frango à Parmesana",
  "dataRefeicao": "2024-02-15T12:00:00Z",
  "periodo": "almoco",
  "observacoes": "Sem cebola",
  "status": "confirmada"
}
```

## ?? Modelos

### Prato
```csharp
public class Prato
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string? Descricao { get; set; }
    public string Categoria { get; set; } // carne, frango, peixe, vegetariano, vegano
    public bool Disponivel { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
```

### Reserva
```csharp
public class Reserva
{
    public int Id { get; set; }
    public string NomePessoa { get; set; }
    public int PratoId { get; set; }
    public string? PratoNome { get; set; }
    public DateTime DataRefeicao { get; set; }
    public string Periodo { get; set; } // almoco, jantar
    public string? Observacoes { get; set; }
    public string Status { get; set; } = "confirmada"; // confirmada, cancelada, servida
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public virtual Prato? Prato { get; set; }
}
```

## ?? Configuração

### CORS
A aplicação está configurada para aceitar requisições do front-end Angular em `http://localhost:4200`.

Se precisar alterar a origem, modifique em `Program.cs`:
```csharp
options.AddPolicy("AllowAngular", policy =>
{
    policy.WithOrigins("sua-origem-aqui")
          .AllowAnyHeader()
          .AllowAnyMethod();
});
```

### Swagger
Acesse a documentação interativa em: `https://localhost:7000/swagger`

## ?? Estrutura do Banco de Dados

### Tabela: Pratos
- `Id` (PK)
- `Nome` (VARCHAR(150), NOT NULL)
- `Descricao` (VARCHAR(500), NULL)
- `Categoria` (VARCHAR(50), NOT NULL)
- `Disponivel` (BIT, DEFAULT 1)
- `CreatedDate` (DATETIME, DEFAULT GETUTCDATE())

### Tabela: Reservas
- `Id` (PK)
- `NomePessoa` (VARCHAR(150), NOT NULL)
- `PratoId` (INT, FK ? Pratos, NOT NULL)
- `PratoNome` (VARCHAR(150), NULL)
- `DataRefeicao` (DATETIME, NOT NULL)
- `Periodo` (VARCHAR(20), NOT NULL)
- `Observacoes` (VARCHAR(500), NULL)
- `Status` (VARCHAR(20), DEFAULT 'confirmada')
- `CreatedDate` (DATETIME, DEFAULT GETUTCDATE())

## ?? Desenvolvimento

### Executar testes:
```bash
dotnet test
```

### Build de release:
```bash
dotnet build -c Release
```

## ?? Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para:
1. Fork o projeto
2. Criar uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abrir um Pull Request

## ?? Licença

Este projeto é de código aberto e disponível sob a [MIT License](LICENSE).

## ????? Autor

**Douglas Provk**
- GitHub: [@DouglasProvk](https://github.com/DouglasProvk)

## ?? Suporte

Para reportar problemas ou sugerir melhorias, abra uma [Issue](https://github.com/DouglasProvk/CozinhaApi/issues).

---

**Última atualização:** Fevereiro de 2026